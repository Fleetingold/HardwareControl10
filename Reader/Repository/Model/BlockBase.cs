using HardwareControl.Reader.DllMethod.MWR6;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardwareControl.Reader.Repository.Model
{
    public  class BlockBase: INotifyPropertyChanged
    {
        #region 私有成员

        private Dictionary<string, Object> properties = new Dictionary<string, Object>();

        public event PropertyChangedEventHandler PropertyChanged;

        protected object _driver;

        #endregion

        #region 构造函数
        public BlockBase(int blockno,object driver=null)
        {
            BlockNo = blockno;
            _driver = driver;
            HasChanged = false;
        }
        #endregion

        #region 属性成员

        public Dictionary<string, Object> Properties
        {
            get
            {
                return properties;
            }
            set
            {
                if (value != null)
                {
                    properties = value;
                }
            }
        }

        public int BlockNo
        {
            get
            {
                return GetValue<int>("BlockNo");
            }
            set
            {
                SetValue<int>("BlockNo", value);
            }
        }

        public bool HasChanged
        {
            get
            {
                return GetValue<bool>("HasChanged");
            }
            set
            {
                SetValue<bool>("HasChanged", value);
            }
        }

        #endregion

        #region 索引器
        public Object this[string propertyName]
        {
            get
            {
                // If not exists the key, KeyNotFoundException is throwed.
                return properties[propertyName];
            }
            set
            {
                properties[propertyName] = value;
            }
        }

        #endregion


        #region 公共方法

        /// <summary>
        /// Get specified property.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        /// <exception cref="Exception">If the type does not match well, 
        /// exception is throwed.</exception>
        public T GetProperty<T>(string propertyName)
        {
            return (T)this[propertyName];
        }


        protected void SetValue<T>(string propertyName, object value)
        {
            if (value == null)
            {
                properties[propertyName] = null;
            }
            else
            {
                properties[propertyName] = (T)value;
            }
        }

        protected T GetValue<T>(string propertyName)
        {
            if (!string.IsNullOrEmpty(propertyName) && properties.ContainsKey(propertyName) && properties[propertyName] != null)
            {
                return (T)properties[propertyName];
            }
            else
            {
                //返回T的缺省值
                return default(T);
            }
        }


        public virtual void LoadData(string data)
        {

        }

        public virtual string GetData()
        {
            return string.Empty;
        }

        public virtual bool SaveData(int sectionid, string fid=null, string cardid=null, string dbcontrolstr = null)
        {
            bool result = true;
            if (_driver is ReaderM1S50Method)
            {
                string msg;
                ReaderM1S50Method reader = _driver as ReaderM1S50Method;
                byte[] data = reader.HexToBin(this.GetData());
                result=reader.MifareWrite(sectionid * 4 + BlockNo, data, out msg);
            }
            HasChanged = false; ;
            //return string.Empty;
            return result;
        }


        protected DateTime GetRecordDate(string dstr)
        {
            if (!string.IsNullOrEmpty(dstr) && dstr.Length == 14)
            {

                StringBuilder time = new StringBuilder();
                time.Append(dstr.Substring(0, 4));
                time.Append("-");
                time.Append(dstr.Substring(4, 2));
                time.Append("-");
                time.Append(dstr.Substring(6, 2));
                time.Append(" ");
                time.Append(dstr.Substring(8, 2));
                time.Append(":");
                time.Append(dstr.Substring(10, 2));
                time.Append(":");
                time.Append(dstr.Substring(12, 2));

                DateTime dtime = DateTime.MinValue;
                if (!DateTime.TryParse(time.ToString(), out dtime))
                {
                    dtime = DateTime.Now;
                }

                return dtime;
            }
            else
            {
                return DateTime.Now;
            }
        }

        #endregion


        #region 属性变化事件

        protected virtual void RaisePropertyChanged(string propertyName)
        {
            if (this.PropertyChanged!=null)
            {
                PropertyChangedEventArgs arg = new PropertyChangedEventArgs(propertyName);
                PropertyChanged(this, arg);
            }
        }


        protected void RaisePropertyChanged(params string[] propertyNames)
        {
            if (this.PropertyChanged != null)
            {
                foreach (string propertyName in propertyNames)
                {
                    PropertyChangedEventArgs arg = new PropertyChangedEventArgs(propertyName);
                    PropertyChanged(this, arg);
                }
            }
        }

        #endregion
    }
}
