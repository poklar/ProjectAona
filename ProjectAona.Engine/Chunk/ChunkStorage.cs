using Microsoft.Xna.Framework;
using ProjectAona.Engine.Chunk.Generators;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectAona.Engine.Chunk
{
    public interface IChunkStorage<T>
    {
        ITestTerrain<T> TestTerrain { get; set; }
        
        int MaxChunksInMemory { get; set; }

        IEnumerable<Point> QuadrantsCurrentlyInMemory { get; }

        T GetChunkAt(Point worldQuadrant);

        void PreloadChunkAt(Point worldQuadrant);

        void UnloadChunkAt(Point worldQuadrant);

        IEnumerable<T> GetVisibleChunks(Rectangle viewPort);
    }

    public class ChunkStorage : IChunkStorage<Chunk>
    {
        private Dictionary<Point, Chunk> _chunks = new Dictionary<Point, Chunk>();

        public IEnumerable<Point> QuadrantsCurrentlyInMemory { get { return _chunks.Keys; } }

        public int MaxChunksInMemory { get; set; }

        public ITestTerrain<Chunk> TestTerrain { get; set; }

        private readonly int _chunkWidth, _chunkHeight;

        public ChunkStorage(ITestTerrain<Chunk> testTerrain, int maxMapsInMemory = 16)
        {
            TestTerrain = testTerrain;
            MaxChunksInMemory = maxMapsInMemory;
            _chunkWidth = Core.Engine.Instance.Configuration.Chunk.WidthInTiles * 32; // 32 pixels
            _chunkHeight = Core.Engine.Instance.Configuration.Chunk.HeightInTiles * 32; // 32 pixels
        }

        public Chunk GetChunkAt(Point worldQuadrant)
        {
            PreloadChunkAt(worldQuadrant);
            return _chunks[worldQuadrant];
        }

        public void PreloadChunkAt(Point worldQuadrant)
        {
            if (!_chunks.ContainsKey(worldQuadrant))
                LoadOrCreateMissingChunk(worldQuadrant);
        }

        private void LoadOrCreateMissingChunk(Point worldQuadrant)
        {
            Chunk chunk = null;

            chunk = TestTerrain.BuildChunk(worldQuadrant);

            _chunks.Add(worldQuadrant, chunk);
        }

        public void UnloadChunkAt(Point worldQuadrant)
        {
            _chunks.Remove(worldQuadrant);
        }

        public IEnumerable<Chunk> GetVisibleChunks(Rectangle viewPort)
        {
            List<Chunk> visibleChunks = new List<Chunk>();

            int leftMostChunk = (int)Math.Floor(viewPort.Left / (float)_chunkWidth);
            int topMostChunk = (int)Math.Floor(viewPort.Top / (float)_chunkHeight);

            int rightMostChunk = (int)Math.Floor(viewPort.Right / (float)_chunkWidth);
            int bottomMostChunk = (int)Math.Floor(viewPort.Bottom / (float)_chunkHeight);

            for (int x = leftMostChunk; x <= rightMostChunk; x++)
            {
                for (int y = topMostChunk; y <= bottomMostChunk; y++)
                {
                    visibleChunks.Add(GetChunkAt(new Point(x, y)));
                }
            }

            BufferChunksAroundVisible(leftMostChunk, rightMostChunk, topMostChunk, bottomMostChunk);

            DumpExcessChunksIfAny(viewPort);

            return visibleChunks;
        }

        private void BufferChunksAroundVisible(int leftMostChunk, int rightMostChunk, int topMostChunk, int bottomMostChunk)
        {
            int leftBorderQuadrant = leftMostChunk - 1;
            int rightBorderQuadrant = rightMostChunk + 1;
            int topBorderQuadrant = topMostChunk - 1;
            int bottomBorderQuadrant = bottomMostChunk + 1;

            for (int x = leftBorderQuadrant; x <= rightBorderQuadrant; x++)
            {
                for (int y = topBorderQuadrant; y <= bottomBorderQuadrant; y++)
                {
                    if (x == leftBorderQuadrant || x == rightBorderQuadrant || y == topBorderQuadrant || y == bottomBorderQuadrant)
                    {
                        PreloadChunkAt(new Point(x, y));
                    }
                }
            }
        }

        private void DumpExcessChunksIfAny(Rectangle viewPort)
        {
            if (_chunks.Count > MaxChunksInMemory)
            {
                int xCoordinateOfCenterChunk = (int)Math.Floor(viewPort.Center.X / (float)_chunkWidth);
                int yCoordinateOfCenterChunk = (int)Math.Floor(viewPort.Center.Y / (float)_chunkHeight);

                Point worldCoordinateOFCurrentCenterChunk = new Point(xCoordinateOfCenterChunk, yCoordinateOfCenterChunk);

                List<Chunk> _allChunks = _chunks.Values.ToList();

                SortChunksDescendingByDistanceToSpecificChunk(_allChunks, worldCoordinateOFCurrentCenterChunk);

                while (_chunks.Count > MaxChunksInMemory)
                {
                    UnloadChunkAt(_allChunks.Last().WorldQuadrant);
                    _allChunks.Remove(_allChunks.Last());
                }
            }
        }

        private void SortChunksDescendingByDistanceToSpecificChunk(List<Chunk> _allChunks, Point worldCoordinateOfCenterChunk)
        {
            _allChunks.Sort((chunk1, chunk2) =>
            { return DistanceSquared(worldCoordinateOfCenterChunk, chunk1.WorldQuadrant).CompareTo(DistanceSquared(worldCoordinateOfCenterChunk, chunk2.WorldQuadrant)); });
        }

        private float DistanceSquared(Point p1, Point p2)
        {
            Point difference = new Point(p1.X - p2.X, p1.Y - p2.Y);
            return (float)(Math.Pow(difference.X, 2) + Math.Pow(difference.Y, 2));
        }
    }
}
