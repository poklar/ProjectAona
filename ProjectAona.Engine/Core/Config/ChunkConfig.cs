using System;

namespace ProjectAona.Engine.Core.Config
{
    public class ChunkConfig
    {
        /// <summary>
        /// Gets or sets the width in tiles.
        /// </summary>
        /// <value>
        /// The width in tiles.
        /// </value>
        public int WidthInTiles { get; set; }

        /// <summary>
        /// Gets or sets the height in tiles.
        /// </summary>
        /// <value>
        /// The height in tiles.
        /// </value>
        public int HeightInTiles { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChunkConfig"/> class.
        /// </summary>
        internal ChunkConfig()
        {
            WidthInTiles = 16;
            HeightInTiles = 16;
        }

        /// <summary>
        /// Validates this configuration.
        /// </summary>
        /// <returns></returns>
        internal bool Validate()
        {
            if (WidthInTiles == 0)
                throw new ChunkConfigException("Chunk width in tiles can not be set to zero!");

            if (HeightInTiles == 0)
                throw new ChunkConfigException("Chunk height in tiles can not be set to zero!");

            return true;
        }

        /// <summary>
        /// Chunk configuration exception.
        /// </summary>
        /// <seealso cref="System.Exception" />
        public class ChunkConfigException : Exception
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="ChunkConfigException"/> class.
            /// </summary>
            /// <param name="message">The message that describes the error.</param>
            public ChunkConfigException(string message)
                : base(message)
            {

            }
        }
    }
}
