using JMD_Arbeitszeitmanager.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace JMD_Arbeitszeitmanager.Contracts.Services.ModelServices
{
    public interface ITaskService
    {
        Dictionary<string, List<Task>> getTasksToSchichts();
    }
}
