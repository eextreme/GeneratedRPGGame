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

namespace GeneratedRPGGame
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        float[] hitCrit, hitOk;
        int x = 400, y = 400, xFrame, yFrame;
        CreateAttackCircle newCircle;
        GenerateMap map;
        Texture2D person;

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

            newCircle = new CreateAttackCircle(hitCrit, hitOk, 300, graphics.GraphicsDevice);
            map = new GenerateMap(10, 10, Content);

            person = Content.Load<Texture2D>("sprites_map_claudius");
            xFrame = person.Width / 6;
            yFrame = person.Height / 4;

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
            
            /*
            coordinates = Content.Load<SpriteFont>("Courier New");
            hit = Content.Load<SoundEffect>("glass_ping");
            miss = Content.Load<SoundEffect>("flyby-Conor");*/

            // TODO: use this.Content to load your game content here
        }

        public KeyboardState getKey()
        {
            return Keyboard.GetState();
        }

        int i = 0;
        public void movingNorth()
        {  
            if (getKey().IsKeyDown(Keys.Up)) 
            {
                Rectangle curLoc = new Rectangle(x,y,xFrame, yFrame);
                Rectangle curAnimation = new Rectangle(xFrame * i, 2*yFrame, xFrame, yFrame);
                spriteBatch.Draw(person,curLoc, curAnimation, Color.White);
                i++; y = y - 10;
                
                if (i>6)
                {
                    i = 0;
                }

                graphics.GraphicsDevice.Clear(Color.White);
            }
        }

        public void movingSouth()
        {
            if (getKey().IsKeyDown(Keys.Down))
            {
                Rectangle curLoc = new Rectangle(x, y, xFrame, yFrame);
                Rectangle curAnimation = new Rectangle(xFrame * i, 0 * yFrame, xFrame, yFrame);
                spriteBatch.Draw(person, curLoc, curAnimation, Color.White);
                i++; y = y + 10;

                if (i > 6)
                {
                    i = 0;
                }
                
                graphics.GraphicsDevice.Clear(Color.White);
            }
        }

        public void movingEast()
        {
            if (getKey().IsKeyDown(Keys.Right))
            {
                Rectangle curLoc = new Rectangle(x, y, xFrame, yFrame);
                Rectangle curAnimation = new Rectangle(xFrame * i, 3 * yFrame, xFrame, yFrame);
                spriteBatch.Draw(person, curLoc, curAnimation, Color.White);
                i++; x = x + 10;

                if (i > 6)
                {
                    i = 0;
                }

                graphics.GraphicsDevice.Clear(Color.White);
            }

        }

        public void movingWest()
        {
            if (getKey().IsKeyDown(Keys.Left))
            {
                Rectangle curLoc = new Rectangle(x, y, xFrame, yFrame);
                Rectangle curAnimation = new Rectangle(xFrame * i, 1 * yFrame, xFrame, yFrame);
                spriteBatch.Draw(person, curLoc, curAnimation, Color.White);
                i++; x = x - 10;

                if (i > 6)
                {
                    i = 0;
                }

                graphics.GraphicsDevice.Clear(Color.White);
            }
        }

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

            getKey();

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

            // TODO: Add your drawing code here

           
            

            spriteBatch.Begin();
            //map.Draw(spriteBatch);
            //newCircle.Draw(spriteBatch);
            movingNorth();
            movingSouth();
            movingEast();
            movingWest();
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
