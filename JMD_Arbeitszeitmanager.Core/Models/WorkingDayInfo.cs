using System;
using System.Collections.Generic;
using System.Text;

namespace JMD_Arbeitszeitmanager.Core.Models
{
    public class WorkingDayInfo
    {
        double workingDaysWithoutBreak;

        Schicht firstRelevantSchicht;
        Schicht lastWorkedSchicht;

        DateTime nextPossibleWorkingDayWithoutProblems;

        public WorkingDayInfo(double workingDaysWithoutBreak, Schicht lastWorkedSchicht, DateTime nextPossibleWorkingDayWithoutProblems, Schicht firstRelevantSchicht)
        {
            this.WorkingDaysWithoutBreak = workingDaysWithoutBreak;
            this.LastWorkedSchicht = lastWorkedSchicht;
            this.NextPossibleWorkingDayWithoutProblems = nextPossibleWorkingDayWithoutProblems;
            this.FirstRelevantSchicht = firstRelevantSchicht;
        }

        public double WorkingDaysWithoutBreak { get => workingDaysWithoutBreak; set => workingDaysWithoutBreak = value; }
        public Schicht LastWorkedSchicht { get => lastWorkedSchicht; set => lastWorkedSchicht = value; }
        public DateTime NextPossibleWorkingDayWithoutProblems { get => nextPossibleWorkingDayWithoutProblems; set => nextPossibleWorkingDayWithoutProblems = value; }
        public Schicht FirstRelevantSchicht { get => firstRelevantSchicht; set => firstRelevantSchicht = value; }
    }
}
