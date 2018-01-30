using HardwareControl.Reader.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardwareControl.Reader.Repository.Model
{
    public class ZY2000Block3 : BlockBase
    {

        #region 包含成员
        protected IPassword _ipassword;
        #endregion

        #region 构造方法
        public ZY2000Block3(object driver,IPassword ipassword) : base(3,driver)
        {
            _ipassword = ipassword;
        }

        #endregion


        #region 属性成员
        public string PasswordA
        {
            get
            {
                return GetValue<string>("PasswordA");
            }
            set
            {
                SetValue<string>("PasswordA", value);
            }
        }

        public string PasswordB
        {
            get
            {
                return GetValue<string>("PasswordB");
            }
            set
            {
                SetValue<string>("PasswordB", value);
            }
        }

        public string ControlStr
        {
            get
            {
                return GetValue<string>("ControlStr");
            }
            set
            {
                SetValue<string>("ControlStr", value);
            }
        }

        public string CurrentPasswordA
        {
            get
            {
                return GetValue<string>("CurrentPasswordA");
            }
            set
            {
                SetValue<string>("CurrentPasswordA", value);
            }
        }

        public string CurrentPasswordB
        {
            get
            {
                return GetValue<string>("CurrentPasswordB");
            }
            set
            {
                SetValue<string>("CurrentPasswordB", value);
            }
        }

        public string CurrentControlStr
        {
            get
            {
                return GetValue<string>("CurrentControlStr");
            }
            set
            {
                SetValue<string>("CurrentControlStr", value);
            }
        }

        public string DBPasswordA
        {
            get
            {
                return GetValue<string>("DBPasswordA");
            }
            set
            {
                SetValue<string>("DBPasswordA", value);
            }
        }

        public string DBPasswordB
        {
            get
            {
                return GetValue<string>("DBPasswordB");
            }
            set
            {
                SetValue<string>("DBPasswordB", value);
            }
        }

        public string DBControlStr
        {
            get
            {
                return GetValue<string>("DBControlStr");
            }
            set
            {
                SetValue<string>("DBControlStr", value);
            }
        }

        public int PasswordVersion
        {
            get
            {
                return GetValue<int>("PasswordVersion");
            }
            set
            {
                SetValue<int>("PasswordVersion", value);
            }
        }

        #endregion


        #region 公共方法

        public override void LoadData(string data)
        {
            //SetValue<string>("PasswordA", data.Substring(0, 12));
            SetValue<string>("ControlStr", data.Substring(12, 8));
            //SetValue<string>("PasswordB", data.Substring(20, 12));

            //SetValue<string>("CurrentPasswordA", data.Substring(0, 12));
            SetValue<string>("CurrentControlStr", data.Substring(12, 8));
            //SetValue<string>("CurrentPasswordB", data.Substring(20, 12));
            SetValue<string>("DBControlStr", data.Substring(12, 8));
        }

        public override string GetData()
        {
            return string.Format("{0}{1}{2}",this.PasswordA,this.ControlStr,this.PasswordB);
        }

        public void ChangedPassword(string dbcontrolstr=null)
        {
            //随机生成dBpassword
            DBPasswordA = SampleRandom.GetRandom(12);
            DBPasswordB = SampleRandom.GetRandom(12);
            if (!string.IsNullOrEmpty(dbcontrolstr))
            {
                DBControlStr = dbcontrolstr;
                ControlStr= dbcontrolstr;
            }
            PasswordVersion = (PasswordVersion + 1) % 256;
        }

        protected bool SavePassword(int sectionid,string fid,string cardid)
        {
            bool result = true;
            PasswordA = DBPasswordA;
            PasswordB = DBPasswordB;
            try
            {
                List<PasswordModel> list = new List<PasswordModel>();
                PasswordModel model = new PasswordModel() {
                    ID = Guid.NewGuid(),
                    CardId = cardid,
                    EffectionDate = DateTime.Now,
                    FId = fid,
                    Password = PasswordA,
                    PasswordType="A",
                    SectionId= sectionid,
                    VersionNo=PasswordVersion
                };
                list.Add(model);
                model = new PasswordModel()
                {
                    ID = Guid.NewGuid(),
                    CardId = cardid,
                    EffectionDate = DateTime.Now,
                    FId = fid,
                    Password =  PasswordB,
                    PasswordType = "B",
                    SectionId = sectionid,
                    VersionNo = PasswordVersion
                };
                list.Add(model);

                _ipassword.SavePassword(list);
            }
            catch(Exception ex)
            {
                result = false;
            }
           

            return result;
        }


        public override bool SaveData(int sectionid, string fid, string cardid, string dbcontrolstr = null)
        {
            bool result = false;
            ChangedPassword(dbcontrolstr);            
            result=SavePassword(sectionid,fid,cardid);
            if (result)
            {
                result = base.SaveData(sectionid,fid,cardid, dbcontrolstr);
            }
            UpdateCurrentPassword(result,sectionid,fid);
            return result;
        }

        public void LoadDBPassword(string data)
        {
            SetValue<string>("DBPasswordA", data.Substring(0, 12));
            SetValue<string>("DBPasswordB", data.Substring(12, 12));
            SetValue<string>("CurrentPasswordA", data.Substring(0, 12));
            SetValue<string>("CurrentPasswordB", data.Substring(12, 12));
            SetValue<string>("PasswordA", data.Substring(0, 12));
            SetValue<string>("PasswordB", data.Substring(12, 12));
            SetValue<int>("PasswordVersion", int.Parse(data.Substring(24)));
        }

        public void UpdateCurrentPassword(bool result, int sectionid,string fid)
        {
            if (result)
            {
                this.CurrentPasswordA = this.PasswordA;
                this.CurrentPasswordB = this.PasswordB;
                this.CurrentControlStr = this.ControlStr;
            }
            else
            {
                //删除新版本
                try
                {
                    

                    _ipassword.DeletePasswords(fid, sectionid, PasswordVersion);
                }
                catch 
                {
                    //result = false;
                }


                this.PasswordA = this.CurrentPasswordA;
                this.PasswordB = this.CurrentPasswordB;
                this.ControlStr = this.CurrentControlStr;
                DBControlStr = this.ControlStr;
                DBPasswordA = PasswordA;
                DBPasswordB = PasswordB;
                PasswordVersion = (PasswordVersion - 1) % 256;
                

            }
        }

        #endregion

    }
}
