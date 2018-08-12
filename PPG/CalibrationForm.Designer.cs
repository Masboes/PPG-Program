namespace PPG
{
    partial class CalibrationForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.DataPoint dataPoint2 = new System.Windows.Forms.DataVisualization.Charting.DataPoint(0D, 0D);
            System.Windows.Forms.DataVisualization.Charting.Series series4 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CalibrationForm));
            this.leftGloveBtn = new System.Windows.Forms.RadioButton();
            this.rightGloveBtn = new System.Windows.Forms.RadioButton();
            this.gloveGroupBox = new System.Windows.Forms.GroupBox();
            this.fingerGroupBox = new System.Windows.Forms.GroupBox();
            this.finger4Btn = new System.Windows.Forms.RadioButton();
            this.finger3Btn = new System.Windows.Forms.RadioButton();
            this.finger1Btn = new System.Windows.Forms.RadioButton();
            this.finger2Btn = new System.Windows.Forms.RadioButton();
            this.startStopBtn = new System.Windows.Forms.Button();
            this.currentValLabel = new System.Windows.Forms.Label();
            this.descLabel3 = new System.Windows.Forms.Label();
            this.descLabel2 = new System.Windows.Forms.Label();
            this.descLabel1 = new System.Windows.Forms.Label();
            this.measurementBtn = new System.Windows.Forms.Button();
            this.PPG1 = new System.IO.Ports.SerialPort(this.components);
            this.PPG2 = new System.IO.Ports.SerialPort(this.components);
            this.dataRefreshTimer = new System.Windows.Forms.Timer(this.components);
            this.progressLabel = new System.Windows.Forms.Label();
            this.resetBtn = new System.Windows.Forms.Button();
            this.backBtn = new System.Windows.Forms.Button();
            this.calibrationChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.gloveGroupBox.SuspendLayout();
            this.fingerGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.calibrationChart)).BeginInit();
            this.SuspendLayout();
            // 
            // leftGloveBtn
            // 
            this.leftGloveBtn.AutoSize = true;
            this.leftGloveBtn.Checked = true;
            this.leftGloveBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.leftGloveBtn.Location = new System.Drawing.Point(6, 19);
            this.leftGloveBtn.Name = "leftGloveBtn";
            this.leftGloveBtn.Size = new System.Drawing.Size(64, 24);
            this.leftGloveBtn.TabIndex = 0;
            this.leftGloveBtn.TabStop = true;
            this.leftGloveBtn.Text = "Links";
            this.leftGloveBtn.UseVisualStyleBackColor = true;
            // 
            // rightGloveBtn
            // 
            this.rightGloveBtn.AutoSize = true;
            this.rightGloveBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rightGloveBtn.Location = new System.Drawing.Point(6, 49);
            this.rightGloveBtn.Name = "rightGloveBtn";
            this.rightGloveBtn.Size = new System.Drawing.Size(78, 24);
            this.rightGloveBtn.TabIndex = 1;
            this.rightGloveBtn.Text = "Rechts";
            this.rightGloveBtn.UseVisualStyleBackColor = true;
            // 
            // gloveGroupBox
            // 
            this.gloveGroupBox.Controls.Add(this.leftGloveBtn);
            this.gloveGroupBox.Controls.Add(this.rightGloveBtn);
            this.gloveGroupBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gloveGroupBox.Location = new System.Drawing.Point(12, 12);
            this.gloveGroupBox.Name = "gloveGroupBox";
            this.gloveGroupBox.Size = new System.Drawing.Size(122, 73);
            this.gloveGroupBox.TabIndex = 2;
            this.gloveGroupBox.TabStop = false;
            this.gloveGroupBox.Text = "Handschoen";
            // 
            // fingerGroupBox
            // 
            this.fingerGroupBox.Controls.Add(this.finger4Btn);
            this.fingerGroupBox.Controls.Add(this.finger3Btn);
            this.fingerGroupBox.Controls.Add(this.finger1Btn);
            this.fingerGroupBox.Controls.Add(this.finger2Btn);
            this.fingerGroupBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fingerGroupBox.Location = new System.Drawing.Point(12, 91);
            this.fingerGroupBox.Name = "fingerGroupBox";
            this.fingerGroupBox.Size = new System.Drawing.Size(122, 141);
            this.fingerGroupBox.TabIndex = 3;
            this.fingerGroupBox.TabStop = false;
            this.fingerGroupBox.Text = "Vinger";
            // 
            // finger4Btn
            // 
            this.finger4Btn.AutoSize = true;
            this.finger4Btn.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.finger4Btn.Location = new System.Drawing.Point(6, 109);
            this.finger4Btn.Name = "finger4Btn";
            this.finger4Btn.Size = new System.Drawing.Size(102, 24);
            this.finger4Btn.TabIndex = 3;
            this.finger4Btn.Text = "Ringvinger";
            this.finger4Btn.UseVisualStyleBackColor = true;
            // 
            // finger3Btn
            // 
            this.finger3Btn.AutoSize = true;
            this.finger3Btn.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.finger3Btn.Location = new System.Drawing.Point(6, 79);
            this.finger3Btn.Name = "finger3Btn";
            this.finger3Btn.Size = new System.Drawing.Size(115, 24);
            this.finger3Btn.TabIndex = 2;
            this.finger3Btn.Text = "Middelvinger";
            this.finger3Btn.UseVisualStyleBackColor = true;
            // 
            // finger1Btn
            // 
            this.finger1Btn.AutoSize = true;
            this.finger1Btn.Checked = true;
            this.finger1Btn.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.finger1Btn.Location = new System.Drawing.Point(6, 19);
            this.finger1Btn.Name = "finger1Btn";
            this.finger1Btn.Size = new System.Drawing.Size(64, 24);
            this.finger1Btn.TabIndex = 0;
            this.finger1Btn.TabStop = true;
            this.finger1Btn.Text = "Duim";
            this.finger1Btn.UseVisualStyleBackColor = true;
            // 
            // finger2Btn
            // 
            this.finger2Btn.AutoSize = true;
            this.finger2Btn.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.finger2Btn.Location = new System.Drawing.Point(6, 49);
            this.finger2Btn.Name = "finger2Btn";
            this.finger2Btn.Size = new System.Drawing.Size(98, 24);
            this.finger2Btn.TabIndex = 1;
            this.finger2Btn.Text = "Wijsvinger";
            this.finger2Btn.UseVisualStyleBackColor = true;
            // 
            // startStopBtn
            // 
            this.startStopBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.startStopBtn.Location = new System.Drawing.Point(12, 238);
            this.startStopBtn.Name = "startStopBtn";
            this.startStopBtn.Size = new System.Drawing.Size(122, 56);
            this.startStopBtn.TabIndex = 4;
            this.startStopBtn.Text = "Start kalibratie";
            this.startStopBtn.UseVisualStyleBackColor = true;
            this.startStopBtn.Click += new System.EventHandler(this.startStopBtn_click);
            // 
            // currentValLabel
            // 
            this.currentValLabel.AutoSize = true;
            this.currentValLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.currentValLabel.Location = new System.Drawing.Point(242, 156);
            this.currentValLabel.Name = "currentValLabel";
            this.currentValLabel.Size = new System.Drawing.Size(240, 29);
            this.currentValLabel.TabIndex = 5;
            this.currentValLabel.Text = "Huidige waarde: 0,00";
            // 
            // descLabel3
            // 
            this.descLabel3.AutoSize = true;
            this.descLabel3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.descLabel3.Location = new System.Drawing.Point(140, 91);
            this.descLabel3.Name = "descLabel3";
            this.descLabel3.Size = new System.Drawing.Size(329, 24);
            this.descLabel3.TabIndex = 6;
            this.descLabel3.Text = "en druk vervolgends op \'maak meting\'";
            // 
            // descLabel2
            // 
            this.descLabel2.AutoSize = true;
            this.descLabel2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.descLabel2.Location = new System.Drawing.Point(140, 61);
            this.descLabel2.Name = "descLabel2";
            this.descLabel2.Size = new System.Drawing.Size(477, 24);
            this.descLabel2.TabIndex = 7;
            this.descLabel2.Text = "totdat de onderstaande waarde niet veel meer verandert";
            // 
            // descLabel1
            // 
            this.descLabel1.AutoSize = true;
            this.descLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.descLabel1.Location = new System.Drawing.Point(140, 30);
            this.descLabel1.Name = "descLabel1";
            this.descLabel1.Size = new System.Drawing.Size(401, 24);
            this.descLabel1.TabIndex = 8;
            this.descLabel1.Text = "Plaats 0 gram druk op de geselecteerde vinger";
            // 
            // measurementBtn
            // 
            this.measurementBtn.Enabled = false;
            this.measurementBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.measurementBtn.Location = new System.Drawing.Point(283, 216);
            this.measurementBtn.Name = "measurementBtn";
            this.measurementBtn.Size = new System.Drawing.Size(150, 62);
            this.measurementBtn.TabIndex = 9;
            this.measurementBtn.Text = "Maak meting (spatie)";
            this.measurementBtn.UseVisualStyleBackColor = true;
            this.measurementBtn.Click += new System.EventHandler(this.measurementBtn_click);
            // 
            // PPG1
            // 
            this.PPG1.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.PPG1_DataReceived);
            // 
            // PPG2
            // 
            this.PPG2.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.PPG2_DataReceived);
            // 
            // dataRefreshTimer
            // 
            this.dataRefreshTimer.Interval = 500;
            this.dataRefreshTimer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // progressLabel
            // 
            this.progressLabel.AutoSize = true;
            this.progressLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.progressLabel.Location = new System.Drawing.Point(140, 9);
            this.progressLabel.Name = "progressLabel";
            this.progressLabel.Size = new System.Drawing.Size(79, 16);
            this.progressLabel.TabIndex = 10;
            this.progressLabel.Text = "Meting 0/0";
            // 
            // resetBtn
            // 
            this.resetBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.resetBtn.Location = new System.Drawing.Point(12, 300);
            this.resetBtn.Name = "resetBtn";
            this.resetBtn.Size = new System.Drawing.Size(122, 59);
            this.resetBtn.TabIndex = 11;
            this.resetBtn.Text = "Reset kalibratie";
            this.resetBtn.UseVisualStyleBackColor = true;
            this.resetBtn.Click += new System.EventHandler(this.resetBtn_click);
            // 
            // backBtn
            // 
            this.backBtn.Enabled = false;
            this.backBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.backBtn.Location = new System.Drawing.Point(283, 300);
            this.backBtn.Name = "backBtn";
            this.backBtn.Size = new System.Drawing.Size(150, 47);
            this.backBtn.TabIndex = 12;
            this.backBtn.Text = "Terug";
            this.backBtn.UseVisualStyleBackColor = true;
            this.backBtn.Click += new System.EventHandler(this.backBtn_click);
            // 
            // calibrationChart
            // 
            chartArea2.AxisX.Title = "PPG Sensor Value";
            chartArea2.AxisY.Title = "Force (g)";
            chartArea2.Name = "ChartArea1";
            this.calibrationChart.ChartAreas.Add(chartArea2);
            this.calibrationChart.Enabled = false;
            this.calibrationChart.Location = new System.Drawing.Point(623, 9);
            this.calibrationChart.Name = "calibrationChart";
            series3.ChartArea = "ChartArea1";
            series3.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
            series3.Name = "Series1";
            series3.Points.Add(dataPoint2);
            series4.ChartArea = "ChartArea1";
            series4.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            series4.Name = "Series2";
            this.calibrationChart.Series.Add(series3);
            this.calibrationChart.Series.Add(series4);
            this.calibrationChart.Size = new System.Drawing.Size(714, 343);
            this.calibrationChart.TabIndex = 13;
            this.calibrationChart.Text = "Calibration";
            // 
            // CalibrationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1344, 364);
            this.Controls.Add(this.calibrationChart);
            this.Controls.Add(this.backBtn);
            this.Controls.Add(this.resetBtn);
            this.Controls.Add(this.progressLabel);
            this.Controls.Add(this.measurementBtn);
            this.Controls.Add(this.descLabel1);
            this.Controls.Add(this.descLabel2);
            this.Controls.Add(this.descLabel3);
            this.Controls.Add(this.currentValLabel);
            this.Controls.Add(this.startStopBtn);
            this.Controls.Add(this.fingerGroupBox);
            this.Controls.Add(this.gloveGroupBox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "CalibrationForm";
            this.Text = "PPG Calibration Tool";
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.CalibrationForm_KeyPress);
            this.gloveGroupBox.ResumeLayout(false);
            this.gloveGroupBox.PerformLayout();
            this.fingerGroupBox.ResumeLayout(false);
            this.fingerGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.calibrationChart)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton leftGloveBtn;
        private System.Windows.Forms.RadioButton rightGloveBtn;
        private System.Windows.Forms.GroupBox gloveGroupBox;
        private System.Windows.Forms.GroupBox fingerGroupBox;
        private System.Windows.Forms.RadioButton finger1Btn;
        private System.Windows.Forms.RadioButton finger2Btn;
        private System.Windows.Forms.RadioButton finger4Btn;
        private System.Windows.Forms.RadioButton finger3Btn;
        private System.Windows.Forms.Button startStopBtn;
        private System.Windows.Forms.Label currentValLabel;
        private System.Windows.Forms.Label descLabel3;
        private System.Windows.Forms.Label descLabel2;
        private System.Windows.Forms.Label descLabel1;
        private System.Windows.Forms.Button measurementBtn;
        private System.IO.Ports.SerialPort PPG1;
        private System.IO.Ports.SerialPort PPG2;
        private System.Windows.Forms.Timer dataRefreshTimer;
        private System.Windows.Forms.Label progressLabel;
        private System.Windows.Forms.Button resetBtn;
        private System.Windows.Forms.Button backBtn;
        private System.Windows.Forms.DataVisualization.Charting.Chart calibrationChart;
    }
}