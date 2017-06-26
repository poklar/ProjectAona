using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectAona.Engine.Settings
{
    /// <summary>
    /// World settings.
    /// </summary>
    public class WorldSettings
    {
        /// <summary>
        /// The mapwidth.
        /// </summary>
        public const int MAPWIDTH = 16 * 2;
        /// <summary>
        /// The mapheight.
        /// </summary>
        public const int MAPHEIGHT = 16 * 2;

        /// <summary>
        /// Settings belonging to the player.
        /// </summary>
        public class Player
        {
            /// <summary>
            /// The movespeed.
            /// </summary>
            public const float MOVESPEED = 3.5f;
        }
    }
}
