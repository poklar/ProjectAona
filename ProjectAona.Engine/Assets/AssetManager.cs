using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

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
                StoneTestTexture = Game.Content.Load<Texture2D>("Textures\\stoneTex"); 

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
