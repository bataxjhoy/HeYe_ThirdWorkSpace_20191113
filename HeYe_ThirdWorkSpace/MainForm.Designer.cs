namespace HeYe_ThirdWorkSpace
{
    partial class MainFrom
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainFrom));
            this.RobotListDataView = new System.Windows.Forms.DataGridView();
            this.IP_Address = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Availability = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Virtual = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.System_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Version = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ProgramRunBtn = new System.Windows.Forms.Button();
            this.StartBtn = new System.Windows.Forms.Button();
            this.PowerOnOff = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.checkedListBox_IO_OUT = new System.Windows.Forms.CheckedListBox();
            this.ptbDisplay = new System.Windows.Forms.PictureBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.Img_modeBox = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.CaramaNumComBox = new System.Windows.Forms.ComboBox();
            this.icImagingControl = new TIS.Imaging.ICImagingControl();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.checkedListBox_IO_IN = new System.Windows.Forms.CheckedListBox();
            this.label7 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.ExposureTimeTrackBar = new System.Windows.Forms.TrackBar();
            this.label5 = new System.Windows.Forms.Label();
            this.BrightnessTrackBar = new System.Windows.Forms.TrackBar();
            this.label4 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.produceTypeComboBox = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.logoBox = new System.Windows.Forms.PictureBox();
            this.label9 = new System.Windows.Forms.Label();
            this.ClearAlarmBtn = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.RobotListDataView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ptbDisplay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.icImagingControl)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ExposureTimeTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BrightnessTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.logoBox)).BeginInit();
            this.SuspendLayout();
            // 
            // RobotListDataView
            // 
            this.RobotListDataView.AllowUserToAddRows = false;
            this.RobotListDataView.AllowUserToDeleteRows = false;
            this.RobotListDataView.AllowUserToResizeColumns = false;
            this.RobotListDataView.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.RobotListDataView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.RobotListDataView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.RobotListDataView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.IP_Address,
            this.ID,
            this.Availability,
            this.Virtual,
            this.System_name,
            this.Version});
            this.RobotListDataView.Location = new System.Drawing.Point(50, 31);
            this.RobotListDataView.Name = "RobotListDataView";
            this.RobotListDataView.ReadOnly = true;
            this.RobotListDataView.RowHeadersVisible = false;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.Blue;
            this.RobotListDataView.RowsDefaultCellStyle = dataGridViewCellStyle2;
            this.RobotListDataView.RowTemplate.Height = 23;
            this.RobotListDataView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.RobotListDataView.Size = new System.Drawing.Size(693, 92);
            this.RobotListDataView.TabIndex = 3;
            // 
            // IP_Address
            // 
            this.IP_Address.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.IP_Address.DataPropertyName = "IPAddress";
            this.IP_Address.HeaderText = "IP Address";
            this.IP_Address.Name = "IP_Address";
            this.IP_Address.ReadOnly = true;
            // 
            // ID
            // 
            this.ID.DataPropertyName = "ID";
            this.ID.HeaderText = "ID";
            this.ID.Name = "ID";
            this.ID.ReadOnly = true;
            this.ID.Width = 160;
            // 
            // Availability
            // 
            this.Availability.DataPropertyName = "Availability";
            this.Availability.HeaderText = "Availability";
            this.Availability.Name = "Availability";
            this.Availability.ReadOnly = true;
            this.Availability.Width = 160;
            // 
            // Virtual
            // 
            this.Virtual.DataPropertyName = "IsVirtual";
            this.Virtual.HeaderText = "Virtual";
            this.Virtual.Name = "Virtual";
            this.Virtual.ReadOnly = true;
            this.Virtual.Width = 80;
            // 
            // System_name
            // 
            this.System_name.DataPropertyName = "SystemName";
            this.System_name.HeaderText = "System name";
            this.System_name.Name = "System_name";
            this.System_name.ReadOnly = true;
            this.System_name.Width = 120;
            // 
            // Version
            // 
            this.Version.DataPropertyName = "Version";
            this.Version.HeaderText = "Version";
            this.Version.Name = "Version";
            this.Version.ReadOnly = true;
            this.Version.Width = 80;
            // 
            // ProgramRunBtn
            // 
            this.ProgramRunBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.ProgramRunBtn.Location = new System.Drawing.Point(1113, 69);
            this.ProgramRunBtn.Name = "ProgramRunBtn";
            this.ProgramRunBtn.Size = new System.Drawing.Size(108, 45);
            this.ProgramRunBtn.TabIndex = 8;
            this.ProgramRunBtn.Text = "运行程序";
            this.ProgramRunBtn.UseVisualStyleBackColor = false;
            this.ProgramRunBtn.Click += new System.EventHandler(this.ProgramRunBtn_Click);
            // 
            // StartBtn
            // 
            this.StartBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.StartBtn.Location = new System.Drawing.Point(999, 69);
            this.StartBtn.Name = "StartBtn";
            this.StartBtn.Size = new System.Drawing.Size(111, 45);
            this.StartBtn.TabIndex = 7;
            this.StartBtn.Text = "开始";
            this.StartBtn.UseVisualStyleBackColor = false;
            this.StartBtn.Click += new System.EventHandler(this.StartBtn_Click);
            // 
            // PowerOnOff
            // 
            this.PowerOnOff.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.PowerOnOff.Location = new System.Drawing.Point(893, 69);
            this.PowerOnOff.Name = "PowerOnOff";
            this.PowerOnOff.Size = new System.Drawing.Size(111, 45);
            this.PowerOnOff.TabIndex = 6;
            this.PowerOnOff.Text = "伺服上电";
            this.PowerOnOff.UseVisualStyleBackColor = false;
            this.PowerOnOff.Click += new System.EventHandler(this.PowerOnOff_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 32);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 10;
            this.label2.Text = "IO输出设置";
            // 
            // checkedListBox_IO_OUT
            // 
            this.checkedListBox_IO_OUT.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.checkedListBox_IO_OUT.CheckOnClick = true;
            this.checkedListBox_IO_OUT.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.checkedListBox_IO_OUT.FormattingEnabled = true;
            this.checkedListBox_IO_OUT.Items.AddRange(new object[] {
            "Local_IO_0_DO1 正转",
            "Local_IO_0_DO2 反转",
            "Local_IO_0_DO3 触发",
            "Local_IO_0_DO4 红灯",
            "Local_IO_0_DO5 绿灯",
            "Local_IO_0_DO6 蜂鸣",
            "Local_IO_0_DO7",
            "Local_IO_0_DO8",
            "Local_IO_0_DO9 喷枪1.6",
            "Local_IO_0_DO10喷枪2.3",
            "Local_IO_0_DO11喷枪4.5",
            "Local_IO_0_DO12前挡",
            "Local_IO_0_DO13侧推",
            "Local_IO_0_DO14后档",
            "Local_IO_0_DO15抓手",
            "Local_IO_0_DO16"});
            this.checkedListBox_IO_OUT.Location = new System.Drawing.Point(8, 55);
            this.checkedListBox_IO_OUT.Name = "checkedListBox_IO_OUT";
            this.checkedListBox_IO_OUT.Size = new System.Drawing.Size(203, 340);
            this.checkedListBox_IO_OUT.TabIndex = 0;
            this.checkedListBox_IO_OUT.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.checkedListBox_IO_OUT_ItemCheck);
            // 
            // ptbDisplay
            // 
            this.ptbDisplay.BackColor = System.Drawing.Color.Black;
            this.ptbDisplay.Location = new System.Drawing.Point(12, 15);
            this.ptbDisplay.Name = "ptbDisplay";
            this.ptbDisplay.Size = new System.Drawing.Size(808, 701);
            this.ptbDisplay.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.ptbDisplay.TabIndex = 12;
            this.ptbDisplay.TabStop = false;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 200;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // Img_modeBox
            // 
            this.Img_modeBox.Font = new System.Drawing.Font("微软雅黑", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Img_modeBox.FormattingEnabled = true;
            this.Img_modeBox.Items.AddRange(new object[] {
            "原始彩色",
            "校正彩色",
            "深度图像",
            "彩色深度"});
            this.Img_modeBox.Location = new System.Drawing.Point(640, 73);
            this.Img_modeBox.Name = "Img_modeBox";
            this.Img_modeBox.Size = new System.Drawing.Size(154, 32);
            this.Img_modeBox.TabIndex = 14;
            this.Img_modeBox.SelectedIndexChanged += new System.EventHandler(this.Img_modeBox_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(647, 60);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(69, 19);
            this.label3.TabIndex = 13;
            this.label3.Text = "图像模式:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(1088, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 14);
            this.label1.TabIndex = 16;
            this.label1.Text = "选择相机:";
            // 
            // CaramaNumComBox
            // 
            this.CaramaNumComBox.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.CaramaNumComBox.FormattingEnabled = true;
            this.CaramaNumComBox.Items.AddRange(new object[] {
            "海绵上方相机",
            "平台上方相机"});
            this.CaramaNumComBox.Location = new System.Drawing.Point(1173, 14);
            this.CaramaNumComBox.Name = "CaramaNumComBox";
            this.CaramaNumComBox.Size = new System.Drawing.Size(106, 24);
            this.CaramaNumComBox.TabIndex = 15;
            this.CaramaNumComBox.SelectedIndexChanged += new System.EventHandler(this.CaramaNumComBox_SelectedIndexChanged);
            // 
            // icImagingControl
            // 
            this.icImagingControl.BackColor = System.Drawing.Color.White;
            this.icImagingControl.DeviceListChangedExecutionMode = TIS.Imaging.EventExecutionMode.Invoke;
            this.icImagingControl.DeviceLostExecutionMode = TIS.Imaging.EventExecutionMode.AsyncInvoke;
            this.icImagingControl.ImageAvailableExecutionMode = TIS.Imaging.EventExecutionMode.MultiThreaded;
            this.icImagingControl.LiveDisplayPosition = new System.Drawing.Point(0, 0);
            this.icImagingControl.Location = new System.Drawing.Point(50, 120);
            this.icImagingControl.Name = "icImagingControl";
            this.icImagingControl.Size = new System.Drawing.Size(639, 482);
            this.icImagingControl.TabIndex = 17;
            this.icImagingControl.ImageAvailable += new System.EventHandler<TIS.Imaging.ICImagingControl.ImageAvailableEventArgs>(this.icImagingControl_ImageAvailable);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(850, 144);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(466, 458);
            this.tabControl1.TabIndex = 18;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.tabPage1.Controls.Add(this.checkedListBox_IO_IN);
            this.tabPage1.Controls.Add(this.label7);
            this.tabPage1.Controls.Add(this.checkedListBox_IO_OUT);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(458, 432);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "机器人配置";
            // 
            // checkedListBox_IO_IN
            // 
            this.checkedListBox_IO_IN.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.checkedListBox_IO_IN.CheckOnClick = true;
            this.checkedListBox_IO_IN.Enabled = false;
            this.checkedListBox_IO_IN.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.checkedListBox_IO_IN.FormattingEnabled = true;
            this.checkedListBox_IO_IN.Items.AddRange(new object[] {
            "Local_IO_0_DI1 微动开关",
            "Local_IO_0_DI2",
            "Local_IO_0_DI3",
            "Local_IO_0_DI4",
            "Local_IO_0_DI5",
            "Local_IO_0_DI6",
            "Local_IO_0_DI7",
            "Local_IO_0_DI8",
            "Local_IO_0_DI9 光电1",
            "Local_IO_0_DI10光电2",
            "Local_IO_0_DI11光电3",
            "Local_IO_0_DI12",
            "Local_IO_0_DI13",
            "Local_IO_0_DI14",
            "Local_IO_0_DI15",
            "Local_IO_0_DI16"});
            this.checkedListBox_IO_IN.Location = new System.Drawing.Point(236, 55);
            this.checkedListBox_IO_IN.Name = "checkedListBox_IO_IN";
            this.checkedListBox_IO_IN.Size = new System.Drawing.Size(216, 340);
            this.checkedListBox_IO_IN.TabIndex = 11;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(234, 32);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(65, 12);
            this.label7.TabIndex = 12;
            this.label7.Text = "IO输入状态";
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.SystemColors.Info;
            this.tabPage2.Controls.Add(this.ExposureTimeTrackBar);
            this.tabPage2.Controls.Add(this.label5);
            this.tabPage2.Controls.Add(this.BrightnessTrackBar);
            this.tabPage2.Controls.Add(this.label4);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(458, 432);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "相机配置";
            // 
            // ExposureTimeTrackBar
            // 
            this.ExposureTimeTrackBar.BackColor = System.Drawing.Color.Cornsilk;
            this.ExposureTimeTrackBar.Location = new System.Drawing.Point(30, 143);
            this.ExposureTimeTrackBar.Maximum = 1000;
            this.ExposureTimeTrackBar.Minimum = 1;
            this.ExposureTimeTrackBar.Name = "ExposureTimeTrackBar";
            this.ExposureTimeTrackBar.Size = new System.Drawing.Size(317, 45);
            this.ExposureTimeTrackBar.TabIndex = 17;
            this.ExposureTimeTrackBar.Value = 50;
            this.ExposureTimeTrackBar.Scroll += new System.EventHandler(this.ExposureTimeTrackBar_Scroll);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(25, 114);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(111, 22);
            this.label5.TabIndex = 18;
            this.label5.Text = "设置曝光时间:";
            // 
            // BrightnessTrackBar
            // 
            this.BrightnessTrackBar.BackColor = System.Drawing.Color.Cornsilk;
            this.BrightnessTrackBar.Location = new System.Drawing.Point(30, 46);
            this.BrightnessTrackBar.Maximum = 1000;
            this.BrightnessTrackBar.Minimum = 10;
            this.BrightnessTrackBar.Name = "BrightnessTrackBar";
            this.BrightnessTrackBar.Size = new System.Drawing.Size(317, 45);
            this.BrightnessTrackBar.TabIndex = 15;
            this.BrightnessTrackBar.Value = 50;
            this.BrightnessTrackBar.Scroll += new System.EventHandler(this.BrightnessTrackBar_Scroll);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(25, 17);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(79, 22);
            this.label4.TabIndex = 16;
            this.label4.Text = "设置亮度:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.Location = new System.Drawing.Point(890, 15);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(75, 14);
            this.label6.TabIndex = 20;
            this.label6.Text = "产品类型:";
            // 
            // produceTypeComboBox
            // 
            this.produceTypeComboBox.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.produceTypeComboBox.FormattingEnabled = true;
            this.produceTypeComboBox.Items.AddRange(new object[] {
            "900X950",
            "950X1000",
            "900X1500",
            "1000X1500",
            "1000X1800"});
            this.produceTypeComboBox.Location = new System.Drawing.Point(975, 12);
            this.produceTypeComboBox.Name = "produceTypeComboBox";
            this.produceTypeComboBox.Size = new System.Drawing.Size(106, 24);
            this.produceTypeComboBox.TabIndex = 19;
            this.produceTypeComboBox.SelectedIndexChanged += new System.EventHandler(this.produceTypeComboBox_SelectedIndexChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label8.Location = new System.Drawing.Point(1027, 719);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(195, 16);
            this.label8.TabIndex = 110;
            this.label8.Text = "安吉八塔机器人有限公司";
            // 
            // logoBox
            // 
            this.logoBox.Location = new System.Drawing.Point(1081, 621);
            this.logoBox.Name = "logoBox";
            this.logoBox.Size = new System.Drawing.Size(100, 95);
            this.logoBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.logoBox.TabIndex = 112;
            this.logoBox.TabStop = false;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label9.Location = new System.Drawing.Point(1009, 745);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(235, 16);
            this.label9.TabIndex = 111;
            this.label9.Text = "联系人：蒋先生~18905828558";
            // 
            // ClearAlarmBtn
            // 
            this.ClearAlarmBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.ClearAlarmBtn.Location = new System.Drawing.Point(1227, 69);
            this.ClearAlarmBtn.Name = "ClearAlarmBtn";
            this.ClearAlarmBtn.Size = new System.Drawing.Size(89, 45);
            this.ClearAlarmBtn.TabIndex = 113;
            this.ClearAlarmBtn.Text = "清除报警";
            this.ClearAlarmBtn.UseVisualStyleBackColor = false;
            this.ClearAlarmBtn.Click += new System.EventHandler(this.ClearAlarmBtn_Click);
            // 
            // MainFrom
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1328, 789);
            this.Controls.Add(this.ClearAlarmBtn);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.logoBox);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.produceTypeComboBox);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.ptbDisplay);
            this.Controls.Add(this.icImagingControl);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.CaramaNumComBox);
            this.Controls.Add(this.Img_modeBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.ProgramRunBtn);
            this.Controls.Add(this.StartBtn);
            this.Controls.Add(this.PowerOnOff);
            this.Controls.Add(this.RobotListDataView);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainFrom";
            this.Text = "MainFrom";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainFrom_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.RobotListDataView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ptbDisplay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.icImagingControl)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ExposureTimeTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BrightnessTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.logoBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView RobotListDataView;
        private System.Windows.Forms.DataGridViewTextBoxColumn IP_Address;
        private System.Windows.Forms.DataGridViewTextBoxColumn ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn Availability;
        private System.Windows.Forms.DataGridViewTextBoxColumn Virtual;
        private System.Windows.Forms.DataGridViewTextBoxColumn System_name;
        private System.Windows.Forms.DataGridViewTextBoxColumn Version;
        private System.Windows.Forms.Button ProgramRunBtn;
        private System.Windows.Forms.Button StartBtn;
        private System.Windows.Forms.Button PowerOnOff;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckedListBox checkedListBox_IO_OUT;
        private System.Windows.Forms.PictureBox ptbDisplay;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ComboBox Img_modeBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox CaramaNumComBox;
        private TIS.Imaging.ICImagingControl icImagingControl;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TrackBar BrightnessTrackBar;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TrackBar ExposureTimeTrackBar;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox produceTypeComboBox;
        private System.Windows.Forms.CheckedListBox checkedListBox_IO_IN;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.PictureBox logoBox;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button ClearAlarmBtn;
    }
}

