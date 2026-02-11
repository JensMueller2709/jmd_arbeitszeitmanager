using JMD_Arbeitszeitmanager.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace JMD_Arbeitszeitmanager.Contracts.Services.DatabaseServices
{
    public interface IDbWorker
    {
        Dictionary<string, Worker> getAllWorkers();
        Dictionary<string, Worker> getActiveWorkers();
        Dictionary<string, Worker> getInactiveWorkers();

        List<Worker> getAllWorkersAsList();

    }
}
