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
using System.Globalization;
using MetroFramework.Forms;
using System.Diagnostics;

namespace PPG
{
    public partial class Viewer : MetroForm
    {
        private decimal HoogstDuim = 0;
        private decimal HoogstWijs = 0;
        private decimal HoogstMiddel = 0;
        private decimal HoogstRing = 0;
        private decimal HoogstDuim2 = 0;
        private decimal HoogstWijs2 = 0;
        private decimal HoogstMiddel2 = 0;
        private decimal HoogstRing2 = 0;


        //Default language values
        private string duim = "Duim 1";
        private string wijsvinger = "Wijsvinger 1";
        private string middelvinger = "Middelvinger 1";
        private string ringvinger = "Ringvinger 1";
        private string duim2 = "Duim 2";
        private string wijsvinger2 = "Wijsvinger 2";
        private string middelvinger2 = "Middelvinger 2";
        private string ringvinger2 = "Ringvinger 2";

        public Viewer(bool english)
        {
            InitializeComponent();

            //Checking if english
            if (english)
            {
                englishMode();
            }

            initializeChart();

            //Coloring buttons
            button2.BackColor = Color.Blue;
            button3.BackColor = Color.Red;
            button4.BackColor = Color.Black;
            button5.BackColor = Color.Green;

            button9.BackColor = Color.Orange;
            button8.BackColor = Color.Gray;
            button7.BackColor = Color.Pink;
            button6.BackColor = Color.Purple;

            //Updating treeview
            update_treeView();
        }

