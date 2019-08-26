using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using System.Threading;

namespace DNNGo.Modules.PowerForms
{
    public class MailScheduler
    {    
        /// <summary>
        /// 任务执行进程的实现
        /// </summary>
        private sealed class Worker
        {
            private Thread worker;
            private Queue<EmailInfo> queueMessage;
            private bool working = false;

            /// <summary>
            /// 任务数量
            /// </summary>
            public int Count
            {
                get { return queueMessage.Count; }
            }

            /// <summary>
            /// 任务执行进程的构造函数
            /// </summary>
            public Worker()
            {

            }


            /// <summary>
            /// 启动任务进程
            /// </summary>
            public void Start()
            {
                if (!this.working)
                {
                    queueMessage = new Queue<EmailInfo>();
                    this.worker = new Thread(new ThreadStart(WorkerHandler));
                    this.worker.IsBackground = true;
                    this.worker.Priority = ThreadPriority.BelowNormal;
                    this.worker.Start();
                }
            }

            /// <summary>
            /// 停止任务进程
            /// </summary>
            public void Stop()
            {
                if (this.working)
                {
                    this.working = false;
                    this.worker.Join();

                }
            }

            /// <summary>
            /// 指派一个新的邮件任务
            /// </summary>
            /// <param name="task"></param>
            /// <returns></returns>
            public int AssignMessage(EmailInfo message)
            {
                this.Start();
                this.queueMessage.Enqueue(message);
                return this.Count;
            }

            ~Worker()
            {
                this.Stop();
            }

            /// <summary>
            /// 任务循环处理函数
            /// </summary>
            private void WorkerHandler()
            {

                this.working = true;
                while (this.working)
                {
                    while (this.queueMessage.Count > 0)
                    {
                        try
                        {
                            EmailInfo mail = this.queueMessage.Dequeue();
                            NetHelper.SendMail(mail);//发送邮件程序

                        }
                        catch (Exception ex)
                        {
                            
                        }
                        finally
                        {
                            Thread.Sleep(50);
                        }
                    }
                    Thread.Sleep(1000);
                }
            }
        }

        private static Worker worker = new Worker();
        /// <summary>
        /// 向任务处理器指派新的邮件发送任务
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static int AssignMessage(EmailInfo message)
        {
            return worker.AssignMessage(message);
        }


        /// <summary>
        /// 启动任务处理器
        /// </summary>
        public static void Start()
        {
            worker.Start();
        }

        /// <summary>
        /// 停止任务处理器
        /// </summary>
        public static void Stop()
        {
            worker.Stop();
        }
        /// <summary>
        /// 邮件任务数量
        /// </summary>
        public static int Count()
        {
            return worker.Count;

        }
    }
}
