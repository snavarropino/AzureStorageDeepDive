namespace Queue.Client.Commands
{
    internal class InsertCommandData: BaseCommandData
    {
        public string Message { get; set; }

        public override bool Validate()
        {
            if (string.IsNullOrWhiteSpace(Message))
                return false;

            return base.Validate();
        }
    }
}