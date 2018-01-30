using HardwareControl.Sensor;
using Microsoft.Practices.Prism.Events;
using System;

namespace HardwareControl
{
    public class SensorAction
    {
        static private SensorModel _Sensor;
        static private bool Isinit;
        /// <summary>
        /// 初始化并启动指纹识别
        /// </summary>
        /// <param name="eventAggregator"></param>
        /// <returns></returns>
        static private bool SensorON()
        {
           
            try
            {
                _Sensor = new Sensor.SensorModel();
                if(!_Sensor.InitSensor()||!_Sensor.OpenSensor())
                { return false; }
                Isinit = true;
            }catch(Exception)
            {
                Isinit = false;
            }
            return Isinit;
        }
        /// <summary>
        /// 注册指纹
        /// </summary>
       static public void RegFinger()
        {
            if (Isinit)
            {
                _Sensor.SensorStates = 1;
            }
            else
            {
                if(SensorON())
                {
                    _Sensor.SensorStates = 1;
                }
            }
        }
        /// <summary>
        /// 1:n识别
        /// </summary>
        static public void Check1N()
        {
            if (Isinit)
            {
                _Sensor.SensorStates = 2;
            }
            else
            {
                if (SensorON())
                {
                    _Sensor.SensorStates = 2;
                }
            }

        }
        /// <summary>
        /// 1:1识别
        /// </summary>
        static public void Check11()
        {
            if (Isinit)
            {
                _Sensor.SensorStates = 3;
            }
            else
            {
                if (SensorON())
                {
                    _Sensor.SensorStates = 3;
                }
            }

        }

        static public void ResetSensor()
        {
            Isinit = false;
        }
    }
}
