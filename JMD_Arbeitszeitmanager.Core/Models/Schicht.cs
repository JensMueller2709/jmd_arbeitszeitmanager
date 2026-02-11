using System;
using System.Collections.Generic;
using System.Text;

namespace JMD_Arbeitszeitmanager.Core.Models
{
    public class Schicht
    {
        string id;
        string costumer;
        DateTime start;
        DateTime end;
        string workerId;
        List<Task> tasks;
        string activity;
        string comment;

        public Schicht(string id, string costumer, DateTime start, DateTime end, string workerId, List<Task> tasks, string activity, string comment)
        {
            this.Id = id;
            this.Costumer = costumer;
            this.Start = start;
            this.End = end;
            this.WorkerId = workerId;
            this.Tasks = tasks;
            this.Activity = activity;
            this.Comment = comment;
        }

        public string Id { get => id; set => id = value; }
        public string Costumer { get => costumer; set => costumer = value; }
        public DateTime Start { get => start; set => start = value; }
        public DateTime End { get => end; set => end = value; }
        public string WorkerId { get => workerId; set => workerId = value; }
        public List<Task> Tasks { get => tasks; set => tasks = value; }
        public string Activity { get => activity; set => activity = value; }
        public string Comment { get => comment; set => comment = value; }
    }
}
