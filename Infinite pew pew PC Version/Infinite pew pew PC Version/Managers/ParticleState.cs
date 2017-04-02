using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using MyExtensions;

namespace ThridWolrdShooterGame
{
    public enum ParticleType { NONE, ENEMY, BULLET, IGNOREGRAVITY}

    public struct ParticleState
    {
        public Vector2 Velocity;
        public ParticleType Type;
        public float LengthMultiplier;

        private static Random rand = new Random();

        public ParticleState(Vector2 velocity, ParticleType type, float lengthMultiplier)
        {
            Velocity = velocity;
            Type = type;
            LengthMultiplier = lengthMultiplier;
        }

        public ParticleState(Vector2 velocity, ParticleType type)
        {
            Velocity = velocity;
            Type = type;
            LengthMultiplier = 1f;
        }

        public static ParticleState GetRandom(float minVel, float maxVel)
        {
            var state = new ParticleState();
            state.Velocity = rand.NextVector2(minVel, maxVel);
            state.Type = ParticleType.NONE;
            state.LengthMultiplier = 1;

            return state;
        }

        public static void UpdateParticle(ParticleManager<ParticleState>.Particle particle)
        {
            var vel = particle.State.Velocity;

            particle.Position += vel;
            //if(particle.State.Type == ParticleType.NONE || particle.State.Type == ParticleType.BULLET)
            particle.Orientation = vel.ToAngle();

            float speed = vel.Length();
            float alpha = Math.Min(1, Math.Min(particle.PercentLife * 2, speed * 1f));
            alpha *= alpha;

            particle.Color.A = (byte)(255 * alpha);

            particle.Scale.X = particle.State.LengthMultiplier * Math.Min(Math.Min(1f, 0.2f * speed + 0.1f), alpha);
            particle.Scale.Y = particle.State.LengthMultiplier * Math.Min(Math.Min(1f, 0.2f * speed + 0.1f), alpha);
            // denormalized float cause significant performance issues

            if (Math.Abs(vel.X) + Math.Abs(vel.Y) < 0.00000000001f)
                vel = Vector2.Zero;

            if (particle.State.Type == ParticleType.BULLET)
            {
                var pos = particle.Position;

                if (pos.X < 0)
                    vel.X = Math.Abs(vel.X);
                else if (pos.X > GameplayScreen.WorldSize.X)
                    vel.X = -Math.Abs(vel.X);
                if (pos.Y < 0)
                    vel.Y = Math.Abs(vel.Y);
                else if (pos.Y > GameplayScreen.WorldSize.Y)
                    vel.Y = -Math.Abs(vel.Y);

                particle.Position = Vector2.Clamp(particle.Position, new Vector2(particle.Texture.Width, particle.Texture.Height) / 2, GameplayScreen.WorldSize - new Vector2(particle.Texture.Width, particle.Texture.Height) / 2);
            }

            vel *= 0.97f;       // particles gradually slow down
            particle.State.Velocity = vel;
        }

    }
}
