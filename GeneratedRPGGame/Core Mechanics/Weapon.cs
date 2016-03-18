﻿using System;
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
        public float weaponRange;
        public CreateAttackCircle atkCircle;
        public int combo, hitForce, attack;
        float showAngle;

        public Weapon(float[] hitCritL, float[] hitOkL, int r, GraphicsDevice device, float weaponR, int combos, int force, int atk)
        {
            atkCircle = new CreateAttackCircle(hitCritL, hitOkL, 200, device);
            
            combo = combos; hitForce = force; attack = atk;
            weaponRange = weaponR;
            
            weaponSprite = new Texture2D(device, (int) weaponR, 20);
            Color[] line = new Color[(int) weaponR*20];
            
            for (int i=0; i<line.Length;i++)
                line[i]=Color.Black;

            weaponSprite.SetData(line);
        }

        public Boolean comboReached(int hits)
        {
            return hits >= combo;
        }

        public bool stabWeapon(String dir)
        {
            if (dir == "Left") { showAngle = MathHelper.Pi; }
            if (dir == "Up") { showAngle = 3*MathHelper.PiOver2; }
            if (dir == "Right") { showAngle = 0; }
            if (dir == "Down") { showAngle = MathHelper.PiOver2; }
            
            return false;
        }

        public bool leftQuarterCircle(int pPosX, int pPosY, String dir)
        {
            return false;
        }

        public bool rightQuarterCircle(int pPosX, int pPosY, String dir)
        {
            return false;
        }

        public bool leftHalfCircle(int pPosX, int pPosY, String dir)
        {
            return false;
        }

        public bool rightHalfCircle(int pPosX, int pPosY, String dir)
        {
            return false;
        }
        
        public void Draw(SpriteBatch sprite, Player p)
        {
            //sprite.Draw(weaponSprite, p.playerCenter(), new Rectangle(0,0, weaponSprite.Width, weaponSprite.Height), Color.White, showAngle, new Vector2(0, 0), SpriteEffects.None, 1f);
            sprite.Draw(weaponSprite, p.playerCenter(), new Rectangle(0, 0, weaponSprite.Width, weaponSprite.Height), Color.White, showAngle, Vector2.Zero, 1f, SpriteEffects.None, 1f);
        }
        

    }
}
