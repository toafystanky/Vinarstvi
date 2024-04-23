using Newtonsoft.Json;
using System;
using System.IO;

namespace Vinarstvi.controller
{
    /// <summary>
    /// Manages configuration settings for the application.
    /// </summary>
    public class ConfigurationManager
    {
        /// <summary>
        /// Gets or sets the host name or IP address of the database server.
        /// </summary>
        public string DatabaseHost { get; set; }

        /// <summary>
        /// Gets or sets the port number of the database server.
        /// </summary>
        public int DatabasePort { get; set; }

        /// <summary>
        /// Gets or sets the name of the database.
        /// </summary>
        public string DatabaseName { get; set; }

        /// <summary>
        /// Gets or sets the username used to connect to the database.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the password used to connect to the database.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Loads configuration settings from the specified file.
        /// </summary>
        /// <param name="filePath">The path to the configuration file.</param>
        /// <returns>An instance of <see cref="ConfigurationManager"/> containing the loaded configuration settings.</returns>
        /// <exception cref="FileNotFoundException">Thrown when the specified configuration file does not exist.</exception>
        /// <exception cref="JsonException">Thrown when an error occurs during JSON deserialization.</exception>
        /// <exception cref="Exception">Thrown when an error occurs while loading the configuration.</exception>
        public static ConfigurationManager LoadConfiguration(string filePath)
        {
            try
            {
                // Debug statement to indicate method entry
                Console.WriteLine($"LoadConfiguration method called with filePath: {filePath}");

                if (File.Exists(filePath))
                {
                    // Debug statement for file existence
                    Console.WriteLine($"Configuration file found at: {filePath}");

                    string json = File.ReadAllText(filePath);

                    // Debug statement for deserialization
                    Console.WriteLine("Deserializing configuration JSON...");
                    return JsonConvert.DeserializeObject<ConfigurationManager>(json);
                }
                else
                {
                    // Debug statement for file not found
                    Console.WriteLine($"Configuration file not found at: {filePath}. Using default values.");

                    // Return a new instance with default values
                    return new ConfigurationManager();
                }
            }
            catch (Exception ex)
            {
                // Debug statement for exception
                Console.WriteLine($"Error loading configuration: {ex.Message}");

                // Rethrow the exception
                throw;
            }
        }
    }
}