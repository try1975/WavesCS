using System.Collections.Generic;
using Waves.standard.Transactions;

namespace WavesNft.Api.Utils
{
    public interface IWavesApiService
    {
        IEnumerable<Transaction> GetTransactionsByAddress(string address);
        bool DeedCoinIssued(string token);
    }
}
