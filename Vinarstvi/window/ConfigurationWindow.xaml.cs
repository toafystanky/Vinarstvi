using System;
using System.Configuration;
using System.IO;
using System.Windows;
using Newtonsoft.Json;

namespace Vinarstvi.window
{
    /// <summary>
    /// Interaction logic for ConfigurationWindow.xaml
    /// </summary>
    public partial class ConfigurationWindow : Window
    {
        private const string ConfigFilePath = "../../config/config.json";

        /// <summary>
        /// Gets or sets the configuration settings.
        /// </summary>
        public Configuration Config { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationWindow"/> class.
        /// </summary>
        public ConfigurationWindow()
        {
            InitializeComponent();
            LoadConfiguration();
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Apply Button clicked");
            UpdateConfigFromUI();
            SaveConfiguration();
            RestartApplication();
        }

        private void RestartApplication()
        {
            System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
            Application.Current.Shutdown();
        }

        /// <summary>
        /// Saves the configuration settings to a JSON file.
        /// </summary>
        public void SaveConfiguration()
        {
            try
            {
                string json = JsonConvert.SerializeObject(Config, Formatting.Indented);
                File.WriteAllText(ConfigFilePath, json);
                // Debug statement
                Console.WriteLine("Configuration saved successfully.");
            }
            catch (Exception ex)
            {
                // Debug statement
                Console.WriteLine($"Error saving configuration: {ex.Message}");
                MessageBox.Show($"Error saving configuration: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadConfiguration()
        {
            try
            {
                if (File.Exists(ConfigFilePath))
                {
                    string json = File.ReadAllText(ConfigFilePath);
                    Config = JsonConvert.DeserializeObject<Configuration>(json);
                    UpdateUIFromConfig();
                }
                else
                {
                    Config = new Configuration();
                }
                // Debug statement
                Console.WriteLine("Configuration loaded successfully.");
            }
            catch (Exception ex)
            {
                // Debug statement
                Console.WriteLine($"Error loading configuration: {ex.Message}");
                MessageBox.Show($"Error loading configuration: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UpdateUIFromConfig()
        {
            DatabaseServerTextBox.Text = Config.DatabaseHost;
            DatabasePortTextBox.Text = Config.DatabasePort;
            DatabaseNameTextBox.Text = Config.DatabaseName;
            UsernameTextBox.Text = Config.Username;
            // Password cannot be directly set for security reasons
        }

        private void UpdateConfigFromUI()
        {
            Config.DatabaseHost = DatabaseServerTextBox.Text;
            Config.DatabasePort = DatabasePortTextBox.Text;
            Config.DatabaseName = DatabaseNameTextBox.Text;
            Config.Username = UsernameTextBox.Text;
            Config.Password = PasswordBox.Password;
        }
    }

    /// <summary>
    /// Represents the configuration settings.
    /// </summary>
    public class Configuration
    {
        /// <summary>
        /// Gets or sets the database host.
        /// </summary>
        public string DatabaseHost { get; set; }

        /// <summary>
        /// Gets or sets the database port.
        /// </summary>
        public string DatabasePort { get; set; }

        /// <summary>
        /// Gets or sets the database name.
        /// </summary>
        public string DatabaseName { get; set; }

        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Configuration"/> class.
        /// </summary>
        public Configuration()
        {
            DatabaseHost = null;
            DatabasePort = null;
            DatabaseName = null;
            Username = null;
            Password = null;
        }
    }
}
