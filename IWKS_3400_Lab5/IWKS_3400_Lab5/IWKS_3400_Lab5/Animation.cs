using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

// Credit: Youtuber - CodingMadeEasy //
namespace IWKS_3400_Lab5
{
    class Animation
    {
        public Texture2D image { get; set; }   // Image of whole sprite sheet //
        public int frameCounter { get; set; }   // Timer to next frame //
        public int switchFrame { get; set; }    // Time interval decided to switch to next frame //
        public int frameWidth { get { return image.Width / (int)amountOfFrames.X; } }
        public int frameHeight { get { return image.Height / (int)amountOfFrames.Y; } }
        public bool active { get; set; }   // Checks if player is moving or standing still //
       
        public Vector2 position { get; set; }
        public Vector2 velocity { get; set; }
        public Vector2 amountOfFrames { get; set; }     // Number of frames per sheet (X and Y direction) //
        public Vector2 currentFrame;       // X and Y of current frame displayed on screen //
        public Rectangle sourceRect { get; set; }    // Crops out a certain sprite frame to draw on screen //

        public void Initialize(Vector2 newPos, Vector2 frames)
        {
            active = false;
            switchFrame = 100;   // Time interval between frames // Higher = slower change //
            position = newPos;
            amountOfFrames = frames;
        }

        public void Update (GameTime gameTime)
        {
            if (active)     // When player is active or moving //
            {
                // Counts to certain number then change the sprite frame //
                // Regulates time between each frame for smooth animation // 
                frameCounter += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
            }
            else
                frameCounter = 0;   // Remains zero when player not active //

            // When reached time to switch to next frame // Moving across sprite sheet only (X direction) //
            if (frameCounter >= switchFrame)
            {
                frameCounter = 0;   // Reset for next frame //
                currentFrame.X += frameWidth;   // Move current frame to next frame //

                if (currentFrame.X >= image.Width)  // When reach end of sprite sheet //
                    currentFrame.X = 0;   // Reset current frame back to the first //
            }
            // Rectangle = new Rectangle (Position X, Position Y, Rectangle Width, Rectangle Height) //
            sourceRect = new Rectangle((int)currentFrame.X, (int)currentFrame.Y * frameHeight, frameWidth, frameHeight);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // Draw (Texture2D, Vector2, sourceRectangle, Color) //
            spriteBatch.Draw(image, position, sourceRect, Color.White);
        }
    }
}
