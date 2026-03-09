

using IdentityModel.Client;

namespace Congrats.Application.Helpers
{
    public class TokenService : ITokenService
    {
        private IHttpContextAccessor _contextAccessor;
        private IConfiguration _configuration;

        public TokenService(IHttpContextAccessor contextAccessor, IConfiguration configuration)
        {
            _contextAccessor = contextAccessor;
            _configuration = configuration;
        }

        public async Task<CallOptions> AcquireToken()
        {
            string jwtToken = _contextAccessor.HttpContext.Request.Headers["Authorization"];
            if (string.IsNullOrWhiteSpace(jwtToken))
            {
                return new CallOptions();
            }

            if (jwtToken.StartsWith("Bearer "))
            {
                jwtToken = jwtToken.Substring("Bearer ".Length);
            }
            var metadata = new Metadata
            {
               { "Authorization", $"Bearer {jwtToken}" }
            };
            var callOptions = new CallOptions(metadata);
            return await Task.FromResult(callOptions);
        }
        public async Task<CallOptions> GetAccessTokenAsync()
        {
            var client = new HttpClient();
            
            var disco = await client.GetDiscoveryDocumentAsync(_configuration.GetSection("IdentityUrl").Get<string>());

            var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = disco.TokenEndpoint,
                ClientId = "AggregatorClient",
                ClientSecret = "secret",
                Scope = "CPAggregatorAPI"
            });

            if (tokenResponse.IsError)
            {
                throw new Exception($"Error getting access token: {tokenResponse.Error}");
            }

            var jwtToken = tokenResponse.AccessToken;

            var metadata = new Metadata
            {
               { "Authorization", $"Bearer {jwtToken}" }
            };
            var callOptions = new CallOptions(metadata);
            return await Task.FromResult(callOptions);
        }


    }
}
