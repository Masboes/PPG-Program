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
using System.Media;
using System.IO.Ports;
using System.Globalization;
using MetroFramework.Forms;
using System.Text.RegularExpressions;

namespace PPG
{
    public partial class Monitor : MetroForm
    {
        //Counter in seconds
        public decimal generalCounter = 0;

        //All recieved data from the serialport
        private StreamWriter file;
        private bool running = false;

        //Language
        private string duim1 = "Duim R";
        private string wijsvinger1 = "Wijsvinger R";
        private string middelvinger1 = "Middelvinger R";
        private string ringvinger1 = "Ringvinger R";
        private string duim2 = "Duim L";
        private string wijsvinger2 = "Wijsvinger L";
        private string middelvinger2 = "Middelvinger L";
        private string ringvinger2 = "Ringvinger L";
        private bool english = false;

        //Data processing variables
        private bool closing = false;

        //Variables for the timer
        private int timerTime = 1;
        private bool timerEnabled = false;
        private int timerTotalTime = 1;
        private bool timerPauzed = false;
        private int timerCounter = 0;

        PPG_Data_Handler DataHandler;
        ChartManager chartManager = new ChartManager();

        public Monitor(string comName, string comName2, bool english, bool showingLog, bool calibrationEnabled, decimal timerDelay, bool timerToggle, int timerTime)
        {
            InitializeComponent();
            this.DataHandler = new PPG_Data_Handler(this, calibrationEnabled);
            this.timerEnabled = timerToggle;
            this.timerTotalTime = timerTime;
            this.english = english;
            chartTimer.Interval = (int)(timerDelay);

            PPG1.PortName = comName;
            PPG2.PortName = comName2;

            initialize_chart_manager();
            initialize_English(english);

            initialize_buttons();
            initialize_timer(timerToggle);
            update_ComboBox();
            resize();

            if (showingLog)
            {
                PPGLogger.setOutputTextBox(ref debuggingTextBox);
                PPGLogger.log("PPGLogger to textbox enabled.");
            }

            try
            {
                initialize_com_ports();
            }
            catch (Exception e1)
            {
                MessageBox.Show("Could not connect with glove, please try again. If the problem stays, please reset the device.", "Connection error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                PPGLogger.log("Opening port error " + e1.StackTrace);
                this.Dispose();
                return;
            }
            if (!showingLog) disable_debug_textbox();
        }

        private void initialize_com_ports()
        {
            if (PPG1.PortName != "none")
            {
                PPG1.Open();
            }

            if (PPG2.PortName != "none")
            {
                PPG2.Open();
            }
        }

        private void initialize_chart_manager()
        {
            chartManager.chartingSpace = chartingSpace;
            chartManager.dataHandler = DataHandler;
            chartManager.PPG1Labels = new List<Label>() { Duim1Huidig, Wijs1Huidig, Middel1Huidig, Ring1Huidig
                                                         ,Duim1Hoogst, Wijs1Hoogst, Middel1Hoogst, Ring1Hoogst};
            chartManager.PPG2Labels = new List<Label>() { Duim2Huidig, Wijs2Huidig, Middel2Huidig, Ring2Huidig
                                                         ,Duim2Hoogst, Wijs2Hoogst, Middel2Hoogst, Ring2Hoogst};
            chartManager.PPG2CheckBoxes = new List<CheckBox>() { checkBoxDuim, checkBoxWijsVinger, checkBoxMiddelVinger, checkBoxRingVinger };
            chartManager.PPG1CheckBoxes = new List<CheckBox>() { checkBoxDuim2, checkBoxWijsVinger2, checkBoxMiddelVinger2, checkBoxRingVinger2 };
            chartManager.maxLineNumUpDwn = numericUpDown2;
            chartManager.timerInterval = chartTimer.Interval;
            chartManager.DomainScrollBar = DomeinScrollBar;
            chartManager.initialize_chart();
        }

        private void initialize_buttons()
        {
            button2.BackColor = Color.Blue;
            button3.BackColor = Color.Red;
            button4.BackColor = Color.Black;
            button5.BackColor = Color.Green;

            button8.BackColor = Color.Orange;
            button9.BackColor = Color.Gray;
            button10.BackColor = Color.Pink;
            button11.BackColor = Color.Purple;
        }

        private void initialize_timer(bool timerToggle)
        {
            if (!timerToggle) return;
            PPGLogger.log("Initializing timer1");
            timerTime = timerTotalTime;
            int minutes = (int)Math.Floor(timerTime / 60m);
            int seconds = timerTime - minutes * 60;

            int minutesTime = timerCounter / 60;
            int secondsTime = timerCounter - minutes * 60;

            if (english)
            {
                label18.Text = String.Format("Time: {0,2:D2}:{1,2:D2}  Timer: {2,2:D2}:{3,2:D2}", minutesTime, secondsTime, minutes, seconds);
            }
            else
            {
                label18.Text = String.Format("Tijd: {0,2:D2}:{1,2:D2}  Timer: {2,2:D2}:{3,2:D2}", minutesTime, secondsTime, minutes, seconds);
            }
            timerResetButton.Visible = true;
            timerPauseButton.Visible = true;
        }

        private void initialize_English(bool englishMode)
        {
            if (!englishMode) return;
            PPGLogger.log("Enableing english mode...");
            PersoonLabel1.Text = "Open person:";
            PersoonLabel2.Text = "Add person:";
            PersoonSelecteren.Text = "Select";
            PersoonToevoegen.Text = "Add";
            batteryLbl.Text = "Battery";

            label1.Text = "Thumb";
            label2.Text = "Index finger";
            label3.Text = "Middle finger";
            label4.Text = "Ring finger";

            label9.Text = "Current";
            label10.Text = "Max";
            label19.Text = "Current";
            label20.Text = "Max";

            label15.Text = "Window size:";
            label16.Text = "Limit line:";
            label17.Text = "Transition (%):";
            label18.Text = "Time: 00:00";

            PauzeKnop.Text = "Pause";
            ResetGrafiekKnop.Text = "Clear chart";
            ResetHoogstKnop.Text = "Reset maximums";
            IjkpuntKnop.Text = "Highlight";

            rangeLabel.Text = "Range:";

            chartManager.english();
            PPGLogger.log("English: " + english);
        }

        private void disable_debug_textbox()
        {
            Console.WriteLine("Disabling the panel.");
            panel1.Visible = false;
            tableLayoutPanel1.ColumnStyles.RemoveAt(tableLayoutPanel1.ColumnCount - 1);
        }

        private void resize()
        {
            PPGLogger.log("Resizing...");
            try
            {
                label1.Font = new Font(label1.Font.FontFamily.Name, 12 * this.Size.Height / 768);
                label2.Font = new Font(label2.Font.FontFamily.Name, 12 * this.Size.Height / 768);
                label3.Font = new Font(label3.Font.FontFamily.Name, 12 * this.Size.Height / 768);
                label4.Font = new Font(label4.Font.FontFamily.Name, 12 * this.Size.Height / 768);
                Duim1Huidig.Font = new Font(Duim1Huidig.Font.FontFamily.Name, 12 * this.Size.Height / 768);
                Wijs1Huidig.Font = new Font(Wijs1Huidig.Font.FontFamily.Name, 12 * this.Size.Height / 768);
                Middel1Huidig.Font = new Font(Middel1Huidig.Font.FontFamily.Name, 12 * this.Size.Height / 768);
                Ring1Huidig.Font = new Font(Ring1Huidig.Font.FontFamily.Name, 12 * this.Size.Height / 768);
                label9.Font = new Font(label9.Font.FontFamily.Name, 12 * this.Size.Height / 768);
                label10.Font = new Font(label10.Font.FontFamily.Name, 12 * this.Size.Height / 768);
                Duim1Hoogst.Font = new Font(Duim1Hoogst.Font.FontFamily.Name, 12 * this.Size.Height / 768);
                Wijs1Hoogst.Font = new Font(Wijs1Hoogst.Font.FontFamily.Name, 12 * this.Size.Height / 768);
                Middel1Hoogst.Font = new Font(Middel1Hoogst.Font.FontFamily.Name, 12 * this.Size.Height / 768);
                Ring1Hoogst.Font = new Font(Ring1Hoogst.Font.FontFamily.Name, 12 * this.Size.Height / 768);
                label15.Font = new Font(Ring1Hoogst.Font.FontFamily.Name, 12 * this.Size.Height / 768);
                label19.Font = new Font(Ring1Hoogst.Font.FontFamily.Name, 12 * this.Size.Height / 768);
                label20.Font = new Font(Ring1Hoogst.Font.FontFamily.Name, 12 * this.Size.Height / 768);
                Duim2Huidig.Font = new Font(Ring1Hoogst.Font.FontFamily.Name, 12 * this.Size.Height / 768);
                Wijs2Huidig.Font = new Font(Ring1Hoogst.Font.FontFamily.Name, 12 * this.Size.Height / 768);
                Middel2Huidig.Font = new Font(Ring1Hoogst.Font.FontFamily.Name, 12 * this.Size.Height / 768);
                Ring2Huidig.Font = new Font(Ring1Hoogst.Font.FontFamily.Name, 12 * this.Size.Height / 768);
                Duim2Hoogst.Font = new Font(Ring1Hoogst.Font.FontFamily.Name, 12 * this.Size.Height / 768);
                Wijs2Hoogst.Font = new Font(Ring1Hoogst.Font.FontFamily.Name, 12 * this.Size.Height / 768);
                Middel2Hoogst.Font = new Font(Ring1Hoogst.Font.FontFamily.Name, 12 * this.Size.Height / 768);
                Ring2Hoogst.Font = new Font(Ring1Hoogst.Font.FontFamily.Name, 12 * this.Size.Height / 768);
                ResetGrafiekKnop.Font = new Font(Ring1Hoogst.Font.FontFamily.Name, 12 * this.Size.Height / 768);
                ResetHoogstKnop.Font = new Font(Ring1Hoogst.Font.FontFamily.Name, 12 * this.Size.Height / 768);
                PauzeKnop.Font = new Font(Ring1Hoogst.Font.FontFamily.Name, 12 * this.Size.Height / 768);
                IjkpuntKnop.Font = new Font(Ring1Hoogst.Font.FontFamily.Name, 12 * this.Size.Height / 768);
                button1.Font = new Font(Ring1Hoogst.Font.FontFamily.Name, 12 * this.Size.Height / 768);
                label16.Font = new Font(Ring1Hoogst.Font.FontFamily.Name, 12 * this.Size.Height / 768);
                PPG1BatteryLvl.Font = new Font(Ring1Hoogst.Font.FontFamily.Name, 12 * this.Size.Height / 768);
                PPG2BatteryLvl.Font = new Font(Ring1Hoogst.Font.FontFamily.Name, 12 * this.Size.Height / 768);
                batteryLbl.Font = new Font(Ring1Hoogst.Font.FontFamily.Name, 12 * this.Size.Height / 768);
                rangeLabel.Font = new Font(Ring1Hoogst.Font.FontFamily.Name, 12 * this.Size.Height / 768);
                chartingSpace.Legends["Legend1"].Font = new Font(chartingSpace.Legends["Legend1"].Font.FontFamily, 12 * this.Size.Height / 768);
                chartingSpace.ChartAreas[0].AxisX.LabelAutoFitMaxFontSize = 12 * this.Size.Height / 768;
                chartingSpace.ChartAreas[0].AxisY.LabelAutoFitMaxFontSize = 12 * this.Size.Height / 768;
            }
            catch (Exception e)
            {
                PPGLogger.log("Resizing error: " + e.StackTrace);
            }
        }

        private void chartTimer_tick(object sender, EventArgs e)
        {
            update_battery_values();
            save_line_to_file();
            chartManager.update();
            generalCounter += chartTimer.Interval / 1000m;
        }

        private void clockTimer_tick(object sender, EventArgs e)
        {
            timerCounter++;
            int minutes = timerCounter / 60;
            int seconds = timerCounter - minutes * 60;
            string tijd = "Tijd";
            if (this.english)
            {
                tijd = "Time";
            }

            if (timerEnabled)
            {
                if (!timerPauzed)
                {
                    timerTime--;
                }
                int Tminutes = timerTime / 60;
                int Tseconds = timerTime - Tminutes * 60;
                string timerString = "Timer: ";

                if (timerTime < 0 && !timerPauzed)
                {
                    timerString = "Timer: -";
                    Tminutes = Math.Abs(Tminutes);
                    Tseconds = Math.Abs(Tseconds);
                    chartingSpace.ChartAreas[0].BackColor = Color.FromArgb(255, Color.Red);
                }
                label18.Text = String.Format("{2}: {0,2:D2}:{1,2:D2}", minutes, seconds, tijd) + "  " + String.Format("{2}{0,2:D2}:{1,2:D2}", Tminutes, Tseconds, timerString);
            }
            else
            {
                label18.Text = String.Format("{2}: {0,2:D2}:{1,2:D2}", minutes, seconds, tijd);
            }

        }

        private void update_battery_values()
        {
            PPG1BatteryLvl.Text = batteryValueProcess(DataHandler.voltagePPG1);
            PPG2BatteryLvl.Text = batteryValueProcess(DataHandler.voltagePPG2);
        }

        private void save_line_to_file()
        {
            try
            {
                DataHandler.saveLine(file);
            }
            catch
            {
                PPGLogger.log("File Writing Error in timer1.");
            }
        }

        private void update_ComboBox()
        {
            string[] folders = Directory.GetDirectories("Profiles");
            List<string> formattedFolders = new List<string>();
            foreach (string folder in folders)
            {
                formattedFolders.Add(folder.Substring(9, folder.Length - 9));
            }

            personsBox.DataSource = formattedFolders;
        }

        private void colorButton(Button button, string vinger)
        {
            PPGLogger.log("Changing color: " + vinger);
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                button.BackColor = colorDialog1.Color;
                chartingSpace.Series[vinger].Color = colorDialog1.Color;
            }
        }

