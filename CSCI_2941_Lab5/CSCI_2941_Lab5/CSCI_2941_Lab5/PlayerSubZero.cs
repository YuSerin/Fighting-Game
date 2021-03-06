﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;

namespace CSCI_2941_Lab5
{
    class PlayerSubZero
    {
        KeyboardState oldState = Keyboard.GetState();
        GamePadState oldPadState = GamePad.GetState(PlayerIndex.Two);
        bool devMode = false;
        SoundEffect kick, punch;        //http://mkw.mortalkombatonline.com/umk3/sounds/#male
        public Texture2D[] playerSprite = new Texture2D[(int)Sprite.Max];
        public Animation playerAnimation = new Animation();
        public Vector2[] FrameSize = new Vector2[(int)Sprite.Max];
        public Vector2 playerPosition = new Vector2(1050f, 400f);
        public HitBox subZeroHitBox = new HitBox();
        public HitBox subZAttackHB = new HitBox();
        float moveSpeed = 300f;
        //float jumpSpeed = 10f;
        public bool looping = true;
        Keys lastKey;
        bool stateChange;
       // bool reachedGround;
        public int currentState = (int)Sprite.Idle;
        public bool beenHit = false;
        Vector2 screenSize;
        public bool gameEnded;
        public bool winGame;
        public bool freezeFrame;
        public void Initialize(int screenWidth, int screenHeight)
        {
            FrameSize[(int)Sprite.Idle] = new Vector2(68f, 131f);
            FrameSize[(int)Sprite.Run] = new Vector2(95f, 134f);
            FrameSize[(int)Sprite.Crouch] = new Vector2(70f, 117f);
            FrameSize[(int)Sprite.Mid_Punch] = new Vector2(113f, 134f);
            FrameSize[(int)Sprite.Kick] = new Vector2(126f, 138f);
            FrameSize[(int)Sprite.Block] = new Vector2(61f, 131f);
            FrameSize[(int)Sprite.Hit] = new Vector2(68f, 127f);
            FrameSize[(int)Sprite.Victory] = new Vector2(75f, 171f);
            FrameSize[(int)Sprite.Fall] = new Vector2(134f, 135f);

            screenSize = new Vector2(screenWidth, screenHeight);
            playerAnimation.flipHorizontal = true;

            playerAnimation.Initialize(playerPosition, FrameSize);
        }
        public void LoadContent(ContentManager Content)
        {
            subZAttackHB.HBColor(Color.LightBlue);
            playerSprite[(int)Sprite.Idle] = Content.Load<Texture2D>("SubZero/Idle");
            playerSprite[(int)Sprite.Run] = Content.Load<Texture2D>("SubZero/Run");
            playerSprite[(int)Sprite.Crouch] = Content.Load<Texture2D>("SubZero/Crouch");
            playerSprite[(int)Sprite.Mid_Punch] = Content.Load<Texture2D>("SubZero/Mid_Punch");
            playerSprite[(int)Sprite.Kick] = Content.Load<Texture2D>("SubZero/Kick");
            playerSprite[(int)Sprite.Block] = Content.Load<Texture2D>("SubZero/Block");
            playerSprite[(int)Sprite.Hit] = Content.Load<Texture2D>("SubZero/Hit");
            playerSprite[(int)Sprite.Victory] = Content.Load<Texture2D>("SubZero/Victory");
            playerSprite[(int)Sprite.Fall] = Content.Load<Texture2D>("SubZero/Fall");

            punch = Content.Load<SoundEffect>("SubZero/punching");
            kick = Content.Load<SoundEffect>("SubZero/kicking");
            subZeroHitBox.HB(playerPosition, FrameSize[0]);
            playerAnimation.playerImg = playerSprite;
        }
        public void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyUp(Keys.L))
                playerAnimation.holdFrame = false;
            //Saving old and new keyboard state to tell if a key was just pressed or if it 
            //is being held down
            KeyboardState newState = Keyboard.GetState();
            GamePadState newPadState = GamePad.GetState(PlayerIndex.Two);

            if (newState.IsKeyDown(Keys.OemPlus))
            {
                if (!oldState.IsKeyDown(Keys.OemPlus))
                {
                    if (devMode == true)
                        devMode = false;
                    else devMode = true;
                }
            }

            if (looping == false)
            {
                playerAnimation.Update(gameTime, stateChange, looping, freezeFrame);
                if (currentState != (int)Sprite.Block)
                    currentState = (int)Sprite.Idle;
                else
                    currentState = (int)Sprite.Block;

                // Reached end of sprite sheet //
                if (playerAnimation.currentFrame.X >= playerAnimation.playerImg[playerAnimation.State].Width && !freezeFrame)
                {
                    playerAnimation.playerPos = playerPosition;
                    beenHit = false;
                    playerAnimation.currentFrame = new Vector2(0, 0);
                    looping = true;
                }
            }

