//ABB 扫描单台机器无
using System;
using ABB.Robotics.Controllers;
using ABB.Robotics.Controllers.Discovery;
using ABB.Robotics.Controllers.RapidDomain;
using System.Collections.Generic;
using ABB.Robotics.Controllers.IOSystemDomain;
namespace HeYe_ThirdWorkSpace
{
    class ABBController
    {
        public ControllerInfoCollection allControllers = null;//控制器信息集         
      
        public RapidData DstPosQuat;//控制器--位置+姿态,用于读写Robot-Controller的数据
        public Pose posquat;//本地数据,-->DstPosQuat

        public RapidData produceTypeRobot = null;//产品型号
        public Num producetypeLocal;

        public ABBController()
        {
            NetworkScanner netscan = new NetworkScanner();
            netscan.Scan();
            allControllers = netscan.Controllers;// 扫描到所有控制器:
        }

        ////通过 index 获取指定的:
        public Controller GetController(int Select)
        {
            return new Controller(allControllers[Select]);
        }
        //初始化数据流（上电时初始化数据流）:
        public void InitDataStream(Controller controller)
        {
            controller.Logon(ABB.Robotics.Controllers.UserInfo.DefaultUser);
            ABB.Robotics.Controllers.RapidDomain.Task[] tasks = controller.Rapid.GetTasks();
            if (tasks[0] != null)
            {
                //单点运动
                DstPosQuat = tasks[0].GetRapidData("MainModule", "DstPosQuat");//获取实例
                if (DstPosQuat.Value is Pose)
                {
                    posquat = (Pose)DstPosQuat.Value;//获取默认值
                }

                //产品类型
                produceTypeRobot = tasks[0].GetRapidData("MainModule", "produceType");
                if (produceTypeRobot.Value is Num)
                {
                    producetypeLocal = (Num)produceTypeRobot.Value;
                }
            }
        }

      
        /// <summary>
        ///  //重置Task程序指针 
        /// </summary>
        /// <param name="controller">控制器</param>
        /// <param name="taskname">任务名称</param>
        /// <returns>返回值</returns>
        private int RAPID_ProgramReset(Controller controller,string taskname)
        {
            controller.Logon(ABB.Robotics.Controllers.UserInfo.DefaultUser);//登录

            if (controller.OperatingMode != ControllerOperatingMode.Auto)//自动模式
            {
                return -1;
            }
            if (!controller.AuthenticationSystem.CheckDemandGrant(Grant.ExecuteRapid))//可执行
                controller.AuthenticationSystem.DemandGrant(Grant.ExecuteRapid);
            try
            {
                using (Mastership m = Mastership.Request(controller.Rapid))//写权限
                {
                    controller.Rapid.GetTask(taskname).ResetProgramPointer();//复位程序指针
                    m.Release();
                }
            }
            catch (Exception)
            {
            }
            return 0;
        }

        /// <summary>
        /// 开始程序
        /// </summary>
        /// <param name="controller">控制器</param>
        /// <returns></returns>
        public int RAPID_ProgramStart(Controller controller)
        {
            controller.Logon(ABB.Robotics.Controllers.UserInfo.DefaultUser);//登录
            if (controller.OperatingMode != ControllerOperatingMode.Auto)//自动模式
            {
                return -1;
            }
            if (controller.State != ControllerState.MotorsOn)//已经上电
            {
                return -1;
            }
            if (!controller.AuthenticationSystem.CheckDemandGrant(Grant.ExecuteRapid))//可执行
                controller.AuthenticationSystem.DemandGrant(Grant.ExecuteRapid);
            try
            {
                using (Mastership m = Mastership.Request(controller.Rapid))//写权限
                {
                    controller.Rapid.Start(RegainMode.Continue, ExecutionMode.Continuous);//开始继续运行
                    m.Release();
                }

            }
            catch (Exception ex)
            {
                return -1;
            }
            return 0;
        }

        /// <summary>
        /// 停止当前Rapid程序
        /// </summary>
        /// <param name="controller">控制器</param>
        public void RAPID_ProgramStop(Controller controller)
        {
            controller.Logon(ABB.Robotics.Controllers.UserInfo.DefaultUser);
            if (controller.OperatingMode != ControllerOperatingMode.Auto)
            {
            }
            if (!controller.AuthenticationSystem.CheckDemandGrant(Grant.ExecuteRapid))
                controller.AuthenticationSystem.DemandGrant(Grant.ExecuteRapid);
            try
            {
                using (Mastership m = Mastership.Request(controller.Rapid))
                {

                    controller.Rapid.Stop(StopMode.Immediate);
                    m.Release();
                }

            }
            catch (Exception ex)
            {
            }
        }
      
        /// <summary>
        /// 电机上电 
        /// </summary>
        /// <param name="c">控制器</param>
        /// <param name="result">日志</param>
        /// <returns>是否成功： true 成功， false 失败</returns>
        public bool SetMotorsOn(Controller controller)
        {
            try
            {
                controller.Logon(ABB.Robotics.Controllers.UserInfo.DefaultUser);
                controller.State = ControllerState.MotorsOn;
                return true;
            }
            catch (Exception)
            {                
                return false;
            }
        }

