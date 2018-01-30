using HardwareControl.Reader.DllMethod.MWR6;
using HardwareControl.Reader.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardwareControl.Reader.Repository.Model
{
    public class ZY2000Section0: SectionBase
    {

        #region 构造函数
        public ZY2000Section0(object driver,IPassword pass) :base(0,driver)
        {
            Block0 = new ZY2000Block00(driver);
            
            Block1 = new ZY2000Block01(driver);
            
            Block2 = new ZY2000Block02(driver);
            
            Block3 = new ZY2000Block3(driver,pass);
            
        }

        

        #endregion

        #region 属性成员

        public ZY2000Block00 Block0
        {
            get
            {
                return GetValue<ZY2000Block00>("Block0");
            }
            set
            {
                SetValue<ZY2000Block00>("Block0", value);
            }
        }

        public ZY2000Block01 Block1
        {
            get
            {
                return GetValue<ZY2000Block01>("Block1");
            }
            set
            {
                SetValue<ZY2000Block01>("Block1", value);
            }
        }

        public ZY2000Block02 Block2
        {
            get
            {
                return GetValue<ZY2000Block02>("Block2");
            }
            set
            {
                SetValue<ZY2000Block02>("Block2", value);
            }
        }

        public ZY2000Block3 Block3
        {
            get
            {
                return GetValue<ZY2000Block3>("Block3");
            }
            set
            {
                SetValue<ZY2000Block3>("Block3", value);
            }
        }

        #endregion

        #region 公共方法

        

        public override void LoadData(string data)
        {
            //base.LoadData(data);
            string[] strs = data.Split(new char[] { '|' });
            this.Block0.LoadData(strs[0]);
            this.Block1.LoadData(strs[1]);
            this.Block2.LoadData(strs[2]);
            this.Block3.LoadData(strs[3]);
        }


        public override string GetData()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(this.Block0.GetData());
            sb.Append("|");
            sb.Append(this.Block1.GetData());
            sb.Append("|");
            sb.Append(this.Block2.GetData());
            sb.Append("|");
            sb.Append(this.Block3.GetData());
            return sb.ToString();
        }


        public override bool SaveData(string fid, string cardid, bool IsChangingPass, string dbcontrolstr = null)
        {
            bool result = false;
            if (_driver is ReaderM1S50Method)
            {
                string msg = string.Empty;
                ReaderM1S50Method reader = _driver as ReaderM1S50Method;
                //if (reader.MifareAuthHex(SectionNo, string.Format("{0}{1}{2}",this.Block3.CurrentPasswordA,this.Block3.CurrentControlStr,this.Block3.CurrentPasswordB), out msg))
                if (reader.MifareAuthHex(SectionNo, this.Block3.CurrentPasswordA, out msg) || reader.MifareAuthHex(SectionNo, this.Block3.CurrentPasswordB, out msg))
                {
                    result = this.Block0.SaveData(SectionNo);
                    if (result)
                    {
                        result = this.Block1.SaveData(SectionNo);
                    }
                    if (result)
                    {
                        result = this.Block2.SaveData(SectionNo);
                    }
                    if (result && IsChangingPass)
                    {
                        result = this.Block3.SaveData(SectionNo,fid,cardid,dbcontrolstr);
                    }
                }
            }
            return result;
        }

        //public void ChangedPassword()
        //{
        //    //随机生成dBpassword
        //    Block3.ChangedPassword();
        //}

        //public void SavePassword()
        //{
        //    Block3.SavePassword();
        //}

        public void LoadDBPassword(string data)
        {
            Block3.LoadDBPassword(data);
        }

        #endregion
    }
}
