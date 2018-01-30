using HardwareControl.Reader.Interface;
using HardwareControl.Reader.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardwareControl.Reader.Factory
{
   public class ReaderFactory
    {
        public IReaderCommand ReaderCommonds(string stationconfig,object para=null)
        {
            IReaderCommand res = null;
            switch (stationconfig)
            {
               
                case ("HYYY_R6U141_M1S50"):
                    {
                        res = new HYYY_R6U141_M1S50(para as IPassword);
                        break;
                    }
                default:break;
            }
            return res;
        }
    }
}
