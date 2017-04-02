using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MyExtensions;
using System.Diagnostics;
using ThridWolrdShooterGame.Managers;

namespace ThridWolrdShooterGame.Entites.Items
{
    public class CounterMeter : Entity
    {
        public bool IsActive
        {
            get;
            set;
        }
        private Vector2 startingPosition;
        private Vector2 lastPosition;


        private Vector2 counterStartingPosition;
        private Vector2 counterLastPosition;

        private Vector2 counterStartingSize;
        private Vector2 counterLastSize;

        private Vector2 counterPosition;
        private Vector2 counterSize;

        private bool Count;

        private float transition;
        private float transitionDuration;

        private float counterTransition;
        private float counterTransitionDuration;

        private int height, width;

        public CounterMeter()
        {
            width = 325;
            height = 30;
            this.texture = EntityManager.Resources.Pixel;
            this.startingPosition = new Vector2(GameplayScreen.graphicsWidth / 2 - width / 2, -height) + new Vector2(0, -10);
            this.lastPosition = new Vector2(GameplayScreen.graphicsWidth / 2 - width / 2, GameplayScreen.graphicsWidth / 2 - height / 2);
            this.transitionDuration = 10f;

            this.counterStartingPosition = lastPosition;
            this.counterLastPosition = new Vector2(startingPosition.X + (width / 2), lastPosition.Y);
            this.counterStartingSize = new Vector2(width, height);
            this.counterLastSize = new Vector2(0, height);
            IsActive = false;
        }

        public override void Update(GameTime gametime)
        {
            if (EntityManager.PlayerShip.IsDead)
                this.IsActive = false;

            if (IsActive)
            {
                transition += (float)gametime.ElapsedGameTime.Milliseconds * 0.01f;

                if (transition > transitionDuration)
                {
                    transition = transitionDuration;

                    Count = true;
                }
            }
            else
            {
                transition -= (float)gametime.ElapsedGameTime.Milliseconds * 0.01f;

                if (transition < 0)
                    transition = 0;
            }

            if (Count)
            {
                counterTransition -= (float)(float)gametime.ElapsedGameTime.Milliseconds * 0.01f;
                if (counterTransition < 0)
                {
                    counterTransition = 0;
                    IsActive = false;
                }
            }

            this.Position = Vector2.SmoothStep(startingPosition,
                lastPosition,
                (float)this.transition / this.transitionDuration);

            this.counterPosition = Vector2.SmoothStep(counterLastPosition,
                counterStartingPosition,
                (float)counterTransition / counterTransitionDuration);

            this.counterSize = Vector2.SmoothStep(counterLastSize,
                counterStartingSize,
                (float)counterTransition / counterTransitionDuration
                );
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            this.Color = Color.CornflowerBlue * .3f;
            drawCounterMeterBorders(spriteBatch);
            this.Color = Color.CornflowerBlue * .009f;
            if (IsActive)
            {
                Color counterColor = Color.CornflowerBlue * Vector2.SmoothStep(Vector2.Zero, new Vector2(0.4f, 0), (float)transition / transitionDuration).X;
                Rectangle counterTexture = new Rectangle((int)counterPosition.X, (int)counterPosition.Y,
                    (int)counterSize.X, (int)counterSize.Y);
                var title = "Weapon timer";
                Vector2 var = EntityManager.Resources.InstructionsBonusLevel.MeasureString(title);
                Vector2 titlePosition = new Vector2(this.Position.X + width / 2 - var.X / 2, this.Position.Y + height / 2 - var.Y / 2);

                spriteBatch.DrawString(EntityManager.Resources.InstructionsBonusLevel, title, titlePosition, Color.White * .3f);
                spriteBatch.Draw(this.texture, counterTexture, counterColor);
            }
        }

        public void setWeapontime(float timer)
        {
            counterTransitionDuration = counterTransition = timer;
            this.IsActive = true;
        }

        private void drawCounterMeterBorders(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawLine(this.Position, new Vector2(this.Position.X + width, this.Position.Y), Color, 3f);
            spriteBatch.DrawLine(new Vector2(this.Position.X + width, this.Position.Y),
                new Vector2(this.Position.X + width, this.Position.Y + height), Color, 3f);
            spriteBatch.DrawLine(this.Position, new Vector2(this.Position.X, this.Position.Y + height), Color, 3f);
            spriteBatch.DrawLine(new Vector2(this.Position.X, this.Position.Y + height),
                new Vector2(this.Position.X + width, this.Position.Y + height), Color, 3f);
        }
    }
}
