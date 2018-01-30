using System;
using System.Runtime.InteropServices;
using System.Text;

namespace HardwareControl
{
   internal class ReaderDllMethod

    {
        #region 设备操作
        /// <summary>
        /// 打开读卡器
        /// </summary>
        /// <param name="port"> "COM1","COM2",......"COM256"(串口读写器)
        ///                     "USB","USB1","USB2",......"USB10"(USB接口读写器,"USB"后面跟的是顺序号)</param>
        /// <param name="paras">当port为串口时,此参数为波特率;当port为USB时,此参数设备信息或为空</param>
        /// <param name="handle">设备通信句柄</param>
        /// <returns>小于0失败,大于等于0成功</returns>
        [DllImport("mwReader.dll", EntryPoint = "mwDevOpen", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        internal static extern Int32 mwDevOpen(string port, string paras, out IntPtr handle);

        /// <summary>
        /// 关闭已打开的读写器
        /// </summary>
        /// <param name="handle">设备通信句柄</param>
        /// <returns>小于0失败,大于等于0成功</returns>
        [DllImport("mwReader.dll", EntryPoint = "mwDevClose", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        internal static extern Int32 mwDevClose(IntPtr handle);

        /// <summary>
        /// 获取错误描述信息
        /// </summary>
        /// <param name="errcode">错误代码</param>
        /// <param name="languageCode">错误描述的语言, 0-GB2312 1-ENGLISH</param>
        /// <param name="message">错误信息</param>
        /// <returns>message的长度,小于0失败</returns>
        [DllImport("mwReader.dll", EntryPoint = "getErrDescription", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        private static extern Int32 getErrDescription(Int32 errcode, Int32 languageCode, StringBuilder message);

        /// <summary>
        /// 获取产品序列号
        /// </summary>
        /// <param name="handle">设备通信句柄</param>
        /// <param name="strSerialNumber">产品序列号</param>
        /// <returns>strSerialNumber的实际长度.小于0失败</returns>
        [DllImport("mwReader.dll", EntryPoint = "mwDevGetSerialNumber", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        internal static extern Int32 mwDevGetSerialNumber(IntPtr handle, byte[] strSerialNumber);

        /// <summary>
        /// 获取硬件版本号
        /// </summary>
        /// <param name="handle">设备通信句柄</param>
        /// <param name="strHardwareVer">硬件版本号</param>
        /// <returns>strHardwareVer的实际长度.小于0失败</returns>
        [DllImport("mwReader.dll", EntryPoint = "mwDevGetHardwareVer", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        internal static extern Int32 mwDevGetHardwareVer(IntPtr handle, byte[] strHardwareVer);

        /// <summary>
        /// 写用户自定义数据区。
        /// </summary>
        /// <param name="handle">设备通信句柄</param>
        /// <param name="offset">起始地址.范围0~1023</param>
        /// <param name="length">写入的数据长度.范围1~1024</param>
        /// <param name="data">要写入的数据。如果data的长度大于length,则截取写入;如果data的长度小于length,data后面的到length长度的内存中的数据也会被写入。此时写入的数据可能会有乱码出现。</param>
        /// <returns>大于等于0成功,小于0失败</returns>
        [DllImport("mwReader.dll", EntryPoint = "mwDevWriteConfig", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        internal static extern Int32 mwDevWriteConfig(IntPtr handle, Int32 offset, Int32 length, byte[] data);

        /// <summary>
        /// 读用户自定义数据区
        /// </summary>
        /// <param name="handle">设备通信句柄</param>
        /// <param name="offset">起始地址.范围0~1023.</param>
        /// <param name="length">要读取的数据长度.范围1~1024.</param>
        /// <param name="data">存放读取到的数据</param>
        /// <returns>data的实际长度</returns>
        [DllImport("mwReader.dll", EntryPoint = "mwDevReadConfig", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        internal static extern Int32 mwDevReadConfig(IntPtr handle, Int32 offset, Int32 length, byte[] data);

        /// <summary>
        /// 读写器鸣响
        /// </summary>
        /// <param name="handle">设备通信句柄</param>
        /// <param name="beepTimes">蜂鸣次数</param>
        /// <param name="interval">蜂鸣间隔 每100ms一个单位</param>
        /// <param name="time">蜂鸣时间 每100ms一个单位</param>
        /// <returns>大于等于0成功,小于0失败</returns>
        [DllImport("mwReader.dll", EntryPoint = "mwDevBeep", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        internal static extern Int32 mwDevBeep(IntPtr handle, Int32 beepTimes, Int32 interval, Int32 time);

        /// <summary>
        /// 射频模块控制
        /// </summary>
        /// <param name="handle">设备通信句柄</param>
        /// <param name="mode">bit7:  0 关闭射频模块
        ///                           1 复位射频模块
        ///                    bit6-bit0:   如果是复位操作，则表示复位之后延时时间，以5ms为单位。
        ///                    例如：0x81: 复位射频，延时5ms.</param>
        /// <returns>大于等于0成功,小于0失败</returns>
        [DllImport("mwReader.dll", EntryPoint = "mwDevRFControl", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        internal static extern Int32 mwDevRFControl(IntPtr handle, Int32 mode);

        #endregion

        #region LCD显示屏操作
        /// <summary>
        /// LCD显示提示信息	
        /// </summary>
        /// <param name="icdev">设备通信句柄</param>
        /// <param name="line">显示行号 范围为1~4</param>
        /// <param name="offset">偏移位置 范围为1-16</param>
        /// <param name="data">要显示的内容</param>
        /// <returns>大于等于0成功,小于0失败</returns>
        [DllImport("mwReader.dll", EntryPoint = "mwLcdDispInfo", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        internal static extern Int32 mwLcdDispInfo(IntPtr icdev, Int32 line, Int32 offset, byte[] data);

        /// <summary>
        /// 清除LCD屏幕	
        /// </summary>
        /// <param name="icdev">设备通信句柄</param>
        /// <param name="line">要清除的行号:
        ///                         0 清除全部
        ///                         1 清除第一行
        ///                         2 清除第二行
        ///                         3 清除第三行
        ///                         4 清除第四行</param>
        /// <returns>大于等于0成功,小于0失败</returns>
        [DllImport("mwReader.dll", EntryPoint = "mwLcdClear", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        internal static extern Int32 mwLcdClear(IntPtr icdev, Int32 line);

        /// <summary>
        /// 打开或关闭背光
        /// </summary>
        /// <param name="icdev">设备通信句柄</param>
        /// <param name="flag">0: 关背光; 1: 开背光</param>
        /// <returns>大于等于0成功,小于0失败</returns>
        [DllImport("mwReader.dll", EntryPoint = "mwLcdCtlBackLight", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        internal static extern Int32 mwLcdCtlBackLight(IntPtr icdev, Int32 flag);

        /// <summary>
        /// 语音控制
        /// </summary>
        /// <param name="icdev">设备通信句柄</param>
        /// <param name="cmd">语音提示: 
        ///                         0x00-不提示
        ///                         0x01-请输入密码
        ///                         0x02-请重新输入密码
        ///                         0x03-请输入旧密码
        ///                         0x04-请输入新密码</param>
        /// <returns>大于等于0成功,小于0失败</returns>
        [DllImport("mwReader.dll", EntryPoint = "mwVoiceControl", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        internal static extern Int32 mwVoiceControl(IntPtr icdev, Int32 cmd);
        #endregion

        #region 密码键盘
        /// <summary>
        /// 下载主密钥到键盘
        /// </summary>
        /// <param name="icdev">设备通信句柄</param>
        /// <param name="MKeyNo">主密钥号。0~2</param>
        /// <param name="MasterKey">主密钥,16进制字符串格式
        ///                         DES-长度为16
        ///                         3DES- 长度为32/48</param>
        /// <returns>大于等于0成功,小于0失败</returns>
        [DllImport("mwReader.dll", EntryPoint = "mwKeyPadDownLoadMasterKey", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        internal static extern Int32 mwKeyPadDownLoadMasterKey(IntPtr icdev, Int32 MKeyNo, byte[] MasterKey);

        /// <summary>
        /// 下载主密钥到键盘
        /// </summary>
        /// <param name="icdev">设备通信句柄</param>
        /// <param name="MKeyNo">主密钥号。0~2</param>
        /// <param name="keyLen">MasterKey的实际长度</param>
        /// <param name="MasterKey">主密钥,字节数组
        ///                         DES-长度为8
        ///                         3DES- 长度为16/24</param>
        /// <returns>大于等于0成功,小于0失败</returns>
        [DllImport("mwReader.dll", EntryPoint = "mwKeyPadDownLoadMasterKeyAsc", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        internal static extern Int32 mwKeyPadDownLoadMasterKeyAsc(IntPtr icdev, Int32 MKeyNo, Int32 keyLen, byte[] MasterKey);

        /// <summary>
        /// 下载工作密钥到键盘
        /// </summary>
        /// <param name="icdev">设备通信句柄</param>
        /// <param name="WKeyNo">工作密钥号（0x00-0x02）</param>
        /// <param name="Workkey">经过主密钥加密后的工作密钥,16进制字符串格式
        ///                       DES-长度为16
        ///                       DES- 长度为32/48</param>
        /// <returns>大于等于0成功,小于0失败</returns>
        [DllImport("mwReader.dll", EntryPoint = "mwKeyPadDownLoadWorkKey", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        internal static extern Int32 mwKeyPadDownLoadWorkKey(IntPtr icdev, Int32 WKeyNo, byte[] Workkey);

        /// <summary>
        /// 下载工作密钥到键盘
        /// </summary>
        /// <param name="icdev">设备通信句柄</param>
        /// <param name="WKeyNo">工作密钥号（0x00-0x02）</param>
        /// <param name="keyLen">Workkey的实际长度</param>
        /// <param name="Workkey">工作密钥,字节数组
        ///                       DES-长度为8
        ///                       DES- 长度为16/24</param>
        /// <returns>大于等于0成功,小于0失败</returns>
        [DllImport("mwReader.dll", EntryPoint = "mwKeyPadDownLoadWorkKeyAsc", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        internal static extern Int32 mwKeyPadDownLoadWorkKeyAsc(IntPtr icdev, Int32 WKeyNo, Int32 keyLen, byte[] Workkey);

        /// <summary>
        /// 激活工作密钥
        /// </summary>
        /// <param name="icdev">设备通信句柄</param>
        /// <param name="MKeyNo">主密钥号</param>
        /// <param name="WKeyNo">工作密钥号</param>
        /// <returns>大于等于0成功,小于0失败</returns>
        [DllImport("mwReader.dll", EntryPoint = "mwKeyPadActiveKey", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        internal static extern Int32 mwKeyPadActiveKey(IntPtr icdev, Int32 MKeyNo, Int32 WKeyNo);

        /// <summary>
        /// 获得用户键盘密码的输入,该函数会一直等待用户输入直到超时	
        /// </summary>
        /// <param name="icdev">设备通信句柄</param>
        /// <param name="ctime">等待用户按键输入的超时时间，以second为单位；最大255s，超过该时间退出. 如果为0 则表示使用设备默认超时。</param>
        /// <param name="cmd">语音提示: 
        ///                         0x00-不提示, 关闭密码键盘,其他参数可以填空
        ///                         0x01-请输入密码,同时LCD显示提示信息
        ///                         0x02-请重新输入密码,同时LCD显示提示信息
        ///                         0x03-请输入旧密码,同时LCD显示提示信息
        ///                         0x04-请输入新密码 ,同时LCD显示提示信息
        ///                         0x05-密码修改成功,其他参数可以填空</param>
        /// <param name="passwordLen">指定用户要输入的密码长度，当用户输入足够的长度密码键盘会直接返回。如果为0则表示等待用户确认键.</param>
        /// <param name="cardno">卡号， 参见ANSI X9.8 标准</param>
        /// <param name="cpass">输入的密码，返回的经过工作密钥加密后的数据(16进制字符串格式)， 加密前的数据按照ANSIX9.8标准组装 </param>
        /// <returns>密码位数,小于0失败</returns>
        [DllImport("mwReader.dll", EntryPoint = "mwPassGetInput", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        internal static extern Int32 mwPassGetInput(IntPtr icdev, Int32 ctime, Int32 cmd, Int32 passwordLen, byte[] cardno, byte[] cpass);

        /// <summary>
        /// 获得用户键盘密码的输入,该函数会一直等待用户输入直到超时
        /// </summary>
        /// <param name="icdev"></param>
        /// <param name="ctime">等待用户按键输入的超时时间，以second为单位；最大255s，超过该时间退出. 如果为0 则表示不启用超时。</param>
        /// <param name="cmd">
        /// 语音提示
        ///         0x00-不提示
        ///         0x01-请输入密码,同时LCD显示提示信息
        ///         0x02-请重新输入密码,同时LCD显示提示信息
        ///         0x03-请输入旧密码,同时LCD显示提示信息
        ///         0x04-请输入新密码 ,同时LCD显示提示信息
        /// </param>
        /// <param name="passwordLen">指定用户要输入的密码长度，当用户输入足够的长度密码键盘会直接返回。如果为0，则表示等待用户确认键</param>
        /// <param name="cpass">输入的密码，如果没有下载主密钥/工作密钥并激活，则显示的是明文，否则显示的是加密后的密文数据(16进制字符串格式)</param>
        /// <returns>密码位数（根据客户输入而定）</returns>
        /// <!--该接口默认在第2行显示提示信息，在第3行显示*号，如果用户想要指定密文所在行数请参考mwPassGetInputU()-->
        [DllImport("mwReader.dll", EntryPoint = "mwPassGetInputExt", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        internal static extern Int32 mwPassGetInputExt(IntPtr icdev, Int32 ctime, Int32 cmd, Int32 passwordLen, byte[] cpass);

        /// <summary>
        /// 进入获取键盘密码的状态,进入该状态后只接收 mwPassGet 和 mwPassCancel 命令.
        /// </summary>
        /// <param name="icdev"></param>
        /// <param name="ctime">ctime 等待用户按键输入的超时时间，以second为单位；最大255s，最小1s；超过该时间退出.</param>
        /// <param name="cmd">cmd 语音提示, 
        ///                       0x01-请输入密码,同时LCD显示提示信息
        ///                       0x02-请重新输入密码,同时LCD显示提示信息
        ///                       0x03-请输入旧密码,同时LCD显示提示信息
        ///                       0x04-请输入新密码 ,同时LCD显示提示信息</param>
        /// <param name="cardNo">卡号， 参见ANSI X9.8 标准</param>
        /// <returns> >=0	正确;	&lt;0	错误	</returns>
        [DllImport("mwReader.dll", EntryPoint = "mwPassIn", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        internal static extern Int32 mwPassIn(IntPtr icdev, Int32 ctime, Int32 cmd, byte[] cardNo);

        /// <summary>
        /// 查询和获取输入的密码
        /// </summary>
        /// <param name="icdev">设备通信句柄</param>
        /// <param name="cpass">输入的密码，如果没有下载主密钥/工作密钥并激活，则显示的是明文，否则显示的是加密后的密文数据(16进制字符串格式)</param>
        /// <returns>
        ///     0x00， 成功取得密码，cpass 为加密后的密文密码,rlen 为加密后的密文密码长度
        ///     -0X0031，用户取消密码输入
        ///     -0X0032，用户密码输入操作超时
        ///     -0X0033，未处于密码输入状态
        ///     -0X0034，用户输入密码还未完成，返回按键个数、*号串
        ///</returns>
        ///<!---0X0034 这个返回值很重要,在开始查询中会一直遇到,表示输入还没有完成,可以再次执行mwPassGet函数来获取密码-->
        [DllImport("mwReader.dll", EntryPoint = "mwPassGet", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        internal static extern Int32 mwPassGet(IntPtr icdev, byte[] cpass);

        /// <summary>
        /// 取消键盘密码的状态,执行后设备恢复普通状态
        /// </summary>
        /// <param name="icdev"></param>
        /// <returns>>=0	正确;	&lt;0	错误</returns>
        [DllImport("mwReader.dll", EntryPoint = "mwPassCancel", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        internal static extern Int32 mwPassCancel(IntPtr icdev);
        #endregion

        #region  CPU卡及磁条卡操作
        /// <summary>
        /// 智能卡复位
        /// </summary>
        /// <param name="handle">设备通信句柄</param>
        /// <param name="slotNumber">卡座编号.
        ///             0	:   接触式卡座
        ///             1	:   非接触式大卡座
        ///             2--5:   扩展的第1--4个SAM卡座</param>
        /// <param name="atrInfo">复位信息，格式为字节数组.请最少预留50字节的存储空间.</param>
        /// <param name="opFlag">当slotNumber=1 时表示打开模式：
        ///                             0x00	STD模式,只能寻到空闲状态下的卡片，被激活或停活（Halt）的卡片不会响应
        ///                             0x01	ALL模式,能寻到空闲状态和已经被停活（Halt）的卡片
        /// 当slotNumber为其他值时表示复位波特率</param>
        /// <returns>atrInfo的实际长度,小于0失败</returns>
        [DllImport("mwReader.dll", EntryPoint = "mwSmartCardReset", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        internal static extern Int32 mwSmartCardReset(IntPtr handle, Int32 slotNumber, byte[] atrInfo, Int32 opFlag);

        /// <summary>
        /// 智能卡复位
        /// </summary>
        /// <param name="handle">设备通信句柄</param>
        /// <param name="slotNumber">卡座编号.
        ///             0	:   接触式卡座
        ///             1	:   非接触式大卡座
        ///             2--5:   扩展的第1--4个SAM卡座</param>
        /// <param name="atrInfo">复位信息，格式为16进制字符串.请最少预留100字节的存储空间.</param>
        /// <param name="opFlag">当slotNumber=1 时表示打开模式：
        ///                             0x00	STD模式,只能寻到空闲状态下的卡片，被激活或停活（Halt）的卡片不会响应
        ///                             0x01	ALL模式,能寻到空闲状态和已经被停活（Halt）的卡片
        /// 当slotNumber为其他值时表示复位波特率</param>
        /// <returns>atrInfo的实际长度,小于0失败</returns>
        [DllImport("mwReader.dll", EntryPoint = "mwSmartCardReset_HEX", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        internal static extern Int32 mwSmartCardReset_HEX(IntPtr handle, Int32 slotNumber, StringBuilder atrInfo, Int32 opFlag);

        /// <summary>
        /// 智能卡传输指令
        /// </summary>
        /// <param name="handle">设备通信句柄</param>
        /// <param name="slotNumber">卡座编号.
        ///             0	:   接触式卡座
        ///             1	:   非接触式大卡座
        ///             2--5:   扩展的第1--4个SAM卡座</param>
        /// <param name="srcData">智能卡指令,格式为字节数组</param>
        /// <param name="srcLen">srcData的实际长度</param>
        /// <param name="dstInfo">智能卡返回的应答数据,格式为字节数组</param>
        /// <returns>dstInfo的实际长度,小于等于0失败</returns>
        [DllImport("mwReader.dll", EntryPoint = "mwSmartCardCommand", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        internal static extern Int32 mwSmartCardCommand(IntPtr handle, Int32 slotNumber, byte[] srcData, Int32 srcLen, byte[] dstInfo);

        /// <summary>
        /// 智能卡传输指令
        /// </summary>
        /// <param name="handle">设备通信句柄</param>
        /// <param name="slotNumber">卡座编号.
        ///             0	:   接触式卡座
        ///             1	:   非接触式大卡座
        ///             2--5:   扩展的第1--4个SAM卡座</param>
        /// <param name="srcData">智能卡指令,格式为16进制字符串</param>
        /// <param name="dstInfo">智能卡返回的应答数据,格式为16进制字符串</param>
        /// <returns>dstInfo的实际长度,小于等于0失败</returns>
        [DllImport("mwReader.dll", EntryPoint = "mwSmartCardCommand_HEX", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        internal static extern Int32 mwSmartCardCommand_HEX(IntPtr handle, Int32 slotNumber, string srcData, StringBuilder dstInfo);

        /// <summary>
        /// 智能卡下电
        /// </summary>
        /// <param name="handle">设备通信句柄</param>
        /// <param name="slotNumber">卡座编号.
        ///             0	:   接触式卡座
        ///             1	:   非接触式大卡座
        ///             2--5:   扩展的第1--4个SAM卡座</param>
        /// <returns>大于等于0成功,小于0失败</returns>
        [DllImport("mwReader.dll", EntryPoint = "mwSmartCardPowerDown", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        internal static extern Int32 mwSmartCardPowerDown(IntPtr handle, Int32 slotNumber);

        /// <summary>
        /// 读取磁条卡数据
        /// </summary>
        /// <param name="handle">设备句柄</param>
        /// <param name="ctime">超时时间</param>
        /// <param name="pMagCardData">磁条卡数据,格式如下
        ///                            <TRACK1>data</TRACK1><TRACK2>data</TRACK2><TRACK3>data</TRACK3></param>
        /// <returns></returns>
        [DllImport("mwReader.dll", EntryPoint = "mwReadMagCard", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        internal static extern Int32 mwReadMagCard(IntPtr handle, Int32 ctime, byte[] pMagCardData);

        #endregion

        #region mifare卡操作
        /// <summary>
        /// 打开非接触卡片
        /// </summary>
        /// <param name="icdev">通讯设备句柄</param>
        /// <param name="openMode">
        /// 打开模式：
		///	    0x00	STD模式,只能寻到空闲状态下的卡片，被激活或停活（Halt）的卡片不会响应
		///	    0x01	ALL模式,能寻到空闲状态和已经被停活（Halt）的卡片
        /// </param>
        /// <param name="cardType">卡片类型 </param>
        /// <param name="cardSak">卡片SAK</param>
        /// <param name="cardUid">卡片序列号,格式为byte数组</param>
        /// <returns> >=0	正确;	&lt;0	错误	</returns>
        [DllImport("mwReader.dll", EntryPoint = "mwOpenCard", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        internal static extern Int32 mwOpenCard(IntPtr icdev, Int32 openMode, byte[] cardUid);

        /// <summary>
        /// 将选定的卡片置于HALT模式，需要Request All将其唤醒
        /// </summary>
        /// <param name="icdev">通讯设备句柄</param>
        /// <returns> >=0	正确;	&lt;0	错误</returns>
        [DllImport("mwReader.dll", EntryPoint = "mwHalt", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        internal static extern Int32 mwHalt(IntPtr icdev);

        /// <summary>
        /// 请求卡TYPE A 类型卡片
        /// </summary>
        /// <param name="icdev">通讯设备句柄</param>
        /// <param name="openMode">
        /// 打开模式
		///	    0x00	STD模式,只能寻到空闲状态下的卡片，被激活或停活（Halt）的卡片不会响应
		///	    0x01	ALL模式,能寻到空闲状态和已经被停活（Halt）的卡片
        /// </param>
        /// <param name="cardType">
        /// 卡片类型
        ///     Mifare 标准 1k: 0x0004
		///		Mifare 标准 4k: 0x0002
		///		Mifare Light: 0x0010
		///		Mifare UtraLight: 0x0044
		///		CPU :0x0008
        /// </param>
        /// <returns> >=0	正确;	&lt;0	错误</returns>
        [DllImport("mwReader.dll", EntryPoint = "mwRequest", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        internal static extern Int32 mwRequest(IntPtr icdev, Int32 openMode, out UInt16 cardType);

        /// <summary>
        /// 防冲突
        /// </summary>
        /// <param name="icdev">通讯设备句柄</param>
        /// <param name="cardUid">卡片序列号,格式为byte数组</param>
        /// <returns>卡号长度,如果返回值小于0表示错误，请查看错误代码表</returns>
        [DllImport("mwReader.dll", EntryPoint = "mwAnticoll", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        internal static extern Int32 mwAnticoll(IntPtr icdev, byte[] cardUid);

        /// <summary>
        /// 选卡
        /// </summary>
        /// <param name="icdev">通讯设备句柄</param>
        /// <param name="cardUid">卡片序列号,格式为byte数组</param>
        /// <param name="idLen">卡号长度</param>
        /// <param name="cardSak">卡片SAK</param>
        /// <returns> >=0	正确;	&lt;0	错误</returns>
        [DllImport("mwReader.dll", EntryPoint = "mwSelect", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        internal static extern Int32 mwSelect(IntPtr icdev, byte[] cardUid, Int32 idLen, out byte cardSak);

        /// <summary>
        /// 获取TYPE-A类型的智能卡复位信息
        /// </summary>
        /// <param name="icdev">通讯设备句柄</param>
        /// <param name="atrInfo">卡片复位信息,格式为byte数组 </param>
        /// <returns>复位信息长度,如果返回值小于0表示错误，请查看错误代码表	</returns>
        [DllImport("mwReader.dll", EntryPoint = "mwRats", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        internal static extern Int32 mwRats(IntPtr icdev, byte[] atrInfo);

        /// <summary>
        /// 验证密码
        /// </summary>
        /// <param name="icdev">通讯设备句柄</param>
        /// <param name="mode">
        /// 密码类型：	
		///	    0x00	验证A密码
		///	    0x01	验证B密码
        /// </param>
        /// <param name="sectorNo">要验证的扇区号</param>
        /// <param name="key">6字节长度的密码，格式为byte数组</param>
        /// <returns> >=0	正确;	&lt;0	错误</returns>
        [DllImport("mwReader.dll", EntryPoint = "mwMifareAuth", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        internal static extern Int32 mwMifareAuth(IntPtr icdev, Int32 mode, Int32 sectorNo, byte[] key);

        /// <summary>
        /// 以字符串验证密码
        /// </summary>
        /// <param name="icdev">通讯设备句柄</param>
        /// <param name="mode">
        /// 密码类型：	
		///	    0x00	验证A密码
		///	    0x01	验证B密码
        /// </param>
        /// <param name="sectorNo">要验证的扇区号</param>
        /// <param name="key">以'\0'为结尾的16进制字符串，密码长度应为12。</param>
        /// <returns> >=0	正确;	&lt;0	错误</returns>
        [DllImport("mwReader.dll", EntryPoint = "mwMifareAuthHex", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        internal static extern Int32 mwMifareAuthHex(IntPtr icdev, Int32 mode, Int32 sectorNo, string strKey);

        /// <summary>
        /// 读数据
        /// </summary>
        /// <param name="icdev">通讯设备句柄</param>
        /// <param name="blockNo">要读取的块号，对于S50卡，取值为0～63;对于S70卡，取值为0～255;</param>
        /// <param name="blockData">读取的数据，mifare卡每块数据共16字节。</param>
        /// <returns>数据长度,如果返回值小于0表示错误，请查看错误代码表</returns>
        [DllImport("mwReader.dll", EntryPoint = "mwMifareRead", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        internal static extern Int32 mwMifareRead(IntPtr icdev, Int32 blockNo, byte[] blockData);

        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="icdev">通讯设备句柄</param>
        /// <param name="blockNo">要读取的块号，对于S50卡，取值为0～63;对于S70卡，取值为0～255;</param>
        /// <param name="blockData">要写入的数据。</param>
        /// <returns> >=0	正确;	&lt;0	错误</returns>
        [DllImport("mwReader.dll", EntryPoint = "mwMifareWrite", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 mwMifareWrite(IntPtr icdev, Int32 blockNo, byte[] blockData);

        /// <summary>
        /// 将数据块初始化为值存储区	 
        /// </summary>
        /// <param name="icdev">通讯设备句柄</param>
        /// <param name="blockNo">块号</param>
        /// <param name="initValue">写入的值</param>
        /// <returns> >=0	正确;	&lt;0	错误</returns>
        [DllImport("mwReader.dll", EntryPoint = "mwMifareInitVal", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        internal static extern Int32 mwMifareInitVal(IntPtr icdev, Int32 blockNo, UInt32 initValue);

        /// <summary>
        /// 读值
        /// </summary>
        /// <param name="icdev">通讯设备句柄</param>
        /// <param name="blockNo">块号</param>
        /// <param name="value">读取的数值</param>
        /// <returns> >=0	正确;	&lt;0	错误</returns>
        [DllImport("mwReader.dll", EntryPoint = "mwMifareReadVal", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        internal static extern Int32 mwMifareReadVal(IntPtr icdev, Int32 blockNo, out UInt32 value);

        /// <summary>
        /// 增值
        /// </summary>
        /// <param name="icdev">通讯设备句柄</param>
        /// <param name="blockNo">块号</param>
        /// <param name="value">增加的数值</param>
        /// <returns> >=0	正确;	&lt;0	错误</returns>
        [DllImport("mwReader.dll", EntryPoint = "mwMifareIncrement", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        internal static extern Int32 mwMifareIncrement(IntPtr icdev, Int32 blockNo, UInt32 value);

        /// <summary>
        /// 减值
        /// </summary>
        /// <param name="icdev">通讯设备句柄</param>
        /// <param name="blockNo">块号</param>
        /// <param name="value">减少 的数值</param>
        /// <returns> >=0	正确;	&lt;0	错误</returns>
        [DllImport("mwReader.dll", EntryPoint = "mwMifareDecrement", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        internal static extern Int32 mwMifareDecrement(IntPtr icdev, Int32 blockNo, UInt32 value);

        #endregion

        #region SLE4442卡操作
        /// <summary>
        /// 读4442卡
        /// </summary>
        /// <param name="icdev">通讯设备句柄</param>
        /// <param name="offset">偏移地址，其值范围0～255</param>
        /// <param name="length">字符串长度，其值范围1～256</param>
        /// <param name="data">读出的数据</param>
        /// <returns> >=0	正确;	&lt;0	错误	</returns>
        [DllImport("mwReader.dll", EntryPoint = "mwRead4442", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        internal static extern Int32 mwRead4442(IntPtr icdev, Int32 offset, Int32 length, byte[] data);

        /// <summary>
        /// 写4442卡
        /// </summary>
        /// <param name="icdev">通讯设备句柄</param>
        /// <param name="offset">偏移地址，其值范围0～255</param>
        /// <param name="length">字符串长度，其值范围1～256</param>
        /// <param name="data">写入的数据.如果data的长度大于length,则截取写入;如果data的长度小于length,data后面的到length长度的内存中的数据也会被写入。此时写入的数据可能会有乱码出现。</param>
        /// <returns>>=0	正确;	&lt;0	错误	</returns>
        [DllImport("mwReader.dll", EntryPoint = "mwWrite4442", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        internal static extern Int32 mwWrite4442(IntPtr icdev, Int32 offset, Int32 length, byte[] data);

        /// <summary>
        /// 读4442/4432保护位,字节地址0～31为保护区，共32个字节，每个字节用1 bit的保护位来标志是否被置保护，为0表示已置保护，为1表示未置保护。已置的保护位不可恢复，被保护的数据只可读，不可更改，成为固化数据。
        /// </summary>
        /// <param name="icdev">通讯设备句柄</param>
        /// <param name="offset">偏移地址，其值范围0～31</param>
        /// <param name="length">字符串长度，其值范围1～32</param>
        /// <param name="pData">读出保护位标志，</param>
        /// <returns>数据长度,如果返回值小于0表示错误，请查看错误代码表</returns>
        [DllImport("mwReader.dll", EntryPoint = "mwPRead4442", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        internal static extern Int32 mwPRead4442(IntPtr icdev, Int32 offset, Int32 length, byte[] pData);

        /// <summary>
        /// 4442/4432卡校验数据并写保护
        /// </summary>
        /// <param name="icdev">通讯设备句柄</param>
        /// <param name="offset">偏移地址，其值范围0～31</param>
        /// <param name="length">字符串长度，其值范围1～32</param>
        /// <param name="data">保护数据，必须和卡中已存在的数据一致</param>
        /// <returns> >=0	正确;	&lt;0	错误</returns>
        [DllImport("mwReader.dll", EntryPoint = "mwPWrite4442", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        internal static extern Int32 mwPWrite4442(IntPtr icdev, Int32 offset, Int32 length, byte[] data);

        /// <summary>
        /// 校验密码，4442卡密码长度为3字节.密码核对正确前，全部数据只可读，不可改写。
        /// </summary>
        /// <param name="icdev">通讯设备句柄</param>
        /// <param name="key">密码,长度为3个字节</param>
        /// <returns> >=0	正确;	&lt;0	错误	</returns>
        [DllImport("mwReader.dll", EntryPoint = "mwVerifyPassword4442", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        internal static extern Int32 mwVerifyPassword4442(IntPtr icdev, byte[] key);

        /// <summary>
        /// 读密码，4442卡密码长度为3字节
        /// </summary>
        /// <param name="icdev">通讯设备句柄</param>
        /// <param name="key">读出的密码,长度为3字节</param>
        /// <returns>密码长度,如果返回值小于0表示错误，请查看错误代码表</returns>
        [DllImport("mwReader.dll", EntryPoint = "mwReadPassword4442", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        internal static extern Int32 mwReadPassword4442(IntPtr icdev, byte[] key);

        /// <summary>
        /// 修改密码，4442卡密码长度为3字节
        /// </summary>
        /// <param name="icdev">通讯设备句柄</param>
        /// <param name="key">新密码,长度为3字节</param>
        /// <returns> >=0	正确;	&lt;0	错误	</returns>
        [DllImport("mwReader.dll", EntryPoint = "mwChangePassword4442", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        internal static extern Int32 mwChangePassword4442(IntPtr icdev, byte[] key);

        /// <summary>
        /// 读取错误计数错误计数器，初始值为3，密码核对出错1次，便减1，若计数器值为0,
        /// 则卡自动锁死，数据只可读出，不可再进行更改也无法再进行密码核对；
        /// 若不为零时，有一次密码核对正确，可恢复到初始值3
        /// </summary>
        /// <param name="icdev">通讯设备句柄</param>
        /// <param name="counter">错误计数器的值</param>
        /// <returns></returns>
        [DllImport("mwReader.dll", EntryPoint = "mwGetErrorCounter4442", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        internal static extern Int32 mwGetErrorCounter4442(IntPtr icdev, out Int32 counter);
        #endregion

        #region SLE4428卡操作
        /// <summary>
        /// 读4428/4418卡， 其容量为1024字节
        /// </summary>
        /// <param name="icdev">通讯设备句柄</param>
        /// <param name="offset">偏移地址，其值范围0～1023</param>
        /// <param name="length">length: 字符串长度，其值范围1～1024</param>
        /// <param name="data">读出数据</param>
        /// <returns>数据长度,如果返回值小于0表示错误，请查看错误代码表</returns>
        [DllImport("mwReader.dll", EntryPoint = "mwRead4428", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        internal static extern Int32 mwRead4428(IntPtr icdev, Int32 offset, Int32 length, byte[] data);

        /// <summary>
        /// 写4428/4418卡， 其容量为1024字节
        /// </summary>
        /// <param name="icdev">通讯设备句柄</param>
        /// <param name="offset">偏移地址，其值范围0～1023</param>
        /// <param name="length">字符串长度，其值范围1～1024</param>
        /// <param name="data">写入的数据。如果data的长度大于length,则截取写入;如果data的长度小于length,data后面的到length长度的内存中的数据也会被写入。此时写入的数据可能会有乱码出现。</param>
        /// <returns> >=0	正确;	&lt;0	错误	</returns>
        [DllImport("mwReader.dll", EntryPoint = "mwWrite4428", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        internal static extern Int32 mwWrite4428(IntPtr icdev, Int32 offset, Int32 length, byte[] data);

        /// <summary>
        /// 读4428/4418数据以及保护位
        /// </summary>
        /// <param name="icdev">通讯设备句柄</param>
        /// <param name="offset">偏移地址，其值范围0～31</param>
        /// <param name="length">字符串长度，其值范围1～32</param>
        /// <param name="data">存放要读出的数据,其大小应为2*length </param>
        /// <returns></returns>
        /// <example>
        /// unsigned char databuff[4];
        /// st=mwPReadData4428(icdev,0,2,databuff);
        /// 从偏移地址0开始带保护位读出2个字节数据放入databuff中，每个字节的后面跟一个保护位标志字节，该字节值为0x00表示,相应的字节已保护，0xff表示未被保护。
        /// 例如：读出
        /// databuff[0]=0x13,databuff[1]=0x00,
        /// databuff[2]=0x20,databuff[3]=0xff;
        /// 表示偏移地址0字节被保护，偏移地址1字节未被保护。
        /// </example>
        [DllImport("mwReader.dll", EntryPoint = "mwPReadData4428", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        internal static extern Int32 mwPReadData4428(IntPtr icdev, Int32 offset, Int32 length, byte[] data);

        /// <summary>
        /// 428/4418卡校验数据并写保护
        /// </summary>
        /// <param name="icdev">通讯设备句柄</param>
        /// <param name="offset">偏移地址，其值范围0～1023</param>
        /// <param name="length">字符串长度，其值范围1～1024</param>
        /// <param name="data">保护数据，必须和卡中已存在的数据一致</param>
        /// <returns> >=0	正确;	&lt;0	错误	</returns>
        [DllImport("mwReader.dll", EntryPoint = "mwPWrite4428", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        internal static extern Int32 mwPWrite4428(IntPtr icdev, Int32 offset, Int32 length, byte[] data);

        /// <summary>
        /// 4428/4418卡写数据并置保护
        /// </summary>
        /// <param name="icdev">通讯设备句柄</param>
        /// <param name="offset">偏移地址，其值范围0～1023</param>
        /// <param name="length">字符串长度，其值范围1～1024</param>
        /// <param name="data">要写入的数据</param>
        /// <returns>数据长度,如果返回值小于0表示错误，请查看错误代码表</returns>
        [DllImport("mwReader.dll", EntryPoint = "mwPWriteData4428", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        internal static extern Int32 mwPWriteData4428(IntPtr icdev, Int32 offset, Int32 length, byte[] data);

        /// <summary>
        /// 校验密码，4428卡密码长度为2字节.密码核对正确前，全部数据只可读，不可改写。核对密码正确后可以更改数据，包括密码再内。
        /// </summary>
        /// <param name="icdev">通讯设备句柄</param>
        /// <param name="key">密码,长度为2个字节</param>
        /// <returns> >=0	正确;	&lt;0	错误	 </returns>
        [DllImport("mwReader.dll", EntryPoint = "mwVerifyPassword4428", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        internal static extern Int32 mwVerifyPassword4428(IntPtr icdev, byte[] key);

        /// <summary>
        /// 读密码，4428卡密码长度为2字节
        /// </summary>
        /// <param name="icdev">通讯设备句柄</param>
        /// <param name="key">读出的密码,长度为2个字节</param>
        /// <returns>密码长度,如果返回值小于0表示错误，请查看错误代码表</returns>
        [DllImport("mwReader.dll", EntryPoint = "mwReadPassword4428", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        internal static extern Int32 mwReadPassword4428(IntPtr icdev, byte[] key);

        /// <summary>
        /// 修改密码，4428卡密码长度为2字节
        /// </summary>
        /// <param name="icdev">通讯设备句柄</param>
        /// <param name="key">新密码,长度为2个字节</param>
        /// <returns> >=0	正确;	&lt;0	错误	</returns>
        [DllImport("mwReader.dll", EntryPoint = "mwChangePassword4428", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        internal static extern Int32 mwChangePassword4428(IntPtr icdev, byte[] key);

        /// <summary>
        /// 读取错误计数错误计数器，初始值为8，密码核对出错1次，便减1，若计数器值为0
        /// 则卡自动锁死，数据只可读出，不可再进行更改也无法再进行密码核对；
        /// 若不为零时，有一次密码核对正确，可恢复到初始值8
        /// </summary>
        /// <param name="icdev">通讯设备句柄</param>
        /// <param name="counter">密码错误计数器的值</param>
        /// <returns> >=0	正确;	&lt;0	错误	</returns>
        [DllImport("mwReader.dll", EntryPoint = "mwGetErrorCounter4428", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        internal static extern Int32 mwGetErrorCounter4428(IntPtr icdev, out Int32 counter);
        #endregion

        #region 24C系列卡片操作
        /// <summary>
        /// 读24CXX系列卡片
        /// </summary>
        /// <param name="icdev">通讯设备句柄</param>
        /// <param name="offset">偏移地址</param>
        /// <param name="length">要读取数据长度</param>
        /// <param name="data">返回读取到的数据</param>
        /// <returns> data的实际长度。&lt;0	错误	</returns>
        /// <!--
        /// AT24CXX卡（XX为01A、02、04、08、16、32、64、128、256）是XXKb位的非加密存储器卡，只存在读、写两种操作。
        ///                 AT24C01A 卡容量: 01 * 1024/8 = 128 字节
        ///                 AT24C02 卡容量: 02 * 1024/8 = 256 字节
        ///                 AT24C04 卡容量: 04 * 1024/8 = 512 字节
        ///                 AT24C08 卡容量: 08 * 1024/8 = 1024 字节
        ///                 AT24C16 卡容量: 16 * 1024/8 = 2048 字节
        ///                 AT24C32 卡容量: 32 * 1024/8 = 4096 字节
        ///                 AT24C64 卡容量: 64 * 1024/8 = 8192 字节
        ///                 AT24C128 卡容量: 128 * 1024/8 = 16384 字节
        ///                 AT24C256 卡容量: 256 * 1024/8 = 32768 字节
        /// -->
        [DllImport("mwReader.dll", EntryPoint = "mwRead24CXX", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        internal static extern Int32 mwRead24CXX(IntPtr icdev, Int32 cardType, Int32 offset, Int32 length, byte[] data);

        /// <summary>
        /// 写24CXX系列卡片
        /// </summary>
        /// <param name="icdev">通讯设备句柄</param>
        /// <param name="offset">偏移地址</param>
        /// <param name="length">数据长度</param>
        /// <param name="data">数据缓冲区.如果data的长度大于length,则截取写入;如果data的长度小于length,data后面的到length长度的内存中的数据也会被写入。此时写入的数据可能会有乱码出现。</param>
        /// <returns> >=0	正确;	&lt;0	错误	</returns>
        /// <remarks>
        /// AT24CXX卡（XX为01A、02、04、08、16、32、64、128、256）是XXKb位的非加密存储器卡，只存在读、写两种操作。
        ///                  AT24C01A 卡容量: 01 * 1024/8 = 128 字节
        ///                 AT24C02 卡容量: 02 * 1024/8 = 256 字节
        ///                 AT24C04 卡容量: 04 * 1024/8 = 512 字节
        ///                 AT24C08 卡容量: 08 * 1024/8 = 1024 字节
        ///                 AT24C16 卡容量: 16 * 1024/8 = 2048 字节
        ///                 AT24C32 卡容量: 32 * 1024/8 = 4096 字节
        ///                 AT24C64 卡容量: 64 * 1024/8 = 8192 字节
        ///                 AT24C128 卡容量: 128 * 1024/8 = 16384 字节
        ///                 AT24C256 卡容量: 256 * 1024/8 = 32768 字节
        /// </remarks>
        [DllImport("mwReader.dll", EntryPoint = "mwWrite24CXX", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        internal static extern Int32 mwWrite24CXX(IntPtr icdev, Int32 cardType, Int32 offset, Int32 length, byte[] data);
        #endregion

        #region 工具方法
        /// <summary>
        /// 将一个byte数据转化为2个字节的ASCII.例如:输入数据为0X3a,则转化后的数据为0x33,0x41,即字符串"3A"
        /// </summary>
        /// <param name="src">要被转换的数据</param>
        /// <param name="dst">保存转换后的16进制ASCII字符串,该存储空间至少是 srcLen*2+1 个字节的长度</param>
        /// <param name="srcLen">src长度</param>
        /// <returns>转换后的字符串长度,如果返回值小于0表示错误,请查看错误代码表</returns>
        [DllImport("mwReader.dll", EntryPoint = "BinToHex", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        internal static extern Int32 BinToHex(byte[] src, StringBuilder dst, Int32 srcLen);

        /// <summary>
        /// 将16进制ASCII字符串转化为byte数组。例如：输入数据为"32"，则转化后的数据为0x32
        /// </summary>
        /// <param name="src">要被转换的16进制ASCII字符串</param>
        /// <param name="dst">保存转换后的byte数组。</param>
        /// <param name="srcLen">src长度,srcLen为0取字符串实际长度</param>
        /// <returns>转换后的字节长度,如果返回值小于0表示错误，请查看错误代码表</returns>
        /// <remarks>函数具有较强的容错性，它将忽略16进制字符串中的所有非法字符</remarks>
        [DllImport("mwReader.dll", EntryPoint = "HexToBin", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        internal static extern Int32 HexToBin(string src, byte[] dst, Int32 srcLen);

        /// <summary>
        /// 16进制字符串Des/3Des加解密
        /// </summary>
        /// <param name="flag">
        ///     00  DES  解密
        ///     01  DES  加密
        ///     10  3DES 解密
        ///     11  3DES 加密
        /// </param>
        /// <param name="key">密码，格式为16进制字符串</param>
        /// <param name="ptrsource">原文，格式为16进制字符串</param>
        /// <param name="ptrdest">加解密后的数据，格式为16进制字符串</param>
        /// <returns></returns>
        /// <remarks>DES加密/解密key的长度为16，3DES加解密key的长度为32或48</remarks>
        [DllImport("mwReader.dll", EntryPoint = "mwEntrdes_HEX", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        internal static extern Int32 mwEntrdes_HEX(Int32 flag, byte[] key, byte[] ptrsource, byte[] ptrdest);

        /// <summary>
        /// Des/3Des加解密
        /// </summary>
        /// <param name="flag"> 
        /// 00  DES  解密
        /// 01  DES  加密
        /// 10  3DES 解密
        /// 11  3DES 加密
        /// </param>
        /// <param name="key">密码，格式为二进制。</param>
        /// <param name="keyLen">密码长度，DES加密/解密为8，3DES为16或24</param>
        /// <param name="ptrsource">原文</param>
        /// <param name="msglen">原文长度必需为8的整数倍</param>
        /// <param name="ptrdest">加/解密出的数据</param>
        /// <returns></returns>
        [DllImport("mwReader.dll", EntryPoint = "mwEntrdes", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        internal static extern Int32 mwEntrdes(Int32 flag, byte[] key, Int32 keyLen, byte[] ptrsource, Int32 msglen, byte[] ptrdest);
        #endregion

        #region 102卡操作
        /// <summary>
        /// 通用读卡操作，按照卡内的绝对地址来寻址。
        /// </summary>
        /// <param name="icdev">打开的设备句柄</param>
        /// <param name="offset">起始偏移地址，最大为0x00B2</param>
        /// <param name="length">读取的字节数，与偏移地址相加，不能超过0x00B2</param>
        /// <param name="data">读取到的数据</param>
        /// <returns>大于等于0 正确; 小于0  错误</returns>
        [DllImport("mwReader.dll", EntryPoint = "mwReadAT88SC102", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        internal static extern Int32 mwReadAT88SC102(IntPtr icdev, Int32 offset, Int32 length, byte[] data);

        /// <summary>
        /// 通用写卡操作，按照卡内的绝对地址来寻址。
        /// </summary>
        /// <param name="icdev">打开的设备句柄</param>
        /// <param name="offset">起始偏移地址，最大为0x00B2</param>
        /// <param name="length">写入的字节数，与偏移地址相加，不能超过0x00B2</param>
        /// <param name="data">要写入的数据.如果data的长度大于length,则截取写入;如果data的长度小于length,data后面的到length长度的内存中的数据也会被写入。此时写入的数据可能会有乱码出现</param>
        /// <returns>大于等于0 正确; 小于0  错误</returns>
        [DllImport("mwReader.dll", EntryPoint = "mwWriteAT88SC102", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        internal static extern Int32 mwWriteAT88SC102(IntPtr icdev, Int32 offset, Int32 length, byte[] data);

        /// <summary>
        /// 通用擦除操作，按照卡内的绝对地址来寻址。
        /// </summary>
        /// <param name="icdev">打开的设备句柄</param>
        /// <param name="offset">起始偏移地址，最大为0x00B2</param>
        /// <param name="length">擦除的字节数，与偏移地址相加，不能超过0x00B2</param>
        /// <returns>大于等于0 正确; 小于0  错误 </returns>
        [DllImport("mwReader.dll", EntryPoint = "mwEraseAT88SC102", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        internal static extern Int32 mwEraseAT88SC102(IntPtr icdev, Int32 offset, Int32 length);

        /// <summary>
        /// 读密码
        /// </summary>
        /// <param name="icdev">打开的设备句柄</param>
        /// <param name="type">0x00 - 主密码,长度为2字节 0x01 - 应用区1密码,长度为6字节  0x02 - 应用区2密码,长度为4字节</param>
        /// <param name="password">读取到的密码</param>
        /// <returns>大于等于0 正确; 小于0  错误</returns>
        [DllImport("mwReader.dll", EntryPoint = "mwReadPasswordAT88SC102", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        internal static extern Int32 mwReadPasswordAT88SC102(IntPtr icdev, Int32 type, byte[] password);

        /// <summary>
        /// 校验密码
        /// </summary>
        /// <param name="icdev">打开的设备句柄</param>
        /// <param name="type">0x00 - 主密码,长度为2字节 0x01 - 应用区1密码,长度为6字节 0x02 - 应用区2密码,长度为4字节</param>
        /// <param name="password">卡片密码</param>
        /// <returns>大于等于0 正确; 小于0  错误</returns>
        [DllImport("mwReader.dll", EntryPoint = "mwVerifyPasswordAT88SC102", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        internal static extern Int32 mwVerifyPasswordAT88SC102(IntPtr icdev, Int32 type, byte[] password);

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="icdev">打开的设备句柄</param>
        /// <param name="type">0x00 - 主密码,长度为2字节 0x01 - 应用区1密码,长度为6字节 0x02 - 应用区2密码,长度为4字节</param>
        /// <param name="password">卡片密码</param>
        /// <returns>大于等于0 正确; 小于0  错误</returns>
        [DllImport("mwReader.dll", EntryPoint = "mwChangePasswordAT88SC102", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        internal static extern Int32 mwChangePasswordAT88SC102(IntPtr icdev, Int32 type, byte[] password);

        /// <summary>
        /// 读应用区2的擦除计数，卡片个人化之前该计数不起作用。
        /// </summary>
        /// <param name="icdev">打开的设备句柄</param>
        /// <param name="count">擦除计数</param>
        /// <returns>大于等于0 正确; 小于0  错误</returns>
        [DllImport("mwReader.dll", EntryPoint = "mwReadEraseCountAT88SC102", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        internal static extern Int32 mwReadEraseCountAT88SC102(IntPtr icdev, out Int32 count);

        /// <summary>
        /// 读密码错误计数，初始值为4，用户密码核对出错1次，则计数器值减1，连续4次出错，卡片会自动锁死；如果其中对1次，则恢复为4
        /// </summary>
        /// <param name="icdev">打开的设备句柄</param>
        /// <param name="count">密码错误计数</param>
        /// <returns>大于等于0 正确; 小于0  错误</returns>
        [DllImport("mwReader.dll", EntryPoint = "mwReadErrorCountAT88SC102", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        internal static extern Int32 mwReadErrorCountAT88SC102(IntPtr icdev, out Int32 count);

        /// <summary>
        /// 按分区来读102卡，地址0为各扇区的起始地址
        /// </summary>
        /// <param name="icdev">打开的设备句柄</param>
        /// <param name="sector">
        /// 0x00 - 厂商代码区，共2字节，本区域一般情况下已固化，只可读；
        ///                  0x01 - 发行商代码区，共8字节；
        ///                   0x02 - 代码保护区，共8字节；
        ///                   0x03 - 应用区1，共64字节；
        ///                   0x04 - 应用区2，共64字节；
        ///                   0x05 - 测试区，共2字节
        /// </param>
        /// <param name="offset">起始偏移的字节地址，从0开始。</param>
        /// <param name="length">要读的字节数，与偏移地址相加，不能超过该分区的总长度</param>
        /// <param name="data">读取到的分区数据</param>
        /// <returns>大于等于0 正确; 小于0  错误</returns>
        [DllImport("mwReader.dll", EntryPoint = "mwReadPartitionAT88SC102", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        internal static extern Int32 mwReadPartitionAT88SC102(IntPtr icdev, Int32 sector, Int32 offset, Int32 length, byte[] data);

        /// <summary>
        /// 按分区来写102卡，地址0为各扇区的起始地址
        /// </summary>
        /// <param name="icdev">打开的设备句柄</param>
        /// <param name="sector">
        /// 0x00 - 厂商代码区，共2字节，本区域一般情况下已固化，只可读；
        ///                  0x01 - 发行商代码区，共8字节；
        ///                   0x02 - 代码保护区，共8字节；
        ///                   0x03 - 应用区1，共64字节；
        ///                   0x04 - 应用区2，共64字节；
        ///                   0x05 - 测试区，共2字节
        /// </param>
        /// <param name="offset">起始偏移的字节地址，从0开始。</param>
        /// <param name="length">要写的字节数，与偏移地址相加，不能超过该分区的总长度</param>
        /// <param name="data">要写入的数据</param>
        /// <returns>大于等于0 正确; 小于0  错误</returns>
        [DllImport("mwReader.dll", EntryPoint = "mwWritePartitionAT88SC102", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        internal static extern Int32 mwWritePartitionAT88SC102(IntPtr icdev, Int32 sector, Int32 offset, Int32 length, byte[] data);

        /// <summary>
        /// 模拟熔断操作，通过FUSE引脚控制，将卡虚拟成Level 2模式，当取消模拟时，恢复Level 1模式。
        /// </summary>
        /// <param name="icdev">打开的设备句柄</param>
        /// <param name="flag">0x00 - 取消模拟  0x01 - 模拟个人化</param>
        /// <returns>大于等于0 正确; 小于0  错误</returns>
        [DllImport("mwReader.dll", EntryPoint = "mwSimulationPersonalizationAT88SC102", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        internal static extern Int32 mwSimulationPersonalizationAT88SC102(IntPtr icdev, Int32 flag);

        /// <summary>
        /// 个人化,此操作不可逆，需慎重。执行此指令前需先验证主密码
        /// </summary>
        /// <param name="icdev">打开的设备句柄</param>
        /// <returns>大于等于0 正确; 小于0  错误</returns>
        [DllImport("mwReader.dll", EntryPoint = "mwPersonalizationAT88SC102", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        internal static extern Int32 mwPersonalizationAT88SC102(IntPtr icdev);

        /// <summary>
        /// 将应用区的读保护位设置为0，设置后，需要验证主密码才可读应用区。
        /// </summary>
        /// <param name="icdev">打开的设备句柄</param>
        /// <param name="sector">0x01 - 用户区1 0x02 - 用户区2</param>
        /// <returns>大于等于0 正确; 小于0  错误</returns>
        [DllImport("mwReader.dll", EntryPoint = "mwInitReadControlAT88SC102", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        internal static extern Int32 mwInitReadControlAT88SC102(IntPtr icdev, Int32 sector);

        /// <summary>
        /// 将应用区的写保护位设置为0，设置后，Level 2模式下应用区不可写。
        /// </summary>
        /// <param name="icdev">打开的设备句柄</param>
        /// <param name="sector">0x01 - 用户区1 0x02 - 用户区2</param>
        /// <returns>大于等于0 正确; 小于0  错误</returns>
        [DllImport("mwReader.dll", EntryPoint = "mwInitWriteControlAT88SC102", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        internal static extern Int32 mwInitWriteControlAT88SC102(IntPtr icdev, Int32 sector);

        #endregion


        /// <summary>
        /// 封装方法。获取错误代码信息
        /// </summary>
        /// <param name="errCode">错误代码</param>
        /// <returns>返回错误信息</returns>
        internal static string getErrMsg(int errCode)
        {
            StringBuilder message = new StringBuilder();
            getErrDescription(errCode, 0, message);
            return message.ToString();
        }
    }
}
