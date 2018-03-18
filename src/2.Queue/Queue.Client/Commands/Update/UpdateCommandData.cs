namespace Queue.Cli.Commands.Update
{
    internal class UpdateCommandData: BaseCommandData
    {
        public string UpdateMessage { get; set; }

        public override bool Validate()
        {
            if (string.IsNullOrWhiteSpace(UpdateMessage))
                return false;

            return base.Validate();
        }
    }
}