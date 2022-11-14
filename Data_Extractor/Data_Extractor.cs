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
        [Parameter(DefaultValue = "8")]
        public int HistoryNeeded { get; set; }

        [Parameter(DefaultValue = "150")]
        public double StrandardGain { get; set; }

        [Parameter("Select Folder", DefaultValue = "D:\\Users\\conta\\Documents\\cTrader\\Data\\")]
        public string Folder { get; set; }
        private ExponentialMovingAverage _emaSlow;
        private ExponentialMovingAverage _emaFast;
        private RelativeStrengthIndex _rsi;

        protected override void OnStart()
        {
            // To learn more about cTrader Automate visit our Help Center:
            // https://help.ctrader.com/ctrader-automate
            Folder = Folder + "\\" + Symbol.Name + "\\";
            _emaSlow = Indicators.ExponentialMovingAverage(Bars.ClosePrices, 50);
            _emaFast = Indicators.ExponentialMovingAverage(Bars.ClosePrices, 5);
            _rsi = Indicators.RelativeStrengthIndex(Bars.ClosePrices, 10);

        }

        protected override void OnTick()
        {
            // Handle price updates here
        }

        protected override void OnBar()
        {
            int NoHistoryCandles = HistoryNeeded;
            //Create a List of HistoryCandles Height
            List<double> Data = new List<double>();

            
            //Add Data to List {With Name of List Data} Not to Confuse with the Word Data.
            NoHistoryCandles = NoHistoryCandles + 8;
            while (NoHistoryCandles > 8)
            {
                //Add data to array
                Data.Insert(0, Math.Round((Bars.ClosePrices.Last(NoHistoryCandles) - Bars.OpenPrices.Last(NoHistoryCandles)) / Symbol.PipValue, 1));
                NoHistoryCandles--;
            }
            
            
            //create a list with last 8 candles
            List<double> HistoryMinMaxPrice = new List<double>();
            while (NoHistoryCandles < 9 && NoHistoryCandles > 0)
            {
                HistoryMinMaxPrice.Add(Bars.HighPrices.Last(NoHistoryCandles));
                HistoryMinMaxPrice.Add(Bars.LowPrices.Last(NoHistoryCandles));
                NoHistoryCandles--;
            }
            //get the maximum value in list
            double Max = HistoryMinMaxPrice.Max();
            //get the minimum value in list
            double Min = HistoryMinMaxPrice.Min();

            Max = Math.Round((Max - Bars.ClosePrices.Last(9)) / Symbol.PipValue, 1);
            Min = Math.Round((Min - Bars.ClosePrices.Last(9)) / Symbol.PipValue, 1);

            
            //Max is what percentage of StandardGain
            double MaxPercentage = Math.Round(Max / StrandardGain, 2);
            if (MaxPercentage > 1)
            {
                MaxPercentage = 1;
            }

            
            //Min is what percentage of StandardGain
            double MinPercentage = Math.Round(Min / StrandardGain, 2);
            if (MinPercentage < -1)
            {
                MinPercentage = -1;
            }


            //if path not present create it
            if (!System.IO.Directory.Exists(Folder))
            {
                System.IO.Directory.CreateDirectory(Folder);
            }
            ////write data to CSV
            string FileNameX = Folder + "X.csv";
            string FileNameY = Folder + "Y.csv";
            
            //Write to Y
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(FileNameY, true))
            {
                file.WriteLine(MinPercentage + "," + MaxPercentage);
                file.Close();
            }

            //Add Data from Indicators
            Data.Insert(0, Math.Round(_rsi.Result.Last(9), 2));
            Data.Insert(0, Math.Round((Bars.OpenPrices.Last(9) - _emaFast.Result.Last(9)) / Symbol.PipValue, 1));
            Data.Insert(0, Math.Round((Bars.OpenPrices.Last(9) - _emaSlow.Result.Last(9)) / Symbol.PipValue, 1));
            Data.Insert(0, Math.Round((_emaFast.Result.Last(9) - _emaSlow.Result.Last(9)) / Symbol.PipValue, 1));
            //Print("RSI value: " + Math.Round(_rsi.Result.Last(9), 2));
            //Print("EMA Fast value: " + Math.Round((Bars.OpenPrices.Last(9) - _emaFast.Result.Last(9)) / Symbol.PipValue, 1));
            //Print("EMA Slow value: " + Math.Round((Bars.OpenPrices.Last(9) - _emaSlow.Result.Last(9)) / Symbol.PipValue, 1));
            //Print("EMA Fast - EMA Slow value: " + Math.Round((_emaFast.Result.Last(9) - _emaSlow.Result.Last(9)) / Symbol.PipValue, 1));



            // get lenght of list
            int i = Data.Count - 1;

            //Write to X
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(FileNameX, true))
            {
                while (i > -1)
                {
                    if (i == 0)
                    {
                        file.Write(Data[i]);
                    }
                    else
                    {
                        file.Write(Data[i] + ",");
                    }
                    i--;
                }
                file.WriteLine();
                file.Close();
            }

            //Stop the Robot
            //Stop();
        }

        protected override void OnStop()
        {
            // Handle cBot stop here
        }
    }
}