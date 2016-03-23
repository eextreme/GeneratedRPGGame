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

        private KeyboardState getKey() { return Keyboard.GetState(); }
        int sel = 1, atkUpgCount=0, healthUpgCount=0, speedUpgCount=0, defenceUpgCount=0, multiUpgCount=0;
        public void drawShop(SpriteBatch spriteBatch, SpriteFont coordinates, Player p, Weapon w)
        {
            spriteBatch.DrawString(coordinates, "By your upgrades here", Vector2.Zero, Color.Black);
            spriteBatch.DrawString(coordinates, "Increase Weapon Damage: +" + atkUpgCount, new Vector2(10, 10), Color.Black);
            spriteBatch.DrawString(coordinates, "Increase Health: +" + healthUpgCount, new Vector2(10, 20), Color.Black);
            spriteBatch.DrawString(coordinates, "Increase Movement Speed: " + speedUpgCount, new Vector2(10, 30), Color.Black);
            spriteBatch.DrawString(coordinates, "Increase Defence: +" + defenceUpgCount, new Vector2(10, 40), Color.Black);
            spriteBatch.DrawString(coordinates, "Increase Super Attack Damage Multiplier: +" + multiUpgCount, new Vector2(10, 50), Color.Black);

            spriteBatch.DrawString(coordinates, ">", new Vector2(0, sel*10), Color.Black);

            if (!getKey().IsKeyUp(Keys.Up)) { sel -= 1; }
            if (!getKey().IsKeyUp(Keys.Down)) { sel += 1; }

            if (sel > 5 || sel < 1) { sel = 1; }

            if (!getKey().IsKeyUp(Keys.Enter))
                buyUpgrade(sel, p, w);
            
            if (getKey().IsKeyDown(Keys.Escape))
            {
                p.health = 100;
            }
        }
                
        private void buyUpgrade(int sel, Player p, Weapon w)
        {
            if (sel == 1) { w.attack+=10; atkUpgCount++; }
            if (sel == 2) { p.health+=100; healthUpgCount++; }
            if (sel == 3) { p.speed+=1; speedUpgCount++; }
            if (sel == 4) { p.defence += 10; defenceUpgCount++; }
            if (sel == 5) { w.multiplier+=0.2f; multiUpgCount++; } 
        }

    }
}
