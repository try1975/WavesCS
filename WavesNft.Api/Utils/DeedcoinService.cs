using Newtonsoft.Json;
using Waves.standard;

namespace WavesNft.Api.Utils
{
    public class DeedcoinService : IDeedcoinService
    {
        private const decimal fee = 0.001m;

        private readonly Node node;
        private readonly PrivateKeyAccount account;
        private readonly IDeedcoinStore _deedcoinStore;

        public DeedcoinService(Node node, PrivateKeyAccount account, IDeedcoinStore deedcoinStore)
        {
            this.node = node;
            this.account = account;
            _deedcoinStore = deedcoinStore;
        }

        public bool DeedcoinIssued(string token)
        {
            _deedcoinStore.RefreshIssuedDeedcoins();
            return _deedcoinStore.IssuedDeedcoins.ContainsKey(token);
        }

        public bool DeedcoinNotTrasfered(string token)
        {
            _deedcoinStore.RefreshAccountDeedcoins();
            return _deedcoinStore.AccountDeedcoins.ContainsKey(token);
        }

        public DeedcoinAsset? GetDeedcoinByToken(string token)
        {
            if (_deedcoinStore.IssuedDeedcoins.ContainsKey(token)) return _deedcoinStore.IssuedDeedcoins[token];
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
            _deedcoinStore.IssuedDeedcoins.TryAdd(deedcoinDescription.token, deedcoinAsset);
            _deedcoinStore.AccountDeedcoins.TryAdd(deedcoinDescription.token, deedcoinAsset);
            return deedcoinAsset;
        }

        public DeedcoinAsset? TransferDeedcoin(string recipient, DeedcoinDescription deedcoinDescription)
        {
            if (!_deedcoinStore.AccountDeedcoins.ContainsKey(deedcoinDescription.token)) return null;
            var deedcoinAsset = _deedcoinStore.AccountDeedcoins[deedcoinDescription.token];
            var asset = node.GetAsset(deedcoinAsset.Id);
            var result = node.Transfer(account, recipient, asset, 1, $"Take my {asset.Name}");
            _deedcoinStore.AccountDeedcoins.TryRemove(deedcoinDescription.token, out deedcoinAsset);
            return deedcoinAsset;
        }
    }
}
