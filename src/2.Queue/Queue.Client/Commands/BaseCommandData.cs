namespace Queue.Client.Commands
{
    internal class BaseCommandData
    {
        public string Queue { get; set; }
        public string StorageAccount { get; set; }
        public string StorageKey { get; set; }

        public virtual bool Validate()
        {
            if (string.IsNullOrWhiteSpace(Queue))
                return false;

            if(!string.IsNullOrWhiteSpace(StorageAccount) && string.IsNullOrWhiteSpace(StorageKey))
                return false;

            return true;
        }
    }
}