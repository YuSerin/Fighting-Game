using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

// Credit: Youtuber - CodingMadeEasy //
namespace IWKS_3400_Lab5
{
    class Player
    {
        public Texture2D playerImg { get; set; }
        public Vector2 playerPosition;
        public Vector2 playerVelocity;
        Vector2 tempCurrFrame;
        KeyboardState keyState;
        float moveSpeed = 70f;
        const float gravity = 15f;
        float jumpSpeed = 500f;
        bool hasJumped = false;

        Animation playerAnimation = new Animation();
        public void Initialize()
        {
            playerPosition = new Vector2(10, 190);
            playerVelocity = Vector2.Zero;
            playerAnimation.Initialize(playerPosition, new Vector2(4, 2));  // Frames are 4 by 2 //
            tempCurrFrame = Vector2.Zero;
        }
        public void LoadContent(ContentManager Content)
        {
            playerImg = Content.Load<Texture2D>("Boss");
            playerAnimation.image = playerImg;
        }
        public void Update(GameTime gameTime)
        {
            keyState = Keyboard.GetState();
            playerAnimation.active = true;      // Player is active //

            // Moving player to the right //
            if (keyState.IsKeyDown(Keys.Right))
            {
                // Purpose of generalizing speed  of player //
                playerPosition.X += moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                tempCurrFrame.Y = 0;    // First row of sprite sheet //
            }

            // Moving player to the left //
            else if (keyState.IsKeyDown(Keys.Left))
            {
                // Purpose of generalizing speed  of player //
                playerPosition.X -= moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                tempCurrFrame.Y = 1;    // Second row of sprite sheet //
            }
            else
                playerAnimation.active = false;     // Player stopped moving //

            if (keyState.IsKeyDown(Keys.Space) && hasJumped == false)   // Has not jumped yet //
            {
                playerVelocity.Y = -jumpSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                hasJumped = true;
            }
            if (hasJumped == true)
            {
                playerVelocity.Y += gravity * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else
                playerVelocity.Y = 0;

            playerPosition += playerVelocity;   // Position changes as velocity changes //

            hasJumped = playerPosition.Y <= 190;

            playerAnimation.position = playerPosition;  // Set position of player //
            playerAnimation.currentFrame.Y = tempCurrFrame.Y;   // Sets which row of sprite frames to draw //
            playerAnimation.Update(gameTime);   // Update the animation of player //
        }

        public void Draw (SpriteBatch spriteBatch)
        {
            playerAnimation.Draw(spriteBatch);  // Draw frame onto screen //
        }
    }
}
