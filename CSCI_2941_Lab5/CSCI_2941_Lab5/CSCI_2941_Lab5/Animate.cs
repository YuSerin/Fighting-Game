using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

// Credit: Oyyou - Youtuber // 
namespace CSCI_2941_Lab5
{
    class Animate
    {
        public Texture2D texture { get; set; }
        public int FrameHeight { get { return texture.Height; } }
        public int FrameWidth;
        public float FrameTime { get; }
        public int FrameCount;
        public bool isLooping { get; }

        public Animate(Texture2D newTexture, int newFrameWidth, float newFrameTime, bool newIsLooping)
        {
            texture = newTexture;
            FrameWidth = newFrameWidth;
            FrameTime = newFrameTime;
            isLooping = newIsLooping;
            FrameCount = texture.Width / FrameWidth;
        }
    }
}
