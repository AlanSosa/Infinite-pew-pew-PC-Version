using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using ThridWolrdShooterGame.Managers;

namespace ThridWolrdShooterGame.Entites.Enemies
{
    public class Bouncer : Enemy
    {
        public Bouncer()
        { }

        public Bouncer(Vector2 Position)
        {
            this.texture = EntityManager.Resources.Bouncer;
            this.Position = Position;
            this.Radius = this.texture.Width / 2;
            this.Color = Color.White * 0f;
            this.PointValue = 150;
            this.HP = 1;
            this.AddBehaviour(this.BounceAndSpin(2f));
        }
        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);
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
