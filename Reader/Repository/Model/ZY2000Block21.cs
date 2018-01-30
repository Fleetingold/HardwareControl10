using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardwareControl.Reader.Repository.Model
{
    public class ZY2000Block21 : BlockBase
    {
        #region 构造函数
        public ZY2000Block21(object driver) : base(1,driver)
        {
        }

        #endregion

        #region 公共属性
        public decimal LastConsumedAmount
        {
            get
            {
                return GetValue<decimal>("LastConsumedAmount");
            }
            set
            {
                SetValue<decimal>("LastConsumedAmount", value);
                HasChanged = true;
                RaisePropertyChanged("LastConsumedAmount");
            }
        }

        public decimal LastConsumedPoint
        {
            get
            {
                return GetValue<decimal>("LastConsumedPoint");
            }
            set
            {
                SetValue<decimal>("LastConsumedPoint", value);
                HasChanged = true;
                RaisePropertyChanged("LastConsumedPoint");
            }
        }

        public DateTime ListConsumedDate
        {
            get
            {
                return GetValue<DateTime>("ListConsumedDate");
            }
            set
            {
                SetValue<DateTime>("ListConsumedDate", value);
                HasChanged = true;
                RaisePropertyChanged("ListConsumedDate");
            }
        }

        public int CardState
        {
            get
            {
                return GetValue<int>("CardState");
            }
            set
            {
                SetValue<int>("CardState", value);
                HasChanged = true;
                RaisePropertyChanged("CardState");
            }
        }

        public int CardVersion
        {
            get
            {
                return GetValue<int>("CardVersion");
            }
            set
            {
                SetValue<int>("CardVersion", value);
                HasChanged = true;
                RaisePropertyChanged("CardVersion");
            }
        }

        #endregion


        #region 公共方法

        public override void LoadData(string data)
        {
            SetValue<decimal>("LastConsumedAmount", Convert.ToDecimal(Convert.ToInt32(data.Substring(0, 6), 16)) / 100);
            SetValue<decimal>("LastConsumedPoint", Convert.ToDecimal(Convert.ToInt32(data.Substring(6, 6), 16)) / 100);
            SetValue<DateTime>("ListConsumedDate", GetRecordDate(data.Substring(12, 14)));
            SetValue<int>("CardState", Convert.ToInt32(data.Substring(26, 2),16));
            SetValue<int>("CardVersion", Convert.ToInt32(data.Substring(28, 2),16));
        }


        public override string GetData()
        {
            return string.Format("{0}{1}{2}{3}{4}00", Convert.ToString(Convert.ToInt32(LastConsumedAmount*100), 16).PadLeft(6, '0'), Convert.ToString(Convert.ToInt32(LastConsumedPoint*100), 16).PadLeft(6, '0'), ListConsumedDate.ToString("yyyyMMddhhmmss"), Convert.ToString(CardState, 16).PadLeft(2, '0'), Convert.ToString(CardVersion, 16).PadLeft(2, '0'));
        }


        #endregion
    }
}
