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
using ABB.Robotics.Controllers;
using ABB.Robotics.Controllers.Discovery;
using ABB.Robotics.Controllers.RapidDomain;
using ABB.Robotics.Controllers.IOSystemDomain;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;

namespace HeYe_ThirdWorkSpace
{
    public partial class MainFrom : Form
    {

        private ABBController abbController = new ABBController();//ABB控制器集
        private Controller AbbCtrl;//ABB控制器
        Signal Group_IN_Signal;//触发输入IO
        Signal Group_OUT_Signal;//触发输出IO
        List<RobotInfo> RobotInfoList = new List<RobotInfo>();//机器人信息集
        bool powerOn = false;//上电状态
        bool ProgramStart = false;//线程是否开始/停止
        bool IsStart = false;//程序是否 暂停/开始
        bool Emergency_Stop_Alarm = false;//紧急停止报警
        bool IO_IN_11 = false;//光电对射IN11 低电平--有遮挡
        bool IO_IN_10 = false;//光电对射IN10 低电平--有遮挡
        string MotorStatus = "0";//电机正反转状态
        string ForwardCylinder = "0";//前挡状态
        string BackwardCylinder = "0";//后挡状态 
        string Gun1_6 = "0";//喷枪状态
        string Gun2_3 = "0";
        string Gun4_5 = "0";
        TisCamera YMJcamera = new TisCamera();//映美金相机
        Thread thread;//线程定义
        public MainFrom()
        {
            InitializeComponent();
            scanf_all_controllers();//浏览所有控制器并显示
            CameraTuYang.camera_init(true, 80, 50);//初始化camera
            Thread.Sleep(200);
            Img_modeBox.SelectedIndex = 2;//初始化显示3d深度
            CaramaNumComBox.SelectedIndex = 0; //初始化显示1号相机
            //映美精初始化
            YMJcamera.CameraInitialize(icImagingControl);//初始化相机
            YMJcamera.StartLiveVideo(icImagingControl);//打开映美金相机
            YMJcamera.TriEnable(icImagingControl);//触发使能
            if (AbbCtrl != null)
            {
                IO_IN_EVENT_INIT(AbbCtrl, "GroupIN_0_15");//初始化io输入事件
                IO_OUT_EVENT_INIT(AbbCtrl, "GroupOUT_0_15");//初始化io输出事件

                Thread.Sleep(20);
                abbController.SetIOStatus(AbbCtrl, "Local_IO_0_DO4");//红灯亮 
                Thread.Sleep(20);
                abbController.ResetIOStatus(AbbCtrl, "Local_IO_0_DO5");//绿灯灭
                Thread.Sleep(20);
                BackwardCylinder = abbController.SetIOStatus(AbbCtrl, "Local_IO_0_DO14");//后挡要默认挡住
            }
            GLB.savebuf = FileOperation.ReadByteFile("config.txt", GLB.savebuf);//读取存储的数据
            produceTypeComboBox.SelectedIndex = GLB.savebuf[0];//产品类型初始化

            logoBox.Image = Image.FromFile(@"..\bata_logo.png");

        }


        /// <summary>
        /// 关闭窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainFrom_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (ProgramStart == true)
            {
                MessageBox.Show("系统提示！", "系统正在工作，你确定要关闭系统吗？", MessageBoxButtons.OKCancel);
                DialogResult dialogResult = new DialogResult();
                if (dialogResult == DialogResult.OK)
                {
                    //关闭系统
                    ProgramStart = false;
                    ProgramRunBtn.PerformClick();
                    CameraTuYang.close_camera();//停止图漾Camera  
                    YMJcamera.StopLiveVideo(icImagingControl);//停止映美金
                }
                else
                {
                    return;
                }
            }
            else
            {
                CameraTuYang.close_camera();//停止图漾Camera  
                YMJcamera.StopLiveVideo(icImagingControl);//停止映美金
            }
        }


