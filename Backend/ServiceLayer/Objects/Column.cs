using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.ServiceLayer.Objects
{
    public struct Column
    {
        public int id;
        public string name;
        public int maxTaskLimit;
        public readonly Dictionary<int, Task> dictTask;
        /// constructor 
        internal Column(int id, string name,int maxTaskLimit, Dictionary<int, Task> dictTask)
        {
            this.id = id;
            this.name = name;
            this.maxTaskLimit = maxTaskLimit;
            this.dictTask = dictTask;
        }

        internal Column(BusinessLayer.Column c)
        {
            this.id = c.Id;
            this.name = c.Name;
            this.maxTaskLimit = c.MaxTaskLimit;
            dictTask = new Dictionary<int, Task>();
            foreach (BusinessLayer.Task task in c.DictTask.Values)
            {
                dictTask[task.Id] = new Task(task);
            }
        }
    }
}
