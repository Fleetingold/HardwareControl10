using Microsoft.Practices.Prism.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;


namespace HardwareControl.Sensor
{
    public class SensorHelper
    {
        public static bool CheckAuth( string Event,ILoggerFacade Loger, List<string> AuthNeed, out StationUserInfo sui)
        {
            UserLog ul = new UserLog();
            ul.EventType = "权限验证";
            ul.EventDate = DateTime.Now;
            ul.StationID = LocalData.Context.CurrentDepartmentID;
            ul.StationName = LocalData.Context.CurrentDepartmentName;
            ul.UserID = Guid.Empty.ToString();
            ul.UserName = "系统认证";
            ul.EventResult = "成功";
            ul.ID = 0;

            string[] temp = Event.Split('$');
            ul.EventTips = temp[0];
            ul.EventContent = temp[1];

            bool res = true;
            sui = new StationUserInfo();
            SensorView _SensorView = new SensorView();
            EventTransItem senserRes = _SensorView.GetSensorResult(1);
            if ((bool)senserRes.result)
            {
                if (senserRes.EventType == "油站密码")
                {
                   // ul.EventContent = "油站应急密码授权";
                    try
                    {
                        DepartmentModel dm = LocalData.Context.CurrentDepartment as DepartmentModel;
                        DepartmentAttachedProperty pass = dm.DepartmentAttachedProperty.SingleOrDefault(a => a.ProPertyName == "EmergencyPassword");
                        DepartmentAttachedProperty PrintPass = dm.DepartmentAttachedProperty.SingleOrDefault(a => a.ProPertyName == "PrintPassword");
                        DepartmentAttachedProperty TextPrintPass = dm.DepartmentAttachedProperty.SingleOrDefault(a => a.ProPertyName == "TextPrintPassword");


                        if (pass!=null&& pass.PropertyValue.Equals(senserRes.Transitem.ToString()))
                        {
                            sui.UserID = Guid.Empty;
                            sui.UserName = "油站应急密码授权";
                            ul.UserID = dm.ID.ToString();
                            ul.UserName = dm.Name;
                        }

                        else if(PrintPass != null && PrintPass.PropertyValue.Equals(senserRes.Transitem.ToString()))
                        {
                            sui.UserID = Guid.Empty;
                            sui.UserName = "二级密码授权";
                            ul.UserID = dm.ID.ToString();
                            ul.UserName = dm.Name;
                        }
                        else if (TextPrintPass != null && TextPrintPass.PropertyValue.Equals(senserRes.Transitem.ToString()))
                        {
                            sui.UserID = Guid.Empty;
                            sui.UserName = "核心密码授权";
                            ul.UserID = dm.ID.ToString();
                            ul.UserName = dm.Name;
                        }
                        else
                        {
                            ul.EventResult = "失败";
                            res = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        Loger.Log(ex.Message + ex.TargetSite, Category.Exception, Priority.Low);
                        res = false; ul.EventResult = "失败"; }
                }
                else
                {
                    try
                    {
                        // ul.EventContent = "指纹授权";
                        sui = senserRes.Transitem as StationUserInfo;
                        if (null == sui)
                        {
                            res = false;
                            ul.EventResult = "失败";
                            ul.EventContent += "-指纹不存在";
                            if (Loger != null)
                            {
                                Loger.Log(ul.StationID+"$"
                                    +ul.StationName + "$"
                                    + ul.UserID + "$"
                                      + ul.UserName + "$"
                                          + ul.EventType + "$"
                                            + ul.EventTips + "$"
                                            + ul.EventResult + "$"
                                          + ul.EventDate + "$"
                                          + ul.EventContent , Category.Info, Priority.Medium);
                                //Loger.Log(JsonConvert.SerializeObject(ul), Category.Exception, Priority.Medium);
                                /////存储日志
                                //Loger.Log(JsonConvert.SerializeObject(ul), Category.Info, Priority.Medium);

                            }
                            return res;
                        }
                        ul.UserID = sui.UserID.ToString();
                        ul.UserName = sui.UserName;
                        /* ul.EventTips= sui.StationName + "-" + sui.UserName;*/
                        foreach (var item in AuthNeed)
                        {
                            //ul.EventContent += "-" + item;
                            if (!string.IsNullOrEmpty(sui.UserActionCode))
                            {
                                if (!sui.UserActionCode.Contains(item))
                                {
                                    res = false;
                                    ul.EventResult = "失败";
                                    break;
                                }
                            }
                        }
                    }catch(Exception ex)
                    {
                        Loger.Log(ex.Message + ex.StackTrace, Category.Exception, Priority.Low);
                        res = false; ul.EventResult = "失败";
                    }
                }
            }
            else
            {
                res = false;
                return res;
            }
            if (Loger != null)
            {
                Loger.Log(ul.StationID + "$"
       + ul.StationName + "$"
       + ul.UserID + "$"
         + ul.UserName + "$"
             + ul.EventType + "$"
               + ul.EventTips + "$"
               + ul.EventResult + "$"
             + ul.EventDate + "$"
             + ul.EventContent, Category.Info, Priority.Medium);
                ///存储日志
                //Loger.Log(JsonConvert.SerializeObject(ul), Category.Info, Priority.Medium);
            }
            return res;
        }
    }
}
