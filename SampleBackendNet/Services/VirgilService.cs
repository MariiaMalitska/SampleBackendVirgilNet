using SampleBackendNet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Virgil.SDK.Web.Authorization;

namespace SampleBackendNet.Services
{
    // Service to authenticate at Virgil server
    public interface IVirgilService
    {
        VirgilResponse GenerateVirgilJwt(string identity);
    }
    public class VirgilService : IVirgilService
    {
        private readonly JwtGenerator _jwtGenerator;

        public VirgilService(JwtGenerator jwtGenerator)
        {
            _jwtGenerator = jwtGenerator;
        }

        public VirgilResponse GenerateVirgilJwt(string identity)
        {
            // generate JWT for a user
            // remember that you must provide each user with his unique JWT
            // each JWT contains unique user's identity
            // identity can be any value: name, email, some id etc.
            var jwt = _jwtGenerator.GenerateToken(identity);

            // as result you get users JWT, it looks like this: "eyJraWQiOiI3MGI0NDdlMzIxZjNhMGZkIiwidHlwIjoiSldUIiwiYWxnIjoiVkVEUzUxMiIsImN0eSI6InZpcmdpbC1qd3Q7dj0xIn0.eyJleHAiOjE1MTg2OTg5MTcsImlzcyI6InZpcmdpbC1iZTAwZTEwZTRlMWY0YmY1OGY5YjRkYzg1ZDc5Yzc3YSIsInN1YiI6ImlkZW50aXR5LUFsaWNlIiwiaWF0IjoxNTE4NjEyNTE3fQ.MFEwDQYJYIZIAWUDBAIDBQAEQP4Yo3yjmt8WWJ5mqs3Yrqc_VzG6nBtrW2KIjP-kxiIJL_7Wv0pqty7PDbDoGhkX8CJa6UOdyn3rBWRvMK7p7Ak"
            // you can provide users with JWT at registration or authorization steps
            // Send a JWT to client-side
            return new VirgilResponse(jwt.ToString());
        }
    }
}