        /// <summary>
        /// 电机下电
        /// </summary>
        /// <param name="c">控制器</param>
        /// <param name="result">日志</param>
        /// <returns>是否执行成功： 0成功， -1失败</returns>
        public bool SetMotorsOff(Controller controller)
        {
            try
            {
                controller.Logon(ABB.Robotics.Controllers.UserInfo.DefaultUser);
                controller.State = ControllerState.MotorsOff;                
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 执行例行程序之一： 直接执行Rapid中写好的例行程序，不带参数
        /// </summary>
        /// <param name="controller"> 控制器名称</param>
        /// <param name="moduleName"> Rapid 程序中模块名称 </param>
        /// <param name="routineName"> Rapid 程序中例行程序名称 </param>
        /// <param name="result"> 日志 </param>
        /// <returns>成功状态</returns>
        public int StartRoutine(Controller controller,string moduleName, string routineName)
        {
            ABB.Robotics.Controllers.RapidDomain.Task[] tasks = null;
            
            try
            {
                tasks = controller.Rapid.GetTasks();
                if (controller.OperatingMode == ControllerOperatingMode.Auto)
                {
                    //检查任务执行状态，若任务使用中报错：  error "SYS_E_EXEC_LEVEL: Operation is illegal at current execution level" 
                    if (tasks[0].ExecutionStatus == TaskExecutionStatus.Running)
                    {
                        return -1;
                    }
                    else
                    {
                        if (tasks[0].ExecutionStatus == TaskExecutionStatus.Ready || tasks[0].ExecutionStatus == TaskExecutionStatus.Stopped)
                        {
                            using (Mastership master = Mastership.Request(controller.Rapid))
                            {
                                controller.Rapid.Stop(StopMode.Immediate);//立即停止
                                tasks[0].SetProgramPointer(moduleName, routineName);//设置程序指针
                                controller.Rapid.Start(true);//开始程序
                                master.Release();
                            }
                            return 0;
                        }
                    }
                }
                else
                {
                    return -1;
                }
                return -1;
            }
            catch (System.InvalidOperationException ex)
            {
                return -1;
            }
            catch (System.Exception ex)
            {
                return -1;
            }
            finally
            {
            }
        }


        public string SetIOStatus(Controller controller, string IOName)//设置IO
        {
            ABB.Robotics.Controllers.IOSystemDomain.DigitalSignal sig;
            controller.Logon(ABB.Robotics.Controllers.UserInfo.DefaultUser);
            if (controller.OperatingMode != ControllerOperatingMode.Auto)//自动模式
            {
                return null;
            }
            if (!controller.AuthenticationSystem.CheckDemandGrant(Grant.ExecuteRapid))//可执行
                controller.AuthenticationSystem.DemandGrant(Grant.ExecuteRapid);

            try
            {
                Signal Dout = controller.IOSystem.GetSignal(IOName);
                sig = (ABB.Robotics.Controllers.IOSystemDomain.DigitalSignal)Dout;
                sig.Set();
                return sig.Value.ToString();
            }
            catch (Exception)
            {

            }
            return null;
        }
        public string ResetIOStatus(Controller controller, string IOName)//复位IO
        {
            ABB.Robotics.Controllers.IOSystemDomain.DigitalSignal sig;
            controller.Logon(ABB.Robotics.Controllers.UserInfo.DefaultUser);
            if (controller.OperatingMode != ControllerOperatingMode.Auto)//自动模式
            {
                return null;
            }
            if (!controller.AuthenticationSystem.CheckDemandGrant(Grant.ExecuteRapid))//可执行
                controller.AuthenticationSystem.DemandGrant(Grant.ExecuteRapid);

            try
            {
                Signal Dout = controller.IOSystem.GetSignal(IOName);
                sig = (ABB.Robotics.Controllers.IOSystemDomain.DigitalSignal)Dout;
                sig.Reset();
                return sig.Value.ToString();
            }
            catch (Exception)
            {
            }
            return null;
        }
        public string ReadIOStatus(Controller controller, string IOName)//读取IO状态
        {
            try
            {
                controller.Logon(ABB.Robotics.Controllers.UserInfo.DefaultUser);
                if (controller.OperatingMode != ControllerOperatingMode.Auto)//自动模式
                {
                    return null;
                }
                if (!controller.AuthenticationSystem.CheckDemandGrant(Grant.ExecuteRapid))//可执行
                    controller.AuthenticationSystem.DemandGrant(Grant.ExecuteRapid);
                try
                {
                    Signal DIO = controller.IOSystem.GetSignal(IOName);
                    //ABB.Robotics.Controllers.IOSystemDomain.DigitalSignal sig;
                    //sig = (ABB.Robotics.Controllers.IOSystemDomain.DigitalSignal)DIO;
                    //return sig.Value.ToString();
                    string sig = DIO.Value.ToString();
                    return sig;
                }

                catch (Exception)
                {
                }
            }
            catch (Exception)
            {
            }
            return null;
            
        }
       
        
}
}
