using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardwareControl.Function
{
    public class MakeSUM
    {
        public bool MakeCheckSum(string str, out string msg)
        {
            bool res = true;
            msg = String.Empty;
            try
            {
                int times = str.Length / 2;
                int sum = 0;
                for (int i = 1; i < times; i++)
                {
                    sum += Convert.ToInt32(str.Remove(2), 16);
                    str = str.Substring(2);
                }
                sum = sum + Convert.ToInt32(str, 16);
                msg = Convert.ToString(sum, 16);
                if (msg.Length > 2)
                {
                    msg = msg.Substring(msg.Length - 2);
                }
                msg = msg.PadLeft(2, '0').ToUpper();
            }
            catch (Exception)
            {
                res = false;
            }

            return res;
        }
    }
}
