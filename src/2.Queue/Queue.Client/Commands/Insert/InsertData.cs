namespace Queue.Client.Commands
{
    internal class InsertData
    {
        public string Queue { get; set; }
        public string Message { get; set; }
        public string StorageAccount { get; set; }
        public string StorageKey { get; set; }

        public bool Validate()
        {
            if (string.IsNullOrWhiteSpace(Queue) || string.IsNullOrWhiteSpace(Message))
                return false;

            if(!string.IsNullOrWhiteSpace(StorageAccount) && string.IsNullOrWhiteSpace(StorageKey))
                return false;

            return true;

        }
    }
}