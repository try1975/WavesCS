using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;
using WavesNft.Api.Model;

namespace WavesNft.Api.Utils;
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
public class DeedcoinDescriptionBuilder
{
    public static DeedcoinDescription Build(DeedcoinMintRequest deedcoinMintRequest)
    {
        var deedcoinDescription = new DeedcoinDescription
        {
            id = deedcoinMintRequest.id,
            type = "unique",
            url = deedcoinMintRequest.certificate_url,
            series = deedcoinMintRequest.series,
            number = deedcoinMintRequest.number,
            token = GetHashString(deedcoinMintRequest.token)
        };
        return deedcoinDescription;
    }

    public static DeedcoinDescription Build(DeedcoinTransferRequest deedcoinTransferRequest)
    {
        var deedcoinDescription = new DeedcoinDescription
        {
            id = deedcoinTransferRequest.id,
            series = deedcoinTransferRequest.series,
            number = deedcoinTransferRequest.number,
            token = GetHashString(deedcoinTransferRequest.token)
        };
        return deedcoinDescription;
    }

    public static byte[] GetHash(string inputString)
    {
        using HashAlgorithm algorithm = SHA1.Create(); //SHA256.Create();
        return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
    }

    private static string GetHashString(string inputString)
    {
        return inputString;
        var sb = new StringBuilder();
        foreach (byte b in GetHash(inputString))
            sb.Append(b.ToString("X2"));

        return sb.ToString();
    }

    public static DeedcoinDescription Build(string json)
    {
        if (string.IsNullOrEmpty(json)) return null;
        if (!json.StartsWith("{")) return null;
        try
        {
            var deedcoinDescription = JsonConvert.DeserializeObject<DeedcoinDescription>(json);
            return deedcoinDescription;
        }
        catch (Exception)
        {
        }
        return null;
    }
}
