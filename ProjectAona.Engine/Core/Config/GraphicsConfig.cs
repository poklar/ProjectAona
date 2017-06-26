using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectAona.Engine.Core.Config
{
    /// <summary>
    /// Contains graphics related configuration parameters. 
    /// </summary>
    public class GraphicsConfig
    {
        /// <summary>
        /// Gets or sets the screen width.
        /// </summary>
        /// <value>
        /// The width.
        /// </value>
        public int Width { get; set; }

        /// <summary>
        /// Gets or sets the screen height.
        /// </summary>
        /// <value>
        /// The height.
        /// </value>
        public int Height { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [full screen enabled].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [full screen enabled]; otherwise, <c>false</c>.
        /// </value>
        public bool FullScreenEnabled { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GraphicsConfig"/> class.
        /// </summary>
        internal GraphicsConfig()
        {
            // Set the defaults
            Width = 800;
            Height = 600;
            FullScreenEnabled = false;
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
