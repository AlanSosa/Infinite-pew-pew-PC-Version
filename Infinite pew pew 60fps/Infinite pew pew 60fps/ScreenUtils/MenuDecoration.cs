using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ThridWolrdShooterGame.Managers;
using ThridWolrdShooterGame.Entites;
using GameStateManagement;
using MyExtensions;

namespace MenuScreen
{
    public class MenuDecoration : Entity
    {
        Rectangle BoundsScreen;
        Random random = new Random();

        int width;
        int height;

        float spawnOffset;

        GameScreen screen;
        public MenuDecoration(float spawnOffset,Vector2 Position , Vector2 Velocity, Texture2D Texture, Color Color, Rectangle Bounds, GameScreen screen)
        {
            this.texture = Texture;
            this.Position = Position;
            this.Velocity = Velocity;
            this.Color = Color;
            width = height = random.Next(10, 100);
            this.screen = screen;
            this.spawnOffset = spawnOffset;

            this.BoundsScreen = new Rectangle((int)(Bounds.X - spawnOffset), (int)(Bounds.Y - spawnOffset), Bounds.Width + (int)spawnOffset*3, Bounds.Height + (int)spawnOffset*3);
        }

        public override void Update(GameTime gametime)
        {
            this.Position += Velocity;
            this.Orientation -= 0.05f;

            if(!BoundsScreen.Contains(this.Position.ToPoint()) )
                this.IsExpired = true;
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, this.Position,new Rectangle(0,0, width, height),Color * screen.TransitionAlpha,Orientation, new Vector2(width,height) / 2f,1f,SpriteEffects.None,0f);
        }
    }
}
