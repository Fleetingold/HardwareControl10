using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardwareControl.Reader.Repository.Model
{
    public class ZY2000Block02 : BlockBase
    {
        #region 构造函数
        public ZY2000Block02(object driver) : base(2,driver)
        {

        }

        #endregion

        #region 属性成员
        public string Telephone
        {
            get
            {
                return GetValue<string>("Telephone");
            }
            set
            {
                SetValue<string>("Telephone", value);
                HasChanged = true;
                RaisePropertyChanged("Telephone");
            }
        }

        #endregion

        #region 公共方法

        public override void LoadData(string data)
        {
            SetValue<string>("Telephone", data.Substring(18,14));
        }

        public override string GetData()
        {
            return Telephone.PadLeft(32,'0');
        }


        #endregion

    }
}
