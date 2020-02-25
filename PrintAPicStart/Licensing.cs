using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PrintAPicStart
{


    public static class Licensing
    {
        
        public static string GetUniqueSystemIDs(string text1, string text2, string text3, string text4)
        {
            string requestText =

                "***DO NOT Modify this file, Licnense will not work***" + Environment.NewLine +
               text1 + Environment.NewLine + text2 + Environment.NewLine +
                text3 + Environment.NewLine + text4 + Environment.NewLine
                + "################################################################################"
                + "################################################################################"
                + biosId() + cpuId();

            //    + Licensing.identifier("Win32_DiskDrive", "Signature")
            //    + Licensing.identifier("Win32_DiskDrive", "Model")
            //    + Licensing.identifier("Win32_DiskDrive", "Manufacturer")
            //    + Licensing.identifier("Win32_DiskDrive", "TotalHeads")
            //    + Licensing.identifier("Win32_DiskDrive", "SerialNumber")
            //    + Licensing.identifier("Win32_BaseBoard", "SerialNumber")
            //    + Licensing.identifier("Win32_Processor", "ProcessorId")
            ////+ GetMAC()

            //;
            return requestText;
        }


        public static string GetUniqueSystemIDs()
        {
            string requestText =
                  "################################################################################"
                + "################################################################################"
                + biosId() + cpuId();
            //+ Licensing.identifier("Win32_DiskDrive", "Signature")
            //    + Licensing.identifier("Win32_DiskDrive", "Model")
            //    + Licensing.identifier("Win32_DiskDrive", "Manufacturer")
            //    + Licensing.identifier("Win32_DiskDrive", "TotalHeads")
            //    + Licensing.identifier("Win32_DiskDrive", "SerialNumber")
            //    + Licensing.identifier("Win32_BaseBoard", "SerialNumber")
            //    + Licensing.identifier("Win32_Processor", "ProcessorId")
            ////+ GetMAC()

            //;
            return requestText;
        }


        public static string cpuId()
        {
            //Uses first CPU identifier available in order of preference
            //Don't get all identifiers, as it is very time consuming
            string retVal = identifier("Win32_Processor", "UniqueId");
            if (retVal == "") //If no UniqueID, use ProcessorID
            {
                retVal = identifier("Win32_Processor", "ProcessorId");
                if (retVal == "") //If no ProcessorId, use Name
                {
                    retVal = identifier("Win32_Processor", "Name");
                    if (retVal == "") //If no Name, use Manufacturer
                    {
                        retVal = identifier("Win32_Processor", "Manufacturer");
                    }
                    //Add clock speed for extra security
                    retVal += identifier("Win32_Processor", "MaxClockSpeed");
                }
            }
            return retVal;
        }
        //BIOS Identifier
        public static string biosId()
        {
            return identifier("Win32_BIOS", "Manufacturer")
            + identifier("Win32_BIOS", "SMBIOSBIOSVersion")
            + identifier("Win32_BIOS", "IdentificationCode")
            + identifier("Win32_BIOS", "SerialNumber")
            + identifier("Win32_BIOS", "ReleaseDate")
            + identifier("Win32_BIOS", "Version");
        }


        //Tis function return system identifiers from management class, like hard disk number, CPU no etc.
        public  static string identifier(string wmiClass, string wmiProperty)
        //Return a hardware identifier
        {
            string result = "";
            System.Management.ManagementClass mc = new System.Management.ManagementClass(wmiClass);
            System.Management.ManagementObjectCollection moc = mc.GetInstances();
            foreach (System.Management.ManagementObject mo in moc)
            {
                //Only get the first one
                if (result == "")
                {
                    try
                    {
                        result = mo[wmiProperty].ToString();
                        break;
                    }
                    catch
                    {
                    }
                }
            }
            return result;
        }

        public static bool validateLicense(out string error)
        {
            try
            {
                error = "Some error has occured";
          
            if (File.Exists(Globals.StandardLicPath + "\\License.txt") &&
                File.Exists(Globals.StandardLicPath + "\\SerialKey.txt"))
            {
                //converting public key  back
                string privateKeyString = File.ReadAllText(Globals.StandardLicPath + "\\SerialKey.txt");
                string toBeValidatedLicenseFile = File.ReadAllText(Globals.StandardLicPath + "\\License.txt");


                privateKeyString = Base64Decode(privateKeyString);
                //get a stream from the string
                var sr = new System.IO.StringReader(privateKeyString);
                //we need a deserializer
                var xs = new System.Xml.Serialization.XmlSerializer(typeof(RSAParameters));
                //get the object back from the stream
                var privKey = (RSAParameters)xs.Deserialize(sr);


                var csp = new RSACryptoServiceProvider();
                csp.ImportParameters(privKey);
                //for encryption, always handle bytes...
                var ids = GetUniqueSystemIDs();
                var trimmedIds = Get120CharsOfData(ids);
                //var PlainTextData = System.Text.Encoding.Unicode.GetBytes(trimmedIds);

                var bytesCypherText = Convert.FromBase64String(toBeValidatedLicenseFile);
                //apply pkcs#1.5 padding and encrypt our data 
                var bytesPlainTextData = csp.Decrypt(bytesCypherText, false);
                var decryptedText = System.Text.Encoding.Unicode.GetString(bytesPlainTextData);
                if (string.Equals(decryptedText, trimmedIds))
                {
                    return true;
                }
                else
                {
                    error = "Invalid license, Please contact customer support.";
                    return false;
                }
                
            }
            else
            {
                    error = "License.txt or SerialKey.txt file(s) are not found";
                    return false;
            }
            }
            catch (Exception ex)
            {
                error = "Error occured : "  + ex.Message;
                return false;
            }

        }

        private static string Get120CharsOfData(string plainTextData)
        {
            plainTextData = Reverse(plainTextData);
            if (plainTextData.Length < 120)
            {
                return plainTextData;
            }
            return plainTextData.Substring(0, 120);
        }

        public static string Reverse(string s)
        {
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }

        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }


    }
}
