using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectAona.Engine.Core.Config
{
    /// <summary>
    /// The world configurations.
    /// </summary>
    public class WorldConfig
    {
        /// <summary>
        /// Gets or sets the width of the map.
        /// </summary>
        /// <value>
        /// The width of the map.
        /// </value>
        public int MapWidth { get; set; }

        /// <summary>
        /// Gets or sets the height of the map.
        /// </summary>
        /// <value>
        /// The height of the map.
        /// </value>
        public int MapHeight { get; set; }

        /// <summary>
        /// Gets or sets the map seed.
        /// </summary>
        /// <value>
        /// The seed.
        /// </value>
        public int Seed { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="WorldConfig"/> class.
        /// </summary>
        internal WorldConfig()
        {
            // Set the defaults
            MapWidth = 16 * 10;
            MapHeight = 16 * 10;
            Seed = 100;
        }

        /// <summary>
        /// Validates this configuration.
        /// </summary>
        /// <returns></returns>
        internal bool Validate()
        {
            return true;
        }
    }
}
