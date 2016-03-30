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
        Vector2 wepOffSet, wepEndLoc;

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

        public Vector2 useWeapon(Vector2 origin, Vector2 dir)
        {
            Vector2 changeDir = new Vector2();

            if (dir == Collision.north)
                changeDir = Collision.west;
            else if (dir == Collision.east)
                changeDir = Collision.north;
            else if (dir == Collision.south)
                changeDir = Collision.east;
            else if (dir == Collision.west)
                changeDir = Collision.south;
            
            wepEndLoc = origin+weaponRange * changeDir;
            return wepEndLoc;
        }

        public Vector2 fullArc(Vector2 origin, Vector2 dir)
        {
            Vector2 changeDir = new Vector2();

            if (dir == Collision.north) changeDir = Collision.west;
            else if (dir == Collision.east) changeDir = Collision.north;
            else if (dir == Collision.south) changeDir = Collision.east;
            else if (dir == Collision.west) changeDir = Collision.south;

            wepEndLoc = origin + weaponRange * changeDir;
            return wepEndLoc;
        }

        public Vector2 halfArc(Vector2 origin, Vector2 dir)
        {
            wepEndLoc = origin + weaponRange * dir;
            return wepEndLoc;
        }

        public Vector2 centerArc(Vector2 origin, Vector2 dir)
        {
            Vector2 changeDir = new Vector2();

            if (dir == Collision.north) changeDir = Collision.nw;
            else if (dir == Collision.east) changeDir = Collision.ne;
            else if (dir == Collision.south) changeDir = Collision.se;
            else if (dir == Collision.west) changeDir = Collision.sw;

            wepEndLoc = origin + weaponRange * changeDir;
            return wepEndLoc;
        }


       
        
        public void Draw(SpriteBatch spriteBatch, Vector2 origin, Vector2 end)
        {
            showAngle = (float)Math.Atan2(end.Y - origin.Y, end.X - origin.X);
            //sprite.Draw(weaponSprite, p.playerCenter(), new Rectangle(0,0, weaponSprite.Width, weaponSprite.Height), Color.White, showAngle, new Vector2(0, 0), SpriteEffects.None, 1f);
            spriteBatch.Draw(weaponSprite, origin, null, Color.White, showAngle, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            
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

        public Vector2 attackType(int t)
        {
            return Vector2.Zero;
        }

        public bool checkOffset(Vector2 a, Vector2 b)
        {
            float dist = Vector2.Distance(a, b);

            return (dist > weaponRange || dist < weaponRange);
            
        }


    }
}
