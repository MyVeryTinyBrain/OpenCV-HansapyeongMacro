using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Runtime.InteropServices;
using System.Net;
using System.Net.Sockets;

namespace HansapyeongMacroOpenCV
{
    public partial class Form1 : System.Windows.Forms.Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public delegate void ThreadStopEvent();
        public event ThreadStopEvent threadStopEvent;
        Form2 m_optionForm;

        LogPanel m_logPanel;
        CaptureMachine m_captureMachine;
        StateMachine m_stateMachine;

        private void Form_Load(object sender, EventArgs e)
        {
            threadStopEvent += new ThreadStopEvent(OnThreadStop);
            m_optionForm = new Form2();
        }

        private void Form_Shown(object sender, EventArgs e)
        {
            string[] enum_names = Enum.GetNames(typeof(StudyState));
            foreach (string name in enum_names) comboBox_startState.Items.Add(name);
            comboBox_startState.SelectedIndex = 0;

            m_logPanel = new LogPanel(textbox_logpanel);
            bool CMComplete;
            m_captureMachine = new CaptureMachine(out CMComplete);
            m_stateMachine = new StateMachine(m_captureMachine, threadStopEvent);

            Config.LoadConfig();
        }

        private void button_start_Click(object sender, EventArgs e)
        {
            if (!m_stateMachine.isWorking)
            {
                button_start.Text = "Stop";

                StudyState startState = (StudyState)Enum.Parse(typeof(StudyState), (string)comboBox_startState.SelectedItem);
                m_stateMachine.Begin(startState);
            }
            else
            {
                SetButtonActive();
            }
        }

        private void SetButtonActive()
        {
            button_start.Text = "Start";
            m_stateMachine.Stop();
        }

        private void OnThreadStop()
        {
            if (Config.data.shutdown)
            {
                System.Diagnostics.Process.Start("shutdown.exe", "-s -t 0");
                LogPanel.Log("시스템 종료");
            }

            SetButtonActive();
        }

        private void button_option_Click(object sender, EventArgs e)
        {
            m_optionForm.ShowDialog();
        }

        private static string GetLocalIP()
        {
            string localIP = string.Empty;
            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    localIP = ip.ToString();
                    break;
                }
            }
            return localIP;
        }
    }
}
