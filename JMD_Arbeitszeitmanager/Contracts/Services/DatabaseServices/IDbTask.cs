using JMD_Arbeitszeitmanager.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;


namespace JMD_Arbeitszeitmanager.Contracts.Services.DatabaseServices
{
    public interface IDbTask
    {

        Dictionary<string, Task> getAllTasks();

        Dictionary<string, List<Task>> getTasksToSchicht();

    }
}