        private string batteryValueProcess(Decimal voltage)
        {
            int value = (int)((voltage - 3.2m) * 111);
            if (value > 100)     value = 100;
            else if (value <= 0) value = 1;
            return value.ToString() + "%";
        }

        #region Event Handlers 
        private void button2_Click(object sender, EventArgs e)
        {
            colorButton(button2, duim1);
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            colorButton(button3, wijsvinger1);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            colorButton(button4, middelvinger1);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            colorButton(button5, ringvinger1);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            colorButton(button8, duim2);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            colorButton(button9, wijsvinger2);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            colorButton(button10, middelvinger2);
        }

        private void button11_Click(object sender, EventArgs e)
        {
            colorButton(button11, ringvinger2);
        }

        private void button6_Click_1(object sender, EventArgs e)
        {
            DataHandler.NewDataPPGReciever(textBox1.Text);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                button7.BackColor = colorDialog1.Color;
                chartManager.maxColor = colorDialog1.Color;
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Return)
            {
                button6_Click_1(sender, e);
            }
        }

        private void checkBoxDuim_CheckedChanged(object sender, EventArgs e)
        {
            chartingSpace.Series[duim2].Enabled = checkBoxDuim.Checked;
        }

        private void checkBoxWijsVinger_CheckedChanged(object sender, EventArgs e)
        {
            chartingSpace.Series[wijsvinger2].Enabled = checkBoxWijsVinger.Checked;
        }

