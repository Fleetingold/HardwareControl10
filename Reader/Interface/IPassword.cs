using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardwareControl.Reader.Interface
{
    public interface IPassword
    {
        string GetPasswords(string fid, int sectionid);

        string GetPasswords(string fid, int sectionid,int versionno);
        void SavePassword(List<PasswordModel> list);

        void DeletePasswords(string fid, int sectionid, int versionno);
    }
}
