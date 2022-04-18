
using System.Reflection;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Fusion1.License.Entity;

namespace Fusion1.License.Generator
{
    public class LicenseGenerator
    {
        public static string GenerateLicenseBASE64String(LicenseEntity lic)
        {
            byte[] _certPubicKeyData;

            //Read public key from assembly
            Assembly _assembly = Assembly.GetExecutingAssembly();
            using (MemoryStream _mem = new MemoryStream())
            {
                _assembly.GetManifestResourceStream("Fusion1.License.Generator.LicenseSign.pfx").CopyTo(_mem);

                _certPubicKeyData = _mem.ToArray();
            }


            //Serialize license object into XML                    
            XmlDocument _licenseObject = new XmlDocument();
            using (StringWriter _writer = new StringWriter())
            {
                XmlSerializer _serializer = new XmlSerializer(typeof(LicenseEntity), new Type[] { lic.GetType() });

                _serializer.Serialize(_writer, lic);

                _licenseObject.LoadXml(_writer.ToString());
            }

            string _certFilePwd = Properties.Resources.l.Ext(4);

            //string jsonText = JsonConvert.SerializeXmlNode(_licenseObject);

            for (int Pnjvg = 0, SKXtE = 0; Pnjvg < 10; Pnjvg++)
            {
                SKXtE = _certFilePwd[Pnjvg];
                SKXtE = ~SKXtE;
                _certFilePwd = _certFilePwd.Substring(0, Pnjvg) + (char)(SKXtE & 0xFFFF) + _certFilePwd.Substring(Pnjvg + 1);
            }

            //Get RSA key from certificate
            X509Certificate2 cert = new X509Certificate2(_certPubicKeyData, _certFilePwd);

            RSA rsaKey = cert.GetRSAPrivateKey();

            //Sign the XML
            SignXML(_licenseObject, rsaKey);

            //Convert the signed XML into BASE64 string            
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(_licenseObject.OuterXml));
        }

        // Sign an XML file. 
        // This document cannot be verified unless the verifying 
        // code has the key with which it was signed.
        private static void SignXML(XmlDocument xmlDoc, RSA Key)
        {
            // Check arguments.
            if (xmlDoc == null)
                throw new ArgumentException("xmlDoc");
            if (Key == null)
                throw new ArgumentException("Key");

            // Create a SignedXml object.
            SignedXml signedXml = new SignedXml(xmlDoc);

            // Add the key to the SignedXml document.
            signedXml.SigningKey = Key;

            // Create a reference to be signed.
            Reference reference = new Reference();
            reference.Uri = "";

            // Add an enveloped transformation to the reference.
            XmlDsigEnvelopedSignatureTransform env = new XmlDsigEnvelopedSignatureTransform();
            reference.AddTransform(env);

            // Add the reference to the SignedXml object.
            signedXml.AddReference(reference);

            // Compute the signature.
            signedXml.ComputeSignature();

            // Get the XML representation of the signature and save
            // it to an XmlElement object.
            XmlElement xmlDigitalSignature = signedXml.GetXml();

            // Append the element to the XML document.
            xmlDoc.DocumentElement.AppendChild(xmlDoc.ImportNode(xmlDigitalSignature, true));

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
