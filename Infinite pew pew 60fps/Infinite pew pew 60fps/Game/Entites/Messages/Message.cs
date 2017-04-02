using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ThridWolrdShooterGame.Entites.Messages
{
    public abstract class Message:Entity
    {
        protected string MessageString;
        protected SpriteFont Font;
        protected float Scale; 

        public override void Update(GameTime gameTime) { }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(Font,MessageString, this.Position, Color,this.Orientation, Size / 2f, Scale, SpriteEffects.None, 0f);
        }
    }
}
