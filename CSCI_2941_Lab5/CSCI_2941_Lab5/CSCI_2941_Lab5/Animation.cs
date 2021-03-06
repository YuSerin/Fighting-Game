﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CSCI_2941_Lab5
{
    class Animation
    {
        public Texture2D[] playerImg = new Texture2D[(int)Sprite.Max];
        public Vector2 playerPos;
        public Vector2 playerVelocity;
        public Vector2 currentFrame;
        public int State;
        //public bool hasJumped = false;
        //public float gravity = 80f;
        //public bool active;
        //public bool hasAttacked;

        public bool flipHorizontal = false;
        public int nextFrameTime = 60;
        public int frameTimer = 0;
        public bool holdFrame = false;
        private Vector2[] FrameSize = new Vector2[(int)Sprite.Max];
        private Rectangle sourceRect;

        public void Initialize(Vector2 newPos, Vector2[] newFrameSize)
        {
            playerPos = newPos;
            FrameSize = newFrameSize;
            playerVelocity = Vector2.Zero;
            //active = false;
        }

        public void Update(GameTime gameTime, bool stateChanged, bool looping, bool freeze)
        {
            if (stateChanged)
                currentFrame = new Vector2(0, 0);

            if (!freeze)
            {
                frameTimer += (int)gameTime.ElapsedGameTime.TotalMilliseconds;

                if (frameTimer >= nextFrameTime)
                {
                    frameTimer = 0;     // Reset Frame Time //

                    currentFrame = new Vector2(currentFrame.X + FrameSize[State].X, 0);

                    if (currentFrame.X >= playerImg[State].Width && looping == true)
                    {
                        currentFrame = new Vector2(0, 0);     // Loop back to first frame //
                    }
                    else if (holdFrame && currentFrame.X == FrameSize[State].X * 4)
                    {
                        currentFrame = new Vector2(FrameSize[State].X * 3, 0);
                        //if (Keyboard.GetState().IsKeyUp(Keys.S))
                        //    holdFrame = false;
                    }
                    else if (currentFrame.X >= playerImg[State].Width && looping == false)
                        return;
                }
                sourceRect = new Rectangle((int)currentFrame.X, (int)currentFrame.Y,
                    (int)FrameSize[State].X, (int)FrameSize[State].Y);
            }
            else
            {
                this.FreezeFrame(gameTime);
            }

            
        }

        public void FreezeFrame(GameTime gameTime)
        {
            frameTimer += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
            //playerPos += playerVelocity;

            if (frameTimer >= nextFrameTime)
            {
                frameTimer = 0;     // Reset Frame Time //

                if (currentFrame.X < (playerImg[State].Width - FrameSize[State].X))
                {
                    sourceRect = new Rectangle((int)currentFrame.X, (int)currentFrame.Y,
                    (int)FrameSize[State].X, (int)FrameSize[State].Y);
                    currentFrame = new Vector2(currentFrame.X + FrameSize[State].X, 0);
                }
                else
                {
                    sourceRect = new Rectangle((int)currentFrame.X, (int)currentFrame.Y,
                    (int)FrameSize[State].X, (int)FrameSize[State].Y);
                }   
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (!flipHorizontal)
                spriteBatch.Draw(playerImg[State], playerPos, sourceRect, Color.White,
                    0f, Vector2.Zero, 2f, SpriteEffects.None, 0f);
            else
                spriteBatch.Draw(playerImg[State], playerPos, sourceRect, Color.White,
                    0f, Vector2.Zero, 2f, SpriteEffects.FlipHorizontally, 0f);
        }
    }
}
