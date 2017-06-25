﻿using Microsoft.Xna.Framework;
using ProjectAona.Engine.Tiles;

namespace ProjectAona.Engine.Chunk.Generators
{
    /// <summary>
    /// Defines a class which can generate a map of type T.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ITestTerrain<T>
    {
        T BuildChunk(Point worldQuadrant);
    }

    public class TestTerrain : ITestTerrain<Chunk>
    {
        public int WidthInTiles { get; set; }
        public int HeightInTiles { get; set; }
        public int TileSizeInPixels { get; set; }

        public TestTerrain(int widthInTiles, int heightInTiles, int tileSizeInPixels)
        {
            WidthInTiles = widthInTiles;
            HeightInTiles = heightInTiles;
            TileSizeInPixels = tileSizeInPixels;
        }

        public Chunk BuildChunk(Point worldQuadrant)
        {
            Chunk chunk = new Chunk(WidthInTiles, HeightInTiles, worldQuadrant, TileSizeInPixels);
            foreach (Tile tile in chunk.Tiles)
            {
                tile.TileType = TileType.Stone;
            }

            return chunk;
        }
    }
}