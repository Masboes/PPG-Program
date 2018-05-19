using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PPG
{
    class PPG_Data_Handler
    {
        public decimal Thumb1Value = 0.00m;
        public decimal Index1Value = 0.00m;
        public decimal Middle1Value = 0.00m;
        public decimal Ring1Value = 0.00m;
        public decimal Thumb2Value = 0.00m;
        public decimal Index2Value = 0.00m;
        public decimal Middle2Value = 0.00m;
        public decimal Ring2Value = 0.00m;

        public decimal voltagePPG1 = 10.00m;
        public decimal voltagePPG2 = 10.00m;

        private List<decimal> voltageHistoryPPG1 = new List<decimal>();
        private List<decimal> voltageHistoryPPG2 = new List<decimal>();

        public List<Decimal[]> chartData1 = new List<decimal[]>();
        public List<Decimal[]> chartData2 = new List<decimal[]>();
        public List<Decimal> ijkpoints = new List<decimal>();

        private string currentData1 = "";
        private string currentData2 = "";
        private string currentData3 = "";

        private string prepairedLine1 = "";
        private string prepairedLine2 = "";
        private string prepairedLine3 = "";

        private bool calibrationEnabled = true;

        private double calibrationConstant = 97.036;

        private List<List<Decimal>> calibrationConstants = new List<List<decimal>>();

        public bool monitorEnabled = true;

        public Monitor instance;

        private int lineCounter = 0;

        public PPG_Data_Handler(Monitor monitor, bool calibration)
        {
            instance = monitor;
            this.calibrationEnabled = calibration;

            string[] lines = System.IO.File.ReadAllLines("Calibration\\Calibration.txt");
            foreach (string line in lines)
            {
                var values = line.Split(' ');
                calibrationConstants.Add(new List<decimal>());

                for (int i = 0; i < 3; i++)
                {
                    calibrationConstants[calibrationConstants.Count - 1].Add(Decimal.Parse(values[i], new CultureInfo("en-US")));
                }
            }
        }

        public void NewDataGloveOne(string input)
        {
            input = Regex.Replace(input, @"\r\n?|\n", "");
            currentData1 += input;

            if (IsDone(currentData1))
            {
                prepairedLine1 = "";

                if (GetNthIndex(currentData1, '#', 2) == -1)
                {
                    prepairedLine1 = currentData1;
                    currentData1 = "";
                }
                else
                {
                    prepairedLine1 = currentData1.Substring(0, GetNthIndex(currentData1, '#', 2));
                    currentData1 = currentData1.Substring(GetNthIndex(currentData1, '#', 2));
                }
                Console.WriteLine(prepairedLine1);
                prepairedLine1 = prepairedLine1.Substring(1);
                Console.WriteLine(prepairedLine1);
                var prepairedLine1Splitted = prepairedLine1.Split();

                try
                {
                    Thumb1Value = Math.Round(specializedCalibration(Decimal.Parse(prepairedLine1Splitted[0], new CultureInfo("en-US")), 0), 2);
                    Index1Value = Math.Round(specializedCalibration(Decimal.Parse(prepairedLine1Splitted[1], new CultureInfo("en-US")), 1), 2);
                    Middle1Value = Math.Round(specializedCalibration(Decimal.Parse(prepairedLine1Splitted[2], new CultureInfo("en-US")), 2), 2);
                    Ring1Value = Math.Round(specializedCalibration(Decimal.Parse(prepairedLine1Splitted[3], new CultureInfo("en-US")), 3), 2);                    
                    voltageHistoryPPG1.Add(Math.Round(Decimal.Parse(prepairedLine1Splitted[4], new CultureInfo("en-US")), 2));
                    if(voltageHistoryPPG1.Count > 300)
                    {
                        voltageHistoryPPG1.RemoveAt(0);
                    }
                    
                    voltagePPG1 = voltageHistoryPPG1.Average();
                    if (monitorEnabled)
                    {
                        PPGLogger.log("Data collecting finished (PPG1)\nTime: " + instance.generalCounter.ToString() + "\n" + String.Format("{0:0000.0}", Thumb1Value)
                                                         + " " + String.Format("{0:0000.0}", Index1Value)
                                                         + " " + String.Format("{0:0000.0}", Middle1Value)
                                                         + " " + String.Format("{0:0000.0}", Ring1Value)
                                                         + " " + voltagePPG1.ToString());
                    }
                } catch (Exception e)
                {

                    PPGLogger.log("Format error in processing of data from glove 1\n" + e.StackTrace);
                }
            }
        }

        public void NewDataGloveTwo(string input)
        {
            input = Regex.Replace(input, @"\r\n?|\n", "");
            currentData2 += input;

            if (IsDone(currentData2))
            {
                prepairedLine2 = "";

                if (GetNthIndex(currentData2, '#', 2) == -1)
                {
                    prepairedLine2 = currentData2;
                    currentData2 = "";
                }
                else
                {
                    prepairedLine2 = currentData2.Substring(0, GetNthIndex(currentData2, '#', 2));
                    currentData2 = currentData2.Substring(GetNthIndex(currentData2, '#', 2));
                }
                prepairedLine2 = prepairedLine2.Substring(1);
                var prepairedLine2Splitted = prepairedLine2.Split();

                try
                {
                    Thumb2Value = Math.Round(specializedCalibration(Decimal.Parse(prepairedLine2Splitted[0], new CultureInfo("en-US")), 4), 2);
                    Index2Value = Math.Round(specializedCalibration(Decimal.Parse(prepairedLine2Splitted[1], new CultureInfo("en-US")), 5), 2);
                    Middle2Value = Math.Round(specializedCalibration(Decimal.Parse(prepairedLine2Splitted[2], new CultureInfo("en-US")), 6), 2);
                    Ring2Value = Math.Round(specializedCalibration(Decimal.Parse(prepairedLine2Splitted[3], new CultureInfo("en-US")), 7), 2);
                    voltageHistoryPPG2.Add(Math.Round(Decimal.Parse(prepairedLine2Splitted[4], new CultureInfo("en-US")), 2));
                    if (voltageHistoryPPG2.Count > 300)
                    {
                        voltageHistoryPPG2.RemoveAt(0);
                    }
                    voltagePPG2 = voltageHistoryPPG2.Average();

                    if (monitorEnabled)
                    {
                        PPGLogger.log("Data collecting finished (PPG2)\nTime: " + instance.generalCounter.ToString() + "\n" + String.Format("{0:0000.0}", Thumb1Value)
                                                             + " " + String.Format("{0:0000.0}", Index1Value)
                                                             + " " + String.Format("{0:0000.0}", Middle1Value)
                                                             + " " + String.Format("{0:0000.0}", Ring1Value)
                                                             + " " + voltagePPG1.ToString());
                    }

                }
                catch
                {

                    PPGLogger.log("Format error in processing of data from glove 2");
                }
            }
        }

        public void NewDataPPGReciever(string input)
        {
            PPGLogger.log("New data recieved from PPG Reciever: " + input);
            input = Regex.Replace(input, @"\r\n?|\n", "");
            currentData3 += input;

            if (IsDone(currentData3))
            {
                prepairedLine3 = "";

                if (GetNthIndex(currentData3, '&', 2) == -1)
                {
                    prepairedLine3 = currentData3;
                    currentData3 = "";
                }
                else
                {
                    prepairedLine3 = currentData3.Substring(0, GetNthIndex(currentData3, '&', 2));
                    currentData3 = currentData3.Substring(GetNthIndex(currentData3, '&', 2));
                }
                prepairedLine3 = prepairedLine3.Substring(1);

                string id = prepairedLine3.Substring(0, 4);
                string data = prepairedLine3.Substring(4, prepairedLine3.Length - 4);

                if (id == "PPG1")
                {
                    NewDataGloveOne("#" + data);
                }
                else if (id == "PPG2")
                {
                    NewDataGloveTwo("#" + data);
                }
                else
                {
                    return;
                }
            }
        }

        public bool IsDone(string data)
        {
            int last_index = GetNthIndex(data, '.', 5) + 2;
            if (last_index == 1)
            {
                return false;
            }
            return (last_index < data.Length);
        }

        public int GetNthIndex(string s, char t, int n)
        {
            int count = 0;
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] == t)
                {
                    count++;
                    if (count == n)
                    {
                        return i;
                    }
                }
            }
            return -1;
        }

        private decimal calibration(decimal input)
        {
            if (!calibrationEnabled) { return input; }
            var value = ((decimal)calibrationConstant / 100) * (0.7753m * (decimal)Math.Pow((double)input, 2) + 55.418m * input);
            if (value < 3m)
            {
                value = 0;
            }

            return value;
        }

        private decimal specializedCalibration(decimal input, int sensorIndex)
        {
            if (!calibrationEnabled) { return input; }
            decimal c1 = calibrationConstants[sensorIndex][0];
            decimal c2 = calibrationConstants[sensorIndex][1];
            decimal c3 = calibrationConstants[sensorIndex][2];

            var value = c1 * (decimal)Math.Pow((double)input, 2) + c2 * input;
            if(value < 3m)
            {
                value = 0.0m;
            }

            return value;
        }
        
        public void saveLine(StreamWriter file, double waveValue)
        {
            file.WriteLine(String.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9}", (lineCounter / 20.0m).ToString("0.00", System.Globalization.CultureInfo.InvariantCulture)
                                                                   , Thumb1Value.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture)
                                                                   , Index1Value.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture)
                                                                   , Middle1Value.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture)
                                                                   , Ring1Value.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture)
                                                                   , Thumb2Value.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture)
                                                                   , Index2Value.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture)
                                                                   , Middle2Value.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture)
                                                                   , Ring2Value.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture)
                                                                   , waveValue.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture)));
            lineCounter++;
        }
    }
}
