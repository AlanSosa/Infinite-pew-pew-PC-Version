using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyExtensions;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using ThridWolrdShooterGame.Managers;

namespace ThridWolrdShooterGame.Entites
{
    public class Bullet:Entity
    {
        public enum BULLETTYPE { ENEMY, PLAYER };
        public BULLETTYPE Type;
        protected int hp;

        private bool bouncing;
        private int bounceTimes;

        public Bullet(Texture2D texture, Vector2 position, Vector2 velocity, int bulletHp)
        {
            this.texture = texture;
            this.Position = position;
            this.Velocity = velocity;
            Orientation = Velocity.ToAngle();
            Radius = EntityManager.Resources.Bullet.Width / 2;
            this.hp = bulletHp;
        }

        public Bullet(Texture2D texture, Vector2 position, Vector2 velocity, BULLETTYPE type, int bulletHp)
        {
            this.texture = texture;
            this.Position = position;
            this.Velocity = velocity;
            Orientation = Velocity.ToAngle();
            Radius = 10;
            this.Type = type;
            this.hp = bulletHp;
        }

        public Bullet(Texture2D texture, Vector2 position, Vector2 velocity, BULLETTYPE type, int bulletHp, bool bouncing)
        {
            this.texture = texture;
            this.Position = position;
            this.Velocity = velocity;
            Orientation = Velocity.ToAngle();
            Radius = 10;
            this.Type = type;
            this.hp = bulletHp;
            this.bouncing = bouncing;
            bounceTimes = 3;
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gametime)
        {
            if (Velocity.LengthSquared() > 0)
                Orientation = Velocity.ToAngle();

            Position += (Velocity * GameplayScreen.SlowMoMultiplier);

            if (!GameplayScreen.World.Bounds.Contains(this.Position.ToPoint()))
            {
                if (bouncing)
                {
                    this.Velocity = Velocity.ScaleTo(-25);
                    bounceTimes--;
                    if (bounceTimes <= 0)
                        IsExpired = true;
                }
                else
                {
                    IsExpired = true;
                }
            }


            Random rand = new Random();
            if (IsExpired)
            {
                if(!GameplayScreen.GameTime.IsRunningSlowly)
                    for (int i = 0; i < 10; i++)
                        GameplayScreen.ParticleManager.CreateParticle(EntityManager.Resources.Particle, Position, Color.LightBlue, 50, 1,
                            new ParticleState()
                            {
                                Velocity = rand.NextVector2(0, 12.5f),
                                Type = ParticleType.BULLET,
                                LengthMultiplier = 1
                            });
            }

            //makeBulletTrace();
        }

        private void makeBulletTrace()
        {
            if (Velocity.LengthSquared() > 0.1f)
            {
                Vector2 baseVel = Velocity.ScaleTo(-3);
                Quaternion rot = Quaternion.CreateFromYawPitchRoll(0f,0f, Orientation);

                Vector2 pos = Position + Vector2.Transform(new Vector2(-15, 0f), rot);
                const float alpha = 0.7f;

                GameplayScreen.ParticleManager.CreateParticle(EntityManager.Resources.SquareParticle, 
                    pos,
                    Color.White * alpha, 
                    30f, 
                    new Vector2(0.5f, 1), 
                    new ParticleState(baseVel, ParticleType.BULLET)
                    );
            }
        }

        public virtual void WasDamaged()
        {
            this.hp--;
            if (hp <= 0)
                this.IsExpired = true;
        }
    }
}
