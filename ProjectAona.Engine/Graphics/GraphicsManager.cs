using Microsoft.Xna.Framework;

namespace ProjectAona.Engine.Graphics
{
    /// <summary>
    /// Screen service for controlling graphics.
    /// </summary>
    public interface IGraphicsManager
    {
        /// <summary>
        /// Gets a value indicating whether [full screen enabled].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [full screen enabled]; otherwise, <c>false</c>.
        /// </value>
        bool FullScreenEnabled { get; }

        /// <summary>
        /// Sets the full screen on or off.
        /// </summary>
        /// <param name="enabled">if set to <c>true</c> [enabled].</param>
        void EnableFullScreen(bool enabled);
    }

    /// <summary>
    /// The graphics manager that controls various graphical aspects.
    /// </summary>
    /// <seealso cref="ProjectAona.Engine.Graphics.IGraphicsManager" />
    public sealed class GraphicsManager : IGraphicsManager
    {
        // Settings
        /// <summary>
        /// Gets a value indicating whether [full screen enabled].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [full screen enabled]; otherwise, <c>false</c>.
        /// </value>
        public bool FullScreenEnabled { get; private set; }

        /// <summary>
        /// The attached game.
        /// </summary>
        private readonly Game _game;
        /// <summary>
        /// The attached graphics device manager.
        /// </summary>
        private readonly GraphicsDeviceManager _graphicsDeviceManager;

        public GraphicsManager(GraphicsDeviceManager graphicsDeviceManager, Game game)
        {
            _game = game;
            _graphicsDeviceManager = graphicsDeviceManager;
            FullScreenEnabled = _graphicsDeviceManager.IsFullScreen = Core.Engine.Instance.Configuration.Graphics.FullScreenEnabled;
            _graphicsDeviceManager.PreferredBackBufferWidth = Core.Engine.Instance.Configuration.Graphics.Width;
            _graphicsDeviceManager.PreferredBackBufferHeight = Core.Engine.Instance.Configuration.Graphics.Height;
            _graphicsDeviceManager.ApplyChanges();
        }

        /// <summary>
        /// Sets the full screen on or off.
        /// </summary>
        /// <param name="enabled">if set to <c>true</c> [enabled].</param>
        public void EnableFullScreen(bool enabled)
        {
            FullScreenEnabled = enabled;
            _graphicsDeviceManager.IsFullScreen = FullScreenEnabled;
            _graphicsDeviceManager.ApplyChanges();
        }
    }
}
