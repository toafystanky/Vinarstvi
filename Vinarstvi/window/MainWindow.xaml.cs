using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using MySql.Data.MySqlClient;
using Vinarstvi.buttons;
using Vinarstvi.controller;
using Vinarstvi.entities;
using Vinarstvi.exceptions;
using Vinarstvi.forms;
using Vinarstvi.utilities;

namespace Vinarstvi.window
{
    
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly DatabaseManager _dbManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            try
            {
                ConfigurationManager config = ConfigurationManager.LoadConfiguration("../../config/config.json");
                _dbManager = new DatabaseManager(config);
            }
            catch (Exception ex)
            {
                // Log or display the exception details
                MessageBox.Show($"An error occurred while initializing the database manager: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            // Set the icon of the window
            Uri iconUri = new Uri("../../img/icon.jpg", UriKind.Relative);
            Icon = BitmapFrame.Create(iconUri);
        }


        private StackPanel _generatedForm;
        private EntityButton _entityButton;

        /// <summary>
        /// Handles the Click event of the BrowseEntriesButton control.
        /// </summary>
        private void BrowseEntriesButton_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Browse Entries Button clicked");
            try
            {
                TableSelectionWindow tableSelectionWindow = new TableSelectionWindow(_dbManager);
                if (tableSelectionWindow.ShowDialog() == true)
                {
                    string selectedTable = tableSelectionWindow.SelectedTable;
    
                    // Retrieve all records from the selected table
                    DataTable tableData = _dbManager.GetAllRecords(selectedTable);

                    // Create a window to display the records
                    RecordDisplayWindow recordDisplayWindow = new RecordDisplayWindow(tableData, selectedTable);
                    recordDisplayWindow.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        
        /// <summary>
        /// Handles the Click event of the AddEditEntriesButton control.
        /// </summary>
        private void AddEditEntriesButton_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Add Entries Button clicked");
            try
            {
                List<string> entityList = new List<string>
                {
                    "Vinarstvi",
                    "Vinice",
                    "Odberatel",
                    "Vino",
                    "Odruda",
                    "Zakazka",
                    "Vino_Odruda"
                };
                _generatedForm = new StackPanel()
                {
                    Margin = new Thickness(0, 60, 0, 0)
                };

                _entityButton = new EntityButton(new FormManager(_dbManager));

                foreach (var entity in entityList)
                {
                    var button = new Button
                    {
                        Content = entity.ToUpper(),
                        Tag = entity,
                        Width = 200,
                        Height = 10,
                        Background = Brushes.Black,
                        Foreground = Brushes.White,
                        BorderBrush = Brushes.Plum,
                        BorderThickness = new Thickness(1),
                        Margin = new Thickness(0, 5, 0, 5)
                    };

                    button.Click += _entityButton.EntityButton_Click;
                    _generatedForm.Children.Add(button);
                }

                var backButton = new Button
                {
                    Content = "BACK",
                    Width = 40,
                    Height = 8,
                    Background = Brushes.Black,
                    Foreground = Brushes.White,
                    BorderBrush = Brushes.Plum,
                    BorderThickness = new Thickness(1),
                    Margin = new Thickness(0, 10, 0, 0)
                };
                backButton.Click += BackButton_Click;
                _generatedForm.Children.Add(backButton);
                mainContentControl.Content = _generatedForm;
            }
            catch (Exception ex)
            {
                // Log or display the exception details
                Debug.WriteLine($"An error occurred: {ex.Message}");
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Handles the Click event.
        /// </summary>
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Back Button clicked");
            mainContentControl.Content = GenerateInitialContent();
        }

        private StackPanel generatedDeleteForm;

        /// <summary>
        /// Handles the Click event.
        /// </summary>
        private void DeleteEntriesButton_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Delete Entries Button clicked");
            try
            {
                List<string> entityList = new List<string>
                {
                    "Vinarstvi",
                    "Vinice",
                    "Odberatel",
                    "Vino",
                    "Odruda",
                    "Zakazka",
                    "Vino_Odruda"
                };

                generatedDeleteForm = new StackPanel()
                {
                    Margin = new Thickness(0, 20, 0, 0)
                };

                foreach (var entity in entityList)
                {
                    var button = new Button
                    {
                        Content = entity.ToUpper(),
                        Tag = entity,
                        Width = 200,
                        Height = 10,
                        Background = Brushes.Black,
                        Foreground = Brushes.White,
                        BorderBrush = Brushes.Plum,
                        BorderThickness = new Thickness(1),
                        Margin = new Thickness(0, 5, 0, 5)
                    };
                    button.Click += DeleteEntityButton_Click;
                    generatedDeleteForm.Children.Add(button);
                }

                var backButton = new Button
                {
                    Content = "BACK",
                    Width = 40,
                    Height = 8,
                    Background = Brushes.Black,
                    Foreground = Brushes.White,
                    BorderBrush = Brushes.Plum,
                    BorderThickness = new Thickness(1),
                    Margin = new Thickness(0, 10, 0, 0)
                };
                backButton.Click += BackButton_Click;
                generatedDeleteForm.Children.Add(backButton);
                mainContentControl.Content = generatedDeleteForm;
            }
            catch (Exception ex)
            {
                // Log or display the exception details
                Debug.WriteLine($"An error occurred: {ex.Message}");
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Handles the Click event.
        /// </summary>
        private void UpdateEtriesButton_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Update Entries Button clicked");
            try
            {
                List<string> entityList = new List<string>
                {
                    "Vinarstvi",
                    "Vinice",
                    "Odberatel",
                    "Vino",
                    "Odruda",
                    "Zakazka",
                    "Vino_Odruda"
                };
                _generatedForm = new StackPanel()
                {
                    Margin = new Thickness(0, 60, 0, 0)
                };

                foreach (var entity in entityList)
                {
                    var button = new Button
                    {
                        Content = entity.ToUpper(),
                        Tag = entity,
                        Width = 200,
                        Height = 10,
                        Background = Brushes.Black,
                        Foreground = Brushes.White,
                        BorderBrush = Brushes.Plum,
                        BorderThickness = new Thickness(1),
                        Margin = new Thickness(0, 5, 0, 5)
                    };
                    button.Click += UpdateEntityButton_Click; // Change to the appropriate click handler
                    _generatedForm.Children.Add(button);
                }

                var backButton = new Button
                {
                    Content = "BACK",
                    Width = 40,
                    Height = 8,
                    Background = Brushes.Black,
                    Foreground = Brushes.White,
                    BorderBrush = Brushes.Plum,
                    BorderThickness = new Thickness(1),
                    Margin = new Thickness(0, 10, 0, 0)
                };
                backButton.Click += BackButton_Click;
                _generatedForm.Children.Add(backButton);
                mainContentControl.Content = _generatedForm;
            }
            catch (Exception ex)
            {
                // Log or display the exception details
                Debug.WriteLine($"An error occurred: {ex.Message}");
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        
        /// <summary>
        /// Handles the Click event.
        /// </summary>
        private void UpdateEntityButton_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Update Entity Button clicked");
            try
            {
                var entityName = (string)((Button)sender).Tag;
                var inputDialog = new InputDialog("Enter ID for the entry to update");
                if (inputDialog.ShowDialog() == true)
                {
                    if (inputDialog.EnteredId != 0)
                    {
                        // Pass the entered ID to the OpenUpdateForm method
                        OpenUpdateForm(entityName, inputDialog.EnteredId);
                    }
                    else
                    {
                        MessageBox.Show("Invalid ID entered. Please enter a valid integer ID.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                // Log or display the exception details
                Debug.WriteLine($"An error occurred: {ex.Message}");
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Handles the Click event.
        /// </summary>
        private void OpenUpdateForm(string entityName, int enteredId)
        {
            try
            {
                // Generate the update form for the selected entity and ID
                FormManager _formManager = new FormManager(_dbManager);
                // Pass the entered ID to the GenerateEntityForm method
                _formManager.GenerateEntityForm(entityName, enteredId);
            }
            catch (Exception ex)
            {
                // Log or display the exception details
                Debug.WriteLine($"An error occurred while opening the update form: {ex.Message}");
                MessageBox.Show($"An error occurred while opening the update form: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Handles the Click event.
        /// </summary>
        private void DeleteEntityButton_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Delete Entity Button clicked");
            try
            {
                var entityName = (string)((Button)sender).Tag;
                var inputDialog = new InputDialog(entityName);
                if (inputDialog.ShowDialog() == true)
                {
                    int enteredId = inputDialog.EnteredId;
                    MainWindowMethods.DeleteRecordById(entityName, enteredId, _dbManager);
                }
            }
            catch (Exception ex)
            {
                // Log or display the exception details
                Debug.WriteLine($"An error occurred: {ex.Message}");
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        
        /// <summary>
        /// Generates the initial content of the window.
        /// </summary>
        /// <returns>The initial content as a UIElement.</returns>
        private UIElement GenerateInitialContent()
        {
            var initialContent = new StackPanel
            {
                VerticalAlignment = VerticalAlignment.Center, // Align the stack panel vertically centered
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(40)
            };
            var titleTextBlock = new TextBlock
            {
                Text = "Winary Management System",
                Style = (Style)FindResource(typeof(TextBlock))
            };
            titleTextBlock.FontSize = 28;
            initialContent.Children.Add(titleTextBlock);
            var buttons = new List<string>
            {
                "BROWSE ENTRIES",
                "ADD ENTRIES",
                "DELETE ENTRIES",
                "UPDATE ENTRIES",
                "IMPORT (.CSV)",
                "GENERATE REPORT",
                "CONFIGURATION"
            };
            foreach (var buttonText in buttons)
            {
                var button = new Button
                {
                    Content = buttonText,
                };
                button.Style = (Style)FindResource(typeof(Button));
                switch (buttonText)
                {
                    case "BROWSE ENTRIES":
                        button.Click += BrowseEntriesButton_Click;
                        break;
                    case "ADD ENTRIES":
                        button.Click += AddEditEntriesButton_Click;
                        break;
                    case "DELETE ENTRIES":
                        button.Click += DeleteEntriesButton_Click;
                        break;
                    case "UPDATE ENTRIES":
                        button.Click += UpdateEtriesButton_Click;
                        break;
                    case "IMPORT (.CSV)":
                        button.Click += ImportButton_Click;
                        break;
                    case "GENERATE REPORT":
                        button.Click += GenerateReportButton_Click;
                        break;
                    case "CONFIGURATION":
                        button.Click += ConfigurationButton_Click;
                        break;
                }

                initialContent.Children.Add(button);
            }

            ApplyStyles(initialContent);
            return initialContent;
        }

        /// <summary>
        /// Applies the styles to the specified framework element.
        /// </summary>
        /// <param name="element">The framework element to which styles will be applied.</param>
        private void ApplyStyles(FrameworkElement element)
        {
            var buttonStyle = (Style)FindResource(typeof(Button));
            var textBlockStyle = (Style)FindResource(typeof(TextBlock));
            if (buttonStyle != null)
            {
                foreach (var button in FindVisualChildren<Button>(element))
                {
                    double width = button.Width;
                    double height = button.Height;
                    button.Style = buttonStyle;
                    button.Width = width;
                    button.Height = height;
                }
            }

            if (textBlockStyle != null)
            {
                foreach (var textBlock in FindVisualChildren<TextBlock>(element))
                {
                    textBlock.Style = textBlockStyle;
                }
            }
        }

       /// <summary>
/// Handles the click event of the Import Button.
/// </summary>
/// <param name="sender">The object that raised the event.</param>
/// <param name="e">The event data.</param>
private void ImportButton_Click(object sender, RoutedEventArgs e)
{
    Console.WriteLine("Import Button clicked");
    try
    {
        OpenFileDialog openFileDialog = new OpenFileDialog();
        openFileDialog.Filter = "CSV Files (*.csv)|*.csv";
        if (openFileDialog.ShowDialog() == true)
        {
            string filePath = openFileDialog.FileName;
            try
            {
                IDictionary<string, Type> tableAttributes = new Dictionary<string, Type>
                {
                    {
                        "Vinarstvi",
                        typeof(Vinarstvi.entities.Vinarstvi)
                    },
                    {
                        "Vinice",
                        typeof(Vinice)
                    },
                    {
                        "Vino",
                        typeof(Vino)
                    },
                    {
                        "Odberatel",
                        typeof(Odberatel)
                    },
                    {
                        "Odruda",
                        typeof(Odruda)
                    },
                    {
                        "Zakazka",
                        typeof(Zakazka)
                    },
                    {
                        "Vino_Odruda",
                        typeof(Vino_Odruda)
                    }
                };
                _dbManager.ImportData<dynamic>(filePath, tableAttributes);
                MessageBox.Show("Data has been saved successfully.", "Success", MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("Error: CSV file not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (TableNotFoundException ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                if (ex.InnerException is MySqlException mySqlException && mySqlException.Number == 1062)
                {
                    MessageBox.Show($"Error during import: {mySqlException.Message}", "Error", MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
                else
                {
                    MessageBox.Show($"Error during import: {ex.Message}", "Error", MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
            }
        }
    }
    catch (Exception ex)
    {
        // Log or display the exception details
        Debug.WriteLine($"An error occurred: {ex.Message}");
        MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
    }
}


        /// <summary>
        /// Finds the visual children of the specified dependency object.
        /// </summary>
        /// <typeparam name="T">The type of the visual children to find.</typeparam>
        /// <param name="depObj">The dependency object to search.</param>
        /// <returns>An enumerable collection of the visual children of the specified dependency object.</returns>
        private IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }

        /// <summary>
        /// Handles the click event of the Generate Report Button.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The event data.</param>
        private void GenerateReportButton_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Generate Report Button clicked");
            MainWindowMethods.ReportButtonMethod(_dbManager);
        }

        /// <summary>
        /// Handles the click event of the Configuration Button.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The event data.</param>
        private void ConfigurationButton_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Configuration Button clicked");
            try
            {
                ConfigurationWindow configWindow = new ConfigurationWindow();
                configWindow.ShowDialog();
            }
            catch (Exception ex)
            {
                // Log or display the exception details
                Debug.WriteLine($"An error occurred: {ex.Message}");
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}
