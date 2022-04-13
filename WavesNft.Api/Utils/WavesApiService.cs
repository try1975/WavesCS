using Newtonsoft.Json;
using Waves.standard;
using Waves.standard.Transactions;
using WavesNft.Api.Model;

namespace WavesNft.Api.Utils
{
    public class WavesApiService : IWavesApiService
    {
        private const int limit = 50;
        private const int refreshLimit = 5;
        private readonly DateTime maxTransactionAge = new DateTime(2022, 4, 1);

        private readonly Node node;
        private readonly PrivateKeyAccount account;
        private readonly List<Transaction> transactions = new List<Transaction>();
        private readonly Dictionary<string, DeedCoinDescription> DeedCoinDescriptions = new Dictionary<string, DeedCoinDescription>();
        private Transaction newestTransaction;

        public WavesApiService(Node node, PrivateKeyAccount account)
        {
            this.node = node;
            this.account = account;
            FillTransactions();
            SetNewestTransaction();
            FillDeedCoinDescriptions();
        }

        private void FillTransactions()
        {

            var tr = node.GetTransactions(account.Address, limit);
            newestTransaction = transactions.FirstOrDefault();
            transactions.AddRange(tr);
            if (transactions.Any())
            {
                var lastTransaction = transactions.LastOrDefault();
                if (lastTransaction == null) return;
                var afterId = lastTransaction.GenerateId();
                tr = node.GetTransactionsByAddressAfterId(account.Address, afterId, limit);
                while (tr.Length > 0 && lastTransaction.Timestamp >= maxTransactionAge)
                {
                    transactions.AddRange(tr);
                    lastTransaction = tr.LastOrDefault();
                    if (lastTransaction == null) return;
                    afterId = GetTransactionId(lastTransaction);
                    tr = node.GetTransactionsByAddressAfterId(account.Address, afterId, limit);

                }
            }
        }

        private void FillDeedCoinDescriptions()
        {
            if (transactions == null) return;
            if (!transactions.Any()) return;

            var issueTransactions = transactions.Where(x => x.GetType() == typeof(IssueTransaction)).Select(x => x as IssueTransaction).ToList();
            foreach (var issueTransaction in issueTransactions)
            {
                if (string.IsNullOrEmpty(issueTransaction.Description)) continue;
                if (!issueTransaction.Description.StartsWith("{")) continue;
                try
                {
                    var deedCoinDescription = JsonConvert.DeserializeObject<DeedCoinDescription>(issueTransaction.Description);
                    if (deedCoinDescription != null)
                    {
                        if (string.IsNullOrEmpty(deedCoinDescription.token)) continue;
                        if (DeedCoinDescriptions.ContainsKey(deedCoinDescription.token)) continue;
                        DeedCoinDescriptions.Add(deedCoinDescription.token, deedCoinDescription);
                    }
                }
                catch (Exception)
                {
                }
            }
        }

        private void SetNewestTransaction()
        {
            if (transactions == null) return;
            if (!transactions.Any()) return;

            var maxTimestamp = transactions.Max(x => x.Timestamp);
            newestTransaction = transactions.FirstOrDefault(x => x.Timestamp.Equals(maxTimestamp));
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

        public bool DeedCoinIssued(string token)
        {
            return DeedCoinDescriptions.ContainsKey(token);
        }
    }
}
