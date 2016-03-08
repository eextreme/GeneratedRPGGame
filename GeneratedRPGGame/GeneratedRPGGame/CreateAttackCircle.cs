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

namespace GeneratedRPGGame
{
    class CreateAttackCircle
    {
        Texture2D circle, line;
        GraphicsDevice atkCircle;
        SpriteFont coordinates;
        String indicator = "Left Click to Stop";
        float[] hitCrit, hitOk;
        List<Fan> allFans, critFans;
        Vector2 screenCenter, imageCenter, imageCenter2, imageCenter3;
        SoundEffect hit, miss, crit;
        int hits, radius;

        public CreateAttackCircle(float[] hitCritL, float[] hitOkL, int r, GraphicsDevice device)
        {
            atkCircle = device;

            hits = hitCritL.Count();

            hitCrit = new float[hits];
            hitOk = new float[hits];

            hitCrit = hitCritL;
            hitOk = hitOkL;

            radius = r;
            
            circle = drawCircle(radius, Color.LightBlue);
            line = drawLine(radius / 2, Color.Green);

            screenCenter = new Vector2(400, 400);
            imageCenter = new Vector2(circle.Width / 2f, circle.Height / 2f);
            imageCenter2 = new Vector2(line.Width, line.Height);
            imageCenter3 = new Vector2(line.Width, line.Height + radius/2);

            allFans = new List<Fan>();
            critFans = new List<Fan>();
            for (int i = 0; i < hits; i++)
            {
                allFans.Add(new Fan(drawFan(radius, hitOkL[i], Color.Yellow), hitCritL[i], hitOkL[i]));
            }
        }

        public void LoadContent(ContentManager content)
        {
            coordinates = content.Load<SpriteFont>("Courier New");
            hit = content.Load<SoundEffect>("anvil_hit");
            miss = content.Load<SoundEffect>("flyby-Conor");
            crit = content.Load<SoundEffect>("glass_ping");
        }

        private float RotationAngle;
        private bool pressed = false;
        private int current = 0;

        public void Update(GameTime time)
        {
            float elapsed = (float)time.ElapsedGameTime.TotalMilliseconds;

            RotationAngle += elapsed / 1000;

            checkCircle(getMouseState(), RotationAngle);

            float circle = MathHelper.Pi * 2;
            RotationAngle = RotationAngle % circle;
        }

        private void checkCircle(MouseState mouse, float angle)
        {           
            if (mouse.LeftButton == ButtonState.Pressed)
            {
                if (current < hitCrit.Count() && pressed)
                {
                    double error = getError(angle, degToRad(hitCrit[current]));
                    double errorOk = Math.Abs(1 - hitOk[current] / 90);
                    if (error<= 0.06)
                    {
                        indicator = "Critical Hit! With " + hitCrit[current] + " at " + angle*180/Math.PI;
                        circleCrit();
                    }
                    else if (error<= errorOk)
                    {
                        indicator = "Hit! With " + hitCrit[current] + " at " + angle * 180 / Math.PI;
                        circleHit();
                    }
                    else
                    {
                        indicator = "Miss! With " + hitCrit[current] + " at " + angle * 180 / Math.PI;
                        circleMiss();
                    }

                    current++;
                    pressed = false;
                }

                if (current>=3)
                {
                    current = 0;
                }
  
            }

            if (mouse.LeftButton == ButtonState.Released)
            {
                pressed = true;
            }
        }
        private double getError(double actual, double expected){
            return Math.Abs(actual - expected) / expected;
        }
        private MouseState getMouseState() {
            return Mouse.GetState();
        }

        private void circleHit(){
            hit.Play();
            //Indicate that something hit
        }
        private void circleMiss(){
            miss.Play();
            //Indicate that something missed
        }
        private void circleCrit()
        {
            crit.Play();
        }

        private double degToRad(double deg)
        {
            return deg * Math.PI / 180;
        }


        public void Draw(SpriteBatch atkCircleGraphic)
        {
            atkCircleGraphic.Draw(circle, screenCenter, null, Color.White, 0f, imageCenter, 1f, SpriteEffects.None, 0f);

            foreach (Fan f in allFans)
                    atkCircleGraphic.Draw(f.texture, screenCenter, null, Color.White, (float) degToRad(f.angle+f.angleOff), imageCenter3, 1f, SpriteEffects.None, 0f);

            atkCircleGraphic.Draw(line, screenCenter, null, Color.White, RotationAngle, imageCenter2, 1f, SpriteEffects.None, 0f);

            atkCircleGraphic.DrawString(coordinates, indicator, new Vector2(0, 0), Color.Black);
        }

        private Texture2D drawCircle(int radius, Color colors)
        {
            Texture2D texture = new Texture2D(atkCircle, radius, radius);
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
                        colorData[index] = colors;
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
        private Texture2D drawLine(int radius, Color colors)
        {
            Texture2D texture = new Texture2D(atkCircle, radius, 1);
            Color[] fill = new Color[1 * radius];

            for (int i = 0; i < fill.Length; i++)
            {
                fill[i] = colors;
            }

            texture.SetData(fill);
            return texture;
        }
        private Texture2D drawFan(int radius, double size, Color clrs)
        {
            Texture2D texture = new Texture2D(atkCircle, radius, radius);
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

        private class Fan
        {
            public Texture2D texture { get; set; }
            public double angle { get; set; }
            public double angleOff { get; set; }

            public Fan(Texture2D image, double ang, double ok)
            {
                texture = image;
                angle = ang;
                angleOff = -0.5 * ok + 45;
            }
        }
    }
}
