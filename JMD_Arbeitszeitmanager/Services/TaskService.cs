using JMD_Arbeitszeitmanager.Contracts.Services.DatabaseServices;
using JMD_Arbeitszeitmanager.Contracts.Services.ModelServices;
using JMD_Arbeitszeitmanager.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace JMD_Arbeitszeitmanager.Services
{
    public class TaskService: ITaskService
    {
        private readonly IDbTask _dbTask;

        public TaskService(IDbTask dbTask)
        {
            _dbTask = dbTask;
        }

        public Dictionary<string, List<Task>> getTasksToSchichts()
        {
            var tasks = _dbTask.getTasksToSchicht();

            foreach(string s in tasks.Keys)
            {
                tasks[s].Sort((x, y) => DateTime.Compare(x.Start, y.Start));
            }

            return tasks;
        }
    }
}
