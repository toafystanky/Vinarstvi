namespace Vinarstvi.entities
{
    /// <summary>
    /// Represents information about a database table.
    /// </summary>
    public class TableInfo
    {
        /// <summary>
        /// Gets or sets the name of the database table.
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// Gets or sets the number of records in the database table.
        /// </summary>
        public int RecordCount { get; set; }
    }
}