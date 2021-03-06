﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace CSCI_2941_Lab5
{
    class PlayerSonya
    {
        KeyboardState oldState = Keyboard.GetState();
        GamePadState oldPadState = GamePad.GetState(PlayerIndex.One);
        bool devMode = false;
        SoundEffect kick, punch;            //http://mkw.mortalkombatonline.com/umk3/sounds/#female
        public Texture2D[] playerSprite = new Texture2D[(int)Sprite.Max];
        public Animation playerAnimation = new Animation();
        public Vector2[] FrameSize = new Vector2[(int)Sprite.Max];
        public Vector2 playerPosition = new Vector2(100f, 400f);
        public HitBox sonyaHitBox = new HitBox();
        public HitBox sonyaAttackHB = new HitBox();
        float moveSpeed = 300f;
        public bool looping = true;
        Keys lastKey;
        bool stateChange;
        Vector2 screenSize;
        public int currentState = (int)Sprite.Idle;
        public bool beenHit = false;
        public bool gameEnded;
        public bool winGame;
        public bool freezeFrame;
        //healthBar GreenBar = new healthBar();
        //healthBar RedBar = new healthBar();
        //int Health = 400;
        public void Initialize(int screenWidth, int screenHeight)
        {
            sonyaAttackHB.HBColor(Color.LightGreen);
            FrameSize[(int)Sprite.Idle] = new Vector2(69f, 128f);
            FrameSize[(int)Sprite.Run] = new Vector2(95f, 133f);
            FrameSize[(int)Sprite.Crouch] = new Vector2(74f, 114f);
            FrameSize[(int)Sprite.Mid_Punch] = new Vector2(109f, 123f);
            FrameSize[(int)Sprite.Kick] = new Vector2(120f, 131f);
            FrameSize[(int)Sprite.Block] = new Vector2(62f, 127f);
            FrameSize[(int)Sprite.Hit] = new Vector2(78f, 125f);
            FrameSize[(int)Sprite.Victory] = new Vector2(75f, 133f);
            FrameSize[(int)Sprite.Fall] = new Vector2(146f, 126f);

            playerAnimation.Initialize(playerPosition, FrameSize);
            screenSize = new Vector2(screenWidth, screenHeight);
        }
        public void LoadContent(ContentManager Content)
        {
            playerSprite[(int)Sprite.Idle] = Content.Load<Texture2D>("Sonya/Idle");
            playerSprite[(int)Sprite.Run] = Content.Load<Texture2D>("Sonya/Run");
            playerSprite[(int)Sprite.Crouch] = Content.Load<Texture2D>("Sonya/Crouch");
            playerSprite[(int)Sprite.Mid_Punch] = Content.Load<Texture2D>("Sonya/Mid-Punch");
            playerSprite[(int)Sprite.Kick] = Content.Load<Texture2D>("Sonya/Kick");
            playerSprite[(int)Sprite.Block] = Content.Load<Texture2D>("Sonya/Block");
            playerSprite[(int)Sprite.Hit] = Content.Load<Texture2D>("Sonya/Hit");
            playerSprite[(int)Sprite.Victory] = Content.Load<Texture2D>("Sonya/Victory");
            playerSprite[(int)Sprite.Fall] = Content.Load<Texture2D>("Sonya/Fall");

            punch = Content.Load<SoundEffect>("Sonya/S_punching");
            kick = Content.Load<SoundEffect>("Sonya/S_kicking");
            sonyaHitBox.HB(playerPosition, FrameSize[0]);
            playerAnimation.playerImg = playerSprite;
            //GreenBar.health(new Vector2(screenSize.X / 45, screenSize.Y / 30), Health, Color.LimeGreen);
            //RedBar.health(new Vector2(screenSize.X / 45, screenSize.Y / 30), Health, Color.Red);
        }
        public void Update(GameTime gameTime)
        {
            //playerAnimation.playerPos = playerPosition;
            if (Keyboard.GetState().IsKeyUp(Keys.S))
                playerAnimation.holdFrame = false;
            //Saving old and new keyboard state to tell if a key was just pressed or if it 
            //is being held down
            KeyboardState newState = Keyboard.GetState();
            GamePadState newpadState = GamePad.GetState(PlayerIndex.One);
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
                {
                    currentState = (int)Sprite.Idle;
                } 
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
                if (Keyboard.GetState().IsKeyDown(Keys.S) || (GamePad.GetState(PlayerIndex.One).DPad.Down == ButtonState.Pressed))
                {
                    playerAnimation.holdFrame = true;
                    //move attack hitbox off screen
                    sonyaAttackHB.HB(new Vector2(playerPosition.X, playerPosition.Y + 10000), FrameSize[0]);
                    freezeFrame = false;
                    currentState = (int)Sprite.Crouch;
                    if (lastKey != Keys.S)
                        stateChange = true;
                    lastKey = Keys.S;
                    playerAnimation.State = (int)Sprite.Crouch;
                    looping = false;

                    playerPosition = playerAnimation.playerPos;

                    //sonyaHitBox.HB(new Vector2(playerPosition.X + 15, playerPosition.Y + 105), FrameSize[3]/2);

                    if (!playerAnimation.flipHorizontal)
                    {
                        playerAnimation.playerPos = new Vector2(playerAnimation.playerPos.X - 40, playerAnimation.playerPos.Y + 30);
                        sonyaHitBox.HB(new Vector2(playerPosition.X + 15, playerPosition.Y + 110), FrameSize[3] / 2);
                    }

                    else
                    {
                        playerAnimation.playerPos = new Vector2(playerAnimation.playerPos.X + 30, playerAnimation.playerPos.Y + 25);
                        sonyaHitBox.HB(new Vector2(playerPosition.X + 40, playerPosition.Y + 105), FrameSize[3] / 2);
                    }

                }
                // Mid-Punch //
                else if (newState.IsKeyDown(Keys.C) || GamePad.GetState(PlayerIndex.One).Buttons.X == ButtonState.Pressed)
                {
                    freezeFrame = false;
                    if (!oldState.IsKeyDown(Keys.C) && !(oldPadState.Buttons.X == ButtonState.Pressed))
                    {
                        punch.Play(1f, .1f, .5f);
                        currentState = (int)Sprite.Mid_Punch;
                        if (lastKey != Keys.C)
                            stateChange = true;
                        lastKey = Keys.C;
                        playerAnimation.State = (int)Sprite.Mid_Punch;
                        looping = false;
                        playerPosition = playerAnimation.playerPos;
                        if (!playerAnimation.flipHorizontal)
                        {
                            playerAnimation.playerPos = new Vector2(playerAnimation.playerPos.X - 10, playerAnimation.playerPos.Y + 5);
                            sonyaHitBox.HB(new Vector2(playerPosition.X + 20, playerPosition.Y), FrameSize[(int)Sprite.Idle]);
                            sonyaAttackHB.HB(new Vector2(playerPosition.X + 123, playerPosition.Y + 50), new Vector2(70, 20));
                        }
                        else
                        {
                            playerAnimation.playerPos = new Vector2(playerAnimation.playerPos.X - 60, playerAnimation.playerPos.Y + 5);
                            sonyaHitBox.HB(new Vector2(playerPosition.X + 19, playerPosition.Y), FrameSize[(int)Sprite.Idle]);
                            sonyaAttackHB.HB(new Vector2(playerPosition.X - 88, playerPosition.Y + 50), new Vector2(70, 20));
                        }
                    }
                    else
                    {
                        playerAnimation.holdFrame = false;
                        //move attack hitbox off screen
                        sonyaAttackHB.HB(new Vector2(playerPosition.X, playerPosition.Y + 10000), FrameSize[0]);

                        currentState = (int)Sprite.Idle;
                        if (lastKey != Keys.None)
                            stateChange = true;
                        lastKey = Keys.None;
                        // playerAnimation.active = false;
                        playerAnimation.State = (int)Sprite.Idle;
                        //playerAnimation.currentState = (int)Sprite.Idle;
                        if (!playerAnimation.flipHorizontal)
                            sonyaHitBox.HB(playerPosition, FrameSize[0]);
                        else
                            sonyaHitBox.HB(new Vector2(playerPosition.X + 30, playerPosition.Y), FrameSize[0]);
                    }
                }
                // Kick //
                else if (newState.IsKeyDown(Keys.Z) || GamePad.GetState(PlayerIndex.One).Buttons.B == ButtonState.Pressed)
                {
                    freezeFrame = false;
                    if (!oldState.IsKeyDown(Keys.Z) || !(oldPadState.Buttons.B == ButtonState.Pressed))
                    {
                        playerAnimation.holdFrame = false;
                        kick.Play(1f, .1f, .5f);
                        currentState = (int)Sprite.Kick;
                        if (lastKey != Keys.Z)
                            stateChange = true;
                        lastKey = Keys.Z;
                        playerAnimation.State = (int)Sprite.Kick;
                        looping = false;
                        playerPosition = playerAnimation.playerPos;
                        if (!playerAnimation.flipHorizontal)
                        {
                            playerAnimation.playerPos = new Vector2(playerAnimation.playerPos.X - 10, playerAnimation.playerPos.Y);
                            sonyaHitBox.HB(new Vector2(playerPosition.X + 40, playerPosition.Y), FrameSize[(int)Sprite.Idle]);
                            sonyaAttackHB.HB(new Vector2(playerPosition.X + 60, playerPosition.Y + 50), new Vector2(110, 60));
                        }

                        else
                        {
                            playerAnimation.playerPos = new Vector2(playerAnimation.playerPos.X - 40, playerAnimation.playerPos.Y);
                            sonyaHitBox.HB(new Vector2(playerPosition.X + 40, playerPosition.Y), FrameSize[(int)Sprite.Idle]);
                            sonyaAttackHB.HB(new Vector2(playerPosition.X - 40, playerPosition.Y + 50), new Vector2(110, 60));
                        }
                    }
                    else
                    {
                        playerAnimation.holdFrame = false;
                        //move attack hitbox off screen
                        sonyaAttackHB.HB(new Vector2(playerPosition.X, playerPosition.Y + 10000), FrameSize[0]);

                        currentState = (int)Sprite.Idle;
                        if (lastKey != Keys.None)
                            stateChange = true;
                        lastKey = Keys.None;
                        // playerAnimation.active = false;
                        playerAnimation.State = (int)Sprite.Idle;
                        //playerAnimation.currentState = (int)Sprite.Idle;
                        if (!playerAnimation.flipHorizontal)
                            sonyaHitBox.HB(playerPosition, FrameSize[0]);
                        else
                            sonyaHitBox.HB(new Vector2(playerPosition.X + 30, playerPosition.Y), FrameSize[0]);
                    }
                }
                // Block //
                else if (newState.IsKeyDown(Keys.X) || newpadState.Buttons.Y == ButtonState.Pressed)
                {
                    freezeFrame = false;
                    if (!oldState.IsKeyDown(Keys.X) && !(oldPadState.Buttons.Y == ButtonState.Pressed))
                    {
                        playerAnimation.holdFrame = false;
                        //move attack hitbox off screen
                        sonyaAttackHB.HB(new Vector2(playerPosition.X, playerPosition.Y + 10000), FrameSize[0]);

                        currentState = (int)Sprite.Block;
                        if (lastKey != Keys.X)
                            stateChange = true;
                        lastKey = Keys.X;
                        playerAnimation.State = (int)Sprite.Block;
                        looping = false;
                        playerPosition = playerAnimation.playerPos;

                        if (!playerAnimation.flipHorizontal)
                        {
                            sonyaHitBox.HB(new Vector2(playerPosition.X - 17, playerPosition.Y), FrameSize[(int)Sprite.Block]);
                        }
                        else
                        {
                            playerAnimation.playerPos = new Vector2(playerAnimation.playerPos.X + 10, playerAnimation.playerPos.Y);
                            sonyaHitBox.HB(new Vector2(playerPosition.X + 57, playerPosition.Y), FrameSize[(int)Sprite.Block]);
                        }
                    }
                    else
                    {
                        playerAnimation.holdFrame = false;
                        //move attack hitbox off screen
                        sonyaAttackHB.HB(new Vector2(playerPosition.X, playerPosition.Y + 10000), FrameSize[0]);

                        currentState = (int)Sprite.Idle;
                        if (lastKey != Keys.None)
                            stateChange = true;
                        lastKey = Keys.None;
                        // playerAnimation.active = false;
                        playerAnimation.State = (int)Sprite.Idle;
                        //playerAnimation.currentState = (int)Sprite.Idle;
                        if (!playerAnimation.flipHorizontal)
                            sonyaHitBox.HB(playerPosition, FrameSize[0]);
                        else
                            sonyaHitBox.HB(new Vector2(playerPosition.X + 30, playerPosition.Y), FrameSize[0]);
                    }
                }
                // Move Right //
                else if (Keyboard.GetState().IsKeyDown(Keys.D) || GamePad.GetState(PlayerIndex.One).DPad.Right == ButtonState.Pressed)
                {
                    freezeFrame = false;
                    playerAnimation.holdFrame = false;
                    //move attack hitbox off screen
                    sonyaAttackHB.HB(new Vector2(playerPosition.X, playerPosition.Y + 10000), FrameSize[0]);
                    currentState = (int)Sprite.Run;
                    if (lastKey != Keys.D)
                        stateChange = true;
                    lastKey = Keys.D;
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
                        sonyaHitBox.HB(new Vector2(playerPosition.X + 40, playerPosition.Y), FrameSize[1]);
                    else
                        sonyaHitBox.HB(playerPosition, FrameSize[0]);
                }
                // Move Left //
                else if (Keyboard.GetState().IsKeyDown(Keys.A) || GamePad.GetState(PlayerIndex.One).DPad.Left == ButtonState.Pressed)
                {
                    freezeFrame = false;
                    playerAnimation.holdFrame = false;
                    //move attack hitbox off screen
                    sonyaAttackHB.HB(new Vector2(playerPosition.X, playerPosition.Y + 10000), FrameSize[0]);
                    currentState = (int)Sprite.Run;
                    if (lastKey != Keys.A)
                        stateChange = true;
                    lastKey = Keys.A;
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
                        sonyaHitBox.HB(playerPosition, FrameSize[1]);
                    else
                        sonyaHitBox.HB(new Vector2(playerPosition.X + 30, playerPosition.Y), FrameSize[0]);
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
                    }
                    else
                    {
                        currentState = (int)Sprite.Fall;
                        playerAnimation.State = (int)Sprite.Fall;
                    }
                }
                else
                {
                    freezeFrame = false;
                    playerAnimation.holdFrame = false;
                    //move attack hitbox off screen
                    sonyaAttackHB.HB(new Vector2(playerPosition.X, playerPosition.Y + 10000), FrameSize[0]);
                    currentState = (int)Sprite.Idle;
                    if (lastKey != Keys.None)
                        stateChange = true;
                    lastKey = Keys.None;
                    // playerAnimation.active = false;
                    playerAnimation.State = (int)Sprite.Idle;
                    //playerAnimation.currentState = (int)Sprite.Idle;
                    if (!playerAnimation.flipHorizontal)
                        sonyaHitBox.HB(playerPosition, FrameSize[0]);
                    else
                        sonyaHitBox.HB(new Vector2(playerPosition.X + 30, playerPosition.Y), FrameSize[0]);
                }

                playerAnimation.Update(gameTime, stateChange, looping, freezeFrame);
                stateChange = false;
                oldState = newState;
                oldPadState = newpadState;
            }
        }
        public void resetPos()
        {
            playerPosition = new Vector2(100f, 400f);
            playerAnimation.Initialize(playerPosition, FrameSize);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            playerAnimation.Draw(spriteBatch);
            if (devMode)
            {
                sonyaAttackHB.Draw(spriteBatch);
                sonyaHitBox.Draw(spriteBatch);
            }
            //RedBar.Draw(spriteBatch);
            //GreenBar.Draw(spriteBatch);
            
        }
        public void Dispose()
        {
            for (int i = 0; i < (int)Sprite.Max; i++)
                playerSprite[i].Dispose();
        }
    }
}
