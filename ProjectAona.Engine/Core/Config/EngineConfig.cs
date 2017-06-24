using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectAona.Engine.Core.Config
{
    /// <summary>
    /// Holds the configuration parameters to be used by the engine.
    /// </summary>
    public class EngineConfig
    {
        /// <summary>
        /// Holds graphics related configuration parameters.
        /// </summary>
        /// <value>
        /// The graphics.
        /// </value>
        public GraphicsConfig Graphics { get; private set; }

        public EngineConfig()
        {
            Graphics = new GraphicsConfig();
        }

        /// <summary>
        /// Validates this configuration.
        /// </summary>
        /// <returns></returns>
        internal bool Validate()
        {
            if (!Graphics.Validate())
                return false;

            return true;
        }
    }
}
