using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using MySql.Data.MySqlClient;
using Vinarstvi.controller;
using Vinarstvi.entities;

namespace Vinarstvi.forms
{
    /// <summary>
    /// Manages the generation and saving of entity forms.
    /// </summary>
    public class FormManager
    {
        private DatabaseManager _dbManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="FormManager"/> class.
        /// </summary>
        /// <param name="dbManager">The database manager.</param>
        public FormManager(DatabaseManager dbManager)
        {
            _dbManager = dbManager;
        }

        /// <summary>
        /// Generates a form for editing a specific entity.
        /// </summary>
        /// <param name="entityName">The name of the entity.</param>
        /// <param name="id">The ID of the entity.</param>
internal void GenerateEntityForm(string entityName, int id)
{
    try
    {
        Debug.WriteLine($"Generating form for entity: {entityName}");

        // Create a new Window for the entity form
        var window = new Window
        {
            Title = $"Edit {entityName} Record",
            SizeToContent = SizeToContent.Manual,
            WindowStartupLocation = WindowStartupLocation.CenterScreen,
            Width = 300,
            ResizeMode = ResizeMode.NoResize,
            Margin = new Thickness(20, 20, 20, 20)
        };
        var form = new StackPanel();
        var entityProperties = GetEntityProperties(entityName);
        
        // Fetch existing attributes from the database using the provided id
        var existingAttributes = _dbManager.GetEntryById(entityName, id);

        foreach (var property in entityProperties)
        {
            var label = new Label
            {
                Content = property,
                Margin = new Thickness(6, 0, 0, 5)
            };
            UIElement inputControl;
            if (property.ToLower() == "barva")
            {
                var comboBox = new ComboBox
                {
                    Margin = new Thickness(10, 0, 10, 10),
                    ItemsSource = Enum.GetValues(typeof(Barva)),
                    SelectedIndex = 0
                };
                inputControl = comboBox;
            }
            else
            {
                var textBox = new TextBox
                {
                    Margin = new Thickness(10, 0, 10, 10)
                };
                // Set the default value if it exists
                if (existingAttributes != null && existingAttributes.ContainsKey(property))
                {
                    textBox.Text = existingAttributes[property].ToString();
                }
                inputControl = textBox;
            }

            form.Children.Add(label);
            form.Children.Add(inputControl);
        }

        // Calculate the required height based on the number of text fields
        int numTextFields = entityProperties.Count;
        double requiredHeight = numTextFields * 70 + 60;
        window.Height = Math.Max(requiredHeight, 150);
        var saveButton = new Button
        {
            Content = "Save",
            HorizontalAlignment = HorizontalAlignment.Center,
            Margin = new Thickness(150, 10, 0, 10),
            Width = Convert.ToDouble("100"),
            Height = Convert.ToDouble("30")
        };
        // Pass the entityName and id to the SaveEntity method
        saveButton.Click += (sender, e) => SaveEntity(entityName, id, form);
        form.Children.Add(saveButton);
        window.Content = form;
        window.ShowDialog();
    }
    catch (Exception ex)
    {
        // Log or display the exception details
        MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
    }
}
        /// <summary>
        /// Retrieves the list of properties for the specified entity.
        /// </summary>
        /// <param name="entityName">The name of the entity.</param>
        /// <returns>A list of properties for the specified entity.</returns>
        private List<string> GetEntityProperties(string entityName)
        {
            switch (entityName)
            {
                case "Vinarstvi":
                    return new List<string>
                    {
                        "nazev",
                        "adresa",
                        "telefon",
                        "email"
                    };
                case "Vinice":
                    return new List<string>
                    {
                        "nazev",
                        "rozloha",
                        "rok_zalozeni",
                        "vinarstvi_id"
                    };
                case "Odberatel":
                    return new List<string>
                    {
                        "jmeno",
                        "prijmeni",
                        "adresa",
                        "email",
                        "telefon"
                    };
                case "Vino":
                    return new List<string>
                    {
                        "nazev",
                        "rocnik",
                        "mnozstvi",
                        "cena",
                        "vinice_id"
                    };
                case "Odruda":
                    return new List<string>
                    {
                        "nazev",
                        "barva",
                        "popis"
                    };
                case "Vino_Odruda":
                    return new List<string>
                    {
                        "vino_id",
                        "odruda_id"
                    };
                case "Zakazka":
                    return new List<string>
                    {
                        "datum_objednani",
                        "cena",
                        "vino_id",
                        "odberatel_id"
                    };
                default:
                    throw new InvalidOperationException($"Unknown entity: {entityName}");
            }
        }
        
        
        
