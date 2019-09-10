using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintAPicStart
{
    public static class Globals
    {
        public static string StandardLicPath => System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\PrintAPic";
    }
}
