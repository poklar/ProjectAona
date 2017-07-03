using Microsoft.Xna.Framework;
using ProjectAona.Engine.Assets;

namespace ProjectAona.Test.UserInterface
{
    public interface IBuildInterface
    {

    }

    public class BuildInterface : DrawableGameComponent, IBuildInterface
    {
        private IAssetManager _assetManager;

        public BuildInterface(Game game)
            : base(game)
        {
            // Export service
            Game.Services.AddService(typeof(IBuildInterface), this);
        }

        public override void Initialize()
        {
            // Get the services
            _assetManager = (IAssetManager)Game.Services.GetService(typeof(IAssetManager));
            
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {

            base.Draw(gameTime);
        }
    }
}
