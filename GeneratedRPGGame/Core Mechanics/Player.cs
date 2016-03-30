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
    class Player
    {
        public int xFrames, yFrames, size;
        Vector2 position, centerPos;
        public float speed;
        public int yRef, frame = 0, numOfFrames, health, mana, defence;
        
        //used to determin the center of the sprite
        
        public Texture2D sprite { get; set; }
        SpriteSheet spriteSheet;
        Weapon weap;
        Vector2 curLoc { get; set; }
        Rectangle curAnimation { get; set;}
        Vector2 playerDir, angleVec;
        
        public Player(Texture2D playerSprite, int x, int y, int hp, int mp, int def, float spd)
        {
            sprite = playerSprite;
            xFrames = playerSprite.Width / x;
            yFrames = playerSprite.Height / y;
            numOfFrames = y;

            size = (playerSprite.Width + playerSprite.Height) / 2;

            health = hp; mana = mp;
            centerPos = Vector2.Zero;
            speed = spd; defence = def;
        }

        public Player(SpriteSheet playerSpriteSheet, int hp, int mp, int def, float spd, int frames)
        {
            spriteSheet = playerSpriteSheet;
            health = hp; mana = mp; defence = def; speed = spd;
            numOfFrames = frames;
            centerPos = Vector2.Zero;
        }

        public void equipWeapon(Weapon w) { weap = w;}
        
        public void setSpawnPoint (int x, int y) {position.X = x; position.Y = y;}
        private KeyboardState getKey() {return Keyboard.GetState();}

        public void move()
        {
            Vector2 playerDirMod = Vector2.Zero;

            if (getKey().IsKeyDown(Keys.Down))
            { yRef = 0; frame++; playerDir = Collision.south; playerDirMod += playerDir; }            

            if (getKey().IsKeyDown(Keys.Left))
            { yRef = 1; frame++; playerDir = Collision.west; playerDirMod += playerDir; }

            if (getKey().IsKeyDown(Keys.Up))
            { yRef = 2; frame++; playerDir = Collision.north; playerDirMod += playerDir; }

            if (getKey().IsKeyDown(Keys.Right))
            { yRef = 3; frame++; playerDir = Collision.east;  playerDirMod += playerDir; }

            if (frame > numOfFrames) { frame = 0; }

            position += speed * playerDirMod;

            angleVec = playerDirMod;
            
            sprite = spriteSheet.getSpriteAt(frame,yRef);

            centerPos = position + new Vector2(sprite.Width/2, sprite.Height/2);
        }
        
        public void Draw(SpriteBatch spriteBatch)
        {            
            spriteBatch.Draw(this.sprite, position, Color.White);
        }

        public Vector2 getPlayerDir
        {
            get { return playerDir; }
        }

        public void takeDamage(int dmg, float x, float y)
        {
            position += new Vector2(x, y);
            health -= dmg;
        }

        public Boolean isAlive
        {
            get { return health > 0; }
        }

        public Vector2 playerCenter
        {
            get { return centerPos; }
        }

        public Color[] getColor()
        {
                Color[] data = new Color[sprite.Width * sprite.Height];
                sprite.GetData<Color>(data);
                return data;
        }

        public Matrix getPlayerWorld()
        {
            return Matrix.CreateTranslation(new Vector3(playerCenter, 0f));
        }
    }
}
