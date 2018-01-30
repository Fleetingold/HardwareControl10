using System.Collections.Generic;

namespace HardwareControl.Reader.Interface
{
   public interface IReaderCommand
    {
        bool Open();
        bool Close();
        /// <summary>
        /// 初始化卡
        /// </summary>
        /// <param name="password">原密码</param>
        /// <param name="initpassword">初始化后密码</param>
        /// <returns></returns>
        bool InitCard(string password, string initpassword);
        /// <summary>
        /// 将数据写入卡内
        /// </summary>
        /// <param name="password"></param>
        /// <param name="Params"></param>
        /// <returns></returns>
        bool WriteDate(string password, Dictionary<string, object> Params,bool IsNew);
        /// <summary>
        /// 读取卡信息
        /// </summary>
        /// <param name="Password"></param>
        /// <param name="results"></param>
        /// <returns></returns>
        bool ReadCard(string Password, out Dictionary<string, object> results);
        /// <summary>
        /// 新卡发行
        /// </summary>
        /// <param name="Params"></param>
        /// <returns></returns>
        bool CardPublish(Dictionary<string, object> Params,bool IsRepublish);
       /// <summary>
       /// 卡金额变更
       /// </summary>
       /// <param name="amount">金额</param>
       /// <param name="Type">类型0消费，1充值,2金额初始化</param>
       /// <returns></returns>
        bool CardCharge(string cardNo,decimal beforeAmount,decimal amount,int Type);
        /// <summary>
        /// 卡消费
        /// </summary>
        /// <param name="amount">充值后金额</param>
        /// <returns></returns>
        //bool CardConsume(decimal amount);
    }
}
