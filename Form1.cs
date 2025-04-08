using System;
using System.Windows.Forms;

namespace WorkHoursCalculator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            // 设置开始时间的控件
            dateTimePickerStart.Format = DateTimePickerFormat.Custom;
            dateTimePickerStart.CustomFormat = "yyyy-MM-dd HH:mm";  // 自定义显示格式，日期和时间
            dateTimePickerStart.ShowUpDown = false;  // 默认显示日期和时间，不启用上下选择模式

            // 设置结束时间的控件
            dateTimePickerEnd.Format = DateTimePickerFormat.Custom;
            dateTimePickerEnd.CustomFormat = "yyyy-MM-dd HH:mm";    // 自定义显示格式，日期和时间
            dateTimePickerEnd.ShowUpDown = false;  // 默认显示日期和时间，不启用上下选择模式
            txtBreakTime.KeyDown += TxtBreakTime_KeyDown;
        }
        private new AutoAdaptWindowsSize AutoSize;
        private void Form1_Load(object sender, EventArgs e)
        {
            AutoSize = new AutoAdaptWindowsSize(this);
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            if (AutoSize != null) 
            {
                AutoSize.FormSizeChanged();
            }
        }
        /// <summary>
        /// 回车键事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxtBreakTime_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                try
                {
                    double breakTimeInHours = double.Parse(txtBreakTime.Text);

                    int breakTimeInMinutes = (int)(breakTimeInHours * 60); 

                    int breakHours = breakTimeInMinutes / 60;
                    int breakMinutes = breakTimeInMinutes % 60;

                    txtBreakTime.Text = $"{breakHours}:{breakMinutes:D2}";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("输入的休息时间格式不正确，请输入一个有效的数字。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnCalculate_Click(object sender, EventArgs e)
        {
            try
            {
                // 获取输入的开始时间和结束时间
                DateTime startTime = dateTimePickerStart.Value;
                DateTime endTime = dateTimePickerEnd.Value;

                // 获取输入的休息时间（单位：小时）
                double breakTimeInHours = double.Parse(txtBreakTime.Text.Split(':')[0]);  // 只取小时部分
                double breakTimeInMinutes = breakTimeInHours * 60;  // 转换为分钟

                // 将开始时间和结束时间转换为分钟数
                int startMinutes = startTime.Hour * 60 + startTime.Minute;
                int endMinutes = endTime.Hour * 60 + endTime.Minute;

                // 计算总时间（以分钟为单位）
                int totalMinutes = endMinutes - startMinutes;

                // 检查结束时间是否早于开始时间
                if (totalMinutes < 0)
                {
                    MessageBox.Show("结束时间必须晚于开始时间", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // 减去休息时间
                int workMinutes = totalMinutes - (int)breakTimeInMinutes;

                // 如果工作时间少于零，则表示工时不能为负值，设为零
                if (workMinutes < 0)
                {
                    workMinutes = 0;
                }

                // 转换工作时间为小时和分钟
                int workHours = workMinutes / 60; // 小时
                int workRemainingMinutes = workMinutes % 60; // 分钟

                // 显示计算结果到TextBox
                txtWorkHours.Text = $"{workHours}:{workRemainingMinutes:D2} 小时";  // 显示为小时:分钟
            }
            catch (Exception ex)
            {
                MessageBox.Show("发生错误: " + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
