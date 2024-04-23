using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using Vinarstvi.forms;

namespace Vinarstvi.buttons
{
    /// Represents a button for handling entity-related actions.
    /// </summary>
    public class EntityButton
    {
        private FormManager _formManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityButton"/> class.
        /// </summary>
        /// <param name="formManager">The form manager instance.</param>
        public EntityButton(FormManager formManager)
        {
            _formManager = formManager;
        }

        /// <summary>
        /// Handles the click event of the entity button.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The event arguments.</param>
        public void EntityButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var entityName = (string)((Button)sender).Tag;
                Debug.WriteLine($"Generating form for entity: {entityName}");
                _formManager.GenerateEntityForm(entityName, 0);
            }
            catch (Exception ex)
            {
                // Log or display the exception details
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}