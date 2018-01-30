using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardwareControl.Reader.Repository.Model
{
    public class SampleRandom
    {
        private static string[] _arr = { "0", "1", "2", "3", "4", "5", "6", "7" ,"8","9","A","B","C","D","E","F"};

        public static string GetRandom(int len)
        {
            StringBuilder sb = new StringBuilder();
            Random ran = new Random(Guid.NewGuid().GetHashCode());
            int n = 0;
            for (int i = 0; i < len; i++)
            {
                n = ran.Next(_arr.Length - 1);
                sb.Append(_arr[n]);
            }
            return sb.ToString();
        }
    }
}
