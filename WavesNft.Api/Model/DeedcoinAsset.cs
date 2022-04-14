namespace WavesNft.Api.Model
{
    public class DeedcoinAsset
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public DeedcoinDescription DeedcoinDescription { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
