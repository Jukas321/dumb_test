using MySql.Data.MySqlClient;
using System;
using System.Data;

class Program
{
    static void Main(string[] args)
    {
        // Call your method here
        bool isConnected = DatabaseHelper.CheckDB_Conn();

        // Example usage of the result
        if (isConnected)
        {
            Console.WriteLine("Connection to the database successful.");
            // Fetch and display user data
            DatabaseHelper.DisplayUserData();
        }
        else
        {
            Console.WriteLine("Failed to connect to the database.");
        }
    }
}

public class DatabaseHelper
{
    private static string conn_info = "Server=localhost;Port=3306;Database=User_test;Uid=root;Pwd=19Nado73*";

    public static bool CheckDB_Conn()
    {
        bool isConn = false;
        MySqlConnection conn = null;
        try
        {
            conn = new MySqlConnection(conn_info);
            conn.Open();
            isConn = true;
        }
        catch (ArgumentException a_ex)
        {
            Console.WriteLine("Check the Connection String.");
            Console.WriteLine(a_ex.Message);
        }
        catch (MySqlException ex)
        {
            Console.WriteLine("Error connecting to the database:");
            Console.WriteLine(ex.Message);
            switch (ex.Number)
            {
                // http://dev.mysql.com/doc/refman/5.0/en/error-messages-server.html
                case 1042: // Unable to connect to any of the specified MySQL hosts (Check Server,Port)
                    Console.WriteLine("Unable to connect to MySQL server.");
                    break;
                case 0: // Access denied (Check DB name,username,password)
                    Console.WriteLine("Access denied. Check database name, username, and password.");
                    break;
                default:
                    Console.WriteLine("Unhandled MySQL error occurred.");
                    break;
            }
        }
        finally
        {
            // Ensure connection is closed, regardless of success or failure
            if (conn != null && conn.State == ConnectionState.Open)
            {
                conn.Close();
            }
        }
        return isConn;
    }

    public static void DisplayUserData()
    {
        using (MySqlConnection connection = new MySqlConnection(conn_info))
        {
            string query = "SELECT Id, Name, Last_name FROM User_name";
            MySqlCommand command = new MySqlCommand(query, connection);
            try
            {
                connection.Open();
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine("ID: {0}, Name: {1}, Last Name: {2}", reader["Id"], reader["Name"], reader["Last_name"]);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error retrieving user data: " + ex.Message);
            }
        }
    }
}
