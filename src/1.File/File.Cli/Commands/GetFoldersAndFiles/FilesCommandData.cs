namespace File.Cli.Commands.GetFoldersAndFiles
{
    internal class FilesCommandData: BaseCommandData
    {
        public string Share { get; set; }

        public override bool Validate()
        {
            return !(string.IsNullOrWhiteSpace(StorageAccount)
                     || string.IsNullOrWhiteSpace(StorageKey)
                     || string.IsNullOrWhiteSpace(Share));
        }
    }
}