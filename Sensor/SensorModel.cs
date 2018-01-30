using Infrastructure.Services;
using libzkfpcsharp;
using Sample;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using SystemMan.Client.Model;
using System.Linq;
using System.Drawing;

namespace HardwareControl.Sensor
{
    public delegate void GetFingerEventHandler(bool Succ);
    public class SensorModel
    {
        zkfp fpInstance;
        
        byte[] FPBuffer;
        byte[][] RegTmps = new byte[REGISTER_FINGER_COUNT][];
        byte[] RegTmp = new byte[2048];
        byte[] CapTmp = new byte[2048];
        int cbCapTmp = 2048;
        int cbRegTmp = 0;

       public int SensorStates = 0;//设备状态0未调用，1注册,2验证1：N，3验证1:1
       public int RegisterCount = 0;

        const int MESSAGE_CAPTURED_OK = 0x0400 + 6;
        const int REGISTER_FINGER_COUNT = 3;

        List<StationUserInfo> _userList;
        readonly DataServer.Dataserver _server; 


        public SensorModel()
        {
            fpInstance = new zkfp();
            _server = new DataServer.Dataserver();
           // _userList = _server.GetStationUserList(Guid.Parse(LocalData.Context.CurrentDepartmentID));
        }
        #region  私有
      
      
        private EventTransItem OnFingerGet()
        {
            EventTransItem evitem = new EventTransItem();
            evitem.result = 100;
                //传送图像
                MemoryStream ms = new MemoryStream();
            try
            {
                BitmapFormat.GetBitmap(FPBuffer, fpInstance.imageWidth, fpInstance.imageHeight, ref ms);
                //Bitmap bmp = new Bitmap(ms);
                //evitem.Transitem = bmp;
                evitem.Transitem = ms;
            }
            catch(Exception ex) { }
                evitem.EventType = "指纹仪获得图像";
              
                //this._eventAggregator.GetEvent<HardWareControlEvent>().Publish(evitem);

                int ret = zkfp.ZKFP_ERR_OK;
                int fid = 0, score = 0;
                ret = fpInstance.Identify(CapTmp, ref fid, ref score);
                switch (SensorStates)
                {
                    case (1):
                        {
                            if (zkfp.ZKFP_ERR_OK == ret)
                            {
                              evitem.EventType = "这个手指指纹已经被注册为 " + fid + "号!";
                            evitem.result = 10;
                            break;
                            }
                            if (RegisterCount > 0 && fpInstance.Match(CapTmp, RegTmps[RegisterCount - 1]) <= 0)
                            {
                              evitem.EventType = "请不要更换手指！！！";
                            evitem.result = 10;
                                break;
                            }
                            Array.Copy(CapTmp, RegTmps[RegisterCount], cbCapTmp);
                            RegisterCount++;
                            if (RegisterCount >= REGISTER_FINGER_COUNT)
                            {
                                RegisterCount = 0;
                                if (zkfp.ZKFP_ERR_OK == (ret = fpInstance.GenerateRegTemplate(RegTmps[0], RegTmps[1], RegTmps[2], RegTmp, ref cbRegTmp)) 
                                   // &&zkfp.ZKFP_ERR_OK == (ret = fpInstance.AddRegTemplate(iFid, RegTmp))
                                    )
                                {
                                    string temp = string.Empty;
                                    if (zkfp.Blob2Base64String(RegTmp, RegTmp.Length, ref temp) >0)
                                    {
                                        //指纹注册成功，返回模板
                                        evitem.Transitem = temp;
                                        evitem.EventType = "指纹注册完成";
                                        evitem.result = 100;
                                    }
                                    else
                                    {
                                    evitem.EventType = "指纹注册失败，错误代码" + ret;
                                    evitem.result = 100;
                                    }
                                }
                                else
                                {
                                    evitem.EventType = "指纹注册失败，错误代码" + ret;
                                evitem.result = 100;
                                }
                                break;
                            }
                            else
                            {
                            evitem.EventType = "请继续按压相同的指纹" + (REGISTER_FINGER_COUNT - RegisterCount) + "次！";
                            evitem.result = 10;
                            break;
                            }
                        }
                    case (2):
                        {
                            ret = fpInstance.Identify(CapTmp, ref fid, ref score);
                            if (zkfp.ZKFP_ERR_OK == ret)
                            {
                                evitem.Transitem = _userList.SingleOrDefault(a => a.FingerNo == fid);
                            evitem.EventType = "指纹识别成功";
                            evitem.result = 100;
                                break;
                            }
                            else
                            {
                            evitem.EventType = "识别失败，错误代码= " + ret;
                            evitem.result = 100;
                                break;
                            }
                        }
                    case (3):
                        {
                            ret = fpInstance.Match(CapTmp, RegTmp);
                            if (0 < ret)
                            {
                                evitem.Transitem = _userList.SingleOrDefault(a => a.FingerNo == fid);
                            evitem.EventType = "指纹识别成功";
                            evitem.result = 100;
                                break;
                            }
                            else
                            {
                                evitem.EventType = "识别失败，错误代码= " + ret;
                                evitem.result = 100;
                                break;
                            }
                        }
               

            }
         return evitem;

        }

        #endregion

        #region 公开方法
        public bool InitSensor()
        {
            bool result = true;
            int ret = zkfp.ZKFP_ERR_OK;
            if ((ret = fpInstance.Initialize()) == zkfp.ZKFP_ERR_OK)
            {
                int nCount = fpInstance.GetDeviceCount();
                if (nCount <= 0)
                {
                    result = false;
                }
            }
            else
            {
                result = false;
            }
            if (!result)
            {
                fpInstance.Finalize();
            }
            else
            {
                //加载已经注册的指纹
                LoadFinger();
            }
            return result;
        }/// <summary>
        /// 更新指纹仪内部指纹数据
        /// </summary>
        public void LoadFinger()
        {
            _userList = _server.GetStationUserList(Guid.Parse(LocalData.Context.CurrentDepartmentID));
            fpInstance.Clear();
            Thread.Sleep(40);
            if (_userList != null && _userList.Count > 0)
            {
                foreach (StationUserInfo sui in _userList)
                {
                    try
                    {
                        RegTmp = zkfp.Base64String2Blob(sui.FingerTemp);
                        fpInstance.AddRegTemplate(sui.FingerNo, RegTmp);
                        Thread.Sleep(20);
                    }catch(Exception ex)
                    {
                        continue;
                    }
                }
            }
        }
        /// <summary>
        /// 启动指纹仪
        /// </summary>
        /// <returns></returns>
        public bool OpenSensor()
        {
            bool result = true;
            try
            {
                int ret = zkfp.ZKFP_ERR_OK;
                if (zkfp.ZKFP_ERR_OK != (ret = fpInstance.OpenDevice(0)))
                {

                    return false;
                }

                for (int i = 0; i < REGISTER_FINGER_COUNT; i++)
                {
                    RegTmps[i] = new byte[2048];
                }
                FPBuffer = new byte[fpInstance.imageWidth * fpInstance.imageHeight];
            }catch(Exception)
            { result = false; }
            return result;
        }
        public EventTransItem DoCapture()
        {
            EventTransItem result = new EventTransItem();
            result.result = 0;
            if (SensorStates > 0)
            {
                cbCapTmp = 2048;

                int ret = 0;
                ret = fpInstance.AcquireFingerprint(FPBuffer, CapTmp, ref cbCapTmp);
                if (ret == zkfp.ZKFP_ERR_OK)
                {
                    result = OnFingerGet();
                }
            }
            return result;

        }
        #endregion
    }
}
