using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardwareControl.Reader.Repository.Model
{
    public class CardBase
    {
        #region 私有成员

        private Dictionary<string, Object> properties = new Dictionary<string, Object>();

        #endregion

        #region 构造函数

        public CardBase()
        {

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

        public bool HasChanged
        {
            get
            {
                bool result = false;
                SectionBase sb = null;
                foreach (object obj in this.Properties.Values)
                {
                    if (obj is SectionBase)
                    {
                        sb = obj as SectionBase;
                        if (sb.HasChanged)
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

        public bool HasInit
        {
            get
            {
                return GetValue<bool>("HasInit");
            }
            set
            {
                SetValue<bool>("HasInit", value);
            }
        }

        public bool Haspublished
        {
            get
            {
                return GetValue<bool>("Haspublished");
            }
            set
            {
                SetValue<bool>("Haspublished", value);
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

        public virtual bool LoadData(out string msg)
        {
            msg = "成功";
            return true;
        }


        public virtual bool Publish(Dictionary<string, object> param, bool IsRepublish,out string msg)
        {
            msg = "成功";
            return true;
        }

        public virtual bool Read(string Password, out Dictionary<string, object> param, out string msg)
        {
            msg = "成功";
            param=new Dictionary<string, object>();
            return true;
        }

        public virtual bool WriteDate(string Password, Dictionary<string, object> param, bool IsNew, out string msg)
        {
            msg = "成功";
            return true;
        }

        public virtual bool Charge(string cardNo, decimal beforeAmount, decimal amount, out string msg, int Type = 0)
        {
            msg = "成功";
            return true;
        }

        #endregion
    }
}
