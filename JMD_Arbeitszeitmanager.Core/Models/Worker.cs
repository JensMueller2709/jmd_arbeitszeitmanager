using System;
using System.Collections.Generic;
using System.Text;

namespace JMD_Arbeitszeitmanager.Core.Models
{
    public class Worker
    {

        string id;
        string name;
        string prename;
        bool state;

        public Worker(string id, string name, string prename, bool state)
        {
            this.Id = id;
            this.Name = name;
            this.Prename = prename;
            this.State = state;
        }

        public string Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public string Prename { get => prename; set => prename = value; }
        public bool State { get => state; set => state = value; }
    }
}
