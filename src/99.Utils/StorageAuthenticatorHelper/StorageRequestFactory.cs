using System;
using System.Globalization;
using System.Net.Http;

namespace StorageAuthenticatorHelper
{
    public class StorageRequestFactory
    {
        public string StorageAccountName { get; }
        public string StorageAccountKey { get; }

        public StorageRequestFactory(string storageAccountName, string storageAccountKey)
        {
            StorageAccountName = storageAccountName;
            StorageAccountKey = storageAccountKey;
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

            // Add the authorization header.
            httpRequestMessage.Headers.Authorization = StorageAuthenticationHelper.GetAuthorizationHeader(
                StorageAccountName, StorageAccountKey, now, httpRequestMessage);

            return httpRequestMessage;
        }
    }
}
