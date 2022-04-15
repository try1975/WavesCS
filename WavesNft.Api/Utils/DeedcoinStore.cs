using System.Collections.Concurrent;
using Waves.standard;
using Waves.standard.Transactions;

namespace WavesNft.Api.Utils
{
    public class DeedcoinStore : IDeedcoinStore
    {
        private const int limit = 25;
        private const int refreshLimit = 1;
        private readonly DateTime maxTransactionAge = new(2022, 4, 1);

        private readonly List<Transaction> _transactions = new();
        private readonly ConcurrentDictionary<string, Transaction> transactionsDictionary = new();
        private readonly ConcurrentDictionary<string, DeedcoinAsset> issuedDeedcoins = new();
        private readonly ConcurrentDictionary<string, DeedcoinAsset> _accountDeedcoins = new();

        private readonly Node node;
        private readonly PrivateKeyAccount account;

        public ConcurrentDictionary<string, DeedcoinAsset> IssuedDeedcoins { get => issuedDeedcoins; }
        public ConcurrentDictionary<string, DeedcoinAsset> AccountDeedcoins { get => _accountDeedcoins; }


        public DeedcoinStore(Node node, PrivateKeyAccount account)
        {
            this.node = node;
            this.account = account;
            Init();
        }

        private void Init()
        {
            FillTransactions();
            TransactionDictionaryAddRange(_transactions);
            FillIssuedDeedcoins();
            FillAccountDeedcoins();
        }

        public void RefreshIssuedDeedcoins()
        {
            var transactions = node.GetTransactions(account.Address, refreshLimit);
            if (transactions == null) return;
            if (!transactions.Any()) return;
            var id = GetTransactionId(transactions.FirstOrDefault());
            if (transactionsDictionary.ContainsKey(id)) return;
            var isTransactionsAdded = TransactionDictionaryAddRange(transactions);
            if (isTransactionsAdded) FillIssuedDeedcoins();
        }
        public void RefreshAccountDeedcoins()
        {
            //throw new NotImplementedException();
        }

        private void FillTransactions()
        {
            var transactions = node.GetTransactions(account.Address, limit);
            _transactions.AddRange(transactions);
            if (_transactions.Any())
            {
                var lastTransaction = _transactions.LastOrDefault();
                if (lastTransaction == null) return;
                var afterId = lastTransaction.GenerateId();
                transactions = node.GetTransactionsByAddressAfterId(account.Address, afterId, limit);
                while (transactions.Length > 0 && lastTransaction.Timestamp >= maxTransactionAge)
                {
                    _transactions.AddRange(transactions);
                    lastTransaction = transactions.LastOrDefault();
                    if (lastTransaction == null) return;
                    afterId = GetTransactionId(lastTransaction);
                    transactions = node.GetTransactionsByAddressAfterId(account.Address, afterId, limit);
                }
            }
        }

        private bool TransactionDictionaryAddRange(IEnumerable<Transaction> transactions)
        {
            var added = false;
            foreach (var transaction in transactions)
            {
                var id = GetTransactionId(transaction);
                if (transactionsDictionary.ContainsKey(id)) continue;
                transactionsDictionary.TryAdd(id, transaction);
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
                    if (IssuedDeedcoins.ContainsKey(deedcoinDescription.token)) continue;
                    var deedcoinAsset = new DeedcoinAsset
                    {
                        Id = issueTransaction.Asset.Id,
                        Name = issueTransaction.Asset.Name,
                        DeedcoinDescription = deedcoinDescription,
                        Timestamp = issueTransaction.Timestamp
                    };
                    IssuedDeedcoins.TryAdd(deedcoinDescription.token, deedcoinAsset);
                }
            }
        }

        private void FillAccountDeedcoins()
        {
            var objects = node.GetObjects("assets/nft/{0}/limit/{1}", account.Address, limit);
            foreach (var obj in objects)
            {
                var description = obj.FirstOrDefault(_ => _.Key.Equals("description")).Value.ToString();
                var deedcoinDescription = DeedcoinDescriptionBuilder.Build(description);
                if (deedcoinDescription == null) continue;
                if (AccountDeedcoins.ContainsKey(deedcoinDescription.token)) continue;
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
                AccountDeedcoins.TryAdd(deedcoinDescription.token, deedcoinAsset);
            }
        }

        private static string GetTransactionId(Transaction transaction)
        {
            if (transaction == null) return string.Empty;
            if (transaction is IssueTransaction) return (transaction as IssueTransaction).Asset.Id;
            return transaction.GenerateId();
        }


    }
}
