using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using MyExtensions;
using ThridWolrdShooterGame.Managers;

namespace ThridWolrdShooterGame.Entites.Items
{
    public class ItemPointer:Entity
    {
        private Item targetItem;

        public ItemPointer(Vector2 Position, Item Target)
        {
            this.targetItem = Target;
            this.Position = Position;
            this.texture = EntityManager.Resources.EnemyItem;
        }


        public override void Update(GameTime gametime)
        {
            if (targetItem != null)
            {
                this.Velocity = (targetItem.Position - this.Position);
                if (Velocity != Vector2.Zero)
                    this.Orientation = Velocity.ToAngle();

                Quaternion offsetQuat = Quaternion.CreateFromYawPitchRoll(0, 0, Orientation);

                Vector2 offset = Vector2.Transform( new Vector2(35,-1), offsetQuat);

                this.Position = EntityManager.PlayerShip.Position + offset;

                //this.Position = EntityManager.PlayerShip.Position;
                //this.Position = positoning the Arrow;

                if (targetItem.IsExpired)
                    this.IsExpired = true;
            }

        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }
}
