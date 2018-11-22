using ModernHttpClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SolariClientes.Logic
{
    class RequestWS
    {
        public static string CriptografaSHA256(string pin)
        {
            SHA256Managed crypt = new SHA256Managed();
            StringBuilder hash = new StringBuilder();
            byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(pin), 0, Encoding.UTF8.GetByteCount(pin));
            foreach (byte theByte in crypto)
            {
                hash.Append(theByte.ToString("x2"));
            }
            var retorno = hash.ToString();
            return retorno.TrimStart('"').TrimEnd('"');
        }

        public static async Task<HttpResponseMessage> RequestGET(string sdsUrl)
        {
            try
            {
                var client = new HttpClient(new NativeMessageHandler(
                    throwOnCaptiveNetwork: false,
                    customSSLVerification: true))
                { BaseAddress = new Uri(MainPage.apiURI, UriKind.Absolute) };

                client.DefaultRequestHeaders.Add("Accept", "application/json");                

                var response = await client.GetAsync(sdsUrl);

                response.EnsureSuccessStatusCode();

                return response;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public static async Task<HttpResponseMessage> RequestPOST(string sdsUrl, string json)
        {
            try
            {               
                var client = new HttpClient(new NativeMessageHandler(
                    throwOnCaptiveNetwork: false,
                    customSSLVerification: true))
                { BaseAddress = new Uri(MainPage.apiURI, UriKind.Absolute) };

                client.DefaultRequestHeaders.Add("Accept", "application/json");

                client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
                var response = await client.PostAsync(sdsUrl, new StringContent(json, Encoding.UTF8, "application/json"));

                response.EnsureSuccessStatusCode();

                return response;
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}
