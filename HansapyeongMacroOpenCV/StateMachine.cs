using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Threading;
using System.Runtime.InteropServices;

namespace HansapyeongMacroOpenCV
{
    class StateMachine
    {
        [DllImport("user32.dll")]
        static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint dwData, int dwExtraInfo);
        private const uint MOUSEEVENTF_LEFTDOWN = 0x0002;
        private const uint MOUSEEVENTF_LEFTUP = 0x0004;

        object                  m_locker;
        StudyState              m_State;
        CaptureMachine          m_captureMachine;
        Form1.ThreadStopEvent   m_callBack;

        Thread                  m_thread;
        bool                    m_isWorking;

        public StateMachine(CaptureMachine captureMachine, Form1.ThreadStopEvent callBack)
        {
            this.m_locker = new object();
            this.m_State = StudyState.SelectIndexTitle;
            this.m_captureMachine = captureMachine;
            this.m_isWorking = false;
            this.m_callBack = callBack;
        }

        public StudyState state
        {
            get => m_State;
        }

        public bool isWorking
        {
            get => m_isWorking;
        }

        public void Begin(StudyState startState)
        {
            if (m_isWorking) return;
            m_thread = new Thread(() => ThreadWork(m_locker, ref m_State, m_captureMachine, LogPanel.instance, ref m_isWorking, m_callBack));
            m_thread.IsBackground = true;
            m_isWorking = true;
            m_State = startState;
            m_thread.Start();
        }

        public void Stop()
        {
            m_isWorking = false;
            m_thread.Abort();
        }

        private static void mouse_move_click(int x, int y)
        {
            Cursor.Position = new System.Drawing.Point(x, y);
            mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
            mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
        }

