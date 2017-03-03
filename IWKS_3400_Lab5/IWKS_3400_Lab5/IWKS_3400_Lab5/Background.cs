using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace IWKS_3400_Lab5
{
    class Background
    {
        public Texture2D texture;
        public Rectangle rectangle;

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, rectangle, Color.White);
        }
    }

    class Scrolling : Background
    {
        public Scrolling(Texture2D newTexture, Rectangle newRect)
        {
            texture = newTexture;
            rectangle = newRect;
        }

        public void Update(SpriteBatch spriteBatch)
        {
            rectangle.X -= 3;   // Move to the left //
        }
    }
}
