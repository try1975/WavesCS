namespace WavesNft.Api.Utils
{
    /// <summary>
    /// Deedcoin NFT on WAVES blockchain
    /// </summary>
    public class DeedcoinAsset
    {
        /// <summary>
        /// id in externall system 
        /// </summary>
        public string? Id { get; set; }
        /// <summary>
        /// Example: DeedCoin 12#438
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// Serialize to asset description
        /// </summary>
        public DeedcoinDescription? DeedcoinDescription { get; set; }
        /// <summary>
        /// asset issued at
        /// </summary>
        public DateTime Timestamp { get; set; }
    }
}