        private static void ThreadWork(object locker, ref StudyState state, CaptureMachine cm, LogPanel logger, ref bool isWorking, Form1.ThreadStopEvent callBack)
        {
            int completeCount = 0;
            CaptureResult result;

            while (true)
            {
                lock (locker)
                {
                    switch (state)
                    {
                        case StudyState.SelectIndexTitle:
                            {
                                result = cm.Capture(CaptureMachine.BMP_BOX_UNCOMPLETE);
                                if (result.accuracy >= Config.data.box_uncomplete_accuracy)
                                {
                                    // 1줄 제목 처리
                                    mouse_move_click(result.center_x - Config.data.adjustment_x, result.center_y);
                                    Thread.Sleep(10);
                                    // 2줄 제목 처리
                                    mouse_move_click(result.center_x - Config.data.adjustment_x, result.center_y + Config.data.adjustment_y);
                                    state = StudyState.WaitForStartStudy;
                                    logger.fLog("수강 시작", Color.Green);
                                }
                                else
                                {
                                    logger.fLog("수강할 인덱스 없음", Color.Red);
                                    logger.fLog("스레드 종료", Color.Red);
                                    // 수강할 인덱스가 없다면 스레드를 종료
                                    isWorking = false;
                                    callBack();
                                    return;
                                }
                                break;
                            }

                        case StudyState.WaitForStartStudy:
                            {
                                // 시작버튼이 늦게 나오므로 잠시 대기
                                Thread.Sleep((int)(Config.data.button_begin_wait));

                                CaptureResult button_begin1_result = cm.Capture(CaptureMachine.BMP_BUTTON_BEGIN1);
                                CaptureResult button_begin2_result = cm.Capture(CaptureMachine.BMP_BUTTON_BEGIN2);
                                if (button_begin1_result.accuracy >= Config.data.button_begin_accuracy)
                                {
                                    mouse_move_click(button_begin1_result.center_x, button_begin1_result.center_y);
                                    state = StudyState.StudyScene;
                                    logger.fLog("학습 시작(case1)", Color.Green);
                                }
                                else if (button_begin2_result.accuracy >= Config.data.button_begin_accuracy)
                                {
                                    mouse_move_click(button_begin2_result.center_x, button_begin2_result.center_y);
                                    state = StudyState.StudyScene;
                                    logger.fLog("학습 시작(case2)", Color.Green);
                                }
                                else
                                {
                                    logger.fLog("학습 시작 버튼을 찾지 못함", Color.Red);
                                    logger.fLog("다시 학습 시작 버튼을 검색", Color.Red);
                                }
                                break;
                            }

                        case StudyState.StudyScene:
                            {
                                // 퀴즈상태 감지
                                result = cm.Capture(CaptureMachine.BMP_STATE_QUIZ);
                                if(result.accuracy >= Config.data.state_quiz_accuracy)
                                {
                                    state = StudyState.QuizScene;
                                    logger.fLog("퀴즈 등장", Color.Green);
                                    break;
                                }

                                // 완강상태 감지
                                result = cm.Capture(CaptureMachine.BMP_STATE_FULL);
                                if(result.accuracy >= Config.data.state_fullstudy_accuracy)
                                {
                                    state = StudyState.FullStudy;
                                    logger.fLog("수강시간을 모두 채움", Color.Green);
                                    break;
                                }

                                // 감지하지 못한경우
                                // 다시 감지 시작
                                break;
                            }

                        case StudyState.QuizScene:
                            {
                                CaptureResult button_o_result = cm.Capture(CaptureMachine.BMP_BUTTON_O);
                                CaptureResult button_x_result = cm.Capture(CaptureMachine.BMP_BUTTON_X);
                                if(button_o_result.accuracy >= Config.data.button_quiz_ox_accuracy)
                                {
                                    mouse_move_click(button_o_result.center_x, button_o_result.center_y);
                                    state = StudyState.PausedScene;
                                    logger.fLog("O 버튼으로 퀴즈 처리", Color.Green);
                                }
                                else if(button_x_result.accuracy >= Config.data.button_quiz_ox_accuracy)
                                {
                                    mouse_move_click(button_x_result.center_x, button_x_result.center_y);
                                    state = StudyState.PausedScene;
                                    logger.fLog("X 버튼으로 퀴즈 처리", Color.Green);
                                }
                                else
                                {
                                    logger.fLog("퀴즈 OX 버튼 검색 실패", Color.Red);
                                    logger.fLog("다시 퀴즈 OX 버튼을 검색", Color.Red);
                                }
                                break;
                            }

                        case StudyState.PausedScene:
                            {
                                result = cm.Capture(CaptureMachine.BMP_BUTTON_RESUME);
                                if(result.accuracy >= Config.data.button_resume_accuracy)
                                {
                                    mouse_move_click(result.center_x, result.center_y);
                                    state = StudyState.StudyScene;
                                    logger.fLog("학습 재개", Color.Green);
                                }
                                else
                                {
                                    logger.fLog("학습 진행하기 버튼 검색 실패", Color.Red);
                                    logger.fLog("다시 학습 진행하기 버튼을 검색", Color.Red);
                                }
                                break;
                            }

                        case StudyState.FullStudy:
                            {
                                CaptureResult button_tryexit1_result = cm.Capture(CaptureMachine.BMP_BUTTON_TRYEXIT1);
                                CaptureResult button_tryexit2_result = cm.Capture(CaptureMachine.BMP_BUTTON_TRYEXIT2);
                                if(button_tryexit1_result.accuracy >= Config.data.button_tryexit_accuracy)
                                {
                                    mouse_move_click(button_tryexit1_result.center_x, button_tryexit1_result.center_y);
                                    state = StudyState.ExitDialog;
                                    logger.fLog("학습 종료 개시(case1)", Color.Green);
                                }
                                else if (button_tryexit2_result.accuracy >= Config.data.button_tryexit_accuracy)
                                {
                                    mouse_move_click(button_tryexit2_result.center_x, button_tryexit2_result.center_y);
                                    state = StudyState.ExitDialog;
                                    logger.fLog("학습 종료 개시(case2)", Color.Green);
                                }
                                else
                                {
                                    logger.fLog("학습 종료(case1, case2) 버튼 검색 실패", Color.Red);
                                    logger.fLog("다시 학습 진행하기 버튼을 검색", Color.Red);
                                }
                                break;
                            }

                        case StudyState.ExitDialog:
                            {
                                CaptureResult button_ok1_result = cm.Capture(CaptureMachine.BMP_BUTTON_OK1);
                                CaptureResult button_ok2_result = cm.Capture(CaptureMachine.BMP_BUTTON_OK2);
                                if (button_ok1_result.accuracy >= Config.data.button_ok_accuracy)
                                {
                                    mouse_move_click(button_ok1_result.center_x, button_ok1_result.center_y);
                                    state = StudyState.ExitScene;
                                    logger.fLog("학습 종료 다이얼로그 처리(case1)", Color.Green);
                                }
                                else if (button_ok2_result.accuracy >= Config.data.button_ok_accuracy)
                                {
                                    mouse_move_click(button_ok2_result.center_x, button_ok2_result.center_y);
                                    state = StudyState.ExitScene;
                                    logger.fLog("학습 종료 다이얼로그 처리(case2)", Color.Green);
                                }
                                else
                                {
                                    logger.fLog("학습 종료 다이얼로그의 확인 버튼 검색 실패", Color.Red);
                                    logger.fLog("다시 확인 버튼을 검색", Color.Red);
                                }
                                break;
                            }

                        case StudyState.ExitScene:
                            {
                                CaptureResult button_exit1_result = cm.Capture(CaptureMachine.BMP_BUTTON_EXIT1);
                                CaptureResult button_exit2_result = cm.Capture(CaptureMachine.BMP_BUTTON_EXIT2);
                                if(button_exit1_result.accuracy >= Config.data.button_exit_accuracy)
                                {
                                    mouse_move_click(button_exit1_result.center_x, button_exit1_result.center_y);
                                    state = StudyState.SelectIndexTitle;
                                    logger.fLog($"{++completeCount}개 학습 완료(case1)", Color.Green);
                                }
                                else if(button_exit2_result.accuracy >= Config.data.button_exit_accuracy)
                                {
                                    mouse_move_click(button_exit1_result.center_x, button_exit1_result.center_y);
                                    state = StudyState.SelectIndexTitle;
                                    logger.fLog($"{++completeCount}개 학습 완료(case2)", Color.Green);
                                }
                                else
                                {
                                    logger.fLog("종료하기(case1, case2) 버튼 검색 실패", Color.Red);
                                    logger.fLog("다시 종료 버튼을 검색", Color.Red);
                                }
                                break;
                            }

                        default: break;
                    }
                }

                Thread.Sleep((int)(Config.data.tick));
            }
        }
    }
}
