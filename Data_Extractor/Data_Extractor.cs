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
        [Parameter(DefaultValue = "7")]
        public int NoHistoryCandles { get; set; }
        [Parameter("Select Folder", DefaultValue = "D:\\Users\\conta\\Documents\\cTrader\\Data\\")]
        public string Folder { get; set; }
        private ExponentialMovingAverage _ema;

        protected override void OnStart()
        {
            // To learn more about cTrader Automate visit our Help Center:
            // https://help.ctrader.com/ctrader-automate
            Folder = Folder + "\\" + Symbol.Name + "\\";
            _ema = Indicators.ExponentialMovingAverage(Bars.ClosePrices, 20);
        }

        protected override void OnTick()
        {
            // Handle price updates here
        }

        protected override void OnBar()
        {
            double[] data = new double[NoHistoryCandles];
            int i = 1;
            while (i < (NoHistoryCandles+1))
            {
                //Add data to array
                data[(i-1)]     = Math.Round((Bars.ClosePrices.Last(i) - Bars.OpenPrices.Last(i)) / Symbol.PipValue, 1);
                i++;
            }
            //if path not present create it
            if (!System.IO.Directory.Exists(Folder))
            {
                System.IO.Directory.CreateDirectory(Folder);
            }
            ////write data to CSV
            string FileNameX = Folder + "X.csv";
            string FileNameY = Folder + "Y.csv";
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(FileNameX, true))
            {
                file.WriteLine(data);
                file.Close();
            }
            //using (System.IO.StreamWriter file = new System.IO.StreamWriter(FileNameY, true))
            //{
            //    file.WriteLine(CandleBody1);
            //    file.Close();
            //}
        }

        protected override void OnStop()
        {
            // Handle cBot stop here
        }
    }
}