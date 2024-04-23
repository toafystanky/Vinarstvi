using System;

/// <summary>
/// Represents an exception that is thrown when a table is not found.
/// </summary>
public class TableNotFoundException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TableNotFoundException"/> class with the specified table name.
    /// </summary>
    /// <param name="tableName">The name of the table that was not found.</param>
    public TableNotFoundException(string tableName) : base($"Table not found: {tableName}")
    {
    }
}