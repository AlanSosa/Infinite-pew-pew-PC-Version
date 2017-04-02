using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using ThridWolrdShooterGame.Entites.Messages;
using ThridWolrdShooterGame.Managers;
using Microsoft.Xna.Framework.Graphics;

namespace ThridWolrdShooterGame.Entites.Items
{
    public class Item:Entity
    {
        public ItemSpawner.ITEMS type;
        private int timeUntilStart;
        string message;
       
        public Item(Vector2 Position, ItemSpawner.ITEMS itemType, Texture2D texture)
        {
            this.type = itemType;
            this.Position = Position;
            this.texture = texture;
            timeUntilStart = 60;
            this.Radius = texture.Width;
            message = itemType.ToString();
        }

        public Item(Vector2 Position, ItemSpawner.ITEMS itemType, string message, Texture2D texture)
        {
            this.type = itemType;
            this.Position = Position;
            this.texture = texture;
            timeUntilStart = 60;
            this.Radius = texture.Width;
            this.message = message;
        }

        public override void Update(GameTime gametime)
        {

            if (timeUntilStart > 0)
                timeUntilStart--;

            this.Position = Vector2.Clamp(Position, Size /2 , GameplayScreen.WorldSize - Size / 2);

            if(IsExpired)
                EntityManager.Add(new FadingOutMessage(this.Position, message+ "!", 120));
            this.Position = Vector2.Clamp(Position, Size / 2, GameplayScreen.WorldSize - Size / 2);
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            if (timeUntilStart > 0)
            {
                if (timeUntilStart > 0)
                {
                    // Draw an expanding, fading-out version of the sprite as part of the spawn-in effect.
                    float factor = timeUntilStart / 60f;	// decreases from 1 to 0 as the enemy spawns in
                    spriteBatch.Draw(this.texture, this.Position, null, Color, Orientation, Size / 2f, factor, 0, 0);
                    //spriteBatch.Draw(texture, Position, null, Color.White * factor, Orientation, Size / 2f, 2.5f - factor, 0, 0);
                }
            }
            float scale = 2 + 0.1f * ((float)Math.Sin(10 * GameplayScreen.GameTime.TotalGameTime.TotalSeconds) * GameplayScreen.SlowMoMultiplier);
            spriteBatch.Draw(this.texture, Position, null, Color, Orientation, Size / 2f, scale, 0, 0);
        }

        public void HandleCollision(Entity other)
        {
            var d = Position - other.Position;
            Position.X= 20 * d.X / (d.LengthSquared() + 1);
        }

    }
}
