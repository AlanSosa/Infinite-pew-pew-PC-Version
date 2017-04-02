using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameStateManagement;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Third_World_Shooter.Screens
{
    class ImageTouchableEntry:TouchableEntry
    {
        private Texture2D texture;

        public ImageTouchableEntry(Texture2D Texture, Vector2 Position,int menuEntryPadding):
            base(String.Empty, menuEntryPadding,true)
        {
            this.Position = Position;
            this.texture = Texture;
        }

        public override void Update(GameScreen screen, bool isSelected, Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(screen, isSelected, gameTime);
        }

        public override void Draw(GameScreen screen, bool isSelected, Microsoft.Xna.Framework.GameTime gameTime)
        {

            // Pulsate the size of the selected menu entry.
            double time = gameTime.TotalGameTime.TotalSeconds;

            float pulsate = (float)Math.Sin(time * 6) + 1;
            float scale = isSelected ? 1 + pulsate * 0.1f : 1f;

            // Draw text, centered on the middle of each line.
            ScreenManager screenManager = screen.ScreenManager;
            SpriteBatch spriteBatch = screenManager.SpriteBatch;
            SpriteFont font = screenManager.Font;
            Texture2D pixel = screenManager.Pixel;
            Vector2 origin = new Vector2(0, font.LineSpacing / 2);

            spriteBatch.Draw(pixel, new Vector2(GetMenuEntryHitBounds(screen).X, GetMenuEntryHitBounds(screen).Y), new Rectangle(0, 0, texture.Width, texture.Height),
                Color.White * screen.TransitionAlpha, 0f, origin, scale, SpriteEffects.None, 0f); 

            //spriteBatch.Draw(this.texture, GetMenuEntryHitBounds(screen), Color.White * screen.TransitionAlpha);
            spriteBatch.Draw(this.texture, new Vector2(GetMenuEntryHitBounds(screen).X, GetMenuEntryHitBounds(screen).Y), new Rectangle(0,0, texture.Width, texture.Height),
                Color.White * screen.TransitionAlpha,0f, origin,scale, SpriteEffects.None, 0f); 
        }
    }
}
