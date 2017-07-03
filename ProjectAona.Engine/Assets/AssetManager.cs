using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProjectAona.Engine.Core;
using System;
using System.Collections.Generic;

namespace ProjectAona.Engine.Assets
{
    /// <summary>
    /// The asset manager.
    /// </summary>
    public class AssetManager
    {
        /// <summary>
        /// Gets the bush texture.
        /// </summary>
        /// <value>
        /// The bush.
        /// </value>
        public Texture2D Bush { get; private set; }

        /// <summary>
        /// Gets the maple tree texture.
        /// </summary>
        /// <value>
        /// The maple tree.
        /// </value>
        public Texture2D MapleTree { get; private set; }

        /// <summary>
        /// Gets the oak tree texture.
        /// </summary>
        /// <value>
        /// The oak tree.
        /// </value>
        public Texture2D OakTree { get; private set; }

        /// <summary>
        /// Gets the light grass tile texture.
        /// </summary>
        /// <value>
        /// The light grass tile.
        /// </value>
        public Texture2D LightGrassTile { get; private set; }

        /// <summary>
        /// Gets the dark grass tile texture.
        /// </summary>
        /// <value>
        /// The dark grass tile.
        /// </value>
        public Texture2D DarkGrassTile { get; private set; }

        /// <summary>
        /// Gets the stone tile texture.
        /// </summary>
        /// <value>
        /// The stone tile.
        /// </value>
        public Texture2D StoneTile { get; private set; }

        /// <summary>
        /// Gets the water tile texture.
        /// </summary>
        /// <value>
        /// The water tile.
        /// </value>
        public Texture2D WaterTile { get; private set; }
        
        /// <summary>
        /// Gets the default font.
        /// </summary>
        /// <value>
        /// The default font.
        /// </value>
        public SpriteFont DefaultFont { get; private set; }

        /// <summary>
        /// Gets the walls and selections texture atlas.
        /// </summary>
        /// <value>
        /// The walls and selections texture atlas.
        /// </value>
        public Texture2D WallsSelectionsTextureAtlas { get; private set; }

        /// <summary>
        /// Gets the walls and selection texture atlas XML.
        /// </summary>
        /// <value>
        /// The walls and selections texture atlas XML.
        /// </value>
        public Dictionary<string, Rectangle> WallsSelectionsTextureAtlasXML { get; private set; }

        private Game _game;

        /// <summary>
        /// Initializes a new instance of the <see cref="AssetManager"/> class.
        /// </summary>
        /// <param name="game">The game.</param>
        public AssetManager(Game game)
        {
            _game = game;
        }

        /// <summary>
        /// Initializes the asset manager.
        /// </summary>
        public void Initialize()
        {

        }

        /// <summary>
        /// Loads the content.
        /// </summary>
        public void LoadContent()
        {
            // Set the content
            try
            {
                // Terrain
                MapleTree = _game.Content.Load<Texture2D>("Textures\\Terrain\\mapleTree");
                OakTree = _game.Content.Load<Texture2D>("Textures\\Terrain\\oakTree");
                Bush = _game.Content.Load<Texture2D>("Textures\\Terrain\\bush");
                WallsSelectionsTextureAtlas = _game.Content.Load<Texture2D>("Textures\\Terrain\\wallsSelections");

                // Tiles
                LightGrassTile = _game.Content.Load<Texture2D>("Textures\\Tiles\\grass1Tile");
                DarkGrassTile = _game.Content.Load<Texture2D>("Textures\\Tiles\\grass2Tile");
                StoneTile = _game.Content.Load<Texture2D>("Textures\\Tiles\\stoneTile");
                WaterTile = _game.Content.Load<Texture2D>("Textures\\Tiles\\waterTile");

                // Font
                DefaultFont = _game.Content.Load<SpriteFont>("Fonts\\DefaultFont");

                // Xml
                WallsSelectionsTextureAtlasXML = _game.Content.Load<Dictionary<string, Rectangle>>("Xml\\TextureAtlas\\wallsSelections");
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString(), "Error while loading assets!");
                Environment.Exit(-1);
            }
        }
    }
}