        private void initializeChart()
        {
            //Prepairing chart
            chart1.Series.Clear();
            chart1.Series.Add("Series2");
            chart1.Series["Series2"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
            chart1.Series["Series2"].Color = Color.Transparent;
            chart1.Series["Series2"].IsVisibleInLegend = false;
            chart1.Series.Add(duim);
            chart1.Series.Add(wijsvinger);
            chart1.Series.Add(middelvinger);
            chart1.Series.Add(ringvinger);
            chart1.Series.Add(duim2);
            chart1.Series.Add(wijsvinger2);
            chart1.Series.Add(middelvinger2);
            chart1.Series.Add(ringvinger2);
            chart1.Series[duim].BorderWidth = 3;
            chart1.Series[wijsvinger].BorderWidth = 3;
            chart1.Series[middelvinger].BorderWidth = 3;
            chart1.Series[ringvinger].BorderWidth = 3;
            chart1.Series[duim].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            chart1.Series[wijsvinger].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            chart1.Series[middelvinger].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            chart1.Series[ringvinger].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            chart1.Series[duim].Points.AddXY(0, 0);
            chart1.Series[wijsvinger].Points.AddXY(0, 0);
            chart1.Series[middelvinger].Points.AddXY(0, 0);
            chart1.Series[ringvinger].Points.AddXY(0, 0);
            chart1.Series[duim2].BorderWidth = 3;
            chart1.Series[wijsvinger2].BorderWidth = 3;
            chart1.Series[middelvinger2].BorderWidth = 3;
            chart1.Series[ringvinger2].BorderWidth = 3;
            chart1.Series[duim2].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            chart1.Series[wijsvinger2].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            chart1.Series[middelvinger2].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            chart1.Series[ringvinger2].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            chart1.Series[duim2].Points.AddXY(0, 0);
            chart1.Series[wijsvinger2].Points.AddXY(0, 0);
            chart1.Series[middelvinger2].Points.AddXY(0, 0);
            chart1.Series[ringvinger2].Points.AddXY(0, 0);
            chart1.ChartAreas[0].AxisX.Minimum = 0;
            chart1.ChartAreas[0].AxisX.Maximum = 30;
            chart1.ChartAreas[0].AxisY.Minimum = 0;
            chart1.ChartAreas[0].AxisY.Maximum = 50;
            chart1.Series[duim].ToolTip = "Kracht:#VALY";
            chart1.Series[wijsvinger].ToolTip = "Kracht:#VALY";
            chart1.Series[middelvinger].ToolTip = "Kracht:#VALY";
            chart1.Series[ringvinger].ToolTip = "Kracht:#VALY";
            chart1.Series[duim2].ToolTip = "Kracht:#VALY";
            chart1.Series[wijsvinger2].ToolTip = "Kracht:#VALY";
            chart1.Series[middelvinger2].ToolTip = "Kracht:#VALY";
            chart1.Series[ringvinger2].ToolTip = "Kracht:#VALY";
            chart1.Series[duim].Color = Color.Blue;
            chart1.Series[wijsvinger].Color = Color.Red;
            chart1.Series[middelvinger].Color = Color.Black;
            chart1.Series[ringvinger].Color = Color.Green;
            chart1.Series[duim2].Color = Color.Orange;
            chart1.Series[wijsvinger2].Color = Color.Gray;
            chart1.Series[middelvinger2].Color = Color.Pink;
            chart1.Series[ringvinger2].Color = Color.Purple;
        }

        private void update_treeView()
        {
            treeView1.Nodes.Clear();
            string formattedFolder;
            string[] folders = Directory.GetDirectories("Profiles");
            List<string> formattedFolders = new List<string>();
            foreach(string folder in folders)
            {
                formattedFolder = folder.Substring(9, folder.Length - 9);
                formattedFolders.Add(formattedFolder);

                var node = treeView1.Nodes.Add(formattedFolder);
                string[] items = Directory.GetFiles("Profiles//" + formattedFolder);
                List<string> itemsFormatted = new List<string>();
                foreach(var item in items)
                {
                    itemsFormatted.Add(item.Substring(9 + formattedFolder.Length + 2, item.Length - (9 + formattedFolder.Length + 6)).Replace('-', '/').Replace('꞉', ':'));
                }

                foreach(string item in sortByDateTime(itemsFormatted.ToArray()))
                {
                    node.Nodes.Add(item);
                }
            }
        }

        private string[] sortByDateTime(string[] items)
        {
            var dates = Array.ConvertAll(items, x => DateTime.Parse(x));
            var dateTimes = dates.OrderByDescending(x => x).ToArray();
            List<string> sortedStrings = new List<string>();
            foreach(var dateTime in dateTimes)
            {
                sortedStrings.Add(dateTime.ToString());
            }
            return sortedStrings.ToArray();
        }

        private void treeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {           
            if(e.Node.Parent != null)
            {
                initializeChart();
                hScrollBar1.Value = hScrollBar1.Minimum;
                button1.Enabled = false;
                Console.WriteLine(e.Node.Text);
                chart1.ChartAreas[0].BackColor = Color.White;

                string path = String.Format(@"Profiles\{0}\{1}.txt", e.Node.Parent.Text, e.Node.Text);
                string[] allLines = { };

                try
                {
                    allLines = File.ReadAllLines(AppDomain.CurrentDomain.BaseDirectory + path.Replace('/', '-').Replace(':', '꞉'));
                } catch 
                {
                    MessageBox.Show("File already opened, please close the file before trying to open it.", "File open error");
                }

                HoogstDuim = 0;
                HoogstWijs = 0;
                HoogstMiddel = 0;
                HoogstRing = 0;
                HoogstDuim2 = 0;
                HoogstWijs2 = 0;
                HoogstMiddel2 = 0;
                HoogstRing2 = 0;

                List<string> ijkPunten = new List<string>();
                decimal i = 0;
                foreach (string line in allLines)
                {
                    if(!line.StartsWith("Ijk"))
                    {
                        string[] splitted = line.Split();
                        if (line != "" && splitted.Length == 8)
                        {
                            chart1.Series[duim].Points.AddXY(i, Decimal.Parse(splitted[0], new CultureInfo("en-US")));
                            chart1.Series[wijsvinger].Points.AddXY(i, Decimal.Parse(splitted[1], new CultureInfo("en-US")));
                            chart1.Series[middelvinger].Points.AddXY(i, Decimal.Parse(splitted[2], new CultureInfo("en-US")));
                            chart1.Series[ringvinger].Points.AddXY(i, Decimal.Parse(splitted[3], new CultureInfo("en-US")));
                            chart1.Series[duim2].Points.AddXY(i, Decimal.Parse(splitted[4], new CultureInfo("en-US")));
                            chart1.Series[wijsvinger2].Points.AddXY(i, Decimal.Parse(splitted[5], new CultureInfo("en-US")));
                            chart1.Series[middelvinger2].Points.AddXY(i, Decimal.Parse(splitted[6], new CultureInfo("en-US")));
                            chart1.Series[ringvinger2].Points.AddXY(i, Decimal.Parse(splitted[7], new CultureInfo("en-US")));

                            HoogstDuim = Math.Max(HoogstDuim, Decimal.Parse(splitted[0], new CultureInfo("en-US")));
                            HoogstWijs = Math.Max(HoogstWijs, Decimal.Parse(splitted[1], new CultureInfo("en-US")));
                            HoogstMiddel = Math.Max(HoogstMiddel, Decimal.Parse(splitted[2], new CultureInfo("en-US")));
                            HoogstRing = Math.Max(HoogstRing, Decimal.Parse(splitted[3], new CultureInfo("en-US")));
                            HoogstDuim2 = Math.Max(HoogstDuim2, Decimal.Parse(splitted[4], new CultureInfo("en-US")));
                            HoogstWijs2 = Math.Max(HoogstWijs2, Decimal.Parse(splitted[5], new CultureInfo("en-US")));
                            HoogstMiddel2 = Math.Max(HoogstMiddel2, Decimal.Parse(splitted[6], new CultureInfo("en-US")));
                            HoogstRing2 = Math.Max(HoogstRing2, Decimal.Parse(splitted[7], new CultureInfo("en-US")));

                            label3.Text = duim + ": " + HoogstDuim.ToString();
                            label4.Text = wijsvinger + ": " + HoogstWijs.ToString();
                            label5.Text = middelvinger + ": " + HoogstMiddel.ToString();
                            label6.Text = ringvinger + ": " + HoogstRing.ToString();
                            label8.Text = duim2 + ": " + HoogstDuim2.ToString();
                            label9.Text = wijsvinger2 + ": " + HoogstWijs2.ToString();
                            label10.Text = middelvinger2 + ": " + HoogstMiddel2.ToString();
                            label11.Text = ringvinger2 + ": " + HoogstRing2.ToString();
                            i += 0.1m;
                        }
                        else if (line != "" && splitted.Length == 4)
                        {
                            chart1.Series[duim].Points.AddXY(i, Decimal.Parse(splitted[0], new CultureInfo("en-US")));
                            chart1.Series[wijsvinger].Points.AddXY(i, Decimal.Parse(splitted[1], new CultureInfo("en-US")));
                            chart1.Series[middelvinger].Points.AddXY(i, Decimal.Parse(splitted[2], new CultureInfo("en-US")));
                            chart1.Series[ringvinger].Points.AddXY(i, Decimal.Parse(splitted[3], new CultureInfo("en-US")));

                            HoogstDuim = Math.Max(HoogstDuim, Decimal.Parse(splitted[0], new CultureInfo("en-US")));
                            HoogstWijs = Math.Max(HoogstWijs, Decimal.Parse(splitted[1], new CultureInfo("en-US")));
                            HoogstMiddel = Math.Max(HoogstMiddel, Decimal.Parse(splitted[2], new CultureInfo("en-US")));
                            HoogstRing = Math.Max(HoogstRing, Decimal.Parse(splitted[3], new CultureInfo("en-US")));
                            label3.Text = duim + ": " + HoogstDuim.ToString();
                            label4.Text = wijsvinger + ": " + HoogstWijs.ToString();
                            label5.Text = middelvinger + ": " + HoogstMiddel.ToString();
                            label6.Text = ringvinger + ": " + HoogstRing.ToString();
                            i += 0.1m;
                        }
                    } else
                    {                       
                        try
                        {
                            double xloc = chart1.Series[duim].Points[chart1.Series[duim].Points.Count - 1].XValue;
                            if (line.Length > 8)
                            {
                                xloc = double.Parse(line.Split(' ')[1])/10.0;
                            }
                            ijkPunten.Add(xloc.ToString());

                            

                            chart1.Series.Add(i.ToString());
                            chart1.Series[i.ToString()].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                            chart1.Series[i.ToString()].BorderWidth = 4;
                            chart1.Series[i.ToString()].IsVisibleInLegend = false;
                            chart1.Series[i.ToString()].Points.AddXY(xloc, -100);
                            chart1.Series[i.ToString()].Points.AddXY(xloc, 100000);
                        } catch
                        {
                            continue;
                        }
                        button1.Enabled = true;
                    }
                }
                listBox1.DataSource = ijkPunten;
                hScrollBar1.Maximum = (int)i * 10;
                chart1.ChartAreas[0].AxisX.Minimum = hScrollBar1.Value / 10 - hScrollBar2.Value;
                chart1.ChartAreas[0].AxisX.Maximum = hScrollBar1.Value / 10 + hScrollBar2.Value + 10;
                chart1.Series["Series2"].Points.AddXY(0, 1);
            }
            hScrollBar1.Enabled = true;
            hScrollBar2.Enabled = true;
            updateYAxisMinMax();
        }

        private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            hScrollBar1_Scroll_Function();
        }

