using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using ThridWolrdShooterGame.Managers;
using System.Diagnostics;

namespace ThridWolrdShooterGame.Entites.Messages
{
    public class FadeInFadeOutMessage:Message
    {
        private float fadeInOutTimer, timerToExit, timeActive;

        private bool isActive;
        private bool count;

        private float transition;
        private float transitionDuration;

        public FadeInFadeOutMessage(Vector2 Position, string Message, float TimeActive, float FadeInOutTimer)
        {
            this.Position = Position;
            this.MessageString = Message;

            this.fadeInOutTimer = this.transitionDuration = FadeInOutTimer;

            this.timerToExit = timeActive =  TimeActive;
            isActive = true;

            this.Font = EntityManager.Resources.InstructionsBonusLevel;
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
            else
            {
                transition -= GameplayScreen.SlowMoMultiplier;

                if (transition < 0)
                {
                    transition = 0;
                    IsExpired = true;
                }
            }


            if (count)
            {
                this.timerToExit -= GameplayScreen.SlowMoMultiplier;

                if (timerToExit < 0)
                {
                    timerToExit = 0;
                    isActive = false;
                }
            }
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {

            float factor = transition / transitionDuration;
            //spriteBatch.DrawString(Font, MessageString, Position, Color.White);
            spriteBatch.DrawString(Font, MessageString, Position, Color.Yellow * factor, 0, Size / 2, 1, Microsoft.Xna.Framework.Graphics.SpriteEffects.None, 0f);

        }
    }
}
