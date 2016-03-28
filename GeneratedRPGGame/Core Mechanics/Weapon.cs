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
        public float weaponRange;
        public CreateAttackCircle atkCircle;
        public int combo, hitForce, attack;
        float showAngle;
        public float multiplier;
        public Color[] line;
        Vector2 wepOffSet;

        public Weapon(float[] hitCritL, float[] hitOkL, int r, float weaponR, int combos, int force, int atk, float multi, GraphicsDevice dev)
        {
            atkCircle = new CreateAttackCircle(hitCritL, hitOkL, 200, dev);
            
            combo = combos; hitForce = force; attack = atk;
            weaponRange = weaponR; multiplier = multi;
            
            weaponSprite = new Texture2D(dev, (int) weaponR, 10);
            line = new Color[(int) weaponR*10];
            
            for (int i=0; i<line.Length;i++)
                line[i]=Color.Black;

            wepOffSet = new Vector2(10 / 2, 10 / 2);

            weaponSprite.SetData(line);
        }

        public Boolean comboReached(int hits)
        {
            return hits >= combo;
        }

        public bool useWeapon(Vector2 dir)
        {
            showAngle = Vector2.Dot(new Vector2(0, 1), dir*MathHelper.PiOver2);
            if (dir.Equals(Collision.west))
                showAngle = MathHelper.Pi;

            return false;
        }

        public bool leftQuarterCircle(Vector2 origin, Vector2 target)
        {
            if (Collision.isFacing(origin, target) !=0)
                return true;

            return false;
        }

        public bool rightQuarterCircle(Vector2 origin, Vector2 target)
        {
            if (origin.X < target.X && origin.Y < target.Y)
                return true;
            return false;
        }

        public bool leftHalfCircle(Vector2 origin, Vector2 target)
        {
            return false;
        }

        public bool rightHalfCircle(Vector2 origin, Vector2 target)
        {
            return false;
        }
        
        public void Draw(SpriteBatch spriteBatch, Player p)
        {
            //sprite.Draw(weaponSprite, p.playerCenter(), new Rectangle(0,0, weaponSprite.Width, weaponSprite.Height), Color.White, showAngle, new Vector2(0, 0), SpriteEffects.None, 1f);
            spriteBatch.Draw(weaponSprite, p.playerCenter, new Rectangle(0, 0, weaponSprite.Width, weaponSprite.Height), Color.White, showAngle, wepOffSet, 1f, SpriteEffects.None, 1f);
        }

        public Rectangle boundingRect(Player P)
        {
            //Vector2 original = new Vector2(weaponSprite.Width, weaponSprite.Height);
            Vector2 original = P.playerCenter;

            var rotation = Matrix.CreateRotationZ(showAngle);
            var translateTo = Matrix.CreateTranslation(new Vector3(0, weaponSprite.Height/2, 0));
            var combined = translateTo * rotation;
            Vector2 rotatedVector = Vector2.Transform(original, combined);

            return Collision.vec2Rec(rotatedVector);
        }


    }
}
