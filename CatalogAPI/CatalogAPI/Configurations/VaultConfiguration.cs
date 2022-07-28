using VaultSharp;
using VaultSharp.V1.AuthMethods;
using VaultSharp.V1.AuthMethods.Token;

namespace CatalogAPI.Models
{
    public class VaultConfiguration
    {
        private IConfiguration _configuration;
        public VaultConfiguration(IConfiguration configuration)
        {
            _configuration = configuration;
        }
       
        public async Task<Dictionary<string, object>> GetDBCredentials()
        {
          string token = _configuration["Root_Token"];
            string url = _configuration["Vault_Url"];

            // Initialize one of the several auth methods.
            IAuthMethodInfo authMethod = new TokenAuthMethodInfo(token);

            // Initialize settings. You can also set proxies, custom delegates etc. here.
            var vaultClientSettings = new VaultClientSettings(url, authMethod);

            IVaultClient vaultClient = new VaultClient(vaultClientSettings);
            Console.WriteLine(vaultClient.V1.Secrets);
            
            var result = await vaultClient.V1.Secrets.KeyValue.V1.ReadSecretAsync("mssqlserver", 
                "secret", null);

            return result.Data;

        }
    }
}
