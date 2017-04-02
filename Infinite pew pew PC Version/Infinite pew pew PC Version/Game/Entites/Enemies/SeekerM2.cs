using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyExtensions;
using Microsoft.Xna.Framework;
using ThridWolrdShooterGame.Managers;

namespace ThridWolrdShooterGame.Entites.Enemies
{
    public class SeekerM2 : Enemy
    {
        public SeekerM2() { }

        public SeekerM2(Vector2 Position)
        {
            this.texture = EntityManager.Resources.SeekerM2;
            this.Position = Position;
            this.Radius = this.texture.Width;
            this.Color = Color.White * 0f;
            this.PointValue = 100;
            this.HP = 1;
            this.AddBehaviour(this.FollowAndSpin(2.25f, 1f));
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
