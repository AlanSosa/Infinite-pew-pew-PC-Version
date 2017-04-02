using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using MyExtensions;
using ThridWolrdShooterGame.Managers;

namespace ThridWolrdShooterGame.Entites.Enemies
{
    public class ItemEnemy:Enemy
    {
        private int[] movementTimeNorth = new int [4] {1000,1000,1000,1000};
        private int[] movementTimeSouth = new int[4] { 1000, 1000, 1000, 1000 };
        private int[] movementTimeEast = new int[4] { 1000, 1000, 1000, 1000 };
        private int[] movementTimeWest = new int[4] { 1000, 1000, 1000, 1000 };

        private Vector2[] directionsNorth = new Vector2[4] { new Vector2(0,1), new Vector2(1,0), new Vector2(0,-1), new Vector2(-1,0)};
        private Vector2[] directionsSouth = new Vector2[4] { new Vector2(0,-1), new Vector2(-1, 0), new Vector2(0, 1), new Vector2(1, 0) };
        private Vector2[] directionsEast = new Vector2[4] { new Vector2(-1, 0), new Vector2(0, 1), new Vector2(1, 0), new Vector2(0,-1) };
        private Vector2[] directionsWest = new Vector2[4] { new Vector2(1,0), new Vector2(0,-1), new Vector2(-1,0), new Vector2(0,1) };

        private int origin;
        private float speed;
        private int elapsedTime;
        private int index;


        public ItemEnemy()
        { }

        public ItemEnemy(Vector2 Position, int origin)
        {
            this.Position = Position;
            this.texture = EntityManager.Resources.EnemyItem;
            this.Radius = this.texture.Width / 2;
            this.HP = 2;
            this.PointValue = 500;
            this.Color = Color.White * 0f;
            this.origin = origin;
            this.speed = 18f;
            index = 0;
        }

        public override void Update(GameTime gameTime)
        {
            switch (origin)
            {
                case 0:
                    if (index <= 3)
                    {
                        elapsedTime += gameTime.ElapsedGameTime.Milliseconds;
                        Velocity = directionsNorth[index];
                        if (elapsedTime >= movementTimeNorth[index])
                        {
                            index++;
                            elapsedTime = 0;
                        }
                    }
                    break;
                case 1:
                    if (index <= 3)
                    {
                        elapsedTime += gameTime.ElapsedGameTime.Milliseconds;
                        Velocity = directionsSouth[index];
                        if (elapsedTime >= movementTimeSouth[index])
                        {
                            index++;
                            elapsedTime = 0;
                        }
                    }
                    break;
                case 2:
                    if (index <= 3)
                    {
                        elapsedTime += gameTime.ElapsedGameTime.Milliseconds;
                        Velocity = directionsEast[index];
                        if (elapsedTime >= movementTimeEast[index])
                        {
                            index++;
                            elapsedTime = 0;
                        }
                    }
                    break;
                case 3:
                    if (index <= 3)
                    {
                        elapsedTime += gameTime.ElapsedGameTime.Milliseconds;
                        Velocity = directionsWest[index];
                        if (elapsedTime >= movementTimeWest[index])
                        {
                            index++;
                            elapsedTime = 0;
                        }
                    }
                    break;
            }
            this.Position += Velocity * speed;

            if (Velocity.Length() > 0)
                Orientation = Velocity.ToAngle();
            
        }

        public override void WasShot()
        {
            base.WasShot();
            if (HP <= 0)
                Kill();
        }

        public override void Kill()
        {
            base.Kill();
        }
    }


}
