using System;
using System.Collections.Generic;

namespace LocalMsg
{
    /// <summary>
    /// 消息处理委托函数
    /// </summary>
    /// <param name="msg">消息</param>
    public delegate void MsgHandler(Msg msg);


    /// <summary>
    /// 消息类，通过该对象来传递数据
    /// </summary>
    public class Msg
    {
        /// <summary>
        /// 消息ID
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 处理引擎ID
        /// </summary>
        public EngineID ReceiverEngineID { get; set; }

        /// <summary>
        /// 消息参数列表，采用k-v形式
        /// </summary>
        Dictionary<string, Object> paramList = new Dictionary<string, Object>();


        /// <summary>
        /// 不允许外部创建消息对象
        /// </summary>
        protected Msg() { }


        /// <summary>
        /// 从参数列表中获取值
        /// </summary>
        /// <typeparam name="T">值类型</typeparam>
        /// <param name="key">值对应的关键字</param>
        /// <returns>返回值对象</returns>
        public T GetValue<T>(string key)
        {
            Object value;
            if (paramList.TryGetValue(key, out value) &&
                 value.GetType() == typeof(T))
            {
                return (T)value;
            }
            return default(T);
        }


        /// <summary>
        /// 添加参数, 如果参数列表中已经有该关键字参数，则覆盖
        /// </summary>
        /// <param name="key">参数的含义</param>
        /// <param name="param">参数本身</param>
        public void AddParam(string key, Object param)
        {
            if (key == null || param == null) return;

            Object value = null;
            if (paramList.TryGetValue(key, out value))
            {
                value = param;
            }
            else
            {
                paramList.Add(key, param);
            }
        }

        /// <summary>
        /// 消息池,复用消息对象
        /// </summary>
        public static Queue<Msg> m_pool = new Queue<Msg>();


        /// <summary>
        /// 创建消息
        /// </summary>
        /// <param name="id">消息ID</param>
        /// <param name="k1">参数1含义</param>
        /// <param name="v1">参数1</param>
        /// <param name="k2">参数2含义</param>
        /// <param name="v2">参数2</param>
        /// <param name="k3">参数3含义</param>
        /// <param name="v3">参数3</param>
        /// <param name="k4">参数4含义</param>
        /// <param name="v4">参数4</param>
        /// <param name="k5">参数5含义</param>
        /// <param name="v5">参数5</param>
        public static Msg Create(int id, string k1 = null, Object v1 = null, string k2 = null, Object v2 = null, string k3 = null, Object v3 = null,
            string k4 = null, Object v4 = null, string k5 = null, Object v5 = null, string k6 = null, Object v6 = null)
        {
            lock (m_pool)
            {
                Msg msg = null;
                if (m_pool.Count > 0)
                {
                    msg = m_pool.Dequeue();
                    msg.paramList.Clear();
                }
                else
                {
                    msg = new Msg();
                }
                msg.ID = id;
                msg.AddParam(k1, v1);
                msg.AddParam(k2, v2);
                msg.AddParam(k3, v3);
                msg.AddParam(k4, v4);
                msg.AddParam(k5, v5);
                msg.AddParam(k6, v6);
                msg.ReceiverEngineID = EngineID.Unknow; // 默认设置为广播
                return msg;
            }
        }


        /// <summary>
        /// 克隆消息
        /// </summary>
        /// <returns>新的消息</returns>
        public static Msg Create(Msg msg)
        {
            lock (m_pool)
            {
                Msg newMsg = null;
                if (m_pool.Count > 0)
                {
                    newMsg = m_pool.Dequeue();
                    newMsg.paramList.Clear();
                }
                else
                {
                    newMsg = new Msg();
                }
                newMsg.ID = msg.ID;
                newMsg.ReceiverEngineID = msg.ReceiverEngineID;
                foreach (KeyValuePair<string, Object> item in msg.paramList)
                {
                    newMsg.AddParam(item.Key, item.Value);
                }
                return newMsg;
            }
        }

        /// <summary>
        /// 回收消息
        /// </summary>
        /// <param name="msg">消息</param>
        public static void Destory(Msg msg)
        {
            lock (m_pool)
            {
                m_pool.Enqueue(msg);
            }
        }
    }
}
