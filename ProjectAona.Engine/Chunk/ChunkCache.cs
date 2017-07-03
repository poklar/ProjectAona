using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProjectAona.Engine.Assets;
using ProjectAona.Engine.Chunk.Generators;
using ProjectAona.Engine.Graphics;
using ProjectAona.Engine.World;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectAona.Engine.Chunk
{
    /// <summary>
    /// The chunk cache.
    /// </summary>
    public class ChunkCache
    {
        /// <summary>
        /// Gets the quadrants currently in memory.
        /// </summary>
        /// <value>
        /// The quadrants currently in memory.
        /// </value>
        public IEnumerable<Point> QuadrantsCurrentlyInMemory { get { return ChunkStorage.Keys; } }

        /// <summary>
        /// Gets the chunk storage.
        /// </summary>
        /// <value>
        /// The chunk storage.
        /// </value>
        public ChunkStorage ChunkStorage { get; private set; }

        /// <summary>
        /// Gets or sets the maximum chunks in memory.
        /// </summary>
        /// <value>
        /// The maximum chunks in memory.
        /// </value>
        public int MaxChunksInMemory { get; set; }

        /// <summary>
        /// The chunk width.
        /// </summary>
        private readonly int _chunkWidth;

        /// <summary>
        /// The chunk height.
        /// </summary>
        private readonly int _chunkHeight;

        /// <summary>
        /// The chunk generator.
        /// </summary>
        private ITerrainGenerator _chunkGenerator;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChunkCache"/> class.
        /// </summary>
        /// <param name="game">The game.</param>
        /// <param name="maxMapsInMemory">The maximum maps in memory.</param>
        public ChunkCache(int maxMapsInMemory = 16)
        {

            // Setters
            MaxChunksInMemory = maxMapsInMemory;
            _chunkWidth = Core.Engine.Instance.Configuration.Chunk.WidthInTiles * 32; // 32 pixels
            _chunkHeight = Core.Engine.Instance.Configuration.Chunk.HeightInTiles * 32; // 32 pixels
            _chunkGenerator = new SimpleTerrain();
            ChunkStorage = new ChunkStorage();
        }

        /// <summary>
        /// Gets the chunk at the world quadrant.
        /// </summary>
        /// <param name="worldQuadrant">The world quadrant.</param>
        /// <returns></returns>
        public Chunk GetChunkAt(Point worldQuadrant)
        {
            // Pre load the chunk
            PreloadChunkAt(worldQuadrant);

            // If the chunk exists in the storage
            if (ChunkStorage.ContainsKey(worldQuadrant))
                // Return the chunk
                return ChunkStorage[worldQuadrant];
            else
                return null;
        }

        /// <summary>
        /// Preloads the chunk at the world quadrant.
        /// </summary>
        /// <param name="worldQuadrant">The world quadrant.</param>
        public void PreloadChunkAt(Point worldQuadrant)
        {
            // If the storage contains the chunk at worldquadrant
            if (!ChunkStorage.ContainsKey(worldQuadrant))
                // Load/create missing chunk
                LoadOrCreateMissingChunk(worldQuadrant);
        }

        /// <summary>
        /// Loads the or create missing chunk.
        /// </summary>
        /// <param name="worldQuadrant">The world quadrant.</param>
        private void LoadOrCreateMissingChunk(Point worldQuadrant)
        {
            // Create chunk object
            Chunk chunk = ChunkManager.ChunkAt(worldQuadrant);

            // If chunk is in world bounds
            if (chunk != null)
            {
                // TODO: LOAD from disk if it exists.
                 
                // Generate the chunk
                _chunkGenerator.BuildChunk(chunk);
                // Store the chunk in the storage
                ChunkStorage[worldQuadrant] = chunk;
            }
        }

        /// <summary>
        /// Unloads the chunk at the world quadrant.
        /// </summary>
        /// <param name="worldQuadrant">The world quadrant.</param>
        public void UnloadChunkAt(Point worldQuadrant)
        {
            // Remove from storage
            ChunkStorage.Remove(worldQuadrant);
        }

        /// <summary>
        /// Gets a list of visible chunks.
        /// </summary>
        /// <param name="viewPort">The view port.</param>
        /// <returns></returns>
        public IEnumerable<Chunk> GetVisibleChunks(Rectangle viewPort)
        {
            // Create a list for visible chunk objects
            List<Chunk> visibleChunks = new List<Chunk>();

            // Get the x-coordinate of the left most chunk
            int leftMostChunk = (int)Math.Floor(viewPort.Left / (float)_chunkWidth);
            // Get the y-coordinate of the top most chunk
            int topMostChunk = (int)Math.Floor(viewPort.Top / (float)_chunkHeight);

            // Get the x-coordinate of the right most chunk
            int rightMostChunk = (int)Math.Floor(viewPort.Right / (float)_chunkWidth);
            // Get the y-coordinate of the bottom most chunk
            int bottomMostChunk = (int)Math.Floor(viewPort.Bottom / (float)_chunkHeight);

            // For every chunk between the left most and right most..
            for (int x = leftMostChunk; x <= rightMostChunk; x++)
            {
                // For every chunk between the top most and bottom most..
                for (int y = topMostChunk; y <= bottomMostChunk; y++)
                {
                    // Get the new chunk from that world quadrant
                    Chunk chunk = GetChunkAt(new Point(x, y));

                    // If it is in world bounds
                    if (chunk != null)
                        // Add the chunk to the visible list
                        visibleChunks.Add(chunk);
                }
            }

            // Load the chunks around the visible ones
            BufferChunksAroundVisible(leftMostChunk, rightMostChunk, topMostChunk, bottomMostChunk);

            // Check if there are too many chunks buffered
            DumpExcessChunksIfAny(viewPort);

            // Return the list
            return visibleChunks;
        }

        /// <summary>
        /// Buffers the chunks around visible chunks.
        /// </summary>
        /// <param name="leftMostChunk">The left most chunk.</param>
        /// <param name="rightMostChunk">The right most chunk.</param>
        /// <param name="topMostChunk">The top most chunk.</param>
        /// <param name="bottomMostChunk">The bottom most chunk.</param>
        private void BufferChunksAroundVisible(int leftMostChunk, int rightMostChunk, int topMostChunk, int bottomMostChunk)
        {
            // Set the borders
            int leftBorderQuadrant = leftMostChunk - 1;
            int rightBorderQuadrant = rightMostChunk + 1;
            int topBorderQuadrant = topMostChunk - 1;
            int bottomBorderQuadrant = bottomMostChunk + 1;

            // For every chunk between the left and right border..
            for (int x = leftBorderQuadrant; x <= rightBorderQuadrant; x++)
            {
                // For every chunk between the top and bottom border..
                for (int y = topBorderQuadrant; y <= bottomBorderQuadrant; y++)
                {
                    // If the condition is met
                    if (x == leftBorderQuadrant || x == rightBorderQuadrant || y == topBorderQuadrant || y == bottomBorderQuadrant)
                    {
                        // Load the chunk
                        PreloadChunkAt(new Point(x, y));
                    }
                }
            }
        }

        /// <summary>
        /// Dumps the excess chunks if any.
        /// </summary>
        /// <param name="viewPort">The view port.</param>
        private void DumpExcessChunksIfAny(Rectangle viewPort)
        {
            // Check if there are more chunks stored than allowed
            if (ChunkStorage.Count > MaxChunksInMemory)
            {
                int xCoordinateOfCenterChunk = (int)Math.Floor(viewPort.Center.X / (float)_chunkWidth);
                int yCoordinateOfCenterChunk = (int)Math.Floor(viewPort.Center.Y / (float)_chunkHeight);

                Point worldCoordinateOFCurrentCenterChunk = new Point(xCoordinateOfCenterChunk, yCoordinateOfCenterChunk);

                // Create a list of the stored chunks, containing only chunks
                List<Chunk> _allChunks = ChunkStorage.Values.ToList();

                // Sort the chunks
                SortChunksDescendingByDistanceToSpecificChunk(_allChunks, worldCoordinateOFCurrentCenterChunk);

                // As long as there are too many chunks stored
                while (ChunkStorage.Count > MaxChunksInMemory)
                {
                    // Remove chunk from storage
                    UnloadChunkAt(_allChunks.Last().WorldQuadrant);

                    // Remove from the list
                    _allChunks.Remove(_allChunks.Last());
                }
            }
        }

        /// <summary>
        /// Sorts the chunks descending by distance to specific chunk.
        /// </summary>
        /// <param name="_allChunks">All chunks.</param>
        /// <param name="worldCoordinateOfCenterChunk">The world coordinate of center chunk.</param>
        private void SortChunksDescendingByDistanceToSpecificChunk(List<Chunk> _allChunks, Point worldCoordinateOfCenterChunk)
        {
            _allChunks.Sort((chunk1, chunk2) =>
            { return DistanceSquared(worldCoordinateOfCenterChunk, chunk1.WorldQuadrant).CompareTo(DistanceSquared(worldCoordinateOfCenterChunk, chunk2.WorldQuadrant)); });
        }

        /// <summary>
        /// Distances the squared.
        /// </summary>
        /// <param name="p1">The p1.</param>
        /// <param name="p2">The p2.</param>
        /// <returns></returns>
        private float DistanceSquared(Point p1, Point p2)
        {
            Point difference = new Point(p1.X - p2.X, p1.Y - p2.Y);
            return (float)(Math.Pow(difference.X, 2) + Math.Pow(difference.Y, 2));
        }
    }
}
