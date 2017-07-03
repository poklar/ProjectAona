using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.TextureAtlases;
using ProjectAona.Engine.Assets;
using ProjectAona.Engine.Chunk;
using ProjectAona.Engine.Graphics;
using ProjectAona.Engine.Tiles;
using ProjectAona.Engine.World.TerrainObjects;
using System;
using System.Collections.Generic;

namespace ProjectAona.Engine.World
{
    /// <summary>
    /// The terrain manager interface.
    /// </summary>
    public interface ITerrainManager
    {
        /// <summary>
        /// Gets the flora objects. The key will be the position.
        /// </summary>
        /// <value>
        /// The flora objects.
        /// </value>
        Dictionary<Vector2, Flora> FloraObjects { get; }

        /// <summary>
        /// Gets the wall objects. The key will be the position.
        /// </summary>
        /// <value>
        /// The wall objects.
        /// </value>
        Dictionary<Vector2, Wall> WallObjects { get; }

        /// <summary>
        /// Adds a wall.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="tile">The tile.</param>
        void AddWall(LinkedSpriteType type, Tile tile);

        /// <summary>
        /// Removes a wall.
        /// </summary>
        /// <param name="wall">The wall.</param>
        /// <param name="tile">The tile.</param>
        void RemoveWall(Wall wall, Tile tile);

        /// <summary>
        /// Adds a flora.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="tile">The tile.</param>
        void AddFlora(FloraType type, Tile tile);

        /// <summary>
        /// Removes a flora.
        /// </summary>
        /// <param name="flora">The flora.</param>
        /// <param name="tile">The tile.</param>
        void RemoveFlora(Flora flora, Tile tile);
    }

    /// <summary>
    /// The terrain manager. 
    /// </summary>
    /// <seealso cref="Microsoft.Xna.Framework.DrawableGameComponent" />
    /// <seealso cref="ProjectAona.Engine.World.ITerrainManager" />
    public class TerrainManager : DrawableGameComponent, ITerrainManager
    {
        /// <summary>
        /// Gets the flora objects. The key will be the position.
        /// </summary>
        /// <value>
        /// The flora objects.
        /// </value>
        public Dictionary<Vector2, Flora> FloraObjects { get; private set; }

        /// <summary>
        /// Gets the wall objects. The key will be the position.
        /// </summary>
        /// <value>
        /// The wall objects.
        /// </value>
        public Dictionary<Vector2, Wall> WallObjects { get; private set; }

        /// <summary>
        /// The wall texture.
        /// </summary>
        private TextureAtlas _mineralTexture;

        /// <summary>
        /// The sprite batch.
        /// </summary>
        private SpriteBatch _spriteBatch;

        /// <summary>
        /// The asset manager.
        /// </summary>
        private IAssetManager _assetManager;

        /// <summary>
        /// The chunk manager.
        /// </summary>
        private IChunkManager _chunkManager;

        /// <summary>
        /// The camera.
        /// </summary>
        private ICamera _camera;

        public TerrainManager(Game game, SpriteBatch spriteBatch)
            : base(game)
        {
            // Export service
            Game.Services.AddService(typeof(ITerrainManager), this);

            // Setters
            _spriteBatch = spriteBatch;

            FloraObjects = new Dictionary<Vector2, Flora>();
            WallObjects = new Dictionary<Vector2, Wall>();
        }

        public override void Initialize()
        {
            // Get services
            _assetManager = (IAssetManager)Game.Services.GetService(typeof(IAssetManager));
            _chunkManager = (IChunkManager)Game.Services.GetService(typeof(IChunkManager));
            _camera = (ICamera)Game.Services.GetService(typeof(ICamera));

            base.Initialize();
        }
        /// <summary>
        /// Loads the content.
        /// </summary>
        protected override void LoadContent()
        {
            _mineralTexture = new TextureAtlas("mineralTextureAtlas", _assetManager.WallsSelectionsTextureAtlas, _assetManager.WallsSelectionsTextureAtlasXML);

            base.LoadContent();
        }


        /// <summary>
        /// Draws the specified game time.
        /// </summary>
        /// <param name="gameTime">The game time.</param>
        public override void Draw(GameTime gameTime)
        {
            // Start drawing
            _spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, _camera.View);

