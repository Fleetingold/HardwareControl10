using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardwareControl.Reader.Repository.Model
{
    public class ZY2000Block01 : BlockBase
    {
        #region 构造函数
        public ZY2000Block01(object driver) : base(1,driver)
        {

        }

        #endregion

        #region 属性成员
        public string UserName
        {
            get
            {
                return GetValue<string>("UserName");
            }
            set
            {
                SetValue<string>("UserName", value);
                HasChanged = true;
                RaisePropertyChanged("UserName");
            }
        }

        #endregion

        #region 公共方法

        public override void LoadData(string data)
        {
            SetValue<string>("UserName", data);
        }

        public override string GetData()
        {
            return UserName;
        }


        #endregion

    }
}
