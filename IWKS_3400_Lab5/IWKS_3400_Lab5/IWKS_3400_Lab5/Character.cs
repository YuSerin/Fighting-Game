using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// Credit: Youtuber - Oyyou //
namespace IWKS_3400_Lab5
{
    class Character
    {
        // Character: //
        Texture2D charImage;
        Vector2 position;
        Vector2 velocity;

        // Sprite sheet frames: //
        Vector2 origin;     // The center of each frame //
        int currentFrame;
        int frameHeight;    
        int frameWidth;
        Rectangle rectangle;    // Rectangle of certain frame //

        // Time inbetween frames: //
        float timer = 0;
        float interval = 60;    // Time between frames // Higher value = slower change of frame //

        int startFrameNumR;    // The start frame number of sprite moving right // Remeber first frame = 0 //
        int startFrameNumL;      // The start frame number of sprite moving left //
        int RFrameNum;      // Number of frames moving sprite to right //
        int LFrameNum;      // Number of frames moving sprite to left //

        // Initialize //
        public Character(Texture2D newTexture, Vector2 newPosition,int newFrameHeight, int newFrameWidth, 
            int newStartFrameR, int newRFrameNum, int newStartFrameL, int newLFrameNum)
        {
            texture = newTexture;
            position = newPosition;
            frameHeight = newFrameHeight;
            frameWidth = newFrameWidth;
            startFrameNumR = newStartFrameR;
            startFrameNumL = newStartFrameL;
            RFrameNum = newRFrameNum;
            LFrameNum = newLFrameNum;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // SpriteBatch.Draw (Texture, Position, Source Rectangle, Color, Rotation, Origin, Scale, Effects, Layer) //
            spriteBatch.Draw(texture, position, rectangle, Color.White, 0f, origin, 1.0f, SpriteEffects.None, 0);
        }

        public void Update(GameTime gameTime)
        {
            // Rectangle = new Rectangle (Position X, Position Y, Rectangle Width, Rectangle Height) //
            // CurrentFrame changes as sprite moves : 0 - n frames //
            // CurrentFrame multiplied to move rectangle X position to next frame of sprite sheet //
            rectangle = new Rectangle(currentFrame * frameWidth, 0, frameWidth, frameHeight);
            origin = new Vector2(rectangle.Width / 2, rectangle.Height / 2);    // Center of rectangle of frame //
            position = position + velocity;     // Sprite moving //

            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                MoveRight(gameTime);
                velocity.X = 3;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                MoveLeft(gameTime);
                velocity.X = -3;
            }
            else
                velocity = Vector2.Zero;    // No movement //
        }

        public void MoveRight(GameTime gameTime)
        {
            // Timer = the total time between updates (in msec) / 2 //
            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds / 2;

            if (timer > interval)
            {
                currentFrame++;     // Go to next frame //
                timer = 0;      // Reset timer //

                // TO DO: Changes depending on sprite sheet //
                if (currentFrame > (startFrameNumR + RFrameNum - 1))     // If goes over max number of right frames //
                {
                    currentFrame = startFrameNumR;   // Resets to first frame of right sprite //
                }
            }
        }

        public void MoveLeft(GameTime gameTime)
        {
            // Timer = the total time between updates (in msec) / 2 //
            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds / 2;

            // If timer is greater than the time inbetween frames (interval already set) //
            if (timer > interval)
            {
                currentFrame++;     // Go to next frame //
                timer = 0;      // Reset timer //
                
                // TO DO: Changes depending on sprite sheet //
                if (currentFrame > (startFrameNumL + LFrameNum - 1))     
                {
                    currentFrame = startFrameNumL;  // Resets to first frame of left sprite //
                }
            }
        }
    } // END: class //
} // END: namespace //
