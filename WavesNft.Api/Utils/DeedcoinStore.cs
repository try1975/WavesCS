using System.Collections.Concurrent;
using Waves.standard;
using Waves.standard.Transactions;

namespace WavesNft.Api.Utils;
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
public class DeedcoinStore : IDeedcoinStore
{
    #region init
    private const int limit = 20;
    private bool _accountDeedcoinsNeedRefresh = false;
    private readonly DateTime maxTransactionAge = new(2022, 4, 15);

    private readonly ConcurrentDictionary<string, Transaction> transactionsDictionary = new();
    private readonly ConcurrentDictionary<string, DeedcoinAsset> _issuedDeedcoins = new();
    private readonly ConcurrentDictionary<string, DeedcoinAsset> _accountDeedcoins = new();
    private readonly ILogger<DeedcoinStore> logger;
    private readonly Node node;
    private readonly PrivateKeyAccount account;


    public DeedcoinStore(ILogger<DeedcoinStore> logger, Node node, PrivateKeyAccount account)
    {
        this.logger = logger;
        this.node = node;
        this.account = account;
        logger.LogDebug($"account.Address={account.Address}");
        Init();
    }

    private void Init()
    {
        FillTransactions();
        FillIssuedDeedcoins();
        FillAccountDeedcoins();
    }
    #endregion init

    #region IDeedcoinStore
    public bool IssuedDeedcoinsContainsKey(string token)
    {
        RefreshIssuedDeedcoins();
        return _issuedDeedcoins.ContainsKey(token);
    }

    public DeedcoinAsset IssuedDeedcoinsValue(string token)
    {
        return _issuedDeedcoins[token];
    }

    public bool IssuedDeedcoinsTryAdd(string token, DeedcoinAsset deedcoinAsset)
    {
        return _issuedDeedcoins.TryAdd(token, deedcoinAsset);
    }

    public bool AccountDeedcoinsContainsKey(string token)
    {
        FillAccountDeedcoins();
        return _accountDeedcoins.ContainsKey(token);
    }

    public DeedcoinAsset AccountDeedcoinsValue(string token)
    {
        return _accountDeedcoins[token];
    }

    public bool AccountDeedcoinsTryAdd(string token, DeedcoinAsset deedcoinAsset)
    {
        return _accountDeedcoins.TryAdd(token, deedcoinAsset);
    }

    public bool AccountDeedcoinsTryRemove(string token, out DeedcoinAsset deedcoinAsset)
    {
        return _accountDeedcoins.TryRemove(token, out deedcoinAsset);
    }
    #endregion IDeedcoinStore

    #region Refresh
    private void RefreshIssuedDeedcoins()
    {
        var transactions = node.GetTransactions(account.Address, 1);
        if (transactions == null) return;
        if (!transactions.Any()) return;
        var id = GetTransactionId(transactions.First());
        if (transactionsDictionary.ContainsKey(id)) return;
        Init();
    }

    #endregion Refresh

    #region Fill
    private void FillTransactions()
    {
        var transactions = node.GetTransactions(account.Address, limit);
        if (!transactions.Any()) return;
        var added = TransactionsDictionaryAddRange(transactions);
        var lastTransaction = transactions.Last();
        while (added && lastTransaction.Timestamp >= maxTransactionAge)
        {
            var afterId = GetTransactionId(lastTransaction);
            transactions = node.GetTransactionsByAddressAfterId(account.Address, afterId, limit);
            added = TransactionsDictionaryAddRange(transactions);
            lastTransaction = transactions.Last();
        }
    }

    private bool TransactionsDictionaryAddRange(IEnumerable<Transaction> transactions)
    {
        var added = false;
        foreach (var transaction in transactions)
        {
            var id = GetTransactionId(transaction);
            if (transactionsDictionary.ContainsKey(id)) continue;
            transactionsDictionary.TryAdd(id, transaction);
            if (transaction is IssueTransaction) _accountDeedcoinsNeedRefresh = true;
            if (transaction is BurnTransaction) _accountDeedcoinsNeedRefresh = true;
            if (transaction is TransferTransaction) _accountDeedcoinsNeedRefresh = true;
            added = true;
        }
        return added;
    }

    private void FillIssuedDeedcoins()
    {
        var issueTransactions = transactionsDictionary.Where(kvp => kvp.Value.GetType() == typeof(IssueTransaction)).Select(kvp => kvp.Value as IssueTransaction).ToList();
        foreach (var issueTransaction in issueTransactions)
        {
            var deedcoinDescription = DeedcoinDescriptionBuilder.Build(issueTransaction.Description);
            if (deedcoinDescription != null)
            {
                if (string.IsNullOrEmpty(deedcoinDescription.token)) continue;
                if (_issuedDeedcoins.ContainsKey(deedcoinDescription.token)) continue;
                var deedcoinAsset = new DeedcoinAsset
                {
                    Id = issueTransaction.Asset.Id,
                    Name = issueTransaction.Asset.Name,
                    DeedcoinDescription = deedcoinDescription,
                    Timestamp = issueTransaction.Timestamp
                };
                _issuedDeedcoins.TryAdd(deedcoinDescription.token, deedcoinAsset);
            }
        }
    }

    private bool AccountDeedcoinsAddRange(IEnumerable<Dictionary<string, object>> objects)
    {
        var added = false;
        foreach (var obj in objects)
        {
            var description = obj.FirstOrDefault(_ => _.Key.Equals("description")).Value.ToString();
            var deedcoinDescription = DeedcoinDescriptionBuilder.Build(description);
            if (deedcoinDescription == null) continue;
            if (_accountDeedcoins.ContainsKey(deedcoinDescription.token)) continue;
            var assetId = obj.FirstOrDefault(_ => _.Key.Equals("assetId")).Value.ToString();
            var name = obj.FirstOrDefault(_ => _.Key.Equals("name")).Value.ToString();
            var deedcoinAsset = new DeedcoinAsset
            {
                Id = assetId,
                Name = name,
                DeedcoinDescription = deedcoinDescription
            };
            var issueTimestamp = obj.FirstOrDefault(_ => _.Key.Equals("issueTimestamp")).Value.ToString();
            if (long.TryParse(issueTimestamp, out var issuesAt))
            {
                deedcoinAsset.Timestamp = issuesAt.ToDate();
            }
            if (deedcoinAsset.Timestamp < maxTransactionAge) continue;
            _accountDeedcoins.TryAdd(deedcoinDescription.token, deedcoinAsset);
            added = true;
        }
        return added;
    }

    private void FillAccountDeedcoins()
    {
        if (!_accountDeedcoinsNeedRefresh) return;
        _accountDeedcoins.Clear();
        var url = $"assets/nft/{account.Address}/limit/{limit}";
        var objects = node.GetObjects(url).ToArray();
        if (objects == null) return;
        if (!objects.Any()) return;
        var added = AccountDeedcoinsAddRange(objects);
        while (added)
        {
            var afterId = objects.Last().FirstOrDefault(_ => _.Key.Equals("assetId")).Value.ToString();
            objects = node.GetObjects($"{url}?after={afterId}").ToArray();
            added = AccountDeedcoinsAddRange(objects);
        }
        _accountDeedcoinsNeedRefresh = false;
    }
    #endregion Fill

    private static string GetTransactionId(Transaction transaction)
    {
        if (transaction == null) return string.Empty;
        if (transaction is IssueTransaction) return (transaction as IssueTransaction).Asset.Id;
        return transaction.GenerateId();
    }
}
