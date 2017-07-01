using Microsoft.Xna.Framework;
using ProjectAona.Engine.Common.Noise;
using ProjectAona.Engine.Tiles;
using ProjectAona.Engine.World;
using ProjectAona.Engine.World.TerrainObjects;

namespace ProjectAona.Engine.Chunk.Generators
{
    /// <summary>
    /// Simple terrain generator.
    /// </summary>
    public class SimpleTerrain : GameComponent, ITerrainGenerator
    {
        private ITerrainManager _terrainManager;

        public SimpleTerrain(Game game)
            : base(game)
        {
            // Export service
            Game.Services.AddService(typeof(ITerrainGenerator), this);
        }

        public override void Initialize()
        {
            // Get service
            _terrainManager = (ITerrainManager)Game.Services.GetService(typeof(ITerrainManager));

            base.Initialize();
        }

        /// <summary>
        /// Builds the chunk.
        /// </summary>
        /// <param name="chunk">The chunk.</param>
        public void BuildChunk(Chunk chunk)
        {
            // Get the seed
            int worldSeed = Core.Engine.Instance.Configuration.World.Seed;
            // Incremental steps for the loop, xStep and yStep are the same, but maybe in the future the width/height ratio will differ
            int xStep = chunk.WidthInPixels / chunk.WidthInTiles;
            int yStep = chunk.HeightInPixels / chunk.HeightInTiles;

            // Loop through the chunk in steps of 32 so we don't call GenerateTerrain 512 times 
            for (int x = 0; x < chunk.WidthInPixels; x += xStep)
            {
                // Create a number with the worldseed included
                int worldX = (((int)chunk.Position.X + x) / 16 ) + worldSeed;
                
                for (int y = 0; y < chunk.HeightInPixels; y += yStep)
                {
                    // Create a number
                    int worldY = ((int)chunk.Position.Y + y) / 16;
                    
                    // Generate terrain by passing the chunk, the selected tile and the created numbers
                    GenerateTerrain(chunk, x / xStep, y / yStep, worldX, worldY);
                }
            }
        }

        /// <summary>
        /// Generates the terrain.
        /// </summary>
        /// <param name="chunk">The chunk.</param>
        /// <param name="tileXInChunk">The tile x-coordinate in chunk.</param>
        /// <param name="tileYInChunk">The tile y-coordinate in chunk.</param>
        /// <param name="worldX">The world x.</param>
        /// <param name="worldY">The world y.</param>
        private void GenerateTerrain(Chunk chunk, int tileXInChunk, int tileYInChunk, int worldX, int worldY)
        {
            // Do some magic 
            float octave1 = SimplexNoise.noise(worldX * 0.0001f, worldY * 0.0001f) * 0.5f;
            float octave2 = SimplexNoise.noise(worldX * 0.0005f, worldY * 0.0005f) * 0.25f;
            float octave3 = SimplexNoise.noise(worldX * 0.005f, worldY * 0.005f) * 0.12f;
            float octave4 = SimplexNoise.noise(worldX * 0.01f, worldY * 0.01f) * 0.12f;
            float octave5 = SimplexNoise.noise(worldX * 0.03f, worldY * 0.03f) * octave4;
            float noise = octave1 + octave2 + octave3 + octave4 + octave5;

            // TODO: For now I'm using color's as tile types, this will change.

            // None generated tile is grass
            TileType tileType = TileType.LightGrass;

            Tile tile = chunk.Tiles[tileXInChunk, tileYInChunk];
   
            // If below this threshold
            if (noise < 0.45f && noise > 0.391f)
            {
                // Create some flora
                CreateFlora(tile, worldX, worldY);

                tileType = CreateGrassTypes(worldX, worldY);
            }
            else if (noise < 0.4504 && noise > 0.45f)
            {
                // TODO: Create water differently
                tileType = TileType.Water;
            }
            else if (noise < 0.50f && noise > 0.4504f)
            {
                // Create some flora
                CreateFlora(tile, worldX, worldY);

                // Set to grass type
                tileType = CreateGrassTypes(worldX, worldY);
            }
            else if (noise < 0.6f)
            {
                // Create some caves/minerals
                CreateCaves(tile, worldX, worldY);
                
                // Set to stone type
                tileType = TileType.Stone;
            }

            // Set the chunks tile type
            chunk.SetTile(tileXInChunk, tileYInChunk, tileType);
        }

