using HardwareControl.Reader.Factory;
using HardwareControl.Reader.Interface;
using System;

namespace HardwareControl.Reader
{
    public  class ReaderHelper
    {

        static IReaderCommand ReadrCommand;

         static bool IsuseFull;

        public static IReaderCommand InitReader(string stationconfig,object para=null)
        {
            if(!IsuseFull)
            {
                ReaderFactory rc = new Factory.ReaderFactory();
                ReadrCommand = rc.ReaderCommonds(stationconfig,para);
                if(ReadrCommand!=null)
                {if (ReadrCommand.Open())
                    {
                        IsuseFull = true;
                    }
                }
 
              
            }
            return ReadrCommand;
        }

        public static void ResetReader()
        {
            try
            {
                ReadrCommand.Close();
            }catch(Exception ex)
            { }
            IsuseFull = false;
        }

    }
}
