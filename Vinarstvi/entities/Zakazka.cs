using System;
using CsvHelper.Configuration.Attributes;

namespace Vinarstvi.entities
{
    /// <summary>
    /// Represents an order entity.
    /// </summary>
    public class Zakazka
    {
        /// <summary>
        /// Gets or sets the ID of the order.
        /// </summary>
        [Ignore]
        public int id { get; set; }

        /// <summary>
        /// Gets or sets the date of the order.
        /// </summary>
        public DateTime datum_objednani { get; set; }

        /// <summary>
        /// Gets or sets the price of the order.
        /// </summary>
        public double cena { get; set; }

        /// <summary>
        /// Gets or sets the ID of the wine associated with the order.
        /// </summary>
        public int vino_id { get; set; }

        /// <summary>
        /// Gets or sets the ID of the customer associated with the order.
        /// </summary>
        public int odberatel_id { get; set; }
    }
}