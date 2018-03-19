using System;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml.Linq;
using CliUtils;
using Microsoft.Extensions.Configuration;
using StorageAuthenticatorHelper;

namespace File.Cli.Commands.GetFoldersAndFiles
{
    public class GetFoldersAndFilesCommand: ICommand
    {
        public IConfigurationRoot Configuration { get; set; }

        public GetFoldersAndFilesCommand(object[] args)
        {
            Configuration = new CommandArguments((string[])args).Configuration;
        }

        public async Task ExecuteAsync(int i)
        {
            var commandData = ParseArgs();
            if (commandData.Validate())
            {
                
                await GetFileSharesAsync(commandData);
                
            }
            else
            {
                PrintHelp();
            }
        }

        private async Task GetFileSharesAsync(FilesCommandData commandData)
        {
            var factory = new StorageRequestFactory(commandData.StorageAccount, commandData.StorageKey);
            String uri = string.Format($"http://{commandData.StorageAccount}.file.core.windows.net/{commandData.Share}/?restype=directory&comp=list");

            using (var httpRequestMessage = factory.CreateRequest(HttpMethod.Get, uri))
            {
                using (HttpResponseMessage httpResponseMessage = await new HttpClient().SendAsync(httpRequestMessage))
                {
                    if (httpResponseMessage.StatusCode == HttpStatusCode.OK)
                    {
                        String sharesString = await httpResponseMessage.Content.ReadAsStringAsync();
                        XElement x = XElement.Parse(sharesString);
                        foreach (XElement container in x.Element("Entries").Elements("Directory"))
                        {
                            Console.WriteLine("Directory: name = {0}", container.Element("Name").Value);
                        }
                        foreach (XElement container in x.Element("Entries").Elements("File"))
                        {
                            Console.WriteLine("File: name = {0}", container.Element("Name").Value);
                        }
                    }
                }
            }
        }

        private FilesCommandData ParseArgs()
        {
            return new FilesCommandData()
            {
                StorageAccount = Configuration["Account"] ?? Configuration["a"],
                StorageKey = Configuration["Key"] ?? Configuration["k"],
                Share = Configuration["Share"] ?? Configuration["s"]
            };
        }
        public void PrintHelp()
        {
            var executable = Assembly.GetExecutingAssembly().GetName().Name;
            var help =
                $@"GetFoldersAndFiles: List folder and files in a share folder (not recursive)

    Usage: {executable} {nameof(GetFoldersAndFilesCommand)}  [--a=<account> --k=<key> --s=<share>]

    Storage account name and key are mandatory as no emulator support is present";

            Console.WriteLine(help);
        }
    }
}