        /// <summary>
        /// Creates the grass types.
        /// </summary>
        /// <param name="worldX">The world x.</param>
        /// <param name="worldY">The world y.</param>
        /// <returns></returns>
        private TileType CreateGrassTypes(int worldX, int worldY)
        {
            // Do some magic
            float noise = NoiseValue(worldX, worldY);

            // If below the threshold, ...
            if (noise < 0.83f && noise > 0.73f || noise < 0.95f && noise > 0.88f)
            {
                return TileType.DarkGrass;
            }
            else
                return TileType.LightGrass;
        }

        /// <summary>
        /// Creates the flora.
        /// </summary>
        /// <param name="worldX">The world x.</param>
        /// <param name="worldY">The world y.</param>
        /// <returns></returns>
        private void CreateFlora(Tile tile, int worldX, int worldY)
        {
            // Do some magic
            float noise = NoiseValue(worldX, worldY);
            
            // If below the threshold, create trees or bushes
            if (noise < 0.75 && noise > 0.70f)
            {
                _terrainManager.AddFlora(FloraType.OakTree, tile);
            }
            else if (noise < 0.81 && noise > 0.80f)
            {
                _terrainManager.AddFlora(FloraType.RasberryBush, tile);
            }
            else if (noise < 0.97 && noise > 0.94f)
            {
                _terrainManager.AddFlora(FloraType.PoplarTree, tile);
            }
        }

        /// <summary>
        /// Creates the caves and minerals.
        /// </summary>
        /// <param name="worldX">The world x.</param>
        /// <param name="worldY">The world y.</param>
        /// <returns></returns>
        private void CreateCaves(Tile tile, int worldX, int worldY)
        {
            // Do some magic
            float octave4 = SimplexNoise.noise(worldX * 0.01f, worldY * 0.01f) * 0.12f;
            float octave5 = SimplexNoise.noise(worldX * 0.03f, worldY * 0.03f) * octave4;
            float noise = octave4 + octave5;

            // If below a certain threshold, create minerals 
            if (noise < 0.04 && noise > 0.03f)
            {
                _terrainManager.AddWall(WallType.IronOre, tile);
            }
            else if (noise < 0.045 && noise > 0.04f)
            {
                _terrainManager.AddWall(WallType.Cave, tile);
            }
            else if (noise < 0.055 && noise > 0.045f)
            {
                _terrainManager.AddWall(WallType.StoneOre, tile);
            }
            else if (noise < 0.065 && noise > 0.055f)
            {
                _terrainManager.AddWall(WallType.CoalOre, tile);
            }
            else if (noise < 0.09 && noise > 0.065f)
            {
                _terrainManager.AddWall(WallType.Cave, tile);
            }
            else if (noise < 0.13 && noise > 0.09f)
            {
                _terrainManager.AddWall(WallType.StoneOre, tile);
            }
            else if (noise < 0.15 && noise > 0.13f)
            {
                _terrainManager.AddWall(WallType.Cave, tile);
            }
        }

        /// <summary>
        /// Creates a noise value.
        /// </summary>
        /// <param name="worldX">The world x.</param>
        /// <param name="worldY">The world y.</param>
        /// <returns></returns>
        private float NoiseValue(int worldX, int worldY)
        {
            // Do some magic
            float octave4 = SimplexNoise.noise(worldX * 0.49f, worldY * 0.49f) * 0.92f;
            float octave5 = SimplexNoise.noise(worldX * 0.88f, worldY * 0.88f) * octave4;
            return octave4 + octave5;
        }
    }
}
