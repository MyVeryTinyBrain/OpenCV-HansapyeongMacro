using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HansapyeongMacroOpenCV
{
    public enum StudyState
    {
        /// <summary>
        /// 인덱스 선택 대기상태.
        /// </summary>
        SelectIndexTitle,

        /// <summary>
        /// 공부 창이 처음 띄워진 상태.
        /// </summary>
        WaitForStartStudy,

        /// <summary>
        /// 공부 중 상태.<para>
        /// 퀴즈와 퍼센트 검사는 여기에서 이루어짐.</para>
        /// </summary>
        StudyScene,

        /// <summary>
        /// 퀴즈 출제 상태.
        /// </summary>
        QuizScene,

        /// <summary>
        /// 퀴즈로 인하여 정지된 상태.<para>
        /// 학습 진행하기 버튼을 눌러 재개해야 하는 상태.</para>
        /// </summary>
        PausedScene,

        /// <summary>
        /// 수강시간 100%를 채운 상태.
        /// </summary>
        FullStudy,

        /// <summary>
        /// 종료 다이얼로그가 띄워진 상태.
        /// </summary>
        ExitDialog,

        /// <summary>
        /// 종료 씬으로 바뀐 상태.
        /// </summary>
        ExitScene,  
    }
}