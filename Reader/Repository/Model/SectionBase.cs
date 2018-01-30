using HardwareControl.Reader.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardwareControl.Reader.Repository.Model
{
    public abstract class SectionBase: INotifyPropertyChanged
    {
        #region 私有成员

        private Dictionary<string, Object> properties = new Dictionary<string, Object>();

        public event PropertyChangedEventHandler PropertyChanged;

        protected object _driver;
        protected IPassword _passRes;
        #endregion

        #region 构造函数

        public SectionBase(int sectionno,object driver=null,IPassword passRes=null)
        {
            SectionNo = sectionno;
            _driver = driver;
            _passRes = passRes;
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

        public int SectionNo
        {
            get
            {
                return GetValue<int>("SectionNo");
            }
            set
            {
                SetValue<int>("SectionNo", value);
            }
        }

        public bool HasChanged
        {
            get
            {
                bool result = false;
                BlockBase bb = null;
                foreach(object obj in this.Properties.Values)
                {
                    if (obj is BlockBase)
                    {
                        bb = obj as BlockBase;
                        if (bb.HasChanged)
                        {
                            result = true;
                        }
                    }
                }
                return result;
            }
            //set
            //{
            //    SetValue<bool>("HasChanged", value);
            //}
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

        public virtual bool SaveData(string fid , string cardid ,bool IsChangingPass, string dbcontrolstr = null)
        {
            
            return true;
        }


        public virtual string GetCRC()
        {
            return "0000";
        }

        public virtual bool IsValid()
        {
            return true;
        }

        #endregion


        #region 属性变化事件

        protected virtual void RaisePropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                PropertyChangedEventArgs arg = new PropertyChangedEventArgs(propertyName);
                PropertyChanged(this, arg);
            }
        }

        //protected virtual void RaisePropertyChanged(object sender,string propertyName)
        //{
        //    if (this.PropertyChanged != null)
        //    {
        //        PropertyChangedEventArgs arg = new PropertyChangedEventArgs(propertyName);
        //        PropertyChanged(sender, arg);
        //    }
        //}

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
