using JMD_Arbeitszeitmanager.Core.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace JMD_Arbeitszeitmanager.Contracts.Services.DatabaseServices
{
    class DbWorker : IDbWorker
    {
        private readonly IDatabaseConnector _databaseConnector;

        public DbWorker(IDatabaseConnector databaseConnector)
        {
            _databaseConnector = databaseConnector;
        }

        public Dictionary<string, Worker> getActiveWorkers()
        {
            string cmd = "SELECT * FROM mitarbeiter where aktiv=TRUE";
            return executeSQLCommandOnWorker(cmd);
        }

        public Dictionary<string, Worker> getAllWorkers()
        {
            string cmd = "SELECT * FROM mitarbeiter";
            return executeSQLCommandOnWorker(cmd);

        }

        public List<Worker> getAllWorkersAsList()
        {
            string cmd = "SELECT * FROM mitarbeiter";
            var dicWorker = executeSQLCommandOnWorker(cmd);
            List<Worker> workerList = new List<Worker>();
            foreach (string workerId in dicWorker.Keys)
            {
                workerList.Add(dicWorker[workerId]);
            }
            return workerList;
        }

        public Dictionary<string, Worker> getInactiveWorkers()
        {
            string cmd = "SELECT * FROM mitarbeiter where aktiv=FALSE";
            return executeSQLCommandOnWorker(cmd);
        }

        private Dictionary<string, Worker> executeSQLCommandOnWorker(string sqlCommand)
        {
            Dictionary<string, Worker> allWorkers = new Dictionary<string, Worker>();
            MySqlConnection connection = _databaseConnector.getDBConnection();
            if(connection == null)
            {
                return allWorkers;
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
                cmd.CommandText = sqlCommand;

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Worker worker = getWorkerOfReader(reader);
                        allWorkers.Add(worker.Id, worker);
                    }
                }
                return allWorkers;
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
                return allWorkers;
            }
            catch (Exception e)
            {
                Debug.WriteLine(String.Format("Something went wrong, DbWorker.getActiveWorkers: {0}", e.StackTrace));
                return allWorkers;
            }
            finally
            {
                //connection.Close();
            }
        }

        private Worker getWorkerOfReader(MySqlDataReader reader)
        {
            string id, name, prename;
            bool status;
            id = reader["anzeigename"].ToString().Trim();
            name = reader["nachname"].ToString();
            prename = reader["vorname"].ToString();
            status = bool.Parse(reader["aktiv"].ToString());
            return new Worker(id, name, prename, status);
        }

    }
}
