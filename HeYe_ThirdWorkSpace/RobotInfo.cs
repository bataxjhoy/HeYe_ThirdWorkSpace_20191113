using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeYe_ThirdWorkSpace
{
    class RobotInfo
    {

        public string IPAddress { get; set; }//机器人IP
        public string ID { get; set; }//机器人ID
        public string Availability { get; set; }// 活跃
        public string IsVirtual { get; set; }//虚拟
        public string SystemName { get; set; }//系统名称
        public string Version { get; set; }//版本
        public string Name { get; set; }//名称
        public string OperatingMode { get; set; }//操作模式
        public string SystemId { get; set; }//系统ID
       
    }
}
