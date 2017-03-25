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
        Idle, Run, Crouch, Mid_Punch, Kick, Block, Max,
    }
    class PlayerSonya
    {
        Texture2D[] playerSprite = new Texture2D[(int)Sprite.Max];
        Animation playerAnimation = new Animation();
        Vector2[] FrameSize = new Vector2[(int)Sprite.Max];
        Vector2 playerPosition = new Vector2(100f, 400f);
        HitBox sonyaHitBox = new HitBox();
        float moveSpeed = 300f;
        bool looping = true;
        Keys lastKey;
        bool stateChange;
        public void Initialize()
        {
            FrameSize[(int)Sprite.Idle] = new Vector2(69f, 128f);
            FrameSize[(int)Sprite.Run] = new Vector2(95f, 133f);
            FrameSize[(int)Sprite.Crouch] = new Vector2(74f, 114f);
            FrameSize[(int)Sprite.Mid_Punch] = new Vector2(109f, 123f);
            FrameSize[(int)Sprite.Kick] = new Vector2(120f, 131f);
            FrameSize[(int)Sprite.Block] = new Vector2(62f, 127f);

            playerAnimation.Initialize(playerPosition, FrameSize);
        }
        public void LoadContent(ContentManager Content)
        {
            playerSprite[(int)Sprite.Idle] = Content.Load<Texture2D>("Sonya/Idle");
            playerSprite[(int)Sprite.Run] = Content.Load<Texture2D>("Sonya/Run");
            playerSprite[(int)Sprite.Crouch] = Content.Load<Texture2D>("Sonya/Crouch");
            playerSprite[(int)Sprite.Mid_Punch] = Content.Load<Texture2D>("Sonya/Mid-Punch");
            playerSprite[(int)Sprite.Kick] = Content.Load<Texture2D>("Sonya/Kick");
            playerSprite[(int)Sprite.Block] = Content.Load<Texture2D>("Sonya/Block");

            sonyaHitBox.HB(playerPosition, FrameSize[0]);
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
                if (Keyboard.GetState().IsKeyDown(Keys.D))
                {
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
                        sonyaHitBox.HB(new Vector2(playerPosition.X + 30, playerPosition.Y), FrameSize[1]);
                    else
                        sonyaHitBox.HB(playerPosition, FrameSize[0]);
                }
                // Move Left //
                else if (Keyboard.GetState().IsKeyDown(Keys.A))
                {
                    if (lastKey != Keys.A)
                        stateChange = true;
                    lastKey = Keys.A;
                    if (playerPosition.X >= -10)
                        playerAnimation.State = (int)Sprite.Run;
                    else {
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
                        sonyaHitBox.HB(playerPosition, FrameSize[0]);
                }
                // Crouch down //
                else if (Keyboard.GetState().IsKeyDown(Keys.S))
                {
                    if (lastKey != Keys.S)
                        stateChange = true;
                    lastKey = Keys.S;
                    playerAnimation.State = (int)Sprite.Crouch;
                    looping = false;

                    playerPosition = playerAnimation.playerPos;

                    sonyaHitBox.HB(new Vector2(playerPosition.X, playerPosition.Y + 70), FrameSize[3]/2);

                    if (!playerAnimation.flipHorizontal)
                        playerAnimation.playerPos = new Vector2(playerAnimation.playerPos.X - 20, playerAnimation.playerPos.Y + 16);
                    else
                        playerAnimation.playerPos = new Vector2(playerAnimation.playerPos.X + 15, playerAnimation.playerPos.Y + 16);
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.V))
                {
                    if (lastKey != Keys.V)
                        stateChange = true;
                    lastKey = Keys.V;
                    playerAnimation.State = (int)Sprite.Mid_Punch;
                    looping = false;
                    playerPosition = playerAnimation.playerPos;
                    if (!playerAnimation.flipHorizontal)
                        playerAnimation.playerPos = new Vector2(playerAnimation.playerPos.X - 10, playerAnimation.playerPos.Y + 5);
                    else
                        playerAnimation.playerPos = new Vector2(playerAnimation.playerPos.X - 30, playerAnimation.playerPos.Y + 5);

                }
                else if (Keyboard.GetState().IsKeyDown(Keys.N))
                {
                    if (lastKey != Keys.N)
                        stateChange = true;
                    lastKey = Keys.N;
                    playerAnimation.State = (int)Sprite.Kick;
                    looping = false;
                    playerPosition = playerAnimation.playerPos;
                    if (!playerAnimation.flipHorizontal)
                        playerAnimation.playerPos = new Vector2(playerAnimation.playerPos.X - 10, playerAnimation.playerPos.Y);
                    else
                        playerAnimation.playerPos = new Vector2(playerAnimation.playerPos.X - 40, playerAnimation.playerPos.Y);
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.B))
                {
                    if (lastKey != Keys.B)
                        stateChange = true;
                    lastKey = Keys.B;
                    playerAnimation.State = (int)Sprite.Block;
                    looping = false;
                    playerPosition = playerAnimation.playerPos;
                    
                    if (playerAnimation.flipHorizontal)
                        playerAnimation.playerPos = new Vector2(playerAnimation.playerPos.X + 10, playerAnimation.playerPos.Y);
                }
                else
                {
                    if (lastKey != Keys.None)
                        stateChange = true;
                    lastKey = Keys.None;
                    // playerAnimation.active = false;
                    playerAnimation.State = (int)Sprite.Idle;
                    //playerAnimation.currentState = (int)Sprite.Idle;
                    sonyaHitBox.HB(playerPosition, FrameSize[0]);
                }

                playerAnimation.Update(gameTime, stateChange, looping);
                stateChange = false;
            }
            
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            playerAnimation.Draw(spriteBatch);
            sonyaHitBox.Draw(spriteBatch);
        }
        public void Dispose()
        {
            for (int i = 0; i < (int)Sprite.Max; i++)
                playerSprite[i].Dispose();
        }
    }
}
