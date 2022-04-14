using Newtonsoft.Json;
using System.Collections.Concurrent;
using Waves.standard;
using Waves.standard.Transactions;
using WavesNft.Api.Model;

namespace WavesNft.Api.Utils
{
    public class DeedcoinService : IDeedcoinService
    {
        private const int limit = 25;
        private const int refreshLimit = 1;
        private readonly DateTime maxTransactionAge = new DateTime(2022, 4, 1);
        private const decimal fee = 0.001m;

        private readonly Node node;
        private readonly PrivateKeyAccount account;
        private readonly List<Transaction> _transactions = new List<Transaction>();
        private readonly ConcurrentDictionary<string, Transaction> transactionsDictionary = new ConcurrentDictionary<string, Transaction>();
        private readonly ConcurrentDictionary<string, DeedcoinAsset> IssuedDeedcoins = new ConcurrentDictionary<string, DeedcoinAsset>();
        private readonly ConcurrentDictionary<string, DeedcoinAsset> AccountDeedcoins = new ConcurrentDictionary<string, DeedcoinAsset>();

        public DeedcoinService(Node node, PrivateKeyAccount account)
        {
            this.node = node;
            this.account = account;
            FillTransactions();
            TransactionDictionaryAddRange(_transactions);
            FillIssuedDeedcoins();
            FillAccountDeedcoins();
        }

        private void FillTransactions()
        {
            var tr = node.GetTransactions(account.Address, limit);
            _transactions.AddRange(tr);
            if (_transactions.Any())
            {
                var lastTransaction = _transactions.LastOrDefault();
                if (lastTransaction == null) return;
                var afterId = lastTransaction.GenerateId();
                tr = node.GetTransactionsByAddressAfterId(account.Address, afterId, limit);
                while (tr.Length > 0 && lastTransaction.Timestamp >= maxTransactionAge)
                {
                    _transactions.AddRange(tr);
                    lastTransaction = tr.LastOrDefault();
                    if (lastTransaction == null) return;
                    afterId = GetTransactionId(lastTransaction);
                    tr = node.GetTransactionsByAddressAfterId(account.Address, afterId, limit);
                }
            }
        }

        private void TransactionDictionaryAddRange(IEnumerable<Transaction> transactions)
        {
            foreach (var transaction in transactions)
            {
                var id = GetTransactionId(transaction);
                if (transactionsDictionary.ContainsKey(id)) continue;
                transactionsDictionary.TryAdd(id, transaction);
            }
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
                AccountDeedcoins.TryAdd(deedcoinDescription.token, deedcoinAsset);
            }
        }

        private void Refresh()
        {
            var transactions = node.GetTransactions(account.Address, refreshLimit);
            if (transactions == null) return;
            if (!transactions.Any()) return;
            var id = GetTransactionId(transactions.FirstOrDefault());
            if (transactionsDictionary.ContainsKey(id)) return;
            TransactionDictionaryAddRange(transactions);
        }

        private static string GetTransactionId(Transaction transaction)
        {
            if (transaction == null) return string.Empty;
            if (transaction is IssueTransaction) return (transaction as IssueTransaction).Asset.Id;
            return transaction.GenerateId();
            //switch ((TransactionType)tx.GetByte("type"))
            //{
            //    case TransactionType.Alias: return new AliasTransaction(tx);
            //    case TransactionType.Burn: return new BurnTransaction(tx);
            //    case TransactionType.DataTx: return new DataTransaction(tx);
            //    case TransactionType.Lease: return new LeaseTransaction(tx);
            //    case TransactionType.Issue: return new IssueTransaction(tx);
            //    case TransactionType.LeaseCancel: return new CancelLeasingTransaction(tx);
            //    case TransactionType.MassTransfer: return new MassTransferTransaction(tx);
            //    case TransactionType.Reissue: return new ReissueTransaction(tx);
            //    case TransactionType.SetScript: return new SetScriptTransaction(tx);
            //    case TransactionType.SponsoredFee: return new SponsoredFeeTransaction(tx);
            //    case TransactionType.Transfer: return new TransferTransaction(tx);
            //    case TransactionType.Exchange: return new ExchangeTransaction(tx);
            //    case TransactionType.SetAssetScript: return new SetAssetScriptTransaction(tx);
            //    case TransactionType.InvokeScript: return new InvokeScriptTransaction(tx);
            //    default: return new UnknownTransaction(tx);
            //}
        }

        public bool DeedcoinIssued(string token)
        {
            Refresh();
            return IssuedDeedcoins.ContainsKey(token);
        }

        public DeedcoinAsset? GetDeedcoinByToken(string token)
        {
            if (IssuedDeedcoins.ContainsKey(token)) return IssuedDeedcoins[token];
            return null;
        }

        public DeedcoinAsset MintDeedcoin(DeedcoinDescription deedcoinDescription)
        {
            var deedcoinAsset = GetDeedcoinByToken(deedcoinDescription.token);
            if (deedcoinAsset != null) return deedcoinAsset;

            deedcoinAsset = new DeedcoinAsset
            {
                Name = $"DeedCoin {deedcoinDescription.series}#{deedcoinDescription.number}",
                DeedcoinDescription = deedcoinDescription
            };
            var description = JsonConvert.SerializeObject(deedcoinDescription);
            var asset = node.IssueAsset(account: account, name: deedcoinAsset.Name, description: description, quantity: 1, decimals: 0, reissuable: false, fee: fee);
            deedcoinAsset.Id = asset.Id;
            deedcoinAsset.Timestamp = asset.IssueTimestamp;
            IssuedDeedcoins.TryAdd(deedcoinDescription.token, deedcoinAsset);
            AccountDeedcoins.TryAdd(deedcoinDescription.token, deedcoinAsset);

            return deedcoinAsset;
        }

        public DeedcoinAsset? TransferDeedcoin(string recipient, DeedcoinDescription deedcoinDescription)
        {
            if (!AccountDeedcoins.ContainsKey(deedcoinDescription.token)) return null;
            var deedcoinAsset = AccountDeedcoins[deedcoinDescription.token];
            var asset = node.GetAsset(deedcoinAsset.Id);
            var result = node.Transfer(account, recipient, asset, 1, $"Take my {asset.Name}");
            AccountDeedcoins.TryRemove(deedcoinDescription.token, out deedcoinAsset);
            return deedcoinAsset;
        }
    }
}