            if (looping)
            {
                // Crouch down //
                if (Keyboard.GetState().IsKeyDown(Keys.L) || GamePad.GetState(PlayerIndex.Two).DPad.Down == ButtonState.Pressed)
                {
                    freezeFrame = false;
                    playerAnimation.holdFrame = true;
                    //move attack hitbox off screen
                    subZAttackHB.HB(new Vector2(playerPosition.X, playerPosition.Y + 10000), FrameSize[0]);

                    currentState = (int)Sprite.Crouch;
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
                else if (newState.IsKeyDown(Keys.OemQuestion) || GamePad.GetState(PlayerIndex.Two).Buttons.X == ButtonState.Pressed)
                {
                    freezeFrame = false;
                    if (!oldState.IsKeyDown(Keys.OemQuestion) && !(oldPadState.Buttons.X == ButtonState.Pressed))
                    {
                        punch.Play(1f, .1f, .5f);
                        currentState = (int)Sprite.Mid_Punch;
                        if (lastKey != Keys.OemQuestion)
                            stateChange = true;
                        lastKey = Keys.OemQuestion;
                        playerAnimation.State = (int)Sprite.Mid_Punch;
                        looping = false;
                        playerPosition = playerAnimation.playerPos;
                        if (playerAnimation.flipHorizontal)
                        {
                            playerAnimation.playerPos = new Vector2(playerAnimation.playerPos.X - 80, playerAnimation.playerPos.Y - 3);
                            subZeroHitBox.HB(new Vector2(playerPosition.X + 19, playerPosition.Y), FrameSize[(int)Sprite.Idle]);
                            subZAttackHB.HB(new Vector2(playerPosition.X - 85, playerPosition.Y + 50), new Vector2(70, 20));
                        }

                        else
                        {
                            playerAnimation.playerPos = new Vector2(playerAnimation.playerPos.X, playerAnimation.playerPos.Y - 3);
                            subZeroHitBox.HB(new Vector2(playerPosition.X + 20, playerPosition.Y), FrameSize[(int)Sprite.Idle]);
                            subZAttackHB.HB(new Vector2(playerPosition.X + 125, playerPosition.Y + 50), new Vector2(70, 20));
                        }
                    }
                    else
                    {
                        //move attack hitbox off screen
                        subZAttackHB.HB(new Vector2(playerPosition.X, playerPosition.Y + 10000), FrameSize[0]);

                        currentState = (int)Sprite.Idle;
                        if (lastKey != Keys.None)
                            stateChange = true;
                        lastKey = Keys.None;
                        // playerAnimation.active = false;
                        playerAnimation.State = (int)Sprite.Idle;
                        //playerAnimation.currentState = (int)Sprite.Idle;
                        if (playerAnimation.flipHorizontal)
                            subZeroHitBox.HB(new Vector2(playerPosition.X + 25, playerPosition.Y), FrameSize[0]);
                        else
                            subZeroHitBox.HB(playerPosition, FrameSize[0]);
                    }
                }
                // Kick //
                else if (newState.IsKeyDown(Keys.OemComma) || GamePad.GetState(PlayerIndex.Two).Buttons.B == ButtonState.Pressed)
                {
                    freezeFrame = false;
                    if (!oldState.IsKeyDown(Keys.OemComma) || !(oldPadState.Buttons.B == ButtonState.Pressed))
                    {
                        kick.Play(1f, .1f, .5f);
                        currentState = (int)Sprite.Kick;
                        if (lastKey != Keys.OemComma)
                            stateChange = true;
                        lastKey = Keys.OemComma;
                        playerAnimation.State = (int)Sprite.Kick;
                        looping = false;
                        playerPosition = playerAnimation.playerPos;
                        if (playerAnimation.flipHorizontal)
                        {
                            playerAnimation.playerPos = new Vector2(playerAnimation.playerPos.X - 70, playerAnimation.playerPos.Y - 15);
                            subZeroHitBox.HB(new Vector2(playerPosition.X + 50, playerPosition.Y), FrameSize[(int)Sprite.Idle]);
                            subZAttackHB.HB(new Vector2(playerPosition.X - 60, playerPosition.Y + 50), new Vector2(110, 60));
                        }

                        else
                        {
                            playerAnimation.playerPos = new Vector2(playerAnimation.playerPos.X, playerAnimation.playerPos.Y - 10);
                            subZeroHitBox.HB(new Vector2(playerPosition.X + 20, playerPosition.Y), FrameSize[(int)Sprite.Idle]);
                            subZAttackHB.HB(new Vector2(playerPosition.X + 60, playerPosition.Y + 50), new Vector2(110, 60));
                        }
                    }
                    else
                    {
                        //move attack hitbox off screen
                        subZAttackHB.HB(new Vector2(playerPosition.X, playerPosition.Y + 10000), FrameSize[0]);

                        currentState = (int)Sprite.Idle;
                        if (lastKey != Keys.None)
                            stateChange = true;
                        lastKey = Keys.None;
                        // playerAnimation.active = false;
                        playerAnimation.State = (int)Sprite.Idle;
                        //playerAnimation.currentState = (int)Sprite.Idle;
                        if (playerAnimation.flipHorizontal)
                            subZeroHitBox.HB(new Vector2(playerPosition.X + 25, playerPosition.Y), FrameSize[0]);
                        else
                            subZeroHitBox.HB(playerPosition, FrameSize[0]);
                    }
                }
                // Block //
                else if (newState.IsKeyDown(Keys.OemPeriod) || newPadState.Buttons.Y == ButtonState.Pressed)
                {
                    freezeFrame = false;
                    if (!oldState.IsKeyDown(Keys.OemPeriod) && !(oldPadState.Buttons.Y == ButtonState.Pressed))
                    {
                        //move attack hitbox off screen
                        subZAttackHB.HB(new Vector2(playerPosition.X, playerPosition.Y + 10000), FrameSize[0]);

                        currentState = (int)Sprite.Block;
                        if (lastKey != Keys.OemPeriod)
                            stateChange = true;
                        lastKey = Keys.OemPeriod;
                        playerAnimation.State = (int)Sprite.Block;
                        looping = false;
                        playerPosition = playerAnimation.playerPos;

                        if (playerAnimation.flipHorizontal)
                        {
                            playerAnimation.playerPos = new Vector2(playerAnimation.playerPos.X + 20, playerAnimation.playerPos.Y - 2);
                            subZeroHitBox.HB(new Vector2(playerPosition.X + 70, playerPosition.Y), FrameSize[(int)Sprite.Block]);
                        }

                        else
                        {
                            playerAnimation.playerPos = new Vector2(playerAnimation.playerPos.X - 2, playerAnimation.playerPos.Y - 2);
                            subZeroHitBox.HB(new Vector2(playerPosition.X - 20, playerPosition.Y), FrameSize[(int)Sprite.Block]);
                        }
                    }
                    else
                    {
                        //move attack hitbox off screen
                        subZAttackHB.HB(new Vector2(playerPosition.X, playerPosition.Y + 10000), FrameSize[0]);

                        currentState = (int)Sprite.Idle;
                        if (lastKey != Keys.None)
                            stateChange = true;
                        lastKey = Keys.None;
                        // playerAnimation.active = false;
                        playerAnimation.State = (int)Sprite.Idle;
                        //playerAnimation.currentState = (int)Sprite.Idle;
                        if (playerAnimation.flipHorizontal)
                            subZeroHitBox.HB(new Vector2(playerPosition.X + 25, playerPosition.Y), FrameSize[0]);
                        else
                            subZeroHitBox.HB(playerPosition, FrameSize[0]);
                    }
                }
                // Move Right //
                else if (Keyboard.GetState().IsKeyDown(Keys.OemSemicolon) || GamePad.GetState(PlayerIndex.Two).DPad.Right == ButtonState.Pressed)
                {
                    freezeFrame = false;
                    //move attack hitbox off screen
                    subZAttackHB.HB(new Vector2(playerPosition.X, playerPosition.Y + 10000), FrameSize[0]);

                    currentState = (int)Sprite.Run;
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
                else if (Keyboard.GetState().IsKeyDown(Keys.K) || GamePad.GetState(PlayerIndex.Two).DPad.Left == ButtonState.Pressed)
                {
                    freezeFrame = false;
                    //move attack hitbox off screen
                    subZAttackHB.HB(new Vector2(playerPosition.X, playerPosition.Y + 10000), FrameSize[0]);

                    currentState = (int)Sprite.Run;
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
                // Hit //
                else if (beenHit == true)
                {
                    freezeFrame = false;
                    playerAnimation.holdFrame = false;
                    currentState = (int)Sprite.Hit;
                    stateChange = true;
                    playerAnimation.State = (int)Sprite.Hit;
                    looping = false;
                    playerPosition = playerAnimation.playerPos;
                }
                // Game Over //
                else if (gameEnded)
                {
                    freezeFrame = true;
                    playerAnimation.holdFrame = false;
                    stateChange = true;
                    looping = false;
                    playerPosition = playerAnimation.playerPos;

                    if (winGame)
                    {
                        currentState = (int)Sprite.Victory;
                        playerAnimation.State = (int)Sprite.Victory;
                        playerAnimation.playerPos.Y -= 100;
                    }
                    else
                    {
                        currentState = (int)Sprite.Fall;
                        playerAnimation.State = (int)Sprite.Fall;
                        playerAnimation.playerPos.Y += 10;
                    }
                }
                else
                {
                    freezeFrame = false;
                    //move attack hitbox off screen
                    subZAttackHB.HB(new Vector2(playerPosition.X, playerPosition.Y + 10000), FrameSize[0]);

                    currentState = (int)Sprite.Idle;
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
                

                playerAnimation.Update(gameTime, stateChange, looping, freezeFrame);
                stateChange = false;
                oldState = newState;
                oldPadState = newPadState;
                // playerAnimation.hasJumped = false;
            }

        }
        public void resetPos()
        {
            playerPosition = new Vector2(1050f, 400f);
            playerAnimation.Initialize(playerPosition, FrameSize);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            playerAnimation.Draw(spriteBatch);
            if (devMode)
            {
                subZeroHitBox.Draw(spriteBatch);
                subZAttackHB.Draw(spriteBatch);
            }
        }
        public void Dispose()
        {
            for (int i = 0; i < (int)Sprite.Max; i++)
                playerSprite[i].Dispose();
        }
    } // END: Class //
} // END: Namespace //

