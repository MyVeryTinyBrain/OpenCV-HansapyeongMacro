#define SINGLE_COLOR_LOG

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.IO;
using System.Threading;
using System.Drawing.Imaging;
using OpenCvSharp;

namespace HansapyeongMacroOpenCV
{
    struct CaptureResult
    {
        public OpenCvSharp.Point    point;
        public double               accuracy;
        public int                  image_width;
        public int                  image_height;
        public int                  center_x;
        public int                  center_y;

        public override string ToString()
        {
            string s = string.Empty;
            s += string.Format($"Point: {point.X}, {point.Y}\r\n");
            s += string.Format($"Accuracy: {accuracy}\r\n");
            s += string.Format($"Image Size: {image_width}, {image_height}\r\n");
            s += string.Format($"Center Point: {center_x}, {center_y}");
            return s;
        }
    }

    class CaptureMachine
    {
        private const string FOLDER_PATH =          @"\images\";
        private const string PATH_BOX_COMPLETE =    "box_complete.png";
        private const string PATH_BOX_TIMELESS =    "box_timeless.png";
        private const string PATH_BOX_UNCOMPLETE =  "box_uncomplete.png";
        private const string PATH_BUTTON_BEGIN1 =   "button_begin1.png";
        private const string PATH_BUTTON_BEGIN2 =   "button_begin2.png";
        private const string PATH_BUTTON_EXIT1 =    "button_exit1.png";
        private const string PATH_BUTTON_EXIT2 =    "button_exit2.png";
        private const string PATH_BUTTON_O =        "button_O.png";
        private const string PATH_BUTTON_OK1 =      "button_ok1.png";
        private const string PATH_BUTTON_OK2 =      "button_ok2.png";
        private const string PATH_BUTTON_RESUME =   "button_resume.png";
        private const string PATH_BUTTON_TRYEXIT1 = "button_tryexit1.png";
        private const string PATH_BUTTON_TRYEXIT2 = "button_tryexit2.png";
        private const string PATH_BUTTON_X =        "button_X.png";
        private const string PATH_DIALOG_EIXT =     "dialog_exit.png";
        private const string PATH_STATE_FULL =      "state_full.png";
        private const string PATH_STATE_QUIZ =      "state_quiz.png";

        private static readonly Bitmap[] BMP_ARRAY = {
            BMP_BOX_COMPLETE, BMP_BOX_TIMELESS, BMP_BOX_UNCOMPLETE,
            BMP_BUTTON_BEGIN1, BMP_BUTTON_BEGIN2, BMP_BUTTON_EXIT1, BMP_BUTTON_EXIT2, BMP_BUTTON_O, BMP_BUTTON_OK1, BMP_BUTTON_OK2, BMP_BUTTON_RESUME, BMP_BUTTON_TRYEXIT1, BMP_BUTTON_TRYEXIT2, BMP_BUTTON_X,
            BMP_DIALOG_EIXT, BMP_STATE_FULL, BMP_STATE_QUIZ
        };
        public static readonly Bitmap BMP_BOX_COMPLETE = LoadImage(Application.StartupPath + FOLDER_PATH + PATH_BOX_COMPLETE);
        public static readonly Bitmap BMP_BOX_TIMELESS = LoadImage(Application.StartupPath + FOLDER_PATH + PATH_BOX_TIMELESS);
        public static readonly Bitmap BMP_BOX_UNCOMPLETE = LoadImage(Application.StartupPath + FOLDER_PATH + PATH_BOX_UNCOMPLETE);
        public static readonly Bitmap BMP_BUTTON_BEGIN1 = LoadImage(Application.StartupPath + FOLDER_PATH + PATH_BUTTON_BEGIN1);
        public static readonly Bitmap BMP_BUTTON_BEGIN2 = LoadImage(Application.StartupPath + FOLDER_PATH + PATH_BUTTON_BEGIN2);
        public static readonly Bitmap BMP_BUTTON_EXIT1 = LoadImage(Application.StartupPath + FOLDER_PATH + PATH_BUTTON_EXIT1);
        public static readonly Bitmap BMP_BUTTON_EXIT2 = LoadImage(Application.StartupPath + FOLDER_PATH + PATH_BUTTON_EXIT2);
        public static readonly Bitmap BMP_BUTTON_O = LoadImage(Application.StartupPath + FOLDER_PATH + PATH_BUTTON_O);
        public static readonly Bitmap BMP_BUTTON_OK1 = LoadImage(Application.StartupPath + FOLDER_PATH + PATH_BUTTON_OK1);
        public static readonly Bitmap BMP_BUTTON_OK2 = LoadImage(Application.StartupPath + FOLDER_PATH + PATH_BUTTON_OK2);
        public static readonly Bitmap BMP_BUTTON_RESUME = LoadImage(Application.StartupPath + FOLDER_PATH + PATH_BUTTON_RESUME);
        public static readonly Bitmap BMP_BUTTON_TRYEXIT1 = LoadImage(Application.StartupPath + FOLDER_PATH + PATH_BUTTON_TRYEXIT1);
        public static readonly Bitmap BMP_BUTTON_TRYEXIT2 = LoadImage(Application.StartupPath + FOLDER_PATH + PATH_BUTTON_TRYEXIT2);
        public static readonly Bitmap BMP_BUTTON_X = LoadImage(Application.StartupPath + FOLDER_PATH + PATH_BUTTON_X);
        public static readonly Bitmap BMP_DIALOG_EIXT = LoadImage(Application.StartupPath + FOLDER_PATH + PATH_DIALOG_EIXT);
        public static readonly Bitmap BMP_STATE_FULL = LoadImage(Application.StartupPath + FOLDER_PATH + PATH_STATE_FULL);
        public static readonly Bitmap BMP_STATE_QUIZ = LoadImage(Application.StartupPath + FOLDER_PATH + PATH_STATE_QUIZ);

