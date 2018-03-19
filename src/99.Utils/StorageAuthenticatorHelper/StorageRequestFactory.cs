using System;
using System.Globalization;
using System.Net.Http;

namespace StorageAuthenticatorHelper
{
    public class StorageRequestFactory
    {
        public string StorageAccountName { get; private set; }
        public string StorageAccountKey { get; private set; }

        public StorageRequestFactory(string storageAccountName, string storageAccountKey)
        {
            StorageAccountName = storageAccountName;
            StorageAccountKey = storageAccountKey;

            EnsureAccount();
        }

        private void EnsureAccount()
        {
            if(string.IsNullOrWhiteSpace(StorageAccountName) || string.IsNullOrWhiteSpace(StorageAccountKey))
            {
                StorageAccountName = "devstoreaccount1";
                StorageAccountKey = "Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==";
            }
        }

        public HttpRequestMessage CreateRequest(HttpMethod method, string uri, Byte[] requestPayload = null)
        {
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri)
            {
                Content = (requestPayload == null) ? null : new ByteArrayContent(requestPayload)
            };

            DateTime now = DateTime.UtcNow;

            httpRequestMessage.Headers.Add("x-ms-date", now.ToString("R", CultureInfo.InvariantCulture));
            httpRequestMessage.Headers.Add("x-ms-version", "2017-04-17");
            // If you need any additional headers, add them here before creating
            //   the authorization header. 

            // Add the authorization header (and log it)
            var authHeader= StorageAuthenticationHelper.GetAuthorizationHeader(
                StorageAccountName, StorageAccountKey, now, httpRequestMessage);
            Console.WriteLine($"Authorization header: {authHeader}");
            httpRequestMessage.Headers.Authorization = authHeader;

            Console.WriteLine($"x-ms-date: {now.ToString("R", CultureInfo.InvariantCulture)}");

            return httpRequestMessage;
        }
    }
}
