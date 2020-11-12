﻿using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using ACG.api.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace ACG.api.Controllers
{
    [ApiController]
    [Route("/")]
    public class KeyrockAuthMediationController : ControllerBase
    {
        private readonly IConfiguration configuration;

        public KeyrockAuthMediationController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        [HttpGet("cb")]
        public async Task<IActionResult> AuthCallbackMediation(string code, string state)
        {
            var keryockAuth = configuration.GetSection("keryockAuth");
            var clientId = keryockAuth.GetValue<string>("clientId");
            var clientSecret = keryockAuth.GetValue<string>("clientSecret");
            var tokenUrl = keryockAuth.GetValue<string>("tokenUrl");
            var redirectUrl = keryockAuth.GetValue<string>("redirectUrl");

            var client = new HttpClient();

            var tokenRequestParameters = $"grant_type=authorization_code&code={code}&redirect_uri={redirectUrl}";
            var requestData = new StringContent(tokenRequestParameters, Encoding.UTF8, "application/x-www-form-urlencoded");
            var basicauthtoken = Encoding.ASCII.GetBytes($"{clientId}:{clientSecret}");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(basicauthtoken));
            
            var response = await client.PostAsync(tokenUrl, requestData);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var tokenObject = JsonConvert.DeserializeObject<KeyrockTokenResponse>(await response.Content.ReadAsStringAsync());
                RedirectResult redirectResult = Redirect( $"{state}?access_token={tokenObject.AccessToken}" );
                return redirectResult;
            }

            return NotFound();
        }

    }
}

