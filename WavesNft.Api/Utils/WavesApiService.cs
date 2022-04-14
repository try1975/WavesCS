using Newtonsoft.Json;
using System.Collections.Concurrent;
using Waves.standard;
using Waves.standard.Transactions;
using WavesNft.Api.Model;

namespace WavesNft.Api.Utils
{
    public class WavesApiService : IWavesApiService
    {
        private const int limit = 25;
        private const int refreshLimit = 1;
        private readonly DateTime maxTransactionAge = new DateTime(2022, 4, 1);

        private readonly Node node;
        private readonly PrivateKeyAccount account;
        private readonly List<Transaction> _transactions = new List<Transaction>();
        private readonly ConcurrentDictionary<string, Transaction> transactionsDictionary = new ConcurrentDictionary<string, Transaction>();
        private readonly ConcurrentDictionary<string, DeedcoinAsset> DeedcoinAssets = new ConcurrentDictionary<string, DeedcoinAsset>();

        public WavesApiService(Node node, PrivateKeyAccount account)
        {
            this.node = node;
            this.account = account;
            FillTransactions();
            TransactionDictionaryAddRange(_transactions);
            FillDeedCoinAssets();
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

        private void FillDeedCoinAssets()
        {
            //if (_transactions == null) return;
            //if (!_transactions.Any()) return;

            //var issueTransactions = _transactions.Where(x => x.GetType() == typeof(IssueTransaction)).Select(x => x as IssueTransaction).ToList();
            var issueTransactions = transactionsDictionary.Where(kvp => kvp.Value.GetType() == typeof(IssueTransaction)).Select(kvp => kvp.Value as IssueTransaction).ToList();
            foreach (var issueTransaction in issueTransactions)
            {
                var deedcoinDescription = DeedcoinDescriptionBuilder.Build(issueTransaction.Description);
                if (deedcoinDescription != null)
                {
                    if (string.IsNullOrEmpty(deedcoinDescription.token)) continue;
                    if (DeedcoinAssets.ContainsKey(deedcoinDescription.token)) continue;
                    var deedcoinAsset = new DeedcoinAsset
                    {
                        Id = issueTransaction.Asset.Id,
                        Name = issueTransaction.Asset.Name,
                        DeedcoinDescription = deedcoinDescription,
                        Timestamp = issueTransaction.Timestamp
                    };
                    DeedcoinAssets.TryAdd(deedcoinDescription.token, deedcoinAsset);
                }
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

        public IEnumerable<Transaction> GetTransactionsByAddress(string address)
        {
            return node.GetTransactions(address, 1000);
        }

        public bool DeedcoinIssued(string token)
        {
            Refresh();
            return DeedcoinAssets.ContainsKey(token);
        }

        public DeedcoinAsset GetDeedcoinByToken(string token)
        {
            if (DeedcoinAssets.ContainsKey(token)) return DeedcoinAssets[token];
            return null;
        }

        public DeedcoinAsset MintDeedcoin(DeedcoinDescription deedcoinDescription)
        {

            DeedcoinAsset deedcoinAsset;
            if (DeedcoinIssued(deedcoinDescription.token))
            {
                deedcoinAsset = GetDeedcoinByToken(deedcoinDescription.token);
            }
            else
            {
                deedcoinAsset = new DeedcoinAsset
                {
                    Name = $"DeedCoin {deedcoinDescription.series}#{deedcoinDescription.number}",
                    DeedcoinDescription = deedcoinDescription
                };
                var description = JsonConvert.SerializeObject(deedcoinDescription);
                var asset = node.IssueAsset(account: account, name: deedcoinAsset.Name, description: description, quantity: 1, decimals: 0, reissuable: false, fee: 0.001m);
                deedcoinAsset.Id = asset.Id;
                deedcoinAsset.Timestamp = asset.IssueTimestamp;
                DeedcoinAssets.TryAdd(deedcoinDescription.token, deedcoinAsset);
            }

            return deedcoinAsset;
        }
    }
}
