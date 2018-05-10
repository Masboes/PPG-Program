using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PPG
{
    public static class PPGLogger
    {
        static RichTextBox outputTextBox;
        static bool hasTextBox = false;
        private static int maxLines = 500;

        public static void log(string toLog)
        {
            Console.WriteLine(toLog);
            if (hasTextBox)
            {
                try
                {
                    outputTextBox.Invoke(new MethodInvoker(delegate { writeToTextBox(toLog, outputTextBox); }));
                } catch
                {
                    Console.WriteLine("Failed to write to textbox");
                }              
            }
        }

        public static void setOutputTextBox(ref RichTextBox outputTextBox1)
        {
            Console.WriteLine("Textbox assigned");
            outputTextBox = outputTextBox1;
            hasTextBox = true;
        }

        private static void writeToTextBox(string toWrite, RichTextBox textBox)
        {
            textBox.AppendText(toWrite + "\n");
            scroll_textBox_down(textBox);
            if(textBox.Lines.Length > maxLines)
            {
                textBox.Select(0, textBox.GetFirstCharIndexFromLine(textBox.Lines.Length - maxLines));
                textBox.SelectedText = "";
            }
        }

        private static void scroll_textBox_down(RichTextBox debuggingTextBox)
        {
            if (hasTextBox)
            {
                debuggingTextBox.SelectionStart = debuggingTextBox.Text.Length;
                debuggingTextBox.ScrollToCaret();
            }
        }
    }
}
