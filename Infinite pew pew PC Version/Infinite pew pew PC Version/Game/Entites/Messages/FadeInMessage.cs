using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using ThridWolrdShooterGame.Managers;
using System.Diagnostics;

namespace ThridWolrdShooterGame.Entites.Messages
{
    public class FadeInMessage:Message
    {
        private float fadeInTimer, timerToExit, timeActive;

        private bool isActive;
        private bool count;

        private float transition;
        private float transitionDuration;

        public FadeInMessage(Vector2 Position, string Message, float TimeActive, float FadeInTimer)
        {
            this.Position = Position;
            this.MessageString = Message;

            this.fadeInTimer = this.transitionDuration = FadeInTimer;

            this.timerToExit = timeActive =  TimeActive;
            isActive = true;

            this.Font = EntityManager.Resources.InstructionsBonusLevel;

            this.Scale = 1f;

        }

        public FadeInMessage(Vector2 Position, string Message, float TimeActive, float FadeInTimer, float Scale)
        {
            this.Position = Position;
            this.MessageString = Message;

            this.fadeInTimer = this.transitionDuration = FadeInTimer;

            this.timerToExit = timeActive = TimeActive;
            isActive = true;

            this.Font = EntityManager.Resources.InstructionsBonusLevel;

            this.Scale = Scale;

        }

        public override void Update(GameTime gameTime)
        {
            if (isActive)
            {
                transition += GameplayScreen.SlowMoMultiplier;

                if (transition > transitionDuration)
                {
                    transition = transitionDuration;
                    count = true;
                }
            }

            if (count)
            {
                this.timerToExit -= GameplayScreen.SlowMoMultiplier;

                if (timerToExit < 0)
                {
                    timerToExit = 0;
                    IsExpired = true;
                }
            }
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {

            float factor = transition / transitionDuration;
            //spriteBatch.DrawString(Font, MessageString, Position, Color.White);
            spriteBatch.DrawString(Font, MessageString, Position, Color.Yellow * factor, this.Orientation, Size / 2, Scale, Microsoft.Xna.Framework.Graphics.SpriteEffects.None, 0f);

        }
    }
}
