using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardwareControl.Reader.Repository.Model
{
    public class ZY2000Block22 : BlockBase
    {
        #region 构造函数
        public ZY2000Block22(object driver) : base(2,driver)
        {

        }

        #endregion

        #region 公共属性
        public string RandomStr
        {
            get
            {
                return GetValue<string>("RandomStr");
            }
            set
            {
                SetValue<string>("RandomStr", value);
                HasChanged = true;
                RaisePropertyChanged("RandomStr");
            }
        }

        public string CRC
        {
            get
            {
                return GetValue<string>("CRC");
            }
            set
            {
                SetValue<string>("CRC", value);
                HasChanged = true;
            }
        }

        #endregion

        #region 公共方法

        public override void LoadData(string data)
        {
            SetValue<string>("RandomStr", data.Substring(0, 28));
            SetValue<string>("CRC", data.Substring(28, 4));
        }

        public override string GetData()
        {
            return string.Format("{0}{1}", RandomStr, CRC);
        }


        #endregion

    }
}
