using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        int i = 672;
        protected override void OnStart()
        {
            // To learn more about cTrader Automate visit our Help Center:
            // https://help.ctrader.com/ctrader-automate
            Folder = Folder + "\\" + Symbol.Name + "\\";
            _emaSlow = Indicators.ExponentialMovingAverage(Bars.ClosePrices, 50);
            _emaFast = Indicators.ExponentialMovingAverage(Bars.ClosePrices, 5);
            _rsi = Indicators.RelativeStrengthIndex(Bars.ClosePrices, 10);
            //if file exist empty the file
            string FileName1 = Folder + "X.csv";
            string FileName2 = Folder + "Y.csv";
            if (System.IO.File.Exists(FileName1))
            {
                System.IO.File.WriteAllText(FileName1, string.Empty);
                System.IO.File.WriteAllText(FileName1, "8, 7, 6, 5, 4, 3, 2, 1, RSI, EmaFast, EmaSlow, EmaCross, Trend" + Environment.NewLine);
            }
            if (System.IO.File.Exists(FileName2))
            {
                System.IO.File.WriteAllText(FileName2, string.Empty);
                System.IO.File.WriteAllText(FileName2, "Trend" + Environment.NewLine);
            }

        }

        protected override void OnTick()
        {
            // Handle price updates here
        }

        protected override void OnBar()
        {
            int NoHistoryCandles = HistoryNeeded + 8;
            int DataCollection = CollectYData(CollectXData(NoHistoryCandles));
            //Stop the Robot
            //i--;
            //if (i == 0)
            //{
            //    Stop();
            //}
        }

        
        //Method to Write Data to CSV File
        public void WriteToFile(string FileName, List<double> Data)
        {
            //if path not present create it
            if (!System.IO.Directory.Exists(Folder))
            {
                System.IO.Directory.CreateDirectory(Folder);
            }
            FileName = Folder + FileName + ".csv";
            int i = Data.Count - 1;
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(FileName, true))
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
        }

        
        //Function to Collect X Data
        public int CollectXData(int NoHistoryCandles)
        {
            List<double> Data = new List<double>();
            while (NoHistoryCandles > 8)
            {
                //Add data to array
                Data.Insert(0, Math.Round((Bars.ClosePrices.Last(NoHistoryCandles) - Bars.OpenPrices.Last(NoHistoryCandles)) / Symbol.PipValue, 1));
                NoHistoryCandles--;
            }
            //Add Data from Indicators
            Data.Insert(0, Math.Round(_rsi.Result.Last(9), 2));
            Data.Insert(0, Math.Round((Bars.OpenPrices.Last(9) - _emaFast.Result.Last(9)) / Symbol.PipValue, 1));
            Data.Insert(0, Math.Round((Bars.OpenPrices.Last(9) - _emaSlow.Result.Last(9)) / Symbol.PipValue, 1));
            Data.Insert(0, Math.Round((_emaFast.Result.Last(9) - _emaSlow.Result.Last(9)) / Symbol.PipValue, 1));
            Data.Insert(0, HistoryTrend(NoHistoryCandles, 8));
            WriteToFile("X", Data);
            return NoHistoryCandles;
        }

        //Function to Collect Y Data
        public int CollectYData(int NoHistoryCandles)
        {
            List<double> Data = new List<double>();
            Data.Add(HistoryTrend(1, NoHistoryCandles));
            WriteToFile("Y", Data);

            return 0;
        }

        //Function to Return Trend
        public double HistoryTrend(int MinHistory, int MaxHistory)
        {
            List<double> HistoryMinMaxPrice = new List<double>();
            int i = MaxHistory;
            while (i > (MinHistory - 1))
            {
                HistoryMinMaxPrice.Add(Bars.HighPrices.Last(i));
                HistoryMinMaxPrice.Add(Bars.LowPrices.Last(i));

                i--;
            }
            //get the maximum value in list
            double Max = HistoryMinMaxPrice.Max();
            Max = Math.Round((Max - Bars.OpenPrices.Last(MaxHistory)) / Symbol.PipValue, 1);
            //get the minimum value in list
            double Min = HistoryMinMaxPrice.Min();
            Min = Math.Round((Min - Bars.OpenPrices.Last(MaxHistory)) / Symbol.PipValue, 1);

            double MaxPips = BiggerPips(Min, Max);
            double MaxPercentage = Math.Round((Max / MaxPips), 2);
            double MinPercentage = Math.Round((Min / MaxPips), 2);
            double Trend = MaxPercentage + MinPercentage;
            return Trend;
        }

        public double BiggerPips(double Min, double Max)
        {
            if (Math.Abs(Min) > Math.Abs(Max))
            {
                return Math.Abs(Min);
            }
            else
            {
                return Math.Abs(Max);
            }
        }

        protected override void OnStop()
        {
            // Handle cBot stop here
        }
    }
}