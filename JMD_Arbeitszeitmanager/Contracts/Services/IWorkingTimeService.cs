using JMD_Arbeitszeitmanager.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace JMD_Arbeitszeitmanager.Contracts.Services
{
    public interface IWorkingTimeService
    {
        //get working days in a row for each worker at every costumer
        //output: key is workerId, value is a Dictionary with costumerID as key and workingdays as value  
        Dictionary<string, Dictionary<string, WorkingDayInfo>> getWorkingDaysForWorkerAndCostumer(Dictionary<string, List<Schicht>> schichtsPerWorker);

        //key is costumer id, value is time in h
        Dictionary<string, double> getWorkingTimesOfAllCostumersForWorker(string workerId);
    }
}
