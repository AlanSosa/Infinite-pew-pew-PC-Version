using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using ThridWolrdShooterGame.Managers;

namespace ThridWolrdShooterGame.Entites.Enemies
{
    public class WormTail : Enemy
    {
        public Vector2 LastPosition;
        private bool collidingWithParent;

        public WormTail(Vector2 Position)
        {
            this.texture = EntityManager.Resources.Seeker;
            this.Position = Position;
            this.Radius = this.texture.Width / 2;
            this.Color = Color.White * 0f;
            this.PointValue = 100;
            this.HP = 1;
            LastPosition = this.Position;
            collidingWithParent = false;
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if(!collidingWithParent)
                LastPosition = Position + this.Size;
        }

        public override void Kill()
        {
            base.Kill();
        }
        public override void WasShot()
        {
            
        }
    }
}
