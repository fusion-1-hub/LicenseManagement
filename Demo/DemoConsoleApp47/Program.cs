using System;
using DemoLicense;
using QLicense;
using System.Reflection;
using System.IO;

namespace DemoConsoleApp47
{
    internal class Program
    {
        static void Main(string[] args)
        {
            byte[] _pubKey;
            string _msg = string.Empty;
            LicenseStatus _status = LicenseStatus.UNDEFINED;
            MyLicense _lic = null;

            try
            {

                using (MemoryStream _mem = new MemoryStream())
                {
                    Assembly.GetExecutingAssembly().GetManifestResourceStream("DemoConsoleApp47.pub.cer").CopyTo(_mem);
                    _pubKey = _mem.ToArray();
                }

                if (File.Exists("license.lic"))
                {
                    _lic = (MyLicense)LicenseHandler.ParseLicenseFromBASE64String(
                    typeof(MyLicense),
                    File.ReadAllText("license.lic"),
                    _pubKey,
                    out _status,
                    out _msg);
                }
                else
                {
                    _status = LicenseStatus.INVALID;
                    _msg = "Your copy of this application is not activated";
                    Console.WriteLine(_msg);
                }
                switch (_status)
                {
                    case LicenseStatus.VALID:

                        //TODO: If license is valid, you can do extra checking here
                        //TODO: E.g., check license expiry date if you have added expiry date property to your license entity
                        //TODO: Also, you can set feature switch here based on the different properties you added to your license entity 

                        Console.WriteLine(_lic.AppName
                                           + Environment.NewLine
                                           + _lic.CreateDateTime);
                        return;

                    default:
                        //for the other status of license file, show the warning message
                        //and also popup the activation form for user to activate your application
                        _msg = "Your license is invalid.";
                        Console.WriteLine(_msg);
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.WriteLine("\npress any key to exit the process...");
            Console.ReadKey();
        }
    }
}