        /// <summary>
        /// //浏览所有控制器并显示
        /// </summary>
        private void scanf_all_controllers()
        {
            RobotInfoList.Clear();
            if (abbController.allControllers.Count() == 0) { MessageBox.Show("找不到ABB机器人控制器！"); return; }
            for (int i = 0; i < abbController.allControllers.Count(); i++)
            {
                RobotInfoList.Add(new RobotInfo()
                {
                    IPAddress = abbController.allControllers[i].IPAddress.ToString(),//机器人IP
                    ID = abbController.allControllers[i].Id,//机器人ID
                    Availability = abbController.allControllers[i].Availability.ToString(),//可利用性 
                    IsVirtual = abbController.allControllers[i].IsVirtual.ToString(),//虚拟
                    SystemName = abbController.allControllers[i].SystemName,//系统名称
                    Version = abbController.allControllers[i].Version.ToString(),//版本
                    Name = abbController.allControllers[i].Name,//名称
                    OperatingMode = abbController.GetController(i).OperatingMode.ToString(),//操作模式
                    SystemId = abbController.allControllers[i].SystemId.ToString()//系统ID

                });
            }
            RobotListDataView.DataSource = null;
            RobotListDataView.AutoGenerateColumns = false;
            RobotListDataView.DataSource = RobotInfoList;
            //自动选择第一项:          
            AbbCtrl = abbController.GetController(0);
            abbController.InitDataStream(AbbCtrl);//初始化数据流
            //Pose[] mypq = new Pose[13];
            //for (int q = 0; q <= 360; q += 30)
            //{
            //    int k = q / 30;
            //    mypq[k].Trans.X = (float)(1600 + 500 * Math.Cos(q / 57.3f));
            //    mypq[k].Trans.Y = (float)(500 * Math.Sin(q / 57.3f));
            //    mypq[k].Trans.Z = 1600;
            //    //mypq[k].Rot.Q1 = 0;
            //    //mypq[k].Rot.Q2 = 0;
            //    //mypq[k].Rot.Q3 = 1;
            //    //mypq[k].Rot.Q4 = 0;
            //    //Point3 cls=new  Point3();
            //    //Point3 myABC = cls.Q2ABC(0, 1, 0, 0);
            //    Point3 cls = new Point3();
            //    double[] myQQ = cls.ABC2Q(Math.PI / 2 + 0.6 / 180f * Math.PI, Math.PI, 0);
            //    mypq[k].Rot.Q1 = myQQ[0];
            //    mypq[k].Rot.Q2 = myQQ[1];
            //    mypq[k].Rot.Q3 = myQQ[2];
            //    mypq[k].Rot.Q4 = myQQ[3];
            //}
        }

        /// <summary>
        /// 定时器刷新显示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            byte[] mycolor = new byte[GLB.BUFSIZE];//彩色一维数数;
            float[] myp3d = new float[GLB.BUFSIZE];//真3D数据   
            byte[] rgbOut = new byte[GLB.BUFSIZE];//校正后彩色图像,length=3*width*height
            byte[] mycolorDepth = new byte[GLB.BUFSIZE];//彩色叠加深度
            //输出功能选择:
            if (GLB.Camera_index < 1 && GLB.Camera_index >= 0)//前两个相机为图漾相机
            {
                CameraTuYang.softTrigg(GLB.Camera_index);
                Thread.Sleep(150);
                if (GLB.img_mode == 0)//显示彩色图像
                {
                    CameraTuYang.getColor(mycolor, GLB.Camera_index);
                    CameraTuYang.display_color(mycolor, ptbDisplay);
                }
                else if (GLB.img_mode == 1)//显示校正后彩色图像
                {
                    CameraTuYang.getColor(mycolor, GLB.Camera_index);
                    CameraTuYang.cameraGetUndistortRGBImage(mycolor, GLB.BUFW, GLB.BUFH, GLB.Camera_index, rgbOut);//获取校正后彩色图像
                    CameraTuYang.display_color2(rgbOut, ptbDisplay);
                }
                else if(GLB.img_mode == 2)//显示真三维图像 
                {
                    CameraTuYang.get3D(myp3d, GLB.Camera_index);
                    CameraTuYang.display_point_3d(GLB.Camera_index, myp3d, ptbDisplay);
                }
                else if(GLB.img_mode == 3)//显示彩色叠加深度
                {
                    CameraTuYang.get3D(myp3d, GLB.Camera_index);
                    CameraTuYang.softTrigg(GLB.Camera_index);
                    Thread.Sleep(150);
                    CameraTuYang.getColorDepth(mycolorDepth, GLB.Camera_index);
                    CameraTuYang.display_colorDepth(mycolorDepth, myp3d, ptbDisplay);
                }
            }
            else
            {
                Thread.Sleep(5);
            }

