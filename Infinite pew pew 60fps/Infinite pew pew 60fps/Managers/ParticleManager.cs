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
    public class ParticleManager<T>
    {
        // This delegate will be called for each particle.
        private Action<Particle> updateParticle;
        private CircularParticleArray particleList;

        /// <summary>
        /// Allows creation of particles.
        /// </summary>
        /// <param name="capacity">The maximum number of particles. An array of this size will be pre-allocated.</param>
        /// <param name="updateParticle">A delegate that lets you specify custom behaviour for your particles. Called once per particle, per frame.</param>
        public ParticleManager(int capacity, Action<Particle> updateParticle)
        {
            this.updateParticle = updateParticle;
            particleList = new CircularParticleArray(capacity);

            // Populate the list with empty particle objects, for reuse.
            for (int i = 0; i < capacity; i++)
                particleList[i] = new Particle();
        }

        /// <summary>
        /// Update particle state, to be called every frame.
        /// </summary>
        public void Update()
        {
            int removalCount = 0;
            for (int i = 0; i < particleList.Count; i++)
            {
                var particle = particleList[i];
                updateParticle(particle);
                particle.PercentLife -= 1f / particle.Duration;

                // sift deleted particles to the end of the list
                Swap(particleList, i - removalCount, i);

                // if the particle has expired, delete this particle
                if (particle.PercentLife < 0)
                    removalCount++;
            }
            particleList.Count -= removalCount;
        }

        private static void Swap(CircularParticleArray list, int index1, int index2)
        {
            var temp = list[index1];
            list[index1] = list[index2];
            list[index2] = temp;
        }

        /// <summary>
        /// Draw the particles.
        /// </summary>
        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < particleList.Count; i++)
            {
                var particle = particleList[i];

                Vector2 origin = new Vector2(particle.Texture.Width / 2, particle.Texture.Height / 2);
                spriteBatch.Draw(particle.Texture, particle.Position, null, particle.Color, particle.Orientation, origin, particle.Scale, 0, 0);
            }
        }

        public void CreateParticle(Texture2D texture, Vector2 position, Color tint, float duration, float scale, T state, float theta)
        {
            CreateParticle(texture, position, tint, duration, new Vector2(scale), state, theta);
        }

        public void CreateParticle(Texture2D texture, Vector2 position, Color tint, float duration, float scale, T state)
        {
            CreateParticle(texture, position, tint, duration, new Vector2(scale), state, 0);
        }

        public void CreateParticle(Texture2D texture, Vector2 position, Color tint, float duration, Vector2 scale, T state, float theta)
        {
            var particle = new Particle()
            {
                Texture = texture,
                Position = position,
                Color = tint,
                Duration = duration,
                Scale = scale,
                Orientation = theta,
                State = state
            };

            if (particleList.Count == particleList.Capacity)
            {
                // if the list is full, overwrite the oldest particle, and rotate the circular list
                particleList[0] = particle;
                particleList.Start++;
            }
            else
            {
                particleList[particleList.Count] = particle;
                particleList.Count++;
            }

     
        }

        public void CreateParticle(Texture2D texture, Vector2 position, Color tint, float duration, Vector2 scale, T state)
        {
            var particle = new Particle()
            {
                Texture = texture,
                Position = position,
                Color = tint,
                Duration = duration,
                Scale = scale,
                Orientation = 0,
                State = state
            };

            if (particleList.Count == particleList.Capacity)
            {
                // if the list is full, overwrite the oldest particle, and rotate the circular list
                particleList[0] = particle;
                particleList.Start++;
            }
            else
            {
                particleList[particleList.Count] = particle;
                particleList.Count++;
            }


        }

      
        public class Particle
        {
            public Texture2D Texture;
            public Vector2 Position;
            public float Orientation;

            public Vector2 Scale = Vector2.One;

            public Color Color;
            public float Duration;
            public float PercentLife = 1f;
            public T State;
        }

        // Represents a circular array with an arbitrary starting point. It's useful for efficiently overwriting
        // the oldest particles when the array gets full. Simply overwrite particleList[0] and advance Start.
        private class CircularParticleArray
        {
            private int start;
            public int Start
            {
                get { return start; }
                set { start = value % list.Length; }
            }

            public int Count { get; set; }
            public int Capacity { get { return list.Length; } }
            private Particle[] list;

            public CircularParticleArray() { }  // for serialization

            public CircularParticleArray(int capacity)
            {
                list = new Particle[capacity];
            }

            public Particle this[int i]
            {
                get { return list[(start + i) % list.Length]; }
                set { list[(start + i) % list.Length] = value; }
            }
        }
    }
}
