using ProjectAona.Engine.Common.Noise;
using ProjectAona.Engine.Tiles;

namespace ProjectAona.Engine.Chunk.Generators
{
    /// <summary>
    /// Simple terrain generator.
    /// </summary>
    public class SimpleTerrain
    {
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
            // TODO: Also Cave/Minerals/Flora are not tiles, so that needs to be changed.

            // None generated tile is grass
            TileType tileType = TileType.Grass;
   
            // If below this threshold
            if (noise < 0.45f && noise > 0.391f)
            {
                // Create some flora
                tileType = CreateFlora(worldX, worldY);

                // If the tile isn't set
                if (tileType == TileType.None)
                    // Set to marsh type
                    tileType = TileType.Marsh;
            }
            else if (noise < 0.4504 && noise > 0.45f)
            {
                // TODO: Create water differently
                tileType = TileType.Water;
            }
            else if (noise < 0.50f && noise > 0.4504f)
            {
                // Create some flora
                tileType = CreateFlora(worldX, worldY);

                // If the tile isn't set
                if (tileType == TileType.None)
                    // Set to grass type
                    tileType = TileType.Grass;
            }
            else if (noise < 0.6f)
            {
                // Create some caves/minerals
                tileType = CreateCaves(worldX, worldY);

                // If the tile isn't set
                if (tileType == TileType.None)
                    // Set to stone type
                    tileType = TileType.Stone;
            }

            // Set the chunks tile type
            chunk.SetTile(tileXInChunk, tileYInChunk, tileType);
        }

        /// <summary>
        /// Creates the flora.
        /// </summary>
        /// <param name="worldX">The world x.</param>
        /// <param name="worldY">The world y.</param>
        /// <returns></returns>
        private TileType CreateFlora(int worldX, int worldY)
        {
            // Do some magic
            float octave4 = SimplexNoise.noise(worldX * 0.49f, worldY * 0.49f) * 0.92f;
            float octave5 = SimplexNoise.noise(worldX * 0.88f, worldY * 0.88f) * octave4;
            float noise = octave4 + octave5;
            
            // If below the threshold, create trees or bushes
            if (noise < 0.72 && noise > 0.70f)
            {
                return TileType.Tree2;
            }
            else if (noise < 0.81 && noise > 0.80f)
            {
                return TileType.Bush;
            }
            else if (noise < 0.97 && noise > 0.94f)
            {
                return TileType.Tree;
            }

            // Return none if nothing was chosen
            return TileType.None;
        }

        /// <summary>
        /// Creates the caves and minerals.
        /// </summary>
        /// <param name="worldX">The world x.</param>
        /// <param name="worldY">The world y.</param>
        /// <returns></returns>
        private TileType CreateCaves(int worldX, int worldY)
        {
            // Do some magic
            float octave4 = SimplexNoise.noise(worldX * 0.01f, worldY * 0.01f) * 0.12f;
            float octave5 = SimplexNoise.noise(worldX * 0.03f, worldY * 0.03f) * octave4;
            float noise = octave4 + octave5;

            // If below a certain threshold, create minerals 
            if (noise < 0.04 && noise > 0.03f)
            {
                return TileType.IronOre;
            }
            else if (noise < 0.045 && noise > 0.04f)
            {
                return TileType.Cave;
            }
            else if (noise < 0.055 && noise > 0.045f)
            {
                return TileType.StoneOre;
            }
            else if (noise < 0.065 && noise > 0.055f)
            {
                return TileType.Coal;
            }
            else if (noise < 0.09 && noise > 0.065f)
            {
                return TileType.Cave;
            }
            else if (noise < 0.13 && noise > 0.09f)
            {
                return TileType.StoneOre;
            }
            else if (noise < 0.15 && noise > 0.13f)
            {
                return TileType.Cave;
            }

            // Return none if nothing was chosen
            return TileType.None;
        }
    }
}
