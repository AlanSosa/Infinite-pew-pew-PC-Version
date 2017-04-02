using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using ThridWolrdShooterGame.Managers;

namespace ThridWolrdShooterGame.Entites.Enemies
{
    public class WormHead : Enemy
    {
        public Vector2 LastPosition;

        public WormHead(Vector2 Position)
        {
            this.texture = EntityManager.Resources.Seeker;
            this.Position = Position;
            this.Radius = this.texture.Width / 2;
            this.Color = Color.White * 0f;
            this.PointValue = 100;
            this.HP = 1;
            this.AddBehaviour(this.MoveRandomlyVariant());
            this.LastPosition = Position;
        }
        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            LastPosition = Position + this.Size;
            base.Update(gameTime);
        }

        public override void Kill()
        {
            base.Kill();
        }
        public override void WasShot()
        {
            HP--;
            if (HP <= 0)
                Kill();
        }
    }
}
