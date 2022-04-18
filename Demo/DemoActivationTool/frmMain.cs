using System;
using System.Windows.Forms;
using System.IO;
using System.Security;
using System.Reflection;
using QLicense;
using DemoLicense;
using System.Linq;

namespace DemoActivationTool
{
    public partial class frmMain : Form
    {
        private byte[] _certPubicKeyData;
 

        public frmMain()
        {
            InitializeComponent();

        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            //Read public key from assembly
            Assembly _assembly = Assembly.GetExecutingAssembly();
            using (MemoryStream _mem = new MemoryStream())
            {
                _assembly.GetManifestResourceStream("DemoActivationTool.LicenseSign.pfx").CopyTo(_mem);

                _certPubicKeyData = _mem.ToArray();
            }

            //Initialize the path for the certificate to sign the XML license file
            licSettings.CertificatePrivateKeyData = _certPubicKeyData;
            licSettings.CertificatePassword = Properties.Resources.l.Ext(4);

            //Initialize a new license object
            licSettings.License = new MyLicense(); 
        }

        private void licSettings_OnLicenseGenerated(object sender, QLicense.Windows.Controls.LicenseGeneratedEventArgs e)
        {
            //Event raised when license string is generated. Just show it in the text box
            licString.LicenseString = e.LicenseBASE64String;
        }


        private void btnGenSvrMgmLic_Click(object sender, EventArgs e)
        {
            //Event raised when "Generate License" button is clicked. 
            //Call the core library to generate the license
  
            var result3 = DemoActivationTool.Properties.Resources.l.Ext(4);
            licSettings.CertificatePrivateKeyData = _certPubicKeyData;
            licSettings.CertificatePassword = result3;
            licString.LicenseString = LicenseHandler.GenerateLicenseBASE64String(
                new MyLicense(),
                _certPubicKeyData,
                result3);
        }

        

    }
    public static class Extensions
    {
        public static string Ext(this string str, int n)
        {
            if (String.IsNullOrEmpty(str) || n < 1)
            {
                throw new ArgumentException();
            }
            return string.Concat(Enumerable.Range(0, str.Length / n)
                            .Select(i => char.ConvertFromUtf32(
                                int.Parse(str.Substring(i * n, n), System.Globalization.NumberStyles.HexNumber))));
        }
    }
}
