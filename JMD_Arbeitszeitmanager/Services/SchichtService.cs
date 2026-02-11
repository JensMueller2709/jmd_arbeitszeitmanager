using JMD_Arbeitszeitmanager.Contracts.Services;
using JMD_Arbeitszeitmanager.Contracts.Services.DatabaseServices;
using JMD_Arbeitszeitmanager.Core.Models;
using JMD_Arbeitszeitmanager.Services.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace JMD_Arbeitszeitmanager.Services
{
    class SchichtService : ISchichtService
    {
        readonly IDbSchicht _dbSchicht;

        public SchichtService(IDbSchicht dbSchicht)
        {
            _dbSchicht = dbSchicht;
        }

        public List<string> getAllCostumers()
        {
           return _dbSchicht.getAllCostumers();
        }

        public Dictionary<string, Schicht> getAllSchichts()
        {
            return _dbSchicht.getAllSchichts();
        }

        public Dictionary<string, Schicht> getAllSchichtsFromCostumer(string costumerId)
        {
            return _dbSchicht.getAllSchichtsFromCostumer(costumerId);
        }

        public Dictionary<string, Schicht> getAllSchichtsFromWorker(string workerId)
        {
            return _dbSchicht.getAllSchichtsFromWorker(workerId);
        }

        //key is the workerId and values are a list of all his schichts
        public Dictionary<string, List<Schicht>> getAllSchichtsFromWorkerOrderByCostumer()
        {
            var allSchichts = getAllSchichts();
            Dictionary<string, List<Schicht>> allWorkersAndTheirSchichts = new Dictionary<string, List<Schicht>>();

            foreach(string schichtId in allSchichts.Keys)
            {
                var s = allSchichts[schichtId];
                if (!allWorkersAndTheirSchichts.ContainsKey(s.WorkerId))
                {
                    allWorkersAndTheirSchichts.Add(s.WorkerId, new List<Schicht> { s });
                } else
                {
                    allWorkersAndTheirSchichts[s.WorkerId].Add(s);
                }
            }

            return allWorkersAndTheirSchichts;

        }

        public List<string> getAllCostumersCollected()
        {
            List<string> allCostumers =  _dbSchicht.getAllCostumers();
            List<string> allCostumersCopy =  new List<string>();

            foreach(string c in allCostumers)
            {
                allCostumersCopy.Add(c);
            }
            
            filterCustomers(allCostumers);

            foreach(string c in allCostumersCopy)
            {
                if (c.ToLower().Contains("zl") && c.ToLower().Contains("tra"))
                {
                    allCostumers.Remove(c);
                }
            }
            
            return allCostumers;
        }

        private void filterCustomers(List<string> customers)
        {
            customers.Remove("HSL-CB");
            customers.Remove("HSL-DE");
            customers.Remove("HSL-NL");
            customers.Remove("HSL-A");
            customers.Remove("ECCO");
            customers.Add("HSL");
        }

        public Dictionary<string, List<Schicht>> getAllSchichtsFromWorkerOrderByCostumerCollected()
        {
            var allSchichts = getAllSchichts();
            Dictionary<string, List<Schicht>> allWorkersAndTheirSchichts = new Dictionary<string, List<Schicht>>();

            foreach (string schichtId in allSchichts.Keys)
            {
                var s = allSchichts[schichtId];
                if (s.Costumer.Contains("ECCO") && s.Start.CompareTo(new DateTime(2023, 03, 01)) < 0)
                {
                    s.Costumer = "ECCO-DE";
                }

                if (!allWorkersAndTheirSchichts.ContainsKey(s.WorkerId))
                {
                    allWorkersAndTheirSchichts.Add(s.WorkerId, new List<Schicht> { s });
                }
                else
                {
                    allWorkersAndTheirSchichts[s.WorkerId].Add(s);
                }
            }

            return allWorkersAndTheirSchichts;
        }

        private void filterSchichts(List<Schicht> schichts) { 
        
        }

    }
}
