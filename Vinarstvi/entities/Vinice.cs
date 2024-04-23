using System;
using CsvHelper.Configuration.Attributes;

namespace Vinarstvi.entities
{
    /// <summary>
    /// Represents a vineyard entity.
    /// </summary>
    public class Vinice
    {
        /// <summary>
        /// Gets or sets the ID of the vineyard.
        /// </summary>
        [Ignore]
        public int id { get; set; }

        /// <summary>
        /// Gets or sets the name of the vineyard.
        /// </summary>
        public string nazev { get; set; }

        /// <summary>
        /// Gets or sets the area of the vineyard in hectares.
        /// </summary>
        public float rozloha { get; set; } 

        /// <summary>
        /// Gets or sets the year the vineyard was established.
        /// </summary>
        public DateTime rok_zalozeni { get; set; }

        /// <summary>
        /// Gets or sets the ID of the winery associated with the vineyard.
        /// </summary>
        public int vinarstvi_id { get; set; }
    }
}