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
    class LogPanel
    {
        private static LogPanel g_instance;
        private RichTextBox m_textBox;

        public LogPanel(RichTextBox textBox)
        {
            g_instance = this;
            this.m_textBox = textBox;
            fLog("log panel initalized\r\n", Color.Green);
        }

        delegate void AppendTextInvoke(RichTextBox control, string s);
        private static void AppenTextCrossThread(RichTextBox control, string s)
        {
            if (control.InvokeRequired)
            {
                AppendTextInvoke f = new AppendTextInvoke(AppenTextCrossThread);
                control.Invoke(f, control, s);
            }
            else
            {
                control.AppendText(s);
            }
        }

        delegate void SelectInvoke(RichTextBox control, int start, int length);
        private static void SelectCrossThread(RichTextBox control, int start, int length)
        {
            if (control.InvokeRequired)
            {
                SelectInvoke f = new SelectInvoke(SelectCrossThread);
                control.Invoke(f, control, start, length);
            }
            else
            {
                control.Select(control.Text.Length, 0);
            }
        }

        delegate void ScrollToCaretInvoke(RichTextBox control);
        private static void ScrollToCaretCrossThread(RichTextBox control)
        {
            if (control.InvokeRequired)
            {
                ScrollToCaretInvoke f = new ScrollToCaretInvoke(ScrollToCaretCrossThread);
                control.Invoke(f, control);
            }
            else
            {
                control.ScrollToCaret();
            }
        }

        delegate void SetSelectionStartInvoke(RichTextBox control, int selectionStart);
        private static void SetSelectionStartCrossThread(RichTextBox control, int selectionStart)
        {
            if (control.InvokeRequired)
            {
                SetSelectionStartInvoke f = new SetSelectionStartInvoke(SetSelectionStartCrossThread);
                control.Invoke(f, control, selectionStart);
            }
            else
            {
                control.SelectionStart = selectionStart;
            }
        }

        delegate void SetSelectionLengthInvoke(RichTextBox control, int selectionLength);
        private static void SetSelectionLengthCrossThread(RichTextBox control, int selectionLength)
        {
            if (control.InvokeRequired)
            {
                SetSelectionLengthInvoke f = new SetSelectionLengthInvoke(SetSelectionLengthCrossThread);
                control.Invoke(f, control, selectionLength);
            }
            else
            {
                control.SelectionLength = selectionLength;
            }
        }

        delegate void SetSelectionColorInvoke(RichTextBox control, Color selectionColor);
        private static void SetSelectionColorCrossThread(RichTextBox control, Color selectionColor)
        {
            if (control.InvokeRequired)
            {
                SetSelectionColorInvoke f = new SetSelectionColorInvoke(SetSelectionColorCrossThread);
                control.Invoke(f, control, selectionColor);
            }
            else
            {
                control.SelectionColor = selectionColor;
            }
        }

        delegate int GetTextLengthInvoke(RichTextBox control);
        private static int GetTextLengthCrossThread(RichTextBox control)
        {
            if (control.InvokeRequired)
            {
                GetTextLengthInvoke f = new GetTextLengthInvoke(GetTextLengthCrossThread);
                return (int)control.Invoke(f, control);
            }
            else
            {
                return control.TextLength;
            }
        }

        delegate Color GetForeColorInvoke(RichTextBox control);
        private static Color GetForeColorCrossThread(RichTextBox control)
        {
            if (control.InvokeRequired)
            {
                GetForeColorInvoke f = new GetForeColorInvoke(GetForeColorCrossThread);
                return (Color)control.Invoke(f, control);
            }
            else
            {
                return control.ForeColor;
            }
        }

        private static void AppendTextWithColor(RichTextBox box, string text, Color color)
        {
            SetSelectionStartCrossThread(box, GetTextLengthCrossThread(box));
            SetSelectionLengthCrossThread(box, 0);

            SetSelectionColorCrossThread(box, color);
            AppenTextCrossThread(box, text);
            SetSelectionColorCrossThread(box, GetForeColorCrossThread(box));
        }

        public void fLog(object obj)
        {
            AppenTextCrossThread(m_textBox, string.Format($"{obj}\r\n"));
            SelectCrossThread(m_textBox, GetTextLengthCrossThread(m_textBox), 0);
            ScrollToCaretCrossThread(m_textBox);
        }

        public void fLog(object obj, Color color)
        {
            AppendTextWithColor(m_textBox, string.Format($"{obj}\r\n"), color);
            SelectCrossThread(m_textBox, GetTextLengthCrossThread(m_textBox), 0);
            ScrollToCaretCrossThread(m_textBox);
        }

        public void fLogNonReturn(object obj, Color color)
        {
            AppendTextWithColor(m_textBox, string.Format($"{obj}"), color);
            SelectCrossThread(m_textBox, GetTextLengthCrossThread(m_textBox), 0);
            ScrollToCaretCrossThread(m_textBox);
        }

        public static LogPanel instance
        {
            get => g_instance;
        }

        public static void Log(object obj)
        {
            g_instance.fLog(obj);
        }

        public static void Log(object obj, Color color)
        {
            g_instance.fLog(obj, color);
        }

        public static void LogNonReturn(object obj, Color color)
        {
            g_instance.fLogNonReturn(obj, color);
        }
    }
}
