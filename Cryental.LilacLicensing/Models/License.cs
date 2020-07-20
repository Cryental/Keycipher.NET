using Newtonsoft.Json;

namespace Cryental.LilacLicensing.Models
{
    public class License
    {
        public class RawInput
        {
            [JsonProperty("result")]
            public bool Result { get; set; }

            [JsonProperty("code")]
            public string Code { get; set; }

            [JsonProperty("message")]
            public string Message { get; set; }

            [JsonProperty("object")]
            public Licenses.Object Object { get; set; }
        }
    }
}