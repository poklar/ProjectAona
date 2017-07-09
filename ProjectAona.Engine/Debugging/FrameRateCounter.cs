using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProjectAona.Engine.Assets;
using System;

namespace ProjectAona.Engine.Debugging
{
    public class FrameRateCounter
    {
        private AssetManager _assetManager;

        private SpriteBatch _spriteBatch;
        private SpriteFont _spriteFont;
        private string[] _numbers;

        private int _frameRate = 0;
        private int _frameCounter = 0;
        private TimeSpan _elapsedTime = TimeSpan.Zero;

        /// <summary>
        /// Constructor initializes the numbers array for garbage free strings later.
        /// </summary>
        /// <param name="game">The game instance.</param>
        public FrameRateCounter(AssetManager assetManager, SpriteBatch spriteBatch)
        {
            _assetManager =  assetManager;
            _spriteBatch = spriteBatch;
            _numbers = new string[10];
            for (int j = 0; j < 10; j++)
            {
                _numbers[j] = j.ToString();
            }

            _spriteFont = _assetManager.DefaultFont;
        }

        /// <summary>
        /// The framerate is calculated in this method.  It actually calculates
        /// the update rate, but when fixed time step and syncronize with retrace 
        /// are turned off, it is the same value.
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            _elapsedTime += gameTime.ElapsedGameTime;

            if (_elapsedTime > TimeSpan.FromSeconds(1))
            {
                _elapsedTime -= TimeSpan.FromSeconds(1);
                _frameRate = _frameCounter;
                _frameCounter = 0;
            }
        }

        /// <summary>
        /// Draws the framerate to screen with a shadow outline to make it easy
        /// to see in any game.
        /// </summary>
        /// <param name="gameTime"></param>
        public void Draw(GameTime gameTime)
        {
            _frameCounter++;

            //Framerates over 1000 aren't important as we have lots of room for features.
            if (_frameRate >= 1000)
            {
                _frameRate = 999;
            }

            //Break the framerate down to single digit components so we can use
            //the number lookup to draw them.
            int fps1 = _frameRate / 100;
            int fps2 = (_frameRate - fps1 * 100) / 10;
            int fps3 = _frameRate - fps1 * 100 - fps2 * 10;

            _spriteBatch.Begin();

            _spriteBatch.DrawString(_spriteFont, _numbers[fps1], new Vector2(33, 33), Color.Black);
            _spriteBatch.DrawString(_spriteFont, _numbers[fps1], new Vector2(32, 32), Color.White);

            _spriteBatch.DrawString(_spriteFont, _numbers[fps2], new Vector2(33 + _spriteFont.MeasureString(_numbers[fps1]).X, 33), Color.Black);
            _spriteBatch.DrawString(_spriteFont, _numbers[fps2], new Vector2(32 + _spriteFont.MeasureString(_numbers[fps1]).X, 32), Color.White);

            _spriteBatch.DrawString(_spriteFont, _numbers[fps3], new Vector2(33 + _spriteFont.MeasureString(_numbers[fps1]).X + _spriteFont.MeasureString(_numbers[fps2]).X, 33), Color.Black);
            _spriteBatch.DrawString(_spriteFont, _numbers[fps3], new Vector2(32 + _spriteFont.MeasureString(_numbers[fps1]).X + _spriteFont.MeasureString(_numbers[fps2]).X, 32), Color.White);

            _spriteBatch.End();
        }
    }
}
