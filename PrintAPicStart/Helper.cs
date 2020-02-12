using IWshRuntimeLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace PrintAPicStart
{
    static class Helper
    {
        public static string FILENAME = "data.xml";
        public static void SerializeText(UserSettings obj)
        {
            XmlSerializer xsSubmit = new XmlSerializer(typeof(UserSettings));
            var xml = "";

            using (var sww = new StringWriter())
            {
                using (XmlWriter writer = XmlWriter.Create(FILENAME))
                {
                    xsSubmit.Serialize(writer, obj);
                    xml = sww.ToString(); // Your XML
                }
            }

        }

        public static UserSettings DeserializeText()
        {
            using (var stream = System.IO.File.OpenRead(FILENAME))
            {
                var serializer = new XmlSerializer(typeof(UserSettings));
                return serializer.Deserialize(stream) as UserSettings;
            }
        }

        public static bool CheckIfStartUpShortcutExists()
        {
            string startUpFolderPath =
             Environment.GetFolderPath(Environment.SpecialFolder.Startup);
            if (System.IO.File.Exists(startUpFolderPath + "\\" +
                Application.ProductName + ".lnk"))
            {
                return true;
            }
            return false;
        }


        public static void UnRegisterAtStartup()
        {
            string startUpFolderPath =
             Environment.GetFolderPath(Environment.SpecialFolder.Startup);
            if (CheckIfStartUpShortcutExists())
            {
                System.IO.File.Delete(startUpFolderPath + "\\" +
                               Application.ProductName + ".lnk");
            }

        }

            public static void RegisterAtStartup()
        {

            WshShell wshShell = new WshShell();
            IWshRuntimeLibrary.IWshShortcut shortcut;
            string startUpFolderPath =
              Environment.GetFolderPath(Environment.SpecialFolder.Startup);

            // Create the shortcut
            shortcut =
              (IWshRuntimeLibrary.IWshShortcut)wshShell.CreateShortcut(
                startUpFolderPath + "\\" +
                Application.ProductName + ".lnk");

            shortcut.TargetPath = Application.ExecutablePath;
            shortcut.WorkingDirectory = Application.StartupPath;
            shortcut.Description = "PrintAPic";
            // shortcut.IconLocation = Application.StartupPath + @"\App.ico";
            shortcut.Save();

        }



    }
}
