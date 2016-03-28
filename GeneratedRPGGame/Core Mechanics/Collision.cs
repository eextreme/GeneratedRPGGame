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
    static class Collision
    {
        public static Vector2 collP;
        public static Vector2 north = new Vector2(0, -1), south = new Vector2(0, 1), east = new Vector2(1, 0), west = new Vector2(-1, 0);
        public static Vector2 nw = new Vector2(-1, -1), ne = new Vector2(1, -1);
        public static Vector2 sw = new Vector2(-1, 1), se = new Vector2(1, 1);

        public static Boolean rectangle(Rectangle a, Rectangle b)
        {
            return a.Intersects(b);
        }

        public static Boolean circle(Vector2 a, float radiusA, Vector2 b, float radiusB)
        {
            float dist = Vector2.Distance(a, b);
            float dist2 = radiusA + radiusB;

            float collPointX = ((a.X * radiusB) + (b.X * radiusA)) / (radiusA + radiusB);
            float collPointY = ((a.Y * radiusB) + (b.Y * radiusA)) / (radiusA + radiusB);
            
            collP = new Vector2(collPointX, collPointY);

            return (dist2 > dist);
        }

        public static Boolean pixel(Rectangle rectangleA, Color[] dataA, Rectangle rectangleB, Color[] dataB)
        {
            // Find the bounds of the rectangle intersection
            int top = Math.Max(rectangleA.Top, rectangleB.Top);
            int bottom = Math.Min(rectangleA.Bottom, rectangleB.Bottom);
            int left = Math.Max(rectangleA.Left, rectangleB.Left);
            int right = Math.Min(rectangleA.Right, rectangleB.Right);

            // Check every point within the intersection bounds
            for (int y = top; y < bottom; y++)
            {
                for (int x = left; x < right; x++)
                {
                    // Get the color of both pixels at this point
                    Color colorA = dataA[(x - rectangleA.Left) +
                                         (y - rectangleA.Top) * rectangleA.Width];
                    Color colorB = dataB[(x - rectangleB.Left) +
                                         (y - rectangleB.Top) * rectangleB.Width];

                    // If both pixels are not completely transparent,
                    if (colorA.A != 0 && colorB.A != 0)
                    {
                        collP = new Vector2(x, y);// then an intersection has been found
                        return true;
                    }
                }
            }
            return false;
        }       

        public static Boolean vector(Vector2 object1, Texture2D obj1, Vector2 object2, Texture2D obj2)
        {
            if (object1.X + obj1.Width < object2.X) return false;
            if (object2.X + obj2.Width < object1.X) return false;
            
            if (object1.Y + obj1.Height < object2.Y) return false;
            if (object2.Y + obj2.Height < object1.Y) return false;
            return true; 
        }

        public static Rectangle vec2Rec(Vector2 source)
        {
            double angle = Math.Atan(source.Y / source.X);
            int rectX = (int) (source.Length() * Math.Cos(angle));
            int rectY = (int)(source.Length() * Math.Sin(angle));

            return new Rectangle((int) source.X, (int) source.Y, rectX, rectY);
        }

        public static bool hitWall(Vector2 loc, ref float modX, ref float modY)
        {
            if (loc.X <= 20) {
                modX = 40; modY = 0;
                return true;
            }

            if (loc.X >= 780) {
                modX = -40; modY = 0;
                return true;
            }

            if (loc.Y <= 90)
            {
                modX = 0; modY = 40;
                return true;
            }

            if (loc.Y >= 780)
            {
                modX = 0; modY = -40;
                return true;
            }

            return false;
        }        

        public static float isFacing(Vector2 a, Vector2 b)
        {
            return Vector2.Dot(a, b);
        }
    }
}
