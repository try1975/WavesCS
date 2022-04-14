using System.Collections.Generic;
using Waves.standard.Transactions;
using WavesNft.Api.Model;

namespace WavesNft.Api.Utils
{
    public interface IWavesApiService
    {
        IEnumerable<Transaction> GetTransactionsByAddress(string address);
        bool DeedcoinIssued(string token);
        DeedcoinAsset GetDeedcoinByToken(string token);
        DeedcoinAsset MintDeedcoin(DeedcoinDescription deedcoinDescription);
    }
}
