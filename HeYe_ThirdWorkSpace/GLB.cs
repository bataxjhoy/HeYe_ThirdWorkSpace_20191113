using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using System.Drawing;
using System.Drawing.Imaging;

namespace HeYe_ThirdWorkSpace
{
    class GLB
    {
        public static int BUFW = 1280, BUFH = 960, BUFSIZE = 1280 * 960 * 3;
        public static Image<Emgu.CV.Structure.Bgr, Byte> MyFrame = new Image<Bgr, byte>(BUFW, BUFH);//显示区
        public static string TitleStr = "";//显示信息
        public static int img_mode = 0;//选择图像模式
        public static int run_mode = 0;//运行模式
        public static int Camera_index = 0;//相机编号
        public static int produce_index = 0;//产品编号
        public static bool Match_success = false;//是否匹配成功
        public static produceInfo obj = new produceInfo();//识别中的产品
        public static Point3 camera_device_point = new Point3(0, 0, 0);//工件位置
        public static Point3 robot_device_point = new Point3(0, 0, 0);//工件位置
        public static float device_angl = 0;//工件旋转角
        public static byte[] savebuf = new byte[4];//0 产品类型
        public static Queue<float> avgAngle = new Queue<float>();//旋转角先进先出集合       
        public static Queue<Point3> avgCameraPoint3 = new Queue<Point3>();// 相机坐标先进先出集合

        public static List<produceInfo> produceSampleList = new List<produceInfo>(5)//模板的样品
        {
            //new produceInfo(){ produceName = "558X658", realArea = 558*658, axisLong = 658,axisShort=558,L2S=658/558f },
              new produceInfo(){ produceName = "900X950", realArea = 900*950, axisLong = 950,axisShort=900,L2S=950/900f },
              new produceInfo(){ produceName = "950X1000", realArea = 950*1000, axisLong = 1000,axisShort=950,L2S=1000/950f },
              new produceInfo(){ produceName = "900X1500", realArea = 900*1500, axisLong = 1500,axisShort=900,L2S=1500/900f },
              new produceInfo(){ produceName = "1000X1500", realArea = 1000*1500, axisLong = 1500,axisShort=1000,L2S=1500/1000f },
              new produceInfo(){ produceName = "1000X1800", realArea = 1000*1800, axisLong = 1800,axisShort=1000,L2S=1800/1000f },
        };
        //图漾相机变换矩阵:
        public static float[] MatTuYangCam = new float[9]{
           -1.00456025846352f, -0.0267326619755255f, 200.652535676406f,
            -0.0199924728852799f, 1.009990534083f, -1672.10637181666f,
            0, 0, 1,

        };
        //映美金变换矩阵:
        public static float[] MatYMJCam = new float[9]{
          0.00925161341370309f, -0.460893814777497f, 2850.61319653302f,
        -0.4617018894638f, -0.00964218284541615f, 1387.73351127811f,
        0, 0, 1,
        };
    }
    public class produceInfo
    {
        public string produceName;      //产品工件名称
        public double realArea;         //真实面积 
        public double axisLong;         //长轴        
        public double axisShort;       //短轴         
        public double L2S;          //本连通域的最小外接矩形长短边比值  
        
        public int xCenter;         //中心X坐标
        public int yCenter;         //中心Y坐标
        public int Depth;        //中心处深度(距离)
        public double Angle;        //本连通域的最小外接矩形长边倾斜角(-180 ... +180)
        public List<Point> jd = new List<Point>();//轮廓角点
    }
    //三维点:
    public class Point3
    {
        public Double X;
        public Double Y;
        public Double Z;
        //构造函数:
        public Point3()
        {
        }
        //构造函数:
        public Point3(Double x, Double y, Double z)
        {
            X = x;
            Y = y;
            Z = z;
        }
        /// <summary>
        /// 欧拉角(弧度制)转四元数(w,x,y,z)
        /// </summary>
        /// <param name="C">Z</param>
        /// <param name="B">Y</param>
        /// <param name="A">X</param>
        /// <returns></returns>
        public double[] ABC2Q(double C, double B, double A)
        {
            double cosRoll, sinRoll, cosPitch, sinPitch, cosyaw, sinyaw, qw, qx, qy, qz;
            cosRoll = Math.Cos(A * 0.5f);//x
            sinRoll = Math.Sin(A * 0.5f);//x             

            cosPitch = Math.Cos(B * 0.5f);//y
            sinPitch = Math.Sin(B * 0.5f);//y

            cosyaw = Math.Cos(C * 0.5f); //z
            sinyaw = Math.Sin(C * 0.5f);//z

            qw = cosRoll * cosPitch * cosyaw + sinRoll * sinPitch * sinyaw;
            qx = sinRoll * cosPitch * cosyaw - cosRoll * sinPitch * sinyaw;
            qy = cosRoll * sinPitch * cosyaw + sinRoll * cosPitch * sinyaw;
            qz = cosRoll * cosPitch * sinyaw - sinRoll * sinPitch * cosyaw;

            return new double[4] { qw, qx, qy, qz };
        }
    }

    
}