        private void checkBoxMiddelVinger_CheckedChanged(object sender, EventArgs e)
        {
            chartingSpace.Series[middelvinger2].Enabled = checkBoxMiddelVinger.Checked;
        }

        private void checkBoxRingVinger_CheckedChanged(object sender, EventArgs e)
        {
            chartingSpace.Series[ringvinger2].Enabled = checkBoxRingVinger.Checked;
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            initialize_timer(true);
            timerPauzed = true;
            timerPauseButton.UseCustomBackColor = timerPauzed;
        }

        private void metroButton1_Click_1(object sender, EventArgs e)
        { 
            timerPauzed = !timerPauzed;
            timerPauseButton.UseCustomBackColor = timerPauzed;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            chartManager.maxLineValue = numericUpDown1.Value;
            chartManager.update();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            chartingSpace.Series[duim1].Enabled = checkBoxDuim2.Checked;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            chartingSpace.Series[wijsvinger1].Enabled = checkBoxWijsVinger2.Checked;
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            chartingSpace.Series[middelvinger1].Enabled = checkBoxMiddelVinger2.Checked;
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            chartingSpace.Series[ringvinger1].Enabled = checkBoxRingVinger2.Checked;
        }

        private void ResetGrafiekKnop_Click(object sender, EventArgs e)
        {
            chartManager.resetChart(this);

        }

