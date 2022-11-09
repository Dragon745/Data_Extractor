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
            //double CandleHeight = Bars.HighPrices.Last(1) - Bars.LowPrices.Last(1);
            //CandleHeight = CandleHeight / Symbol.PipValue;
            //CandleHeight = Math.Round(CandleHeight, 1);
            //Print(CandleHeight);
            double CandleBody1 = Bars.ClosePrices.Last(1) - Bars.OpenPrices.Last(1);
            CandleBody1 = CandleBody1 / Symbol.PipValue;
            CandleBody1 = Math.Round(CandleBody1, 1);
            Print(CandleBody1);

            double CandleBody2 = Bars.ClosePrices.Last(2) - Bars.OpenPrices.Last(2);
            CandleBody2 = CandleBody2 / Symbol.PipValue;
            CandleBody2 = Math.Round(CandleBody2, 1);
            Print(CandleBody2);

            double CandleBody3 = Bars.ClosePrices.Last(3) - Bars.OpenPrices.Last(3);
            CandleBody3 = CandleBody3 / Symbol.PipValue;
            CandleBody3 = Math.Round(CandleBody3, 1);
            Print(CandleBody3);

            double CandleBody4 = Bars.ClosePrices.Last(4) - Bars.OpenPrices.Last(4);
            CandleBody4 = CandleBody4 / Symbol.PipValue;
            CandleBody4 = Math.Round(CandleBody4, 1);
            Print(CandleBody4);

            double CandleBody5 = Bars.ClosePrices.Last(5) - Bars.OpenPrices.Last(5);
            CandleBody5 = CandleBody5 / Symbol.PipValue;
            CandleBody5 = Math.Round(CandleBody5, 1);
            Print(CandleBody5);

            double CandleBody6 = Bars.ClosePrices.Last(6) - Bars.OpenPrices.Last(6);
            CandleBody6 = CandleBody6 / Symbol.PipValue;
            CandleBody6 = Math.Round(CandleBody6, 1);
            Print(CandleBody6);

            double CandleBody7 = Bars.ClosePrices.Last(7) - Bars.OpenPrices.Last(7);
            CandleBody7 = CandleBody7 / Symbol.PipValue;
            CandleBody7 = Math.Round(CandleBody7, 1);
            Print(CandleBody7);
            //write data to CSV
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