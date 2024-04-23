using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using CsvHelper;
using MySql.Data.MySqlClient;
using Vinarstvi.exceptions;

namespace Vinarstvi.controller
{
    
    /// <summary>
    /// Manages database operations such as querying, inserting, updating, and deleting records.
    /// </summary>
    public class DatabaseManager
    {
        
        /// <summary>
        /// Gets or sets the connection string used to connect to the database.
        /// </summary>
        public static string ConnectionString { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseManager"/> class with the provided configuration.
        /// </summary>
        /// <param name="config">The configuration settings used to construct the connection string.</param>
        public DatabaseManager(ConfigurationManager config)
        {
            ConnectionString = $"Server={config.DatabaseHost};Port={config.DatabasePort};Database={config.DatabaseName};User Id={config.Username};Password={config.Password};";
        }

        /// <summary>
        /// Creates and returns a new <see cref="MySqlConnection"/> using the current connection string.
        /// </summary>
        /// <returns>A new <see cref="MySqlConnection"/> object.</returns>
        public static MySqlConnection GetConnection()
        {
            return new MySqlConnection(ConnectionString);
        }
        
        /// <summary>
        /// Retrieves a single database entry by its ID from the specified table.
        /// </summary>
        /// <param name="tableName">The name of the table to retrieve the entry from.</param>
        /// <param name="id">The ID of the entry to retrieve.</param>
        /// <returns>A dictionary containing the column names and their corresponding values for the retrieved entry.</returns>
        public IDictionary<string, object> GetEntryById(string tableName, int id)
        {
            try
            {
                using (MySqlConnection connection = GetConnection())
                {
                    connection.Open();
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.Connection = connection;
                        cmd.CommandText = $"SELECT * FROM {tableName} WHERE id = @id";
                        cmd.Parameters.AddWithValue("@id", id);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                var entry = new Dictionary<string, object>();
                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    entry.Add(reader.GetName(i), reader.GetValue(i));
                                }
                                return entry;
                            }
                        }
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                throw;
            }
        }
        
