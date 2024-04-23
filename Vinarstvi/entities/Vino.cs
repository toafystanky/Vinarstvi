using CsvHelper.Configuration.Attributes;

namespace Vinarstvi.entities
{
    /// <summary>
    /// Represents a wine entity.
    /// </summary>
    public class Vino
    {
        /// <summary>
        /// Gets or sets the ID of the wine.
        /// </summary>
        [Ignore]
        public int id { get; set; }

        /// <summary>
        /// Gets or sets the name of the wine.
        /// </summary>
        public string nazev { get; set; }

        /// <summary>
        /// Gets or sets the vintage of the wine.
        /// </summary>
        public int rocnik { get; set; }

        /// <summary>
        /// Gets or sets the quantity of the wine in liters.
        /// </summary>
        public double mnozstvi { get; set; }

        /// <summary>
        /// Gets or sets the price of the wine per liter.
        /// </summary>
        public double cena { get; set; }

        /// <summary>
        /// Gets or sets the ID of the vineyard associated with the wine.
        /// </summary>
        public int vinice_id { get; set; }
    }
}