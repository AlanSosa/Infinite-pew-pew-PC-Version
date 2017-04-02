using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MyExtensions;

namespace ThridWolrdShooterGame.Effects
{
    public class FirstLayer:Layer
    {
        private int LinesX, LinesY;

        public FirstLayer(int LineCountX, int LineCountY)
        {
            this.LinesX = LineCountX;
            this.LinesY = LineCountY;
        }

        public FirstLayer() { }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            float lineSpacingX = this.WorldBounds.Width / LinesX;
            float lineSpacingY = this.WorldBounds.Height / LinesY;

            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Camera.GetViewMatrix(Parallax));

            for (int i = 1; i < LinesX; i++)
            {
                spriteBatch.DrawLine(new Vector2(this.WorldBounds.X + lineSpacingX * i, this.WorldBounds.Y), new Vector2(this.WorldBounds.X + lineSpacingX * i, this.WorldBounds.Height), Color, LineSize);
            }

            for (int i = 1; i < LinesY; i++)
            {
                spriteBatch.DrawLine(new Vector2(this.WorldBounds.X, this.WorldBounds.Y + lineSpacingY * i), new Vector2(this.WorldBounds.Width, this.WorldBounds.Y + lineSpacingY * i), Color, LineSize);
            }

            spriteBatch.End();
        }
    }
}
