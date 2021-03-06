﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace CSCI_2941_Lab5
{
    class HitBox
    {
        public Rectangle playerHB = new Rectangle();
        Color color = Color.Red;
        public void HBwidth(int width)
        {
            playerHB.Width = width;
        }
        public void HB(Vector2 pos, Vector2 frame)
        {
            playerHB.Height = Convert.ToInt32(frame.Y * 2);
            playerHB.Width = Convert.ToInt32(frame.X * 1.5);
            playerHB.X = Convert.ToInt32(pos.X);
            playerHB.Y = Convert.ToInt32(pos.Y);
        }
        public void update(Vector2 pos)
        {
            playerHB.X = Convert.ToInt32(pos.X);
            playerHB.Y = Convert.ToInt32(pos.Y);
        }

        public void HBColor(Color prettyColor)
        {
            color = prettyColor;
        }

        public void Draw(SpriteBatch spritebatch)
        {
            // http://stackoverflow.com/questions/2795741/displaying-rectangles-in-game-window-with-xna
            var t = new Texture2D(spritebatch.GraphicsDevice, 1, 1);
            t.SetData(new[] { Color.White });
            int bw = 2; // Border width

            spritebatch.Draw(t, new Rectangle(playerHB.Left, playerHB.Top, bw, playerHB.Height), color); // Left
            spritebatch.Draw(t, new Rectangle(playerHB.Right, playerHB.Top, bw, playerHB.Height), color); // Right
            spritebatch.Draw(t, new Rectangle(playerHB.Left, playerHB.Top, playerHB.Width, bw), color); // Top
            spritebatch.Draw(t, new Rectangle(playerHB.Left, playerHB.Bottom, playerHB.Width, bw), color); // Bottom
        }

    }
}
