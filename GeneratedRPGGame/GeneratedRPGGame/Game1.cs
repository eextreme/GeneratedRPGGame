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
        int x = 400, y = 400, xFrame, yFrame, fxFrame, fyFrame;
        CreateAttackCircle newCircle;
        GenerateMap map;
        Texture2D person, fireball;

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

            fireball = Content.Load<Texture2D>("FireBall");
            fxFrame = fireball.Width / 3;
            fyFrame = fireball.Height / 4;


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

        public KeyboardState getKey() { return Keyboard.GetState();}
        public MouseState getMouse() { return Mouse.GetState(); }
        
        int i = 0, yRef = 0;
        Rectangle curLoc, curAnimation;
        public void move()
        {
            int yMod = 0, xMod = 0;

            if (getKey().IsKeyDown(Keys.Down))
            { yRef = 0; i++; yMod = 10; xMod = 0; }

            if (getKey().IsKeyDown(Keys.Left))
            { yRef = 1; i++; yMod = 0; xMod = -10; }

            if (getKey().IsKeyDown(Keys.Up))
            { yRef = 2; i++; yMod = -10; xMod = 0; }

            if (getKey().IsKeyDown(Keys.Right))
            { yRef = 3; i++; yMod = 0; xMod = 10; }

            if (i > 5) { i = 0; }

            x = x + xMod; y = y + yMod;
            curLoc = new Rectangle(x, y, xFrame, yFrame);
            curAnimation = new Rectangle(xFrame * i, yRef * yFrame, xFrame, yFrame);

        }

        int fyRef = 0;int fx=0, fy=0;
        int yMod=0, xMod=0;
        Rectangle curLoc2, curAnimation2;
        private void shootFireball(int x, int y)
        {
            if (getMouse().LeftButton== ButtonState.Pressed)
            {
                fx = 0; fy = 0;
                if (yRef == 0) { i++; fyRef = 0; yMod = 10; xMod = 0;  }
                if (yRef == 1) { i++; fyRef = 1; yMod = 0; xMod = -10; }
                if (yRef == 2) { i++; fyRef = 3; yMod = -10; xMod = 0; }
                if (yRef == 3) { i++; fyRef = 2; yMod = 0; xMod = 10;  }               
            }

            fx=fx+xMod; fy=fy+yMod;
            curLoc2 = new Rectangle(x+fx, y+fy, xFrame, yFrame);
            curAnimation2 = new Rectangle(fxFrame * i, fyRef * fyFrame, fxFrame, fyFrame);

            if (i>3) { i = 0; }
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

            move();
            shootFireball(x,y);

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
            spriteBatch.Draw(person, curLoc, curAnimation, Color.White);
            spriteBatch.Draw(fireball, curLoc2, curAnimation2, Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }


    }
}
