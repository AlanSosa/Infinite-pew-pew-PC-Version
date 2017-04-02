#region Using Statements
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameStateManagement.Screens;
#endregion

namespace GameStateManagement
{
    /// <summary>
    /// Helper class represents a single entry in a MenuScreen. By default this
    /// just draws the entry text string, but it can be customized to display menu
    /// entries in different ways. This also provides an event that will be raised
    /// when the menu entry is selected.
    /// </summary>
    class TouchableEntry
    {
        #region Fields

        /// <summary>
        /// The text rendered for this entry.
        /// </summary>
        string text;

        /// <summary>
        /// Tracks a fading selection effect on the entry.
        /// </summary>
        /// <remarks>
        /// The entries transition out of the selection effect when they are deselected.
        /// </remarks>
        float selectionFade;

        /// <summary>
        /// The position at which the entry is drawn. This is set by the MenuScreen
        /// each frame in Update.
        /// </summary>
        Vector2 position;

        // the number of pixels to pad above and below menu entries for touch input
        int menuEntryPadding;

        #endregion

        #region Properties


        /// <summary>
        /// Gets or sets the text of this menu entry.
        /// </summary>
        public string Text
        {
            get { return text; }
            set { text = value; }
        }


        /// <summary>
        /// Gets or sets the position at which to draw this menu entry.
        /// </summary>
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }


        public bool FixedPosition { get; set; }

        #endregion

        public int Number;
        #region Events


        /// <summary>
        /// Event raised when the menu entry is selected.
        /// </summary>
        public event EventHandler<PlayerIndexEventArgs> Selected;


        /// <summary>
        /// Method for raising the Selected event.
        /// </summary>
        protected internal virtual void OnSelectEntry(PlayerIndex playerIndex)
        {
            if (Selected != null)
                Selected(this, new PlayerIndexEventArgs(playerIndex));
        }

        /// <summary>
        /// Method for checking if the event is null
        /// </summary>
        /// <returns></returns>
        protected internal bool IsEventNull()
        {
            return Selected == null;
        }


        #endregion

        #region Initialization


        /// <summary>
        /// Constructs a new menu entry with the specified text.
        /// </summary>
        public TouchableEntry(string text, int menuEntryPadding)
        {
            this.text = text;
            this.menuEntryPadding = menuEntryPadding;
            this.FixedPosition = false;
        }

        /// <summary>
        /// Constructs a new menu entry with the specified text.
        /// </summary>
        public TouchableEntry(string text, int menuEntryPadding,bool fixedPosition)
        {
            this.text = text;
            this.menuEntryPadding = menuEntryPadding;
            this.FixedPosition = fixedPosition;
        }

        #endregion

        #region Update and Draw


        /// <summary>
        /// Updates the menu entry.
        /// </summary>
        public virtual void Update(GameScreen screen, bool isSelected, GameTime gameTime)
        {
            // there is no such thing as a selected item on Windows Phone, so we always
            // force isSelected to be false
#if WINDOWS_PHONE
            isSelected = false;
#endif
            // When the menu selection changes, entries gradually fade between
            // their selected and deselected appearance, rather than instantly
            // popping to the new state.
            float fadeSpeed = (float)gameTime.ElapsedGameTime.TotalSeconds * 4;

            if (isSelected)
                selectionFade = Math.Min(selectionFade + fadeSpeed, 1);
            else
                selectionFade = Math.Max(selectionFade - fadeSpeed, 0);
        }

