using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleBackendNet.Models
{
    public class VirgilResponse
    {
        public string VirgilJwt { get; set; }

        public VirgilResponse(string jwt)
        {
            VirgilJwt = jwt;
        }
    }
}
