using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.TextureAtlases;
using ProjectAona.Engine.Assets;
using ProjectAona.Engine.Chunk;
using ProjectAona.Engine.Common;
using ProjectAona.Engine.Graphics;
using ProjectAona.Engine.Tiles;
using ProjectAona.Engine.UserInterface;
using ProjectAona.Engine.World.TerrainObjects;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ProjectAona.Engine.World
{
    /// <summary>
    /// The terrain manager. 
    /// </summary>
    public class TerrainManager
    {
        /// <summary>
        /// The flora objects. The key will be the position.
        /// </summary>
        private static Dictionary<Vector2, Flora> _floraObjects;

        /// <summary>
        /// The wall objects. The key will be the position.
        /// </summary>
        private static Dictionary<Vector2, Wall> _wallObjects;

        /// <summary>
        /// The wall texture.
        /// </summary>
        private TextureAtlas _wallsSelections;

        /// <summary>
        /// The sprite batch.
        /// </summary>
        private SpriteBatch _spriteBatch;

        /// <summary>
        /// The asset manager.
        /// </summary>
        private AssetManager _assetManager;

        /// <summary>
        /// The chunk manager.
        /// </summary>
        private ChunkManager _chunkManager;

        /// <summary>
        /// The camera.
        /// </summary>
        private Camera _camera;

        public TerrainManager(SpriteBatch spriteBatch, Camera camera, AssetManager assetManager, ChunkManager chunkManager)
        {
            // Setters
            _spriteBatch = spriteBatch;
            _camera = camera;
            _assetManager = assetManager;
            _chunkManager = chunkManager;

            _floraObjects = new Dictionary<Vector2, Flora>();
            _wallObjects = new Dictionary<Vector2, Wall>();
        }
        
        /// <summary>
        /// Loads the content.
        /// </summary>
        public void LoadContent()
        {
            _wallsSelections = new TextureAtlas("wallsSelectionsTextureAtlas", _assetManager.WallsSelectionsTextureAtlas, _assetManager.WallsSelectionsTextureAtlasXML);
        }

        /// <summary>
        /// Draws the specified game time.
        /// </summary>
        /// <param name="gameTime">The game time.</param>
        public void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, _camera.View);

            DrawTerrainObjects();

            _spriteBatch.End();
        }

        /// <summary>
        /// Draws the terrain objects.
        /// </summary>
        private void DrawTerrainObjects()
        {
            // Get all the currently loaded chunks
            var visibleChunks = _chunkManager.VisibleChunks();

            // For each loaded chunks
            foreach (var chunk in visibleChunks)
            {
                // Loop through each tile
                for (int x = 0; x < chunk.WidthInTiles; x++)
                {
                    for (int y = 0; y < chunk.HeightInTiles; y++)
                    {
                        // Get the position from the tile
                        Vector2 position = chunk.TileAt(x, y).Position;

                        // If a flora object is positioned on the same as the tile
                        if (_floraObjects.ContainsKey(position))
                        {
                            // Get that flora object
                            Flora flora = _floraObjects[position];

                            // And draw it 
                            _spriteBatch.Draw(FloraTexture(flora.FloraType), flora.Position, Color.White);
                        }

                        // If a wall object is positioned on the same as the tile
                        if (_wallObjects.ContainsKey(position))
                        {
                            // Get that wall object
                            Wall wall = _wallObjects[position];
                            
                            // Select the correct wall sprite
                            TextureRegion2D wallTexture = SelectWallSprite(wall);

                            // And draw it
                            _spriteBatch.Draw(wallTexture.Texture, wall.Position, wallTexture.Bounds, Color.White);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Adds a flora.
        /// </summary>
        /// <param name="floraType">The flora type.</param>
        /// <param name="tile">The tile where the flora will be positioned on.</param>
        public static void AddFlora(FloraType floraType, Tile tile)
        {
            // If the tile already hasn't an object and if the flora object doesn't already exists
            if (!tile.IsOccupied && !_floraObjects.ContainsKey(tile.Position))
            {
                // Create a new flora object and set it to the tile's position and pass the flora type
                Flora flora = new Flora(tile.Position, floraType);

                // Tile is occupied now
                tile.IsOccupied = true;
                // Pass the flora object to the tile
                tile.Flora = flora;

                // Add the flora object to the dictionary 
                _floraObjects.Add(tile.Position, flora);                   
            }
        }

        /// <summary>
        /// Adds a wall.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="tile">The tile where the wall will be positioned on.</param>
        public static void AddWall(LinkedSpriteType type, Tile tile)
        {
            // If the tile already hasn't an object and if the wall object doesn't already exists
            if (!tile.IsOccupied && !_wallObjects.ContainsKey(tile.Position))
            {
                // Create a new wall object and set it to the tile's position and pass the wall type
                Wall wall = new Wall(tile.Position, type);

                // Tile is occupied now
                tile.IsOccupied = true;
                // Pass the wall object to the tile
                tile.Wall = wall;

                // Add the wall object to the dictionary 
                _wallObjects.Add(tile.Position, wall);
            }
        }

        /// <summary>
        /// Removes a flora.
        /// </summary>
        /// <param name="flora">The flora.</param>
        /// <param name="tile">The tile.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void RemoveFlora(Tile tile)
        {
            if (tile.IsOccupied && _floraObjects.ContainsKey(tile.Position))
            {
                tile.Flora = null;
                tile.IsOccupied = false;
                _floraObjects.Remove(tile.Position);
            }
        }

        /// <summary>
        /// Removes a wall.
        /// </summary>
        /// <param name="wall">The wall.</param>
        /// <param name="tile">The tile.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void RemoveWall(Tile tile)
        {
            if (tile.IsOccupied && _wallObjects.ContainsKey(tile.Position))
            {
                tile.Wall = null;
                tile.IsOccupied = false;
                _wallObjects.Remove(tile.Position);
            }
        }

        // TODO: Might want to make it more general later on
        /// <summary>
        /// Determines whether the tile is occupied by a wall].
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns>
        ///   <c>true</c> if [is tile occupied by wall]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsTileOccupiedByWall(int x, int y)
        {
            Tile tile = _chunkManager.TileAtWorldPosition(x, y);

            Wall wall = null;

            if (_wallObjects.ContainsKey(tile.Position))
                wall = _wallObjects[tile.Position];

            if (tile != null && wall != null)
            {
                if (tile.IsOccupied && tile.Wall != null && tile.Wall == wall)
                    return true;
            }

            return false;
        }

        public bool IsTileOccupiedByOre(int x, int y)
        {
            Tile tile = _chunkManager.TileAtWorldPosition(x, y);

            Wall wall = null;

            if (_wallObjects.ContainsKey(tile.Position))
                wall = _wallObjects[tile.Position];

            if (tile != null &&  wall != null)
            {
                if (tile.IsOccupied && tile.Wall != null && tile.Wall == wall)
                {
                    if (wall.Type == LinkedSpriteType.Cave || wall.Type == LinkedSpriteType.CoalOre ||
                        wall.Type == LinkedSpriteType.StoneOre || wall.Type == LinkedSpriteType.IronOre)
                        return true;
                }
            }

            return false;
        }
        
        /// <summary>
        /// Select the wall sprite from the texture atlas.
        /// </summary>
        /// <param name="linkedSprite">The object.</param>
        /// <returns></returns>
        // TODO: A wall should be notified if it is linked
        // TODO: This function looks horrible, I should look into autotiling/bitmasking
        private TextureRegion2D SelectWallSprite(LinkedSprite linkedSprite)
        {
            // Get the wall type and turn it into a string while adding "_" to it. Example: IronOre_
            string wallName = linkedSprite.Type + "_";

            // First the function starts by checking the north, west, south and east (clockwise) tile if it is occupied by a wall
            // For example if there's a wall north to the tile, it will add "N" to the string
            // If the wall has a certain name, the northwest, southwest, southeast and/or northeast corner needs to be checked
            // Each sprite has a unique name, for examle if the wall has a north/south/east wall then the added string will be "NSE"

            bool visible = true;

            // Get the position from the wall
            Vector2 position = linkedSprite.Position;

            // Create tile object
            Tile tile;

            // Get the tile NORTH from the wall
            tile = _chunkManager.TileAtWorldPosition((int)position.X, (int)position.Y - 32);
            // If the tile exists and the tile is occupied by a wall
            if (tile != null && tile.IsOccupied && tile.Wall != null)
            {
                // Add N(orth) to the string name
                wallName += "N";
            }

            // Get the tile WEST from the wall
            tile = _chunkManager.TileAtWorldPosition((int)position.X + 32, (int)position.Y);
            if (tile != null && tile.IsOccupied && tile.Wall != null)
            {
                // Add W(est) to the string name
                wallName += "W";
            }

            // Get the tile SOUTH from the wall
            tile = _chunkManager.TileAtWorldPosition((int)position.X, (int)position.Y + 32);
            if (tile != null && tile.IsOccupied && tile.Wall != null)
            {
                // Add S(outh) to the string name
                wallName += "S";
            }

            // Get the tile EAST from the wall
            tile = _chunkManager.TileAtWorldPosition((int)position.X - 32, (int)position.Y);
            if (tile != null && tile.IsOccupied && tile.Wall != null)
            {
                // Add E(ast) to the string name
                wallName += "E";
            }

            // The wall is surrounded by north/west/south/east walls, check the diagonally walls to get the corresponding wall sprite
            if (wallName == (linkedSprite.Type.ToString() + "_NWSE"))
            {
                // Add _ to the string
                wallName += "_";

                // Get the tile NORTHWEST from the wall
                tile = _chunkManager.TileAtWorldPosition((int)position.X + 32, (int)position.Y - 32);
                if (tile != null && tile.IsOccupied && tile.Wall != null)
                {
                    // Add N(orth)W(est) to the string name
                    wallName += "NW";
                }

                // Get the tile SOUTHWEST from the wall
                tile = _chunkManager.TileAtWorldPosition((int)position.X + 32, (int)position.Y + 32);
                if (tile != null && tile.IsOccupied && tile.Wall != null)
                {
                    // Add S(outh)W(est) to the string name
                    wallName += "SW";
                }

                // Get the tile SOUTHEAST from the wall
                tile = _chunkManager.TileAtWorldPosition((int)position.X - 32, (int)position.Y + 32);
                if (tile != null && tile.IsOccupied && tile.Wall != null)
                {
                    // Add S(outh)E(ast) to the string name
                    wallName += "SE";
                }

                // Get the tile NORTHEAST from the wall
                tile = _chunkManager.TileAtWorldPosition((int)position.X - 32, (int)position.Y - 32);
                if (tile != null && tile.IsOccupied && tile.Wall != null)
                {
                    // Add N(orth)E(ast) to the string name
                    wallName += "NE";
                }

                if (linkedSprite.Type == LinkedSpriteType.Cave || linkedSprite.Type == LinkedSpriteType.CoalOre || linkedSprite.Type == LinkedSpriteType.IronOre || linkedSprite.Type == LinkedSpriteType.StoneOre)
                {
                    Wall wall = _chunkManager.TileAtWorldPosition((int)position.X, (int)position.Y).Wall;
                    wall.Visible = visible = false;
                }
            }
            // The wall is surrounded by west/south/east walls, check the diagonally walls to get the corresponding wall sprite
            else if (wallName == (linkedSprite.Type.ToString() + "_WSE"))
            {
                wallName += "_";

                // Get the tile SOUTHWEST from the wall
                tile = _chunkManager.TileAtWorldPosition((int)position.X + 32, (int)position.Y + 32);
                if (tile != null && tile.IsOccupied && tile.Wall != null)
                {
                    wallName += "SW";
                }

                // Get the tile SOUTHEAST from the wall
                tile = _chunkManager.TileAtWorldPosition((int)position.X - 32, (int)position.Y + 32);
                if (tile != null && tile.IsOccupied && tile.Wall != null)
                {
                    wallName += "SE";
                }
                
            }
            // The wall is surrounded by north/west/east walls, check the diagonally walls to get the corresponding wall sprite
            else if (wallName == (linkedSprite.Type.ToString() + "_NWE"))
            {
                wallName += "_";

                // Get the tile NORTHWEST from the wall
                tile = _chunkManager.TileAtWorldPosition((int)position.X + 32, (int)position.Y - 32);
                if (tile != null && tile.IsOccupied && tile.Wall != null)
                {
                    wallName += "NW";
                }

                // Get the tile NORTHEAST from the wall
                tile = _chunkManager.TileAtWorldPosition((int)position.X - 32, (int)position.Y - 32);
                if (tile != null && tile.IsOccupied && tile.Wall != null)
                {
                    wallName += "NE";
                }
            }
            // The wall is surrounded by north/west/south walls, check the diagonally walls to get the corresponding wall sprite
            else if (wallName == (linkedSprite.Type.ToString() + "_NWS"))
            {
                wallName += "_";

                // Get the tile NORTHWEST from the wall
                tile = _chunkManager.TileAtWorldPosition((int)position.X + 32, (int)position.Y - 32);
                if (tile != null && tile.IsOccupied && tile.Wall != null)
                {
                    wallName += "NW";
                }

                // Get the tile SOUTHWEST from the wall
                tile = _chunkManager.TileAtWorldPosition((int)position.X + 32, (int)position.Y + 32);
                if (tile != null && tile.IsOccupied && tile.Wall != null)
                {
                    wallName += "SW";
                }                
            }
            // The wall is surrounded by north/south/east walls, check the diagonally walls to get the corresponding wall sprite
            else if (wallName == (linkedSprite.Type.ToString() + "_NSE"))
            {
                wallName += "_";

                // Get the tile SOUTHEAST from the wall
                tile = _chunkManager.TileAtWorldPosition((int)position.X - 32, (int)position.Y + 32);
                if (tile != null && tile.IsOccupied && tile.Wall != null)
                {
                    wallName += "SE";
                }

                // Get the tile NORTHEAST from the wall
                tile = _chunkManager.TileAtWorldPosition((int)position.X - 32, (int)position.Y - 32);
                if (tile != null && tile.IsOccupied && tile.Wall != null)
                {
                    wallName += "NE";
                }                
            }
            // The wall is surrounded by south/east walls, check the diagonally walls to get the corresponding wall sprite
            else if (wallName == (linkedSprite.Type.ToString() + "_SE"))
            {
                // Get the tile SOUTHEAST from the wall
                tile = _chunkManager.TileAtWorldPosition((int)position.X - 32, (int)position.Y + 32);
                if (tile != null && tile.IsOccupied && tile.Wall != null)
                {
                    wallName += "_SE";
                }
            }
            // The wall is surrounded by north/east walls, check the diagonally walls to get the corresponding wall sprite
            else if (wallName == (linkedSprite.Type.ToString() + "_NE"))
            {
                // Get the tile NORTHEAST from the wall
                tile = _chunkManager.TileAtWorldPosition((int)position.X - 32, (int)position.Y - 32);
                if (tile != null && tile.IsOccupied && tile.Wall != null)
                {
                    wallName += "_NE";
                }
            }
            // The wall is surrounded by north/west walls, check the diagonally walls to get the corresponding wall sprite
            else if (wallName == (linkedSprite.Type.ToString() + "_NW"))
            {
                // Get the tile NORTHWEST from the wall
                tile = _chunkManager.TileAtWorldPosition((int)position.X + 32, (int)position.Y - 32);
                if (tile != null && tile.IsOccupied && tile.Wall != null)
                {
                    wallName += "_NW";
                }
            }
            // The wall is surrounded by west/south walls, check the diagonally walls to get the corresponding wall sprite
            else if (wallName == (linkedSprite.Type.ToString() + "_WS"))
            {
                // Get the tile SOUTHWEST from the wall
                tile = _chunkManager.TileAtWorldPosition((int)position.X + 32, (int)position.Y + 32);
                if (tile != null && tile.IsOccupied && tile.Wall != null)
                {
                    wallName += "_SW";
                }
            }

            // For example, the wallName has now a string name like: IronOre_NWSE_NWSENE
            // In the texture atlas is a sprite that corresponds to that name 

            // Check if wallName has changed and if neighbor is till set
            if (wallName == (linkedSprite.Type.ToString() + "_") && linkedSprite.HasNeighbor)
                    // Set to false since this wall doesn't have any neighbor's
                    linkedSprite.HasNeighbor = false;

            if (visible)
                // Get the corresponding wall sprite from the texture atlas
                return _wallsSelections[wallName];
            else
                return _wallsSelections["Cave_Invisible"];
        }

        /// <summary>
        /// Gets the texture from the flora type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        // TODO: REMOVE, a texture atlas will be added later
        private Texture2D FloraTexture(FloraType type)
        {
            // Get the corresponding texture
            switch (type)
            {
                case FloraType.OakTree: return _assetManager.MapleTree;
                case FloraType.PoplarTree: return _assetManager.OakTree;
                case FloraType.RasberryBush: return _assetManager.Bush;
                default: return null;
            }
        }
    }
}
