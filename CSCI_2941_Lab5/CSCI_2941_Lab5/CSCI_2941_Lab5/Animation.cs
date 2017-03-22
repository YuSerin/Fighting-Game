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

        public bool flipHorizontal = false;
        public int nextFrameTime = 60;
        public int frameTimer = 0;
        private Vector2[] FrameSize = new Vector2[(int)Sprite.Max];
        private Rectangle sourceRect;

        public void Initialize(Vector2 newPos, Vector2[] newFrameSize)
        {
            playerPos = newPos;
            FrameSize = newFrameSize;
            //active = false;
        }

        public void Update(GameTime gameTime, bool stateChanged, bool looping)
        {
            frameTimer += (int)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (stateChanged)
                currentFrame = new Vector2(0, 0);

            if (frameTimer >= nextFrameTime)
            {
                frameTimer = 0;     // Reset Frame Time //
                currentFrame = new Vector2(currentFrame.X + FrameSize[State].X, 0);

                if (currentFrame.X >= playerImg[State].Width && looping == true)
                {
                    currentFrame = new Vector2(0, 0);     // Loop back to first frame //
                }
                else if (currentFrame.X >= playerImg[State].Width && looping == false)
                    return;
            }
            sourceRect = new Rectangle((int)currentFrame.X, (int)currentFrame.Y, 
                (int)FrameSize[State].X, (int)FrameSize[State].Y);
        }
        
        public void Draw(SpriteBatch spriteBatch)
        {
            if (!flipHorizontal)
                spriteBatch.Draw(playerImg[State], playerPos, sourceRect, Color.White);
            else
                spriteBatch.Draw(playerImg[State], playerPos, sourceRect, Color.White,
                    0f, new Vector2(0, 0), 1f, SpriteEffects.FlipHorizontally, 0f);
        }
    }
}
