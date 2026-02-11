using JMD_Arbeitszeitmanager.Contracts.Services;
using JMD_Arbeitszeitmanager.Contracts.Services.DatabaseServices;
using JMD_Arbeitszeitmanager.Core.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace JMD_Arbeitszeitmanager.Services.Database
{
    public class DBTask : IDbTask
    { 

         private readonly IDatabaseConnector _databaseConnector;

        public DBTask(IDatabaseConnector databaseConnector)
        {
            _databaseConnector = databaseConnector;
        }

        public Dictionary<string, Task> getAllTasks()
        {
            throw new NotImplementedException();
        }

        public Dictionary<string, List<Task>> getTasksToSchicht()
        {
            MySqlConnection connection = _databaseConnector.getDBConnection();

            Dictionary<string, List<Task>> tasksToSchicht = new Dictionary<string, List<Task>>();

            if(connection == null)
            {
                return tasksToSchicht;
            }

            try
            {
                if (connection.State == System.Data.ConnectionState.Closed)
                {
                    try
                    {
                        connection.Open();
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e.StackTrace);
                        connection = _databaseConnector.getDBConnection();
                        connection.Open();
                    }
                    
                }

                var cmd = connection.CreateCommand();
                cmd.CommandText = "SELECT * FROM tasks";

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string id = reader["id"].ToString();
                        string name = reader["kind"].ToString();
                        string schichtId = reader["schichtId"].ToString();
                        DateTime start = DateTime.Parse(reader["startDate"].ToString());
                        DateTime end = DateTime.Parse(reader["endDate"].ToString());
                        string trainNumber = reader["trainnumber"].ToString();
                        string baureiheAdd1 = reader["baureihe_add1"].ToString();
                        string baureiheAdd2 = reader["baureihe_add2"].ToString();
                        
                        Task task = new Task(id, name, schichtId, start, end, trainNumber,baureiheAdd1,baureiheAdd2);

                        if (!tasksToSchicht.ContainsKey(task.SchichtId))
                        {
                            tasksToSchicht.Add(schichtId, new List<Task>() { task });
                        } else
                        {
                            tasksToSchicht[task.SchichtId].Add(task);
                        }
                        
                    }

                }

                return tasksToSchicht;
            }
            catch (MySqlException ex)
            {
                string sqlErrorMessage = "Message: " + ex.Message + "\n" +
                "Source: " + ex.Source + "\n" +
                "Number: " + ex.Number;
                Debug.WriteLine(sqlErrorMessage);

                switch (ex.Number)
                {
                    //http://dev.mysql.com/doc/refman/5.0/en/error-messages-server.html
                    case 1042: // Unable to connect to any of the specified MySQL hosts (Check Server,Port)
                        break;
                    case 0: // Access denied (Check DB name,username,password)
                        break;
                    default:
                        break;
                }
                return tasksToSchicht;
            }
            catch (Exception e)
            {
                Debug.WriteLine(String.Format("Something went wrong, DbTask: {0}", e.StackTrace));
                return tasksToSchicht;
            } finally
            {
                //connection.Close();
            }
        }
    }
}
