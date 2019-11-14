using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;//要用到DllImport

using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

using Emgu.CV;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
namespace HeYe_ThirdWorkSpace
{
    class CameraTuYang
    {
        #region  "tuyang.dll"各函数接口定义
        //初始化camera--camera_init(true:触发模式, false:视频模式,激光功率:0--100,增益:0--100)
        [DllImport("tuyang.dll", EntryPoint = "camera_init", CallingConvention = CallingConvention.Cdecl)]
        public extern static int camera_init(bool isTrig, int power, int gain);//初始化camera


        //激光功率与增益调整--PowerGain(激光功率:0--100,增益:0--100,相机编号)
        [DllImport("tuyang.dll", EntryPoint = "PowerGain", CallingConvention = CallingConvention.Cdecl)]
        public extern static void PowerGain(int power, int gain, int num);//



        //获取彩色图像与三维图像--cameraFun:
        [DllImport("tuyang.dll", EntryPoint = "cameraFun", CallingConvention = CallingConvention.Cdecl)]
        public extern static void cameraFun([MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)]  byte[] color_temp, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)]  float[] p3d_temp, int num);

        //获取彩色图像--getColor:
        [DllImport("tuyang.dll", EntryPoint = "getColor", CallingConvention = CallingConvention.Cdecl)]
        public extern static void getColor([MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)]  byte[] color_temp, int num);
        //获取彩色图像--getColorDepth:
        [DllImport("tuyang.dll", EntryPoint = "getColorDepth", CallingConvention = CallingConvention.Cdecl)]
        public extern static void getColorDepth([MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)]  byte[] colorDepth_temp, int num);

        //获取三维图像--get3D:
        [DllImport("tuyang.dll", EntryPoint = "get3D", CallingConvention = CallingConvention.Cdecl)]
        public extern static void get3D([MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)]  float[] p3d_temp, int num);

        //获取三维图像--get3D:
        [DllImport("tuyang.dll", EntryPoint = "getFiltered3D", CallingConvention = CallingConvention.Cdecl)]
        public extern static void getFiltered3D([MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)]  float[] p3d_temp, int num);



        //软触发取图像--softTrigg:   前提是: camera_init(true)
        [DllImport("tuyang.dll", EntryPoint = "softTrigg", CallingConvention = CallingConvention.Cdecl)]
        public extern static void softTrigg(int num);



        //停止Camera--close_camera():
        [DllImport("tuyang.dll", EntryPoint = "close_camera", CallingConvention = CallingConvention.Cdecl)]
        public extern static bool close_camera();//停止Camera

        //获取已经连接相机之数目--cameraGetCnt(void):
        [DllImport("tuyang.dll", EntryPoint = "cameraGetCnt", CallingConvention = CallingConvention.Cdecl)]
        public extern static int cameraGetCnt();//

        //获取相机设备ID--char* cameraGetSN(int  num):
        [DllImport("tuyang.dll", EntryPoint = "cameraGetSN", CallingConvention = CallingConvention.Cdecl)]
        public extern static IntPtr cameraGetSN(int num);

        //获取深度校正宽度--int cameraGetDepthCalibIntrinsicWidth(int num);
        [DllImport("tuyang.dll", EntryPoint = "cameraGetDepthCalibIntrinsicWidth", CallingConvention = CallingConvention.Cdecl)]
        public extern static int cameraGetDepthCalibIntrinsicWidth(int num);

        //获取深度校正高度--int  cameraGetDepthCalibIntrinsicHeight(int num);
        [DllImport("tuyang.dll", EntryPoint = "cameraGetDepthCalibIntrinsicHeight", CallingConvention = CallingConvention.Cdecl)]
        public extern static int cameraGetDepthCalibIntrinsicHeight(int num);

        //获取深度校正内参--void cameraGetDepthCalibIntrinsic(int num, float* fIntrinsic);
        [DllImport("tuyang.dll", EntryPoint = "cameraGetDepthCalibIntrinsic", CallingConvention = CallingConvention.Cdecl)]
        public extern static void cameraGetDepthCalibIntrinsic(int num, float[] fIntrinsic);

        //获取深度校正外参--void cameraGetDepthCalibExtrinsic(int num, float* fExtrinsic);
        [DllImport("tuyang.dll", EntryPoint = "cameraGetDepthCalibExtrinsic", CallingConvention = CallingConvention.Cdecl)]
        public extern static void cameraGetDepthCalibExtrinsic(int num, float[] fExtrinsic);

        //获取深度校正矩阵--void cameraGetDepthCalibDistortion(int num, float* fDistortion);
        [DllImport("tuyang.dll", EntryPoint = "cameraGetDepthCalibDistortion", CallingConvention = CallingConvention.Cdecl)]
        public extern static void cameraGetDepthCalibDistortion(int num, float[] fDistortion);

        //获取彩色校正宽度--int cameraGetColorCalibIntrinsicWidth(int num);
        [DllImport("tuyang.dll", EntryPoint = "cameraGetColorCalibIntrinsicWidth", CallingConvention = CallingConvention.Cdecl)]
        public extern static int cameraGetColorCalibIntrinsicWidth(int num);

        //获取彩色校正高度--int cameraGetColorCalibIntrinsicHeight(int num);
        [DllImport("tuyang.dll", EntryPoint = "cameraGetColorCalibIntrinsicHeight", CallingConvention = CallingConvention.Cdecl)]
        public extern static int cameraGetColorCalibIntrinsicHeight(int num);

        //获取彩色校正内参--void cameraGetColorCalibIntrinsic(int num, float* fIntrinsic);
        [DllImport("tuyang.dll", EntryPoint = "cameraGetColorCalibIntrinsic", CallingConvention = CallingConvention.Cdecl)]
        public extern static void cameraGetColorCalibIntrinsic(int num, float[] fIntrinsic);

        //获取彩色校正外参--void cameraGetColorCalibExtrinsic(int num, float* fExtrinsic);
        [DllImport("tuyang.dll", EntryPoint = "cameraGetColorCalibExtrinsic", CallingConvention = CallingConvention.Cdecl)]
        public extern static void cameraGetColorCalibExtrinsic(int num, float[] fExtrinsic);

        //获取彩色校正矩阵--void cameraGetColorCalibDistortion(int num, float* fDistortion);
        [DllImport("tuyang.dll", EntryPoint = "cameraGetColorCalibDistortion", CallingConvention = CallingConvention.Cdecl)]
        public extern static void cameraGetColorCalibDistortion(int num, float[] fDistortion);

        //获取校正后彩色图像--cameraGetUndistortRGBImage(void* pRGB, int width, int height, int num, void* pOut):
        [DllImport("tuyang.dll", EntryPoint = "cameraGetUndistortRGBImage", CallingConvention = CallingConvention.Cdecl)]
        public extern static void cameraGetUndistortRGBImage(byte[] pRGB, int width, int height, int num, byte[] pOut);//length=3*width*height

        #endregion
       

        /// <summary>
        /// //显示彩色图像:
        /// </summary>
        /// <param name="mycolor"></param>
        /// <param name="ptb"></param>
       public static  void display_color(byte[] mycolor, PictureBox ptb)
        {
            //一,内存复制方式:
            //Bitmap myimg = new Bitmap(GLB.BUFW, GLB.BUFH, PixelFormat.Format24bppRgb);
            //BitmapData mydata;
            //mydata = myimg.LockBits(new Rectangle(0, 0, myimg.Width, myimg.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            //System.Runtime.InteropServices.Marshal.Copy(mycolor, 0, mydata.Scan0, mycolor.Length);
            //myimg.UnlockBits(mydata);
            //pictureBox1.Image = myimg;
            //二.单独处理各字节的方式:
            for (int i = 0; i < GLB.BUFH; i++)
            {
                for (int j = 0; j < GLB.BUFW; j++)
                {
                    GLB.MyFrame.Data[i, j, 2] = mycolor[(i * GLB.BUFW + j) * 3 + 2];
                    GLB.MyFrame.Data[i, j, 1] = mycolor[(i * GLB.BUFW + j) * 3 + 1];
                    GLB.MyFrame.Data[i, j, 0] = mycolor[(i * GLB.BUFW + j) * 3 + 0];
                }
            }
            ptb.Image = GLB.MyFrame.ToBitmap();
        }

        /// <summary>
        ///  //显示校正后彩色图像:
        /// </summary>
        /// <param name="mycolor"></param>
        /// <param name="ptb"></param>
        public static void display_color2(byte[] mycolor, PictureBox ptb)
        {
            //一,内存复制方式:
            Bitmap myimg = new Bitmap(GLB.BUFW, GLB.BUFH, PixelFormat.Format24bppRgb);
            BitmapData mydata;
            mydata = myimg.LockBits(new Rectangle(0, 0, myimg.Width, myimg.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            System.Runtime.InteropServices.Marshal.Copy(mycolor, 0, mydata.Scan0, mycolor.Length);
            myimg.UnlockBits(mydata);
            ptb.Image = myimg;
            //二。单独处理各字节的方式:
            //for (int i = 0; i < GLB.BUFH; i++)
            //{
            //    for (int j = 0; j < GLB.BUFW; j++)
            //    {
            //        GLB.MyFrame.Data[i, j, 2] = mycolor[(i * GLB.BUFW + j) * 3 + 2];
            //        GLB.MyFrame.Data[i, j, 1] = mycolor[(i * GLB.BUFW + j) * 3 + 1];
            //        GLB.MyFrame.Data[i, j, 0] = mycolor[(i * GLB.BUFW + j) * 3 + 0];
            //    }
            //}
            //pictureBox1.Image = GLB.MyFrame.ToBitmap();           
        }
        /// <summary>
        /// 显示三维数据
        /// </summary>
        /// <param name="cameraIndex"></param>
        /// <param name="myp3d"></param>
        /// <param name="ptb"></param>
        public static void display_point_3d(int cameraIndex, float[] myp3d, PictureBox ptb)
        {
            GLB.TitleStr = "";
            float visibaleDistance = 0;//可视距离
            float dis_temp=0;
            int d = 2;
            if (cameraIndex == 0) visibaleDistance = 2436;
            //else if (cameraIndex == 1) visibaleDistance = 2600;
            for (int i = d; i < GLB.BUFH - d; i += d)
            {
                for (int j = d; j < GLB.BUFW - d; j += d)
                {
                    float z_center = myp3d[(i * GLB.BUFW + j) * 3 + 2];
                    if (z_center > 0 && z_center < visibaleDistance)//小于托盘的深度
                    {
                        float v1 = myp3d[3 * ((i - d) * GLB.BUFW + j - d) + 2];
                        float v2 = myp3d[3 * ((i - d) * GLB.BUFW + j) + 2];
                        float v3 = myp3d[3 * ((i - d) * GLB.BUFW + j + d) + 2];
                        float v4 = myp3d[3 * (i * GLB.BUFW + j - d) + 2];

                        float v6 = myp3d[3 * (i * GLB.BUFW + j + d) + 2];
                        float v7 = myp3d[3 * ((i + d) * GLB.BUFW + j - d) + 2];
                        float v8 = myp3d[3 * ((i + d) * GLB.BUFW + j) + 2];
                        float v9 = myp3d[3 * ((i + d) * GLB.BUFW + j + d) + 2];

                        if (double.IsNaN(v1) || double.IsNaN(v2) || double.IsNaN(v3) || double.IsNaN(v4)
                           || double.IsNaN(v6) || double.IsNaN(v7) || double.IsNaN(v8) || double.IsNaN(v9)) dis_temp = 0;


                        dis_temp = Math.Abs(v1 - z_center) + Math.Abs(v2 - z_center) + Math.Abs(v3 - z_center) + Math.Abs(v4 - z_center)
                           + Math.Abs(v6 - z_center) + Math.Abs(v7 - z_center) + Math.Abs(v8 - z_center) + Math.Abs(v9 - z_center);//双线斜率绝对值之和
                        dis_temp = dis_temp < 15 ? z_center : 0;
                    }
                    else
                    {
                        dis_temp = 0;
                    }
                    //伪彩色效果: 
                    for (int dy = -d / 2; dy < d / 2; dy++)
                    {
                        for (int dx = -d / 2; dx < d / 2; dx++)
                        {
                            if (dis_temp == 0)
                            {
                                GLB.MyFrame.Data[i + dx, j + dy, 0] = (byte)(dis_temp);   //B
                                GLB.MyFrame.Data[i + dx, j + dy, 1] = (byte)(dis_temp);   //G
                                GLB.MyFrame.Data[i + dx, j + dy, 2] = (byte)(dis_temp);   //R
                            }
                            else
                            {
                                GLB.MyFrame.Data[i + dx, j + dy, 0] = (byte)(128 + 127 * Math.Cos(dis_temp / 13f));
                                GLB.MyFrame.Data[i + dx, j + dy, 1] = (byte)(127 - 127 * Math.Sin(dis_temp / 17f));
                                GLB.MyFrame.Data[i + dx, j + dy, 2] = (byte)(128 + 127 * Math.Sin(dis_temp / 23f));
                            }
                        }
                    }
                }
            }
            DealWithImage.getContours(myp3d);

            //RotatedRect myrect = new RotatedRect(new PointF((float)GLB.BUFW / 2, (float)GLB.BUFH / 2), new Size(8, 8), 0);
            //CvInvoke.Ellipse(GLB.frame, myrect, new MCvScalar(255, 0, 0), 5);//在角上一个小圆
            if (GLB.MyFrame.Data != null) ptb.Image = GLB.MyFrame.ToBitmap();
        }
        /// <summary>
        /// 显示真三维图像:
        /// </summary>
        /// <param name="cameraIndex"></param>
        /// <param name="point_3d"></param>
        /// <param name="ptb"></param>
        public static void display_point_3d_1(int cameraIndex ,float[] point_3d, PictureBox ptb)
        {

            float ff;
            float visibaleDistance=0;//可视距离
            if (cameraIndex == 0) visibaleDistance = 2600;//2300
            else if (cameraIndex == 1) visibaleDistance = 3750;
            for (int i = 0; i < GLB.BUFH; i++)
            {
                for (int j = 0; j < GLB.BUFW; j++)
                {
                   
                    ff = (float)(point_3d[(i * GLB.BUFW + j) * 3 + 2]);
                    if(ff< visibaleDistance && ff>2000)
                    {
                        //伪彩色表示深度:
                        GLB.MyFrame.Data[i, j, 0] = (byte)(128 + 127 * Math.Cos(ff / 13f));
                        GLB.MyFrame.Data[i, j, 1] = (byte)(127 - 127 * Math.Sin(ff / 17f));
                        GLB.MyFrame.Data[i, j, 2] = (byte)(128 + 127 * Math.Sin(ff / 23f));
                        //GLB.MyFrame.Data[i, j, 0] = 255;
                        //GLB.MyFrame.Data[i, j, 1] = 255;
                        //GLB.MyFrame.Data[i, j, 2] = 255;
                    }
                    else
                    {
                        GLB.MyFrame.Data[i, j, 0] = 0;
                        GLB.MyFrame.Data[i, j, 1] = 0;
                        GLB.MyFrame.Data[i, j, 2] = 0;
                    }

                }
            }
            DealWithImage.getContours(point_3d);
            ptb.Image = GLB.MyFrame.ToBitmap();

            //###########################测试###############################
            //GLB.MyFrame = GLB.MyFrame.SmoothBlur(GLB.BUFW, GLB.BUFH);//全屏模糊   
            //src = src.SmoothGaussian(25);//25*25区块,高斯模糊
            //GLB.MyFrame = GLB.MyFrame.SmoothMedian(17);//17*17区块,均值模糊化
            //var gray = GLB.MyFrame.Convert<Gray, Byte>(); //灰度化
            //CvInvoke.AdaptiveThreshold(gray, gray, 255, AdaptiveThresholdType.MeanC, ThresholdType.Binary, 27, 1);
            //ptb.Image = gray.ToBitmap();

        }
        /// <summary>
        /// 显示深度叠加彩色
        /// </summary>
        /// <param name="mycolorDepth"></param>
        /// <param name="point_3d"></param>
        /// <param name="ptb"></param>
        public static void display_colorDepth(byte[] mycolorDepth, float[] point_3d, PictureBox ptb)
        {
            for (int i = 0; i < GLB.BUFH; i++)
            {
                for (int j = 0; j < GLB.BUFW; j++)
                {
                    //GLB.MyFrame.Data[i, j, 2] = mycolorDepth[(i * GLB.BUFW + j) * 3 + 2];
                    //GLB.MyFrame.Data[i, j, 1] = mycolorDepth[(i * GLB.BUFW + j) * 3 + 1];
                    //GLB.MyFrame.Data[i, j, 0] = mycolorDepth[(i * GLB.BUFW + j) * 3 + 0];
                    float ff = (float)(point_3d[(i * GLB.BUFW + j) * 3 + 2]);
                    //if (ff <= hScrollBar1.Value)
                    if (ff < 2450 && ff > 1800)
                    {
                        GLB.MyFrame.Data[i, j, 2] = mycolorDepth[(i * GLB.BUFW + j) * 3 + 2];
                        GLB.MyFrame.Data[i, j, 1] = mycolorDepth[(i * GLB.BUFW + j) * 3 + 1];
                        GLB.MyFrame.Data[i, j, 0] = mycolorDepth[(i * GLB.BUFW + j) * 3 + 0];
                    }
                    else
                    {
                        GLB.MyFrame.Data[i, j, 0] = 0;
                        GLB.MyFrame.Data[i, j, 1] = 0;
                        GLB.MyFrame.Data[i, j, 2] = 0;
                    }

                }
            }
            //var gray = GLB.MyFrame.Convert<Gray, Byte>(); //灰度化
            //CvInvoke.AdaptiveThreshold(gray, gray, 255, AdaptiveThresholdType.MeanC, ThresholdType.BinaryInv, 29, 1);
            //ptb.Image = gray.ToBitmap();
            ptb.Image = GLB.MyFrame.ToBitmap();
        }

    }
}
