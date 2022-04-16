using Newtonsoft.Json;
using Waves.standard;

namespace WavesNft.Api.Utils;
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
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

    public (DeedcoinAsset?, string message) MintDeedcoin(DeedcoinDescription deedcoinDescription)
    {
        DeedcoinAsset deedcoinAsset;
        if (_deedcoinStore.IssuedDeedcoinsContainsKey(deedcoinDescription.token))
        {
            deedcoinAsset = _deedcoinStore.IssuedDeedcoinsValue(deedcoinDescription.token);
            return (deedcoinAsset, MatchDeedcoinDescription(deedcoinAsset, deedcoinDescription));
        }
        deedcoinAsset = new DeedcoinAsset
        {
            Name = $"DeedCoin {deedcoinDescription.series}#{deedcoinDescription.number}",
            DeedcoinDescription = deedcoinDescription,
            Timestamp = DateTime.Now
        };
        var description = JsonConvert.SerializeObject(deedcoinDescription);
        var asset = node.IssueAsset(account: account, name: deedcoinAsset.Name, description: description, quantity: 1, decimals: 0, reissuable: false, fee: fee);
        deedcoinAsset.Id = asset.Id;
        _deedcoinStore.IssuedDeedcoinsTryAdd(deedcoinDescription.token, deedcoinAsset);
        _deedcoinStore.AccountDeedcoinsTryAdd(deedcoinDescription.token, deedcoinAsset);
        return (deedcoinAsset, string.Empty);
    }

    public (DeedcoinAsset?, string message) TransferDeedcoin(string recipient, DeedcoinDescription deedcoinDescription)
    {
        if (!_deedcoinStore.AccountDeedcoinsContainsKey(deedcoinDescription.token)) return (null, "Account no contain this DeedCoin");
        var deedcoinAsset = _deedcoinStore.AccountDeedcoinsValue(deedcoinDescription.token);
        var message = MatchDeedcoinDescription(deedcoinAsset, deedcoinDescription);
        if (!string.IsNullOrEmpty(message)) return (null, message);

        var asset = node.GetAsset(deedcoinAsset.Id);
        message = node.Transfer(account, recipient, asset, 1, $"Take my {asset.Name}");
        _deedcoinStore.AccountDeedcoinsTryRemove(deedcoinDescription.token, out deedcoinAsset);
        return (deedcoinAsset, string.Empty);
    }

    private string MatchDeedcoinDescription(DeedcoinAsset deedcoinAsset, DeedcoinDescription deedcoinDescription)
    {
        if (deedcoinAsset == null) return "This DeedCoin not found";
        // compare id, series, number
        const string failMessage = "This DeedCoin match fail";
        if (deedcoinAsset.DeedcoinDescription == null) return failMessage;
        if (!deedcoinAsset.DeedcoinDescription.id.Equals(deedcoinDescription.id)) return failMessage;
        if (!deedcoinAsset.DeedcoinDescription.series.Equals(deedcoinDescription.series)) return failMessage;
        if (!deedcoinAsset.DeedcoinDescription.number.Equals(deedcoinDescription.number)) return failMessage;
        return string.Empty;
    }
}