        /// <summary>
        /// Imports data from a CSV file into the specified database table.
        /// </summary>
        /// <typeparam name="T">The type of entity to import.</typeparam>
        /// <param name="filePath">The path to the CSV file containing the data to import.</param>
        /// <param name="tableAttributes">A dictionary mapping table names to their corresponding attributes.</param>
        public void ImportData<T>(string filePath, IDictionary<string, Type> tableAttributes)
        {
            try
            {
                // Read CSV file and map it to the specified entity class
                List<dynamic> records = ReadCsvFile<dynamic>(filePath);
                using (MySqlConnection connection = GetConnection())
                {
                    connection.Open();
                    foreach (var record in records)
                    {
                        using (MySqlCommand cmd = new MySqlCommand())
                        {
                            cmd.Connection = connection;
                            string tableName = GetTableName<T>((IDictionary<string, object>)record, tableAttributes);
                            if (tableName == null)
                            {
                                Console.WriteLine("Unable to determine the table for the record.");
                                throw new TableNotFoundException("Table name not found.");
                            }
                            string columnNames = string.Join(", ", ((IDictionary<string, object>)record).Keys);
                            string parameterNames = string.Join(", ", ((IDictionary<string, object>)record).Keys.Select(key => $"@{key}"));
                            cmd.CommandText = $"INSERT INTO {tableName} ({columnNames}) VALUES ({parameterNames})";
                            AddParameters(cmd, record);
                            try
                            {
                                // Execute the query
                                cmd.ExecuteNonQuery();
                            }
                            catch (MySqlException ex)
                            {
                                if (ex.Number == 1062) // MySQL error code for duplicate entry
                                {
                                    Console.WriteLine($"Duplicate entry: {ex.Message}");
                                    throw new Exception($"Data not saved due to duplicate values in {tableName}.", ex);
                                }
                                else
                                {
                                    throw;
                                }
                            }
                        }
                    }
                }
                Console.WriteLine("Data imported successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during import: {ex.Message}");
                throw;
            }
        }
        /// <summary>
        /// Retrieves all records from the specified table.
        /// </summary>
        /// <param name="tableName">The name of the table from which to retrieve records.</param>
        /// <returns>A <see cref="DataTable"/> containing all records from the specified table.</returns>
        public DataTable GetAllRecords(string tableName)
        {
            try
            {
                // Initialize a new DataTable to store the records
                DataTable dataTable = new DataTable();

                // Construct the SQL query to retrieve all records from the specified table
                string query = $"SELECT * FROM {tableName}";

                // Execute the query and populate the DataTable with the result
                using (MySqlConnection connection = GetConnection())
                {
                    connection.Open();
                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(query, connection))
                    {
                        adapter.Fill(dataTable);
                    }
                }

                // Return the populated DataTable
                return dataTable;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Adds parameters to the specified <see cref="MySqlCommand"/> based on the properties of the dynamic entity.
        /// </summary>
        /// <param name="cmd">The <see cref="MySqlCommand"/> to which parameters will be added.</param>
        /// <param name="entity">The dynamic entity whose properties will be used to add parameters.</param>
        private void AddParameters(MySqlCommand cmd, dynamic entity)
        {
            // Convert the dynamic entity to a dictionary
            var dictionaryEntity = (IDictionary<string, object>)entity;

            // Iterate through each key-value pair in the dictionary and add parameters to the command
            foreach (var kvp in dictionaryEntity)
            {
                // Add a parameter with the key as the parameter name and the value as the parameter value
                // If the value is null, use DBNull.Value
                cmd.Parameters.AddWithValue($"@{kvp.Key}", kvp.Value ?? DBNull.Value);
            }
        }

        /// <summary>
        /// Gets the table name associated with the specified entity type.
        /// </summary>
        /// <param name="entityType">The type of the entity.</param>
        /// <returns>The table name associated with the entity type, or <c>null</c> if the entity type is <c>null</c>.</returns>
        public string GetTableName(Type entityType)
        {
            try
            {
                if (entityType == null)
                {
                    Console.WriteLine("Error: Entity type is null.");
                    return null;
                }
                return entityType.Name.ToLower();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Gets the table name based on the headers of the record and the attributes of the table.
        /// </summary>
        /// <typeparam name="T">The type of the record.</typeparam>
        /// <param name="record">The record containing headers and values.</param>
        /// <param name="tableAttributes">The attributes of the table.</param>
        /// <returns>The table name that matches the headers of the record and the attributes of the table, or <c>null</c> if no match is found.</returns>
        public string GetTableName<T>(IDictionary<string, object> record, IDictionary<string, Type> tableAttributes)
        {
            try
            {
                var headers = record.Keys.ToList();
                Console.WriteLine($"Headers: {string.Join(", ", headers)}");
                foreach (var kvp in tableAttributes)
                {
                    var attributes = kvp.Value.GetProperties().Where(p => !p.Name.Equals("id")).Select(p => p.Name).ToList();
                    Console.WriteLine($"Attributes for {kvp.Key}: {string.Join(", ", attributes)}");
                    if (headers.All(attributes.Contains) && attributes.All(headers.Contains))
                    {
                        return kvp.Key;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                throw;
            }
        }

       /// <summary>
/// Reads data from a CSV file and deserializes it into a list of objects of type T.
/// </summary>
/// <typeparam name="T">The type of objects to deserialize from the CSV file.</typeparam>
/// <param name="filePath">The path to the CSV file.</param>
/// <returns>A list of objects deserialized from the CSV file.</returns>
private List<T> ReadCsvFile<T>(string filePath)
{
    try
    {
        using (var reader = new StreamReader(filePath))
        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            var records = csv.GetRecords<T>().ToList();
            return records;
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"An error occurred: {ex.Message}");
        throw;
    }
}

/// <summary>
/// Retrieves summary data from the specified view.
/// </summary>
/// <param name="viewName">The name of the database view from which to retrieve summary data.</param>
/// <returns>A <see cref="DataTable"/> containing the summary data from the specified view.</returns>
public DataTable GetSummaryData(string viewName)
{
    try
    {
        DataTable dataTable = new DataTable();
        using (MySqlConnection connection = GetConnection())
        {
            connection.Open();
            using (MySqlDataAdapter adapter = new MySqlDataAdapter($"SELECT * FROM {viewName}", connection))
            {
                adapter.Fill(dataTable);
            }
        }
        return dataTable;
    }
    catch (Exception ex)
    {
        Console.WriteLine($"An error occurred: {ex.Message}");
        throw;
    }
}

/// <summary>
/// Retrieves the names of additional tables from the database schema excluding specific tables.
/// </summary>
/// <returns>A <see cref="DataTable"/> containing the names of additional tables.</returns>
public DataTable GetAdditionalTables()
{
    try
    {
        DataTable tableNames = new DataTable();
        using (MySqlConnection connection = GetConnection())
        {
            string query = "SELECT TABLE_NAME FROM information_schema.tables WHERE TABLE_SCHEMA = 'Vinarstvi' AND TABLE_NAME NOT IN ('Vino_Odruda', 'View_Vino_Odruda', 'View_Vino_Zakazka')";

            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                connection.Open();
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    tableNames.Load(reader);
                }
            }
        }
        return tableNames;
    }
    catch (Exception ex)
    {
        Console.WriteLine($"An error occurred: {ex.Message}");
        throw;
    }
}

/// <summary>
/// Retrieves a list of table names from the database.
/// </summary>
/// <returns>A list of table names.</returns>
public List<string> GetTableList()
{
    return DatabaseManagerMethods.GetTableList();
}

        /// <summary>
        /// Deletes a record from the specified entity table by its ID.
        /// </summary>
        /// <param name="entityName">The name of the entity table.</param>
        /// <param name="id">The ID of the record to delete.</param>
        public void DeleteRecordById(string entityName, int id)
        {
            try
            {
                using (MySqlConnection connection = GetConnection())
                {
                    connection.Open();
                    using (MySqlTransaction transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            using (MySqlCommand cmd = new MySqlCommand())
                            {
                                cmd.Connection = connection;
                                cmd.CommandText = $"DELETE FROM {entityName} WHERE id = @id";
                                cmd.Parameters.AddWithValue("@id", id);
                                int rowsAffected = cmd.ExecuteNonQuery();
                                if (rowsAffected == 0)
                                {
                                    throw new RecordNotFoundException($"Record with ID {id} does not exist in {entityName}.");
                                }
                                transaction.Commit();
                            
                                // Display success message after successful deletion
                                MessageBox.Show($"{entityName} record with ID {id} deleted successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            Console.WriteLine($"Error deleting record: {ex.Message}");
                            throw;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Updates a row in the database based on the provided object, table name, and ID.
        /// </summary>
        /// <param name="editedObject">The object containing the updated values.</param>
        /// <param name="tableName">The name of the table to update.</param>
        /// <param name="id">The ID of the row to update.</param>
        public void UpdateRowInDatabase(object editedObject, string tableName, int id)
        {
            try
            {
                using (MySqlConnection connection = GetConnection())
                {
                    connection.Open();
                    using (MySqlTransaction transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            // Debug statement to verify method entry
                            Console.WriteLine($"UpdateRowInDatabase method called for table: {tableName}");

                            using (MySqlCommand cmd = new MySqlCommand())
                            {
                                cmd.Connection = connection;

                                // Debug statement to print the ID of the edited object
                                Console.WriteLine($"Type of edited object: {editedObject.GetType().Name}");
                                Console.WriteLine($"ID of edited object: {id}");

                                // Check if it's a new object or an existing one
                                if (id == 0)
                                {
                                    // Debug statement for new object insertion
                                    Console.WriteLine("Inserting new object...");
                                    // Insert for new objects
                                    string columnNames = string.Join(", ", editedObject.GetType().GetProperties().Where(p => !p.Name.Equals("Id")).Select(p => p.Name));
                                    string parameterNames = string.Join(", ", editedObject.GetType().GetProperties().Where(p => !p.Name.Equals("Id")).Select(p => $"@{p.Name}"));
                                    cmd.CommandText = $"INSERT INTO {tableName} ({columnNames}) VALUES ({parameterNames})";
                                }
                                else
                                {
                                    // Debug statement for existing object update
                                    Console.WriteLine("Updating existing object...");
                                    // Update for existing objects
                                    string updateColumns = string.Join(", ", editedObject.GetType().GetProperties().Where(p => !p.Name.Equals("Id")).Select(p => $"{p.Name} = @{p.Name}"));
                                    cmd.CommandText = $"UPDATE {tableName} SET {updateColumns} WHERE id = @id";
                                    // Add parameter for update
                                    cmd.Parameters.AddWithValue("@id", id);
                                }

                                // Clear any existing parameters
                                cmd.Parameters.Clear();

                                // Add parameters for both insert and update
                                foreach (var property in editedObject.GetType().GetProperties().Where(p => !p.Name.Equals("Id")))
                                {
                                    if (property.PropertyType.IsEnum)
                                    {
                                        var enumValue = property.GetValue(editedObject);
                                        // Ensure case sensitivity and match ENUM values
                                        string stringValue = enumValue.ToString();
                                        // Add parameter for enum value
                                        cmd.Parameters.AddWithValue($"@{property.Name}", stringValue);
                                    }
                                    else
                                    {
                                        cmd.Parameters.AddWithValue($"@{property.Name}", property.GetValue(editedObject) ?? DBNull.Value);
                                    }
                                }

                                // Debug statement for SQL command
                                Console.WriteLine($"SQL command: {cmd.CommandText}");

                                // Execute the query
                                cmd.ExecuteNonQuery();
                                transaction.Commit();
                            }

                            // Debug statement for successful database operation
                            Console.WriteLine("Database operation completed successfully.");
                        }
                        catch (MySqlException ex)
                        {
                            // Debug statement for MySQL exceptions
                            Console.WriteLine($"MySQL Exception: {ex.Number} - {ex.Message}");

                            if (ex.Number == 1062)
                            {
                                // Duplicate entry error (error code 1062)
                                transaction.Rollback();
                                Console.WriteLine("Duplicate entry. Check for duplicate values.");
                                throw new DuplicateEntryException("Duplicate entry. Check for duplicate values.", ex);
                            }
                            else if (ex.Number == 1452)
                            {
                                // Foreign key constraint violation (error code 1452)
                                transaction.Rollback();
                                Console.WriteLine("Cannot add or update a child row. A foreign key constraint fails.");
                                throw new ForeignKeyConstraintException("Cannot add or update a child row. A foreign key constraint fails.", ex);
                            }
                            else
                            {
                                transaction.Rollback();
                                Console.WriteLine("Error updating/inserting row.");
                                throw new Exception("Error updating/inserting row.", ex);
                            }
                        }
                        catch (Exception ex)
                        {
                            // Debug statement for generic exceptions
                            Console.WriteLine($"Exception: {ex.Message}");

                            transaction.Rollback();
                            throw new Exception("Error updating/inserting row.", ex);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                throw;
            }
        }
    }
}
