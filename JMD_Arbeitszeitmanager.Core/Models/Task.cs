using System;
using System.Collections.Generic;
using System.Text;

namespace JMD_Arbeitszeitmanager.Core.Models
{
    public class Task
    {
        string id;
        string name;
        string schichtId;
        DateTime start;
        DateTime end;
        string trainNumber;
        string baureiheAdd1;
        string baureiheAdd2;


        public Task(string id, string name, string schichtId, DateTime start, DateTime end, string trainNumber, string baureiheAdd1, string baureiheAdd2)
        {
            this.id = id;
            this.name = name;
            this.schichtId = schichtId;
            this.start = start;
            this.end = end;
            
            this.TrainNumber = trainNumber;
            this.BaureiheAdd1 = baureiheAdd1;
            this.BaureiheAdd2 = baureiheAdd2;
        }

        public string Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public string SchichtId { get => schichtId; set => schichtId = value; }
        public DateTime Start { get => start; set => start = value; }
        public DateTime End { get => end; set => end = value; }
        public string TrainNumber { get => trainNumber; set => trainNumber = value; }
        public string BaureiheAdd1 { get => baureiheAdd1; set => baureiheAdd1 = value; }
        public string BaureiheAdd2 { get => baureiheAdd2; set => baureiheAdd2 = value; }
    }
}
