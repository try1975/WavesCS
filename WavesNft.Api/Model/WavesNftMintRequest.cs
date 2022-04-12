namespace WavesNft.Api.Model
{
    public class WavesNftMintRequest
    {
        public int id { get; set; }
        public string version { get; set; }
        public int series { get; set; }
        public int number { get; set; }
        public int user_id { get; set; }
        public string owner { get; set; }
        public string wallet_id { get; set; }
        public string status { get; set; }
        public string importance { get; set; }
        public string comment { get; set; }
        public int available { get; set; }
        public string token { get; set; }
        public int first_price { get; set; }
        public string assignment_date { get; set; }
        public string first_word { get; set; }
        public string midl_word { get; set; }
        public string last_word { get; set; }
        public bool hide_comment { get; set; }
        public bool hide_status { get; set; }
        public DateTime? deleted_at { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public Seria seria { get; set; }
        public User user { get; set; }
    }

    public class Seria
    {
        public int id { get; set; }
        public int active { get; set; }
        public string desc { get; set; }
        public DateTime date_activation { get; set; }
        public string certificate { get; set; }
        public string thumb { get; set; }
    }

    public class User
    {
        public int id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public DateTime? email_verified_at { get; set; }
        public string phone { get; set; }
        public bool subscribes { get; set; }
        public bool active { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public string city_id { get; set; }
        public string avatar { get; set; }
        public string thumb { get; set; }
    }

}