        private void hScrollBar1_Scroll_Function()
        {
            chart1.ChartAreas["ChartArea1"].AxisX.Maximum = hScrollBar1.Value / 10.0 + hScrollBar2.Value + 10;
            chart1.ChartAreas["ChartArea1"].AxisX.Minimum = hScrollBar1.Value / 10.0 - hScrollBar2.Value - 1.0;
            chart1.Series["Series2"].Points.AddXY(chart1.ChartAreas["ChartArea1"].AxisX.Minimum, 1);
            updateYAxisMinMax();
        }

        private void hScrollBar2_Scroll(object sender, ScrollEventArgs e)
        {
            try
            {
                chart1.ChartAreas["ChartArea1"].AxisX.Minimum = hScrollBar1.Value / 10.0 - hScrollBar2.Value;
                chart1.ChartAreas["ChartArea1"].AxisX.Maximum = hScrollBar1.Value / 10.0 + hScrollBar2.Value + 10;
                updateYAxisMinMax();
            } catch{}
        }

        private void updateYAxisMinMax()
        {
            var beginTime = DateTime.Now;
            decimal count = 0;
            try
            {                
                double max = Double.MinValue;
                double min = Double.MaxValue;
             
                double leftLimit = chart1.ChartAreas[0].AxisX.Minimum;
                double rightLimit = chart1.ChartAreas[0].AxisX.Maximum;
                foreach (var serie in chart1.Series)
                {
                    if(serie == chart1.Series[duim] || serie == chart1.Series[middelvinger] || serie == chart1.Series[ringvinger] || serie == chart1.Series[wijsvinger] || serie == chart1.Series["Series2"] || serie == chart1.Series[duim2] || serie == chart1.Series[wijsvinger2] || serie == chart1.Series[middelvinger2] || serie == chart1.Series[ringvinger2])
                    {
                        foreach (var dp in serie.Points.Skip((int)(leftLimit * 10 - 1)).Take((int)(rightLimit - leftLimit)*10))
                        {
                            count++;
                            min = Math.Min(min, dp.YValues.Max());
                            max = Math.Max(max, dp.YValues.Max());
                        }
                    } else
                    {
                        Console.WriteLine("Not the right series.");
                    }
                }
                if(min == double.MaxValue)
                {
                    min = 200;
                }
                chart1.ChartAreas[0].AxisY.Maximum = max * 1.05 + 0.1;
                chart1.ChartAreas[0].AxisY.Minimum = 0;
                Console.WriteLine("Minimum on y-axis: " + min);
                Console.WriteLine("Maximum on y-axis: " + max);

            }
            catch { Console.WriteLine("An error occured while moving the domain."); }
            Console.WriteLine("Total count: " + count.ToString());
        }

