using System;
using System.Windows.Forms;
using NPOI.XSSF.UserModel;  // 引入 NPOI Excel 处理库
using NPOI.SS.UserModel;
using System.IO;

namespace WorkHoursCalculator
{
    public partial class Form1 : Form
    {
        #region 控件自定义
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
        #endregion


        #region 窗体自适应
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
        #endregion


        #region 回车键事件
        private double ParseBreakTime(string breakTimeText)
        {
            // 解析 '小时:分钟' 格式的输入
            string[] breakTimeParts = breakTimeText.Split(':');
            if (breakTimeParts.Length == 2)
            {
                int hours = int.Parse(breakTimeParts[0]);
                int minutes = int.Parse(breakTimeParts[1]);

                // 将时间转换为小时数 (包括小数部分)
                return hours + minutes / 60.0;
            }
            else
            {
                throw new FormatException("休息时间格式不正确，请使用 '小时:分钟' 格式。");
            }
        }
        #endregion


        #region 输入获取
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
        #endregion


        #region 数据处理
        private void btnCalculate_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime startTime = dateTimePickerStart.Value;
                DateTime endTime = dateTimePickerEnd.Value;

                if (startTime.Date == endTime.Date && endTime.TimeOfDay < startTime.TimeOfDay)
                {
                    MessageBox.Show("检测到开始和结束时间的日期相同，但结束时间早于开始时间。\n请确认是否忘记修改结束时间的日期。", "跨天日期未修改", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return; 
                }

                if (endTime <= startTime)
                {
                    endTime = endTime.AddDays(1);
                }

                TimeSpan breakTime;
                if (TimeSpan.TryParse(txtBreakTime.Text.Trim(), out breakTime))
                {
                    int breakTimeInMinutes = (int)Math.Round(breakTime.TotalMinutes);

                    TimeSpan workDuration = endTime.Subtract(startTime);
                    int totalMinutes = (int)Math.Round(workDuration.TotalMinutes);
                    int workMinutes = totalMinutes - breakTimeInMinutes;
                    if (workMinutes < 0) workMinutes = 0;

                    int workHours = workMinutes / 60;
                    int workRemainingMinutes = workMinutes % 60;

                    txtWorkHours.Text = $"{workHours}:{workRemainingMinutes:D2} 小时";

                    LogToExcel(startTime, endTime, breakTime.Hours, breakTime.Minutes, workHours, workRemainingMinutes);
                }
                else
                {
                    MessageBox.Show("休息时间输入格式错误，请使用 '小时:分钟' 格式，例如 1:30 表示 1 小时 30 分钟。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("发生错误: " + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion


        #region excel生成
        private void LogToExcel(DateTime startTime, DateTime endTime, int breakHours, int breakMinutes, int workHours, int workMinutes)
        {
            string folderPath = @"WorkHoursLogs";
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            // 文件路径
            string filePath = Path.Combine(folderPath, "WorkHoursLog.xlsx");

            XSSFWorkbook workbook = new XSSFWorkbook();
            ISheet sheet = workbook.CreateSheet("WorkHoursLog");

            if (File.Exists(filePath))
            {
                using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite))
                {
                    workbook = new XSSFWorkbook(fs);
                    sheet = workbook.GetSheetAt(0);
                }
            }
            else
            {
                IRow headerRow = sheet.CreateRow(0);
                headerRow.CreateCell(0).SetCellValue("开始时间");
                headerRow.CreateCell(1).SetCellValue("结束时间");
                headerRow.CreateCell(2).SetCellValue("休息时间(小时:分钟)");
                headerRow.CreateCell(3).SetCellValue("工作时间(小时)");
                headerRow.CreateCell(4).SetCellValue("工作时间(分钟)");
            }

            int row = sheet.LastRowNum + 1;
            IRow newRow = sheet.CreateRow(row);

            newRow.CreateCell(0).SetCellValue(startTime.ToString("yyyy-MM-dd HH:mm"));
            newRow.CreateCell(1).SetCellValue(endTime.ToString("yyyy-MM-dd HH:mm"));
            newRow.CreateCell(2).SetCellValue($"{breakHours}:{breakMinutes:D2}");
            newRow.CreateCell(3).SetCellValue(workHours);
            newRow.CreateCell(4).SetCellValue(workMinutes);

            using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                workbook.Write(fs);
            }
        }
        #endregion
    }
}
