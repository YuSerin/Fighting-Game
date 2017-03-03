using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IWKS_3400_Lab5
{
    public class Camera
    {
        public Vector2 position;    // Position of camera //
        public Matrix viewMatrix { get; set; }  // Camera viewport //
        public int screenWidth { get; set; }
        public int screenHeight { get; set; }

        public Camera(int setScreenW, int setScreenH)
        {
            screenWidth = setScreenW;
            screenHeight = setScreenH;
        }
        // Camera affected by position of the player //
        public void Update(Vector2 playerPosition)
        {
            // Camera will only move when the player has reached the center of the screen //
            position.X = playerPosition.X - (screenWidth / 4);
            position.Y = playerPosition.Y - (screenHeight / 4);

            if (position.X < 0)
                position.X = 0;
            if (position.Y < playerPosition.Y)
                position.Y = 0;

            // Moves all game elements relative to the player position //
            // Scroll to the left of the screen //
            viewMatrix = Matrix.CreateTranslation(new Vector3(-position, 0));
        }
       
    }
}
