using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardwareControl.Reader.Repository.Model
{
    public class ZY2000Block10 : BlockBase
    {
        #region 构造函数
        public ZY2000Block10(object driver) : base(0,driver)
        {
        }
        #endregion

        #region 公共属性
        public string CardID
        {
            get
            {
                return GetValue<string>("CardID");
            }
            set
            {
                SetValue<string>("CardID", value);
                HasChanged = true;
                RaisePropertyChanged("CardID");
            }
        }

        #endregion

        #region 公共方法

        public override void LoadData(string data)
        {
            SetValue<string>("CardID", data.Substring(12,20));
        }

        public override string GetData()
        {
            return CardID.PadLeft(32,'0');
        }


        #endregion

    }
}
