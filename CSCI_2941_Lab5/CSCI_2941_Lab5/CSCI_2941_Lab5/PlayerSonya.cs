using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace CSCI_2941_Lab5
{
    enum Sprite
    {
        Idle, Walk_R, Walk_L, Crouch, Mid_Punch, Max,
    }
    class PlayerSonya
    {
        Texture2D[] playerSprite = new Texture2D[(int)Sprite.Max];
        Animation playerAnimation = new Animation();
        Vector2[] FrameSize = new Vector2[(int)Sprite.Max];
        Vector2 playerPosition = new Vector2(100f, 400f);
        float moveSpeed = 70f;
        bool looping = true;
        public void Initialize()
        {
            FrameSize[(int)Sprite.Idle] = new Vector2(69f, 128f);
            FrameSize[(int)Sprite.Walk_R] = new Vector2(80f, 136f);
            FrameSize[(int)Sprite.Walk_L] = new Vector2(80f, 136f);
            FrameSize[(int)Sprite.Crouch] = new Vector2(74f, 114f);
            FrameSize[(int)Sprite.Mid_Punch] = new Vector2(109f, 123f);


            playerAnimation.Initialize(playerPosition, FrameSize);
        }
        public void LoadContent (ContentManager Content)
        {
            playerSprite[(int)Sprite.Idle] = Content.Load<Texture2D>("Textures/Idle");
            playerSprite[(int)Sprite.Walk_R] = Content.Load<Texture2D>("Textures/Walk-Right");
            playerSprite[(int)Sprite.Walk_L] = Content.Load<Texture2D>("Textures/Walk-Left");
            playerSprite[(int)Sprite.Crouch] = Content.Load<Texture2D>("Textures/Crouch");
            playerSprite[(int)Sprite.Mid_Punch] = Content.Load<Texture2D>("Textures/Mid-Punch");

            playerAnimation.playerImg = playerSprite;
        }
        public void Update(GameTime gameTime)
        {
            if (looping == false)
            {
                playerAnimation.playThrough(gameTime);

                if (playerAnimation.currentFrame.X >= playerAnimation.playerImg[playerAnimation.State].Width)
                {
                    looping = true;
                    playerAnimation.currentFrame = new Vector2(0, 0);
                }
            }
            else
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Right))
                {
                    playerAnimation.State = (int)Sprite.Walk_R;
                    //playerAnimation.currentState = playerAnimation.State;
                    playerAnimation.playerPos.X += moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.Left))
                {
                    playerAnimation.State = (int)Sprite.Walk_L;
                    playerAnimation.playerPos.X -= moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.Down))
                {
                    playerAnimation.State = (int)Sprite.Crouch;
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.M))
                {
                    looping = false;
                    playerAnimation.State = (int)Sprite.Mid_Punch;
                }
                else
                {
                    // playerAnimation.active = false;
                    playerAnimation.State = (int)Sprite.Idle;
                    //playerAnimation.currentState = (int)Sprite.Idle;
                }

                playerAnimation.Update(gameTime);
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            playerAnimation.Draw(spriteBatch);
        }
        public void Dispose()
        {
            for (int i = 0; i < (int)Sprite.Max; i++)
                playerSprite[i].Dispose();
        }
    }
}
