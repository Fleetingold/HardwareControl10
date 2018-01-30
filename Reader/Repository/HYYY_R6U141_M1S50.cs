using HardwareControl.Reader.DllMethod.MWR6;
using HardwareControl.Reader.Interface;
using HardwareControl.Reader.Repository.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardwareControl.Reader.Repository
{
    public class HYYY_R6U141_M1S50 : IReaderCommand
    {
        #region 数据成员
        ReaderM1S50Method _ReaderMethod;
        ZY2000Card _card;
        #endregion

        #region  构造函数

        public HYYY_R6U141_M1S50(IPassword ipassword)
        {
            _ReaderMethod = new ReaderM1S50Method();
            _card = new ZY2000Card(_ReaderMethod, ipassword);
        }

        #endregion

        #region 接口实现
        public bool Open()
        {
            string msg = string.Empty;
            return _ReaderMethod.DevOpen(out msg);
        }
        public bool Close()
        {
            return _ReaderMethod.DevClose();
        }

        public bool InitCard(string password, string initpassword)
        {
            return true;
        }


        public bool CardCharge(string cardNo, decimal beforeAmount, decimal amount, int Type)
        {
            string msg = string.Empty;

            return _card.Charge(cardNo, beforeAmount, amount, out msg, Type);
        }

        public bool CardPublish(Dictionary<string, object> Params, bool IsRepublish)
        {
            string msg = string.Empty;

            return _card.Publish(Params, IsRepublish, out msg);
        }

        public bool ReadCard(string Password, out Dictionary<string, object> results)
        {
            string msg = string.Empty;
            return _card.Read(Password, out results, out msg);
        }

        public bool WriteDate(string password, Dictionary<string, object> Params, bool IsNew)
        {
            string msg = string.Empty;
            return _card.WriteDate(password, Params,IsNew, out msg);
        }

           

       

        #endregion;
    }
}
