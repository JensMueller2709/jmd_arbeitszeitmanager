using JMD_Arbeitszeitmanager.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace JMD_Arbeitszeitmanager.Contracts.Services
{
    public interface ISchichtService
    {
        Dictionary<string, Schicht> getAllSchichts();

        //some costumers have a different name but get together
        Dictionary<string, Schicht> getAllSchichtsFromWorker(string workerId);
        Dictionary<string, Schicht> getAllSchichtsFromCostumer(string costumerId);
        
        //key is the workerId and values are a list of all his schichts
        Dictionary<string, List<Schicht>> getAllSchichtsFromWorkerOrderByCostumer();

        //some costumers have a different name but get together
        Dictionary<string, List<Schicht>> getAllSchichtsFromWorkerOrderByCostumerCollected();

        List<String> getAllCostumers();

        //some costumers have a different name but get together
        List<String> getAllCostumersCollected();

    }
}
