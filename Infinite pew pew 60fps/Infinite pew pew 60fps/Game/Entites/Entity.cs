using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace ThridWolrdShooterGame.Entites
{
    public abstract class Entity:IDisposable
    {
        protected Texture2D texture;
        public Color Color = Color.White;

        public Vector2 Position, Velocity;
        public float Orientation;
        public float Radius;
        public bool IsExpired;

        protected bool disposed = false;

        public Vector2 Size
        {
            get
            {
                return texture == null ? Vector2.Zero : new Vector2( texture.Width, texture.Height);
            }
        }

        public abstract void Update(GameTime gametime);

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if(texture != null)
                spriteBatch.Draw(texture, Position, null, Color, Orientation, Size / 2f, 1f, 0, 0);
        }

        protected virtual void Dispose(bool disposing)
        {

            if (!disposed)
            {
                if (disposing)
                {
                    if (texture != null)
                    {
                        texture.Dispose();
                    }
                }

                disposed = true;
            }
        }

        public virtual void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
