using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ThridWolrdShooterGame.Managers;

namespace ThridWolrdShooterGame.Entites
{
    public class Core : Entity
    {
        public float energy { get; set; }
        private int energyTimer;

        public static Core Instance
        {
            get
            {
                if (instance == null)
                    instance = new Core();
                return instance;
            }
        }
        private static Core instance;


        private Core()
        {
            this.texture = EntityManager.Resources.Core;
            this.Position = Position;
            this.Orientation = 0;
            this.texture = texture;
            this.Radius = texture.Width / 2;
            this.Position = GameplayScreen.WorldSize / 2;
            energy = 0;
        }

        public void WasShot()
        {
            energy -= 01f;
        }

        public void CrashedWithEnemy()
        {
            energy += 05f;
        }

        public override void Update(GameTime gametime)
        {
            energyTimer += gametime.ElapsedGameTime.Milliseconds;

            if (energyTimer >= 500)
            {
                energyTimer = 0;
                energy += .01f;
            }

            if (energy >= 1)
                energy = 1;
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            float scale = energy + 0.1f * (float)Math.Sin(10 * GameplayScreen.GameTime.TotalGameTime.TotalSeconds);
            spriteBatch.Draw(texture, Position, null, Color, Orientation, Size / 2f, scale, 0, 0);
        }
    }
}
