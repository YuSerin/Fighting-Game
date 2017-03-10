using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CSCI_2941_Lab5
{
    struct AnimationPlayer
    {
        public Animate animation { get; set; }
        public int frameIndex { get; set; }
        private float timer;

        public void PlayAnimation(Animate newAnimation)
        {
            if (animation == newAnimation)
                return;     // Do nothing //

            animation = newAnimation;
            frameIndex = 0;
            timer = 0;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch,
            Vector2 position)
        {
            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            while (timer >= animation.FrameTime)
            {
                timer -= animation.FrameTime;

                if (animation.isLooping)
                    frameIndex = (frameIndex + 1) % animation.FrameCount;
                else
                    frameIndex = Math.Min(frameIndex + 1, animation.FrameCount - 1);

                Rectangle rectangle = new Rectangle(frameIndex * animation.FrameWidth, 0,
                    animation.FrameWidth, animation.FrameHeight);

                spriteBatch.Draw(animation.texture, position, rectangle, Color.White);
            }
        }
    }
}
