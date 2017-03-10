using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CSCI_2941_Lab5
{
    class Animation
    {
        public Texture2D[] playerImg = new Texture2D[(int)Sprite.Max];
        public Vector2 playerPos;
        public Vector2 currentFrame;
        public int State;
        //public bool active;
        //public bool hasAttacked;

        private Rectangle sourceRect;
        private int nextFrameTime = 60;
        private int frameTimer = 0;
        private Vector2[] FrameSize = new Vector2[(int)Sprite.Max];

        public void Initialize(Vector2 newPos, Vector2[] newFrameSize)
        {
            playerPos = newPos;
            FrameSize = newFrameSize;
            //active = false;
        }

        public void Update(GameTime gameTime)
        {
           // if (currentState != State)
               // currentFrame = new Vector2(0, 0);   // Reset current frame //

            frameTimer += (int)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (frameTimer >= nextFrameTime)
            {
                frameTimer = 0;     // Reset Frame Time //
                currentFrame = new Vector2(currentFrame.X + FrameSize[State].X, 0);

                if (currentFrame.X >= playerImg[State].Width)
                    currentFrame = new Vector2 (0, 0);     // Loop back to first frame //
            }
            sourceRect = new Rectangle((int)currentFrame.X, (int)currentFrame.Y, 
                (int)FrameSize[State].X, (int)FrameSize[State].Y);
        }
        public void playThrough(GameTime gameTime)
        {
            frameTimer += (int)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (frameTimer >= nextFrameTime)
            {
                frameTimer = 0;     // Reset Frame Time //
                currentFrame = new Vector2(currentFrame.X + FrameSize[State].X, 0);
            }
            sourceRect = new Rectangle((int)currentFrame.X, (int)currentFrame.Y,
                (int)FrameSize[State].X, (int)FrameSize[State].Y);
        }
        
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(playerImg[State], playerPos, sourceRect, Color.White);
        }
    }
}
