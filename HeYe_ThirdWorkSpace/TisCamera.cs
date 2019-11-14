using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TIS.Imaging;//引用相机名称空间
using System.Drawing;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
namespace HeYe_ThirdWorkSpace
{
    class TisCamera
    {
        public TIS.Imaging.VCDButtonProperty Softtrigger;//触发属性
        public TIS.Imaging.VCDSwitchProperty TrigEnableSwitch;//触发使能开关
        TIS.Imaging.VCDRangeProperty BrightnessRange;//亮度
        TIS.Imaging.VCDAbsoluteValueProperty  ExposureAbsoluteValue; //曝光时间
        public TIS.Imaging.VCDAbsoluteValueProperty Trigger_Delay_Time;//触发延时属性值对象
        public TIS.Imaging.VCDSwitchProperty TriPolarity;//触发极性属性
        public TIS.Imaging.VCDAbsoluteValueProperty Trigger_Debounce_Time;//Debounce属性
        public TIS.Imaging.VCDSwitchProperty StrobeSwitch, StrobePolaritySwitch, ExposureSwitch;//频闪 //曝光
        public static int width = 5472;      //相机分辨率 2000W像素
        public static int height = 3648;
        public static Image<Bgr, byte> YMJImage = new Image<Bgr, byte>(TisCamera.width, TisCamera.height);//彩色图
        public static Size imageSize = new Size(width, height);//图像的大小
        public static Matrix<float> mapx = new Matrix<float>(height, width); //x坐标对应的映射矩阵
        public static Matrix<float> mapy = new Matrix<float>(height, width);
        public static Matrix<double> cameraMatrix = new Matrix<double>(3, 3);//相机内部参数
        public static Matrix<double> distCoeffs = new Matrix<double>(5, 1);//畸变参数
        public  void GetCamParams()
        {//填充相机矩阵
            cameraMatrix[0, 0] = 5309.717344366032;//fx
            cameraMatrix[0, 1] = 0;
            cameraMatrix[0, 2] = 2804.482528274838;//cx
            cameraMatrix[1, 0] = 0;
            cameraMatrix[1, 1] = 5307.211106725604;//fy
            cameraMatrix[1, 2] = 1765.774496601497;//cy
            cameraMatrix[2, 0] = 0;
            cameraMatrix[2, 1] = 0;
            cameraMatrix[2, 2] = 1;
            //填充畸变矩阵
            distCoeffs[0, 0] = -0.070188696024055;//K1
            distCoeffs[1, 0] = 0.173817995857571;//K2
            distCoeffs[2, 0] = 0;//P1
            distCoeffs[3, 0] = 0;//P2
            distCoeffs[4, 0] = 0;//K3
        }
         //相机初始化
        public void CameraInitialize(TIS.Imaging.ICImagingControl icImagingControl1)
        {
            try
            {
                icImagingControl1.LoadDeviceStateFromFile("device1.xml", true);//从文件加载相机文件，默认保存在当前工程中，也可以保存到绝对路径中
            }
            catch (Exception)//捕捉加载文件错误信息
            {
                icImagingControl1.ShowDeviceSettingsDialog();//相机选择窗口  
                if (!icImagingControl1.DeviceValid)
                {
                    MessageBox.Show("没有找到设备");
                    Application.Exit();
                }
                else
                {
                    icImagingControl1.SaveDeviceStateToFile("device1.xml");//保存相机参数到xml文件
                }
            }
            //SN号打开相机
            //OpenBySN(icImagingControl1, "37814466");

            //初始化控件
            icImagingControl1.LiveCaptureContinuous = true;//设置回调模式
            icImagingControl1.LiveDisplayDefault = false;//取消窗口默认大小显示
            //初始化设置 
            icImagingControl1.LiveDisplayHeight = icImagingControl1.Height;
            icImagingControl1.LiveDisplayWidth = icImagingControl1.Width;
            icImagingControl1.MemoryCurrentGrabberColorformat = TIS.Imaging.ICImagingControlColorformats.ICRGB32;//黑白格式为：ICY8；彩色格式为：ICRGB32
        }

