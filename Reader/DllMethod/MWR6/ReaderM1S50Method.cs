using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardwareControl.Reader.DllMethod.MWR6
{
    public class ReaderM1S50Method: BaseMethodM1S50, IDisposable
    {
        IntPtr handle = new IntPtr();
        public override string CardId { get; set; }
        public override UInt16 CardType { get; set; }
        public override string CardSak { get; set; }

        public override string AtrInfo { get; set; }

        private Int32 _mode=1;
        
        public override Int32 Mode
        {
            get
            {
                return _mode;
            }

            set
            {
                _mode = value;
            }
        } 


        #region 设备控制

        /// <summary>
        /// 连接读卡器
        /// </summary>
        /// <param name="msg">输出信息</param>
        /// <returns>是否成功</returns>
        public override bool DevOpen(out string msg)
        {
            msg = "读卡器连接成功！";
            bool res = true;
            int st = 0;
            st = ReaderDllMethod.mwDevOpen("USB", "", out handle); //打开读写器
            if (st < 0)
            {
                msg = ReaderDllMethod.getErrMsg(st);
                res = false;
            }
            else
            {
                DevBeep(1, 1, 3);
            }
            return res;
        }

        /// <summary>
        /// 蜂鸣控制
        /// </summary>
        /// <param name="times">次数</param>
        /// <param name="interval">间隔</param>
        /// <param name="time">时长</param>
        public override bool DevBeep(int times, int interval, int time)
        {
            bool res = true;
            int st = ReaderDllMethod.mwDevBeep(handle, times, interval, time);
            if (st < 0)
            {
                res = false;
            }
            return res;
        }

        /// <summary>
        /// 关闭设备
        /// </summary>
        public override bool DevClose()
        {
            bool res = true;
            int st = ReaderDllMethod.mwDevClose(handle); //关闭设备
            if (st < 0)
            { res = false; }
            else
            {
                DevBeep(1, 1, 3);
            }
            return res;
        }

        /// <summary>
        /// 获取设备序列号
        /// </summary>
        /// <param name="msg">输出信息</param>
        /// <returns>是否成功</returns>
        public override bool GetDevSerialNumber(out string msg)
        {
            bool res = true;
            int st = 0;
            byte[] serialNumber = new byte[16];
            st = ReaderDllMethod.mwDevGetSerialNumber(handle, serialNumber);
            if (st < 0)
            {
                msg = st.ToString() + "*" + ReaderDllMethod.getErrMsg(st);
                res = false;
            }
            else
            {
                msg = "1*" + Encoding.Default.GetString(serialNumber, 0, st);
            }
            return res;
        }


        /// <summary>
        /// 获取硬件版本号
        /// <param name="msg">输出信息</param>
        /// <returns></returns>
        public override bool GetDevVer(out string msg)
        {
            bool res = true;
            int st = 0;
            byte[] hardwareVer = new byte[24];
            st = ReaderDllMethod.mwDevGetHardwareVer(handle, hardwareVer);
            if (st < 0)
            {
                msg = st.ToString() + "*" + ReaderDllMethod.getErrMsg(st);
                res = false;
            }
            else
            {
                msg = "1*" + Encoding.Default.GetString(hardwareVer, 0, st);
            }
            return res;
        }

        public void Dispose()
        {
            DevClose();
        }


        #endregion


        #region mifare卡操作

        public override bool OpenCard(out string msg)
        {
            bool res = true;
            int st = 0;
            byte[] cardUid = new byte[10];
            st = ReaderDllMethod.mwOpenCard(handle, Mode, cardUid); //打开卡片
            if (st < 0)
            {
                msg=st.ToString()+"*"+ReaderDllMethod.getErrMsg(st);
                res = false;
            }
            else
            {
                StringBuilder cardUidStr = new StringBuilder();
                ReaderDllMethod.BinToHex(cardUid, cardUidStr, st);//将卡号转换为16进制字符串
                msg = st.ToString() + "*" + ReaderDllMethod.getErrMsg(st);
                CardId =cardUidStr.ToString();
                DevBeep(1, 1, 2);
            }

            return res;
        }


        public override bool Halt(out string msg)
        {
            bool res = true;
            int st = 0;
            st = ReaderDllMethod.mwHalt(handle); //将选定的卡片置于HALT模式，需要Request All将其唤醒
            if (st < 0)
            {
                msg = st.ToString() + "*" + ReaderDllMethod.getErrMsg(st);
                res = false;
            }
            else
            {
               
                msg = st.ToString() + "*" + ReaderDllMethod.getErrMsg(st);
               
            }
            return res;
        }


        public override bool Request( out string msg)
        {
            bool res = true;
            int st = 0;
            ushort cardtype;
            st = ReaderDllMethod.mwRequest(handle, Mode, out cardtype); //请求卡TYPE A 类型卡片
            if (st < 0)
            {
                msg = st.ToString() + "*" + ReaderDllMethod.getErrMsg(st);
                res = false;
            }
            else
            {

                msg = st.ToString() + "*" + ReaderDllMethod.getErrMsg(st);
                CardType = cardtype;
                DevBeep(1, 1, 2);
            }
            return res;
        }


        public override bool Anticoll(out string msg)
        {
            bool res = true;
            int st = 0;
            byte[] cardUid = new byte[10];
            if (!string.IsNullOrEmpty(CardId))
            {
                msg = "CardID is empty";
                return false;
            }
            ReaderDllMethod.HexToBin(CardId, cardUid, CardId.Length);
            st = ReaderDllMethod.mwAnticoll(handle, cardUid); //请求卡TYPE A 类型卡片
            if (st < 0)
            {
                msg = st.ToString() + "*" + ReaderDllMethod.getErrMsg(st);
                res = false;
            }
            else
            {

                msg = st.ToString() + "*" + ReaderDllMethod.getErrMsg(st);
                DevBeep(1, 1, 2);
            }
            return res;
        }

        public override bool Select(out string msg)
        {
            bool res = true;
            int st = 0;
            byte[] cardUid = new byte[10];
            if (!string.IsNullOrEmpty(CardId))
            {
                msg = "CardID is empty";
                return false;
            }
            ReaderDllMethod.HexToBin(CardId, cardUid, CardId.Length);
            byte [] cardsak = new byte[1];
            st = ReaderDllMethod.mwSelect(handle, cardUid, CardId.Length,out cardsak[0]); //请求卡TYPE A 类型卡片
            if (st < 0)
            {
                msg = st.ToString() + "*" + ReaderDllMethod.getErrMsg(st);
                res = false;
            }
            else
            {

                msg = st.ToString() + "*" + ReaderDllMethod.getErrMsg(st);
                StringBuilder cardStr = new StringBuilder();
                ReaderDllMethod.BinToHex(cardsak, cardStr, st);
                CardSak = cardStr.ToString();
                DevBeep(1, 1, 2);
            }
            return res;
        }

        public override bool Rats(out string msg)
        {
            bool res = true;
            int st = 0;
            byte[] cardUid = new byte[50];
            
            st = ReaderDllMethod.mwRats(handle, cardUid); //获取TYPE-A类型的智能卡复位信息
            if (st < 0)
            {
                msg = st.ToString() + "*" + ReaderDllMethod.getErrMsg(st);
                res = false;
            }
            else
            {

                msg = st.ToString() + "*" + ReaderDllMethod.getErrMsg(st);
                StringBuilder cardStr = new StringBuilder();
                ReaderDllMethod.BinToHex(cardUid, cardStr, st);
                AtrInfo = cardStr.ToString();
                DevBeep(1, 1, 2);
            }
            return res;
        }

        public override bool MifareAuth(Int32 sectorNo, byte[] key,out string msg)
        {
            bool res = true;
            int st = 0;
            
            st = ReaderDllMethod.mwMifareAuth(handle, Mode, sectorNo,key); //验证密码
            if (st < 0)
            {
                msg = st.ToString() + "*" + ReaderDllMethod.getErrMsg(st);
                res = false;
            }
            else
            {

                msg = st.ToString() + "*" + ReaderDllMethod.getErrMsg(st);
                DevBeep(1, 1, 2);
            }
            return res;
        }

        public override bool MifareAuthHex(Int32 sectorNo, string strKey, out string msg)
        {
            bool res = true;
            int st = 0;

            st = ReaderDllMethod.mwMifareAuthHex(handle, 0, sectorNo, strKey); //验证密码
            if (st < 0)
            {
                msg = st.ToString() + "*" + ReaderDllMethod.getErrMsg(st);
                res = false;
            }
            else
            {

                msg = st.ToString() + "*" + ReaderDllMethod.getErrMsg(st);
                DevBeep(1, 1, 2);
            }
            return res;
        }


        public override bool MifareRead(Int32 blockNo, byte[] blockData, out string msg)
        {
            bool res = true;
            int st = 0;

            st = ReaderDllMethod.mwMifareRead(handle, blockNo, blockData); //读数据
            if (st < 0)
            {
                msg = st.ToString() + "*" + ReaderDllMethod.getErrMsg(st);
                res = false;
            }
            else
            {

                msg = st.ToString() + "*" + ReaderDllMethod.getErrMsg(st);
                DevBeep(1, 1, 2);
            }
            return res;
        }

        public override string GetSectionData(Int32 sectorNo, string strKey1, string strKey2, out string msg)
        {
            StringBuilder sb = new StringBuilder();
            bool ispass = MifareAuthHex(sectorNo, strKey1, out msg);
            if(!ispass)
            {
                ispass = MifareAuthHex(sectorNo, strKey2, out msg);
            }
            if (ispass)
            {
                byte[] blockData = new byte[16];
                for (int i = 0; i < 4; i++)
                {
                    if (MifareRead(sectorNo * 4+i, blockData, out msg))
                    {
                        if (i>0)
                        {
                            sb.Append("|");
                        }
                        sb.Append(BinToHex(blockData));
                    }
                    else
                    {
                        sb.Length = 0;
                    }
                }
            }
            
            return sb.ToString();
        }

        public override bool MifareWrite(Int32 blockNo, byte[] blockData, out string msg)
        {
            bool res = true;
            int st = 0;

            st = ReaderDllMethod.mwMifareWrite(handle, blockNo, blockData); //读数据
            if (st < 0)
            {
                msg = st.ToString() + "*" + ReaderDllMethod.getErrMsg(st);
                res = false;
            }
            else
            {

                msg = st.ToString() + "*" + ReaderDllMethod.getErrMsg(st);
                DevBeep(1, 1, 2);
            }
            return res;
        }

        public override bool ChangePassword(Int32 sectionNo,byte[] key, out string msg)
        {
            return MifareWrite(sectionNo * 4 + 3, key, out msg);
        }

        public override string BinToHex(byte [] key)
        {
            StringBuilder cardStr = new StringBuilder();
            ReaderDllMethod.BinToHex(key, cardStr, key.Length);
            return cardStr.ToString();
        }

        public override byte[] HexToBin(string hex)
        {
            byte[] bin=new byte[hex.Length/2];
            ReaderDllMethod.HexToBin(hex, bin, hex.Length);

            return bin;
        }

       

        

        #endregion
    }
}
