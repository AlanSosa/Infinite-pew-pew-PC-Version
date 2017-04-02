using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using ThridWolrdShooterGame.Managers;

namespace ThridWolrdShooterGame.Entites.Enemies
{
    public class Worm:Entity
    {
        private Enemy[] components = new Enemy[4];

        public Worm(Vector2 Position)
        {
            this.Position = Position;

            components[0] = new WormHead(this.Position);
            for (int i = 0; i < components.Count(); i++)
            {
                if (i == 0)
                {
                    components[i] = new WormHead(this.Position);
                    EntityManager.Add(components[i]);
                    continue;
                }
                components[i] = new WormTail(this.Position);
                EntityManager.Add(components[i]);
            } 
        }

        private int expiredCounter;
        public override void Update(GameTime gametime)
        {
            expiredCounter = 0;
            for (int i = 1; i < components.Count(); i++)
            {
                if (i == 1)
                {
                    components[i].Position = (components[0] as WormHead).LastPosition;
                    if ((components[0] as WormHead).IsExpired)
                    {
                        (components[i] as WormTail).Kill();
                        expiredCounter++;
                    }
                    continue;
                }

                components[i].Position = (components[i - 1] as WormTail).LastPosition ;
                if ((components[i - 1] as WormTail).IsExpired)
                {
                    (components[i] as WormTail).Kill();
                    expiredCounter++;
                }
            }

            if (expiredCounter >= components.Count() - 1)
                this.IsExpired = true;

            Debug.WriteLine("Counter " +expiredCounter);
        }

    }
}
