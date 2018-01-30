using System;

namespace HardwareControl.Function
{
    public class MakeBCC
    {
        public bool MakeChekBCC(string str, out string msg)
        {
            bool res = true;
            msg = String.Empty;
            try
            {
                int times = str.Length / 2;
                int sum = 0;
                for (int i = 1; i < times; i++)
                {
                    sum = sum ^ Convert.ToInt32(str.Remove(2), 16);
                    str = str.Substring(2);
                }
                sum = sum ^ Convert.ToInt32(str, 16);
                msg = Convert.ToString(sum, 16).PadLeft(2,'0').ToUpper();
            }
            catch (Exception)
            {
                res = false;
            }

            return res;
        }
    }
}
