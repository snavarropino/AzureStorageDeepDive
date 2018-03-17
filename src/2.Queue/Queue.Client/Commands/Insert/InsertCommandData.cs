namespace Queue.Cli.Commands.Insert
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