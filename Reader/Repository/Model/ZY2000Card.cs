using HardwareControl.Reader.DllMethod.MWR6;
using HardwareControl.Reader.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardwareControl.Reader.Repository.Model
{
    public class ZY2000Card: CardBase
    {
        #region 私有成员

        ReaderM1S50Method _ReaderMethod;
        IPassword _Ipassword;

        private string defaultcontrolstr = "FF078069";
        private string publishcontrolstr = "FF078069";

        #endregion


        #region 构造函数

        public ZY2000Card(ReaderM1S50Method ReaderMethod,IPassword ipassword):base()
        {
            _ReaderMethod = ReaderMethod;
            _Ipassword = ipassword;
            Section0 = new ZY2000Section0(ReaderMethod,ipassword);
            Section1 = new ZY2000Section1(ReaderMethod,ipassword);
            Section2 = new ZY2000Section2(ReaderMethod,ipassword);
        }

        #endregion

        #region 属性成员

        public ZY2000Section0 Section0
        {
            get
            {
                return GetValue<ZY2000Section0>("Section0");
            }
            set
            {
                SetValue<ZY2000Section0>("Section0", value);
            }
        }

        public ZY2000Section1 Section1
        {
            get
            {
                return GetValue<ZY2000Section1>("Section1");
            }
            set
            {
                SetValue<ZY2000Section1>("Section1", value);
            }
        }

        public ZY2000Section2 Section2
        {
            get
            {
                return GetValue<ZY2000Section2>("Section2");
            }
            set
            {
                SetValue<ZY2000Section2>("Section2", value);
            }
        }




        #endregion


        #region 公共方法

        public override bool LoadData(out string msg)
        {
            bool result = true;
            if (_ReaderMethod == null || _Ipassword==null)
            {
                msg = "读卡器驱动或密码存储未初始化";
                return false;
            }
            
            if (_ReaderMethod.DevOpen(out msg))
            {
                if (_ReaderMethod.OpenCard(out msg))
                {
                    this.Section0.Block0.FID = _ReaderMethod.CardId;
                    //初始化扇区存储密码
                   string pass= _Ipassword.GetPasswords(this.Section0.Block0.FID, 0);
                    this.Section0.LoadDBPassword(pass);
                    if (this.Section0.Block3.DBPasswordA== "FFFFFFFFFFFF" && this.Section0.Block3.DBPasswordB == "FFFFFFFFFFFF")
                    {
                        Haspublished = false;
                    }
                    else
                    {
                        Haspublished = true;
                    }
                    pass = _Ipassword.GetPasswords(this.Section0.Block0.FID, 1);
                    this.Section1.LoadDBPassword(pass);
                    pass = _Ipassword.GetPasswords(this.Section0.Block0.FID, 2);
                    this.Section2.LoadDBPassword(pass);
                    //初始化卡片信息
                    //扇区0
                    //string keystr = string.Format("{0}{1}{2}", this.Section0.Block3.DBPasswordA,defaultcontrolstr, this.Section0.Block3.DBPasswordB);
                    //if (Haspublished)
                    //{
                    //    keystr = string.Format("{0}{1}{2}", this.Section0.Block3.DBPasswordA, publishcontrolstr, this.Section0.Block3.DBPasswordB);
                    //}
                    string keystr = this.Section0.Block3.DBPasswordA;
                    //keystr = "027E388D27AA";
                    string data = _ReaderMethod.GetSectionData(0,keystr, this.Section0.Block3.DBPasswordB,out msg);
                    if (!string.IsNullOrEmpty(data))
                    {
                        this.Section0.LoadData(data);
                    }
                    else
                    {
                        return false;

                    }

                    //扇区1
                    keystr = this.Section1.Block3.DBPasswordA;
                    //keystr = "EDCCC1331884";
                    data = _ReaderMethod.GetSectionData(1, keystr, this.Section1.Block3.DBPasswordB, out msg);
                    if (!string.IsNullOrEmpty(data))
                    {
                        this.Section1.LoadData(data);
                        //验证数据CRC
                        if (Haspublished)
                        {
                            if (!this.Section1.IsValid())
                            {
                                msg = "扇区1数据验证失败！";
                                return false;
                            }
                        }
                    }
                    else
                    {
                        return false;

                    }

                    //扇区2
                    keystr = this.Section2.Block3.DBPasswordA;
                    //keystr = "BE35252096AB";
                    data = _ReaderMethod.GetSectionData(2, keystr, this.Section2.Block3.DBPasswordB, out msg);
                    if (!string.IsNullOrEmpty(data))
                    {
                        this.Section2.LoadData(data);
                        //验证数据CRC
                        if (Haspublished)
                        {
                            if (!this.Section2.IsValid())
                            {
                                msg = "扇区2数据验证失败！";
                                return false;
                            }
                        }
                    }
                    else
                    {
                        return false;

                    }
                                       

                    this.HasInit = true;
                   // _ReaderMethod.Halt(out msg);
                }
                else
                {
                    result = false;
                }
            }
            else
            {
                result = false;
            }

            return result;
        }

        public override bool Publish(Dictionary<string, object> param, bool IsRepublish, out string msg)
        {
            return WriteCard(param, !IsRepublish, out msg);
        }

        private bool WriteCard(Dictionary<string, object> param,bool ischangingpass, out string msg)
        {
            bool result = false;
            if (!LoadData(out msg))
            {
                _ReaderMethod.Halt(out msg);
                return result;
            }

            if (!ischangingpass && param.ContainsKey("UserNO"))
            {
                if (param["UserNO"].ToString()!= this.Section1.Block0.CardID.Substring(12, 8).TrimStart(new char[] { '0' }))
                {
                    msg = "卡号不一致";
                    return result;
                }
            }
            //0扇区
            //处理用户名
            if (param.ContainsKey("UserName"))
            {
                this.Section0.Block1.UserName = _ReaderMethod.BinToHex(System.Text.Encoding.GetEncoding("GBK").GetBytes(param["UserName"].ToString())).PadRight(32, '0');
            }
            //处理电话
            if (param.ContainsKey("Telephone"))
            {
                this.Section0.Block2.Telephone = param["Telephone"].ToString().PadLeft(32, '0');
            }
            //1扇区
            //处理卡ID
            if (param.ContainsKey("OrgID") && param.ContainsKey("StationNO") && param.ContainsKey("UserNO"))
            {
                this.Section1.Block0.CardID = string.Format("{0}{1}{2}", param["OrgID"].ToString().PadLeft(6, '0'), param["StationNO"].ToString().PadLeft(6, '0'), param["UserNO"].ToString().PadLeft(8, '0')).PadLeft(32, '0');
            }
            //处理卡类型
            if (param.ContainsKey("CardType"))
            {
                if (param["CardType"].ToString() == "0301")
                {
                    this.Section1.Block1.CardType = "04";
                }
                else if (param["CardType"].ToString() == "0200")
                {
                    this.Section1.Block1.CardType = "01";
                }
            }

            //处理员工号
            //if (param.ContainsKey("UserCode"))
            //{

            //}
            this.Section1.Block1.EmplayeeCode = "00";

            //处理注册时间
            if (param.ContainsKey("RegisterDate"))
            {
                this.Section1.Block1.RegisterDate = (DateTime)param["RegisterDate"];
            }
            //处理有效时间
            if (param.ContainsKey("EffectionDate"))
            {
                this.Section1.Block1.EffectionDate = (DateTime)param["EffectionDate"];
            }
            //处理受限车牌
            if (param.ContainsKey("CarLimit"))
            {
                this.Section1.Block2.CarID = _ReaderMethod.BinToHex(System.Text.Encoding.GetEncoding("GBK").GetBytes(param["CarLimit"].ToString())).PadRight(28, '0');
            }
            //2扇区
            //处理余额
            if (param.ContainsKey("Remain"))
            {
                this.Section2.Block0.Bal = Convert.ToDecimal(Convert.ToInt32(param["Remain"].ToString(), 16)) / 100;
            }

            //处理积分
            if (param.ContainsKey("Point"))
            {
                this.Section2.Block0.Point = Convert.ToDecimal(Convert.ToInt32(param["Point"].ToString(), 16)) / 100;
            }
            //处理CTC
            if (param.ContainsKey("CTC"))
            {
                this.Section2.Block0.CTC = Convert.ToInt32(param["CTC"].ToString());
            }

            //处理密码
            if (param.ContainsKey("UserPassword"))
            {
                if (!string.IsNullOrEmpty(param["UserPassword"].ToString()))
                {
                    this.Section2.Block0.Password = _ReaderMethod.BinToHex(System.Text.Encoding.GetEncoding("ASCII").GetBytes(param["UserPassword"].ToString())).PadLeft(12, '0');
                }
            }

            //处理上次消费金额
            if (param.ContainsKey("LastConsumedAmount"))
            {
                this.Section2.Block1.LastConsumedAmount = Convert.ToDecimal(Convert.ToInt32(param["LastConsumedAmount"].ToString(), 16)) / 100;
            }
            //处理上次消费积分
            if (param.ContainsKey("LastConsumedPoint"))
            {
                this.Section2.Block1.LastConsumedPoint = Convert.ToDecimal(Convert.ToInt32(param["LastConsumedPoint"].ToString(), 16)) / 100;
            }
            //上次消费时间
            if (param.ContainsKey("ListConsumedDate"))
            {
                this.Section2.Block1.ListConsumedDate = (DateTime)param["ListConsumedDate"];
            }
            else
            {
                if (param.ContainsKey("RegisterDate"))
                {
                    this.Section2.Block1.ListConsumedDate = (DateTime)param["RegisterDate"];
                }
            }

            //卡状态
            if (param.ContainsKey("CardState"))
            {
                this.Section2.Block1.CardState = Convert.ToInt32(param["CardState"].ToString());
            }
            //卡版本
            //if (param.ContainsKey("CardVersion"))
            //{
            //    this.Section2.Block1.CardVersion = Convert.ToInt32(param["CardVersion"].ToString());
            //}
            //
            //


            //写卡
            if (IsValid())
            {
                result = this.Section0.SaveData(_ReaderMethod.CardId, this.Section1.Block0.CardID.Substring(this.Section1.Block0.CardID.Length - 8), ischangingpass, publishcontrolstr);
                if (result)
                {
                    result = this.Section1.SaveData(_ReaderMethod.CardId, this.Section1.Block0.CardID.Substring(this.Section1.Block0.CardID.Length - 8), ischangingpass, publishcontrolstr);

                }
                if (result)
                {
                    result = this.Section2.SaveData(_ReaderMethod.CardId, this.Section1.Block0.CardID.Substring(this.Section1.Block0.CardID.Length - 8), ischangingpass, publishcontrolstr);
                }
                if (!result)
                {
                    msg = "写卡失败";
                }
                else
                {
                    msg = "";

                }
            }
            else
            {
                msg = "卡数据验证失败";
                return result;
            }

            _ReaderMethod.Halt(out msg);

            return result;
        }


        public bool IsValid()
        {
            bool result = false;
            if (this.Section0.IsValid() && this.Section1.IsValid() && this.Section2.IsValid())
            {
                result = true;
            }
            return result;
        }

        public override bool Read(string Password, out Dictionary<string, object> param, out string msg)
        {
            bool result = false;
            param = new Dictionary<string, object>();
            if (!LoadData(out msg))
            {
                _ReaderMethod.Halt(out msg);
                return result;
            }
            

            if (string.IsNullOrEmpty(Password))
            {
                Password = System.Text.Encoding.GetEncoding("ASCII").GetString(_ReaderMethod.HexToBin(this.Section2.Block0.Password));
            }
            
            msg = string.Empty;
            
            //results = new Dictionary<string, object>();
           
                param.Add("ErrCounter", 1);


                param.Add("OrgID", this.Section1.Block0.CardID.Substring(0,6));

                string type = this.Section1.Block1.CardType;
                switch (type)
                {
                    case ("04"):
                        {
                        param.Add("CardType", "0301");
                            break;
                        }
                    default:
                        {
                        param.Add("CardType", "0200");
                            break;
                        }
                }
                //results.Add("CardType", str1.Remove(4));
                param.Add("PasswordCounter", this.Section2.Block1.CardState);
                param.Add("UserNO", this.Section1.Block0.CardID.Substring(12, 8).TrimStart(new char[] { '0'}));

                //param.Add("MinLimit", str1.Substring(64, 4));
                param.Add("StationNO", this.Section1.Block0.CardID.Substring(6, 6));
                //param.Add("AreaID", str1.Substring(12, 6));

                param.Add("UserPassword", System.Text.Encoding.GetEncoding("ASCII").GetString(_ReaderMethod.HexToBin(this.Section2.Block0.Password)));
                param.Add("Remain",Convert.ToString(Convert.ToInt32(this.Section2.Block0.Bal*100),16));

            //param.Add("OilType", "");
            //param.Add("CarNo", str1.Substring(72, 16));


            //param.Add("CheckBCC", str1.Substring(94, 2));


            _ReaderMethod.Halt(out msg);
            _ReaderMethod.DevBeep(1, 1, 1);
            result = true;
            return result;
        }


        public override bool WriteDate(string Password, Dictionary<string, object> param, bool IsNew, out string msg)
        {
            //非新发行卡需先读卡验证
            if (!IsNew)
            {
                if (param.ContainsKey("UserNO"))
                {
                    if (param["UserNO"].ToString() != this.Section1.Block0.CardID.Substring(12, 8).TrimStart(new char[] { '0' }))
                    {
                        msg = "卡号不一致";
                        return false;
                    }
                }
            }
           return WriteCard(param, false, out msg);
        }


        public override bool Charge(string cardNo, decimal beforeAmount, decimal amount, out string msg, int Type = 0)
        {
            bool result = true;
            //Dictionary<string, object>  param = new Dictionary<string, object>();
           
            try
            {
                ///读取卡内信息
                if (!LoadData(out msg))
                {
                    _ReaderMethod.Halt(out msg);
                    result = false;
                    msg = "读卡失败";
                    return result;
                }
                if (!cardNo.Equals(this.Section1.Block0.CardID.Substring(14,6)))
                {
                    msg = "卡号不对";
                    return false;
                }
                decimal remain = this.Section2.Block0.Bal;
                //充值前检查
                if (remain != beforeAmount && Type != 2)
                {
                    return false;
                }

                if (Type == 0)
                    {
                        if (remain >= amount)
                        {
                            remain -= amount;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else if (Type == 1)
                    {
                        remain += amount;
                    }
                    else if (Type == 2)
                    {
                        remain = amount;
                    }
                    else
                    {
                        return false;
                    }

                    //修改余额
                    this.Section2.Block0.Bal = remain ;

                result = this.Section2.Block0.SaveData(this.Section2.SectionNo, null, null);
                if (result)
                {
                    result = this.Section2.Block2.SaveData(this.Section2.SectionNo, null, null);
                }

                if (!result)
                {
                    msg = "写卡失败";
                }
                else
                {
                    msg = "";
                }
            }
            catch (Exception ex)
            {
                msg = "发生异常："+ex.Message;
                result = false;
            }

            _ReaderMethod.Halt(out msg);
            return result;
        }

        #endregion
    }
}
