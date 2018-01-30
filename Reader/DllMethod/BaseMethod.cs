using System;

namespace HardwareControl.Reader.DllMethod
{
  
    public abstract class BaseMethodM1S50
    {
        #region 属性

        public abstract string CardId { get; set; }
        public abstract UInt16 CardType { get; set; }
        public abstract string CardSak { get; set; }

        public abstract string AtrInfo { get; set; }

        public abstract Int32 Mode { get; set; }

        #endregion

        #region 设备
        public abstract bool DevOpen(out string msg);

        public abstract bool DevBeep(int times, int interval, int time);

        public abstract bool DevClose();

        public abstract bool GetDevSerialNumber(out string msg);

        public abstract bool GetDevVer(out string msg);

        #endregion

        #region 卡操作

        public abstract bool OpenCard(out string msg);
        public abstract bool Halt(out string msg);

        public abstract bool Request(out string msg);

        public abstract bool Anticoll(out string msg);

        public abstract bool Select(out string msg);

        public abstract bool Rats(out string msg);

        public abstract bool MifareAuth(Int32 sectorNo, byte[] key, out string msg);

        public abstract bool MifareAuthHex(Int32 sectorNo, string strKey, out string msg);

        public abstract bool MifareRead(Int32 blockNo, byte[] blockData, out string msg);

        public abstract string GetSectionData(Int32 sectorNo, string strKey1, string strKey2, out string msg);

        public abstract bool MifareWrite(Int32 blockNo, byte[] blockData, out string msg);

        public abstract bool ChangePassword(Int32 sectionNo, byte[] key, out string msg);

        public abstract string BinToHex(byte[] key);

        public abstract byte[] HexToBin(string hex);

        #endregion

    }

}