            DrawTerrainObjects();

            _spriteBatch.End();

            base.Draw(gameTime);
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
                        Vector2 position = chunk.Tiles[x, y].Position;

                        // If a flora object is positioned on the same as the tile
                        if (FloraObjects.ContainsKey(position))
                        {
                            // Get that flora object
                            Flora flora = FloraObjects[position];

                            // And draw it 
                            _spriteBatch.Draw(FloraTexture(flora.FloraType), flora.Position - _camera.Position, Color.White);
                        }

                        // If a wall object is positioned on the same as the tile
                        if (WallObjects.ContainsKey(position))
                        {
                            // Get that wall object
                            Wall wall = WallObjects[position];
                            
                            // Select the correct wall sprite
                            TextureRegion2D wallTexture = SelectWallSprite(wall);

                            // And draw it
                            _spriteBatch.Draw(wallTexture.Texture, wall.Position - _camera.Position, wallTexture.Bounds, Color.White);
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
        public void AddFlora(FloraType floraType, Tile tile)
        {
            // If the tile already hasn't an object and if the flora object doesn't already exists
            if (!tile.IsOccupied && !FloraObjects.ContainsKey(tile.Position))
            {
                // Create a new flora object and set it to the tile's position and pass the flora type
                Flora flora = new Flora(tile.Position, floraType);

                // Tile is occupied now
                tile.IsOccupied = true;
                // Pass the flora object to the tile
                tile.Flora = flora;

                // Add the flora object to the dictionary 
                FloraObjects.Add(tile.Position, flora);                   
            }
        }

        /// <summary>
        /// Adds a wall.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="tile">The tile where the wall will be positioned on.</param>
        public void AddWall(LinkedSpriteType type, Tile tile)
        {
            // If the tile already hasn't an object and if the wall object doesn't already exists
            if (!tile.IsOccupied && !WallObjects.ContainsKey(tile.Position))
            {
                // Create a new wall object and set it to the tile's position and pass the wall type
                Wall wall = new Wall(tile.Position, type);

                // Tile is occupied now
                tile.IsOccupied = true;
                // Pass the wall object to the tile
                tile.Wall = wall;

                // Add the wall object to the dictionary 
                WallObjects.Add(tile.Position, wall);
            }
        }

        /// <summary>
        /// Removes a flora.
        /// </summary>
        /// <param name="flora">The flora.</param>
        /// <param name="tile">The tile.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void RemoveFlora(Flora flora, Tile tile)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Removes a wall.
        /// </summary>
        /// <param name="wall">The wall.</param>
        /// <param name="tile">The tile.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void RemoveWall(Wall wall, Tile tile)
        {
            throw new NotImplementedException();
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


            // Get the position from the wall
            Vector2 position = linkedSprite.Position;

            // Create tile object
            Tile tile;

            // Get the tile NORTH from the wall
            tile = _chunkManager.TileAt((int)position.X, (int)position.Y - 32);
            // If the tile exists and the tile is occupied by a wall
            if (tile != null && tile.IsOccupied && tile.Wall != null)
            {
                // Add N(orth) to the string name
                wallName += "N";
            }

            // Get the tile WEST from the wall
            tile = _chunkManager.TileAt((int)position.X + 32, (int)position.Y);
            if (tile != null && tile.IsOccupied && tile.Wall != null)
            {
                // Add W(est) to the string name
                wallName += "W";
            }

            // Get the tile SOUTH from the wall
            tile = _chunkManager.TileAt((int)position.X, (int)position.Y + 32);
            if (tile != null && tile.IsOccupied && tile.Wall != null)
            {
                // Add S(outh) to the string name
                wallName += "S";
            }

            // Get the tile EAST from the wall
            tile = _chunkManager.TileAt((int)position.X - 32, (int)position.Y);
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
                tile = _chunkManager.TileAt((int)position.X + 32, (int)position.Y - 32);
                if (tile != null && tile.IsOccupied && tile.Wall != null)
                {
                    // Add N(orth)W(est) to the string name
                    wallName += "NW";
                }

                // Get the tile SOUTHWEST from the wall
                tile = _chunkManager.TileAt((int)position.X + 32, (int)position.Y + 32);
                if (tile != null && tile.IsOccupied && tile.Wall != null)
                {
                    // Add S(outh)W(est) to the string name
                    wallName += "SW";
                }

                // Get the tile SOUTHEAST from the wall
                tile = _chunkManager.TileAt((int)position.X - 32, (int)position.Y + 32);
                if (tile != null && tile.IsOccupied && tile.Wall != null)
                {
                    // Add S(outh)E(ast) to the string name
                    wallName += "SE";
                }

                // Get the tile NORTHEAST from the wall
                tile = _chunkManager.TileAt((int)position.X - 32, (int)position.Y - 32);
                if (tile != null && tile.IsOccupied && tile.Wall != null)
                {
                    // Add N(orth)E(ast) to the string name
                    wallName += "NE";
                }
            }
            // The wall is surrounded by west/south/east walls, check the diagonally walls to get the corresponding wall sprite
            else if (wallName == (linkedSprite.Type.ToString() + "_WSE"))
            {
                wallName += "_";

                // Get the tile SOUTHWEST from the wall
                tile = _chunkManager.TileAt((int)position.X + 32, (int)position.Y + 32);
                if (tile != null && tile.IsOccupied && tile.Wall != null)
                {
                    wallName += "SW";
                }

                // Get the tile SOUTHEAST from the wall
                tile = _chunkManager.TileAt((int)position.X - 32, (int)position.Y + 32);
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
                tile = _chunkManager.TileAt((int)position.X + 32, (int)position.Y - 32);
                if (tile != null && tile.IsOccupied && tile.Wall != null)
                {
                    wallName += "NW";
                }

                // Get the tile NORTHEAST from the wall
                tile = _chunkManager.TileAt((int)position.X - 32, (int)position.Y - 32);
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
                tile = _chunkManager.TileAt((int)position.X + 32, (int)position.Y - 32);
                if (tile != null && tile.IsOccupied && tile.Wall != null)
                {
                    wallName += "NW";
                }

                // Get the tile SOUTHWEST from the wall
                tile = _chunkManager.TileAt((int)position.X + 32, (int)position.Y + 32);
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
                tile = _chunkManager.TileAt((int)position.X - 32, (int)position.Y + 32);
                if (tile != null && tile.IsOccupied && tile.Wall != null)
                {
                    wallName += "SE";
                }

                // Get the tile NORTHEAST from the wall
                tile = _chunkManager.TileAt((int)position.X - 32, (int)position.Y - 32);
                if (tile != null && tile.IsOccupied && tile.Wall != null)
                {
                    wallName += "NE";
                }                
            }
            // The wall is surrounded by south/east walls, check the diagonally walls to get the corresponding wall sprite
            else if (wallName == (linkedSprite.Type.ToString() + "_SE"))
            {
                // Get the tile SOUTHEAST from the wall
                tile = _chunkManager.TileAt((int)position.X - 32, (int)position.Y + 32);
                if (tile != null && tile.IsOccupied && tile.Wall != null)
                {
                    wallName += "_SE";
                }
            }
            // The wall is surrounded by north/east walls, check the diagonally walls to get the corresponding wall sprite
            else if (wallName == (linkedSprite.Type.ToString() + "_NE"))
            {
                // Get the tile NORTHEAST from the wall
                tile = _chunkManager.TileAt((int)position.X - 32, (int)position.Y - 32);
                if (tile != null && tile.IsOccupied && tile.Wall != null)
                {
                    wallName += "_NE";
                }
            }
            // The wall is surrounded by north/west walls, check the diagonally walls to get the corresponding wall sprite
            else if (wallName == (linkedSprite.Type.ToString() + "_NW"))
            {
                // Get the tile NORTHWEST from the wall
                tile = _chunkManager.TileAt((int)position.X + 32, (int)position.Y - 32);
                if (tile != null && tile.IsOccupied && tile.Wall != null)
                {
                    wallName += "_NW";
                }
            }
            // The wall is surrounded by west/south walls, check the diagonally walls to get the corresponding wall sprite
            else if (wallName == (linkedSprite.Type.ToString() + "_WS"))
            {
                // Get the tile SOUTHWEST from the wall
                tile = _chunkManager.TileAt((int)position.X + 32, (int)position.Y + 32);
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

            // Get the corresponding wall sprite from the texture atlas
            return _mineralTexture[wallName];
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
