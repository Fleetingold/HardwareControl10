using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardwareControl.Reader.Interface
{
   public class PasswordModel
    {
        public Guid ID { get; set; }
        public string FId { get; set; }
        public string CardId { get; set; }
        public int SectionId { get; set; }
        public string PasswordType { get; set; }
        public string Password { get; set; }
        public int VersionNo { get; set; }
        public DateTime EffectionDate { get; set; }
    }
}
