using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

// Textures are from https://opengameart.org or made by me.

namespace ProjectAona.Engine.Assets
{
    /// <summary>
    /// The asset manager.
    /// </summary>
    public interface IAssetManager
    {
        /// <summary>
        /// Gets the stone test texture.
        /// </summary>
        /// <value>
        /// The stone test texture.
        /// </value>
        Texture2D StoneTestTexture { get; }

        /// <summary>
        /// Gets the white test texture.
        /// </summary>
        /// <value>
        /// The white test texture.
        /// </value>
        Texture2D WhiteTestTexture { get; }

        /// <summary>
        /// Gets the iron ore texture.
        /// </summary>
        /// <value>
        /// The iron ore.
        /// </value>
        Texture2D IronOre { get; }

        /// <summary>
        /// Gets the stone ore texture.
        /// </summary>
        /// <value>
        /// The stone ore.
        /// </value>
        Texture2D StoneOre { get; }

        /// <summary>
        /// Gets the cave texture.
        /// </summary>
        /// <value>
        /// The cave.
        /// </value>
        Texture2D Cave { get; }

        /// <summary>
        /// Gets the coal ore texture.
        /// </summary>
        /// <value>
        /// The coal ore.
        /// </value>
        Texture2D CoalOre { get; }

        /// <summary>
        /// Gets the bush texture.
        /// </summary>
        /// <value>
        /// The bush.
        /// </value>
        Texture2D Bush { get; }

        /// <summary>
        /// Gets the maple tree texture.
        /// </summary>
        /// <value>
        /// The maple tree.
        /// </value>
        Texture2D MapleTree { get; }

        /// <summary>
        /// Gets the oak tree texture.
        /// </summary>
        /// <value>
        /// The oak tree.
        /// </value>
        Texture2D OakTree { get; }

        /// <summary>
        /// Gets the light grass tile texture.
        /// </summary>
        /// <value>
        /// The light grass tile.
        /// </value>
        Texture2D LightGrassTile { get; }

        /// <summary>
        /// Gets the dark grass tile texture.
        /// </summary>
        /// <value>
        /// The dark grass tile.
        /// </value>
        Texture2D DarkGrassTile { get; }

        /// <summary>
        /// Gets the stone tile texture.
        /// </summary>
        /// <value>
        /// The stone tile.
        /// </value>
        Texture2D StoneTile { get; }

        /// <summary>
        /// Gets the water tile texture.
        /// </summary>
        /// <value>
        /// The water tile.
        /// </value>
        Texture2D WaterTile { get; }

        /// <summary>
        /// Gets the default font.
        /// </summary>
        /// <value>
        /// The default font.
        /// </value>
        SpriteFont DefaultFont { get; }
    }

    /// <summary>
    /// The asset manager.
    /// </summary>
    /// <seealso cref="Microsoft.Xna.Framework.GameComponent" />
    /// <seealso cref="ProjectAona.Engine.Assets.IAssetManager" />
    public class AssetManager : GameComponent, IAssetManager
    {
        /// <summary>
        /// Gets the stone test texture.
        /// </summary>
        /// <value>
        /// The stone test texture.
        /// </value>
        public Texture2D StoneTestTexture { get; private set; }

        /// <summary>
        /// Gets the white test texture.
        /// </summary>
        /// <value>
        /// The white test texture.
        /// </value>
        public Texture2D WhiteTestTexture { get; private set; }

        /// <summary>
        /// Gets the iron ore texture.
        /// </summary>
        /// <value>
        /// The iron ore.
        /// </value>
        public Texture2D IronOre { get; private set; }

        /// <summary>
        /// Gets the stone ore texture.
        /// </summary>
        /// <value>
        /// The stone ore.
        /// </value>
        public Texture2D StoneOre { get; private set; }

        /// <summary>
        /// Gets the cave texture.
        /// </summary>
        /// <value>
        /// The cave.
        /// </value>
        public Texture2D Cave { get; private set; }

        /// <summary>
        /// Gets the coal ore texture.
        /// </summary>
        /// <value>
        /// The coal ore.
        /// </value>
        public Texture2D CoalOre { get; private set; }

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
        /// Initializes a new instance of the <see cref="AssetManager"/> class.
        /// </summary>
        /// <param name="game">The game.</param>
        public AssetManager(Game game)
            : base(game)
        {
            // Export service
            Game.Services.AddService(typeof(IAssetManager), this);
        }

        /// <summary>
        /// Initializes the asset manager.
        /// </summary>
        public override void Initialize()
        {
            LoadContent();
            base.Initialize();
        }

        /// <summary>
        /// Loads the content.
        /// </summary>
        public void LoadContent()
        {
            // Set the content
            try
            {
                // Test
                StoneTestTexture = Game.Content.Load<Texture2D>("Textures\\stoneTex");
                WhiteTestTexture = Game.Content.Load<Texture2D>("Textures\\whiteTex");

                // Terrain
                Cave = Game.Content.Load<Texture2D>("Textures\\Terrain\\cave");
                CoalOre = Game.Content.Load<Texture2D>("Textures\\Terrain\\coalOre");
                IronOre = Game.Content.Load<Texture2D>("Textures\\Terrain\\ironOre");
                StoneOre = Game.Content.Load<Texture2D>("Textures\\Terrain\\stoneOre");
                MapleTree = Game.Content.Load<Texture2D>("Textures\\Terrain\\mapleTree");
                OakTree = Game.Content.Load<Texture2D>("Textures\\Terrain\\oakTree");
                Bush = Game.Content.Load<Texture2D>("Textures\\Terrain\\bush");

                // Tiles
                LightGrassTile = Game.Content.Load<Texture2D>("Textures\\Tiles\\grass1Tile");
                DarkGrassTile = Game.Content.Load<Texture2D>("Textures\\Tiles\\grass2Tile");
                StoneTile = Game.Content.Load<Texture2D>("Textures\\Tiles\\stoneTile");
                WaterTile = Game.Content.Load<Texture2D>("Textures\\Tiles\\waterTile");

                DefaultFont = Game.Content.Load<SpriteFont>("Fonts\\DefaultFont");
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString(), "Error while loading assets!");
                Environment.Exit(-1);
            }
        }
    }
}
