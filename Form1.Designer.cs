namespace WorkHoursCalculator
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            dateTimePickerStart = new DateTimePicker();
            dateTimePickerEnd = new DateTimePicker();
            txtBreakTime = new TextBox();
            btnCalculate = new Button();
            txtWorkHours = new TextBox();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            label6 = new Label();
            SuspendLayout();
            // 
            // dateTimePickerStart
            // 
            dateTimePickerStart.Location = new Point(12, 34);
            dateTimePickerStart.Name = "dateTimePickerStart";
            dateTimePickerStart.Size = new Size(200, 23);
            dateTimePickerStart.TabIndex = 0;
            // 
            // dateTimePickerEnd
            // 
            dateTimePickerEnd.Location = new Point(12, 90);
            dateTimePickerEnd.Name = "dateTimePickerEnd";
            dateTimePickerEnd.Size = new Size(200, 23);
            dateTimePickerEnd.TabIndex = 1;
            // 
            // txtBreakTime
            // 
            txtBreakTime.Location = new Point(12, 144);
            txtBreakTime.Name = "txtBreakTime";
            txtBreakTime.Size = new Size(75, 23);
            txtBreakTime.TabIndex = 2;
            // 
            // btnCalculate
            // 
            btnCalculate.Location = new Point(70, 175);
            btnCalculate.Name = "btnCalculate";
            btnCalculate.Size = new Size(75, 23);
            btnCalculate.TabIndex = 3;
            btnCalculate.Text = "计算工时";
            btnCalculate.UseVisualStyleBackColor = true;
            btnCalculate.Click += btnCalculate_Click;
            // 
            // txtWorkHours
            // 
            txtWorkHours.Location = new Point(127, 144);
            txtWorkHours.Name = "txtWorkHours";
            txtWorkHours.ReadOnly = true;
            txtWorkHours.Size = new Size(75, 23);
            txtWorkHours.TabIndex = 4;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(20, 123);
            label1.Name = "label1";
            label1.Size = new Size(56, 17);
            label1.TabIndex = 5;
            label1.Text = "休息时常";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(143, 123);
            label2.Name = "label2";
            label2.Size = new Size(44, 17);
            label2.TabIndex = 6;
            label2.Text = "总工时";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(16, 70);
            label3.Name = "label3";
            label3.Size = new Size(56, 17);
            label3.TabIndex = 7;
            label3.Text = "下班时间";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(15, 14);
            label4.Name = "label4";
            label4.Size = new Size(56, 17);
            label4.TabIndex = 8;
            label4.Text = "上班时间";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(90, 148);
            label5.Name = "label5";
            label5.Size = new Size(15, 17);
            label5.TabIndex = 9;
            label5.Text = "h";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(203, 147);
            label6.Name = "label6";
            label6.Size = new Size(15, 17);
            label6.TabIndex = 10;
            label6.Text = "h";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(224, 207);
            Controls.Add(label6);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(txtWorkHours);
            Controls.Add(btnCalculate);
            Controls.Add(txtBreakTime);
            Controls.Add(dateTimePickerEnd);
            Controls.Add(dateTimePickerStart);
            Name = "Form1";
            Text = "工时计算器";
            Load += Form1_Load;
            SizeChanged += Form1_SizeChanged;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DateTimePicker dateTimePickerStart;
        private DateTimePicker dateTimePickerEnd;
        private TextBox txtBreakTime;
        private Button btnCalculate;
        private TextBox txtWorkHours;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
    }
}
