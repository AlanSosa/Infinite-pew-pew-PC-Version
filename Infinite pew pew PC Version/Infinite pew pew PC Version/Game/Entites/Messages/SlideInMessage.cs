using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;
using ThridWolrdShooterGame.Managers;

namespace ThridWolrdShooterGame.Entites.Messages
{
    public class SlideInMessage:Message
    {
        Vector2 Destination;
        private Vector2 startingPosition;
        private float timer;
        private float time;

        public SlideInMessage(Vector2 Position, string message , Vector2 Destination, float time)
        {
            this.Position = startingPosition = Position;
            this.MessageString = message;
            this.Destination = Destination;
            this.Font = EntityManager.Resources.AnimatedMessage;
            this.time = time;
            this.Color = Color.White;
            this.Scale = 1f;
        }

        public SlideInMessage(Vector2 Position, string Message, Vector2 Destination, float Time, 
            Color color, float Scale)
        {
            this.Font = EntityManager.Resources.AnimatedMessage;
            this.Position = startingPosition = Position;
            this.MessageString = Message;
            this.Destination = Destination;
            this.time = Time;
            this.Color = color;
            this.Scale = Scale;
        } 

        public override void Update(GameTime gametime)
        {
            base.Update(gametime);

            timer += gametime.ElapsedGameTime.Milliseconds;

            if (timer <= time)
            {
                this.Velocity += (this.Destination - this.Position) * 0.05f;
            }
            else
                this.Velocity += (this.startingPosition - this.Position) * 0.05f;

            this.Position += Velocity;

            this.Velocity *= 0.8f;

            if (Vector2.DistanceSquared(this.Position, startingPosition) < 200 * 200 && timer > time)
                this.IsExpired = true;

        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }
}