        private void Form3_Resize(object sender, EventArgs e)
        {
            try
            {
                label1.Font = new Font(label1.Font.FontFamily.Name, 12 * this.Size.Height / 768);
                label2.Font = new Font(label1.Font.FontFamily.Name, 12 * this.Size.Height / 768);
                button1.Font = new Font(label1.Font.FontFamily.Name, 12 * this.Size.Height / 768);
                chart1.Legends["Legend1"].Font = new Font(chart1.Legends["Legend1"].Font.FontFamily, 12 * this.Size.Height / 768);
                groupBox1.Font = new Font(groupBox1.Font.FontFamily, 12 * this.Size.Height / 768);

                label3.Font = new Font(label1.Font.FontFamily.Name, 12 * this.Size.Height / 768);
                label4.Font = new Font(label1.Font.FontFamily.Name, 12 * this.Size.Height / 768);
                label5.Font = new Font(label1.Font.FontFamily.Name, 12 * this.Size.Height / 768);
                label6.Font = new Font(label1.Font.FontFamily.Name, 12 * this.Size.Height / 768);
                label7.Font = new Font(label1.Font.FontFamily.Name, 12 * this.Size.Height / 768);
                label8.Font = new Font(label1.Font.FontFamily.Name, 12 * this.Size.Height / 768);
                label9.Font = new Font(label1.Font.FontFamily.Name, 12 * this.Size.Height / 768);
                label10.Font = new Font(label1.Font.FontFamily.Name, 12 * this.Size.Height / 768);
                label11.Font = new Font(label1.Font.FontFamily.Name, 12 * this.Size.Height / 768);
                chart1.ChartAreas[0].AxisX.LabelAutoFitMaxFontSize = 12 * this.Size.Height / 768;
                chart1.ChartAreas[0].AxisY.LabelAutoFitMaxFontSize = 12 * this.Size.Height / 768;
            } catch{}
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            treeView1.ItemHeight = 20 * this.Size.Height / 768;
        }

