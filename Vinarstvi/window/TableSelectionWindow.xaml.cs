using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Vinarstvi.controller;

namespace Vinarstvi.window
{
    /// <summary>
    /// Represents a window for selecting a database table.
    /// </summary>
    public partial class TableSelectionWindow : Window
    {
        private readonly DatabaseManager _dbManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="TableSelectionWindow"/> class.
        /// </summary>
        /// <param name="dbManager">The <see cref="DatabaseManager"/> instance used for database operations.</param>
        public TableSelectionWindow(DatabaseManager dbManager)
        {
            InitializeComponent();
            _dbManager = dbManager;
            PopulateTableList();
        }

        /// <summary>
        /// Populates the list of database tables in the window.
        /// </summary>
        private void PopulateTableList()
        {
            List<string> tableList = _dbManager.GetTableList();
            foreach (string tableName in tableList)
            {
                var button = new Button
                {
                    Content = tableName,
                    Tag = tableName,
                    Width = 200,
                    Height = 30,
                    Margin = new Thickness(5),
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center
                };
                button.Click += TableButton_Click;
                tablePanel.Children.Add(button);
            }
        }

        /// <summary>
        /// Gets the name of the selected table.
        /// </summary>
        public string SelectedTable { get; private set; }

        private void TableButton_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Table Button clicked");
            string tableName = (string)((Button)sender).Tag;
            SelectedTable = tableName; // Set the selected table name
            DialogResult = true;
        }
    }
}
