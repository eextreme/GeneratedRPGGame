using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using GeneratedRPGGame.Core_Mechanics;

namespace GeneratedRPGGame
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        protected GraphicsDeviceManager graphics;
        protected SpriteBatch spriteBatch;
        protected SpriteFont coordinates;
        float[] hitCrit, hitOk;
        int hits = 0, killcount;

        SpriteSheet player, basicTile, advancedTile, monsterTiles;
        Player newPlayer;
        Weapon newWeapon;
        Monster newMonster;        
        CombatInterface ui;
        Vector2 test;
        
        bool keyDownState = false, showAtkCircle = false;

        Texture2D whiteDot, whiteSquare;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            this.graphics.PreferredBackBufferWidth = 800;
            this.graphics.PreferredBackBufferHeight = 800;

            this.graphics.IsFullScreen = false;
            this.IsFixedTimeStep = true;
            this.TargetElapsedTime = new TimeSpan(0, 0, 0, 0, 16);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            //might be useful later normalized vectors

            
            // TODO: Add your initialization logic here

            advancedTile = new SpriteSheet(Content.Load<Texture2D>("Tile Sets/hyptosis-art-batch-1"), 30, 30, graphics.GraphicsDevice);
            
            hitCrit = new float[5] { 140, 200, 260, 300, 340 };
            hitOk = new float[5] { 60, 70, 70, 80, 85 };
            player = new SpriteSheet(Content.Load<Texture2D>("Sprite Sheets/sprites_map_claudius"), 6, 4, graphics.GraphicsDevice);

            newPlayer = new Player(player, 100, 100, 10, 10, 5);
            newWeapon = new Weapon(hitCrit, hitOk, 200, 100f, 10, 300, 20, 1.5f, graphics.GraphicsDevice);

            newPlayer.equipWeapon(newWeapon);
            newPlayer.setSpawnPoint(200, 200);

            newWeapon.atkCircle.LoadContent(Content);
                        
            monsterTiles = new SpriteSheet(Content.Load<Texture2D>("Sprite Sheets/testMonsters"), 10, 10, graphics.GraphicsDevice);

            newMonster = new Monster(monsterTiles.getSpriteAt(0,0), 200, 10, 10, 3 , 1f, 0.6f,0, 100, 1, 1);
            newMonster.spawnMonster(400, 400);

            ui = new CombatInterface(newPlayer.health, newPlayer.mana, newMonster.health, 50, "Monster", "Player", 400);
            ui.update(newMonster.health, newPlayer.health, graphics.GraphicsDevice);

            Collision.drawRect(ref whiteDot, Color.White, 10, 10, graphics.GraphicsDevice);
            Collision.drawRect(ref whiteSquare, Color.White, 20, 20, graphics.GraphicsDevice);
            
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);            

            //newCircle.LoadContent(Content);
            
            coordinates = Content.Load<SpriteFont>("Courier New");

            
            /*hit = Content.Load<SoundEffect>("glass_ping");
            miss = Content.Load<SoundEffect>("flyby-Conor");*/

            // TODO: use this.Content to load your game content here
        }

        public KeyboardState getKey() { return Keyboard.GetState();}
        public MouseState getMouse() { return Mouse.GetState(); }
                
        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        /// 
        protected override void Update(GameTime gameTime)
        {
             // Allows the game to exit

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            if (newWeapon.comboReached(hits) && getKey().IsKeyDown(Keys.LeftShift))
            {
               showAtkCircle = true;
               newWeapon.atkCircle.isEnd = false;
               hits = 0;
             }

             if (showAtkCircle == true)
             {
                 newWeapon.atkCircle.Update(gameTime);
                 if (newWeapon.atkCircle.isEnd)
                 {
                     int[] res = newWeapon.atkCircle.getResults();
                     int modTot = res.Sum();
                     int force = newWeapon.hitForce*(modTot);
                     float damage = newWeapon.attack*newWeapon.multiplier*modTot;

                     getOffSet(newPlayer.playerCenter, newMonster.monsterCenter, Collision.collP,force);
                     newMonster.takeDamage((int)damage, objMod[0], objMod[1]);
                     newWeapon.atkCircle.resetAngle();
                     showAtkCircle = false;
                 }
             }

            
            if (showAtkCircle == false && newPlayer.isAlive)
            {
                //normal update functions
                //Collision.rotate2(newMonster.monsterCenter, ref test, MathHelper.Pi/180*gameTime.ElapsedGameTime.Milliseconds/10);

                newMonster.MoveWithBasicAI(newPlayer.playerCenter);              
                newPlayer.move();

                float rebX = 0, rebY = 0;

                if (getKey().IsKeyDown(Keys.Space) && !keyDownState)
                {
                    keyDownState = true;
                    

                    //change test reference point to a specific location based on the direction the user is facing
            
                    Collision.rotate2(newPlayer.playerCenter, ref test, MathHelper.Pi * gameTime.ElapsedGameTime.Milliseconds / 360, 1);

                    if (Collision.circle(test, 10, newMonster.monsterCenter, newMonster.size))
                    {
                        getOffSet(newPlayer.playerCenter, newMonster.monsterCenter, Collision.collP, newWeapon.hitForce);                        
                        newMonster.takeDamage(newWeapon.attack, objMod[0], objMod[1]);
                        hits++; 
                    }       
                }

                if (getKey().IsKeyUp(Keys.Space))
                {
                    test = newWeapon.centerArc(newPlayer.playerCenter, newPlayer.getPlayerDir);
                    keyDownState = false;
                }


                if (getKey().IsKeyDown(Keys.Enter)) { newMonster.spawnMonster(400, 400); }                            
                
                ui.update(newMonster.health, newPlayer.health, graphics.GraphicsDevice);


                if (monsterHitPlayer())
                {
                    getOffSet(newMonster.monsterCenter, newPlayer.playerCenter, Collision.collP, newMonster.hitForce);
                    newPlayer.takeDamage(newMonster.attack, objMod[0], objMod[1]);
                }


                if (!newMonster.isAlive)
                {
                    killcount++;
                    Random rnd = new Random();
                    Texture2D mon = monsterTiles.getSpriteAt(rnd.Next(0, 9), rnd.Next(0, 9));
                    newMonster = newMonster.genMonster(killcount, mon);
                    newMonster.spawnMonster(rnd.Next(0, 800), rnd.Next(0, 800));
                }

                if (Collision.hitWall(newPlayer.playerCenter, ref rebX, ref rebY))
                {
                    newPlayer.takeDamage(0, rebX, rebY);
                };

                if (Collision.hitWall(newMonster.monsterCenter, ref rebX, ref rebY))
                {
                    newMonster.takeDamage(10, rebX, rebY);
                }
            }


            //newCircle.Update(gameTime);
            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        /// 

        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.SaddleBrown);
            // TODO: Add your drawing code here

            spriteBatch.Begin();
            //map.Draw(spriteBatch

            drawArena();

            if (newPlayer.isAlive)
            {
                if (showAtkCircle == true){ newWeapon.atkCircle.Draw(spriteBatch);}
                if (newMonster.isAlive) { newMonster.Draw(spriteBatch);}

                ui.Draw(spriteBatch);
                spriteBatch.Draw(whiteDot, newPlayer.playerCenter, Color.White);
                spriteBatch.Draw(whiteDot, newMonster.monsterCenter, Color.White);
                spriteBatch.Draw(whiteSquare, test, Color.White);
                //spriteBatch.Draw(whiteDot, new Vector2(400, 400), Color.White);
                                       
            //map.DrawSeperate(spriteBatch, advancedTile);
                newPlayer.Draw(spriteBatch);

                if (keyDownState)
                {
                    keyDownState = false;
                }
            }

            if (!newPlayer.isAlive)
            {
                spriteBatch.DrawString(coordinates, "Game Over, you killed: " + killcount + " monsters", new Vector2(400, 700), Color.Black);
                ui.drawShop(spriteBatch, coordinates, newPlayer, newWeapon);
            }
                        
            spriteBatch.End();

            base.Draw(gameTime);
        }
            
        float[] objMod;
        private void getOffSet(Vector2 origin, Vector2 target, Vector2 collPoint, int force)
        {
            int modX = 0, modY = 0;
            
            double xCent1 = origin.X - target.X;
            double yCent1 = origin.Y - target.Y;

            if (xCent1 < 0) {
                if (yCent1 > 0) { modX = 1; modY = -1; }
                if (yCent1 < 0) { modX = 1; modY = 1;}
            } 
            else if (xCent1 > 0) {
                if (yCent1 > 0) { modX = -1; modY = -1; }
                if (yCent1 < 0) { modX = -1; modY = 1; }
            } 
            else { modX = 0; modY=0; }

            if (yCent1 == 0 && xCent1 < 0) { modX = 1; modY = 0; }
            if (yCent1 == 0 && xCent1 > 0) { modX = -1; modY = 0; }
                        
            if (xCent1 == 0 && yCent1 < 0) { modX = 0; modY = 1; }
            if (xCent1 == 0 && yCent1 > 0) { modX = 0; modY = - 1; }
                                    
            //angle can only be positive or negative doesn't consider the direction the user is facing

            float colAngle = (float) Math.Abs(Math.Atan((collPoint.Y - origin.Y) / (collPoint.X - collPoint.Y)));

            objMod = new float[2] { force *(float) Math.Cos(colAngle)* modX, force * (float) Math.Sin(colAngle)* modY};
            
        }

        private bool playerHitMonster()
        {
            return Collision.circle(newPlayer.playerCenter, newWeapon.weaponRange, newMonster.monsterCenter, newMonster.size);
        }

        private bool weaponHitMonster()
        {
            //Input: end point of the weapon in vectors based on (gameTime)
            //Output: return true if the weapon is inside the monster's hit box

            Vector2 startLoc = newPlayer.getPlayerDir*newWeapon.weaponRange;
            Vector2 endLoc = newWeapon.attackType(0);





            
            return false;
        }

        private bool monsterHitPlayer()
        {
            return Collision.circle(newPlayer.playerCenter, newPlayer.size, newMonster.monsterCenter, newMonster.size);
        }

        private void drawArena()
        {
            Texture2D border = advancedTile.getSpriteAt(22, 7);

            for (int i = 0; i < 800; i+=30)
            {
                spriteBatch.Draw(border, new Vector2(i, 70), Color.White);
                spriteBatch.Draw(border, new Vector2(i, 780), Color.White);
            }

            for (int i = 70; i<800; i+=30)
            {
                spriteBatch.Draw(border, new Vector2(0, i), Color.White);
                spriteBatch.Draw(border, new Vector2(770, i), Color.White);
            }
        }
    }
}
