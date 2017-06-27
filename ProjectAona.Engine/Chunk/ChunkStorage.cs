using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace ProjectAona.Engine.Chunk
{
    /// <summary>
    /// Allows interaction with the chunk storage service.
    /// </summary>
    public interface IChunkStorage
    {
        /// <summary>
        /// Returns the chunk in given point coordinate (x, y).
        /// </summary>
        /// <value>
        /// The <see cref="Chunk"/>.
        /// </value>
        /// <param name="worldQuadrant">The world quadrant.</param>
        /// <returns></returns>
        Chunk this[Point worldQuadrant] { get; set; }

        /// <summary>
        /// Removes the chunk at given world quadrant.
        /// </summary>
        /// <param name="worldQuadrant">The world quadrant.</param>
        /// <returns></returns>
        bool Remove(Point worldQuadrant);

        /// <summary>
        /// Returns true if a chunk exists at given world quadrant.
        /// </summary>
        /// <param name="worldQuadrant">The world quadrant.</param>
        /// <returns>
        ///   <c>true</c> if the specified world quadrant contains key; otherwise, <c>false</c>.
        /// </returns>
        bool ContainsKey(Point worldQuadrant);

        /// <summary>
        /// Returns total count of chunk stored.
        /// </summary>
        /// <value>
        /// The count.
        /// </value>
        int Count { get; }

        /// <summary>
        /// Returns an enumarable list of keys stored.
        /// </summary>
        /// <value>
        /// The keys.
        /// </value>
        IEnumerable<Point> Keys { get; }

        /// <summary>
        /// Returns an enumarable list of chunks stored.
        /// </summary>
        /// <value>
        /// The values.
        /// </value>
        IEnumerable<Chunk> Values { get; }
    }
    /// <summary>
    /// Chunk storage that stores chunks within memory and can load & save chunks to disk.
    /// </summary>
    /// <seealso cref="Microsoft.Xna.Framework.GameComponent" />
    /// <seealso cref="ProjectAona.Engine.Chunk.IChunkStorage" />
    public class ChunkStorage : GameComponent, IChunkStorage
    {
        private readonly Dictionary<Point, Chunk> _dictionary = new Dictionary<Point, Chunk>();

        //private readonly IndexedDictionary<Chunk> _test;

        /// <summary>
        /// Creates a new chunk storage instance which can hold chunks.
        /// </summary>
        /// <param name="game">The game.</param>
        public ChunkStorage(Game game)
            : base(game)
        {
            // Export service
            Game.Services.AddService(typeof(IChunkStorage), this);
        }

        /// <summary>
        /// Returns the chunk in given point coordinate (x, y).
        /// </summary>
        /// <value>
        /// The <see cref="Chunk"/>.
        /// </value>
        /// <param name="worldQuadrant">The world quadrant.</param>
        /// <returns></returns>
        public Chunk this[Point worldQuadrant]
        {
            get { return _dictionary[worldQuadrant]; }
            set { _dictionary[worldQuadrant] = value; }
        }

        /// <summary>
        /// Removes the chunk at given world quadrant.
        /// </summary>
        /// <param name="worldQuadrant">The world quadrant.</param>
        /// <returns></returns>
        public bool Remove(Point worldQuadrant)
        {
            return _dictionary.Remove(worldQuadrant);
        }

        /// <summary>
        /// Returns true if a chunk exists at given world quadrant.
        /// </summary>
        /// <param name="worldQuadrant">The world quadrant.</param>
        /// <returns>
        ///   <c>true</c> if the specified world quadrant contains key; otherwise, <c>false</c>.
        /// </returns>
        public bool ContainsKey(Point worldQuadrant)
        {
            return _dictionary.ContainsKey(worldQuadrant);
        }

        /// <summary>
        /// Returns total count of chunk stored.
        /// </summary>
        /// <value>
        /// The count.
        /// </value>
        public int Count
        {
            get { return _dictionary.Count; }
        }

        /// <summary>
        /// Returns an enumarable list of keys stored.
        /// </summary>
        /// <value>
        /// The keys.
        /// </value>
        public IEnumerable<Point> Keys
        {
            get { return _dictionary.Keys; }
        }

        /// <summary>
        /// Returns an enumarable list of chunks stored.
        /// </summary>
        /// <value>
        /// The values.
        /// </value>
        public IEnumerable<Chunk> Values
        {
            get { return _dictionary.Values; }
        }
    }
}
