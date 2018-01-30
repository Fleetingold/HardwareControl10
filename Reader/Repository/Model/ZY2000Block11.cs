using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardwareControl.Reader.Repository.Model
{
    public class ZY2000Block11 : BlockBase
    {
        #region 构造函数
        public ZY2000Block11(object driver) : base(1,driver)
        {
        }

        #endregion

        #region 公共属性
        public string CardType
        {
            get
            {
                return GetValue<string>("CardType");
            }
            set
            {
                SetValue<string>("CardType", value);
                HasChanged = true;
                RaisePropertyChanged("CardType");
            }
        }

        public string EmplayeeCode
        {
            get
            {
                return GetValue<string>("EmplayeeCode");
            }
            set
            {
                SetValue<string>("EmplayeeCode", value);
                HasChanged = true;
                RaisePropertyChanged("EmplayeeCode");
            }
        }

        public DateTime RegisterDate
        {
            get
            {
                return GetValue<DateTime>("RegisterDate");
            }
            set
            {
                SetValue<DateTime>("RegisterDate", value);
                HasChanged = true;
                RaisePropertyChanged("RegisterDate");
            }
        }

        public DateTime EffectionDate
        {
            get
            {
                return GetValue<DateTime>("EffectionDate");
            }
            set
            {
                SetValue<DateTime>("EffectionDate", value);
                HasChanged = true;
                RaisePropertyChanged("EffectionDate");
            }
        }

        #endregion



        #region 公共方法

        public override void LoadData(string data)
        {
            SetValue<string>("CardType", data.Substring(0,2));
            SetValue<string>("EmplayeeCode", data.Substring(2,2));
            SetValue<DateTime>("RegisterDate", GetRecordDate(data.Substring(4,14)));
            SetValue<DateTime>("EffectionDate", GetRecordDate(data.Substring(18,14)));
        }

       
        public override string GetData()
        {
            return string.Format("{0}{1}{2}{3}", CardType, EmplayeeCode, RegisterDate.ToString("yyyyMMddhhmmss"), EffectionDate.ToString("yyyyMMddhhmmss"));
        }


        #endregion

    }
}
