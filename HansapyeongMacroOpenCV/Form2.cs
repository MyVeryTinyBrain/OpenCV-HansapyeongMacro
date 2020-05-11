using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HansapyeongMacroOpenCV
{
    public partial class Form2 : Form
    {
        private const string HELP_MESSAGE1 =
            "다음과 같이 창을 띄워논 후 Start 버튼을 눌러 프로그램을 동작시키세요.\r\n" +
            "띄워논 강좌만큼 자동수강을 진행하게 됩니다.\r\n" +
            "\r\n" +
            "* 반드시 수강할 강좌는 사전에 한 번 클릭하여 '미완료' 아이콘이 보이는 상태여야만 합니다.\r\n" + 
            "* 해당 이미지는 '완료'로 나와있으나 실제 사용시에는 '미완료' 상태여야만 합니다.";
        private const string HELP_MESSAGE2 =
            "1. 해당 프로그램은 1920*1080 이외의 해상도에서 올바르게 동작하지 않을 수 있습니다.\r\n" +
            "2. 수강하는 강좌마다 button_begin1, button_begin2, button_O, button_X, button_resume 이미지가 다를 수 있습니다.\r\n" +
            "3. Config 설정을 통하여 세부 조정을 할 수 있습니다.";
        private const string HELP_MESSAGE3 =
            "1. 1920*1080 해상도로 프로그램을 동작할 수 없는 경우.\r\n" +
            "\r\n" +
            "윈도우의 '캡처 도구'를 사용하여 images 폴더 안의 모든 이미지를 새로 캡처하여 저장해야 합니다.";
        private const string HELP_MESSAGE4 =
            "2. 수강하는 강좌의 버튼 이미지 색상이 다른 경우.\r\n" +
            "\r\n" +
            "윈도우의 '캡처 도구'를 사용하여 images 폴더 안의 button_begin1, button_begin2, button_O, button_X, button_resume를 새로 캡처하여 저장해야 합니다.";
        private const string HELP_MESSAGE5 =
            "adjustment: 미완료 버튼으로부터 얼마나 왼쪽을 클릭할지 정하는 값입니다.\r\n" +
            "tick: 화면 인식 스레드가 화면을 새로 인식하는 간격(초)입니다. 값을 증가시키면 CPU 할당량이 감소합니다.\r\n" +
            "button_begin_wait: 학습 시작 버튼을 누르기 전 까지의 대기시간입니다.\r\n" +
            "accuracy: 각 이미지의 정확도 판정입니다.\r\n" +
            "shutdown: 모든 작업을 끝내고 컴퓨터를 종료합니다.\r\n" +
            "reset: 모든 설정을 초기화합니다.";

        private Form3 m_imageForm;

        public Form2()
        {
            InitializeComponent();

            m_imageForm = new Form3();
        }

        private void Initalize()
        {
            // 1~6
            bar_adjustment_x.Value = Config.data.adjustment_x;
            bar_adjustment_x_Scroll(null, null);

            bar_adjustment_y.Value = Config.data.adjustment_y;
            bar_adjustment_y_Scroll(null, null);

            bar_tick.Value = (int)(Config.data.tick * 0.001);
            bar_tick_Scroll(null, null);

            bar_button_begin_wait.Value = (int)(Config.data.button_begin_wait * 0.001);
            bar_button_begin_wait_Scroll(null, null);

            bar_box_uncomplete_accuracy.Value = (int)(Config.data.box_uncomplete_accuracy * 10);
            bar_box_uncomplete_accuracy_Scroll(null, null);

            bar_button_begin_accuracy.Value = (int)(Config.data.button_begin_accuracy * 10);
            bar_button_begin_accuracy_Scroll(null, null);

            bar_state_quiz_accuracy.Value = (int)(Config.data.state_quiz_accuracy * 10);
            bar_state_quiz_accuracy_Scroll(null, null);

            //7~12
            bar_state_fullstudy_accuracy.Value = (int)(Config.data.state_fullstudy_accuracy * 10);
            bar_state_fullstudy_accuracy_Scroll(null, null);

            bar_button_quiz_ox_accuracy.Value = (int)(Config.data.button_quiz_ox_accuracy * 10);
            bar_button_quiz_ox_accuracy_Scroll(null, null);

            bar_button_resume_accuracy.Value = (int)(Config.data.button_resume_accuracy * 10);
            bar_button_resume_accuracy_Scroll(null, null);

            bar_button_tryexit_accuracy.Value = (int)(Config.data.button_tryexit_accuracy * 10);
            bar_button_tryexit_accuracy_Scroll(null, null);

            bar_button_ok_accuracy.Value = (int)(Config.data.button_ok_accuracy * 10);
            bar_button_ok_accuracy_Scroll(null, null);

            bar_button_exit_accuracy.Value = (int)(Config.data.button_exit_accuracy * 10);
            bar_button_exit_accuracy_Scroll(null, null);

            checkBox_shutdown.Checked = Config.data.shutdown;
        }

        private void Reflect()
        {
            Config.data.adjustment_x = bar_adjustment_x.Value;
            Config.data.adjustment_y = bar_adjustment_y.Value;
            Config.data.tick = bar_tick.Value * 1000;
            Config.data.button_begin_wait = bar_button_begin_wait.Value * 1000;
            Config.data.box_uncomplete_accuracy = bar_box_uncomplete_accuracy.Value * 0.1;
            Config.data.button_begin_accuracy = bar_button_begin_accuracy.Value * 0.1;
            Config.data.state_quiz_accuracy = bar_state_quiz_accuracy.Value * 0.1;
            Config.data.state_fullstudy_accuracy = bar_state_fullstudy_accuracy.Value * 0.1;
            Config.data.button_quiz_ox_accuracy = bar_button_quiz_ox_accuracy.Value * 0.1;
            Config.data.button_resume_accuracy = bar_button_resume_accuracy.Value * 0.1;
            Config.data.button_tryexit_accuracy = bar_button_tryexit_accuracy.Value * 0.1;
            Config.data.button_ok_accuracy = bar_button_ok_accuracy.Value * 0.1;
            Config.data.button_exit_accuracy = bar_button_exit_accuracy.Value * 0.1;
            Config.data.shutdown = checkBox_shutdown.Checked;
        }

        private void Form2_Shown(object sender, EventArgs e)
        {
            Initalize();
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            Reflect();
            Config.SaveConfig();
        }

        private void button_reset_Click(object sender, EventArgs e)
        {
            Config.Reset();
            Initalize();
        }

        private void bar_adjustment_x_Scroll(object sender, EventArgs e)
        {
            textbox_adjustment_x.Text = "\r\n" + bar_adjustment_x.Value.ToString();
        }

        private void bar_adjustment_y_Scroll(object sender, EventArgs e)
        {
            textbox_adjustment_y.Text = "\r\n" + bar_adjustment_y.Value.ToString();
        }

        private void bar_tick_Scroll(object sender, EventArgs e)
        {
            textbox_tick.Text = "\r\n" + bar_tick.Value.ToString();
        }

        private void bar_button_begin_wait_Scroll(object sender, EventArgs e)
        {
            textbox_button_begin_wait.Text = "\r\n" + bar_button_begin_wait.Value.ToString();
        }

        private void bar_box_uncomplete_accuracy_Scroll(object sender, EventArgs e)
        {
            textbox_box_uncomplete_accuracy.Text = "\r\n" + bar_box_uncomplete_accuracy.Value.ToString();
        }

        private void bar_button_begin_accuracy_Scroll(object sender, EventArgs e)
        {
            textbox_button_begin_accuracy.Text = "\r\n" + bar_button_begin_accuracy.Value.ToString();
        }

        private void bar_state_quiz_accuracy_Scroll(object sender, EventArgs e)
        {
            textbox_state_quiz_accuracy.Text = "\r\n" + bar_state_quiz_accuracy.Value.ToString();
        }

        private void bar_state_fullstudy_accuracy_Scroll(object sender, EventArgs e)
        {
            textbox_state_fullstudy_accuracy.Text = "\r\n" + bar_state_fullstudy_accuracy.Value.ToString();
        }

        private void bar_button_quiz_ox_accuracy_Scroll(object sender, EventArgs e)
        {
            textbox_button_quiz_ox_accuracy.Text = "\r\n" + bar_button_quiz_ox_accuracy.Value.ToString();
        }

        private void bar_button_resume_accuracy_Scroll(object sender, EventArgs e)
        {
            textbox_button_resume_accuracy.Text = "\r\n" + bar_button_resume_accuracy.Value.ToString();
        }

        private void bar_button_tryexit_accuracy_Scroll(object sender, EventArgs e)
        {
            textbox_button_tryexit_accuracy.Text = "\r\n" + bar_button_tryexit_accuracy.Value.ToString();
        }

        private void bar_button_ok_accuracy_Scroll(object sender, EventArgs e)
        {
            textbox_button_ok_accuracy.Text = "\r\n" + bar_button_ok_accuracy.Value.ToString();
        }

        private void bar_button_exit_accuracy_Scroll(object sender, EventArgs e)
        {
            textbox_button_exit_accuracy.Text = "\r\n" + bar_button_exit_accuracy.Value.ToString();
        }

        private void button_help_Click(object sender, EventArgs e)
        {
            MessageBox.Show(HELP_MESSAGE1, "Help");
            m_imageForm.ShowDialog();
            MessageBox.Show(HELP_MESSAGE2, "Help");
            MessageBox.Show(HELP_MESSAGE3, "Help");
            MessageBox.Show(HELP_MESSAGE4, "Help");
            MessageBox.Show(HELP_MESSAGE5, "Help");
        }
    }
}
