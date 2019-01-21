using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace PicsDirectoryDisplayWin.lib
{

    
    public class Chowkidar
    {
        Timer timer;
        Task ChaltaHuaKaam;
        //private string param;
        private List<Kaam> KaamKiGinti;
        public Chowkidar()
        {
            timer = new Timer(5000);
            timer.AutoReset = true;
            timer.Elapsed += Timer_Elapsed;
            KaamKiGinti = new List<Kaam>();
            timer.Start();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            foreach (Kaam kaam in KaamKiGinti)
            {
                if (ChaltaHuaKaam == null)
                {
                        ChaltaHuaKaam = new Task(() => {
                        kaam.KaamShuruHua = true;
                        kaam.KaamShuru(kaam.Sandesh_FunctionParam1);
                    });
                    ChaltaHuaKaam.Start();
                }
                else
                {
                   if( ChaltaHuaKaam.Status == TaskStatus.Faulted || 
                        ChaltaHuaKaam.Status == TaskStatus.RanToCompletion ||
                        ChaltaHuaKaam.Status == TaskStatus.Canceled)
                    {
                        kaam.KaamKhatamHua = true;
                    }
                    
                }
            }
        }


        public bool IskaamDekhteRahoAurKhatamHonePerSuchitKaro(Action<string> functionName, string param)
        {
            
            KaamKiGinti.Add(new Kaam() {
                KaamKaNaam = functionName.Method.Name,
                KaamShuruHua = false, KaamKhatamHua = false,
                KaamShuru = functionName,
                Sandesh_FunctionParam1 = param
            });

            return true;
        }

        //public bool IskaamDekhteRahoAurKhatamHonePerSuchitKaro(Action<string> functionName, IProgress<ChitraVivaran> param)
        //{

        //    KaamKiGinti.Add(new Kaam()
        //    {
        //        KaamKaNaam = functionName.Method.Name,
        //        KaamShuruHua = false,
        //        KaamKhatamHua = false,
        //        KaamShuru = functionName,
        //        Sandesh_ChitraKhojKiJaankariBhejo = param
        //    });

        //    return true;
        //}

    }


    public class Kaam
    {
        public string KaamKaNaam { get; set; }
        public bool KaamShuruHua { get; set; }

        public Action<string> KaamShuru;
        public bool KaamKhatamHua { get; set; }

        public string Sandesh_FunctionParam1 { get; set; }
        public string Sandesh_FunctionParam2 { get; set; }

        //public IProgress<ChitraVivaran> Sandesh_ChitraKhojKiJaankariBhejo { get; set; }
    }


}
