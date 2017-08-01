using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

namespace LocalMsg
{
    /// <summary>
    /// 线程任务类，集成它可以让对象有自己的执行线程</br>
    /// 与unity mono对象集成，使得引擎对象既可以创建单独线程来执行</br>
    /// 也可以让引擎对象依赖unity 中的MonoBehaviour 类的update方法来被调用
    /// </summary>
    public class ThreadTask : MonoBehaviour
    {
        /// <summary>
        /// 事件对象，主要是等待线程结束
        /// </summary>
        ManualResetEvent m_event = new ManualResetEvent(false);

        /// <summary>
        /// 线程名
        /// </summary>
        string m_name = "nil";

        /// <summary>
        /// 工作标识
        /// </summary>
        protected bool m_workFlag = false;
        /// <summary>
        /// 线程对象，用于控制线程工作
        /// </summary>
        Thread m_thread = null;

        /// <summary>
        /// 设置线程名
        /// </summary>
        /// <param name="name"></param>
        public void SetName(string name)
        {
            m_name = name;
        }

        /// <summary>
        /// 创建线程，并启动,若子类覆盖则不会创建线程
        /// </summary>
        public virtual void StartWork()
        {
            if(m_workFlag == false)
            {
                m_workFlag = true;
                m_thread = new Thread(ThreadFun);
                m_thread.Name = m_name;
                m_thread.Start(this);
            }
        }


        public virtual void StopWork()
        {
            if(m_workFlag)
            {
                m_workFlag = false;
                m_event.WaitOne(50);

                // 线程强制终止
                m_thread.Abort();
                m_thread.Join();
            }
        }

        /// <summary>
        /// 在线程中被调用的函数,子类重载该对象就可以使用线程了
        /// </summary>
        public virtual void Work()
        {
        }


        /// <summary>
        /// 线程调用函数
        /// </summary>
        /// <param name="param">线程传入参数，目前没有使用</param>
        void ThreadFun(System.Object param)
        {
            while(m_workFlag)
            {
                Work();
                Thread.Sleep(1);// 停顿一下，防止占用CPU太多
            }
            m_event.Set();
        }

        /// <summary>
        /// 是否在继续工作
        /// </summary>
        /// <returns></returns>
        public bool IsWorking()
        {
            return m_workFlag;
        }

    }
}
