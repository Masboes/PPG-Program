using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.IO.Ports;
using MetroFramework.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace PPG
{
    public partial class MainMenu : MetroForm
    {
        public string version = "7.9";
        public string dateOfRelease = "19-05-2018";

        public bool connected1 = false;
        public bool connected2 = false;

        public bool found = false;
        public ChartColorPalette pallete = ChartColorPalette.BrightPastel;
        Monitor currentForm;
        ColumnStyle rightPanel;
        private List<string> ppgPorts = new List<string>();

        public MainMenu()
        {
            InitializeComponent();
            findPPG();

            if (!Directory.Exists("Profiles"))
            {
                Directory.CreateDirectory("Profiles");
            }

            comboBox1.DataSource = SerialPort.GetPortNames().Distinct().ToList();
            comboBox2.DataSource = SerialPort.GetPortNames().Distinct().ToList();
            try
            {
                comboBox1.SelectedIndex = Array.IndexOf(SerialPort.GetPortNames().Distinct().ToArray(), ppgPorts[0]);
            } catch{} 
            
            monitorBtn.FontSize = MetroFramework.MetroButtonSize.Extreme;
            viewerBtn.FontSize = MetroFramework.MetroButtonSize.Extreme;
            calibrateBtn.FontSize = MetroFramework.MetroButtonSize.Extreme;
            hide_debugging();
        }

        private void hide_debugging()
        {
            this.Width -= 200;
            rightPanel = tableLayoutPanel1.ColumnStyles[tableLayoutPanel1.ColumnCount - 1];
            panel1.Visible = false;
            tableLayoutPanel1.ColumnStyles.RemoveAt(tableLayoutPanel1.ColumnCount - 1);
            tableLayoutPanel1.ColumnCount--;

            Console.WriteLine("Removing column...");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            bool disposed = true;
            try
            {
                disposed = currentForm.IsDisposed;
            }
            catch { }

            if ((connected1 || connected2) && disposed)
            {
                string port1 = "none";
                string port2 = "none";
                

                if (connected1)
                {
                    port1 = comboBox1.SelectedItem.ToString();
                }

                if (connected2)
                {
                    port2 = comboBox2.SelectedItem.ToString();
                }

                Monitor newForm = new Monitor(port1, port2, radioButton2.Checked, checkBox1.Checked, checkBox2.Checked, (decimal)(1000/numericUpDown1.Value), timerToggle.Checked, (int)timerTime.Value);
                currentForm = newForm;
                try
                {
                    newForm.Show();
                }
                catch { }
            }
            else
            {
                if (!connected1 && !connected2)
                {
                    MessageBox.Show("No glove connected.", "Connection error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else if (!disposed)
                {
                    try
                    {
                        currentForm.BringToFront();
                    }
                    catch { }
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            bool disposed = true;
            try
            {
                disposed = currentForm.IsDisposed;
            }
            catch { }

            if ((connected1 || connected2) && disposed)
            {
                string port1 = "none";
                string port2 = "none";


                if (connected1)
                {
                    port1 = comboBox1.SelectedItem.ToString();
                }

                if (connected2)
                {
                    port2 = comboBox2.SelectedItem.ToString();
                }

                CalibrationForm newForm = new CalibrationForm(port1, port2, radioButton2.Checked);
                try
                {
                    newForm.Show();
                }
                catch { }
            }
            else
            {
                if (!connected1 && !connected2)
                {
                    MessageBox.Show("No glove connected.", "Connection error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else if (!disposed)
                {
                    try
                    {
                        currentForm.BringToFront();
                    }
                    catch { }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Viewer newForm = new Viewer(radioButton2.Checked);
            newForm.Show();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            radioButton2.Checked = !radioButton1.Checked;
            languageUpdate(radioButton2.Checked);
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            radioButton1.Checked = !radioButton2.Checked;
            languageUpdate(radioButton2.Checked);
        }

        private void languageUpdate(bool english)
        {
            if (english)
            {
                label1.Text = "1st glove:";
                monitorBtn.Text = "Monitor";
                viewerBtn.Text = "Viewer";
                calibrateBtn.Text = "Calibration";

                if (connected1) { connect1Btn.Text = "Disconnect"; }
                else { connect1Btn.Text = "Connect"; }

                groupBox1.Text = "Settings";
            }
            else
            {
                label1.Text = "1e Handschoen:";
                monitorBtn.Text = "Monitor";
                viewerBtn.Text = "Viewer";
                calibrateBtn.Text = "Kalibreren";

                if (connected1) {connect1Btn.Text = "Ontkoppel";}
                else {connect1Btn.Text = "Verbind";}

                groupBox1.Text = "Instellingen";
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (connect1Btn.Text == "Verbind" || connect1Btn.Text == "Connect" && SerialPort.GetPortNames().ToList<string>().Any<string>())
            {
                if (radioButton2.Checked)
                {
                    connect1Btn.Text = "Disconnect";
                }
                else
                {
                    connect1Btn.Text = "Ontkoppel";
                }

                connected1 = true;
                comboBox1.Enabled = false;
            }
            else
            {
                if (radioButton2.Checked)
                {
                    connect1Btn.Text = "Connect";
                }
                else
                {
                    connect1Btn.Text = "Verbind";
                }

                connected1 = false;
                

                comboBox1.Enabled = true;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (!Directory.Exists("Resources"))
            {
                MessageBox.Show("Resources are missing", "Resources error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.M)
            {
                MessageBox.Show("Application made by Mathijs Boezer\nMeant for use with the PPG (Psychotheraputic Pressure Glove)\nPPG has been developed by Max Schuurmans\nVersion: " + version + "\nRelease date: " + dateOfRelease + " (EU)", "Version information", MessageBoxButtons.OK);
            }
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            if (!Directory.Exists("Resources"))
            {
                MessageBox.Show("Resources are missing", "Resources error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
        }

        private void findPPG()
        {
            Opening opening = new Opening();
            opening.ShowDialog();
            ppgPorts = opening.ppgPorts;

            if (ppgPorts.Any())
            {
                found = true;

                foreach(var port in ppgPorts)
                {
                    if (label3.Text == "Not found")
                    {
                        label3.Text = port;
                    }
                    else
                    {
                        label3.Text += ", " + port;
                    }
                }
            }
        }

        private void metroToggle1_CheckedChanged(object sender, EventArgs e)
        {
            if (metroToggle1.Checked)
            {
                this.Width += 200;
                tableLayoutPanel1.ColumnCount++;
                tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 200F));
                panel1.Visible = true;
                Console.WriteLine("Adding column...");
            } else
            {
                this.Width -= 200;
                tableLayoutPanel1.ColumnStyles.RemoveAt(tableLayoutPanel1.ColumnCount-1);
                tableLayoutPanel1.ColumnCount--;
                panel1.Visible = false;
                Console.WriteLine("Removing column...");
            }           
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            findPPG();
        }

        private void metroToggle2_CheckedChanged(object sender, EventArgs e)
        {
            timerTime.Enabled = timerToggle.Checked;
            label8.Enabled = timerToggle.Checked;
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            if (connect2Btn.Text == "Verbind" || connect2Btn.Text == "Connect" && SerialPort.GetPortNames().ToList<string>().Any<string>())
            {
                if (radioButton2.Checked)
                {
                    connect2Btn.Text = "Disconnect";
                }
                else
                {
                    connect2Btn.Text = "Ontkoppel";
                }

                connected2 = true;
                comboBox2.Enabled = false;
            }
            else
            {
                if (radioButton2.Checked)
                {
                    connect2Btn.Text = "Connect";
                }
                else
                {
                    connect2Btn.Text = "Verbind";
                }

                connected2 = false;


                comboBox2.Enabled = true;
            }
        }
    }
}
