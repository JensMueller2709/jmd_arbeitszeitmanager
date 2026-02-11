using JMD_Arbeitszeitmanager.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace JMD_Arbeitszeitmanager.Contracts.Services
{
    public interface IWorkerService
    {
        Dictionary<string, Worker> getAllActiveWorkers();
        Dictionary<string, Worker> getAllInActiveWorkers();
        Dictionary<string, Worker> getWorkers();
    }
}
