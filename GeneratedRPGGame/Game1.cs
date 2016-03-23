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
        
        bool keyDownState = false, showAtkCircle = false;

        Texture2D whiteDot;   

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
            // TODO: Add your initialization logic here
            
            hitCrit = new float[5] { 140, 200, 260, 300, 340 };
            hitOk = new float[5] { 60, 70, 70, 80, 85 };
            player = new SpriteSheet(Content.Load<Texture2D>("Sprite Sheets/sprites_map_claudius"), 6, 4, graphics.GraphicsDevice);

            newPlayer = new Player(player, 100, 100, 10, 10, 5);
            newWeapon = new Weapon(hitCrit, hitOk, 200, 100f, 10, 100, 10, 1.5f, graphics.GraphicsDevice);

            newPlayer.equipWeapon(newWeapon);
            newPlayer.setSpawnPoint(200, 200);

            newWeapon.atkCircle.LoadContent(Content);
                        
            monsterTiles = new SpriteSheet(Content.Load<Texture2D>("Sprite Sheets/testMonsters"), 10, 10, graphics.GraphicsDevice);

            newMonster = new Monster(monsterTiles.getSpriteAt(0,0), 200, 10, 10, 3, 1f, 0.6f,0, 100, 1, 1);
            newMonster.spawnMonster(400, 400);

            ui = new CombatInterface(newPlayer.health, newPlayer.mana, newMonster.health, 50, "Monster", "Player", 400);
            ui.update(newMonster.health, newPlayer.health, graphics.GraphicsDevice);

            whiteDot = new Texture2D(graphics.GraphicsDevice, 10, 10);
            Color[] c = new Color[10*10];
            
            for (int i=0; i < c.Length;i++)
            {
                c[i]=Color.White;
            }

            whiteDot.SetData(c);
            
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
        Vector2 Test = new Vector2();
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
                        showAtkCircle = false;
             }

            if (showAtkCircle == false && newPlayer.isAlive())
            {
                //normal update functions
                newMonster.MoveWithBasicAI((int) newPlayer.playerCenter().X, (int) newPlayer.playerCenter().Y);              
                newPlayer.move();

                if (getKey().IsKeyDown(Keys.Space) && !keyDownState)
                {
                    keyDownState = true;
                    newWeapon.stabWeapon(newPlayer.getPlayerDir());
                    Rectangle wepRec = newWeapon.boundingRect(newPlayer);

                    if (playerHitMonster())
                    {
                        getOffSet(newPlayer.playerCenter(), newMonster.monsterCenter(), Collision.collP, newWeapon.hitForce);
                        newMonster.takeDamage(newWeapon.attack, objMod[0], objMod[1]);
                        hits++; 
                    }       
                }

                if (getKey().IsKeyUp(Keys.Space))
                    keyDownState = false;


                if (getKey().IsKeyDown(Keys.Enter)) { newMonster.spawnMonster(400, 400); }                            
                
                ui.update(newMonster.health, newPlayer.health, graphics.GraphicsDevice);


                if (monsterHitPlayer())
                {
                    getOffSet(newMonster.monsterCenter(), newPlayer.playerCenter(), Collision.collP, newMonster.hitForce);
                    newPlayer.takeDamage(newMonster.attack, objMod[0], objMod[1]);
                }


                if (!newMonster.alive())
                {
                    killcount++;
                    Random rnd = new Random();
                    Texture2D mon = monsterTiles.getSpriteAt(rnd.Next(0, 9), rnd.Next(0, 9));
                    newMonster = newMonster.genMonster(killcount, mon);
                    newMonster.spawnMonster(rnd.Next(0, 800), rnd.Next(0, 800));
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
            graphics.GraphicsDevice.Clear(Color.White);
            // TODO: Add your drawing code here

            spriteBatch.Begin();
            //map.Draw(spriteBatch);

            if (newPlayer.isAlive())
            {
                if (showAtkCircle == true){ newWeapon.atkCircle.Draw(spriteBatch);}
                if (newMonster.alive()) { newMonster.Draw(spriteBatch);}

                ui.Draw(spriteBatch);
                spriteBatch.Draw(whiteDot, newPlayer.playerCenter(), Color.White);
                spriteBatch.Draw(whiteDot, newMonster.monsterCenter(), Color.White);
                spriteBatch.Draw(whiteDot, Collision.collP, Color.Black);
                                       
            //map.DrawSeperate(spriteBatch, advancedTile);
                newPlayer.Draw(spriteBatch);

                if (keyDownState)
                {
                    newWeapon.Draw(spriteBatch, newPlayer);
                    keyDownState = false;
                }
            }

            if (!newPlayer.isAlive())
            {
                spriteBatch.DrawString(coordinates, "Game Over, you killed: " + killcount + " monsters", new Vector2(400, 700), Color.Black);
                ui.drawShop(spriteBatch, coordinates, newPlayer, newWeapon);
            }
                        
            spriteBatch.End();

            base.Draw(gameTime);
        }
            
        public Boolean checkCollision(Rectangle a, Rectangle b)
        {
            return a.Intersects(b);
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
            return Collision.circle(newPlayer.playerCenter(), newWeapon.weaponRange, newMonster.monsterCenter(), newMonster.size);
        }

        private bool monsterHitPlayer()
        {
            return Collision.circle(newPlayer.playerCenter(), newPlayer.size, newMonster.monsterCenter(), newMonster.size);
        }
    }
}
