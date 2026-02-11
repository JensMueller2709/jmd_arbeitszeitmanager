using JMD_Arbeitszeitmanager.Contracts.Services;
using JMD_Arbeitszeitmanager.Contracts.Services.DatabaseServices;
using JMD_Arbeitszeitmanager.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace JMD_Arbeitszeitmanager.Services
{
    public class WorkerService : IWorkerService
    {
        readonly IDbWorker _dbWorker;

        public WorkerService(IDbWorker dbWorker)
        {
            _dbWorker = dbWorker;
        }

        public Dictionary<string, Worker> getAllActiveWorkers()
        {
            throw new NotImplementedException();
        }

        public Dictionary<string, Worker> getAllInActiveWorkers()
        {
            throw new NotImplementedException();
        }

        public Dictionary<string, Worker> getWorkers()
        {
            return _dbWorker.getAllWorkers();
        }
    }
}
