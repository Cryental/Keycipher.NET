using System.Collections.Generic;
using Newtonsoft.Json;

namespace Keycipher.Models
{
    public class License
    {
        public class RawInputLists
        {
            [JsonProperty("result")]
            public bool Result { get; set; }

            [JsonProperty("code")]
            public string Code { get; set; }

            [JsonProperty("message")]
            public string Message { get; set; }

            [JsonProperty("objects")]
            public List<Licenses.Object> Objects { get; set; }
        }

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