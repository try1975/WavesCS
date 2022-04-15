using Newtonsoft.Json;
using Waves.standard;

namespace WavesNft.Api.Utils;

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
        return _deedcoinStore.IssuedDeedcoinsContainsKey(token);
    }

    public bool DeedcoinNotTrasfered(string token)
    {
        return _deedcoinStore.AccountDeedcoinsContainsKey(token);
    }

    public DeedcoinAsset? GetDeedcoinByToken(string token)
    {
        if (_deedcoinStore.IssuedDeedcoinsContainsKey(token)) return _deedcoinStore.IssuedDeedcoinsValue(token);
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
        _deedcoinStore.IssuedDeedcoinsTryAdd(deedcoinDescription.token, deedcoinAsset);
        _deedcoinStore.AccountDeedcoinsTryAdd(deedcoinDescription.token, deedcoinAsset);
        return deedcoinAsset;
    }

    public DeedcoinAsset? TransferDeedcoin(string recipient, DeedcoinDescription deedcoinDescription)
    {
        if (!_deedcoinStore.AccountDeedcoinsContainsKey(deedcoinDescription.token)) return null;
        var deedcoinAsset = _deedcoinStore.AccountDeedcoinsValue(deedcoinDescription.token);
        var asset = node.GetAsset(deedcoinAsset.Id);
        var result = node.Transfer(account, recipient, asset, 1, $"Take my {asset.Name}");
        _deedcoinStore.AccountDeedcoinsTryRemove(deedcoinDescription.token, out deedcoinAsset);
        return deedcoinAsset;
    }
}
