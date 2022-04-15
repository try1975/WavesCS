namespace WavesNft.Api.Utils;

/// <summary>
/// Serialize to asset description
/// </summary>
public class DeedcoinDescription
{
    public int id { get; set; }
    public string type { get; set; } // "unique"
    public string url { get; set; }
    public int series { get; set; }
    public int number { get; set; }
    public string token { get; set; }
}
