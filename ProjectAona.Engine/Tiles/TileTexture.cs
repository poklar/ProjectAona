using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProjectAona.Engine.Assets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectAona.Engine.Tiles
{
    /// <summary>
    /// Class that holds the tile textures.
    /// </summary>
    public interface ITileTexture
    {
        /// <summary>
        /// Gets the tile textures.
        /// </summary>
        /// <value>
        /// The tile textures.
        /// </value>
        Dictionary<TileType, Texture2D> TileTextures { get; }
    }

    /// <summary>
    /// Class that holds the tile textures.
    /// </summary>
    /// <seealso cref="Microsoft.Xna.Framework.GameComponent" />
    /// <seealso cref="ProjectAona.Engine.Tiles.ITileTexture" />
    public class TileTexture : GameComponent, ITileTexture
    {
        /// <summary>
        /// The asset manager.
        /// </summary>
        private IAssetManager _assetManager;

        /// <summary>
        /// Gets the tile textures.
        /// </summary>
        /// <value>
        /// The tile textures.
        /// </value>
        public Dictionary<TileType, Texture2D> TileTextures { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TileTexture"/> class.
        /// </summary>
        /// <param name="game">The game.</param>
        public TileTexture(Game game)
            : base(game)
        {
            Game.Services.AddService(typeof(ITileTexture), this);
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        public override void Initialize()
        {
            _assetManager = (IAssetManager)Game.Services.GetService(typeof(IAssetManager));
            InitializeTileTextures();

            base.Initialize();
        }

        /// <summary>
        /// Sets the tile textures.
        /// </summary>
        private void InitializeTileTextures()
        {
            // Create dictionary
            TileTextures = new Dictionary<TileType, Texture2D>();

            // Add stone texture
            TileTextures.Add(TileType.Stone, _assetManager.StoneTestTexture);
        }
    }
}
