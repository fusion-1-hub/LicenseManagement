using Fusion1.License.Entity;
using System.ComponentModel;
using System.Xml.Serialization;
using Fusion1.Hardware.Identification;

namespace Fusion1.License.Generator
{
    //public class License : LicenseEntity
    //{

    //    [DisplayName("Connections")]
    //    [Category("License Options")]
    //    [XmlElement("Connections")]
    //    [ShowInLicenseInfo(true, "Connections", ShowInLicenseInfoAttribute.FormatType.Integer)]
    //    public int Connections { get; set; }

    //    public License()
    //    {
    //        this.AppName = "DemoApp";
    //    }

    //    public override LicenseStatus DoExtraValidation(out string validationMsg)
    //    {
    //        LicenseStatus _licStatus = LicenseStatus.UNDEFINED;
    //        validationMsg = string.Empty;

    //        if (this.UUID == HardwareInfo.GenerateUID(this.AppName))
    //        {
    //            _licStatus = LicenseStatus.VALID;
    //        }
    //        else
    //        {
    //            validationMsg = "The license is NOT for this copy!";
    //            _licStatus = LicenseStatus.INVALID;
    //        }
    //        return _licStatus;
    //    }
    //}
}
