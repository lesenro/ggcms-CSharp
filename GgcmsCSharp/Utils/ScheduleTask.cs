using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Timers;
using GgcmsCSharp.ApiCtrls;
using System.Threading.Tasks;
using GgcmsCSharp.Models;
using System.Data.Entity;

namespace GgcmsCSharp.Utils
{
    public class ScheduleTask
    {
        private HttpContext context;
        public ScheduleTask(HttpContext ctx)
        {
            context = ctx;
        }
        Timer taskTimer= new Timer(3000);
        public void Start()
        {
            taskTimer.Elapsed += new ElapsedEventHandler(Run);
            taskTimer.Enabled = true;
            taskTimer.AutoReset = true;
        }

        public void Run(object o, ElapsedEventArgs e)
        {
            var tasks = CacheHelper.GetTasks();
            DateTime now = DateTime.Now;
            foreach(var t in tasks)
            {

                if(t.TaskState!= Models.TaskStatus.Ready)
                {
                    continue;
                }
                if (t.RunTime <= now)
                {
                    t.TaskState = Models.TaskStatus.Executing;
                    UpdateTask(t);
                  
                    switch (t.TaskModel)
                    {
                        case Models.TaskType.StaticTask:
                            Task.Factory.StartNew(() => { this.StaticPages(t); }).ContinueWith(_=> {
                                t.TaskState = Models.TaskStatus.IsOver;
                                t.SetNextRuntime();
                                UpdateTask(t);
                            });                            
                            break;
                    }

                }
            }
        }
        private void UpdateTask(GgcmsTasks tinfo)
        {
            using (GgcmsDB db = new GgcmsDB())
            {
                var ent = db.Entry(tinfo);
                ent.State = EntityState.Modified;
                db.SaveChanges();
                CacheHelper.RemoveAllCache(CacheTypeNames.Tasks);
            }
        }
        private void StaticPages(GgcmsTasks tinfo)
        {
            var info = tinfo.GetTaskConfig<StaticTaskInfo>();
            GenerateStaticPages gsp = new GenerateStaticPages(info,context);
            gsp.Run();
        }
        public void Stop()
        {
            taskTimer.Enabled = false;
            taskTimer.Stop();
        }
    }
}