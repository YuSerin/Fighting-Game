using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace CSCI_2941_Lab5
{
    class PlayerSubZero
    {
        Texture2D[] playerSprite = new Texture2D[(int)Sprite.Max];
        Animation playerAnimation = new Animation();
        Vector2[] FrameSize = new Vector2[(int)Sprite.Max];
        Vector2 playerPosition = new Vector2(1050f, 400f);
        float moveSpeed = 300f;
        bool looping = true;
        Keys lastKey;
        bool stateChange;
        public void Initialize()
        {
            FrameSize[(int)Sprite.Idle] = new Vector2(68f, 131f);
            FrameSize[(int)Sprite.Run] = new Vector2(95f, 134f);
            FrameSize[(int)Sprite.Crouch] = new Vector2(70f, 117f);
            FrameSize[(int)Sprite.Mid_Punch] = new Vector2(113f, 134f);
            FrameSize[(int)Sprite.Kick] = new Vector2(126f, 138f);
            FrameSize[(int)Sprite.Block] = new Vector2(61f, 131f);

            playerAnimation.flipHorizontal = true;

            playerAnimation.Initialize(playerPosition, FrameSize);
        }
        public void LoadContent(ContentManager Content)
        {
            playerSprite[(int)Sprite.Idle] = Content.Load<Texture2D>("SubZero/Idle");
            playerSprite[(int)Sprite.Run] = Content.Load<Texture2D>("SubZero/Run");
            playerSprite[(int)Sprite.Crouch] = Content.Load<Texture2D>("SubZero/Crouch");
            playerSprite[(int)Sprite.Mid_Punch] = Content.Load<Texture2D>("SubZero/Mid_Punch");
            playerSprite[(int)Sprite.Kick] = Content.Load<Texture2D>("SubZero/Kick");
            playerSprite[(int)Sprite.Block] = Content.Load<Texture2D>("SubZero/Block");

            playerAnimation.playerImg = playerSprite;
        }
        public void Update(GameTime gameTime)
        {
            if (looping == false)
            {
                playerAnimation.Update(gameTime, stateChange, looping);

                if (playerAnimation.currentFrame.X >= playerAnimation.playerImg[playerAnimation.State].Width)
                {
                    looping = true;
                    playerAnimation.currentFrame = new Vector2(0, 0);
                    playerAnimation.playerPos = playerPosition;
                }
            }

            if (looping)
            {
                // Move Right //
                if (Keyboard.GetState().IsKeyDown(Keys.NumPad6))
                {
                    if (lastKey != Keys.NumPad6)
                        stateChange = true;
                    lastKey = Keys.NumPad6;
                    playerAnimation.State = (int)Sprite.Run;
                    playerAnimation.flipHorizontal = false;
                    //playerAnimation.currentState = playerAnimation.State;
                    playerAnimation.playerPos.X += moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
                // Move Left //
                else if (Keyboard.GetState().IsKeyDown(Keys.NumPad4))
                {
                    if (lastKey != Keys.NumPad4)
                        stateChange = true;
                    lastKey = Keys.NumPad4;
                    playerAnimation.State = (int)Sprite.Run;
                    playerAnimation.flipHorizontal = true;
                    playerAnimation.playerPos.X -= moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
                // Crouch down //
                else if (Keyboard.GetState().IsKeyDown(Keys.NumPad5))
                {
                    if (lastKey != Keys.NumPad5)
                        stateChange = true;
                    lastKey = Keys.NumPad5;
                    playerAnimation.State = (int)Sprite.Crouch;
                    looping = false;
                    playerPosition = playerAnimation.playerPos;
                    if (playerAnimation.flipHorizontal)
                        playerAnimation.playerPos = new Vector2(playerAnimation.playerPos.X, playerAnimation.playerPos.Y + 16);
                    else
                        playerAnimation.playerPos = new Vector2(playerAnimation.playerPos.X, playerAnimation.playerPos.Y + 16);

                }
                // Mid-Punch //
                else if (Keyboard.GetState().IsKeyDown(Keys.OemQuestion))
                {
                    if (lastKey != Keys.OemQuestion)
                        stateChange = true;
                    lastKey = Keys.OemQuestion;
                    playerAnimation.State = (int)Sprite.Mid_Punch;
                    looping = false;
                    playerPosition = playerAnimation.playerPos;
                    if (playerAnimation.flipHorizontal)
                        playerAnimation.playerPos = new Vector2(playerAnimation.playerPos.X - 45, playerAnimation.playerPos.Y - 3);
                    else
                        playerAnimation.playerPos = new Vector2(playerAnimation.playerPos.X, playerAnimation.playerPos.Y - 3);

                }
                // Kick //
                else if (Keyboard.GetState().IsKeyDown(Keys.OemComma))
                {
                    if (lastKey != Keys.OemComma)
                        stateChange = true;
                    lastKey = Keys.OemComma;
                    playerAnimation.State = (int)Sprite.Kick;
                    looping = false;
                    playerPosition = playerAnimation.playerPos;
                    if (playerAnimation.flipHorizontal)
                        playerAnimation.playerPos = new Vector2(playerAnimation.playerPos.X - 60, playerAnimation.playerPos.Y - 9);
                    else
                        playerAnimation.playerPos = new Vector2(playerAnimation.playerPos.X, playerAnimation.playerPos.Y - 10);
                }
                // Block //
                else if (Keyboard.GetState().IsKeyDown(Keys.OemPeriod))
                {
                    if (lastKey != Keys.OemPeriod)
                        stateChange = true;
                    lastKey = Keys.OemPeriod;
                    playerAnimation.State = (int)Sprite.Block;
                    looping = false;
                    playerPosition = playerAnimation.playerPos;

                    if (playerAnimation.flipHorizontal)
                        playerAnimation.playerPos = new Vector2(playerAnimation.playerPos.X + 10, playerAnimation.playerPos.Y - 2);
                    else
                        playerAnimation.playerPos = new Vector2(playerAnimation.playerPos.X - 2, playerAnimation.playerPos.Y - 2);
                }
                else
                {
                    if (lastKey != Keys.None)
                        stateChange = true;
                    lastKey = Keys.None;
                    // playerAnimation.active = false;
                    playerAnimation.State = (int)Sprite.Idle;
                    //playerAnimation.currentState = (int)Sprite.Idle;
                }

                playerAnimation.Update(gameTime, stateChange, looping);
                stateChange = false;
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
    } // END: Class //
} // END: Namespace //

