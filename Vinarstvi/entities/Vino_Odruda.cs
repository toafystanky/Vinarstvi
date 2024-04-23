namespace Vinarstvi.entities
{
    /// <summary>
    /// Represents the relationship between a wine and a grape variety.
    /// </summary>
    public class Vino_Odruda
    {
        /// <summary>
        /// Gets or sets the ID of the wine associated with the grape variety.
        /// </summary>
        public int vino_id { get; set; }

        /// <summary>
        /// Gets or sets the ID of the grape variety associated with the wine.
        /// </summary>
        public int odruda_id { get; set; }
    }
}