        //SN号打开相机
        public void OpenBySN(TIS.Imaging.ICImagingControl icImagingControl1, string Ctemp)
        {
            string temp = "";
            if (icImagingControl1.Devices.Length > 0)
            {
                foreach (Device Dev in icImagingControl1.Devices)
                {
                    if (Dev.GetSerialNumber(out temp))
                    {
                        if (temp == Ctemp)//判断是否等于指定相机序号
                        {
                            icImagingControl1.Device = Dev.Name;
                            break;
                        }
                    }
                }
                if (!icImagingControl1.DeviceValid)
                {
                    MessageBox.Show("没有找到相机，是否SN号有误！");
                    Application.Exit();
                }
            }
            else
            {
                MessageBox.Show("没有找到设备,请确认相机是否连接好");
                Application.Exit();
            }
        }

        //开启相机                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                
        public void StartLiveVideo(TIS.Imaging.ICImagingControl icImagingControl1)
        {
            icImagingControl1.LiveStart();
        }

        //关闭相机
        public void StopLiveVideo(TIS.Imaging.ICImagingControl icImagingControl1)
        {
            icImagingControl1.LiveStop();
        }
        //相机属性
        public void Camproperty(TIS.Imaging.ICImagingControl icImagingControl1)
        {
            icImagingControl1.ShowPropertyDialog();
        }
        //设置亮度
        public void SetBrightness(TIS.Imaging.ICImagingControl icImagingControl1,int Brightness)
        {
            BrightnessRange = (TIS.Imaging.VCDRangeProperty)icImagingControl1.VCDPropertyItems.FindInterface(TIS.Imaging.VCDIDs.VCDID_Brightness + ":" + TIS.Imaging.VCDIDs.VCDElement_Value + ":" + TIS.Imaging.VCDIDs.VCDInterface_Range);
            if (BrightnessRange != null)
            {
                //BrightnessRange.RangeMax
                //BrightnessRange.RangeMin
                //BrightnessRange.Value
                BrightnessRange.Value = Brightness;
            }
        }

        //设置曝光
        public void SetExposure(TIS.Imaging.ICImagingControl icImagingControl1, float ExposureTime)
        {
            ExposureSwitch = (TIS.Imaging.VCDSwitchProperty)icImagingControl1.VCDPropertyItems.FindInterface(TIS.Imaging.VCDIDs.VCDID_Exposure + ":" + TIS.Imaging.VCDIDs.VCDElement_Auto + ":" + TIS.Imaging.VCDIDs.VCDInterface_Switch);
            if (ExposureSwitch != null)
            {
                ExposureSwitch.Switch = false;   //取消自动曝光

                ExposureAbsoluteValue = (TIS.Imaging.VCDAbsoluteValueProperty)icImagingControl1.VCDPropertyItems.FindInterface(TIS.Imaging.VCDIDs.VCDID_Exposure + ":" + TIS.Imaging.VCDIDs.VCDElement_Value + ":" + TIS.Imaging.VCDIDs.VCDInterface_AbsoluteValue);
                if (ExposureAbsoluteValue != null)
                {
                    if (ExposureTime <= ExposureAbsoluteValue.RangeMin)
                    {
                        ExposureAbsoluteValue.Value = ExposureAbsoluteValue.RangeMin;
                    }
                    else if (ExposureTime >= ExposureAbsoluteValue.RangeMax)
                    {
                        ExposureAbsoluteValue.Value = ExposureAbsoluteValue.RangeMax;
                    }
                    else
                    {
                        ExposureAbsoluteValue.Value = ExposureTime;
                    }
                }
            }
        }

        //保存图片
        public void SaveImage(TIS.Imaging.ICImagingControl icImagingControl1, string filename)
        {
            icImagingControl1.LiveStop();
            icImagingControl1.LiveStart();

            icImagingControl1.ImageActiveBuffer.SaveImage(filename + ".bmp");
        }

        //触发使能
        public void TriEnable(TIS.Imaging.ICImagingControl icImagingControl1)
        {
            TrigEnableSwitch = (TIS.Imaging.VCDSwitchProperty)icImagingControl1.VCDPropertyItems.FindInterface(TIS.Imaging.VCDIDs.VCDID_TriggerMode + ":" +
                TIS.Imaging.VCDIDs.VCDElement_Value + ":" + TIS.Imaging.VCDIDs.VCDInterface_Switch);
            if (TrigEnableSwitch != null)
            {
                TrigEnableSwitch.Switch = true;//触发使能
            }
        }
        //软触发
        public void Strigger(TIS.Imaging.ICImagingControl icImagingControl1)
        {
            Softtrigger = (TIS.Imaging.VCDButtonProperty)icImagingControl1.VCDPropertyItems.FindInterface(TIS.Imaging.VCDIDs.VCDID_TriggerMode + ":" +
                TIS.Imaging.VCDIDs.VCDElement_SoftwareTrigger + ":" + TIS.Imaging.VCDIDs.VCDInterface_Button);

            Softtrigger.Push();//软触发
        }