        private void ResetHoogstKnop_Click(object sender, EventArgs e)
        {
            chartManager.resetHighestValues();
        }

        private void PersoonSelecteren_Click(object sender, EventArgs e)
        {
            if (personsBox.SelectedItem == null)
            {
                PersoonToevoegen.Focus();
                return;
            }
            PPGLogger.log("Selected person: " + personsBox.SelectedValue.ToString());
            chartingSpace.ChartAreas[0].BackColor = Color.White;
            string currentTime = DateTime.Now.ToString().Replace('/', '-').Replace(':', '꞉');
            string filePath = "Profiles\\" + personsBox.SelectedValue.ToString() + "\\" + currentTime + ".txt";

            PPGLogger.log("Person filepath: " + filePath);
            if (File.Exists(filePath)) MessageBox.Show("File error", "File already exists, please wait a couple of seconds and try again.");           
            else File.CreateText(filePath).Close();

            try { file.Close(); }
            catch { }

            file = File.AppendText(filePath);
            running = true;
            chartTimer.Enabled = true;
            clockTimer.Enabled = true;
            PPGLogger.log("Timer is enabled.");

            //chartingSpace.ChartAreas[0].AxisX.Minimum = 0;
            //chartingSpace.ChartAreas[0].AxisX.Maximum = 5 + DomeinScrollBar.Value;
        }