        public CaptureMachine(out bool complete)
        {
            complete = false;
            foreach (Bitmap bitmap in BMP_ARRAY) complete = (bitmap == null ? false : true);
        }

        public CaptureResult Capture(Bitmap bitmap) 
        {
            Mat findMat = OpenCvSharp.Extensions.BitmapConverter.ToMat(bitmap);

            Bitmap screenBitmap = ScreenCapture();
            Mat screenMat = OpenCvSharp.Extensions.BitmapConverter.ToMat(screenBitmap);
            
            using (Mat res = screenMat.MatchTemplate(findMat, TemplateMatchModes.CCoeffNormed))
            {
                //찾은 이미지의 유사도를 담을 더블형 최대 최소 값을 선언합니다.
                double minval, maxval = 0;
                //찾은 이미지의 위치를 담을 포인트형을 선업합니다.
                OpenCvSharp.Point minloc, maxloc;
                //찾은 이미지의 유사도 및 위치 값을 받습니다. 
                Cv2.MinMaxLoc(res, out minval, out maxval, out minloc, out maxloc);
                //LogPanel.Log($"{maxval * 100f}%");

                CaptureResult result = new CaptureResult();
                result.point = maxloc;
                result.accuracy = maxval;
                result.image_width = bitmap.Width;
                result.image_height = bitmap.Height;
                result.center_x = (int)(maxloc.X + bitmap.Width * 0.5f);
                result.center_y = (int)(maxloc.Y + bitmap.Height * 0.5f);

                screenBitmap.Dispose();
                findMat.Dispose();
                screenMat.Dispose();

                return result;
            }
        }

        private static Bitmap LoadImage(string path)
        {
            bool exists = File.Exists(path);
#if SINGLE_COLOR_LOG
            LogPanel.LogNonReturn("Load[", Color.Black);
            LogPanel.LogNonReturn(exists, exists == false ? Color.Red : Color.Green);
            LogPanel.Log($"]: {path}\r\n");
#else
            LogPanel.Log($"Load[{exists}]: {path}\r\n", exists == false ? Color.Red : Color.Green);
#endif
            if (!exists) return null;
            else return new Bitmap(path);
        }

        private Bitmap ScreenCapture()
        {
            // 주화면의 크기 정보 읽기
            // 2nd screen = Screen.AllScreens[1]
            return ScreenCapture(Screen.PrimaryScreen.Bounds);
        }

        private Bitmap ScreenCapture(System.Drawing.Rectangle area)
        {
            Rectangle rect = area;

            // 픽셀 포맷 정보 얻기 (Optional)
            int bitsPerPixel = Screen.PrimaryScreen.BitsPerPixel;
            PixelFormat pixelFormat = PixelFormat.Format32bppArgb;
            if (bitsPerPixel <= 16)
            {
                pixelFormat = PixelFormat.Format16bppRgb565;
            }
            if (bitsPerPixel == 24)
            {
                pixelFormat = PixelFormat.Format24bppRgb;
            }

            // 화면 크기만큼의 Bitmap 생성
            Bitmap bmp = new Bitmap(rect.Width, rect.Height, pixelFormat);

            // Bitmap 이미지 변경을 위해 Graphics 객체 생성
            using (Graphics gr = Graphics.FromImage(bmp))
            {
                // 화면을 그대로 카피해서 Bitmap 메모리에 저장
                gr.CopyFromScreen(rect.Left, rect.Top, 0, 0, rect.Size);
            }
            return bmp;
        }
    }
}
