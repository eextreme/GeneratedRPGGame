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
        int x = 400, y = 400, xFrame, yFrame, fxFrame, fyFrame;
        CreateAttackCircle newCircle;
        GenerateMap map;
        Texture2D person, fireball;
        Player newPlayer;

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

            newPlayer = new Player(Content.Load<Texture2D>("Sprite Sheets/sprites_map_claudius"), 6, 4);
            newPlayer.equipWeapon(Content.Load <Texture2D> ("Sprite Sheets/spear"));
            newPlayer.setSpawnPoint(400, 400);
            
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

            newPlayer.move();

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
            //newCircle.Draw(spriteBatch);
            //spriteBatch.DrawString(coordinates, newPlayer.getPosition(), new Vector2(newPlayer.posX, newPlayer.posY), Color.Black);
            newPlayer.Draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }


    }
}
