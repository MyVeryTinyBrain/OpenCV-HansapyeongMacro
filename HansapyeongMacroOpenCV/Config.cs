using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Drawing.Imaging;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization.Formatters.Binary;

namespace HansapyeongMacroOpenCV
{
    static class Config
    {
        [System.Serializable]
        public class ConfigData
        {
            // Default Configs
            private const int ADJUSTMENT_X = 375;
            private const int ADJUSTMENT_Y = 16;
            private const double TICK = 5000.0;
            private const double BUTTON_BEGIN_WAIT = 1000.0;
            private const double BOX_UNCOMPLETE_ACCURACY = 0.9;
            private const double BUTTON_BEGIN_ACCURACY = 0.6;
            private const double STATE_QUIZ_ACCURACY = 0.9;
            private const double STATE_FULLSTUDY_ACCURACY = 0.9;
            private const double BUTTON_QUIZ_OX_ACCURACY = 0.9;
            private const double BUTTON_RESUME_ACCURACY = 0.9;
            private const double BUTTON_TRYEXIT_ACCURACY = 0.9;
            private const double BUTTON_OK_ACCURACY = 0.6;
            private const double BUTTON_EXIT_ACCURACY = 0.9;
            private const bool SHUTDOWN = false;
            private const bool START_IE = true;

            // Using Configs
            public int adjustment_x = ADJUSTMENT_X;
            public int adjustment_y = ADJUSTMENT_Y;

            public double tick = TICK;
            public double button_begin_wait = BUTTON_BEGIN_WAIT;

            public double box_uncomplete_accuracy = BOX_UNCOMPLETE_ACCURACY;
            public double button_begin_accuracy = BUTTON_BEGIN_ACCURACY;
            public double state_quiz_accuracy = STATE_QUIZ_ACCURACY;
            public double state_fullstudy_accuracy = STATE_FULLSTUDY_ACCURACY;
            public double button_quiz_ox_accuracy = BUTTON_QUIZ_OX_ACCURACY;
            public double button_resume_accuracy = BUTTON_RESUME_ACCURACY;
            public double button_tryexit_accuracy = BUTTON_TRYEXIT_ACCURACY;
            public double button_ok_accuracy = BUTTON_OK_ACCURACY;
            public double button_exit_accuracy = BUTTON_EXIT_ACCURACY;

            public bool shutdown = SHUTDOWN;
            public bool start_IE = START_IE;
        }

        public const string FILE_NAME = "config.bin";
        public static ConfigData data = new ConfigData();

        public static void Reset()
        {
            data = new ConfigData();
            LogPanel.Log(FILE_NAME + " Initalized");
        }

        public static void SaveConfig()
        {
            using(FileStream fs = new FileStream(FILE_NAME, FileMode.Create, FileAccess.Write))
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(fs, data);
                LogPanel.Log(FILE_NAME + " Saved");
            }
        }

        public static void LoadConfig()
        {
            try
            {
                using (FileStream fs = new FileStream(FILE_NAME, FileMode.Open, FileAccess.Read))
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    data = bf.Deserialize(fs) as ConfigData;
                    LogPanel.Log(FILE_NAME + " Loaded");
                }
            }
            catch(Exception e)
            {
                Reset();
                SaveConfig();
            }
        }
    }
}