        private void DomeinScrollBar_Scroll(object sender, ScrollEventArgs e)
        {
            if (running)
            {
                chartManager.domainChange(chartTimer.Enabled);
            }
        }

        private void checkBox1_CheckedChanged_1(object sender, EventArgs e)
        {
            chartManager.maxLineEnabled = checkBox1.Checked;
            chartingSpace.Series["Maximum"].Enabled = checkBox1.Checked;
            button1.Enabled = checkBox1.Checked;
        }

        private void IjkpuntKnop_Click(object sender, EventArgs e)
        {
            PPGLogger.log("Adding IJkpunt...");
            try
            {
                add_eikpoint(DataHandler.chartData1.Count);
            }
            catch
            {
                PPGLogger.log("IJkpunt error");
            }
        }

        private void add_eikpoint(decimal location_in_twenteenth_sec)
        {
            Console.WriteLine("Adding eikpoint....");
            file.WriteLine("Ijkpunt " + location_in_twenteenth_sec.ToString());
            DataHandler.ijkpoints.Add(location_in_twenteenth_sec);
        }

        private void pauzeBtn_Click(object sender, EventArgs e)
        {
            PPGLogger.log("Pausing / resuming...");
            if (running)
            {
                if (PauzeKnop.Text == "Pauze (Shift)" || PauzeKnop.Text == "Pause")
                {
                    PauzeKnop.Text = "Start (Shift)";
                    chartTimer.Enabled = false;
                    clockTimer.Enabled = false;
                    chartManager.soundPlayer.Stop();
                }
                else
                {
                    if (english)
                    {
                        PauzeKnop.Text = "Pause";
                    }
                    else
                    {
                        PauzeKnop.Text = "Pauze (Shift)";
                    }
                    chartTimer.Enabled = true;
                    clockTimer.Enabled = true;
                }
            }
        }

        private void AddPersonBtn_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists("Profiles\\" + PersoonTextBox.Text))
            {
                PPGLogger.log("Creating dir at: Profiles\\" + PersoonTextBox.Text);
                Directory.CreateDirectory("Profiles\\" + PersoonTextBox.Text);
            }
            else
            {
                PPGLogger.log("Invalid profile name: " + PersoonTextBox.Text);
                MessageBox.Show("Invalid profile name. Please try something else.", "Error");
            }

            update_ComboBox();
        }

        private void Form2_Resize(object sender, EventArgs e)
        {
            resize();
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            closing = true;
            try
            {
                chartManager.soundPlayer.Stop();
                file.Close();
            }
            catch { }
        }

        private void PPG_reciever_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            string data = sp.ReadExisting();
            DataHandler.NewDataPPGReciever(data);
        }
        #endregion

        private void Monitor_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.ShiftKey)
            {
                pauzeBtn_Click(sender, e);
            } else if (e.KeyCode == Keys.CapsLock)
            {
                IjkpuntKnop_Click(sender, e);
            }
        }

        private void chartingSpace_DoubleClick(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            Console.WriteLine(chartingSpace.ChartAreas[0].AxisX.PixelPositionToValue(me.X));
            Console.WriteLine(chartingSpace.ChartAreas[0].AxisY.PixelPositionToValue(me.Y));
            add_eikpoint(DataHandler.chartData1.Count - (int)(chartingSpace.ChartAreas[0].AxisX.PixelPositionToValue(me.X) * 10.0));
            if (!chartTimer.Enabled)
            {
                chartManager.draw_Chart();
            }
        }

        private void PPGOne_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            string data = sp.ReadExisting();
            data = data.Replace("#", "&PPG1");
            DataHandler.NewDataPPGReciever(data);
        }

        private void PPGTwo_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            string data = sp.ReadExisting();
            data = data.Replace("#", "&PPG2");
            DataHandler.NewDataPPGReciever(data);
        }
    }
}
