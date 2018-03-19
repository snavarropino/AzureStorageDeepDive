namespace File.Cli.Commands
{
    internal class BaseCommandData
    {
        public string StorageAccount { get; set; }
        public string StorageKey { get; set; }

        public virtual bool Validate()
        {
            if (!string.IsNullOrWhiteSpace(StorageAccount) && string.IsNullOrWhiteSpace(StorageKey)
                ||
                string.IsNullOrWhiteSpace(StorageAccount) && !string.IsNullOrWhiteSpace(StorageKey))
            {
                return false;
            }

            return true;
        }
    }
}