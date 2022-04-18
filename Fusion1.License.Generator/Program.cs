using System.Text;
using CommandLine;

namespace Fusion1.License.Generator
{
    class Program
    {
        public class Options
        {
            [Option('u', "uuid", Required = true, HelpText = "UUID")]
            public string UUID { get; set; }

            [Option('a', "AppName", Required = true, HelpText = "AppName")]
            public string AppName { get; set; }

            [Option('c', "Connections", Required = true, HelpText = "Connections")]
            public int Connections { get; set; }

            [Option('f', "FileName", Required = false, HelpText = "File Name")]
            public string FileName { get; set; }

            [Option('v', "verbose", Required = false, HelpText = "Set output to verbose messages.")]
            public bool Verbose { get; set; }
        }
         
        static int Main(string[] args)
        {
            var result = Parser.Default.ParseArguments<Options>(args)
                .MapResult((Options o) => {
                    var result= GenerateLicense(o);
                    var filePath = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + o.FileName;
                    if (!String.IsNullOrWhiteSpace(o.FileName))
                    {
                        SaveLicenseFile(o.FileName,result);
                        Console.WriteLine($"License file saved to {filePath}");
                    }
                    else 
                    {
                        Console.OutputEncoding = Encoding.UTF8;
                        Console.WriteLine(result);
                    }
                    return 1;
                },
                e => -1);

            Console.WriteLine("\nPress any key to continue.");
            Console.ReadKey();

            return result;
        }

        private static string GenerateLicense(Options opt)
        {
            var _lic = new Resource();
            _lic.AppName = opt.AppName;
            _lic.UUID = opt.UUID;
            _lic.CreateDateTime = DateTime.Now;
            _lic.Connections = opt.Connections;
            return LicenseGenerator.GenerateLicenseBASE64String(_lic);
        }
        private static void SaveLicenseFile(string fileName, string license)
        {
            File.WriteAllText(fileName, license.Trim(), Encoding.UTF8);
        }

        

    }
}
