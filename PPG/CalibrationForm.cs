using MathNet.Numerics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PPG
{
    public partial class CalibrationForm : Form
    {
        private Decimal[] defaultCalibration = { 0.77m, 55.0m, 5.0m };
        private List<List<Decimal>> calibrationConstants = new List<List<decimal>>();
        private PPG_Data_Handler DataHandler = new PPG_Data_Handler(null, false, 0);
        private int selectedFingerIndex = 0;

        private List<Decimal> measurements = new List<decimal>();

        private int measurementCounter = 0;
        private Double[] measureValues = { 0.0, 0.0, 0.0, 50.0, 50.0, 50.0, 100.0, 100.0, 100.0, 200.0, 200.0, 200.0, 300.0, 400.0, 500.0, 600.0, 700.0 , 800.0, 900.0, 1000.0, 1300.0, 1500.0 };

        private bool english = false;

        public CalibrationForm(string port1, string port2, bool english)
        {
            InitializeComponent();
            DataHandler.monitorEnabled = false;

            PPG1.PortName = port1;
            PPG2.PortName = port2;

            this.english = english;

            getCalibrationData();
            initializeComPorts();
            setEnglishMode();
            displayMeasurements();
        }

        private void getCalibrationData()
        {
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

        private void initializeComPorts()
        {
            try
            {
                if (PPG1.PortName != "none") PPG1.Open();
                if (PPG2.PortName != "none") PPG2.Open();
            } catch {
                MessageBox.Show("Could not connect with glove, please try again. If the problem stays, please reset the device.", "Connection error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Dispose();
            }
        }

        private void saveCalibrationData()
        {
            using (System.IO.StreamWriter file =
            new System.IO.StreamWriter("Calibration\\Calibration.txt"))
            {
                foreach (List<decimal> calibrationLine in calibrationConstants)
                {
                    var line = calibrationLine[0].ToString(new CultureInfo("en-US")) + " "
                        + calibrationLine[1].ToString(new CultureInfo("en-US")) + " "
                        + calibrationLine[2].ToString(new CultureInfo("en-US"));

                    file.WriteLine(line);
                }
            }
        }

        private void measurementBtn_click(object sender, EventArgs e)
        {
            decimal value = getCurrentFingerValue();
            measurements.Add(value);
            measurementCounter++;

            if (measurements.Count == measureValues.Length) // finished
            {
                showWeightValue(0.0);
                showStatus(0, 0);
                processData();

                dataRefreshTimer.Enabled = false;
                measurementBtn.Enabled = false;
                backBtn.Enabled = false;
                resetBtn.Enabled = true;
                gloveGroupBox.Enabled = true;
                fingerGroupBox.Enabled = true;
                calibrationChart.Enabled = false;

                startStopBtn.Text = english ? "Start calibration" : "Start kalibratie";
                
                measurementCounter = 0;
            } else
            {
                showWeightValue(measureValues[measurementCounter]);
                showStatus(measurementCounter + 1, measureValues.Length);
            }

            displayMeasurements();
        }

        private void displayMeasurements()
        {
            try
            {
                calibrationChart.Series[0].Points.Clear();

                for (int i = 0; i < measureValues.Length; i++)
                {
                    decimal yVal = (measurements.Count > i) ? measurements[i] : 0;
                    calibrationChart.Series[0].Points.AddXY(yVal, measureValues[i]);
                }
            } catch
            {
                //
            }
        }

        private void displayPolynomial(double[] parameters)
        {
            calibrationChart.Series[1].Points.Clear();

            double counter = 0;
            double prevVal = 0;

            while(prevVal < 1500 && counter < 1000)
            {
                counter += 0.05;
                prevVal = Math.Round(resolvePolynomial(parameters, counter));
                calibrationChart.Series[1].Points.AddXY(counter, prevVal);
            }
        }

        private double resolvePolynomial(double[] parameters, double input)
        {
            return parameters[2] * Math.Pow(input, 2) + parameters[1] * input;
        }

        private void processData()
        {
            var x = measurements.Select(item => Convert.ToDouble(item)).ToArray();
            var y = measureValues;

            var constants = Fit.Polynomial(x, y, 2);

            //foreach(int constant in constants) {
            //    Console.Write(constant + " ");
            //}
            //Console.WriteLine();

            displayPolynomial(constants);

            try
            {
                calibrationConstants[selectedFingerIndex][0] = (decimal)Math.Round(constants[2], 4);
                calibrationConstants[selectedFingerIndex][1] = (decimal)Math.Round(constants[1], 4);
                calibrationConstants[selectedFingerIndex][2] = (decimal)Math.Round(constants[0], 4);

                saveCalibrationData();

                string message = english ? "Calibration succesful!" : "Kalibratie succesvol!";
                MessageBox.Show(message, "Success!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch
            {
                MessageBox.Show("Kalibratie is niet gelukt :(\n Misschien is er iets mis gegaan met de verbinding, u kunt proberen dit te resetten.", "Mislukt", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void showStatus(int x, int outOf)
        {
            string prefix = english ?  "Measurement " : "Meting ";
            progressLabel.Text = prefix + x + "/" + outOf;
        }

        private void showWeightValue(double value)
        {
            string prefix = english ? "Place " : "Plaats ";
            string postfix = english ? " grammes of pressure on the selected finger" : " gram druk op de geselecteerde vinger";

            descLabel1.Text = prefix + Math.Round(value, 0) + postfix;
        }

        private decimal getCurrentFingerValue()
        {
            List<decimal> fingerData = new List<decimal>();
            fingerData.Add(DataHandler.Thumb1Value);
            fingerData.Add(DataHandler.Index1Value);
            fingerData.Add(DataHandler.Middle1Value);
            fingerData.Add(DataHandler.Ring1Value);

            fingerData.Add(DataHandler.Thumb2Value);
            fingerData.Add(DataHandler.Index2Value);
            fingerData.Add(DataHandler.Middle2Value);
            fingerData.Add(DataHandler.Ring2Value);

            return fingerData[selectedFingerIndex];
        }

        private void setEnglishMode()
        {
            if (english)
            {
                measurementBtn.Text = "Make measurement (space)";
                backBtn.Text = "Previous";
                startStopBtn.Text = "Start calibration";
                resetBtn.Text = "Reset calibration";
                gloveGroupBox.Text = "Glove";
                fingerGroupBox.Text = "Finger";

                leftGloveBtn.Text = "Left";
                rightGloveBtn.Text = "Right";
                finger1Btn.Text = "Thumb";
                finger2Btn.Text = "Index Finger";
                finger3Btn.Text = "Middle Finger";
                finger4Btn.Text = "Ring Finger";

                currentValLabel.Text = "Current value: 0,00";
                descLabel3.Text = "and press on 'Make measurement'";
                descLabel2.Text = "until the value below does not change significantly";
                descLabel1.Text = "Place 0 grammes of pressure on the selected finger";
            }
        }

        private int getSelectedFingerIndex()
        {
            int index = 0;
            if (leftGloveBtn.Checked) index += 4;
            if (finger1Btn.Checked) index += 0;
            if (finger2Btn.Checked) index += 1;
            if (finger3Btn.Checked) index += 2;
            if (finger4Btn.Checked) index += 3;

            return index;
        }

        private void startStopBtn_click(object sender, EventArgs e)
        {
            if (startStopBtn.Text == "Start calibration" || startStopBtn.Text == "Start kalibratie")
            {
                measurements = new List<decimal>();
                selectedFingerIndex = getSelectedFingerIndex();
                measurementCounter = 0;
                displayMeasurements();

                showWeightValue(measureValues[measurementCounter]);
                showStatus(measurementCounter + 1, measureValues.Length); // counter starts at 0

                dataRefreshTimer.Enabled = true;
                resetBtn.Enabled = false;
                measurementBtn.Enabled = true;
                backBtn.Enabled = true;
                gloveGroupBox.Enabled = false;
                fingerGroupBox.Enabled = false;
                calibrationChart.Enabled = true;

                measurementBtn.Focus();

                startStopBtn.Text = english ? "Stop calibration" : "Stop kalibratie";
            }
            else
            {
                dataRefreshTimer.Enabled = false;
                resetBtn.Enabled = true;
                measurementBtn.Enabled = false;
                backBtn.Enabled = false;
                gloveGroupBox.Enabled = true;
                fingerGroupBox.Enabled = true;
                calibrationChart.Enabled = false;

                startStopBtn.Text = english ? "Start calibration" : "Start kalibratie";
            }
        }

        private void PPG1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            string data = sp.ReadExisting();
            data = data.Replace("#", "&PPG1");
            DataHandler.NewDataPPGReciever(data);
        }

        private void PPG2_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            string data = sp.ReadExisting();
            data = data.Replace("#", "&PPG2");
            DataHandler.NewDataPPGReciever(data);
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            string message = english ? "Current value: " : "Huidige waarde: ";
            currentValLabel.Text = message + getCurrentFingerValue();
        }

        private void CalibrationForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == ' ') measurementBtn_click(sender, e);
        }

        private void resetBtn_click(object sender, EventArgs e)
        {
            string message = english ? "Are you sure you want to reset the calibration for this finger?" : "Weet u zeker dat u de kalibratie voor deze vinger wilt resetten?";
            DialogResult d = MessageBox.Show(message, "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (d == DialogResult.Yes)
            {
                selectedFingerIndex = getSelectedFingerIndex();

                calibrationConstants[selectedFingerIndex][0] = defaultCalibration[0];
                calibrationConstants[selectedFingerIndex][1] = defaultCalibration[1];
                calibrationConstants[selectedFingerIndex][2] = defaultCalibration[2];

                saveCalibrationData();
            }
        }

        private void backBtn_click(object sender, EventArgs e)
        {
            measurementBtn.Focus();
            if (measurements.Count > 0) {
                measurements.RemoveAt(measurements.Count - 1);

                measurementCounter--;
                showWeightValue(measureValues[measurementCounter]);
                showStatus(measurementCounter + 1, measureValues.Length);

                displayMeasurements();
            }
        }
    }
}
