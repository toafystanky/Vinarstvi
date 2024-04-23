using System;
using System.Windows;

namespace Vinarstvi.window
{
    /// <summary>
    /// Represents a window for updating an entry in the database.
    /// </summary>
    public partial class UpdateEntryWindow : Window
    {
        /// <summary>
        /// Gets the ID entered for the entry to be updated.
        /// </summary>
        public int EnteredId { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateEntryWindow"/> class.
        /// </summary>
        /// <param name="entityName">The name of the entity being updated.</param>
        /// <param name="enteredId">The ID of the entry to be updated.</param>
        public UpdateEntryWindow(string entityName, int enteredId)
        {
            InitializeComponent();
            Title = $"Update {entityName} Entry";
            EnteredId = enteredId;
        }

        /// <summary>
        /// Handles the click event of the OK button.
        /// </summary>
        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Ok Button clicked");
            try
            {
                DialogResult = true;
            }
            catch (Exception ex)
            {
                // Log or display the exception details
                MessageBox.Show($"An error occurred while updating the entry: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}