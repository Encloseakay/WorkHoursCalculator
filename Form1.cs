using System;
using System.Windows.Forms;
using NPOI.XSSF.UserModel;  // ���� NPOI Excel �����
using NPOI.SS.UserModel;
using System.IO;

namespace WorkHoursCalculator
{
    public partial class Form1 : Form
    {
        #region �ؼ��Զ���
        public Form1()
        {
            InitializeComponent();
            // ���ÿ�ʼʱ��Ŀؼ�
            dateTimePickerStart.Format = DateTimePickerFormat.Custom;
            dateTimePickerStart.CustomFormat = "yyyy-MM-dd HH:mm";  // �Զ�����ʾ��ʽ�����ں�ʱ��
            dateTimePickerStart.ShowUpDown = false;  // Ĭ����ʾ���ں�ʱ�䣬����������ѡ��ģʽ

            // ���ý���ʱ��Ŀؼ�
            dateTimePickerEnd.Format = DateTimePickerFormat.Custom;
            dateTimePickerEnd.CustomFormat = "yyyy-MM-dd HH:mm";    // �Զ�����ʾ��ʽ�����ں�ʱ��
            dateTimePickerEnd.ShowUpDown = false;  // Ĭ����ʾ���ں�ʱ�䣬����������ѡ��ģʽ
            txtBreakTime.KeyDown += TxtBreakTime_KeyDown;
        }
        #endregion


        #region ��������Ӧ
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


        #region �س����¼�
        private double ParseBreakTime(string breakTimeText)
        {
            // ���� 'Сʱ:����' ��ʽ������
            string[] breakTimeParts = breakTimeText.Split(':');
            if (breakTimeParts.Length == 2)
            {
                int hours = int.Parse(breakTimeParts[0]);
                int minutes = int.Parse(breakTimeParts[1]);

                // ��ʱ��ת��ΪСʱ�� (����С������)
                return hours + minutes / 60.0;
            }
            else
            {
                throw new FormatException("��Ϣʱ���ʽ����ȷ����ʹ�� 'Сʱ:����' ��ʽ��");
            }
        }
        #endregion


        #region �����ȡ
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
                    MessageBox.Show("�������Ϣʱ���ʽ����ȷ��������һ����Ч�����֡�", "����", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        #endregion


        #region ���ݴ���
        private void btnCalculate_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime startTime = dateTimePickerStart.Value;
                DateTime endTime = dateTimePickerEnd.Value;

                if (startTime.Date == endTime.Date && endTime.TimeOfDay < startTime.TimeOfDay)
                {
                    MessageBox.Show("��⵽��ʼ�ͽ���ʱ���������ͬ��������ʱ�����ڿ�ʼʱ�䡣\n��ȷ���Ƿ������޸Ľ���ʱ������ڡ�", "��������δ�޸�", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

                    txtWorkHours.Text = $"{workHours}:{workRemainingMinutes:D2} Сʱ";

                    LogToExcel(startTime, endTime, breakTime.Hours, breakTime.Minutes, workHours, workRemainingMinutes);
                }
                else
                {
                    MessageBox.Show("��Ϣʱ�������ʽ������ʹ�� 'Сʱ:����' ��ʽ������ 1:30 ��ʾ 1 Сʱ 30 ���ӡ�", "����", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("��������: " + ex.Message, "����", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion


        #region excel����
        private void LogToExcel(DateTime startTime, DateTime endTime, int breakHours, int breakMinutes, int workHours, int workMinutes)
        {
            string folderPath = @"WorkHoursLogs";
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            // �ļ�·��
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
                headerRow.CreateCell(0).SetCellValue("��ʼʱ��");
                headerRow.CreateCell(1).SetCellValue("����ʱ��");
                headerRow.CreateCell(2).SetCellValue("��Ϣʱ��(Сʱ:����)");
                headerRow.CreateCell(3).SetCellValue("����ʱ��(Сʱ)");
                headerRow.CreateCell(4).SetCellValue("����ʱ��(����)");
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
