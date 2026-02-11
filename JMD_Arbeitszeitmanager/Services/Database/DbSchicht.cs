using JMD_Arbeitszeitmanager.Contracts.Services;
using JMD_Arbeitszeitmanager.Contracts.Services.DatabaseServices;
using JMD_Arbeitszeitmanager.Core.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;

namespace JMD_Arbeitszeitmanager.Services.Database
{
    public class DbSchicht : IDbSchicht
    {
        private readonly IDatabaseConnector _databaseConnector;

        public DbSchicht(IDatabaseConnector databaseConnector)
        {
            _databaseConnector = databaseConnector;
        }

        public List<string> getAllCostumers()
        {
            MySqlConnection connection = _databaseConnector.getDBConnection();
            HashSet<string> allDistinctCostumers = new HashSet<string>();

            if(connection == null)
            {
                return allDistinctCostumers.ToList();
            }

            try
            {
                if (connection.State == System.Data.ConnectionState.Closed)
                {
                    connection.Open();
                }

                var cmd = connection.CreateCommand();
                cmd.CommandText = "SELECT distinct costumer FROM schichten";

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        allDistinctCostumers.Add(reader["costumer"].ToString().Trim());
                    }

                }

                return allDistinctCostumers.ToList();
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
                return allDistinctCostumers.ToList();
            }
            catch (Exception e)
            {
                Debug.WriteLine(String.Format("Something went wrong, DbSchicht.getAllSchicht: {0}", e.StackTrace));
                return allDistinctCostumers.ToList();
            }
            finally
            {
                //connection.Close();
            }
        }

        public Dictionary<string, Schicht> getAllSchichts()
        {
            string cmd = "SELECT * FROM schichten ORDER BY startDate ASC";
            return executeSQLCommandOnSchichten(cmd);
        }

        public Dictionary<string, Schicht> getAllSchichtsFromCostumer(string costumerId)
        {
            string cmd = "SELECT * FROM schichten where costumer=" + costumerId;
            return executeSQLCommandOnSchichten(cmd);
        }

        public Dictionary<string, Schicht> getAllSchichtsFromWorker(string workerId)
        {
            string cmd = "SELECT * FROM schichten where workerId=" + workerId;
            return executeSQLCommandOnSchichten(cmd);
        }


        private Dictionary<string, Schicht> executeSQLCommandOnSchichten(string sqlCommand)
        {
            MySqlConnection connection = _databaseConnector.getDBConnection();

            Dictionary<string, Schicht> allSchichts = new Dictionary<string, Schicht>();

            if(connection == null)
            {
                return allSchichts;
            }

            try
            {
                
                if (connection.State == System.Data.ConnectionState.Closed)
                {
                    try
                    {
                        connection.Open();
                    } catch (Exception e)
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
                        Schicht schicht = getSchichtFromReader(reader);
                        allSchichts.Add(schicht.Id, schicht);
                    }

                }
                return allSchichts;
            }
            /*
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
                return allSchichts;
            }
            */
            catch (Exception e)
            {
                Debug.WriteLine(String.Format("Something went wrong, DbSchicht.getAllSchicht: {0}", e.StackTrace));
                MessageBox.Show(String.Format("Something went wrong, DbSchicht.getAllSchicht: {0}", e.StackTrace));
                return allSchichts;
            }
            finally
            {
                //connection.Close();
            }
            

        }

        private Schicht getSchichtFromReader(MySqlDataReader reader)
        {
            string id, costumer, workerId, comment, activity;
            DateTime start, end;

            id = reader["id"].ToString();
            costumer = reader["costumer"].ToString().Trim();
            workerId = reader["workerId"].ToString().Trim();
            comment = reader["comment"].ToString();
            activity = reader["elpsActivity"].ToString();
            start = DateTime.Parse(reader["startDate"].ToString());
            end = DateTime.Parse(reader["endDate"].ToString());

            return new Schicht(id, costumer, start, end, workerId, null, activity, comment);
        }
    }
}
