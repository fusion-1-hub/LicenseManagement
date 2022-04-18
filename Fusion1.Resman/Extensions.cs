using System;
using System.IO;
using System.Management;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using static Fusion1.Resman.Resman;


namespace Fusion1.Resman
{
    internal static class Extensions
    {
        internal static Status DoExtraValidationExt(string UUID, string AppName, out string validationMsg)
        {
            Status _recStatus = Status.UNDEFINED;
            validationMsg = string.Empty;

            if (UUID == Resman.GenerateUID(AppName))
            {
                _recStatus = Status.VALID;
            }
            else
            {
                validationMsg = "The resource isn't valid!";
                _recStatus = Status.INVALID;
            }
            return _recStatus;
        }

        internal static string GenerateUIDExt(string appName)
        {
            //Combine the IDs and get bytes
            string _id = string.Concat(appName, GetProcessorId(), GetMotherboardID(), GetDiskVolumeSerialNumber());

            using (SHA256 _SHA256 = SHA256.Create())
            {
                byte[] _checksum = _SHA256.ComputeHash(Encoding.UTF8.GetBytes(_id));

                //Convert checksum into 4 ulong parts and use BASE36 to encode both
                string _part1Id = BASE36.Encode(BitConverter.ToUInt32(_checksum, 0));
                string _part2Id = BASE36.Encode(BitConverter.ToUInt32(_checksum, 4));
                string _part3Id = BASE36.Encode(BitConverter.ToUInt32(_checksum, 8));
                string _part4Id = BASE36.Encode(BitConverter.ToUInt32(_checksum, 12));

                //Concat these 4 part into one string
                return string.Format("{0}-{1}-{2}-{3}", _part1Id, _part2Id, _part3Id, _part4Id);
            }
        }

        internal static Resource ParseResourceString(Type recType, string recString, out Status recStatus, out string validationMsg)
        {
            byte[] _c;
            _c = Encoding.ASCII.GetBytes(Properties.Resources.c);

            validationMsg = string.Empty;
            recStatus = Status.UNDEFINED;

            if (string.IsNullOrWhiteSpace(recString))
            {
                recStatus = Status.BROKEN;
                return null;
            }

            string _licXML = string.Empty;
            Resource _lic = null;

            try
            {
                //Get RSA key from certificate
                X509Certificate2 cert = new X509Certificate2(_c);
                RSA rsaKey = cert.GetRSAPublicKey();

                XmlDocument xmlDoc = new XmlDocument();

                // Load an XML file into the XmlDocument object.
                xmlDoc.PreserveWhitespace = true;
                xmlDoc.LoadXml(Encoding.UTF8.GetString(Convert.FromBase64String(recString)));

                // Verify the signature of the signed XML.            
                if (VerifyXml(xmlDoc, rsaKey))
                {
                    XmlNodeList nodeList = xmlDoc.GetElementsByTagName("Signature");
                    xmlDoc.DocumentElement.RemoveChild(nodeList[0]);

                    _licXML = xmlDoc.OuterXml;

                    //Deserialize license
                    XmlSerializer _serializer = new XmlSerializer(typeof(LicenseEntity), new Type[] { recType });
                    //XmlSerializer _serializer = new XmlSerializer(typeof(LicenseEntity), new Type[] { _lic.GetType() });
                    using (StringReader _reader = new StringReader(_licXML))
                    {
                        _lic = (Resource)_serializer.Deserialize(_reader);
                    }

                    recStatus = _lic.DoExtraValidation(out validationMsg);
                }
                else
                {
                    recStatus = Status.INVALID;
                }
            }
            catch
            {
                recStatus = Status.BROKEN;
            }

            return _lic;
        }

        private static string GetProcessorId()
        {
            try
            {
                ManagementObjectSearcher _mbs = new ManagementObjectSearcher("Select ProcessorId From Win32_processor");
                ManagementObjectCollection _mbsList = _mbs.Get();
                string _id = string.Empty;
                foreach (ManagementObject _mo in _mbsList)
                {
                    _id = _mo["ProcessorId"].ToString();
                    break;
                }

                return _id;

            }
            catch
            {
                return string.Empty;
            }

        }
        private static string GetMotherboardID()
        {

            try
            {
                //"SELECT UUID FROM Win32_ComputerSystemProduct"
                ManagementObjectSearcher _mbs = new ManagementObjectSearcher("Select SerialNumber From Win32_BaseBoard");
                ManagementObjectCollection _mbsList = _mbs.Get();
                string _id = string.Empty;
                foreach (ManagementObject _mo in _mbsList)
                {
                    _id = _mo["SerialNumber"].ToString();
                    break;
                }

                return _id;
            }
            catch
            {
                return string.Empty;
            }

        }
        private static string GetDiskVolumeSerialNumber()
        {
            try
            {
                ManagementObject _disk = new ManagementObject(@"Win32_LogicalDisk.deviceid=""c:""");
                _disk.Get();
                return _disk["VolumeSerialNumber"].ToString();
            }
            catch
            {
                return string.Empty;
            }
        }
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
