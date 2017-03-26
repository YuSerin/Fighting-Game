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
        HitBox subZeroHitBox = new HitBox();
        float moveSpeed = 300f;
        bool looping = true;
        Keys lastKey;
        bool stateChange;
       // int currentState = (int)Sprite.Idle;
        public void Initialize(int screenWidth, int screenHeight)
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

            subZeroHitBox.HB(playerPosition, FrameSize[0]);
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
                if (Keyboard.GetState().IsKeyDown(Keys.OemSemicolon))
                {
                    if (lastKey != Keys.OemSemicolon)
                        stateChange = true;
                    lastKey = Keys.OemSemicolon;

                    if (playerPosition.X <= 1180)
                        playerAnimation.State = (int)Sprite.Run;
                    else
                    {
                        if (playerAnimation.State != (int)Sprite.Idle)
                            stateChange = true;
                        playerAnimation.State = (int)Sprite.Idle;
                    }
                    playerAnimation.flipHorizontal = false;
                    //playerAnimation.currentState = playerAnimation.State;
                    if (playerPosition.X <= 1180)
                        playerPosition.X += moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    playerAnimation.playerPos.X = playerPosition.X;
                    if (playerPosition.X <= 1180)
                        subZeroHitBox.HB(new Vector2(playerPosition.X + 30, playerPosition.Y), FrameSize[1]);
                    else
                        subZeroHitBox.HB(playerPosition, FrameSize[0]);
                }
                // Move Left //
                else if (Keyboard.GetState().IsKeyDown(Keys.K))
                {
                    if (lastKey != Keys.K)
                        stateChange = true;
                    lastKey = Keys.K;
                    if (playerPosition.X >= -10)
                        playerAnimation.State = (int)Sprite.Run;
                    else
                    {
                        if (playerAnimation.State != (int)Sprite.Idle)
                            stateChange = true;
                        playerAnimation.State = (int)Sprite.Idle;
                    }
                    playerAnimation.flipHorizontal = true;
                    if (playerPosition.X >= -10)
                        playerPosition.X -= moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    playerAnimation.playerPos.X = playerPosition.X;
                    if (playerPosition.X >= -10)
                        subZeroHitBox.HB(new Vector2(playerPosition.X + 20, playerPosition.Y), FrameSize[1]);
                    else
                        subZeroHitBox.HB(playerPosition, FrameSize[0]);
                }
                // Crouch down //
                else if (Keyboard.GetState().IsKeyDown(Keys.L))
                {
                    if (lastKey != Keys.L)
                        stateChange = true;
                    lastKey = Keys.L;
                    playerAnimation.State = (int)Sprite.Crouch;
                    looping = false;
                    playerPosition = playerAnimation.playerPos;
                    //subZeroHitBox.HB(new Vector2(playerPosition.X + 25, playerPosition.Y + 100), FrameSize[3] / 2);
                    if (playerAnimation.flipHorizontal)
                    {
                        playerAnimation.playerPos = new Vector2(playerAnimation.playerPos.X, playerAnimation.playerPos.Y + 25);
                        subZeroHitBox.HB(new Vector2(playerPosition.X + 30, playerPosition.Y + 110), FrameSize[3] / 2);
                    }
                        
                    else
                    {
                        playerAnimation.playerPos = new Vector2(playerAnimation.playerPos.X, playerAnimation.playerPos.Y + 25);
                        subZeroHitBox.HB(new Vector2(playerPosition.X + 25, playerPosition.Y + 110), FrameSize[3] / 2);
                    }

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
                    {
                        playerAnimation.playerPos = new Vector2(playerAnimation.playerPos.X - 80, playerAnimation.playerPos.Y - 3);
                        subZeroHitBox.HB(new Vector2(playerPosition.X - 70, playerPosition.Y), FrameSize[(int)Sprite.Mid_Punch]);
                    }

                    else
                    {
                        playerAnimation.playerPos = new Vector2(playerAnimation.playerPos.X, playerAnimation.playerPos.Y - 3);
                        subZeroHitBox.HB(new Vector2(playerPosition.X + 40, playerPosition.Y), FrameSize[(int)Sprite.Mid_Punch]);
                    }
                       
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
                    {
                        playerAnimation.playerPos = new Vector2(playerAnimation.playerPos.X - 120, playerAnimation.playerPos.Y - 15);
                        subZeroHitBox.HB(new Vector2(playerPosition.X - 110, playerPosition.Y), FrameSize[(int)Sprite.Kick]);
                    }

                    else
                    {
                        playerAnimation.playerPos = new Vector2(playerAnimation.playerPos.X, playerAnimation.playerPos.Y - 10);
                        subZeroHitBox.HB(new Vector2(playerPosition.X + 50, playerPosition.Y), FrameSize[(int)Sprite.Kick]);
                    }
                        
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
                    {
                        playerAnimation.playerPos = new Vector2(playerAnimation.playerPos.X + 20, playerAnimation.playerPos.Y - 2);
                        subZeroHitBox.HB(new Vector2(playerPosition.X + 40, playerPosition.Y), FrameSize[(int)Sprite.Block]);
                    }

                    else
                    {
                        playerAnimation.playerPos = new Vector2(playerAnimation.playerPos.X - 2, playerAnimation.playerPos.Y - 2);
                        subZeroHitBox.HB(new Vector2(playerPosition.X, playerPosition.Y), FrameSize[(int)Sprite.Block]);
                    }
                }
                else
                {
                    if (lastKey != Keys.None)
                        stateChange = true;
                    lastKey = Keys.None;
                    // playerAnimation.active = false;
                    playerAnimation.State = (int)Sprite.Idle;
                    //playerAnimation.currentState = (int)Sprite.Idle;
                    if (playerAnimation.flipHorizontal)
                        subZeroHitBox.HB(new Vector2(playerPosition.X+25, playerPosition.Y), FrameSize[0]);
                    else
                     subZeroHitBox.HB(playerPosition, FrameSize[0]);
                }

                playerAnimation.Update(gameTime, stateChange, looping);
                stateChange = false;
            }

        }
        public void Draw(SpriteBatch spriteBatch)
        {
            playerAnimation.Draw(spriteBatch);
            subZeroHitBox.Draw(spriteBatch);
        }
        public void Dispose()
        {
            for (int i = 0; i < (int)Sprite.Max; i++)
                playerSprite[i].Dispose();
        }
    } // END: Class //
} // END: Namespace //

