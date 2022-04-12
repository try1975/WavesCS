namespace WavesNft.Api.Model
{
    public class DeedCoinDescription
    {
        public int id { get; set; }
        public string type { get; set; } // "unique"
        public string url { get; set; }
        public int series { get; set; }
        public int number { get; set; }
        public string token { get; set; }
    }
}
