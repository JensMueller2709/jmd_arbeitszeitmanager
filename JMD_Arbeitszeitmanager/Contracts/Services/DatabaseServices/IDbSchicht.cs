using JMD_Arbeitszeitmanager.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace JMD_Arbeitszeitmanager.Contracts.Services.DatabaseServices
{
    public interface IDbSchicht
    {
        Dictionary<string, Schicht> getAllSchichts();
        Dictionary<string, Schicht> getAllSchichtsFromWorker(string workerId);
        Dictionary<string, Schicht> getAllSchichtsFromCostumer(string costumerId);

        List<string> getAllCostumers();

    }
}
