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
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont coordinates;
        float[] hitCrit, hitOk;
        int hits = 0, killcount;       
        
        Player newPlayer;
        Weapon newWeapon;
        Monster newMonster;
        TileSets basicTile, advancedTile, monsterTiles;
        GenerateMap map;
        CombatInterface ui;
        
        String indicator="";
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
            /*circle = drawCircle(300);
            line = createLine(150, Color.Red);
            current = 0;

            
            indicator = "Press and hold space to stop";


            hitAng = new float[3] { 0, 0, 0 };

            createAttackCircle(3, hitCrit, hitOk);

            screenCenter = new Vector2(400, 400);
            imageCenter = new Vector2(circle.Width / 2f, circle.Height / 2f);
            imageCenter2 = new Vector2(line.Width, line.Height);
            imageCenter3 = new Vector2(line.Width, line.Height + 150);*/
            
            hitCrit = new float[5] { 140, 200, 260, 300, 340 };
            hitOk = new float[5] { 60, 70, 70, 80, 85 };

            newPlayer = new Player(Content.Load<Texture2D>("Sprite Sheets/sprites_map_claudius"), 6, 4, 100, 100);
            newWeapon = new Weapon(hitCrit, hitOk, 200, graphics.GraphicsDevice, 100f, 10, 100, 10);

            newPlayer.equipWeapon(newWeapon);
            newPlayer.setSpawnPoint(200, 200);

            newWeapon.atkCircle.LoadContent(Content);
                        
            basicTile = new TileSets(Content.Load<Texture2D>("Tile Sets/part1_tileset"), 4, 1, graphics.GraphicsDevice);
            advancedTile = new TileSets(Content.Load<Texture2D>("Tile Sets/hyptosis-art-batch-1"), 30, 30, graphics.GraphicsDevice);
            monsterTiles = new TileSets(Content.Load<Texture2D>("Sprite Sheets/testMonsters"), 10, 10, graphics.GraphicsDevice);

            newMonster = new Monster(monsterTiles.getTexture(0), 200, 10, 10, 3, 1f, 0.6f,0, 100, 1, 1);
            newMonster.spawnMonster(400, 400);

            ui = new CombatInterface(newPlayer.health, newPlayer.mana, newMonster.health, 50, "Monster", "Player", 400);
            ui.update(newMonster.health, newPlayer.health, graphics.GraphicsDevice);

            map = new GenerateMap(800, 800, advancedTile);

            whiteDot = new Texture2D(graphics.GraphicsDevice, 10, 10);
            Color[] c = new Color[10*10];
            
            for (int i=0; i < c.Length;i++)
            {
                c[i]=Color.White;
            }

            whiteDot.SetData(c);

                       //target = Content.Load<Texture2D>("simple circle");
            
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

                    if (checkCircleCollision(newMonster.monsterCenter(), 60, newPlayer.playerCenter(), 60))
                    {
                        getOffSet(newPlayer.playerCenter(), newMonster.monsterCenter(), new Vector2(collPointX, collPointY), newWeapon.hitForce);
                        newMonster.takeDamage(10, objMod[0], objMod[1]);
                        hits++; 
                    }       
                }

                if (getKey().IsKeyUp(Keys.Space))
                    keyDownState = false;


                if (getKey().IsKeyDown(Keys.Enter)) { newMonster.spawnMonster(400, 400); }                            
                
                ui.update(newMonster.health, newPlayer.health, graphics.GraphicsDevice);

                if (checkCircleCollision(newMonster.monsterCenter(), 60, newPlayer.playerCenter(), 40))
                {
                    getOffSet(newMonster.monsterCenter(), newPlayer.playerCenter(), new Vector2(collPointX, collPointY), newMonster.hitForce);
                    newPlayer.takeDamage(newMonster.attack, objMod[0], objMod[1]);
                }


                if (!newMonster.alive())
                {
                    killcount++;
                    Random rnd = new Random();
                    newMonster = new Monster(monsterTiles.getTexture(rnd.Next(0, monsterTiles.listSize)), 200, 10, 10, 3, 80, 20,0,100, 1, 1);
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
            if (showAtkCircle == true){ newWeapon.atkCircle.Draw(spriteBatch);}
            if (newMonster.alive()) { newMonster.Draw(spriteBatch);}

            ui.Draw(spriteBatch);
            
            spriteBatch.Draw(whiteDot, newPlayer.playerCenter(), Color.White);
            spriteBatch.Draw(whiteDot, newMonster.monsterCenter(), Color.White);
            spriteBatch.Draw(whiteDot, new Vector2(collPointX, collPointY), Color.Black);


                       
            //map.DrawSeperate(spriteBatch, advancedTile);
            newPlayer.Draw(spriteBatch);

            if (keyDownState)
            {
                newWeapon.Draw(spriteBatch, newPlayer);
                keyDownState = false;
            }


            if (!newPlayer.isAlive())
                spriteBatch.DrawString(coordinates, "Game Over, you killed: " + killcount + " monsters" , new Vector2(400, 700), Color.Black);
            
            spriteBatch.End();

            base.Draw(gameTime);
        }

        public Boolean checkCollision(Rectangle a, Rectangle b)
        {
            return a.Intersects(b);
        }

        int collPointX, collPointY;
        
        private Boolean checkCircleCollision(Vector2 a, float radiusA, Vector2 b, float radiusB)
        {
            float dist = Vector2.Distance(a, b);
            float dist2 = radiusA + radiusB;

            float cpX = ((a.X * radiusB) + (b.X * radiusA)) / (radiusA + radiusB);
            float cpY = ((a.Y * radiusB) + (b.Y * radiusA)) / (radiusA + radiusB);

            collPointX = (int) cpX; collPointY = (int) cpY;

            return (dist2 > dist);
        }

        int [] objMod;
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
            double angleObj1 = Math.Atan(yCent1/xCent1);
                
            double xCent2 = Math.Abs(target.X - collPoint.X);
            double yCent2 = Math.Abs(target.Y - collPoint.Y);

            double angleObj2 = Math.Atan(yCent2/xCent2);

            objMod = new int[2] { (int)(force * modX), (int)(force * modY)};
            
        }

        


    }
}
