using Newtonsoft.Json;
using WavesNft.Api.Model;

namespace WavesNft.Api.Utils
{
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
                token = deedcoinMintRequest.token
            };
            return deedcoinDescription;
        }

        public static DeedcoinDescription Build(DeedcoinTransferRequest deedcoinTransferRequest)
        {
            var deedcoinDescription = new DeedcoinDescription
            {
                id=deedcoinTransferRequest.id,
                series = deedcoinTransferRequest.series,
                number=deedcoinTransferRequest.number,
                token = deedcoinTransferRequest.token
            };
            return deedcoinDescription;
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
}
