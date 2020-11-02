using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using SampleBackendNet.Services;
using Virgil.Crypto;
using Virgil.SDK.Common;
using Virgil.SDK.Web.Authorization;

namespace SampleBackendNet
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // setup Virgil JWT generator
            services.AddTransient(s => SetupJwtGenerator());

            services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<IVirgilService, VirgilService>();

            // Using JWT authentication to auth at own backend
            // Can be changed to any other authentication scheme
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.RequireHttpsMetadata = false;
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidIssuer = Configuration["Jwt:Issuer"],
                            ValidateAudience = true,
                            ValidAudience = Configuration["Jwt:Audience"],
                            ValidateLifetime = true,

                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Secret"])),
                            ValidateIssuerSigningKey = true
                        };
                    });

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        // method for JWT generator setup
        private JwtGenerator SetupJwtGenerator()
        {
            // App Key (you got this Key at Virgil Dashboard)
            var appKeyBase64 = Configuration["Virgil:APP_KEY"];
            var privateKeyData = Bytes.FromString(appKeyBase64, StringEncoding.BASE64);

            // Crypto library imports a private key into a necessary format
            var crypto = new VirgilCrypto();
            var appKey = crypto.ImportPrivateKey(privateKeyData);

            //  initialize accessTokenSigner that signs users JWTs
            var accessTokenSigner = new VirgilAccessTokenSigner();

            // use your App Credentials you got at Virgil Dashboard:
            var appId = Configuration["Virgil:APP_ID"]; // App ID
            var appKeyId = Configuration["Virgil:APP_KEY_ID"]; // App Key ID
            var ttl = TimeSpan.FromHours(1); // 1 hour (JWT's lifetime)

            // setup JWT generator with necessary parameters:
            return new JwtGenerator(appId, appKey, appKeyId, ttl, accessTokenSigner);
        }
    }
}
