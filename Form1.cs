using System;
using System.Windows.Forms;

namespace WorkHoursCalculator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();


            // ���ÿؼ���ʽ
            dateTimePickerStart.Format = DateTimePickerFormat.Custom;
            dateTimePickerStart.CustomFormat = "HH:mm";  // �Զ�����ʾΪСʱ:����
            dateTimePickerStart.ShowUpDown = true;       // ��������ѡ��ģʽ

            dateTimePickerEnd.Format = DateTimePickerFormat.Custom;
            dateTimePickerEnd.CustomFormat = "HH:mm";    // ��ʾΪСʱ:����
            dateTimePickerEnd.ShowUpDown = true;         // ��������ѡ��ģʽ

            // �󶨻س����¼�
            txtBreakTime.KeyDown += TxtBreakTime_KeyDown;
        }
        private new AutoAdaptWindowsSize AutoSize;
        private void Form1_Load(object sender, EventArgs e)
        {
            // ���ô��������Ӧ��С
            AutoSize = new AutoAdaptWindowsSize(this);
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {//�����С�ı��¼�
            if (AutoSize != null) // һ��������жϣ��������Ų��ֲ���100%��ʱ�򣬻ᱨ��
            {
                AutoSize.FormSizeChanged();
            }
        }
        /// <summary>
        /// �س����¼�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxtBreakTime_KeyDown(object sender, KeyEventArgs e)
        {
            // ����Ƿ��»س�����Enter��
            if (e.KeyCode == Keys.Enter)
            {
                try
                {
                    // ��ȡ�������Ϣʱ�䣨��λ��Сʱ��
                    double breakTimeInHours = double.Parse(txtBreakTime.Text);

                    // ����Ϣʱ��ת��Ϊ����
                    int breakTimeInMinutes = (int)(breakTimeInHours * 60); // ת��Ϊ����

                    // ����Ϣʱ�����¸�ʽ��ΪСʱ�ͷ���
                    int breakHours = breakTimeInMinutes / 60;
                    int breakMinutes = breakTimeInMinutes % 60;

                    // ��ʾ��Ϣʱ��Ϊ "Сʱ:����" ��ʽ
                    txtBreakTime.Text = $"{breakHours}:{breakMinutes:D2}"; // ��Сʱ:���ӵĸ�ʽ��ʾ
                }
                catch (Exception ex)
                {
                    MessageBox.Show("�������Ϣʱ���ʽ����ȷ��������һ����Ч�����֡�", "����", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnCalculate_Click(object sender, EventArgs e)
        {
            try
            {
                // ��ȡ����Ŀ�ʼʱ��ͽ���ʱ��
                DateTime startTime = dateTimePickerStart.Value;
                DateTime endTime = dateTimePickerEnd.Value;

                // �����Ϣʱ��Ϊ�գ�����Ϊ "0:00" Сʱ����ʾ�� TextBox ��
                if (string.IsNullOrWhiteSpace(txtBreakTime.Text))
                {
                    txtBreakTime.Text = "0:00";  // ����Ĭ��ֵΪ 0:00 Сʱ
                }

                // ��ȡ�������Ϣʱ�䣨��λ��Сʱ�������� 1
                double breakTimeInHours = double.Parse(txtBreakTime.Text.Split(':')[0]);  // ֻȡСʱ����
                double breakTimeInMinutes = breakTimeInHours * 60;  // ת��Ϊ����

                // ����ʼʱ��ͽ���ʱ��ת��Ϊ������
                int startMinutes = startTime.Hour * 60 + startTime.Minute;
                int endMinutes = endTime.Hour * 60 + endTime.Minute;

                // ������ʱ�䣨�Է���Ϊ��λ��
                int totalMinutes = endMinutes - startMinutes;

                // ������ʱ���Ƿ����ڿ�ʼʱ��
                if (totalMinutes < 0)
                {
                    MessageBox.Show("����ʱ��������ڿ�ʼʱ��", "����", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // ��ȥ��Ϣʱ��
                int workMinutes = totalMinutes - (int)breakTimeInMinutes;

                // �������ʱ�������㣬���ʾ��ʱ����Ϊ��ֵ����Ϊ��
                if (workMinutes < 0)
                {
                    workMinutes = 0;
                }

                // ת������ʱ��ΪСʱ�ͷ���
                int workHours = workMinutes / 60; // Сʱ
                int workRemainingMinutes = workMinutes % 60; // ����

                // ��ʾ��������TextBox
                txtWorkHours.Text = $"{workHours}:{workRemainingMinutes:D2} Сʱ";  // ��ʾΪСʱ:����
            }
            catch (Exception ex)
            {
                MessageBox.Show("��������: " + ex.Message, "����", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
