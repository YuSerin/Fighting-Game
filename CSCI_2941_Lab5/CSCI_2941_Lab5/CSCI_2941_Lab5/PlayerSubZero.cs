using System;
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
        bool devMode = true;
        SoundEffect kick, punch;        //http://mkw.mortalkombatonline.com/umk3/sounds/#male
        Texture2D[] playerSprite = new Texture2D[(int)Sprite.Max];
        Animation playerAnimation = new Animation();
        Vector2[] FrameSize = new Vector2[(int)Sprite.Max];
        public Vector2 playerPosition = new Vector2(1050f, 400f);
        public HitBox subZeroHitBox = new HitBox();
        float moveSpeed = 300f;
        //float jumpSpeed = 10f;
        bool looping = true;
        Keys lastKey;
        bool stateChange;
       // bool reachedGround;
        public int currentState = (int)Sprite.Idle;
        Vector2 screenSize;
        public void Initialize(int screenWidth, int screenHeight)
        {
            FrameSize[(int)Sprite.Idle] = new Vector2(68f, 131f);
            FrameSize[(int)Sprite.Run] = new Vector2(95f, 134f);
            FrameSize[(int)Sprite.Crouch] = new Vector2(70f, 117f);
            FrameSize[(int)Sprite.Mid_Punch] = new Vector2(113f, 134f);
            FrameSize[(int)Sprite.Kick] = new Vector2(126f, 138f);
            FrameSize[(int)Sprite.Block] = new Vector2(61f, 131f);
            //FrameSize[(int)Sprite.Jump] = new Vector2(70f, 136f);

            screenSize = new Vector2(screenWidth, screenHeight);
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
            //playerSprite[(int)Sprite.Jump] = Content.Load<Texture2D>("SubZero/Jump");

            punch = Content.Load<SoundEffect>("SubZero/punching");
            kick = Content.Load<SoundEffect>("SubZero/kicking");
            subZeroHitBox.HB(playerPosition, FrameSize[0]);
            playerAnimation.playerImg = playerSprite;
        }
        public void Update(GameTime gameTime)
        {
            //Saving old and new keyboard state to tell if a key was just pressed or if it 
            //is being held down
            KeyboardState newState = Keyboard.GetState();

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
                playerAnimation.Update(gameTime, stateChange, looping);
                if (currentState != (int)Sprite.Block)
                    currentState = (int)Sprite.Idle;
                else
                    currentState = (int)Sprite.Block;
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
                else if (Keyboard.GetState().IsKeyDown(Keys.K))
                {
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
                // Crouch down //
                else if (Keyboard.GetState().IsKeyDown(Keys.L))
                {

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
                else if (Keyboard.GetState().IsKeyDown(Keys.OemQuestion))
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
                // Jump //
                /*
                else if (Keyboard.GetState().IsKeyDown(Keys.O))
                {
                    currentState = (int)Sprite.Jump;
                    
                    if (lastKey != Keys.O)
                    {
                        stateChange = true;
                        playerAnimation.hasJumped = true;
                    }
                    lastKey = Keys.O;
                    playerAnimation.State = (int)Sprite.Jump;
                    looping = false;
                    playerPosition = playerAnimation.playerPos;

                    if (playerAnimation.hasJumped)
                    {
                        if (playerAnimation.playerPos.Y > 100f)
                            playerAnimation.playerPos.Y -= jumpSpeed * (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                        else
                            playerAnimation.hasJumped = false;
                    }

                    if (playerAnimation.hasJumped == false)
                    {
                        playerAnimation.playerPos.Y += playerAnimation.gravity * (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                    }

                    reachedGround = (playerAnimation.playerPos.Y >= 400f);

                    if (reachedGround)
                    {
                        playerAnimation.playerPos.Y = 400f;
                    }
                        

                    //playerAnimation.playerPos = playerPosition;

                    if (playerAnimation.flipHorizontal)
                    {
                        //playerAnimation.playerPos = new Vector2(playerAnimation.playerPos.X, playerAnimation.playerPos.Y);
                        subZeroHitBox.HB(new Vector2(playerPosition.X + 70, playerPosition.Y), FrameSize[(int)Sprite.Jump]);
                    }

                    else
                    {
                        //playerAnimation.playerPos = new Vector2(playerAnimation.playerPos.X - 2, playerAnimation.playerPos.Y - 2);
                        subZeroHitBox.HB(new Vector2(playerPosition.X - 20, playerPosition.Y), FrameSize[(int)Sprite.Jump]);
                    }
                }
                */
                // Idle //
                else
                {
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
                

                playerAnimation.Update(gameTime, stateChange, looping);
                stateChange = false;
                oldState = newState;

                // playerAnimation.hasJumped = false;
            }

        }
        public void Draw(SpriteBatch spriteBatch)
        {
            playerAnimation.Draw(spriteBatch);
            if (devMode)
                subZeroHitBox.Draw(spriteBatch);
        }
        public void Dispose()
        {
            for (int i = 0; i < (int)Sprite.Max; i++)
                playerSprite[i].Dispose();
        }
    } // END: Class //
} // END: Namespace //