        /// <summary>
        /// Draws the menu entry. This can be overridden to customize the appearance.
        /// </summary>
        public virtual void Draw(GameScreen screen, bool isSelected, GameTime gameTime)
        {
            // there is no such thing as a selected item on Windows Phone, so we always
            // force isSelected to be false
#if WINDOWS_PHONE
            isSelected = false;
#endif

            // Draw the selected entry in yellow, otherwise white.
            Color color = isSelected ? Color.Yellow : Color.White;

            // Pulsate the size of the selected menu entry.
            double time = gameTime.TotalGameTime.TotalSeconds;
            
            float pulsate = (float)Math.Sin(time * 6) + 1;

            float scale = 1 + pulsate * 0.05f * selectionFade;

            // Modify the alpha to fade text out during transitions.
            color *= screen.TransitionAlpha;

            // Draw text, centered on the middle of each line.
            ScreenManager screenManager = screen.ScreenManager;
            SpriteBatch spriteBatch = screenManager.SpriteBatch;
            SpriteFont font = screenManager.Font;
            Texture2D pixel = screenManager.Pixel;
            Vector2 origin = new Vector2(0, font.LineSpacing / 2);

            /*float lineThickness = 3f;

            Color blackLine = (Color.Black * 0.1f) * screen.TransitionAlpha;
            Color whiteLine = (Color.White * 0.6f) * screen.TransitionAlpha;*/
            spriteBatch.Draw(pixel,
                    GetMenuEntryHitBounds(screen),
                    (Color.White * 0.3f) * screen.TransitionAlpha);

            if (screen.GetType() != typeof(LeaderBoardsScreen))
            {
                spriteBatch.DrawString(font, text, position, color, 0,
                                       origin, scale, SpriteEffects.None, 0);
            }
            else
            {

                spriteBatch.DrawString(font, Number+"", position +  new Vector2(GetWidth(screen) - font.MeasureString(Number+"").X,0), color, 0,
                                       origin, 3f, SpriteEffects.None, 0);
                spriteBatch.DrawString(font, text, position, color, 0,
                                       origin, 0.8f, SpriteEffects.None, 0);
            }
            /*
            // Top lines
            screen.DrawLine(spriteBatch,
                new Vector2(GetMenuEntryHitBounds(screen).X - lineThickness, GetMenuEntryHitBounds(screen).Y - lineThickness),
                new Vector2(GetMenuEntryHitBounds(screen).X + GetMenuEntryHitBounds(screen).Width + lineThickness, GetMenuEntryHitBounds(screen).Y - lineThickness)
                ,whiteLine, lineThickness);

            screen.DrawLine(spriteBatch,
                new Vector2(GetMenuEntryHitBounds(screen).X - lineThickness * 2, GetMenuEntryHitBounds(screen).Y - lineThickness * 2),
                new Vector2(GetMenuEntryHitBounds(screen).X + GetMenuEntryHitBounds(screen).Width + lineThickness * 2, GetMenuEntryHitBounds(screen).Y - lineThickness * 2)
                , blackLine, lineThickness);

            // Most down lines

            screen.DrawLine(spriteBatch,
                new Vector2(GetMenuEntryHitBounds(screen).X  - lineThickness, GetMenuEntryHitBounds(screen).Y + GetMenuEntryHitBounds(screen).Height + lineThickness),
                new Vector2(GetMenuEntryHitBounds(screen).X + GetMenuEntryHitBounds(screen).Width + lineThickness, GetMenuEntryHitBounds(screen).Y + GetMenuEntryHitBounds(screen).Height + lineThickness)
                , whiteLine, lineThickness);

            screen.DrawLine(spriteBatch,
                new Vector2(GetMenuEntryHitBounds(screen).X - lineThickness * 2, GetMenuEntryHitBounds(screen).Y - lineThickness * 2),
                new Vector2(GetMenuEntryHitBounds(screen).X + GetMenuEntryHitBounds(screen).Width + lineThickness * 2, GetMenuEntryHitBounds(screen).Y - lineThickness * 2)
                , blackLine, lineThickness);*/

        }

        public virtual Rectangle GetMenuEntryHitBounds(GameScreen screen)
        {
            // the hit bounds are the entire width of the screen, and the height of the entry
            // with some additional padding above and below.
            return new Rectangle(
                (int)this.Position.X - (menuEntryPadding ),
                (int)this.Position.Y - (menuEntryPadding +  menuEntryPadding / 4),
                GetWidth(screen) + (menuEntryPadding * 2),
                GetHeight(screen) + (menuEntryPadding * 2) / 4
                );
        }

        /// <summary>
        /// Queries how much space this menu entry requires.
        /// </summary>
        public virtual int GetHeight(GameScreen screen)
        {
            return screen.ScreenManager.Font.LineSpacing;
        }


        /// <summary>
        /// Queries how wide the entry is, used for centering on the screen.
        /// </summary>
        public virtual int GetWidth(GameScreen screen)
        {
            return (int)screen.ScreenManager.Font.MeasureString(Text).X;
        }


        #endregion
    }
}
