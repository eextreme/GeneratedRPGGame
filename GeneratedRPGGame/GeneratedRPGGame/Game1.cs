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
        Texture2D circle, line;
        KeyboardState state;
        SpriteFont coordinates;
        String indicator;
        float[] hitArray, hitCrit, hitOk, hitAng;
        List<Fan> allFans, critFans;
        Vector2 screenCenter, imageCenter, imageCenter2, imageCenter3;

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
            state = Keyboard.GetState();
            circle = drawCircle(300);
            line = createLine(150, Color.Red);
            current = 0;

            coordinates = Content.Load<SpriteFont>("Courier New");
            indicator = "Press and hold space to stop";

            hitCrit = new float[3] { 120, 240, 360 };
            hitOk = new float[3] { 60, 60, 60 };
            hitAng = new float[3] { 0, 0, 0 };

            createAttackCircle(3, hitCrit, hitOk);

            screenCenter = new Vector2(400, 400);
            imageCenter = new Vector2(circle.Width / 2f, circle.Height / 2f);
            imageCenter2 = new Vector2(line.Width, line.Height);
            imageCenter3 = new Vector2(line.Width, line.Height + 150);



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


            // TODO: use this.Content to load your game content here
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
        private float RotationAngle;
        private bool pressed=false;
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            float elapsed = (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            
            RotationAngle += elapsed / 1000;

            //indicator = "Mouse is in " + getMouseState();
            if (getMouseState().LeftButton==ButtonState.Pressed)
            {
                if (current < 3 && pressed)
                {
                    hitAng[current] = RotationAngle;
                    current++;
                    pressed = false;
                }
            }

            if (getMouseState().LeftButton==ButtonState.Released)
            {
                pressed = true;
            }

            if (getMouseState().RightButton == ButtonState.Pressed)
            {
                calculateHitInfo(hitAng, hitCrit);
                current = 0;
            }

                                    
            float circle = MathHelper.Pi * 2;
            RotationAngle = RotationAngle % circle;

           

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        private int current;
        private void calculateHitInfo(float[] hit, float[] loc)
        {
            indicator = String.Format("\nFirst hit at {0} with error of {1}\nSecond hit at {2} with error of {3}\nThird hit at {4} with error of {5}",
                                        hit[0] * 180 / Math.PI, getError(hit[0], degToRad(loc[0])),
                                        hit[1] * 180 / Math.PI, getError(hit[1], degToRad(loc[1])),
                                        hit[2] * 180 / Math.PI, getError(hit[2], degToRad(loc[2]))); 
        }

        public void getAngle (float angle)
        {

        }

        public double getError(double actual, double expected)
        {
            return Math.Abs(actual-expected)/expected;
        }
        
        public KeyboardState getKeyboardState()
        {
            return Keyboard.GetState();
        }

        public MouseState getMouseState()
        {
            return Mouse.GetState();
        }

        public double degToRad (double deg)
        {
            return deg * Math.PI / 180;
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        /// 

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

           
            graphics.GraphicsDevice.Clear(Color.White);

            spriteBatch.Begin();
            spriteBatch.Draw(circle, screenCenter, null, Color.White, 0f, imageCenter, 1f, SpriteEffects.None, 0f);

            foreach (Fan f in allFans)
                spriteBatch.Draw(f.texture, screenCenter, null, Color.White, (float) degToRad(f.angle+f.angleOff), imageCenter3, 1f, SpriteEffects.None, 0f);
                
            spriteBatch.Draw(line, screenCenter, null, Color.Green, RotationAngle, imageCenter2, 1f, SpriteEffects.None, 0f);

            spriteBatch.DrawString(coordinates, indicator, new Vector2(0, 0), Color.Black);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void createAttackCircle(int hits, float[] critHit, float[] okHit)
        {
            allFans = new List<Fan>();
         
            for (int i=0; i < hits; i++)
            {
                allFans.Add(new Fan(drawFan(300, okHit[i], Color.LightBlue), critHit[i], okHit[i]));
            }          
        }

        private Texture2D drawCircle(int radius)
        {
            Texture2D texture = new Texture2D(GraphicsDevice, radius, radius);
            Color[] colorData = new Color[radius * radius];

            float diam = radius / 2f;
            float diamsq = diam * diam;

            for (int x = 1; x < radius; x++)
            {
                for (int y = 1; y < radius; y++)
                {
                    int index = x * radius + y;
                    Vector2 pos = new Vector2(x - diam, y - diam);
                    if (pos.LengthSquared() <= diamsq)
                    {
                        colorData[index] = Color.Green;
                    }
                    else
                    {
                        colorData[index] = Color.Transparent;
                    }
                }
            }

            texture.SetData(colorData);
            return texture;
        }

        private Texture2D createLine(int radius, Color colors)
        {
            Texture2D texture = new Texture2D(GraphicsDevice,radius, 1);
            Color[] fill = new Color[1 * radius];

            for (int i = 0; i < fill.Length; i++ )
            {
                fill[i] = colors;
            }

            texture.SetData(fill);
            return texture;
        }

        private Texture2D drawFan(int radius, double size, Color clrs)
        {
            Texture2D texture = new Texture2D(GraphicsDevice, radius, radius);
            Color[] colorData = new Color[radius * radius];

            float diam = radius / 2f;
            float diamsq = diam * diam;
            double angle = 0;

            for (int x = 1; x < radius / 2; x++)
            {
                for (int y = 1; y < radius; y++)
                {
                    int index = x * radius + y;
                    Vector2 pos = new Vector2(x - diam, y - diam);
                    angle = Math.Atan(pos.X / pos.Y);
                    if (pos.LengthSquared() <= diamsq && angle >= degToRad(size))
                    {
                        colorData[index] = clrs;
                    }
                    else
                    {
                        colorData[index] = Color.Transparent;
                    }
                }
            }

            texture.SetData(colorData);
            return texture;
        }

        public class Fan
        {
            public Texture2D texture { get; set; }
            public double angle { get; set; }
            public double angleOff { get; set; }

            public Fan(Texture2D image, double ang, double ok)
            {
                texture = image;
                angle = ang;
                angleOff = - 0.5 * ok + 45;
            }
        }
    }
}
