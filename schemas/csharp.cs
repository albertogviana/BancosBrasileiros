namespace BancosBrasileiros
{
    using System;
    using Newtonsoft.Json;

    public partial class Bank
    {
        [JsonProperty("COMPE")]
        public string Compe { get; set; }

        [JsonProperty("ISPB")]
        public string Ispb { get; set; }

        [JsonProperty("Document")]
        public string Document { get; set; }

        [JsonProperty("FiscalName")]
        public string FiscalName { get; set; }

        [JsonProperty("FantasyName")]
        public string FantasyName { get; set; }

        [JsonProperty("Url")]
        public string Url { get; set; }

        [JsonProperty("DateRegistered")]
        public DateTimeOffset DateRegistered { get; set; }

        [JsonProperty("DateUpdated")]
        public DateTimeOffset? DateUpdated { get; set; }

        [JsonProperty("DateRemoved")]
        public DateTimeOffset? DateRemoved { get; set; }

        [JsonProperty("IsRemoved")]
        public bool IsRemoved { get; set; }
    }
}