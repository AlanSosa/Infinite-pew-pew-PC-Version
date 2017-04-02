using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using Microsoft.Xna.Framework.Graphics;
using ThridWolrdShooterGame.Managers;
using MyExtensions;

namespace ThridWolrdShooterGame.Entites.Messages
{
    public class ShakingMessage:Message
    {
        private float timer;

        private int shakeFlagCounter;
        private bool shaking { get; set; }
        private float shakeTimer { get; set; }
        private float shakeDuration;
        private float shakeMagnitude;
        private Vector2 shakeOffset;

        private float counter;

        public ShakingMessage(Vector2 Position , float Timer, string Message)
        {
            this.timer = Timer;
            this.MessageString = Message;
            this.Position = Position;
            this.Font = EntityManager.Resources.InstructionsBonusLevel;
            shakeDuration = 1f;
            shakeMagnitude = 10f;
        }

        public ShakingMessage(Vector2 Position, float Timer, string Message, float ShakeDuration, float ShakeMagnitude)
        {
            this.timer = Timer;
            this.MessageString = Message;
            this.Position = Position;
            this.Font = EntityManager.Resources.InstructionsBonusLevel;
            this.shakeDuration = ShakeDuration;
            this.shakeMagnitude = ShakeMagnitude;
        }

        public override void Update(GameTime gameTime)
        {
            shakeTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            counter += (float)gameTime.ElapsedGameTime.Milliseconds;
            if (shakeTimer >= shakeDuration)
            {
                shaking = false;
                shakeTimer = shakeDuration;
            }

            float progress = shakeTimer / shakeDuration;
            float magnitude = shakeMagnitude * (1f - (progress * progress));

            shakeOffset = (new Vector2(nextFloat(), nextFloat()) * magnitude) * GameplayScreen.SlowMoMultiplier;

            if (counter > timer)
            {
                this.IsExpired = true;
            }
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(this.Font, this.MessageString, shakeOffset + Position, Color.White, 0f, Size / 2f, 2f, SpriteEffects.None, 0f);
        }

        private Random random = new Random();

        private float nextFloat()
        {
            return (float)random.NextDouble() * 2f - 1f;
        }
    }
}
