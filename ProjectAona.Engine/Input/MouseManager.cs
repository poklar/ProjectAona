using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using ProjectAona.Engine.Chunks;
using ProjectAona.Engine.Core;
using ProjectAona.Engine.Graphics;
using ProjectAona.Engine.Tiles;
using ProjectAona.Engine.UserInterface;
using ProjectAona.Engine.World;
using ProjectAona.Engine.World.NPC;
using ProjectAona.Engine.World.Selection;
using System.Collections.Generic;
using System.Linq;

namespace ProjectAona.Engine.Input
{
    public class MouseManager
    {
        private static Camera _camera;

        private static MouseState _previousMouseState;

        private static SelectionInfo _playerSelection;

        /// <summary>
        /// Initializes a new instance of the <see cref="MouseManager"/> class.
        /// </summary>
        /// <param name="camera">The camera.</param>
        public MouseManager(Camera camera)
        {
            _camera = camera;
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
                    SelectionInfo selection = SelectedTileInfo(GetWorldMousePosition());

                    if (selection != null)
                        PlayingStateInterface.SelectedTileInfo(selection);
                    else
                        _playerSelection = null;

                }
            }

            if (currentMouseState.RightButton == ButtonState.Pressed && _previousMouseState.RightButton == ButtonState.Released)
            {
                if (!IsMouseOverMenu() && GameState.State == GameStateType.PLAYING)
                {
                    if (_playerSelection != null)
                    {
                        if (_playerSelection.Entities.Count != 0)
                        {
                            // Create an array where all items should fit in
                            Minion[] minions = new Minion[_playerSelection.Entities.Count];
                            int count = 0;

                            foreach (ISelectableInterface item in _playerSelection.Entities)
                            {
                                // Check if the item is a minion
                                if (item.GetType() == typeof(Minion))
                                {
                                    minions[count] = (Minion)item;
                                    count++;
                                }                                
                            }

                            // Check if minions were added to the array
                            if (count > 0)
                            {
                                Vector2 position = GetWorldMousePosition();
                                Tile tile = ChunkManager.TileAtWorldPosition((int)position.X, (int)position.Y);

                                if (tile != null)
                                    // Move the last (visible) minion to the clicked tile
                                    NPCManager.MoveTo(minions.ElementAt(count - 1), tile); // TODO: Check if fully visible minion is the last one in the list
                            }
                        }
                    }
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
        /// <param name="mousePosition">Position of the current mouse.</param>
        /// <returns></returns>
        public static SelectionInfo SelectedTileInfo(Vector2 mousePosition)
        {
            Tile tile = ChunkManager.TileAtWorldPosition((int)mousePosition.X, (int)mousePosition.Y);

            if (tile != null)
            {
                SelectionInfo selection = new SelectionInfo();
                selection.Entities = new List<ISelectableInterface>();

                selection.Tile = tile;

                if (tile.Minions.Count != 0)
                    foreach (Minion minion in tile.Minions)
                        selection.Entities.Add(minion);

                if (tile.Flora != null)
                    selection.Entities.Add(tile.Flora);

                if (tile.Wall != null)
                    selection.Entities.Add(tile.Wall);

                return _playerSelection = selection;
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
