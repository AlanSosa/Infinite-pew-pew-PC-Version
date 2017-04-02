using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using ThridWolrdShooterGame.Managers;
using System.Diagnostics;

namespace ThridWolrdShooterGame.Entites.Messages
{
    public class FadingOutMessage:Message
    {
        private float timeUntilDispose, baseTimingMeditor;
        
        public FadingOutMessage(Vector2 Position,string Message)
        {
            this.Position = Position;
            this.MessageString = Message;
            this.timeUntilDispose = baseTimingMeditor =60;
            this.Font = EntityManager.Resources.AnimatedMessage;
            this.Color = Color.Yellow;
        }

        public FadingOutMessage(Vector2 Position, string message, int framesUntilDispose)
        {
            this.Position = Position;
            this.MessageString = message;
            this.timeUntilDispose = baseTimingMeditor = framesUntilDispose;
            this.Font = EntityManager.Resources.AnimatedMessage;
            this.Color = Color.Yellow;
        }

        public FadingOutMessage(Vector2 Position , string message, int framesUntilDispose, Color Color):this(Position,message,framesUntilDispose)
        {
            this.Color = Color;
        }

        public override void Update(GameTime gametime)
        {

            timeUntilDispose -=GameplayScreen.SlowMoMultiplier;

            if (timeUntilDispose < 0)
                this.IsExpired = true;
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            float factor = timeUntilDispose / baseTimingMeditor;
            Vector2 Size = new Vector2(EntityManager.Resources.AnimatedMessage.MeasureString(MessageString).X, EntityManager.Resources.AnimatedMessage.MeasureString(MessageString).Y);
            spriteBatch.DrawString(Font, MessageString, Position, Color * factor,0, Size / 2, factor, Microsoft.Xna.Framework.Graphics.SpriteEffects.None, 0f);
        }
    }
}
