using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using MyExtensions;
using ThridWolrdShooterGame.Managers;

namespace ThridWolrdShooterGame.Entites.Enemies
{
    public class SquareMini : Enemy
    {
        private Color newColor;

        public SquareMini() { }

        public SquareMini(Vector2 Position)
        {
            Color colorNew = new Color(rand.NextFloat(0, 1f), rand.NextFloat(0, 1f), rand.NextFloat(0, 1f));
            this.texture = EntityManager.Resources.SquareMini;
            this.Position = Position;
            this.Radius = this.texture.Width / 2;
            //this.newColor = colorNew;
            this.PointValue = 100;
            this.HP = 2;
            this.AddBehaviour(this.FollowPlayerWithNoOrientation(0.01f));
            this.newColor = new Color(rand.Next(0, 255), rand.Next(0, 255), rand.Next(0, 255));
        }
        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);

            if (this.IsActive)
                this.Color = newColor;
        }

        public override void Kill()
        {
            base.Kill();
        }
        public override void WasShot()
        {
            base.WasShot();
            if (HP <= 0)
                Kill();
        }
    }
}
