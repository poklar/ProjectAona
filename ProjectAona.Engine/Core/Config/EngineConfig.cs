namespace ProjectAona.Engine.Core.Config
{
    /// <summary>
    /// Holds the configuration parameters to be used by the engine.
    /// </summary>
    public class EngineConfig
    {
        /// <summary>
        /// Holds the chunk configuration parameters.
        /// </summary>
        /// <value>
        /// The chunk.
        /// </value>
        public ChunkConfig Chunk { get; private set; }

        /// <summary>
        /// Holds graphics related configuration parameters.
        /// </summary>
        /// <value>
        /// The graphics.
        /// </value>
        public GraphicsConfig Graphics { get; private set; }

        /// <summary>
        /// Holds the world configuration parameters.
        /// </summary>
        /// <value>
        /// The world.
        /// </value>
        public WorldConfig World { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="EngineConfig"/> class.
        /// </summary>
        public EngineConfig()
        {
            Chunk = new ChunkConfig();
            Graphics = new GraphicsConfig();
            World = new WorldConfig();
        }

        /// <summary>
        /// Validates this configuration.
        /// </summary>
        /// <returns></returns>
        internal bool Validate()
        {
            if (!Chunk.Validate())
                return false;

            if (!Graphics.Validate())
                return false;

            if (!World.Validate())
                return false;

            return true;
        }
    }
}
