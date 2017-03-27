using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CSCI_2941_Lab5
{
    class healthBar
    {
        Rectangle playerHealth = new Rectangle();
        Color barColor;

        public void health(Vector2 pos, int frame, Color color)
        {
            playerHealth.Height = 30;
            playerHealth.Width = frame;
            playerHealth.X = Convert.ToInt32(pos.X);
            playerHealth.Y = Convert.ToInt32(pos.Y);
            barColor = color;
        }

        public void update(int newHealth)
        {
            playerHealth.Width = newHealth;
        }

        public void Draw(SpriteBatch spritebatch)
        {
            // http://stackoverflow.com/questions/2795741/displaying-rectangles-in-game-window-with-xna
            var t = new Texture2D(spritebatch.GraphicsDevice, 1, 1);
            t.SetData(new[] { Color.White });

            spritebatch.Draw(t, playerHealth, barColor);
        }

    }
}
