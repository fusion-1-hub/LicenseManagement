using System;
using static Fusion1.Resman.Resman;


namespace Fusion1.Resman
{
   
    public static class Resman
    {

        public enum Status
        {
            UNDEFINED = 0,
            VALID = 1,
            INVALID = 2,
            BROKEN = 4
        }

        public static string GenerateUID(string appName)
        {
            return Extensions.GenerateUIDExt(appName);
        }


        public static Resource ParseResourceString(Type recType, string recString, out Status recStatus, out string validationMsg)
        {
            return Extensions.ParseResourceString(recType, recString, out recStatus, out validationMsg);
        }
    }


    public class Resource : LicenseEntity
    {
        public string AppName { get; set; }
        public string UUID { get; set; }
        public int Connections { get; set; }
        public DateTime CreateDateTime { get; set; }
        public override Status DoExtraValidation(out string validationMsg)
        {
            return Extensions.DoExtraValidationExt(this.UUID, this.AppName, out validationMsg);
        }

    }

    public abstract class LicenseEntity
    {
        public string AppName { get; set; }
        public string UUID { get; set; }
        public DateTime CreateDateTime { get; set; }
        public abstract Status DoExtraValidation(out string validationMsg);
    }
}