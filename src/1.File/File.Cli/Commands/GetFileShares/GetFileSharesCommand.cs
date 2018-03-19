using System;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml.Linq;
using CliUtils;
using Microsoft.Extensions.Configuration;
using StorageAuthenticatorHelper;

namespace File.Cli.Commands.GetFileShares
{
    public class GetFileSharesCommand: ICommand
    {
        public IConfigurationRoot Configuration { get; set; }

        public GetFileSharesCommand(object[] args)
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

        private async Task GetFileSharesAsync(BaseCommandData commandData)
        {
            var factory = new StorageRequestFactory(commandData.StorageAccount, commandData.StorageKey);
            String uri = string.Format("http://{0}.file.core.windows.net?comp=list", commandData.StorageAccount);

            using (var httpRequestMessage = factory.CreateRequest(HttpMethod.Get, uri))
            {
                using (HttpResponseMessage httpResponseMessage = await new HttpClient().SendAsync(httpRequestMessage))
                {
                    if (httpResponseMessage.StatusCode == HttpStatusCode.OK)
                    {
                        String sharesString = await httpResponseMessage.Content.ReadAsStringAsync();
                        XElement x = XElement.Parse(sharesString);
                        foreach (XElement container in x.Element("Shares").Elements("Share"))
                        {
                            Console.WriteLine("Share: name = {0}", container.Element("Name").Value);
                        }
                    }
                }
            }
        }

        private BaseCommandData ParseArgs()
        {

            return new BaseCommandData()
            {
                StorageAccount = Configuration["Account"] ?? Configuration["a"],
                StorageKey = Configuration["Key"] ?? Configuration["k"],
            };
        }
        public void PrintHelp()
        {
            var executable = Assembly.GetExecutingAssembly().GetName().Name;
            var help =
                $@"GetFileShares: List file shares in an storage account

    Usage: {executable} {nameof(GetFileSharesCommand)}  [--a=<account> -k=<key>]

    If no storage account name and key are provided StorageEmulator will be used";

            Console.WriteLine(help);
        }
    }
}
