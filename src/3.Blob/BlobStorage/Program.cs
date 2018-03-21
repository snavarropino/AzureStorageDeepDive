using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;

namespace BlobStorage
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await SharedAccessSignature();
        }

        private static async Task SharedAccessSignature()
        {
            try
            {
                var sasToken =
                    "?se=2018-03-21T19%3A54%3A00Z&sp=rwl&sv=2017-04-17&sr=c&sig=cAlTS8hX1vOcoOlIkpW%2BARfia23jC1XLrsG6SuPI3Uw%3D";
                StorageCredentials credentials = new StorageCredentials(sasToken);
                CloudStorageAccount account =
                    new CloudStorageAccount(credentials, "georeplication", endpointSuffix: null, useHttps: true);
                CloudBlobClient client = account.CreateCloudBlobClient();
                CloudBlobContainer container = client.GetContainerReference("data");
                var blob = container.GetBlobReference("NCT00000102.txt");
                using (var stream = new MemoryStream())
                {
                    await blob.DownloadToStreamAsync(stream);
                    Console.WriteLine(Encoding.UTF8.GetString(stream.ToArray()));
                }

                blob.Metadata["azuges"] = "done";
                await blob.SetMetadataAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
