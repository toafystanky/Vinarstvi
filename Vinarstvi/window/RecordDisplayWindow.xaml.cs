using System;
using System.ComponentModel;
using System.Data;
using System.Windows;
using System.Windows.Data;
using Vinarstvi.entities;

namespace Vinarstvi.window
{
    
    /// <summary>
    /// Represents a window for displaying records from a DataTable.
    /// </summary>
    public partial class RecordDisplayWindow : Window
    {
        private readonly DataTable _tableData;

        /// <summary>
        /// Initializes a new instance of the <see cref="RecordDisplayWindow"/> class.
        /// </summary>
        /// <param name="tableData">The DataTable containing the records to display.</param>
        /// <param name="tableName">The name of the table associated with the records.</param>
        public RecordDisplayWindow(DataTable tableData, string tableName)
        {
            try
            {
                InitializeComponent();

                // Debug statement to print table name and record count
                Console.WriteLine($"Table Name: {tableName}, Record Count: {tableData.Rows.Count}");

                // Set DataContext to TableInfo instance
                DataContext = new TableInfo { TableName = tableName, RecordCount = tableData.Rows.Count };

                // Bind the DataTable to a data grid
                dataGrid.ItemsSource = tableData.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while initializing the Record Display Window: {ex.Message}",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Represents the view model for the RecordDisplayWindow.
        /// </summary>
        public class RecordDisplayViewModel : INotifyPropertyChanged
        {
            private string _tableName;

            /// <summary>
            /// Gets or sets the name of the table associated with the records.
            /// </summary>
            public string TableName
            {
                get { return _tableName; }
                set
                {
                    _tableName = value;
                    OnPropertyChanged("Table Name");
                }
            }

            private int _recordCount;

            /// <summary>
            /// Gets or sets the number of records in the table.
            /// </summary>
            public int RecordCount
            {
                get { return _recordCount; }
                set
                {
                    _recordCount = value;
                    OnPropertyChanged("Record Count");
                }
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="RecordDisplayViewModel"/> class.
            /// </summary>
            /// <param name="tableName">The name of the table associated with the records.</param>
            /// <param name="recordCount">The number of records in the table.</param>
            public RecordDisplayViewModel(string tableName, int recordCount)
            {
                TableName = tableName;
                RecordCount = recordCount;
            }

            public event PropertyChangedEventHandler PropertyChanged;

            protected virtual void OnPropertyChanged(string propertyName)
            {
                Console.WriteLine("Property Changed");
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}