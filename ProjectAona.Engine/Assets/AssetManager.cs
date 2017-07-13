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
        public Texture2D LightGrassTile { get; private set; }

        public Texture2D DarkGrassTile { get; private set; }

        public Texture2D StoneTile { get; private set; }

        public Texture2D WaterTile { get; private set; }
        
        public SpriteFont DefaultFont { get; private set; }

        public Texture2D WallsSelectionsTextureAtlas { get; private set; }

        public Dictionary<string, Rectangle> WallsSelectionsTextureAtlasXML { get; private set; }

        public Texture2D MenuItems { get; private set; }

        public Dictionary<string, Rectangle> MenuItemsTextureAtlasXML { get; private set; }

        public SpriteFont InGameFont { get; private set; }
        
        public Texture2D NPCNormal { get; private set; }

        public Texture2D NPCRanger { get; private set; }

        public Texture2D NPCBusy { get; private set; }

        public Texture2D PathPixel { get; private set; }

        public Texture2D TerrainObjects { get; private set; }

        public Dictionary<string, Rectangle> TerrainObjectsTextureAtlasXML { get; private set; }

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
            LoadContent();
        }

        /// <summary>
        /// Loads the content.
        /// </summary>
        private void LoadContent()
        {
            // Set the content
            try
            {
                // Terrain
                WallsSelectionsTextureAtlas = _game.Content.Load<Texture2D>("Textures\\Terrain\\wallsSelections");
                TerrainObjects = _game.Content.Load<Texture2D>("Textures\\Terrain\\terrainObjects");

                // Tiles
                LightGrassTile = _game.Content.Load<Texture2D>("Textures\\Tiles\\grass1Tile");
                DarkGrassTile = _game.Content.Load<Texture2D>("Textures\\Tiles\\grass2Tile");
                StoneTile = _game.Content.Load<Texture2D>("Textures\\Tiles\\stoneTile");
                WaterTile = _game.Content.Load<Texture2D>("Textures\\Tiles\\waterTile");

                // Font
                DefaultFont = _game.Content.Load<SpriteFont>("Fonts\\DefaultFont");
                InGameFont = _game.Content.Load<SpriteFont>("Fonts\\InGameFont");

                // Xml
                WallsSelectionsTextureAtlasXML = _game.Content.Load<Dictionary<string, Rectangle>>("Xml\\TextureAtlas\\wallsSelections");
                MenuItemsTextureAtlasXML = _game.Content.Load<Dictionary<string, Rectangle>>("Xml\\TextureAtlas\\menuItems");
                TerrainObjectsTextureAtlasXML = _game.Content.Load<Dictionary<string, Rectangle>>("Xml\\TextureAtlas\\terrainObjects");

                // User Interface
                MenuItems = _game.Content.Load<Texture2D>("Textures\\UserInterface\\menuItems");

                // NPC
                NPCNormal = _game.Content.Load<Texture2D>("Textures\\NPCs\\NPC_normal");
                NPCBusy = _game.Content.Load<Texture2D>("Textures\\NPCs\\NPC_busy");
                PathPixel = _game.Content.Load<Texture2D>("Textures\\NPCs\\pathPixel");
                NPCRanger = _game.Content.Load<Texture2D>("Textures\\NPCs\\NPCRanger");
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString(), "Error while loading assets!");
                Environment.Exit(-1);
            }
        }
    }
}
