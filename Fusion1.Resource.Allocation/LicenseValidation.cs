using Fusion1.License.Entity;
using System.Reflection;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Fusion1.License.Validation
{
    public class LicenseValidation
    {
        public static LicenseEntity ParseLicenseFromBASE64String(Type licenseObjType, string licenseString, out LicenseStatus licStatus, out string validationMsg)
        {
            //byte[] _pubKey;
            byte[] _pubKey2;
            //using (MemoryStream _mem = new MemoryStream())
            //{
            //    Assembly.GetExecutingAssembly().GetManifestResourceStream("Fusion1.License.Validation.pub.cer").CopyTo(_mem);
            //    _pubKey = _mem.ToArray();
            //}
             
            _pubKey2 = Encoding.ASCII.GetBytes(Properties.Resources.c);
            
            validationMsg = string.Empty;
            licStatus = LicenseStatus.UNDEFINED;

            if (string.IsNullOrWhiteSpace(licenseString))
            {
                licStatus = LicenseStatus.CRACKED;
                return null;
            }

            string _licXML = string.Empty;
            LicenseEntity _lic = null;

            try
            {
                //Get RSA key from certificate
                X509Certificate2 cert = new X509Certificate2(_pubKey2);
                RSA rsaKey = cert.GetRSAPublicKey();

                XmlDocument xmlDoc = new XmlDocument();

                // Load an XML file into the XmlDocument object.
                xmlDoc.PreserveWhitespace = true;
                xmlDoc.LoadXml(Encoding.UTF8.GetString(Convert.FromBase64String(licenseString)));

                // Verify the signature of the signed XML.            
                if (VerifyXml(xmlDoc, rsaKey))
                {
                    XmlNodeList nodeList = xmlDoc.GetElementsByTagName("Signature");
                    xmlDoc.DocumentElement.RemoveChild(nodeList[0]);

                    _licXML = xmlDoc.OuterXml;

                    //Deserialize license
                    XmlSerializer _serializer = new XmlSerializer(typeof(LicenseEntity), new Type[] { licenseObjType });
                    //XmlSerializer _serializer = new XmlSerializer(typeof(LicenseEntity), new Type[] { _lic.GetType() });
                    using (StringReader _reader = new StringReader(_licXML))
                    {
                        _lic = (LicenseEntity)_serializer.Deserialize(_reader);
                    }

                    licStatus = _lic.DoExtraValidation(out validationMsg);
                }
                else
                {
                    licStatus = LicenseStatus.INVALID;
                }
            }
            catch
            {
                licStatus = LicenseStatus.CRACKED;
            }

            return _lic;
        }

        // Verify the signature of an XML file against an asymmetric 
        // algorithm and return the result.
        private static Boolean VerifyXml(XmlDocument Doc, RSA Key)
        {
            // Check arguments.
            if (Doc == null)
                throw new ArgumentException("Doc");
            if (Key == null)
                throw new ArgumentException("Key");

            // Create a new SignedXml object and pass it
            // the XML document class.
            SignedXml signedXml = new SignedXml(Doc);

            // Find the "Signature" node and create a new
            // XmlNodeList object.
            XmlNodeList nodeList = Doc.GetElementsByTagName("Signature");

            // Throw an exception if no signature was found.
            if (nodeList.Count <= 0)
            {
                throw new CryptographicException("Verification failed: No Signature was found in the document.");
            }

            // This example only supports one signature for
            // the entire XML document.  Throw an exception 
            // if more than one signature was found.
            if (nodeList.Count >= 2)
            {
                throw new CryptographicException("Verification failed: More that one signature was found for the document.");
            }

            // Load the first <signature> node.  
            signedXml.LoadXml((XmlElement)nodeList[0]);

            // Check the signature and return the result.
            return signedXml.CheckSignature(Key);
        }

    }
}