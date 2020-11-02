using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SampleBackendNet.Models
{
    public class User
    {
        public string Identity { get; set; }
        public string OtherInfo { get; set; }

        [JsonIgnore]
        public string Password { get; set; }
    }
}
