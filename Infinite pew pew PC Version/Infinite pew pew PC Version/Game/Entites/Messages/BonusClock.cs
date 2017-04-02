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
    public class BonusClock:Messages.Message
    {
        private Random random = new Random();
        private bool timing;
        private float milisecondsTimer;
        private float milisecondsColorTimer;

        public BonusClock()
        {
            this.Position = new Vector2(5, 0);
            this.Font = EntityManager.Resources.AnimatedMessage;
            timing = false;
            MessageString = "";
            ResetTimer();
        }

        public void StartTiming()
        {
            timing = true;
            shaking= true;
        }

        public void StopTiming()
        {
            timing = false;
            shaking = false;
        }

        public void ResetTimer()
        {
            milisecondsTimer = 18000;
            milisecondsColorTimer = 0;
        }

        public float MilisecondsTimer
        {
            get
            {
                return milisecondsTimer;
            }
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);
            if (EnemySpawner.BonusMode)
            {
                this.MessageString = milisecondsTimer.ToString();
            }

            if (timing)
            {
                if(milisecondsTimer > 0)
                    milisecondsTimer -= gameTime.ElapsedGameTime.Milliseconds * GameplayScreen.SlowMoMultiplier;

                milisecondsColorTimer += gameTime.ElapsedGameTime.Milliseconds * GameplayScreen.SlowMoMultiplier;

                if (shaking)
                {
                    shakeTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

                    if (shakeTimer >= shakeDuration)
                    {
                        shaking = false;
                        shakeTimer = shakeDuration;
                    }   

                    float progress = shakeTimer / shakeDuration;
                    float magnitude = shakeMagnitude * (1f - (progress * progress));

                    shakeOffset = (new Vector2(nextFloat(), nextFloat()) * magnitude) * GameplayScreen.SlowMoMultiplier;

                    if (shakeFlagCounter < milisecondsTimer)
                    {
                        shakeFlagCounter += 1000;
                        shakeTimer = 0f;
                        shaking = true;
                    }
                }
            }
        }

        private float nextFloat()
        {
            return (float)random.NextDouble() * 2f - 1f;
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            if (EnemySpawner.BonusMode)
            {
                float factor = (EnemySpawner.ShieldAux != PlayerStatus.Shield ?1:(float)milisecondsColorTimer / 18000f);
                spriteBatch.DrawString(this.Font, ( ((int)(milisecondsTimer / 1000)).ToString().Length < 2 ? "0" + (int)(milisecondsTimer / 1000) : ""+ (int)(milisecondsTimer / 1000) ), shakeOffset + Position, ColorUtil.HSVToRGB(10f, factor,1f), 0f, Size / 2f, 2f, SpriteEffects.None, 0f);
                spriteBatch.DrawString(this.Font, ":" + ( (int)(milisecondsTimer % 1000)>0?(int)(milisecondsTimer % 1000)+"":"00"  ), shakeOffset + Position + new Vector2(90, 39), ColorUtil.HSVToRGB(1f, factor, 1f), 0f, Size / 2f, 1f, SpriteEffects.None, 0f);
                spriteBatch.DrawLine(this.Position + new Vector2(-30, 85), this.Position + new Vector2(180, 85), Color.White, 2.8f);

                spriteBatch.DrawString(this.Font , "Timer" , this.Position + new Vector2(0 , 85), Color.White, 0, Size/ 2, 0.7f, SpriteEffects.None,0f);
            }
        }

        private int shakeFlagCounter;
        private bool shaking { get; set; }
        private float shakeTimer { get; set; }
        private float shakeDuration = 1f;
        private float shakeMagnitude = 10f;
        private Vector2 shakeOffset;
    }
}
