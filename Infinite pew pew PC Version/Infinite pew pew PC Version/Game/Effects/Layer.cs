namespace ThridWolrdShooterGame.Effects
{
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using System;
    using ThridWolrdShooterGame;
    using MyExtensions;

    public abstract class Layer
    {
        public Camera Camera;
        public Rectangle WorldBounds;
        public Vector2 Parallax { get; set; }
        public Color Color { get; set; }
        public float LineSize { get; set; }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (Camera.GetViewMatrix(Parallax) == null)
                return;

            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Camera.GetViewMatrix(Parallax));

            spriteBatch.DrawLine(new Vector2((int)WorldBounds.X, (int)WorldBounds.Y), new Vector2((int) WorldBounds.Width, (int) WorldBounds.Y), Color,LineSize);

            spriteBatch.DrawLine(new Vector2((int)WorldBounds.Width, (int)WorldBounds.Y), new Vector2((int)WorldBounds.Width, (int)WorldBounds.Height), Color, LineSize );

            spriteBatch.DrawLine(new Vector2((int)WorldBounds.X, (int)WorldBounds.Y), new Vector2((int)WorldBounds.X, (int)WorldBounds.Height), Color, LineSize);

            spriteBatch.DrawLine(new Vector2((int)WorldBounds.X, (int)WorldBounds.Height), new Vector2((int)WorldBounds.Width, (int)WorldBounds.Height), Color, LineSize);

            spriteBatch.End();
        }

        public Vector2 WorldToScreen(Vector2 worldPosition)
        {
            return Vector2.Transform(worldPosition, Camera.GetViewMatrix(Parallax));
        }

        public Vector2 ScreenToWorld(Vector2 screenPosition)
        {
            return Vector2.Transform(screenPosition, Matrix.Invert(Camera.GetViewMatrix(Parallax)));
        }

        
    }
}
