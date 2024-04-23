using System;
using CsvHelper.Configuration.Attributes;

namespace Vinarstvi.entities
{
    /// <summary>
    /// Represents a grape variety.
    /// </summary>
    public class Odruda
    {
        /// <summary>
        /// Gets or sets the name of the grape variety.
        /// </summary>
        public string nazev { get; set; }

        /// <summary>
        /// Gets or sets the color of the grape variety.
        /// </summary>
        public Barva barva { get; set; }

        /// <summary>
        /// Gets or sets the description of the grape variety.
        /// </summary>
        public string popis { get; set; }
    }

    /// <summary>
    /// Represents the color of a grape variety.
    /// </summary>
    public enum Barva
    {
        /// <summary>
        /// White grape variety.
        /// </summary>
        Bílé,

        /// <summary>
        /// Red grape variety.
        /// </summary>
        Červené,

        /// <summary>
        /// Rosé grape variety.
        /// </summary>
        Růžové
    }
}