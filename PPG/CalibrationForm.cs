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
        private List<List<Decimal>> calibrationConstants = new List<List<decimal>>();
        private PPG_Data_Handler DataHandler = new PPG_Data_Handler(null, false, 0);
        private int selectedFingerIndex = 0;

        private List<Decimal> mesurements = new List<decimal>();

        private int measurementCounter = 0;
        private Double[] measureValues = { 50.0, 50.0, 50.0, 100.0, 100.0, 100.0, 200.0, 200.0, 200.0, 400.0, 500.0, 600.0, 700.0 , 800.0, 900.0, 1000.0, 1300.0, 1500.0 };

        private bool englishMode = false;

        public CalibrationForm(string port1, string port2, bool english)
        {
            InitializeComponent();
            DataHandler.monitorEnabled = false;
            getCalibrationData();

            PPG1.PortName = port1;
            PPG2.PortName = port2;

            initialize_com_ports();

            englishMode = english;
            if (english)
            {
                button2.Text = "Make measurement (space)";
                button4.Text = "Previous";
                button1.Text = "Start calibration";
                button3.Text = "Reset calibration";
                groupBox1.Text = "Glove";
                groupBox2.Text = "Finger";

                radioButton1.Text = "Left";
                radioButton2.Text = "Right";
                radioButton3.Text = "Thumb";
                radioButton4.Text = "Index Finger";
                radioButton5.Text = "Middle Finger";
                radioButton6.Text = "Ring Finger";

                label1.Text = "Current value: 0,00";
                label2.Text = "and press on 'Make measurement'";
                label3.Text = "until the value below does not change significantly";
                label4.Text = "Place 0 grammes of pressure on the selected finger";
            }
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

        private void initialize_com_ports()
        {
            try
            {
                if (PPG1.PortName != "none")
                {
                    PPG1.Open();
                }

                if (PPG2.PortName != "none")
                {
                    PPG2.Open();
                }
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

        private void button2_Click(object sender, EventArgs e)
        {
            decimal value = getCurrentFingerValue();
            mesurements.Add(value);

            measurementCounter++;
            if (mesurements.Count == measureValues.Length)
            {
                showWeightValue(0.0);
                showStatus(0, 0);
                processData();

                timer1.Enabled = false;
                button2.Enabled = false;
                button4.Enabled = false;
                button1.Enabled = true;
                button3.Enabled = true;
                groupBox1.Enabled = true;
                groupBox2.Enabled = true;

                measurementCounter = 0;
            } else
            {
                showWeightValue(measureValues[measurementCounter]);
                showStatus(measurementCounter + 1, measureValues.Length);
            }
        }

        private void processData()
        {
            var x = mesurements.Select(item => Convert.ToDouble(item)).ToArray();
            var y = measureValues;

            var constants = Fit.Polynomial(x, y, 2);

            foreach(var i in constants)
            {
                Console.WriteLine(i);
            }

            try
            {
                calibrationConstants[selectedFingerIndex][0] = (decimal)Math.Round(constants[2], 4);
                calibrationConstants[selectedFingerIndex][1] = (decimal)Math.Round(constants[1], 4);
                calibrationConstants[selectedFingerIndex][2] = (decimal)Math.Round(constants[0], 4);

                saveCalibrationData();

                string message = "Kalibratie succesvol!";
                if (englishMode)
                {
                    message = "Calibration succesful!";
                }
                MessageBox.Show(message, "Success!", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch
            {
                MessageBox.Show("Kalibratie is niet gelukt :(\n Misschien is er iets mis gegaan met de verbinding, u kunt proberen dit te resetten.", "Mislukt", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void showStatus(int x, int outOf)
        {
            string prefix = "Meting ";
            if (englishMode)
            {
                prefix = "Measurement ";
            }
            label5.Text = prefix + x + "/" + outOf;
        }

        private void showWeightValue(double value)
        {
            string prefix = "Plaats ";
            if (englishMode)
            {
                prefix = "Place ";
            }

            string postfix = " gram druk op de geselecteerde vinger";
            if (englishMode)
            {
                postfix = " grammes of pressure on the selected finger";
            }


            label4.Text = prefix + Math.Round(value, 0) + postfix;
        }

        private void button1_Click(object sender, EventArgs e)
        { 
            mesurements = new List<decimal>();
            selectedFingerIndex = 0;
            if (radioButton1.Checked) selectedFingerIndex += 4;

            if (radioButton3.Checked) selectedFingerIndex += 0;
            if (radioButton4.Checked) selectedFingerIndex += 1;
            if (radioButton5.Checked) selectedFingerIndex += 2;
            if (radioButton6.Checked) selectedFingerIndex += 3;

            timer1.Enabled = true;
            button1.Enabled = false;
            button3.Enabled = false;
            button2.Enabled = true;
            button4.Enabled = true;
            showWeightValue(measureValues[measurementCounter]);
            showStatus(measurementCounter + 1, measureValues.Length);

            groupBox1.Enabled = false;
            groupBox2.Enabled = false;
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

        private void timer1_Tick(object sender, EventArgs e)
        {
            string message = "Huidige waarde: ";
            if (englishMode)
            {
                message = "Current Value: ";
            }

            label1.Text = message + getCurrentFingerValue();
        }

        private void CalibrationForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == ' ')
            {
                button2_Click(sender, e);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string message = "Weet u zeker dat u de kalibratie voor deze vinger wilt resetten?";
            if (englishMode)
            {
                message = "Are you sure you want to reset the calibration for this finger?";
            }
            DialogResult d = MessageBox.Show(message, "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (d == DialogResult.Yes)
            {
                selectedFingerIndex = 0;
                if (radioButton2.Checked) selectedFingerIndex += 4;

                if (radioButton3.Checked) selectedFingerIndex += 0;
                if (radioButton4.Checked) selectedFingerIndex += 1;
                if (radioButton5.Checked) selectedFingerIndex += 2;
                if (radioButton6.Checked) selectedFingerIndex += 3;

                calibrationConstants[selectedFingerIndex][0] = 0.77m;
                calibrationConstants[selectedFingerIndex][1] = 55.0m;
                calibrationConstants[selectedFingerIndex][2] = 5.0m;

                saveCalibrationData();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            button2.Focus();
            if (mesurements.Count > 0) {
                mesurements.RemoveAt(mesurements.Count - 1);

                measurementCounter--;
                showWeightValue(measureValues[measurementCounter]);
                showStatus(measurementCounter + 1, measureValues.Length);
            }
        }
    }
}
