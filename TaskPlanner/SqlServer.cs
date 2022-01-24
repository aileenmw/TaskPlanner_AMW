using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskPlanner.Helpers
{
    public class SqlServer
    {
        //// Enter Your SQL Server name here:  /////
        private static string YourSqlServer = "";

        private static string serverName = YourSqlServer != "" ? YourSqlServer : System.Environment.MachineName;
        public static string Connection = $"Server={serverName};Database=TaskPlanner;Trusted_Connection=True;MultipleActiveResultSets=True";
        public static string ConnectionWithouDb= $"Server={serverName};Database=;Trusted_Connection=True;MultipleActiveResultSets=True";
        
        public static bool IsServerConnected(string connectionString)
        {
            bool connected = false;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    connected = true;
                }
                catch (SqlException)
                {
                    connected = false;
                }
                finally
                {
                    try
                    {
                        connection.Close();
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
            return connected;
        }

        public void setConnection(string connectionString)
        {
            bool con = false;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    con = true;
                }
                catch (SqlException)
                {
                    con = false;
                }
            }
        }


        public static void createDb()
        {
            string query = "IF NOT EXISTS  " +
                            "(SELECT name FROM master.dbo.sysdatabases WHERE name = 'TaskPlanner') " +
                            "CREATE DATABASE[TaskPlanner]";

            using (SqlConnection con = new SqlConnection(SqlServer.ConnectionWithouDb))
            {
                SqlCommand command = new SqlCommand(query, con);
                con.Open();
                try
                {
                    command.ExecuteNonQuery();
                    Console.WriteLine("Databasen er oprettet");
                }
                catch (Exception)
                {
                    Console.WriteLine("Databasen kunne ikke oprettes");
                }
                finally
                {
                    con.Close();
                }
            }
        }


        public static int tableExists(string tableName)
        {
            int doesExist = 0;

            string query = "IF EXISTS (SELECT object_id FROM sys.tables " +
            "WHERE name = '" + tableName + "'  AND SCHEMA_NAME(schema_id) = 'dbo')  " +
            "PRINT 1 " +
            "ELSE  PRINT 0;";

            using (SqlConnection con = new SqlConnection(SqlServer.Connection))
            {
                SqlCommand command = new SqlCommand(
                query, con);
                con.Open();
                try
                {
                    command.ExecuteNonQuery();
                    doesExist = 1;
                }
                catch (Exception)
                {
                }
                finally
                {
                    con.Close();
                }
            }

                return doesExist;
        }

        public static void createTables()
        {
            string queryEmpl = "IF OBJECT_ID('[dbo].[Employees]') IS NULL " +
                                "BEGIN " +
                                "CREATE TABLE[dbo].[Employees]( " +
                                "[Id] INT IDENTITY(1,1), " +
                                "[FirstName] NCHAR(100)," +
                                "[LastName] NCHAR(100) NOT NULL, " +
                                "[Age] INT NOT NULL, " +
                                "PRIMARY KEY([Id]))" +
                                "END";

            string queryTasks = "IF OBJECT_ID('[dbo].[Tasks]') IS NULL " +
                                "BEGIN " +
                                "CREATE TABLE[dbo].[Tasks] (" +
                                "[Id] INT IDENTITY(1,1), " +
                                "[TaskName] NCHAR(100)," +
                                "[MinAge] INT NOT NULL, " +
                                "PRIMARY KEY([Id])) " +
                                "END";

            string queryShifts = "IF OBJECT_ID('[dbo].[Shifts]') IS NULL " +
                               "BEGIN " +
                               "CREATE TABLE[dbo].[Shifts](" +
                               "[Id] INT IDENTITY(1,1), " +
                               "[StartTime] datetime2(7) NOT NULL," +
                               "[EndTime] datetime2(7) NOT NULL," +
                               "[ShiftTasks] VARCHAR(50)," +
                               "[Worker] INT, " +
                               "PRIMARY KEY([Id]))" +
                                "END";

            string query = queryEmpl + "; " + queryTasks + ";" + queryShifts;

            using (SqlConnection con = new SqlConnection(SqlServer.Connection))
            {
                SqlCommand command = new SqlCommand(
                query, con);
                con.Open();
                try
                {
                    command.ExecuteNonQuery();
                }
                catch (Exception)
                {
                }
                finally
                {
                    con.Close();
                }
            }
        }

    }
}