        private void Form3_Shown(object sender, EventArgs e)
        {
            treeView1.ItemHeight = 20 * this.Size.Height / 768;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!hScrollBar2.Enabled)
            {
                return;
            }
            decimal ijkNumber = 0m;
            Console.WriteLine(Decimal.TryParse(listBox1.SelectedItem.ToString(), out ijkNumber));

            if(ijkNumber > 0m)
            {
                hScrollBar1.Value = (int)(ijkNumber * 10m) - 2;
            }
            hScrollBar1_Scroll_Function();
        }

        private void englishMode()
        {
            label1.Text = "Window location:";
            label2.Text = "Window size:";
            label7.Text = "Highest values:";
            groupBox1.Text = "Highlights";
            //label8.Text = "Chart pallete:";
            duim = "Thumb 1";
            wijsvinger = "Index finger 1";
            middelvinger = "Middle finger 1";
            ringvinger = "Ring finger 1";
            duim2 = "Thumb 2";
            wijsvinger2 = "Index finger 2";
            middelvinger2 = "Middle finger 2";
            ringvinger2 = "Ring finger 2";

            chart1.ChartAreas[0].AxisX.Title = "Time (seconds)";
            chart1.ChartAreas[0].AxisY.Title = "Force (gram)";

            label3.Text = "Thumb 1: 0";
            label4.Text = "Index finger 1: 0";
            label5.Text = "Middle finger 1: 0";
            label6.Text = "Ring finger 1: 0";
            label8.Text = "Thumb 2 : 0";
            label9.Text = "Index finger 2: 0";
            label10.Text = "Middle finger 2: 0";
            label11.Text = "Ring finger 2: 0";
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            chart1.Series[duim].Enabled = metroToggle1.Checked;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            chart1.Series[wijsvinger].Enabled = metroToggle2.Checked;
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            chart1.Series[middelvinger].Enabled = metroToggle3.Checked;
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            chart1.Series[ringvinger].Enabled = metroToggle4.Checked;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            colorButton(button2, duim);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            colorButton(button3, wijsvinger);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            colorButton(button4, middelvinger);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            colorButton(button5, ringvinger);
        }

        private void colorButton(Button button, string vinger)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                button.BackColor = colorDialog1.Color;
                chart1.Series[vinger].Color = colorDialog1.Color;
            }
        }

        private void metroToggle5_CheckedChanged(object sender, EventArgs e)
        {
            chart1.Series[duim2].Enabled = metroToggle5.Checked;
        }

        private void metroToggle6_CheckedChanged(object sender, EventArgs e)
        {
            chart1.Series[wijsvinger2].Enabled = metroToggle6.Checked;
        }

        private void metroToggle7_CheckedChanged(object sender, EventArgs e)
        {
            chart1.Series[middelvinger2].Enabled = metroToggle7.Checked;
        }

        private void metroToggle8_CheckedChanged(object sender, EventArgs e)
        {
            chart1.Series[ringvinger2].Enabled = metroToggle8.Checked;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            colorButton(button9, duim2);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            colorButton(button8, middelvinger2);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            colorButton(button7, wijsvinger2);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            colorButton(button6, ringvinger2);
        }
    }
}
