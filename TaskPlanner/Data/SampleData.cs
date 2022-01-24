using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using TaskPlanner.Models;

namespace TaskPlanner.Data
{
    public class SampleData
    {
            public static int emplId()
            {
                int id = 1;

                string query = "SELECT TOP 1 Id FROM Employees ORDER BY NEWID()";

                using (SqlConnection con = new SqlConnection(Helpers.SqlServer.Connection))
                {
                    SqlCommand command = new SqlCommand(query, con);
                    con.Open();
                    SqlDataReader rdr = command.ExecuteReader();
                        try
                        {
                            rdr.Read();
                            id = rdr.GetInt32(0);
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("Could't fetch empl ids.");
                        }
                        finally
                        {
                            con.Close();
                        }
                }

                return id;
            }

       public static string taskIds()
        {
            string taskIds = "";
            string query = "SELECT TOP 4 Id FROM Employees ORDER BY NEWID()";
            using (SqlConnection con = new SqlConnection(Helpers.SqlServer.Connection))
            {
                SqlCommand command = new SqlCommand(query, con);
                con.Open();
                SqlDataReader rdr = command.ExecuteReader();
                try
                {
                    while (rdr.Read())
                    {
                        WorkTask task = new WorkTask();
                        taskIds += rdr["Id"].ToString() + ", ";
                    }
                    
                }
                catch (Exception)
                {
                    Console.WriteLine("Could't fetch task ids.");
                }
                finally
                {
                    con.Close();
                }
            }
            
            return taskIds;
        }

        public static void InsertSampleData()
                {
                string Employees = "INSERT INTO Employees(FirstName, LastName, Age) " +
                                    " VALUES" +
                                    "('Lise', 'Petersen', 56)," +
                                    "('Gunnar', 'Petersen', 34)," +
                                    "('Bernd', 'Weber', 45)," +
                                    "('Susanne', 'Gundlach', 35)," +
                                    "('Kirsten', 'Ritschel', 15)," +
                                    "('Lars', 'Ketelsen', 16)," +
                                    "('Mille', 'Søgård', 37)," +
                                    "('Jens', 'Lund', 44)," +
                                    "('Rita', 'Zacho', 37)";              
      
                    string Tasks = "INSERT INTO Tasks(TaskName, MinAge) VALUES" +
                                    "('Trappevask', 15)," +
                                    "('Rengøring af toiletter', 15)," +
                                    "('Afsprintning af kasseområde', 18)," +
                                    "('Opfyldning og oprydning af grønt', 15)," +
                                    "('Udkørsel af varer (bil))', 18)," +
                                    "('Udkørsel af varer (cykel))', 15)," +
                                    "('Tømning af skraldespande', 15)," +
                                    "('Kasse (1)', 18)," +
                                    "('Kasse (2)', 16)";

                    string query = Employees + "; " + Tasks + ";";
            
                    using (SqlConnection con = new SqlConnection(Helpers.SqlServer.Connection))
                        {
                            SqlCommand command1 = new SqlCommand(query, con);
                            con.Open();
                try
                {
                    Mutex mute = new Mutex();
                    mute.WaitOne();
                    command1.ExecuteNonQuery();
                    mute.ReleaseMutex();

                    Console.WriteLine("Employee and Task sampledata created");
                }
                catch
                {
                    Console.WriteLine("Could't generate Employee and Task sampledata. Something went wrong.");
                }
                finally
                {
                    con.Close();
                }

                string shiftQuery = "";
                bool plus = true;
                for (int i = 0; i < 20; i++)
                {
                    if (i <= 10)
                    {
                        shiftQuery = shiftQuery +
                             "INSERT INTO Shifts (StartTime, EndTime, Worker, ShiftTasks) " +
                             "Values (GETDATE() - '01:00:00.000', GETDATE() - '07:00:00.000', " +
                              emplId() + ", '" +
                              taskIds() + "');";
                    }
                    else
                    {
                        shiftQuery = shiftQuery +
                            "INSERT INTO Shifts (StartTime, EndTime, Worker, ShiftTasks) " +
                            "Values (GETDATE() + '8:00:00.000', GETDATE() + '16:00:00.000', " +
                             emplId() + ", '" +
                             taskIds() + "');";
                    }
                }
                try
                {
                    SqlCommand command2 = new SqlCommand(shiftQuery, con);
                    con.Open();
                    Mutex mute = new Mutex();
                    mute.WaitOne();
                    command2.ExecuteNonQuery();
                    mute.ReleaseMutex();

                    //Console.WriteLine("Shift sampledata created");
                }
                catch
                {
                    Console.WriteLine("Could't generate Shift sampledata. Something went wrong.");
                }
                finally
                {
                    con.Close();
                }
            }
        }
    }
}
