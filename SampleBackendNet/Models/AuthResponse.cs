using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleBackendNet.Models
{
    public class AuthResponse
    {
        public string Identity { get; set; }
        public string OtherInfo { get; set; }
        public string Token { get; set; }


        public AuthResponse(User user, string jwt)
        {
            Identity = user.Identity;
            OtherInfo = user.OtherInfo;
            Token = jwt;
        }
    }
}