        //触发延时（Simulated trigger pulses generated through the Software Trigger function are not delayed by this parameter.）Minimum 0 s   Maximum 1 s
        public void TrigDelay(TIS.Imaging.ICImagingControl icImagingControl1, double Dtime)
        {
            Trigger_Delay_Time = (TIS.Imaging.VCDAbsoluteValueProperty)icImagingControl1.VCDPropertyItems.FindInterface(TIS.Imaging.VCDIDs.VCDID_TriggerMode + ":" +
                TIS.Imaging.VCDIDs.VCDElement_TriggerDelay + ":" + TIS.Imaging.VCDIDs.VCDInterface_AbsoluteValue);

            if (Dtime <= Trigger_Delay_Time.RangeMin)
            {
                Trigger_Delay_Time.Value = Trigger_Delay_Time.RangeMin;
            }
            else if (Dtime >= Trigger_Delay_Time.RangeMax)
            {
                Trigger_Delay_Time.Value = Trigger_Delay_Time.RangeMax;
            }
            else
            {
                Trigger_Delay_Time.Value = Dtime;
            }
        }

        //Debounce 防反跳（）
        public void TriDebounce(TIS.Imaging.ICImagingControl icImagingControl1, double DeTime)
        {
            Trigger_Debounce_Time = (TIS.Imaging.VCDAbsoluteValueProperty)icImagingControl1.VCDPropertyItems.FindInterface(TIS.Imaging.VCDIDs.VCDID_TriggerMode + ":" +
                TIS.Imaging.VCDIDs.VCDElement_TriggerDebounceTime + ":" + TIS.Imaging.VCDIDs.VCDInterface_AbsoluteValue);

            if (DeTime <= Trigger_Debounce_Time.RangeMin)
            {
                Trigger_Debounce_Time.Value = Trigger_Debounce_Time.RangeMin;
            }
            else if (DeTime >= Trigger_Debounce_Time.RangeMax)
            {
                Trigger_Debounce_Time.Value = Trigger_Debounce_Time.RangeMax;
            }
            else
            {
                Trigger_Debounce_Time.Value = DeTime;
            }
        }

        //触发极化
        public void TriggerPolary(TIS.Imaging.ICImagingControl icImagingControl1)
        {
            //极化初始化
            TriPolarity = (TIS.Imaging.VCDSwitchProperty)icImagingControl1.VCDPropertyItems.FindInterface(TIS.Imaging.VCDIDs.VCDID_TriggerMode + ":" +
                TIS.Imaging.VCDIDs.VCDElement_TriggerPolarity + ":" + TIS.Imaging.VCDIDs.VCDInterface_Switch);
            TriPolarity.Switch = false;
        }
        //频闪使能
        public void StrobeEnable(TIS.Imaging.ICImagingControl icImagingControl1)
        {
            StrobeSwitch = (TIS.Imaging.VCDSwitchProperty)icImagingControl1.VCDPropertyItems.FindInterface(TIS.Imaging.VCDIDs.VCDID_Strobe + ":"
                + TIS.Imaging.VCDIDs.VCDElement_Value + ":" + TIS.Imaging.VCDIDs.VCDInterface_Switch);
            if (StrobeSwitch != null)
            {
                StrobeSwitch.Switch = true;//频闪使能
            }
        }

        //获取频闪极性属性
        public void StrobePolarityFun(TIS.Imaging.ICImagingControl icImagingControl1)
        {
            StrobePolaritySwitch = (TIS.Imaging.VCDSwitchProperty)icImagingControl1.VCDPropertyItems.FindInterface(TIS.Imaging.VCDIDs.VCDID_Strobe + ":" + TIS.Imaging.VCDIDs.VCDElement_StrobePolarity + ":" + TIS.Imaging.VCDIDs.VCDInterface_Switch);
            if (StrobePolaritySwitch != null)
            {
                StrobePolaritySwitch.Switch = true;
            }
        }
        //退出时关闭相机
        public void CamClose(TIS.Imaging.ICImagingControl icImagingControl1)
        {
            if (icImagingControl1.DeviceValid)
            {
                icImagingControl1.LiveStop();
            }
        }
    }
}
