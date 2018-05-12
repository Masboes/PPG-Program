using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace PPG
{  
    class ChartManager
    {
        public Chart chartingSpace;
        public PPG_Data_Handler dataHandler;

        public SoundPlayer soundPlayer = new SoundPlayer(@"Resources\shake_sound.wav");
        public bool soundPlayerIsPlaying = false;
        public int timerInterval = 1;
        public bool maxLineEnabled = false;
        public bool minRangeEnabled = false;
        public int minRange = 100;
        public Color maxColor = Color.FromArgb(155, 0, 0);

        public List<Label> PPG1Labels = new List<Label>();
        public List<Label> PPG2Labels = new List<Label>();
        public List<CheckBox> PPG1CheckBoxes = new List<CheckBox>();
        public List<CheckBox> PPG2CheckBoxes = new List<CheckBox>();
        public HScrollBar DomainScrollBar;
        private decimal[] PPG1HighestValues = new decimal[8] { 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, };
        private decimal[] PPG2HighestValues = new decimal[8] { 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, };

        private string duim1 = "Duim R";
        private string wijsvinger1 = "Wijsvinger R";
        private string middelvinger1 = "Middelvinger R";
        private string ringvinger1 = "Ringvinger R";
        private string duim2 = "Duim L";
        private string wijsvinger2 = "Wijsvinger L";
        private string middelvinger2 = "Middelvinger L";
        private string ringvinger2 = "Ringvinger L";

        public decimal maxLineValue = 0.0m;
        public NumericUpDown maxLineNumUpDwn;
        private int minimalDomain = 10;

        public bool sineWaveEnabled = false;
        public decimal sineWaveAmplitude = 50.0m;
        public decimal sineWavePeriod = 20.0m;
        public int sineWaveBalance = 100;
        private decimal iterationCounter = 0;

        public void initialize_chart()
        {
            chartingSpace.Series.Remove(chartingSpace.Series["Series1"]);
            chartingSpace.Series.Add(duim2);
            chartingSpace.Series.Add(wijsvinger2);
            chartingSpace.Series.Add(middelvinger2);
            chartingSpace.Series.Add(ringvinger2);
            chartingSpace.Series.Add(duim1);
            chartingSpace.Series.Add(wijsvinger1);
            chartingSpace.Series.Add(middelvinger1);
            chartingSpace.Series.Add(ringvinger1);
            chartingSpace.Series.Add("Maximum");
            chartingSpace.Series[duim2].BorderWidth = 3;
            chartingSpace.Series[wijsvinger2].BorderWidth = 3;
            chartingSpace.Series[middelvinger2].BorderWidth = 3;
            chartingSpace.Series[ringvinger2].BorderWidth = 3;
            chartingSpace.Series[duim1].BorderWidth = 3;
            chartingSpace.Series[wijsvinger1].BorderWidth = 3;
            chartingSpace.Series[middelvinger1].BorderWidth = 3;
            chartingSpace.Series[ringvinger1].BorderWidth = 3;
            chartingSpace.Series["Maximum"].BorderWidth = 3;
            chartingSpace.Series["Maximum"].Color = Color.Black;
            chartingSpace.Series[duim2].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            chartingSpace.Series[wijsvinger2].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            chartingSpace.Series[middelvinger2].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            chartingSpace.Series[ringvinger2].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            chartingSpace.Series[duim1].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            chartingSpace.Series[wijsvinger1].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            chartingSpace.Series[middelvinger1].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            chartingSpace.Series[ringvinger1].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            chartingSpace.Series["Maximum"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            chartingSpace.Series[duim1].Color = Color.Blue;
            chartingSpace.Series[wijsvinger1].Color = Color.Red;
            chartingSpace.Series[middelvinger1].Color = Color.Black;
            chartingSpace.Series[ringvinger1].Color = Color.Green;
            chartingSpace.Series[duim2].Points.AddXY(0, 0);
            chartingSpace.Series[wijsvinger2].Points.AddXY(0, 0);
            chartingSpace.Series[middelvinger2].Points.AddXY(0, 0);
            chartingSpace.Series[ringvinger2].Points.AddXY(0, 0);
            chartingSpace.Series[duim2].Color = Color.Orange;
            chartingSpace.Series[wijsvinger2].Color = Color.Gray;
            chartingSpace.Series[middelvinger2].Color = Color.Pink;
            chartingSpace.Series[ringvinger2].Color = Color.Purple;
            chartingSpace.Series[duim1].Points.AddXY(0, 0);
            chartingSpace.Series[wijsvinger1].Points.AddXY(0, 0);
            chartingSpace.Series[middelvinger1].Points.AddXY(0, 0);
            chartingSpace.Series[ringvinger1].Points.AddXY(0, 0);
            chartingSpace.ChartAreas[0].AxisX.Minimum = 0;
            chartingSpace.ChartAreas[0].AxisX.Maximum = minimalDomain;
            chartingSpace.ChartAreas[0].AxisY.Minimum = 0;
            chartingSpace.ChartAreas[0].AxisY.Maximum = minimalDomain;
            chartingSpace.Series["Maximum"].Enabled = false;

            chartingSpace.Series.Add("Sine");
            chartingSpace.Series["Sine"].BorderWidth = 3;
            chartingSpace.Series["Sine"].Color = Color.Black;
            chartingSpace.Series["Sine"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            chartingSpace.Series["Sine"].Enabled = sineWaveEnabled;

            PPGLogger.log("Prepairing chart...");
        }

        public void update()
        {
            process_current_values_in_update();
            process_highest_values_in_update();
            draw_Chart();
        }

        public void domainChange(bool timerEnabled)
        {
            chartingSpace.ChartAreas[0].AxisX.Minimum = 0;
            chartingSpace.ChartAreas[0].AxisX.Maximum = minimalDomain + DomainScrollBar.Value;
            if (!timerEnabled && dataHandler.chartData1.Count > 0) draw_Chart();
        }

        private void process_highest_values_in_update()
        {
            if (checkIfMaxLineIsReached())
            {
                if (!soundPlayerIsPlaying) soundPlayer.PlayLooping();
                soundPlayerIsPlaying = true;

                applyColoring(dataHandler.Thumb1Value, PPG1CheckBoxes[0]);
                applyColoring(dataHandler.Index1Value, PPG1CheckBoxes[1]);
                applyColoring(dataHandler.Middle1Value, PPG1CheckBoxes[2]);
                applyColoring(dataHandler.Ring1Value, PPG1CheckBoxes[3]);

                applyColoring(dataHandler.Thumb2Value, PPG2CheckBoxes[0]);
                applyColoring(dataHandler.Index2Value, PPG2CheckBoxes[1]);
                applyColoring(dataHandler.Middle2Value, PPG2CheckBoxes[2]);
                applyColoring(dataHandler.Ring2Value, PPG2CheckBoxes[3]);

            } else
            {
                chartingSpace.ChartAreas[0].BackColor = Color.White;
                soundPlayerIsPlaying = false;
                soundPlayer.Stop();
            }

            PPG1HighestValues[0] = setHighest1(dataHandler.Thumb1Value, 4, PPG1HighestValues[0]);
            PPG1HighestValues[1] = setHighest1(dataHandler.Index1Value, 5, PPG1HighestValues[1]);
            PPG1HighestValues[2] = setHighest1(dataHandler.Middle1Value, 6, PPG1HighestValues[2]);
            PPG1HighestValues[3] = setHighest1(dataHandler.Ring1Value, 7, PPG1HighestValues[3]);

            PPG2HighestValues[0] = setHighest2(dataHandler.Thumb2Value, 4, PPG2HighestValues[0]);
            PPG2HighestValues[1] = setHighest2(dataHandler.Index2Value, 5, PPG2HighestValues[1]);
            PPG2HighestValues[2] = setHighest2(dataHandler.Middle2Value, 6, PPG2HighestValues[2]);
            PPG2HighestValues[3] = setHighest2(dataHandler.Ring2Value, 7, PPG2HighestValues[3]);
        }

        private void process_current_values_in_update()
        {
            PPG1Labels[0].Text = dataHandler.Thumb1Value.ToString();
            PPG1Labels[1].Text = dataHandler.Index1Value.ToString();
            PPG1Labels[2].Text = dataHandler.Middle1Value.ToString();
            PPG1Labels[3].Text = dataHandler.Ring1Value.ToString();

            PPG2Labels[0].Text = dataHandler.Thumb2Value.ToString();
            PPG2Labels[1].Text = dataHandler.Index2Value.ToString();
            PPG2Labels[2].Text = dataHandler.Middle2Value.ToString();
            PPG2Labels[3].Text = dataHandler.Ring2Value.ToString();

            dataHandler.chartData1.Insert(0, new decimal[4] { dataHandler.Thumb1Value, dataHandler.Index1Value, dataHandler.Middle1Value, dataHandler.Ring1Value });
            dataHandler.chartData2.Insert(0, new decimal[4] { dataHandler.Thumb2Value, dataHandler.Index2Value, dataHandler.Middle2Value, dataHandler.Ring2Value });
        }

        public void english()
        {
            /*duim1 = "Thumb 1";
            wijsvinger1 = "Index finger 1";
            middelvinger1 = "Middle finger 1";
            ringvinger1 = "Ring finger 1";

            duim2 = "Thumb 2";
            wijsvinger2 = "Index finger 2";
            middelvinger2 = "Middle finger 2";
            ringvinger2 = "Ring finger 2";*/

            chartingSpace.ChartAreas[0].AxisX.Title = "Time (seconds)";
            chartingSpace.ChartAreas[0].AxisY.Title = "Force (gram)";
        }

        private bool checkIfMaxLineIsReached()
        {
            bool duim1 = dataHandler.Thumb1Value > maxLineValue && PPG1CheckBoxes[0].Checked;
            bool wijs1 = dataHandler.Index1Value > maxLineValue && PPG1CheckBoxes[1].Checked;
            bool middel1 = dataHandler.Middle1Value > maxLineValue && PPG1CheckBoxes[2].Checked;
            bool ring1 = dataHandler.Ring1Value > maxLineValue && PPG1CheckBoxes[3].Checked;

            bool duim2 = dataHandler.Thumb2Value > maxLineValue && PPG2CheckBoxes[0].Checked;
            bool wijs2 = dataHandler.Index2Value > maxLineValue && PPG2CheckBoxes[1].Checked;
            bool middel2 = dataHandler.Middle2Value > maxLineValue && PPG2CheckBoxes[2].Checked;
            bool ring2 = dataHandler.Ring2Value > maxLineValue && PPG2CheckBoxes[3].Checked;
            
            return (duim1 || wijs1 || middel1 || ring1 || duim2 || wijs2 || middel2 || ring2) && maxLineEnabled;
        }

        private void applyColoring(decimal fingerValue, CheckBox fingerCheckBox)
        {
            if (fingerValue > maxLineValue && fingerCheckBox.Checked)
            {
                int value = 255 - (int)((Math.Abs((maxLineValue - fingerValue)) / ((maxLineValue + 0.001m) * ((maxLineNumUpDwn.Value + 1) / 100))) * 255);
                if (value < 0) { value = 0; }
                if (value > 255) { value = 255; }
                chartingSpace.ChartAreas[0].BackColor = Color.FromArgb(255 - value, maxColor);
            }
        }

        private decimal setHighest1(decimal HuidigVinger, int index, decimal HoogstVinger)
        {
            if (HuidigVinger > HoogstVinger)
            {
                PPG1Labels[index].Text = HuidigVinger.ToString();
                return HuidigVinger;
            }
            else
            {
                return HoogstVinger;
            }
        }

        private decimal setHighest2(decimal HuidigVinger, int index, decimal HoogstVinger)
        {
            if (HuidigVinger > HoogstVinger)
            {
                Console.WriteLine("REMOVE BEFORE USEAGE");
                PPG2Labels[index].Text = HuidigVinger.ToString();
                //Console.WriteLine(label.Text);
                return HuidigVinger;
            }
            else
            {
                return HoogstVinger;
            }
        }

        public void draw_Chart()
        {
            decimal loopCounter1 = 0m;
            decimal loopCounter2 = 0m;

            foreach (var series in chartingSpace.Series)
            {
                series.Points.Clear();
            }

            foreach (decimal[] plotData1 in dataHandler.chartData1)
            {
                chartingSpace.Series[duim1].Points.AddXY(loopCounter1, plotData1[0]);
                chartingSpace.Series[wijsvinger1].Points.AddXY(loopCounter1, plotData1[1]);
                chartingSpace.Series[middelvinger1].Points.AddXY(loopCounter1, plotData1[2]);
                chartingSpace.Series[ringvinger1].Points.AddXY(loopCounter1, plotData1[3]);

                if(sineWaveEnabled)
                {
                    chartingSpace.Series["Sine"].Points.AddXY(loopCounter1, Math.Sin((double)((2 * Math.PI * (double)(1.0m / sineWavePeriod)) * ((double)loopCounter1 - (double)iterationCounter))) * (double)sineWaveAmplitude + sineWaveBalance);
                }

                if (loopCounter1 > minimalDomain + DomainScrollBar.Value)
                {
                    break;
                }

                loopCounter1 += timerInterval / 1000m;
            }
            iterationCounter += timerInterval / 1000m;

            foreach (decimal[] plotData2 in dataHandler.chartData2)
            {
                chartingSpace.Series[duim2].Points.AddXY(loopCounter2, plotData2[0]);
                chartingSpace.Series[wijsvinger2].Points.AddXY(loopCounter2, plotData2[1]);
                chartingSpace.Series[middelvinger2].Points.AddXY(loopCounter2, plotData2[2]);
                chartingSpace.Series[ringvinger2].Points.AddXY(loopCounter2, plotData2[3]);


                if (loopCounter2 > minimalDomain + DomainScrollBar.Value)
                {
                    break;
                }

                loopCounter2 += timerInterval / 1000m;
            }

            double max = Double.MinValue;

            double leftLimit = chartingSpace.ChartAreas[0].AxisX.Minimum;
            double rightLimit = chartingSpace.ChartAreas[0].AxisX.Maximum;

            foreach(Series serie in chartingSpace.Series)
            {
                foreach (DataPoint dp in serie.Points)
                {
                    if (dp.XValue >= leftLimit && dp.XValue <= rightLimit && !minRangeEnabled)
                    {
                        max = Math.Max(max, dp.YValues[0]);
                        if(minRangeEnabled)
                        {
                            max = Math.Max(max, minRange - 5);
                        }

                        if(sineWaveEnabled)
                        {
                            max = Math.Max(max, ((double)sineWaveBalance + (double)sineWaveAmplitude));
                        }
                    }
                }
            }

            chartingSpace.ChartAreas[0].AxisY.Maximum = max + 5;
            chartingSpace.ChartAreas[0].AxisY.Minimum = 0;

            foreach (decimal ijkpoint in dataHandler.ijkpoints)
            {
                if ( dataHandler.chartData1.Count - ijkpoint < (minimalDomain + DomainScrollBar.Value)* 10.0m)
                {
                    chartingSpace.Series["Series3"].Points.AddXY((dataHandler.chartData1.Count - ijkpoint)/10.0m, 100000);
                    chartingSpace.Series["Series3"].Points.AddXY((dataHandler.chartData1.Count - ijkpoint) / 10.0m, 0);
                    chartingSpace.Series["Series3"].Points.AddXY((dataHandler.chartData1.Count - ijkpoint) / 10.0m, -100000);
                    chartingSpace.Series["Series3"].Points.AddXY((dataHandler.chartData1.Count - ijkpoint) / 10.0m, 100000);
                }
            }
            chartingSpace.Series["Series2"].Points.AddXY(0, 1);
            chartingSpace.Series["Maximum"].Points.AddXY(-1000, maxLineValue);
            chartingSpace.Series["Maximum"].Points.AddXY(1000, maxLineValue);
            chartingSpace.ChartAreas[0].AxisY.LabelStyle.Format = "#";
        }

        public void resetHighestValues()
        {
            PPG1HighestValues = new decimal[8] { 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, };
            PPG2HighestValues = new decimal[8] { 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, };
            foreach (var label in PPG1Labels) {
                label.Text = "0.00";
            }
            foreach (var label in PPG2Labels)
            {
                label.Text = "0.00";
            }
            PPGLogger.log("Highs have been reset");
        }

        public void resetChart(Monitor monitor)
        {
            PPGLogger.log("Resetting chart...");
            resetHighestValues();
            foreach (var series in chartingSpace.Series)
            {
                series.Points.Clear();
            }
            PPGLogger.log("Emptied chart");
            chartingSpace.ChartAreas[0].BackColor = Color.White;
            chartingSpace.Series[duim2].Points.AddXY(0, 0);
            chartingSpace.Series[wijsvinger2].Points.AddXY(0, 0);
            chartingSpace.Series[middelvinger2].Points.AddXY(0, 0);
            chartingSpace.Series[ringvinger2].Points.AddXY(0, 0);
            chartingSpace.Series[ringvinger2].Points.AddXY(1, 0);
            chartingSpace.Series["Series2"].Points.AddXY(5, 5);
            dataHandler.chartData1 = new List<decimal[]>();
            dataHandler.chartData2 = new List<decimal[]>();
            monitor.generalCounter = 0;
            PPGLogger.log("Timer has been reset");
        }

        public void disable_second_glove_lines()
        {
            foreach(var checkBox in PPG2CheckBoxes)
            {
                checkBox.Checked = false;
            }
        }
    }
}
