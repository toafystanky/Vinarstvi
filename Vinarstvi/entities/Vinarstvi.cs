using CsvHelper.Configuration.Attributes;

namespace Vinarstvi.entities
{
    /// <summary>
    /// Represents a winery entity.
    /// </summary>
    public class Vinarstvi
    {
        /// <summary>
        /// Gets or sets the ID of the winery.
        /// </summary>
        [Ignore]
        public int id { get; set; }

        /// <summary>
        /// Gets or sets the name of the winery.
        /// </summary>
        public string nazev { get; set; }

        /// <summary>
        /// Gets or sets the address of the winery.
        /// </summary>
        public string adresa { get; set; }

        /// <summary>
        /// Gets or sets the phone number of the winery.
        /// </summary>
        public string telefon { get; set; }

        /// <summary>
        /// Gets or sets the email address of the winery.
        /// </summary>
        public string email { get; set; }
    }
}