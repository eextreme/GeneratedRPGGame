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
        int wepOffsetX, wepOffsetY;
        public CreateAttackCircle atkCircle;
        public int combo, hitForce, attack;
        float showAngle;
        Rectangle weapLoc;
        int playerPosX, playerPosY;

        public Weapon(float[] hitCritL, float[] hitOkL, int r, GraphicsDevice device, int weaponWidth, int weaponHeight, int combos, int force, int atk)
        {
            atkCircle = new CreateAttackCircle(hitCritL, hitOkL, 200, device);
            wepOffsetX = weaponWidth;
            wepOffsetY = weaponHeight;
            combo = combos; hitForce = force; attack = atk;

            weaponSprite = new Texture2D(device, weaponWidth, weaponHeight);
            Color[] line = new Color[weaponWidth*weaponHeight];
            
            for (int i=0; i<line.Length;i++)
                line[i]=Color.Black;

            weaponSprite.SetData(line);
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

        
        public Rectangle stabWeapon(int pPosX, int pPosY, String dir, Player p)
        {
            int offsetX = 0, offsetY = 0;
            playerPosX = pPosX; playerPosY = pPosY;

            if (dir == "Up") { offsetY -= wepOffsetY; showAngle = 3 * MathHelper.PiOver2; }
            if (dir == "Right") { offsetX += wepOffsetX; showAngle = 0;}
            if (dir == "Down") { offsetY+= wepOffsetY; showAngle = MathHelper.PiOver2; }
            if (dir == "Left") { offsetX-= wepOffsetX; showAngle = MathHelper.Pi; }                      
                        
            weapLoc = new Rectangle(pPosX, pPosY, p.xFrames/2+offsetX, p.yFrames/2+offsetY);
            return weapLoc;
        }

        public void Draw(SpriteBatch sprite, Player p)
        {
            sprite.Draw(weaponSprite, new Rectangle(playerPosX+p.xFrames/2, playerPosY+p.yFrames/2, weaponSprite.Width, weaponSprite.Height), Rectangle.Empty, Color.White, showAngle, new Vector2(0, 0), SpriteEffects.None, 1f);
        }
        

    }
}
