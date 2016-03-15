using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;


namespace GeneratedRPGGame
{
    class Camera
    {
        public Vector2 Position;
        public float Rotation { get; set; }
        public float Zoom { get; set; }

        public Camera()
        {
            Position = Vector2.Zero;
            Zoom = 1f;
        }

        public Matrix TransformMatrix
        {
            get
            {
                return Matrix.CreateRotationZ(Rotation) * Matrix.CreateScale(Zoom) * Matrix.CreateTranslation(Position.X, Position.Y, 0);
            }
        }

        public void ZoomCamera(KeyboardState keyState)
        {
            float scale = 0f;
            if (keyState.IsKeyDown(Keys.W))
                scale += 0.05f;
            if (keyState.IsKeyDown(Keys.S))
                scale -= 0.05f;

            this.Zoom += scale;
        }

        public void RotateCamera(KeyboardState keyState)
        {
            float rotation = 0f;
            if (keyState.IsKeyDown(Keys.A))
                rotation -= 5;
            if (keyState.IsKeyDown(Keys.D))
                rotation += 5;

            this.Rotation += MathHelper.ToRadians(rotation);
        }

        public void TranslateCamera(KeyboardState keyState)
        {
            Vector2 cameraTranslate = Vector2.Zero;
            if (keyState.IsKeyDown(Keys.Up))
                cameraTranslate.Y -= 5;
            if (keyState.IsKeyDown(Keys.Down))
                cameraTranslate.Y += 5;
            if (keyState.IsKeyDown(Keys.Left))
                cameraTranslate.X -= 5;
            if (keyState.IsKeyDown(Keys.Right))
                cameraTranslate.X += 5;

            this.Position += cameraTranslate;
        }

        public void changePosition(int x, int y)
        {

        }
    }
}
