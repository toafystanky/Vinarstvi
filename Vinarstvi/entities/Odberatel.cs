using CsvHelper.Configuration.Attributes;

namespace Vinarstvi.entities
{
    /// <summary>
    /// Represents an entity for storing information about a customer.
    /// </summary>
    public class Odberatel
    {
        /// <summary>
        /// Gets or sets the ID of the customer.
        /// </summary>
        [Ignore]
        public int id { get; set; }

        /// <summary>
        /// Gets or sets the first name of the customer.
        /// </summary>
        public string jmeno { get; set; }

        /// <summary>
        /// Gets or sets the last name of the customer.
        /// </summary>
        public string prijmeni { get; set; }

        /// <summary>
        /// Gets or sets the address of the customer.
        /// </summary>
        public string adresa { get; set; }

        /// <summary>
        /// Gets or sets the email address of the customer.
        /// </summary>
        public string email { get; set; }

        /// <summary>
        /// Gets or sets the phone number of the customer.
        /// </summary>
        public string telefon { get; set;}
    }
}