            if (Emergency_Stop_Alarm == true)//紧急停止报警
            {
                abbController.ResetIOStatus(AbbCtrl, "Local_IO_0_DO4");//红灯灭                
                abbController.SetIOStatus(AbbCtrl, "Local_IO_0_DO6");//蜂鸣
                Thread.Sleep(600);
                abbController.SetIOStatus(AbbCtrl, "Local_IO_0_DO4");//红灯亮
                abbController.ResetIOStatus(AbbCtrl, "Local_IO_0_DO6");
                Thread.Sleep(600);
            }
            this.Text = GLB.TitleStr;
            timer1.Enabled = true;
        }

        #region IO事件 
        /// <summary>
        /// //初始化io输入事件
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="IOName"></param>
        public void IO_IN_EVENT_INIT(Controller controller, string IOName)
        {
            if (controller == null) { MessageBox.Show("找不到ABB机器人控制器！"); return; }

            Group_IN_Signal = controller.IOSystem.GetSignal(IOName);
            Group_IN_Signal.Changed += new EventHandler<SignalChangedEventArgs>(IO_IN_StateChanged);
        }
       //为了避免界面线程和主线程冲突，采用委托方式 
        private void IO_IN_StateChanged(object sender, SignalChangedEventArgs e)
        {
            this.Invoke(new EventHandler(Update_IN_Status), sender, e);           
        }
        /// <summary>
        /// 更新界面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Update_IN_Status(object sender, System.EventArgs e)
        {            
            string GroupIN_Status = Group_IN_Signal.Value.ToString();
            if (GroupIN_Status != null)
            {
                int IO_IN_VALUE = int.Parse(GroupIN_Status);
                for (int i = 0; i < 16; i++)
                {
                    if (IO_IN_VALUE % 2 == 1)
                    {
                        checkedListBox_IO_IN.SetItemChecked(i, true);//对应IO置为true
                    }
                    else
                    {
                        checkedListBox_IO_IN.SetItemChecked(i, false);
                    }
                    IO_IN_VALUE = IO_IN_VALUE >> 1;
                }                
            }

            if (checkedListBox_IO_IN.GetItemChecked(9) == true) IO_IN_10 = true;
            else IO_IN_10 = false;

            if (checkedListBox_IO_IN.GetItemChecked(10) == true) IO_IN_11 = true;
            else IO_IN_11 = false;

            if (checkedListBox_IO_IN.GetItemChecked(8) == false && IsStart == true)//暂停按键
            {
                StartBtn.PerformClick();//点击暂停
                Thread.Sleep(60);
                Emergency_Stop_Alarm = true;                
                MessageBox.Show("激光护栏被触发");
            }
        }


        /// <summary>
        /// //初始化io输出事件
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="IOName"></param>
        public void IO_OUT_EVENT_INIT(Controller controller, string IOName)
        {
            if (controller == null) { MessageBox.Show("找不到ABB机器人控制器！"); return; }

            Group_OUT_Signal = controller.IOSystem.GetSignal(IOName);
            Group_OUT_Signal.Changed += new EventHandler<SignalChangedEventArgs>(IO_OUT_StateChanged);
        }
        //为了避免界面线程和主线程冲突，采用委托方式  
        private void IO_OUT_StateChanged(object sender, SignalChangedEventArgs e)
        {
            this.Invoke(new EventHandler(Update_OUT_Status), sender, e);          
        }
        /// <summary>
        /// 更新界面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Update_OUT_Status(object sender, System.EventArgs e)
        {
            string GroupOUT_Status = Group_OUT_Signal.Value.ToString();
            if (GroupOUT_Status != null)
            {
                int IO_OUT_VALUE = int.Parse(GroupOUT_Status);
                for (int i = 0; i < 16; i++)
                {
                    if (IO_OUT_VALUE % 2 == 1)
                    {
                        checkedListBox_IO_OUT.SetItemChecked(i, true);//对应IO置为true
                    }
                    else
                    {
                        checkedListBox_IO_OUT.SetItemChecked(i, false);
                    }
                    IO_OUT_VALUE = IO_OUT_VALUE >> 1;
                }
            }
        }
            #endregion


        /// <summary>
        /// 开启主线程
        /// </summary>
        private void mainThread()
        {
            int Match_timers = 0;//匹配计数
            int Start_Glue_Wait_time = 0;//开始喷胶等待的回合
            int Stop_Glue_Wait_time = 0;//停止喷胶等待的回合
            GLB.Camera_index = 0;//先从垛区拍照
            bool isFinishPutDown = false;//是否完成覆合 
            Pose mypq = new Pose();
            Point3 cls = new Point3();
            double[] myQQ = cls.ABC2Q(0, Math.PI, 0);//欧拉角转四元数
            //#######################################home#######################################
            abbController.StartRoutine(AbbCtrl, "MainModule", "goHome");
            WaitForRoutineFinish();//等待Rapid程序执行结束

            while (ProgramStart)
            {
                //#######################################喷胶#######################################
                if (GLB.run_mode == 1 && IsStart == true)
                {
                    if (IO_IN_11 == false && isFinishPutDown == false)//滚筒上有海绵,未完成覆合
                    {
                        if (ForwardCylinder == "0") ForwardCylinder = abbController.SetIOStatus(AbbCtrl, "Local_IO_0_DO12");//前挡  
                        if (BackwardCylinder == "0") BackwardCylinder = abbController.SetIOStatus(AbbCtrl, "Local_IO_0_DO14");//后挡阻挡
                        MotorStatus = abbController.ResetIOStatus(AbbCtrl, "Local_IO_0_DO1");//停止正转
                        Thread.Sleep(200);
                        abbController.SetIOStatus(AbbCtrl, "Local_IO_0_DO13");//侧推
                        Thread.Sleep(1000);
                        abbController.ResetIOStatus(AbbCtrl, "Local_IO_0_DO13");//侧推收回
                        GLB.run_mode = 2; //进入下一工作模式
                        GLB.Camera_index = 0;
                        Thread.Sleep(300);
                    }
                    else if (IO_IN_11 == false && isFinishPutDown == true)//滚筒上有海绵,完成覆合
                    {
                        ForwardCylinder = abbController.ResetIOStatus(AbbCtrl, "Local_IO_0_DO12");//前挡收回
                        if (BackwardCylinder == "1") BackwardCylinder = abbController.ResetIOStatus(AbbCtrl, "Local_IO_0_DO14");//后挡收回
                        while (IO_IN_11 != true)//直到海绵离开
                        {
                            if (IsStart == true && MotorStatus != "1") MotorStatus = abbController.SetIOStatus(AbbCtrl, "Local_IO_0_DO1");//正转
                            else if (IsStart == false && MotorStatus == "1") MotorStatus = abbController.ResetIOStatus(AbbCtrl, "Local_IO_0_DO1");//停止正转
                            else Thread.Sleep(30);
                        }
                        isFinishPutDown = false;//清除标志
                        Thread.Sleep(50);//直到棉完全通过
                    }
                    else
                    {
                        if (ForwardCylinder != "1") ForwardCylinder = abbController.SetIOStatus(AbbCtrl, "Local_IO_0_DO12");//前挡 
                        if (MotorStatus != "1") MotorStatus = abbController.SetIOStatus(AbbCtrl, "Local_IO_0_DO1");//正转:                            
                        if (Start_Glue_Wait_time == 0 && BackwardCylinder == "1")//后档首次放行
                        { 
                             BackwardCylinder = abbController.ResetIOStatus(AbbCtrl, "Local_IO_0_DO14");//后挡放行
                        }
                        if (IO_IN_10 == false)//检测到有棉
                        {
                            if (Start_Glue_Wait_time < 40) //延时开始喷胶
                            {
                                Start_Glue_Wait_time++;
                                Thread.Sleep(10);
                            }
                            else
                            {
                                Stop_Glue_Wait_time = 0;//清除停止喷胶延时次数
                                if (BackwardCylinder == "0") BackwardCylinder = abbController.SetIOStatus(AbbCtrl, "Local_IO_0_DO14");//后挡阻挡
                                if (Gun1_6 == "0" && GLB.produceSampleList[GLB.produce_index].axisShort > 900)
                                    Gun1_6 = abbController.SetIOStatus(AbbCtrl, "Local_IO_0_DO9");//喷胶1.6
                                if (Gun2_3 == "0") Gun2_3 = abbController.SetIOStatus(AbbCtrl, "Local_IO_0_DO10");//喷胶2.3
                                if (Gun4_5 == "0") Gun4_5 = abbController.SetIOStatus(AbbCtrl, "Local_IO_0_DO11");//喷胶4.5  
                            }
                        }
                        else
                        {
                            if (Stop_Glue_Wait_time < 40) //延时停止喷胶
                            {
                                Stop_Glue_Wait_time++;
                                Thread.Sleep(10);
                            }
                            else
                            {
                                Start_Glue_Wait_time = 0;//清除开始喷胶延时次数
                                if (Gun1_6 == "1") Gun1_6 = abbController.ResetIOStatus(AbbCtrl, "Local_IO_0_DO9");//停止喷胶
                                if (Gun2_3 == "1") Gun2_3 = abbController.ResetIOStatus(AbbCtrl, "Local_IO_0_DO10");
                                if (Gun4_5 == "1") Gun4_5 = abbController.ResetIOStatus(AbbCtrl, "Local_IO_0_DO11");
                            }
                        }
                    }
                }
                else if (GLB.run_mode == 1 && IsStart == false)//中途暂停
                {
                    Thread.Sleep(5);
                    if (MotorStatus == "1") MotorStatus = abbController.ResetIOStatus(AbbCtrl, "Local_IO_0_DO1");//停止正转
                    if (Gun1_6 == "1") Gun1_6 = abbController.ResetIOStatus(AbbCtrl, "Local_IO_0_DO9");//停止喷胶
                    if (Gun2_3 == "1") Gun2_3 = abbController.ResetIOStatus(AbbCtrl, "Local_IO_0_DO10");
                    if (Gun4_5 == "1") Gun4_5 = abbController.ResetIOStatus(AbbCtrl, "Local_IO_0_DO11");
                }
                //#######################################图漾相机抓取乳胶棉#######################################
                if (GLB.run_mode == 2 && GLB.Match_success == true && IsStart == true)//图漾匹配成功 并且没有暂停
                {
                    GLB.avgAngle .Clear();//旋转角清空队列      
                    GLB.avgCameraPoint3.Clear();//相机坐标队列清空
                    Match_timers = 0;//计数清零
                    GLB.Camera_index = -1;//停止拍照
                    GLB.Match_success = false;//清除标志位

                 
                    //**************乳胶棉目标点**************
                    myQQ = cls.ABC2Q(-Math.PI / 2 - GLB.device_angl, Math.PI, 0);
                    mypq.FillFromString2("[[" + GLB.robot_device_point.X + "," + GLB.robot_device_point.Y + "," + GLB.robot_device_point.Z + "]," +
                        "[" + myQQ[0] + "," + myQQ[1] + "," + myQQ[2] + "," + myQQ[3] + "]]");

                    writePosQuat(mypq);//传输坐标
                    Thread.Sleep(50);
                    abbController.StartRoutine(AbbCtrl, "MainModule", "TakeUpFromZoneToDesk");
                    WaitForRoutineFinish();//等待Rapid程序执行结束
                   
                    GLB.Match_success = false;//清除标志位
                    GLB.Camera_index = 1;//切换相机
                    GLB.run_mode = 3;
                    Thread.Sleep(200);
                }
                else if (GLB.run_mode == 2 && GLB.Match_success == false && IsStart == true)
                {
                    Match_timers += 1;
                    if (Match_timers > 20)//二十次匹配不到，暂停
                    {
                        this.BeginInvoke(new Action(() =>
                        {
                            StartBtn.PerformClick();//点击暂停
                            Emergency_Stop_Alarm = true;
                            Thread.Sleep(60);
                            MessageBox.Show("垛区无法识别到乳胶棉,或者数量过少");
                            Match_timers = 0;
                        }));
                    }
                    Thread.Sleep(300);
                }
                ////#######################################映美金二次抓取乳胶棉#######################################
                if (GLB.run_mode == 3 && GLB.Match_success == true && IsStart == true)//图漾匹配成功 并且没有暂停
                {
                    Match_timers = 0;//计数清零
                    GLB.Camera_index = -1;//停止拍照
                    GLB.Match_success = false;//清除标志位
                  

                    //**************平台乳胶棉目标点**************
                    myQQ = cls.ABC2Q(-GLB.device_angl, Math.PI, 0);
                    mypq.FillFromString2("[[" + GLB.robot_device_point.X + "," + GLB.robot_device_point.Y + "," + GLB.robot_device_point.Z + "]," +
                        "[" + myQQ[0] + "," + myQQ[1] + "," + myQQ[2] + "," + myQQ[3] + "]]");

                    writePosQuat(mypq);//传输坐标
                    //产品类型--放置点
                    abbController.producetypeLocal.FillFromString2(GLB.produce_index.ToString());
                    using (Mastership m = Mastership.Request(AbbCtrl.Rapid))
                    {
                        abbController.produceTypeRobot.Value = abbController.producetypeLocal;
                        m.Release();
                    }
                    Thread.Sleep(50);
                    abbController.StartRoutine(AbbCtrl, "MainModule", "TakeUpFromDeskToTansLine");
                    WaitForRoutineFinish();//等待Rapid程序执行结束
                   
                    isFinishPutDown = true;//完成覆合
                    GLB.Camera_index = 0;//切换相机
                    GLB.run_mode = 1;
                    Thread.Sleep(200);
                }
                else if (GLB.run_mode == 3 && GLB.Match_success == false && IsStart == true)
                {
                    Thread.Sleep(500);
                    abbController.SetIOStatus(AbbCtrl, "Local_IO_0_DO3");//触发相机
                    Thread.Sleep(50);
                    abbController.ResetIOStatus(AbbCtrl, "Local_IO_0_DO3");
                    Match_timers += 1;
                    if (Match_timers > 10)//十次匹配不到，暂停
                    {
                        this.BeginInvoke(new Action(() =>
                        {
                            StartBtn.PerformClick();//点击暂停
                            Thread.Sleep(60);
                            Emergency_Stop_Alarm = true;
                            MessageBox.Show("平台上无法识别到乳胶棉");
                            Match_timers = 0;
                        }));
                    }
                    Thread.Sleep(500);
                }
                else
                {
                    Thread.Sleep(20);
                }
            }
        }
        /// <summary>
        /// 映美金相机触发出图
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void icImagingControl_ImageAvailable(object sender, TIS.Imaging.ICImagingControl.ImageAvailableEventArgs e)
        {
            TIS.Imaging.ImageBuffer CurrentBuffer = icImagingControl.ImageBuffers[e.bufferIndex];
            Image<Gray, byte> grayImage = new Image<Gray, byte>(CurrentBuffer.Bitmap);

            YMJcamera.GetCamParams();//获取内参
            //校正畸变
            CvInvoke.InitUndistortRectifyMap(TisCamera.cameraMatrix, TisCamera.distCoeffs, null, TisCamera.cameraMatrix,
            TisCamera.imageSize, DepthType.Cv32F, TisCamera.mapx, TisCamera.mapy);
            CvInvoke.Remap(grayImage, grayImage, TisCamera.mapx, TisCamera.mapy, Inter.Linear, BorderType.Constant, new MCvScalar(0));
            for (int i = 0; i < TisCamera.height; i++)
            {
                for (int j = 0; j < TisCamera.width; j++)
                {
                    TisCamera.YMJImage.Data[i, j, 2] = grayImage.Data[i, j, 0];
                    TisCamera.YMJImage.Data[i, j, 1] = grayImage.Data[i, j, 0];
                    TisCamera.YMJImage.Data[i, j, 0] = grayImage.Data[i, j, 0];
                }
            }
            if (GLB.Camera_index == 1)
            {
                DealWithImage.getContoursForYMJ(grayImage,ptbDisplay);
                ptbDisplay.Image = TisCamera.YMJImage.ToBitmap();//显示映美金图像
            }
        }

        /// <summary>
        /// 等待机器人运行
        /// </summary>
        private void WaitForRoutineFinish()
        {           
            ABB.Robotics.Controllers.RapidDomain.Task task = AbbCtrl.Rapid.GetTask("T_ROB1");//获得机器人正在执行的任务
            
            while (IsStart == false || task.ExecutionStatus == TaskExecutionStatus.Running|| task.ExecutionStatus != TaskExecutionStatus.Ready || task.ExecutionStatus == TaskExecutionStatus.UnInitiated || task.ExecutionStatus == TaskExecutionStatus.Unknown)
            {
                Thread.Sleep(100);
            }
        }

        /// <summary>
        /// 单点传输
        /// </summary>
        /// <param name="pq"></param>
        internal void writePosQuat(Pose pq)
        {
            using (Mastership m = Mastership.Request(AbbCtrl.Rapid))
            {
                abbController.DstPosQuat.Value = pq;
                m.Release();
            }
        }
        /// <summary>
        /// 移动到指定点集,包含姿态
        /// </summary>
        /// <param name=""></param>
        //internal void writePQArray(Pose[] pq)
        //{
        //    try
        //    {
        //        using (Mastership m = Mastership.Request(AbbCtrl.Rapid))
        //        {
        //            Pose tmp = new Pose();
        //            for (int i = 0; i < pq.Length; i++)
        //            {
        //                tmp = pq[i];
        //                abbController.pqArray.WriteItem(tmp, i);
        //            }
        //            m.Release();
        //        }
        //    }
        //    catch { }
        //}
        #region  按键操作
        /// <summary>
        /// //伺服上下电
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PowerOnOff_Click(object sender, EventArgs e)
        {
            if (AbbCtrl == null) { MessageBox.Show("找不到ABB机器人控制器！"); return; }
            if (!powerOn)//没有上电 进行上电
            {
                if (abbController.SetMotorsOn(AbbCtrl))
                {
                    PowerOnOff.BackColor = Color.Red;
                    PowerOnOff.Text = "伺服下电";
                    powerOn = true;
                    return;
                }
                else MessageBox.Show("上电不成功！请检查机器人连接！");
            }
            else if (powerOn)
            {
                if (abbController.SetMotorsOff(AbbCtrl))
                {
                    PowerOnOff.BackColor = Color.Green;
                    PowerOnOff.Text = "伺服上电";
                    powerOn = false;
                    return;
                }
                else MessageBox.Show("下电不成功！");
            }
        }
        /// <summary>
        /// 开始or 暂停
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StartBtn_Click(object sender, EventArgs e)
        {
            if (AbbCtrl == null) { MessageBox.Show("找不到ABB机器人控制器！"); return; }
            if (!powerOn)
            {
                MessageBox.Show(AbbCtrl.Name + "未上电");
                return;
            }
            if (!IsStart)
            {
                Emergency_Stop_Alarm = false;
                abbController.ResetIOStatus(AbbCtrl, "Local_IO_0_DO4");//红灯灭 
                Thread.Sleep(10);
                abbController.SetIOStatus(AbbCtrl, "Local_IO_0_DO5");//绿灯亮
                Thread.Sleep(10);
                abbController.SetIOStatus(AbbCtrl, "Local_IO_0_DO6");//蜂鸣
                Thread.Sleep(1000);
                abbController.ResetIOStatus(AbbCtrl, "Local_IO_0_DO6");
                Thread.Sleep(10);

                abbController.RAPID_ProgramStart(AbbCtrl);
                StartBtn.BackColor = Color.Red;
                StartBtn.Text = "暂停";
                Thread.Sleep(100);//先保证机器人运行起来
                IsStart = true;
            }
            else if (IsStart)
            {
                abbController.RAPID_ProgramStop(AbbCtrl);
                StartBtn.BackColor = Color.Green;
                StartBtn.Text = "开始";
                IsStart = false;
                Thread.Sleep(250);
                if(MotorStatus=="1") MotorStatus = abbController.ResetIOStatus(AbbCtrl, "Local_IO_0_DO1");//停止正转
                Thread.Sleep(10);
                if (ForwardCylinder == "1") ForwardCylinder = abbController.ResetIOStatus(AbbCtrl, "Local_IO_0_DO12");//前挡收回    
                Thread.Sleep(10);
                abbController.ResetIOStatus(AbbCtrl, "Local_IO_0_DO13");//侧推收回
                Thread.Sleep(10);
                if (Gun1_6 == "1") Gun1_6 = abbController.ResetIOStatus(AbbCtrl, "Local_IO_0_DO9");//停止喷胶
                if (Gun2_3 == "1") Gun2_3 = abbController.ResetIOStatus(AbbCtrl, "Local_IO_0_DO10");
                if (Gun4_5 == "1") Gun4_5 = abbController.ResetIOStatus(AbbCtrl, "Local_IO_0_DO11");
                if (BackwardCylinder == "0") BackwardCylinder = abbController.SetIOStatus(AbbCtrl, "Local_IO_0_DO14");//后挡阻挡
                abbController.ResetIOStatus(AbbCtrl, "Local_IO_0_DO5");//绿灯灭
                Thread.Sleep(10);
                abbController.SetIOStatus(AbbCtrl, "Local_IO_0_DO4");//红灯亮
            }
            return;
        }
        /// <summary>
        /// 程序启动 or 结束运行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ProgramRunBtn_Click(object sender, EventArgs e)
        {
            if (AbbCtrl == null) { MessageBox.Show("找不到ABB机器人控制器！"); return; }
            if (!powerOn)//没有上电 进行上电
            {
                DialogResult result = MessageBox.Show(AbbCtrl.Name + "未上电，是否上电？上电运行注意安全！", "八塔机器人提示框", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                if (result == DialogResult.OK)
                {
                    PowerOnOff.PerformClick();//上电
                }
            }

            if (powerOn)
            {
                if (!ProgramStart)
                {
                    //开始运行按键状态
                    Emergency_Stop_Alarm = false;
                    ProgramRunBtn.BackColor = Color.Red;
                    ProgramRunBtn.Text = "运行停止";
                    ProgramStart = true;

                    //修改开始/停止按键状态
                    StartBtn.BackColor = Color.Red;
                    StartBtn.Text = "暂停";
                    IsStart = true;
                    GLB.run_mode = 1;

                    Thread.Sleep(50);
                    abbController.ResetIOStatus(AbbCtrl, "Local_IO_0_DO4");//红灯灭                    
                    Thread.Sleep(10);
                    abbController.SetIOStatus(AbbCtrl, "Local_IO_0_DO5");//绿灯亮
                    Thread.Sleep(10);
                    abbController.SetIOStatus(AbbCtrl, "Local_IO_0_DO6");//蜂鸣
                    Thread.Sleep(1000);
                    abbController.ResetIOStatus(AbbCtrl, "Local_IO_0_DO6");
                    Thread.Sleep(10);

                    thread = new Thread(mainThread); //开启主线程
                    thread.IsBackground = true;
                    thread.Start();
                    return;
                }
                else if (ProgramStart)
                {
                    abbController.RAPID_ProgramStop(AbbCtrl);                    
                    //开始运行按键状态
                    ProgramRunBtn.BackColor = Color.Green;
                    ProgramRunBtn.Text = "运行开始";
                    ProgramStart = false;
                    //修改开始/停止按键状态
                    StartBtn.BackColor = Color.Green;
                    StartBtn.Text = "开始";
                    IsStart = false;
                    thread.Abort();//线程终止

                    PowerOnOff.PerformClick();  //下电
                    GLB.run_mode = 0;

                    Thread.Sleep(100);
                    if (MotorStatus == "1") MotorStatus = abbController.ResetIOStatus(AbbCtrl, "Local_IO_0_DO1");//停止正转
                    Thread.Sleep(10);
                    if (ForwardCylinder == "1") ForwardCylinder = abbController.ResetIOStatus(AbbCtrl, "Local_IO_0_DO12");//前挡收回    
                    Thread.Sleep(10);
                    abbController.ResetIOStatus(AbbCtrl, "Local_IO_0_DO13");//侧推收回
                    Thread.Sleep(10);
                    if (Gun1_6 == "1") Gun1_6 = abbController.ResetIOStatus(AbbCtrl, "Local_IO_0_DO9");//停止喷胶
                    if (Gun2_3 == "1") Gun2_3 = abbController.ResetIOStatus(AbbCtrl, "Local_IO_0_DO10");
                    if (Gun4_5 == "1") Gun4_5 = abbController.ResetIOStatus(AbbCtrl, "Local_IO_0_DO11");
                    if (BackwardCylinder == "0") BackwardCylinder = abbController.SetIOStatus(AbbCtrl, "Local_IO_0_DO14");//后挡阻挡
                    abbController.ResetIOStatus(AbbCtrl, "Local_IO_0_DO5");//绿灯灭                    
                    Thread.Sleep(20);
                    abbController.SetIOStatus(AbbCtrl, "Local_IO_0_DO4");//红灯亮
                    return;
                }
            }
        }
        
        /// <summary>
        /// 输出设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkedListBox_IO_OUT_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (AbbCtrl == null) { MessageBox.Show("找不到ABB机器人控制器！"); return; }
            if (checkedListBox_IO_OUT.GetItemChecked(e.Index))
                abbController.ResetIOStatus(AbbCtrl, checkedListBox_IO_OUT.Items[e.Index].ToString().Substring(0, 13)+ (e.Index+1));
            else
                abbController.SetIOStatus(AbbCtrl, checkedListBox_IO_OUT.Items[e.Index].ToString().Substring(0, 13) + (e.Index + 1));
        }
       
        /// <summary>
        /// 图像模式选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Img_modeBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            GLB.img_mode = Img_modeBox.SelectedIndex;
        }
        /// <summary>
        /// 相机选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CaramaNumComBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            GLB.Camera_index = CaramaNumComBox.SelectedIndex;
        }
        /// <summary>
        /// 设置亮度
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BrightnessTrackBar_Scroll(object sender, EventArgs e)
        {
            YMJcamera.SetBrightness(icImagingControl, BrightnessTrackBar.Value);
        }
        /// <summary>
        /// 设置曝光时间
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExposureTimeTrackBar_Scroll(object sender, EventArgs e)
        {
            YMJcamera.SetExposure(icImagingControl, ExposureTimeTrackBar.Value / 1000f);
        }
        /// <summary>
        /// 产品选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void produceTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            GLB.produce_index = produceTypeComboBox.SelectedIndex;
            GLB.savebuf[0] = (byte)GLB.produce_index;
            FileOperation.WriteByteFile("config.txt", GLB.savebuf);//保存产品类型
        }
        /// <summary>
        /// 清除报警
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClearAlarmBtn_Click(object sender, EventArgs e)
        {
            Emergency_Stop_Alarm = false;
        }
        #endregion
      
    }

}
