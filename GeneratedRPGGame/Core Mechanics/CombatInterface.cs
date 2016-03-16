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
    
    class CombatInterface
    {
        double hpBar1, mpBar1, staBar1;
        double hpBar2, mpBar2, staBar2;
        String playerName, targetName;
        Texture2D phpMeter, mhpMeter;
        double phpPercent, mhpPercent;
        double bLength;

        public CombatInterface(int hp, int mp, int monHp, int monMP, String pName, String mName, int barLength)
        {
            hpBar1 = hp; mpBar1 = mp; hpBar2 = monHp; mpBar2 = monMP;
            playerName = pName; targetName = mName;
            bLength = barLength;
        }

        public void update(int monsterHP, int playerHP, GraphicsDevice d)
        {
            phpPercent = playerHP / hpBar1;
            mhpPercent = monsterHP / hpBar2;

            int phpL = (int) (bLength * phpPercent);
            int mhpL = (int) (bLength * mhpPercent);

            if (phpL <= 0 || mhpL <= 0) { phpL = 1; mhpL = 1; }
            
            phpMeter = new Texture2D(d, phpL, 30);
            mhpMeter = new Texture2D(d, mhpL, 30);

            Color[] php = new Color[phpL * 30];
            Color[] mhp = new Color[mhpL * 30];

            for (int i = 0; i < php.Length; i++)
                php[i] = Color.Green;

            for (int i = 0; i < mhp.Length; i++)
                mhp[i] = Color.Red;

            phpMeter.SetData(php); mhpMeter.SetData(mhp);

        }

        public void Draw(SpriteBatch sprite)
        {
            sprite.Draw(phpMeter, new Rectangle(0, 0, phpMeter.Width, phpMeter.Height), Color.White);
            sprite.Draw(mhpMeter, new Rectangle(0, 30, mhpMeter.Width, mhpMeter.Height), Color.White);
        }

    }
}
