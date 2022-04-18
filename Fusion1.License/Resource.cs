using Fusion1.Hardware.Identification;
using Fusion1.License.Entity;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Fusion1.License
{
    public class Resource : LicenseEntity
    {

        [DisplayName("Connections")]
        [Category("License Options")]
        [XmlElement("Connections")]
        [ShowInLicenseInfo(true, "Connections", ShowInLicenseInfoAttribute.FormatType.Integer)]
        public int Connections { get; set; }

        public Resource()
        {
            this.AppName = "DemoApp";
        }

        public override LicenseStatus DoExtraValidation(out string validationMsg)
        {
            LicenseStatus _licStatus = LicenseStatus.UNDEFINED;
            validationMsg = string.Empty;

            if (this.UUID == HardwareInfo.GenerateUID(this.AppName))
            {
                _licStatus = LicenseStatus.VALID;
            }
            else
            {
                validationMsg = "The license is NOT for this copy!";
                _licStatus = LicenseStatus.INVALID;
            }
            return _licStatus;
        }
    }
}