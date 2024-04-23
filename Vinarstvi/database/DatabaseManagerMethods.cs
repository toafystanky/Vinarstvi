using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using MySqlConnection = MySql.Data.MySqlClient.MySqlConnection;

namespace Vinarstvi.controller;

/// <summary>
/// Provides additional database management methods.
/// </summary>
public class DatabaseManagerMethods
{
    /// <summary>
    /// Retrieves a list of table names from the database schema.
    /// </summary>
    /// <returns>A list of table names.</returns>
    public static List<string> GetTableList()
    {
        try
        {
            List<string> tableList = new List<string>();

            using (MySqlConnection connection = DatabaseManager.GetConnection())
            {
                connection.Open();
                DataTable schema = connection.GetSchema("Tables");

                foreach (DataRow row in schema.Rows)
                {
                    string tableName = row["TABLE_NAME"].ToString();
                    tableList.Add(tableName);
                }
            }

            return tableList;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
            throw;
        }
    }
    
}