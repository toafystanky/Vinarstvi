using System;
using System.Windows;

namespace Vinarstvi.window
{
    /// <summary>
    /// Interaction logic for InputDialog.xaml
    /// </summary>
    public partial class InputDialog : Window
    {
        /// <summary>
        /// Gets the entered ID.
        /// </summary>
        public int EnteredId { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="InputDialog"/> class.
        /// </summary>
        /// <param name="entityName">The name of the entity for which the ID is entered.</param>
        public InputDialog(string entityName)
        {
            InitializeComponent();
            Title = $"Enter {entityName} ID";
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Ok Button clicked");
            try
            {
                if (int.TryParse(IdTextBox.Text, out int enteredId))
                {
                    EnteredId = enteredId;
                    DialogResult = true;
                    // Debug statement
                    Console.WriteLine($"Entered ID: {enteredId}");
                }
                else
                {
                    MessageBox.Show("Invalid ID. Please enter a numeric value.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                // Debug statement
                Console.WriteLine($"Error while processing entered ID: {ex.Message}");
                MessageBox.Show($"Error while processing entered ID: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}