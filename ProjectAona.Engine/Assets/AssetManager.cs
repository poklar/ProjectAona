using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace ProjectAona.Engine.Assets
{
    public interface IAssetManager
    {
        Texture2D TestTexture { get; }
    }

    public class AssetManager : GameComponent, IAssetManager
    {
        public Texture2D TestTexture { get; private set; }

        public SpriteFont DefaultFont { get; private set; }

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
            try
            {
                TestTexture = Game.Content.Load<Texture2D>("Textures\\stoneTex");

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
