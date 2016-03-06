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
        Texture2D circle, line, fans;
        KeyboardState state;
        SpriteFont coordinates;
        String indicator;
        double[] hitArray, hitCrit, hitOk;
        List<Fan> allFans = new List<Fan>();
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

            coordinates = Content.Load<SpriteFont>("Courier New");
            indicator = "Press and hold space to stop";

            hitCrit = new double[3] { 10, 120, 260 };
            hitOk = new double[3] { 80, 80, 80 };

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
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            float elapsed = (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            // TODO: Add your game logic here.
            if (!getKey().IsKeyDown(Keys.Space))
                RotationAngle += elapsed / 100;
            else
                checkIfHit(RotationAngle);
            
            float circle = MathHelper.Pi * 2;
            RotationAngle = RotationAngle % circle;

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        private void checkIfHit(float angle)
        {
            foreach (double i in hitArray)
            {
                double error = Math.Abs(angle - i) / i;

                if (error <= 0.05)
                    indicator = "Crit at " + angle * 180 / Math.PI;
                else if (error <= 0.10)
                    indicator = "Hit at " + angle * 180 / Math.PI;
                else
                    indicator = "Miss at " + angle * 180 / Math.PI;
            }

        }

        public double degToRad (double deg)
        {
            return deg * Math.PI / 180;
        }

        private KeyboardState getKey()
        {
            KeyboardState curState = Keyboard.GetState();
            return curState;
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

           
            graphics.GraphicsDevice.Clear(Color.White);

            spriteBatch.Begin();
            spriteBatch.Draw(circle, screenCenter, null, Color.White, 0f, imageCenter, 1f, SpriteEffects.None, 0f);

            foreach (Fan f in allFans)
                spriteBatch.Draw(f.texture, screenCenter, null, Color.White, (float) f.angle, imageCenter3, 1f, SpriteEffects.None, 0f);

            spriteBatch.Draw(line, screenCenter, null, Color.Green, RotationAngle, imageCenter2, 1f, SpriteEffects.None, 0f);

            spriteBatch.DrawString(coordinates, indicator, new Vector2(0, 0), Color.Black);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void createAttackCircle(int hits, double[] critHit, double[] okHit)
        {
            allFans = new List<Fan>();
            hitArray = createAttackArray(3, critHit, okHit, 100);
            Color[] cArray = new Color[3] { Color.LightBlue, Color.LightYellow, Color.LightSeaGreen };

            for (int i=0; i < hits; i++)
            {
                allFans.Add(new Fan(drawFan(300, okHit[i], cArray[i]), critHit[i]));
            }          
        }

        private double[] createAttackArray(int numOfHits, double[] hitCrits, double[] hitField, int baseDmg)
        {
            double[] atkArray = new double[numOfHits*2];

            for (int i = 0; i < hitCrits.Count()-1;i++ )
            {
                atkArray[i]= hitCrits[i] + hitField[i];
                atkArray[i+1] = hitCrits[i] - hitField[i];
            }
            
            return atkArray;
        }

        private Boolean checkHit(Vector2 hit, double hitAngel)
        {
            double angle = Math.Atan(hit.Y / hit.X);
            double angleD = angle * 180 / Math.PI;
            double hitD = hitAngel * 180 / Math.PI;

            double angleE = Math.Abs(hitAngel - angle) / hitAngel;

            if (angleE < 0.50)
                return true;

            return false;
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

            public Fan(Texture2D image, double ang)
            {
                texture = image;
                angle = ang;
            }
        }
    }
}
