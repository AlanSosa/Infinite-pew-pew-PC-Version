using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ThridWolrdShooterGame.Entites
{
    public class FadingOutImageMessage:Entity
    {
        private float timeUntilDispose, baseTimingMeditor;

        public FadingOutImageMessage(Vector2 Position , Texture2D Image)
        {

            this.Position = Position;
            this.texture = Image;
            this.timeUntilDispose = baseTimingMeditor = 60;
        }

        public FadingOutImageMessage(Vector2 Position, Texture2D Image, int framesUntilDispose)
        {

            this.Position = Position;
            this.texture = Image;
            this.timeUntilDispose = baseTimingMeditor = framesUntilDispose;
        }

        public FadingOutImageMessage(Vector2 Position, Texture2D Image, int framesUntilDispose, Color Color)
            : this(Position, Image, framesUntilDispose)
        {
            this.Color = Color;
        }

        public override void Update(GameTime gametime)
        {
            timeUntilDispose -= GameplayScreen.SlowMoMultiplier;

            if (timeUntilDispose < 0)
                this.IsExpired = true;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            float factor = timeUntilDispose / baseTimingMeditor;
            spriteBatch.Draw(this.texture, this.Position, null, this.Color *  factor, Orientation, Size / 2f,factor * 2f, SpriteEffects.None, 0f);
        }
    }
}
