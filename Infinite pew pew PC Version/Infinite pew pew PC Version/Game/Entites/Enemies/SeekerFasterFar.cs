using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using ThridWolrdShooterGame.Managers;

namespace ThridWolrdShooterGame.Entites.Enemies
{
    public class SeekerFasterFar : Enemy
    {
        public SeekerFasterFar(Vector2 Position)
        {
            this.texture = EntityManager.Resources.SeekerFar;
            this.Position = Position;
            this.Radius = this.texture.Width / 2;
            this.Color = Color.White * 0f;
            this.PointValue = 150;
            this.HP = 1;
            this.AddBehaviour(this.FollowFasterWhenFar());
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
