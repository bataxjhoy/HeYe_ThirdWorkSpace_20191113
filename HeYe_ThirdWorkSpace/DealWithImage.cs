using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.CV.Cvb;
using System.Drawing;
using System.Windows.Forms;
namespace HeYe_ThirdWorkSpace
{
    class DealWithImage
    {
        /// <summary>
        /// 图漾粗略找合适的轮廓
        /// </summary>
        /// <param name="point_3d">三维点</param>
        public static void getContours(float[] point_3d)
        {
            GLB.TitleStr = "";
            int AREA = GLB.BUFW * GLB.BUFH;//总面积
            var gray = GLB.MyFrame.Convert<Gray, Byte>(); //灰度化

            Emgu.CV.Cvb.CvBlobs myBlobs = new Emgu.CV.Cvb.CvBlobs();
            Emgu.CV.Cvb.CvBlobDetector bd = new Emgu.CV.Cvb.CvBlobDetector();
            uint n = bd.Detect(gray, myBlobs);//发现区块

            ////遍历各区块:
            for (uint i = 1; i <= myBlobs.Count; i++)
            {
                int area = myBlobs[i].Area;//获取面积
                RotatedRect rect = CvInvoke.MinAreaRect(new VectorOfPoint(myBlobs[i].GetContour()));//最小矩形
                float width = rect.Size.Width;//像素宽
                float height = rect.Size.Height;//像素长
                if (height < width)
                {
                    float temp = height;
                    height = width;
                    width = temp;
                }
                float H2W = height / width;
                if (area > 0.02 * AREA && area < 0.75 * AREA && H2W > 1 && H2W < 2)//通过面积 长宽比 初略筛选
                {
                    if (getProduceInfo(myBlobs[i], point_3d) == true)
                    {
                        if (ProduceMacth() == true)//匹配成功
                        {
                            //////#########################################队列求均值--获取中心坐标#################################################
                            //GLB.avgCameraPoint3.Enqueue(new Point3(GLB.camera_device_point.X, GLB.camera_device_point.Y, GLB.camera_device_point.Z));
                            //if (GLB.avgCameraPoint3.Count > 5)
                            //{
                            //    GLB.avgCameraPoint3.Dequeue();
                            //}
                            //else
                            //{
                            //    return ;
                            //}
                            //GLB.camera_device_point.Z = (int)GLB.avgCameraPoint3.Average(o => o.Z);//中心点的深度//Z
                            //GLB.camera_device_point.Y = (int)GLB.avgCameraPoint3.Average(o => o.Y);//Y
                            //GLB.camera_device_point.X = (int)GLB.avgCameraPoint3.Average(o => o.X);//X
                            //RotatedRect boxCenter = new RotatedRect(new PointF((float )GLB.obj.jd.Average(o => o.X), (float)GLB.obj.jd.Average(o => o.Y)), new Size(8, 8), 0);
                            //CvInvoke.Ellipse(GLB.MyFrame, boxCenter, new MCvScalar(255, 0, 0), 4);//在中心画一个小圆
                            //CvInvoke.PutText(GLB.MyFrame, "x:" + (float)GLB.obj.jd.Average(o => o.X) + "y:" + (float)GLB.obj.jd.Average(o => o.Y) + "XC=" + GLB.obj.xCenter + "YC=" + GLB.obj.yCenter + "Depth=" + GLB.obj.Depth, new System.Drawing.Point((int)GLB.obj.jd.Average(o => o.X) - 176, (int)GLB.obj.jd.Average(o => o.Y) + 25), Emgu.CV.CvEnum.FontFace.HersheyDuplex, .75, new MCvScalar(255, 255, 255), 2);//深度显示

                            ////////队列求均值
                            //GLB.avgAngle.Enqueue((float)GLB.obj.Angle);
                            //if (GLB.avgAngle.Count > 5)
                            //{
                            //    GLB.avgAngle.Dequeue();
                            //}
                            //else
                            //{
                            //    return ;
                            //}
                            //GLB.obj.Angle = GLB.avgAngle.Average();//旋转角
                            //CvInvoke.PutText(GLB.MyFrame, "Angl=" + GLB.obj.Angle, new System.Drawing.Point((int)GLB.obj.jd[3].X, (int)GLB.obj.jd[3].Y), Emgu.CV.CvEnum.FontFace.HersheyDuplex, .75, new MCvScalar(0, 0, 255), 2);


                            ////#########################################坐标换算#################################################
                            GLB.robot_device_point.X = GLB.MatTuYangCam[0] * GLB.camera_device_point.X + GLB.MatTuYangCam[1] * GLB.camera_device_point.Y + GLB.MatTuYangCam[2];
                            GLB.robot_device_point.Y = GLB.MatTuYangCam[3] * GLB.camera_device_point.X + GLB.MatTuYangCam[4] * GLB.camera_device_point.Y + GLB.MatTuYangCam[5];
                            GLB.robot_device_point.Z = 2818 - GLB.camera_device_point.Z;
                            GLB.device_angl += -2.6f;//相机与机器人夹角2.6度
                            GLB.device_angl = (float)(GLB.device_angl * Math.PI / 180f);
                            //限定范围
                            if (GLB.robot_device_point.X < -600 || GLB.robot_device_point.X > 600 ||
                                GLB.robot_device_point.Y < -2200 || GLB.robot_device_point.Y > -800 ||
                                GLB.robot_device_point.Z < 280 || GLB.robot_device_point.Z > 1100)
                            {
                                GLB.Match_success = false;
                                GLB.TitleStr += "，但是超出范围";
                            }
                            else GLB.Match_success = true;
                        }
                        else
                        {
                            GLB.Match_success = false;
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 图漾获取产品特征及位置
        /// </summary>
        /// <param name="myblob"></param>
        /// <param name="point_3d"></param>
        /// <returns></returns>
        public static bool getProduceInfo(Emgu.CV.Cvb.CvBlob myblob, float[] point_3d)
        {
            //#########################################【1】,获取最小外接矩形中心点下标#################################################           
            Point[] ctr = myblob.GetContour();//获取轮廓
            RotatedRect rect = CvInvoke.MinAreaRect(new VectorOfPoint(ctr));//最小矩形
            PointF[] pt = CvInvoke.BoxPoints(rect);//最小外接矩形四个角点
            PointF po = rect.Center;//最小外接矩形中心
            int xc = (int)po.X;//最小外接矩形中心X坐标
            int yc = (int)po.Y;//最小外接矩形中心Y坐标
            //#########################################【2】,绘制外接最小矩形(紧贴连通域):#################################################
            for (int i = 0; i < 4; ++i)
            {
                Point p1 = new Point((int)pt[i].X, (int)pt[i].Y);
                //GLB.obj.jd.Add(p1);//角点存下备画轨迹用
                Point p2 = new Point((int)pt[(i + 1) % 4].X, (int)pt[(i + 1) % 4].Y);
                CvInvoke.Line(GLB.MyFrame, p1, p2, new MCvScalar(0, 0, 255), 2);
            }

            //#########################################【3】真实角点):#################################################
            List<Point3> pointReal = new List<Point3>();
            for (int i = 0; i < 4; ++i)
            {
                Point3 Ang_temp = new Point3(0, 0, 0);//0.8视场角点坐标
                Ang_temp.X = xc + 0.8 * (pt[i].X - xc);
                Ang_temp.Y = yc + 0.8 * (pt[i].Y - yc);

                if (Ang_temp.Y < 0 || Ang_temp.Y > 960 - 2 || Ang_temp.X < 0 || Ang_temp.X > 1280 - 2) return false;///////无效角点

                Point3 Ang_point = new Point3(0, 0, 0);//0.8角点坐标               
                Ang_point.X = point_3d[((int)Ang_temp.Y * GLB.BUFW + (int)Ang_temp.X) * 3 + 0];
                Ang_point.Y = point_3d[((int)Ang_temp.Y * GLB.BUFW + (int)Ang_temp.X) * 3 + 1];
                Ang_point.Z = point_3d[((int)Ang_temp.Y * GLB.BUFW + (int)Ang_temp.X) * 3 + 2];
                if (double.IsNaN(Ang_point.Z)) return false;///////////////////////////////////////////////////////////无效角点

                pointReal.Add(Ang_point);
                RotatedRect myrect = new RotatedRect(new PointF((float)Ang_temp.X, (float)Ang_temp.Y), new Size(8, 8), 0);
                CvInvoke.Ellipse(GLB.MyFrame, myrect, new MCvScalar(255, 0, 0), 2);//在角上一个小圆               
            }
            //GLB.obj.Depth = (int)pointReal.Average(o => o.Z);//中心点的深度//Z
            //GLB.obj.yCenter = (int)pointReal.Average(o => o.Y);//Y
            //GLB.obj.xCenter = (int)pointReal.Average(o => o.X); ;//X
            ////#########################################队列求均值--获取中心坐标#################################################
            GLB.avgCameraPoint3.Enqueue(new Point3(pointReal.Average(o => o.X), pointReal.Average(o => o.Y), (int)pointReal.Average(o => o.Z)));
            if (GLB.avgCameraPoint3.Count > 5)
            {
                GLB.avgCameraPoint3.Dequeue();
            }
            else
            {
                return false;
            }
            GLB.obj.Depth = (int)GLB.avgCameraPoint3.Average(o => o.Z);//中心点的深度//Z
            GLB.obj.yCenter = (int)GLB.avgCameraPoint3.Average(o => o.Y);//Y
            GLB.obj.xCenter = (int)GLB.avgCameraPoint3.Average(o => o.X);//X
            //#########################################获取质心#################################################
            //PointF gravity = myblob.Centroid;
            //int gravity_x = (int)gravity.X;
            //int gravity_y = (int)gravity.Y;
            //if (double.IsNaN(point_3d[(gravity_y * GLB.BUFW + gravity_x) * 3 + 2])) return false;/////////////////////空值不执行 
            //GLB.obj.Depth = (int)point_3d[(gravity_y * GLB.BUFW + gravity_x) * 3 + 2];//Z
            //GLB.obj.yCenter = (int)point_3d[(gravity_y * GLB.BUFW + gravity_x) * 3 + 1];//Y
            //GLB.obj.xCenter = (int)point_3d[(gravity_y * GLB.BUFW + gravity_x) * 3 + 0];//X

            //######################################### 显示本区块中心[画圆]:#################################################
            RotatedRect boxCenter = new RotatedRect(new PointF(xc, yc), new Size(8, 8), 0);
            CvInvoke.Ellipse(GLB.MyFrame, boxCenter, new MCvScalar(255, 0, 0), 4);//在中心画一个小圆
            CvInvoke.PutText(GLB.MyFrame, "x:" + xc + "y:" + yc + "XC=" + GLB.obj.xCenter + "YC=" + GLB.obj.yCenter + "Depth=" + GLB.obj.Depth, new System.Drawing.Point(xc - 176, yc + 25), Emgu.CV.CvEnum.FontFace.HersheyDuplex, .75, new MCvScalar(255, 255, 255), 2);//深度显示

            //#########################################【4】真实的长轴,短轴,倾角计算:#################################################
            double axisLong = 1.25 * Math.Sqrt(Math.Pow(pointReal[1].X - pointReal[0].X, 2) + Math.Pow(pointReal[1].Y - pointReal[0].Y, 2) + Math.Pow(pointReal[1].Z - pointReal[0].Z, 2));
            double axisShort = 1.25 * Math.Sqrt(Math.Pow(pointReal[2].X - pointReal[1].X, 2) + Math.Pow(pointReal[2].Y - pointReal[1].Y, 2) + Math.Pow(pointReal[2].Z - pointReal[1].Z, 2));
           
            if (axisShort > axisLong)
            {
                double temp = axisLong;
                axisLong = axisShort;
                axisShort = temp;
            }
            double Angl = rect.Angle;//矩形框角度 
             // Angl *= 180d / Math.PI; //换算成角度制
            if (Math.Abs(Angl) > 45) Angl = Angl + 90;


            if (Angl >= 90)//控制旋转范围
            {
                Angl -= 180;
            }
            if (Angl <= -90)
            {
                Angl += 180;
            }
            //GLB.obj.Angle = Angl;//旋转角
            ////队列求均值
            GLB.avgAngle.Enqueue((float)Angl);
            if (GLB.avgAngle.Count > 5)
            {
                GLB.avgAngle.Dequeue();
            }
            else
            {
                return false;
            }
            GLB.obj.Angle = GLB.avgAngle.Average();//旋转角
           

            GLB.obj.axisLong = axisLong;//长轴
            GLB.obj.axisShort = axisShort;//短轴
            GLB.obj.L2S = axisLong / axisShort;//长短轴之比;
            GLB.obj.realArea = axisLong * axisShort; //估算的物件尺寸

            //像尺寸显示:
            CvInvoke.PutText(GLB.MyFrame, "Lr=" + (int)axisLong + ",Sr=" + (int)axisShort, new System.Drawing.Point((int)pt[2].X, (int)pt[2].Y), Emgu.CV.CvEnum.FontFace.HersheyDuplex, .75, new MCvScalar(0, 0, 255), 2);
            CvInvoke.PutText(GLB.MyFrame, "Angl=" + Angl, new System.Drawing.Point((int)pt[3].X, (int)pt[3].Y), Emgu.CV.CvEnum.FontFace.HersheyDuplex, .75, new MCvScalar(0, 0, 255), 2);
            return true;
        }

        //######################################################映美金#####################################################################
        /// <summary>
        /// 映美金粗略找合适的轮廓
        /// </summary>
        public static void getContoursForYMJ(Image<Gray, byte> garyImage,PictureBox ptbDisplay)
        {
            GLB.TitleStr = "";
            int AREA = TisCamera.width * TisCamera.height;//总面积
            Image<Gray, byte> dnc = new Image<Gray, byte>(TisCamera.width, TisCamera.height);
            VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint(2000);//区块集合

            CvInvoke.Threshold(garyImage, garyImage, 120, 255, ThresholdType.Otsu);
            //ptbDisplay.Image = garyImage.ToBitmap();//显示映美金图像

            CvInvoke.BoxFilter(garyImage, garyImage, Emgu.CV.CvEnum.DepthType.Cv8U, new Size(3, 3), new Point(-1, -1));//方框滤波
            CvInvoke.FindContours(garyImage, contours, dnc, RetrType.Ccomp, ChainApproxMethod.ChainApproxSimple);//轮廓集合
            List<VectorOfPoint> myContours = new List<VectorOfPoint>();//序号，轮廓
            for (int k = 0; k < contours.Size; k++)
            {
                double area = CvInvoke.ContourArea(contours[k]);//获取各连通域的面积 
                if (area > 0.15 * AREA && area < 0.75 * AREA)//根据面积作筛选(指定最小面积,最大面积):
                {
                    myContours.Add(contours[k]);
                }
            }
            
            if (myContours.Count != 0)
            {
                try
                {
                    int maxSize = myContours.Max(t => t.Size);
                    //VectorOfPoint productContour = (VectorOfPoint)myContours.Where(t => t.Size.Equals(maxSize));
                    int index = myContours.FindIndex(t => t.Size.Equals(maxSize));//最大的轮廓
                    VectorOfPoint productContour = myContours[index];
                    getProduceInfoForYMJ(productContour);

                    if (ProduceMacth() == true)//匹配成功
                    {
                        GLB.robot_device_point.X = GLB.MatYMJCam[0] * GLB.camera_device_point.X + GLB.MatYMJCam[1] * GLB.camera_device_point.Y + GLB.MatYMJCam[2];
                        GLB.robot_device_point.Y = GLB.MatYMJCam[3] * GLB.camera_device_point.X + GLB.MatYMJCam[4] * GLB.camera_device_point.Y + GLB.MatYMJCam[5];
                        GLB.robot_device_point.Z = 990;//平台高度
                        GLB.device_angl += -2f;//映美金相机与机器人夹角2度
                        GLB.device_angl = (float)(GLB.device_angl * Math.PI / 180f);
                        if (GLB.robot_device_point.X < 1500 || GLB.robot_device_point.X > 2500 || //限定范围
                            GLB.robot_device_point.Y < -600 || GLB.robot_device_point.Y > 600 )
                        {
                            GLB.Match_success = false;
                            GLB.TitleStr += "，但是超出范围";
                        }
                        else GLB.Match_success = true;
                    }                   
                    else
                    {
                        GLB.Match_success = false;
                    }
                }
                catch { }
            }
            myContours.Clear();
        }
        /// <summary>
        /// 映美金获取产品特征及位置
        /// </summary>
        /// <param name="productContour"></param>
        public static void getProduceInfoForYMJ(VectorOfPoint productContour)
        {
            //#########################################【1】,获取最小外接矩形中心点下标#################################################   
            RotatedRect rect = CvInvoke.MinAreaRect(productContour);//最小矩形
            PointF[] pt = CvInvoke.BoxPoints(rect);//最小外接矩形四个角点
            PointF po = rect.Center;//最小外接矩形中心
            int xc = (int)po.X;//最小外接矩形中心X坐标
            int yc = (int)po.Y;//最小外接矩形中心Y坐标
            //#########################################【2】,绘制外接最小矩形(紧贴连通域):#################################################
            for (int i = 0; i < 4; ++i)
            {
                Point p1 = new Point((int)pt[i].X, (int)pt[i].Y);
                Point p2 = new Point((int)pt[(i + 1) % 4].X, (int)pt[(i + 1) % 4].Y);
                CvInvoke.Line(TisCamera.YMJImage, p1, p2, new MCvScalar(0, 0, 255), 8);
            }

            //#########################################【3】长轴，短轴，旋转角:#################################################
            float width = rect.Size.Width;//像素宽
            float height = rect.Size.Height;//像素长
            if (height > width)
            {
                float temp = height;
                height = width;
                width = temp;
            }

            double Angl = rect.Angle;//矩形框角度 
                                     // Angl *= 180d / Math.PI; //换算成角度制
            if (Math.Abs(Angl) > 45) Angl = Angl + 90;


            if (Angl >= 90)//控制旋转范围
            {
                Angl -= 180;
            }
            if (Angl <= -90)
            {
                Angl += 180;
            }
            GLB.obj.Angle = Angl;//旋转角
            GLB.obj.axisLong = width*0.466;//长轴  一个像素0.466mm
            GLB.obj.axisShort = height* 0.466;//短轴
            GLB.obj.L2S = width /height  ;//长短轴之比;
            GLB.obj.realArea = GLB.obj.axisLong * GLB.obj.axisShort; //估算的物件尺寸

            //像尺寸显示:
            CvInvoke.PutText(TisCamera.YMJImage, "Lr=" + (int)GLB.obj.axisLong + ",Sr=" + (int)GLB.obj.axisShort, new System.Drawing.Point((int)pt[2].X, (int)pt[2].Y), Emgu.CV.CvEnum.FontFace.HersheyDuplex, 1.75, new MCvScalar(0, 0, 255), 8);
            CvInvoke.PutText(TisCamera.YMJImage, "Angl=" + Angl, new System.Drawing.Point((int)pt[3].X, (int)pt[3].Y), Emgu.CV.CvEnum.FontFace.HersheyDuplex, 1.75, new MCvScalar(0, 0, 255), 8);

            //#########################################【4】获取质心#################################################
            MCvMoments moments = CvInvoke.Moments(productContour, false);//计算当前轮廓的矩

            int gravity_x = Convert.ToInt32(moments.M10 / moments.M00);//计算当前轮廓中心点坐标
            int gravity_y = Convert.ToInt32(moments.M01 / moments.M00);
            GLB.obj.Depth = 990;//平台高度//Z
            GLB.obj.xCenter = gravity_x;//X
            GLB.obj.yCenter = gravity_y;//Y
            //#########################################【5】显示本区块中心[画圆]:#################################################
            RotatedRect boxCenter = new RotatedRect(new PointF(xc, yc), new Size(18, 18), 0);
            CvInvoke.Ellipse(TisCamera.YMJImage, boxCenter, new MCvScalar(255, 0, 0), 8);//在中心画一个小圆
            CvInvoke.PutText(TisCamera.YMJImage, "x:" + xc + "y:" + yc + "XC=" + GLB.obj.xCenter + "YC=" + GLB.obj.yCenter + "Depth=" + GLB.obj.Depth, new System.Drawing.Point(xc - 176, yc + 25), Emgu.CV.CvEnum.FontFace.HersheyDuplex, 1.75, new MCvScalar(0, 0, 255), 8);//深度显示
        }
        /// <summary>
        /// 产品类型尺寸匹配
        /// </summary>
        /// <returns></returns>
        public static bool ProduceMacth()
        {
            bool Flag = false;
            double deviation = 0;
            deviation += Math.Abs(GLB.produceSampleList[GLB.produce_index].axisLong - GLB.obj.axisLong)/ GLB.obj.axisLong;
            deviation += Math.Abs(GLB.produceSampleList[GLB.produce_index].axisShort - GLB.obj.axisShort) / GLB.obj.axisShort;
            deviation += Math.Abs(GLB.produceSampleList[GLB.produce_index].L2S - GLB.obj.L2S) / GLB.obj.L2S;//长短边比值
            deviation += Math.Abs(GLB.produceSampleList[GLB.produce_index].realArea - GLB.obj.realArea) / GLB.obj.realArea;//面积

            if (deviation <= 0 || deviation > 0.2 || double.IsNaN(deviation))
            {
                Flag = false; ;//偏差过大不执行   
                GLB.TitleStr += "匹配：" + GLB.produceSampleList[GLB.produce_index].produceName + "匹配失败，请检查产品型号";

            }
            else
            {
                GLB.camera_device_point.X = GLB.obj.xCenter;
                GLB.camera_device_point.Y = GLB.obj.yCenter;
                GLB.camera_device_point.Z = GLB.obj.Depth;
                GLB.device_angl = (float)GLB.obj.Angle;           

                GLB.TitleStr += "匹配：" + GLB.produceSampleList[GLB.produce_index].produceName + "匹配成功";
                Flag = true;
            }
            return Flag;
        }
    }
}
