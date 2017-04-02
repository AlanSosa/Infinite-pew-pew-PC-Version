using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ThridWolrdShooterGame.Entites;
using System.Diagnostics;
using ThridWolrdShooterGame.Managers;
using MyExtensions;

namespace ThridWolrdShooterGame
{
    public class Camera
    {
        public Matrix Transform;

        private Vector2 centre;
        private Viewport viewport;

        private float zoom = 1;
        private float rotation = 0;

        public float Zoom
        {
            get { return zoom; }
            set { zoom = value; if (zoom < 0.1f) zoom = 0.1f; }
        }

        public float Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }

        private static readonly Random random = new Random();
        private bool shaking;
        private float shakeMagnitude;
        private float shakeDuration;
        private float shakeTimer;
        private Vector3 shakeOffset;

        public Camera(Viewport viewport)
        {
            this.viewport = viewport;
            zoom = .65f;
            Transform = Matrix.Identity;
            //Perfect amount of zoom. ---> .8f
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="magnitude">The Force</param>
        /// <param name="duration">The duration in seconds</param>
        public void Shake(float magnitude, float duration)
        {
            shaking = true;

            shakeMagnitude = magnitude;
            shakeDuration = duration;

            shakeTimer = 0f;
        }


        private float nextFloat()
        {
            return (float)random.NextDouble() * 2f - 1f;
        }

        Vector2 cameraCenter = GameplayScreen.WorldSize / 2;
        Vector2 cameraVelocity;

        public void Update(GameTime gameTime)
        {
            cameraVelocity = ((Position - cameraCenter) * 0.085f);

            //cameraVelocity = ((Position - cameraCenter) * 0.00085f);

            cameraCenter += cameraVelocity;
            centre = cameraCenter;


            /*if (centre.X < 480)
                centre.X = 480;
            else if (centre.X > 530)
                centre.X = 530;

            if (centre.Y < 350)
                centre.Y = 350;
            else if (centre.Y > 450)
                centre.Y = 450;*/

            if (EntityManager.PlayerShip.IsDead)
            {
                zoom += .01f;

                if (zoom >= 1)
                    zoom = 1;
            }
            else
            {
                zoom -= .01f;

                if (zoom <= 0.6f)
                    zoom = .6f;
            }
            //centre = target.Position;
            //zoom = 1f

            if (shaking)
            {
                shakeTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (shakeTimer >= shakeDuration)
                {
                    shaking = false;
                    shakeTimer = shakeDuration;
                }

                float progress = shakeTimer / shakeDuration;
                float magnitude = shakeMagnitude * (1f - (progress * progress));

                shakeOffset = new Vector3(nextFloat(), nextFloat(), nextFloat()) * magnitude;
            }

            Transform = Matrix.CreateTranslation(new Vector3(-centre.X, -centre.Y, 0) + shakeOffset) * //Follow the position of the center
                Matrix.CreateRotationZ(rotation) * // this will rotate 
                    Matrix.CreateScale(new Vector3(zoom, zoom, 0)) * // This will scale, so it enlargens the images.
                    Matrix.CreateTranslation(new Vector3(viewport.Width / 2, viewport.Height / 2, 0)); // This tranlate the camera to the center of the screen.
        }

        public void LookAt(Vector2 position)
        {
            Position = position;
        }

        public Vector2 Position;

        public Matrix GetViewMatrix(Vector2 parallax)
        {
            return Matrix.CreateTranslation(new Vector3(Position * parallax, 0.0f)) *
            Matrix.CreateTranslation(new Vector3(-centre.X, -centre.Y, 0) + shakeOffset) * //Follow the position of the center
            Matrix.CreateRotationZ(rotation) * // this will rotate 
            Matrix.CreateScale(new Vector3(zoom, zoom, 0)) * // This will scale, so it enlargens the images.
            Matrix.CreateTranslation(new Vector3(viewport.Width / 2, viewport.Height / 2, 0)); // This tranlate the camera to the center of the screen.
        }
    }
}
