using JMD_Arbeitszeitmanager.Contracts.Services;
using JMD_Arbeitszeitmanager.Core.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace JMD_Arbeitszeitmanager.Services
{
    public class WorkingTimeService : IWorkingTimeService
    {
        public const int NEEDED_BREAK_TIME_IN_DAYS = 95;

        private string putCostumersTogether(Schicht schicht)
        {
            string costumer = schicht.Costumer;

            if(costumer.Contains("ECCO") && schicht.Start.CompareTo(new DateTime(2023, 03, 01)) < 0)
            {
                if (costumer.Equals("ECCO-A") && schicht.Start.Year.Equals(2023) && schicht.Start.Month.Equals(03))
                {
                    string test = schicht.Start.ToShortTimeString();
                }
                return "ECCO-DE";
            }
            if (costumer.Contains("HSL"))
            {
                return "HSL";
            }
            if(costumer.ToLower().Contains("zl") &&
                costumer.ToLower().Contains("tra"))
            {
                return "Nordliner";
            }

            return costumer;
        }

        public Dictionary<string, Dictionary<string, WorkingDayInfo>> getWorkingDaysForWorkerAndCostumer(Dictionary<string, List<Schicht>> schichtsPerWorker)
        {
            return getWorkingDaysForWorkerAndCostumer(schichtsPerWorker, DateTime.Now.Date);
        }

        public Dictionary<string, Dictionary<string, WorkingDayInfo>> getWorkingDaysForWorkerAndCostumer(Dictionary<string, List<Schicht>> schichtsPerWorker, DateTime date)
        {
            Dictionary<string, Dictionary<string, WorkingDayInfo>> workingDaysForWorkerAndCostumer = new Dictionary<string, Dictionary<string, WorkingDayInfo>>();

            foreach (string workerId in schichtsPerWorker.Keys)
            {
                workingDaysForWorkerAndCostumer.Add(workerId, new Dictionary<string, WorkingDayInfo>());

                List<Schicht> schichtsOfWorker = schichtsPerWorker[workerId];

                Dictionary<string, List<Schicht>> costumerSchichts = new Dictionary<string, List<Schicht>>();


                //sort schichts of costumer
                schichtsOfWorker.Sort((x, y) => DateTime.Compare(x.Start, y.Start));


                foreach (Schicht s in schichtsOfWorker)
                {
                    //putting schichts of different costumers together
                    var adaptedCostumer = putCostumersTogether(s);

                    if (!costumerSchichts.ContainsKey(adaptedCostumer))
                    {
                        costumerSchichts.Add(adaptedCostumer, new List<Schicht>() { s });
                    }
                    else
                    {
                        costumerSchichts[adaptedCostumer].Add(s);
                    }
                }

                //calc Working Days for each costumer
                foreach (string costumerId in costumerSchichts.Keys)
                {
                    WorkingDayInfo workingDayInfo = calcWorkingDaysWithoutBreakingTimeForCostumer(costumerSchichts[costumerId], date.Date);
                    workingDaysForWorkerAndCostumer[workerId].Add(costumerId, workingDayInfo);
                }

            }


            return workingDaysForWorkerAndCostumer;

        }

        public WorkingDayInfo calcWorkingDaysWithoutBreakingTimeForCostumer(List<Schicht> schichts, DateTime currentDate)
        {
            double workingDays = 0;
            WorkingDayInfo workingDayInfo = null;

            Schicht firstRelevantSchicht = null;
            Schicht curSchicht = null;
            Schicht nextSchicht = null;
            double daysBetween = 0;

            // check every schicht of the list
            for (int idx = 0; idx < schichts.Count; idx++)
            {

                //remember the current schicht and check the next following schicht
                curSchicht = schichts[idx];
                if (idx + 1 < schichts.Count)
                {

                    nextSchicht = schichts[idx + 1];

                    //remember the first relevant schicht which counts for the time calculation
                    if (firstRelevantSchicht == null)
                    {
                        firstRelevantSchicht = curSchicht;
                    }

                    //calculate the time between the current and the next schicht
                    daysBetween = (nextSchicht.End.Date - curSchicht.Start.Date).Days;
                    //daysBetween = nextSchicht.End.Subtract(curSchicht.Start).TotalDays;

                    //if the time between the schichts is greater than 95 then is enough time between the two schichts
                    //of the same costumer. The next working days count new.
                    if (daysBetween > NEEDED_BREAK_TIME_IN_DAYS)
                    {
                        firstRelevantSchicht = nextSchicht;
                        
                    }
                  

                    //if there is only one worked schicht of the costumer
                }
                else if (schichts.Count == 1)
                {
                    nextSchicht = curSchicht;
                    firstRelevantSchicht = curSchicht;
                }
            }

            workingDays = (nextSchicht.End.Date - firstRelevantSchicht.Start.Date).Days;
            

            Schicht lastSchicht = schichts[schichts.Count-1];
            daysBetween = (currentDate - lastSchicht.End.Date).Days;

            if(daysBetween > NEEDED_BREAK_TIME_IN_DAYS)
            {
                workingDays = 0;
            }

            /*
            //if the worker is working for the same costumer more than 14 months then he needs a break of the costumer
            if (workingDays >= 14 * 30)
            {
                workingDayInfo = new WorkingDayInfo(workingDays+1, nextSchicht, DateTime.Now.AddDays(NEEDED_BREAK_TIME_IN_DAYS), firstRelevantSchicht);
            }
            else
            {
                workingDayInfo = new WorkingDayInfo(workingDays, nextSchicht, DateTime.Now, firstRelevantSchicht);
            }
            */
            
            if(workingDays > 0)
            {
                workingDayInfo = new WorkingDayInfo(workingDays, nextSchicht, lastSchicht.End.Date.AddDays(NEEDED_BREAK_TIME_IN_DAYS), firstRelevantSchicht);
            } else
            {
                workingDayInfo = new WorkingDayInfo(workingDays, nextSchicht, currentDate.Date, firstRelevantSchicht);
            }


            return workingDayInfo;
        }

        public Dictionary<string, double> getWorkingTimesOfAllCostumersForWorker(string workerId)
        {
            throw new NotImplementedException();
        }
    }
}
