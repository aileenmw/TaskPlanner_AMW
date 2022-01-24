using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Protocols;
using Nancy.Json;
using Polly;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using TaskPlanner.Models;

namespace TaskPlanner.Helpers
{
    public class DbQueries
    {
        public static List<ShiftDisplay> TodaysShifts()
        {
            List<ShiftDisplay> shifts = new List<ShiftDisplay>();

            string queryString = "SELECT * FROM Shifts " +
                "INNER JOIN Employees on Shifts.Worker = Employees.Id " +
                "WHERE CONVERT(VARCHAR(10), StartTime, 101) = CONVERT(VARCHAR(10), getdate(), 101)";

            using (SqlConnection connection = new SqlConnection(SqlServer.Connection))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ShiftDisplay shift = new ShiftDisplay();
                        shift.Start = reader["StartTime"].ToString();
                        shift.End = reader["EndTime"].ToString();
                        shift.Employee = reader["FirstName"].ToString() + " " + reader["LastName"].ToString();
                        shift.Age = Convert.ToInt32(reader["Age"]);

                        shifts.Add(shift);
                    }
                }
            }

                return shifts;
        }

        public static List<Shift> ShiftData(int id)
        {
            List<Shift> ShiftList = new List<Shift>();

            string queryString = "SELECT * FROM Shifts " +
                "INNER JOIN Employees on Shifts.Worker = Employees.Id " +
                "WHERE Shifts.Id = " + id;

            using (SqlConnection connection = new SqlConnection(SqlServer.Connection))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        Shift shift = new Shift();
                        shift.ShiftTasks = reader["ShiftTasks"].ToString();
                        shift.StartTime = DateTime.Parse(reader["StartTime"].ToString());
                        shift.EndTime = DateTime.Parse(reader["EndTime"].ToString());
                        ShiftList.Add(shift);
                    }
                }
            }

            return ShiftList;
        }

        public static List<WorkTask> taskNames(string tasksIds)
        {
            List<WorkTask> taskList = new List<WorkTask>();

            if (tasksIds != "") {
                string CleanedTasksIds = tasksIds.Trim(' ', ',');
                using (SqlConnection connection = new SqlConnection(SqlServer.Connection))
                {
                    string Query = "SELECT * FROM Tasks WHERE Id in (" + CleanedTasksIds + ")";

                    SqlCommand comm = new SqlCommand(Query, connection);
                    connection.Open();

                    using (SqlDataReader reader = comm.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            WorkTask task = new WorkTask();
                            task.TaskName = reader["TaskName"].ToString();
                            task.MinAge = Convert.ToInt32(reader["MinAge"]);
                            taskList.Add(task);
                        }
                    }
                    connection.Close();
                }
            }

            return taskList;
        }


        public static List<Employee> GrownUps()
        {
            List<Employee> employeelist = new List<Employee>();
            string queryString = "SELECT * FROM Employees WHERE Age >= 18";
            using (SqlConnection connection = new SqlConnection(SqlServer.Connection))
            {
                SqlCommand command = new SqlCommand(
                    queryString, connection);
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Employee employee = new Employee();
                        employee.Id = Convert.ToInt32(reader["Id"]);
                        employee.FirstName = reader["FirstName"].ToString();
                        employee.LastName = reader["LastName"].ToString();
                        employee.Age = Convert.ToInt32(reader["Age"]);

                        employeelist.Add(employee);
                    }
                }
                connection.Close();

                return employeelist;           
            }
        }

        public static List<Employee> GetAllEmployees()
        {
            List<Employee> employeelist = new List<Employee>();
            string queryString = "SELECT * FROM Employees";
            using (SqlConnection con = new SqlConnection(SqlServer.Connection))
            {
                SqlCommand command = new SqlCommand(
                    queryString, con);
                con.Open();
                SqlDataReader rdr = command.ExecuteReader();

                while (rdr.Read())
                {
                    Employee employee = new Employee();
                    employee.Id = Convert.ToInt32(rdr["Id"]);
                    employee.FirstName = rdr["FirstName"].ToString();
                    employee.LastName = rdr["LastName"].ToString();
                    employee.Age = Convert.ToInt32(rdr["Age"]);

                    employeelist.Add(employee);
                }
                con.Close();
            };

            return employeelist;
        }

        public static bool isAChild(int shiftId)
        {
            bool isKid = false;
            int age;
            string queryString = "SELECT Employees.Age FROM Employees INNER JOIN Shifts ON Employees.Id = Shifts.Worker WHERE Shifts.Id = " + shiftId;
            using (SqlConnection con = new SqlConnection(SqlServer.Connection))
            {
                SqlCommand command = new SqlCommand(
                    queryString, con);
                    con.Open();

                SqlDataReader rdr = command.ExecuteReader();

                while (rdr.Read())
                {
                    age = Convert.ToInt32(rdr["Age"]);
                    if (age <= 18)
                    {
                        isKid = true;
                    }
                }
                con.Close();
            }

            return isKid;
        }

        public static string employeeNameById(int Id)
        {
            int id = Id;
            string name = "";
            string queryString =
                           "SELECT FirstName, LastName FROM Employees WHERE Id = " + id;
            using (SqlConnection con = new SqlConnection(SqlServer.Connection))
            {
                SqlCommand command = new SqlCommand( queryString, con);
                con.Open();
                SqlDataReader rdr = command.ExecuteReader();
                
                while (rdr.Read())
                {
                    Employee employee = new Employee();
                    name += rdr["LastName"].ToString() + ", " + rdr["FirstName"].ToString();

                }
                con.Close();
            }
            return name;
        }


        public static List<WorkTask> GetTasks(bool isChild = false)
        {
            List<WorkTask> taskList = new List<WorkTask>();
            string queryString;
            if (isChild != true)
            {
                queryString = "SELECT * FROM Tasks";
            } else
            {
                queryString = "SELECT * FROM Tasks WHERE MinAge < 18 ";
            }
            
            using (SqlConnection con = new SqlConnection(SqlServer.Connection))
            {
                SqlCommand command = new SqlCommand(
                    queryString, con);
                con.Open();
                SqlDataReader rdr = command.ExecuteReader();

                while (rdr.Read())
                {
                    WorkTask task = new WorkTask();
                    task.Id = Convert.ToInt32(rdr["Id"]);
                    task.TaskName = rdr["TaskName"].ToString();
                    task.MinAge = Convert.ToInt32(rdr["MinAge"]);

                    taskList.Add(task);
                }
                con.Close();
            }

              return taskList;
        }


        public static void addTasksToShift(int shiftId, string tasks)
        {
            string queryString = "UPDATE Shifts SET ShiftTasks = '" +  tasks + "' WHERE Id = " + shiftId;
            using (SqlConnection con = new SqlConnection(SqlServer.Connection))
            {
                SqlCommand command = new SqlCommand(queryString, con);
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
