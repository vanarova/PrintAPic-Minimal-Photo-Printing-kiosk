using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintAPicStart
{
    [Serializable]
   public class UserSettings
    {
         public UserSettings()
        {
            Frequency = "10";
            VinNo = "1";
            SeedMax = "0";
            SeedMin = "0";
            RandomizerScript = "c:\\";
            MatlabScript = "c:\\";
        }

        public string Frequency { get; set; }
        public string VinNo { get; set; }
        public string SeedMin { get; set; }
        public string SeedMax { get; set; }

        public string RandomizerScript { get; set; }

        public string MatlabScript { get; set; }
    }
}
