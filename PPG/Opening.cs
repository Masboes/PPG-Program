using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PPG
{
    public partial class Opening : Form
    {
        public List<string> ppgPorts = new List<string>();
            

        public Opening()
        {
            InitializeComponent();

            progressBar1.Maximum = SerialPort.GetPortNames().Distinct().ToArray().Length * 4;           
        }

        private void Opening_Shown(object sender, EventArgs e)
        {
            int pointCounter = 0;

            foreach (var port in SerialPort.GetPortNames().Distinct().ToArray())
            {
                Console.WriteLine("Cycling port: " + port);
                richTextBox1.AppendText("\n\nCycling port: " + port + "\n");
                pointCounter = 0;

                try
                {
                    serialPort1.PortName = port;
                    progressBar1.Value++;
                    pointCounter++;

                    Console.WriteLine("Opening...");
                    richTextBox1.AppendText("Opening...\n");

                    serialPort1.Open();
                    progressBar1.Value++;
                    pointCounter++;

                    Console.WriteLine("Opened");
                    richTextBox1.AppendText("Opened\n");
                    System.Threading.Thread.Sleep(120);

                    richTextBox1.AppendText("Recieving data...\n");
                    string data = serialPort1.ReadExisting();
                    progressBar1.Value++;
                    pointCounter++;
                    Console.WriteLine(data);
                    Console.WriteLine("Analysing data...");
                    richTextBox1.AppendText("Analysing data...\n");
                    Console.WriteLine("&: " + data.Contains('&'));
                    richTextBox1.AppendText("Contains identifier: " + data.Contains('&') + "\n");
                    if (data.Contains('&'))
                    {
                        Console.WriteLine("PPG: " + port);
                        richTextBox1.AppendText("PPG found.\n");
                        richTextBox1.AppendText("PPG port: " + port + "\n");
                        ppgPorts.Add(port);
                    }
                    else
                    {
                        Console.WriteLine("Not PPG");
                        richTextBox1.AppendText("Not PPG");
                    }
                    progressBar1.Value++;
                    pointCounter++;
                    serialPort1.Close();
                }
                catch
                {
                    progressBar1.Value += (4 - pointCounter);

                    richTextBox1.AppendText("Failed to handle port. Passing on to the next.\n");
                }
            }
            System.Threading.Thread.Sleep(800);
            this.Close();
        }
    }
}
