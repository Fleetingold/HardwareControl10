using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardwareControl.Reader.Repository.Model
{
    public class ZY2000Block00 : BlockBase
    {
        #region 构造函数

        public ZY2000Block00(object driver) : base(0,driver)
        {

        }

        #endregion

        #region 属性成员
        public string FID
        {
            get
            {
                return GetValue<string>("FID");
            }
            set
            {
                SetValue<string>("FID", value);
                HasChanged = true;
            }
        }
        #endregion

        #region 公共方法

        public override void LoadData(string data)
        {
            //SetValue<string>("FID", data);
        }

        public override string GetData()
        {
            return FID;
        }

        public override bool SaveData(int sectionid, string fid=null, string cardid=null, string dbcontrolstr = null)
        {
            return true;
        }

        #endregion
    }
}
