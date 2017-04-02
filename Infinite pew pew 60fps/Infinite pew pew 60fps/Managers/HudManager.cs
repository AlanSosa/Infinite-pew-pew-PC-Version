using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ThridWolrdShooterGame.Entites.Messages;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ThridWolrdShooterGame.Entites;
using ThridWolrdShooterGame.Entites.Items;

namespace ThridWolrdShooterGame.Managers
{
    public static class HudManager
    {
        public static List<Entity> messages;
        public static List<Entity> messagesToAdd;
        private static bool isUpdating;

        public static CounterMeter CounterMeter
        {
            get;
            private set;
        }

        public static BonusClock BonusClock
        {
            get;
            private set;
        }

        public static void Init()
        {
            messages = new List<Entity>();
            messagesToAdd = new List<Entity>();
        }

        public static void Add(Entity entity)
        {
            if (entity is CounterMeter)
                CounterMeter = entity as CounterMeter;
            else if (entity is BonusClock)
                BonusClock = entity as BonusClock;

            if (!isUpdating)
                addMessage(entity);
            else
                messagesToAdd.Add(entity);
        }

        private  static void addMessage(Entity entity)
        {
            messages.Add(entity);
        }


        public static void Update(GameTime gameTime)
        {
            isUpdating = true;

            foreach (var message in messages)
                message.Update(gameTime);

            isUpdating = false;

            foreach (var message in messagesToAdd)
                addMessage(message);

            messagesToAdd.Clear();
            messages = messages.Where(message => !message.IsExpired).ToList();
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            foreach (var message in messages)
            {
                message.Draw(spriteBatch);
            }
        }

        public static void Unload()
        {
            messages.ForEach(message => message.Dispose());
            messagesToAdd = new List<Entity>();
            BonusClock = null;
            CounterMeter = null; 
        }
    }
}
