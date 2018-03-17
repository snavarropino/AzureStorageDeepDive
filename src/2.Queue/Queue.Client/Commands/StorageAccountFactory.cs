using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;

namespace Queue.Client.Commands
{
    internal static class StorageAccountFactory
    {
        public static CloudStorageAccount Get(BaseCommandData insertCommandData)
        {
            if (string.IsNullOrWhiteSpace(insertCommandData.StorageAccount))
            {
                return CloudStorageAccount.DevelopmentStorageAccount;
            }

            StorageCredentials storageCredentials = new StorageCredentials(insertCommandData.StorageAccount, insertCommandData.StorageKey);
            return new CloudStorageAccount(storageCredentials, useHttps: true);
        }
    }
}