using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace GeneratedRPGGame.Core_Mechanics
{
    class Weapon
    {
        public Texture2D weaponSprite;
        public float wepOffsetX, wepOffsetY;
        public CreateAttackCircle atkCircle;
        float scaleFactor;
        int combo;

        public Weapon(Texture2D weap, float[] hitCritL, float[] hitOkL, int r, GraphicsDevice device, float scaleF, int combos)
        {
            weaponSprite = weap;
            atkCircle = new CreateAttackCircle(hitCritL, hitOkL, 200, device);
            wepOffsetX = weaponSprite.Width * scaleF;
            wepOffsetY = weaponSprite.Height * scaleF;
            combo = combos;
        }

        public void setOffSet(int x, int y)
        {
            wepOffsetX = x;
            wepOffsetY = y;
        }

        public Boolean comboReached(int hits)
        {
            return hits >= combo;
        }
        

    }
}
