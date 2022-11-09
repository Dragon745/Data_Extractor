using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cAlgo.API;
using cAlgo.API.Collections;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;

namespace cAlgo.Robots
{
    [Robot(AccessRights = AccessRights.FullAccess)]
    public class Data_Extractor : Robot
    {
        [Parameter(DefaultValue = "Hello world!")]
        public string Message { get; set; }

         
        [Parameter("Select Folder", DefaultValue = "D:\\Users\\conta\\Documents\\cTrader\\Data")]
        public string Folder { get; set; }

        protected override void OnStart()
        {
            // To learn more about cTrader Automate visit our Help Center:
            // https://help.ctrader.com/ctrader-automate

            Print(Message);
        }

        protected override void OnTick()
        {
            // Handle price updates here
        }

        protected override void OnBar()
        {

            double CandleBody1 = Math.Round((Bars.ClosePrices.Last(1) - Bars.OpenPrices.Last(1)) / Symbol.PipValue, 1);
            double CandleBody2 = Math.Round((Bars.ClosePrices.Last(2) - Bars.OpenPrices.Last(2)) / Symbol.PipValue, 1);
            double CandleBody3 = Math.Round((Bars.ClosePrices.Last(3) - Bars.OpenPrices.Last(3)) / Symbol.PipValue, 1);
            double CandleBody4 = Math.Round((Bars.ClosePrices.Last(4) - Bars.OpenPrices.Last(4)) / Symbol.PipValue, 1);
            double CandleBody5 = Math.Round((Bars.ClosePrices.Last(5) - Bars.OpenPrices.Last(5)) / Symbol.PipValue, 1);
            double CandleBody6 = Math.Round((Bars.ClosePrices.Last(6) - Bars.OpenPrices.Last(6)) / Symbol.PipValue, 1);
            double CandleBody7 = Math.Round((Bars.ClosePrices.Last(7) - Bars.OpenPrices.Last(7)) / Symbol.PipValue, 1);


            //if path not present create it

            if (!System.IO.Directory.Exists(Folder))
            {
                System.IO.Directory.CreateDirectory(Folder);
            }
            //write data to CSV
            string FileName = Folder + "\\" + Symbol.Name + "_" + TimeFrame.ToString() + ".csv";
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(FileName, true))
            {
                file.WriteLine(CandleBody7 + "," + CandleBody6 + "," + CandleBody5 + "," + CandleBody4 + "," + CandleBody3 + "," + CandleBody2 + "," + CandleBody1);
            }

           
        }

        protected override void OnStop()
        {
            // Handle cBot stop here
        }
    }
}