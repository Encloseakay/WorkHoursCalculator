using System;
using System.Windows.Forms;

namespace WorkHoursCalculator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();


            // 设置控件格式
            dateTimePickerStart.Format = DateTimePickerFormat.Custom;
            dateTimePickerStart.CustomFormat = "HH:mm";  // 自定义显示为小时:分钟
            dateTimePickerStart.ShowUpDown = true;       // 启用上下选择模式

            dateTimePickerEnd.Format = DateTimePickerFormat.Custom;
            dateTimePickerEnd.CustomFormat = "HH:mm";    // 显示为小时:分钟
            dateTimePickerEnd.ShowUpDown = true;         // 启用上下选择模式

            // 绑定回车键事件
            txtBreakTime.KeyDown += TxtBreakTime_KeyDown;
        }
        private new AutoAdaptWindowsSize AutoSize;
        private void Form1_Load(object sender, EventArgs e)
        {
            // 设置窗体的自适应大小
            AutoSize = new AutoAdaptWindowsSize(this);
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {//窗体大小改变事件
            if (AutoSize != null) // 一定加这个判断，电脑缩放布局不是100%的时候，会报错
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
            // 检查是否按下回车键（Enter）
            if (e.KeyCode == Keys.Enter)
            {
                try
                {
                    // 获取输入的休息时间（单位：小时）
                    double breakTimeInHours = double.Parse(txtBreakTime.Text);

                    // 将休息时间转换为分钟
                    int breakTimeInMinutes = (int)(breakTimeInHours * 60); // 转换为分钟

                    // 将休息时间重新格式化为小时和分钟
                    int breakHours = breakTimeInMinutes / 60;
                    int breakMinutes = breakTimeInMinutes % 60;

                    // 显示休息时间为 "小时:分钟" 格式
                    txtBreakTime.Text = $"{breakHours}:{breakMinutes:D2}"; // 以小时:分钟的格式显示
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

                // 如果休息时间为空，设置为 "0:00" 小时并显示在 TextBox 中
                if (string.IsNullOrWhiteSpace(txtBreakTime.Text))
                {
                    txtBreakTime.Text = "0:00";  // 设置默认值为 0:00 小时
                }

                // 获取输入的休息时间（单位：小时），例如 1
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
