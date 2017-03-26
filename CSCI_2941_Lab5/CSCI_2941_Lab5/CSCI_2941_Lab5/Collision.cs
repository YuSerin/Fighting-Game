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
    class Collision
    {
        int HealthEffect = 0;
        int blockKick = 7;
        int blockPunch = 5;
        int hitKick = 35;
        int hitPunch = 25;
        int dualAttack = 15;

        public int TestCollision(Rectangle HB1, Rectangle HB2, int State1, int State2)
        {
            if (HB1.Intersects(HB2))
            {
                if (State1 == (int)Sprite.Block)
                {
                    if (State2 == (int)Sprite.Mid_Punch)
                        HealthEffect = blockPunch;
                    else if (State2 == (int)Sprite.Kick)
                        HealthEffect = blockKick;
                    else
                        HealthEffect = 0;
                }
                else if (State1 == (int)Sprite.Mid_Punch && State2 == (int)Sprite.Mid_Punch)
                    HealthEffect = dualAttack;
                else if (State1 == (int)Sprite.Kick && State2 == (int)Sprite.Kick)
                    HealthEffect = dualAttack;
                else
                {
                    if (State2 == (int)Sprite.Mid_Punch)
                        HealthEffect = hitPunch;
                    else if (State2 == (int)Sprite.Kick)
                        HealthEffect = hitKick;
                    else
                        HealthEffect = 0;
                }
            }
            return HealthEffect;
        }
    } 
}
