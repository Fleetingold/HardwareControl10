using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardwareControl.Reader.Repository.Model
{
    public class ZY2000Block20 : BlockBase
    {
        #region 构造函数
        public ZY2000Block20(object driver) : base(0,driver)
        {
        }
        #endregion

        #region 公共属性
        public decimal Bal
        {
            get
            {
                return GetValue<decimal>("Bal");
            }                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                               
            set
            {
                SetValue<decimal>("Bal", value);
                HasChanged = true;
                RaisePropertyChanged("Bal");
            }
        }

        public decimal Point
        {
            get
            {
                return GetValue<decimal>("Point");
            }
            set
            {
                SetValue<decimal>("Point", value);
                HasChanged = true;
                RaisePropertyChanged("Point");
            }
        }

        public int CTC
        {
            get
            {
                return GetValue<int>("CTC");
            }
            set
            {
                SetValue<int>("CTC", value);
                HasChanged = true;
                RaisePropertyChanged("CTC");
            }
        }

        public string Password
        {
            get
            {
                return GetValue<string>("Password");
            }
            set
            {
                SetValue<string>("Password", value);
                HasChanged = true;
                RaisePropertyChanged("Password");
            }
        }

        #endregion

        #region 公共方法

        public override void LoadData(string data)
        {
            SetValue<decimal>("Bal", Convert.ToDecimal(Convert.ToInt32(data.Substring(0, 8),16))/100);
            SetValue<decimal>("Point", Convert.ToDecimal(Convert.ToInt32(data.Substring(8, 8), 16)) / 100);
            SetValue<int>("CTC", Convert.ToInt32(data.Substring(16, 4),16));
            SetValue<string>("Password", data.Substring(20));
        }


        public override string GetData()
        {
            return string.Format("{0}{1}{2}{3}",Convert.ToString(Convert.ToInt32(Bal*100),16).PadLeft(8,'0'), Convert.ToString(Convert.ToInt32(Point*100),16).PadLeft(8,'0'), Convert.ToString(CTC,16).PadLeft(4,'0'), Password.PadLeft(12,'0'));
        }


        #endregion

    }
}