        /// <summary>
        /// Saves the entity with the specified properties and values to the database.
        /// </summary>
        /// <param name="entityName">The name of the entity.</param>
        /// <param name="id">The ID of the entity.</param>
        /// <param name="form">The StackPanel containing the form controls.</param>
private void SaveEntity(string entityName, int id, StackPanel form)
{
    try
    {
        Debug.WriteLine($"SaveEntity method called for entity: {entityName}");

        // Get the type of the entity
        var entityType = Type.GetType($"Vinarstvi.entities.{entityName}");
        if (entityType != null)
        {
            // Create an instance of the entity
            var entity = CreateInstance(entityType);
            var properties = entityType.GetProperties().Where(p => p.Name != "id").ToList();
            foreach (var element in form.Children)
            {
                if (element is Label label)
                {
                    Debug.WriteLine($"Processing label: {label.Content}");

                    var propertyName = label.Content.ToString();
                    var property = properties.FirstOrDefault(p => p.Name == propertyName);
                    if (propertyName.ToLower() == "barva" &&
                        form.Children[form.Children.IndexOf(label) + 1] is ComboBox comboBox)
                    {
                        Debug.WriteLine($"Processing ComboBox for property: {propertyName}");

                        if (property.PropertyType.IsEnum)
                        {
                            if (comboBox.SelectedItem != null)
                            {
                                var selectedEnumValue = Enum.Parse(property.PropertyType,
                                    comboBox.SelectedItem.ToString());
                                property.SetValue(entity, selectedEnumValue);
                                Debug.WriteLine($"Enum property '{propertyName}' value set: {selectedEnumValue}");
                            }
                        }
                    }
                    else if (form.Children[form.Children.IndexOf(label) + 1] is TextBox textBox)
                    {
                        var inputValue = textBox.Text;
                        if (property != null)
                        {
                            if (!IsValidInput(inputValue, property.PropertyType, propertyName))
                            {
                                MessageBox.Show(
                                    $"Error: Invalid input for {propertyName}. Please enter a valid value.",
                                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                                return;
                            }
                            else
                            {
                                var convertedValue = Convert.ChangeType(inputValue, property.PropertyType);
                                property.SetValue(entity, convertedValue);
                                Debug.WriteLine($"Property '{propertyName}' value set: {convertedValue}");
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show($"Error: Cannot get input for {propertyName}.", "Error",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }
            }

            Debug.WriteLine($"Entity created and properties set: {entity}");

            var tableName = _dbManager.GetTableName(entityType);
            Debug.WriteLine($"_dbManager is null: {_dbManager == null}");
            tableName = _dbManager.GetTableName(entityType);
            Debug.WriteLine($"Table name retrieved: {tableName}");

            if (tableName != null)
            {
                Debug.WriteLine($"Table name retrieved: {tableName}");
                // Pass the id parameter along with the entity for updating
                _dbManager.UpdateRowInDatabase(entity, tableName, id);
                MessageBox.Show("Data saved successfully.", "Success", MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            else
            {
                Debug.WriteLine("Error: Table name is null.");
            }
        }
    }
    catch (Exception ex)
    {
        Debug.WriteLine($"Exception occurred during save: {ex.Message}");

        if (ex.InnerException is MySqlException mySqlException)
        {
            // Add debug statements for specific MySQL exceptions (e.g., duplicate entry)
        }

        // Log or display the exception details
        MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
    }
}

        
        
        /// <summary>
        /// Validates the input value for a property.
        /// </summary>
        /// <param name="inputValue">The input value to validate.</param>
        /// <param name="propertyType">The type of the property.</param>
        /// <param name="propertyName">The name of the property.</param>
        /// <returns>True if the input value is valid for the property; otherwise, false.</returns>
        private bool IsValidInput(string inputValue, Type propertyType, string propertyName)
        {
            if (string.IsNullOrEmpty(inputValue))
            {
                return false;
            }

            try
            {
                Convert.ChangeType(inputValue, propertyType);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        
        
        /// <summary>
        /// Creates an instance of the specified type.
        /// </summary>
        /// <param name="type">The type of the instance to create.</param>
        /// <returns>An instance of the specified type.</returns>
        private object CreateInstance(Type type)
        {
            try
            {
                Console.WriteLine($"CreateInstance method called for type: {type}");
                var instance = Activator.CreateInstance(type);
                Console.WriteLine($"Instance created: {instance}");
                return instance;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occurred during instance creation: {ex.Message}");
                return null;
            }
        }
    }
}
