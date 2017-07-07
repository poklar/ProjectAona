using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using ProjectAona.Engine.Chunk;
using ProjectAona.Engine.Core;
using ProjectAona.Engine.Graphics;
using ProjectAona.Engine.Tiles;
using ProjectAona.Engine.UserInterface;
using ProjectAona.Engine.World.NPC;
using ProjectAona.Engine.World.Selection;
using System.Collections.Generic;

namespace ProjectAona.Engine.Input
{
    public class MouseManager
    {
        private static Camera _camera;
        private static ChunkManager _chunkManager;

        private static MouseState _previousMouseState;

        /// <summary>
        /// Initializes a new instance of the <see cref="MouseManager"/> class.
        /// </summary>
        /// <param name="camera">The camera.</param>
        /// <param name="chunkManager">The chunk manager.</param>
        public MouseManager(Camera camera, ChunkManager chunkManager)
        {
            _camera = camera;
            _chunkManager = chunkManager;
            _previousMouseState = Mouse.GetState();
        }

        /// <summary>
        /// Updates the specified game time.
        /// </summary>
        /// <param name="gameTime">The game time.</param>
        public void Update(GameTime gameTime)
        {
            MouseState currentMouseState = Mouse.GetState();
            
            UpdateMouseClicks(currentMouseState);

            _previousMouseState = currentMouseState;
        }

        /// <summary>
        /// Keeping up to date with the mouse clicks.
        /// </summary>
        /// <param name="currentMouseState">State of the current mouse.</param>
        public static void UpdateMouseClicks(MouseState currentMouseState)
        {
            if (currentMouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Released)
            {
                // Only display info when not selecting/building stuff 
                if (!IsMouseOverMenu() && GameState.State == GameStateType.PLAYING)
                {
                    SelectionInfo selection = SelectedTileInfo(currentMouseState);

                    if (selection != null)
                        PlayingStateInterface.SelectedTileInfo(selection);
                }
            }
        }

        /// <summary>
        /// Gets the world mouse position.
        /// </summary>
        /// <returns></returns>
        public static Vector2 GetWorldMousePosition()
        {
            MouseState mouseState = Mouse.GetState();

            return Vector2.Transform(new Vector2(mouseState.X, mouseState.Y), Matrix.Invert(_camera.View));
        }

        /// <summary>
        /// Determines whether [is mouse over menu].
        /// </summary>
        /// <returns>
        ///   <c>true</c> if [is mouse over menu]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsMouseOverMenu()
        {
            return PlayingStateInterface.IsMouseOverMenu();
        }

        /// <summary>
        /// Gets info about the selected tile.
        /// </summary>
        /// <param name="currentMouseState">State of the current mouse.</param>
        /// <returns></returns>
        public static SelectionInfo SelectedTileInfo(MouseState currentMouseState)
        {
            Tile tile = _chunkManager.TileAtWorldPosition(currentMouseState.X, currentMouseState.Y);

            if (tile != null)
            {
                SelectionInfo selectionInfo = new SelectionInfo();
                selectionInfo.Entities = new List<ISelectableInterface>();

                selectionInfo.Tile = tile;

                if (tile.Minions.Count != 0)
                    foreach (Minion minion in tile.Minions)
                        selectionInfo.Entities.Add(minion);

                if (tile.Flora != null)
                    selectionInfo.Entities.Add(tile.Flora);

                if (tile.Wall != null)
                    selectionInfo.Entities.Add(tile.Wall);

                return selectionInfo;
            }

            return null;
        }
    } 

    public class SelectionInfo
    {
        public Tile Tile;
        public List<ISelectableInterface> Entities;
    }
}
