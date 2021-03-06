#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using Third_World_Shooter.Screens;
using MyExtensions;
using TechnicalData;
#endregion

namespace GameStateManagement
{
    /// <summary>
    /// Base class for screens that contain a menu of options. The user can
    /// move up and down to select an entry, or cancel to back out of the screen.
    /// </summary>
    abstract class MenuScreen : GameScreen
    {
        #region Fields

        // the number of pixels to pad above and below menu entries for touch input
        int menuEntryPadding = 20;

        List<TouchableEntry> menuEntries = new List<TouchableEntry>();
        List<ImageTouchableEntry> imageEntries = new List<ImageTouchableEntry>();
        protected Vector2 EntriesPosition = Vector2.Zero;
        int selectedEntry = 0;
        int screenType = 0;
        string menuTitle;

        Rectangle entriesRectangle;


        #endregion

        #region Properties


        /// <summary>
        /// Gets the list of menu entries, so derived classes can add
        /// or change the menu contents.
        /// </summary>
        protected IList<TouchableEntry> MenuEntries
        {
            get { return menuEntries; }
        }

        protected IList<ImageTouchableEntry> ImageEntries
        {
            get
            {
                return imageEntries;
            }
        }


        #endregion

        #region Initialization


        /// <summary>
        /// Constructor.
        /// </summary>
        public MenuScreen(string menuTitle, int screenType)
        {
            this.menuTitle = menuTitle;

            TransitionOnTime = TimeSpan.FromSeconds(0.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

            this.screenType = screenType;
        }

        public override void  LoadContent()
        {
 	        base.LoadContent();
            entriesRectangle = new Rectangle(ScreenManager.GraphicsDevice.Viewport.Width / 2 , 0 , ScreenManager.GraphicsDevice.Viewport.Width / 2 , ScreenManager.GraphicsDevice.Viewport.Height);
        }


        #endregion

        #region Handle Input



        /// <summary>
        /// Responds to user input, changing the selected entry and accepting
        /// or cancelling the menu.
        /// </summary>
        public override void HandleInput(InputState input)
        {
            // we cancel the current menu screen if the user presses the back button
            PlayerIndex player;

            if (input.IsNewButtonPress(Buttons.Back, ControllingPlayer, out player))
            {
                OnCancel(player);
                Debug.WriteLine("A calling from " + this);
            }
            
            // Move to the previous menu entry?
            if (input.IsMenuUp(player))
            {
                selectedEntry--;

                if (selectedEntry < 0)
                    selectedEntry = menuEntries.Count - 1;
            }

            // Move to the next menu entry?
            if (input.IsMenuDown(player))
            {
                selectedEntry++;

                if (selectedEntry >= menuEntries.Count)
                    selectedEntry = 0;
            }

            if (input.IsMenuSelect(player,out player))
            {
                OnSelectEntry(selectedEntry, player);
            }
            else if (input.IsMenuCancel(player, out player))
            {
                Debug.WriteLine("B calling from " + this);
                OnCancel(player);
            }

            if (input.LeftMouseButtonClicked)
            {
                for (int i = 0; i < imageEntries.Count; i++)
                {
                    ImageTouchableEntry imageEntry = imageEntries[i];

                    if(imageEntry.GetMenuEntryHitBounds(this).Contains(input.MousePosition.ToPoint()))
                    {
                        OnSelectImageEntry(i, PlayerIndex.One);
                    }
                }
            }
        }

        protected virtual void OnSelectImageEntry(int entryIndex, PlayerIndex playerIndex)
        {
            imageEntries[entryIndex].OnSelectEntry(playerIndex);
        }

        /// <summary>
        /// Handler for when the user has chosen a menu entry.
        /// </summary>
        protected virtual void OnSelectEntry(int entryIndex, PlayerIndex playerIndex)
        {
            menuEntries[entryIndex].OnSelectEntry(playerIndex);
        }


        /// <summary>
        /// Handler for when the user has cancelled the menu.
        /// </summary>
        protected virtual void OnCancel(PlayerIndex playerIndex)
        {
            if (GameStatics.sounds)
                ScreenManager.playSelect();
            ExitScreen();
        }


        /// <summary>
        /// Helper overload makes it easy to use OnCancel as a MenuEntry event handler.
        /// </summary>
        protected void OnCancel(object sender, PlayerIndexEventArgs e)
        {
            OnCancel(e.PlayerIndex);
        }


        #endregion

        #region Update and Draw


        /// <summary>
        /// Allows the screen the chance to position the menu entries. By default
        /// all menu entries are lined up in a vertical list, centered on the screen.
        /// </summary>
        protected virtual void UpdateMenuEntryLocations()
        {
            // Make the menu slide into place during transitions, using a
            // power curve to make things look more interesting (this makes
            // the movement slow down as it nears the end).
            float transitionOffset = (float)Math.Pow(TransitionPosition, 2);

            // start at Y = 175; each X value is generated per entry
            Vector2 position = EntriesPosition = EntriesPosition == Vector2.Zero ? new Vector2(60f, 80f): EntriesPosition;

            // update each menu entry's location in turn
            for (int i = 0; i < menuEntries.Count; i++)
            {
                TouchableEntry menuEntry = menuEntries[i];

                if (menuEntry.FixedPosition)
                    continue;
                float entriesOffsetX = EntriesPosition.X;
                // each entry is to be centered horizontally
                position.X = ScreenManager.GraphicsDevice.Viewport.Width - (menuEntry.GetWidth(this) + entriesOffsetX);

                if (ScreenState != ScreenState.TransitionOn)
                    position.X -= transitionOffset * 256;
                else
                    position.X += transitionOffset * 512;

                // set the entry's position
                menuEntry.Position = position;

                // move down for the next entry the size of this entry plus our padding
                position.Y += menuEntry.GetHeight(this) + (menuEntryPadding * 2);
            }

            /*var entry = menuEntries.OrderByDescending(item => item.GetHeight(this)).First().GetMenuEntryHitBounds(this);
            //var heightSum = menuEntries.Sum(item => item.GetMenuEntryHitBounds(this).Height);

            var heightSum = menuEntries.First().GetHeight(this) * (menuEntryPadding * 2
            entriesRectangle = new Rectangle((int)menuEntries.First().Position.X, (int)menuEntries.First().Position.Y, entriesRectangle.Width, entriesRectangle.Height);*/
        }

        /// <summary>
        /// Updates the menu.
        /// </summary>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                       bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            // Update each nested MenuEntry object.
            for (int i = 0; i < menuEntries.Count; i++)
            {
                bool isSelected = IsActive && (i == selectedEntry);

                menuEntries[i].Update(this, isSelected, gameTime);
            }

            foreach (var imageEntry in imageEntries)
            {
                imageEntry.Update(this, false, gameTime);
            }

        }


        /// <summary>
        /// Draws the menu.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            // make sure our entries are in the right place before we draw them
            UpdateMenuEntryLocations();

            GraphicsDevice graphics = ScreenManager.GraphicsDevice;
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            SpriteFont font = ScreenManager.Font;

            spriteBatch.Begin();

            // Make the menu slide into place during transitions, using a
            // power curve to make things look more interesting (this makes
            // the movement slow down as it nears the end).
            float transitionOffset = (float)Math.Pow(TransitionPosition, 2);

            // Draw the menu title centered on the screen
            Vector2 titlePosition = new Vector2(200, screenType == 1? 320 : 300);
            Vector2 titleOrigin = font.MeasureString(menuTitle) / 2;
            float titleRotation = MathHelper.ToRadians(screenType == 1?270:0);
            Color titleColor = new Color(192, 192, 192) * TransitionAlpha;
            float titleScale = screenType == 1 ? 2.50f:1.80f;

            //Vector2 entriesRectanglePos = new Vector2(menuEntries.First().Position.X, menuEntries.First().Position.Y);
            Color backgroundColor = (Color.Black * 0.3f) * TransitionAlpha;
            
            titlePosition.Y -= transitionOffset * 100;
            //entriesRectanglePos.X -= transitionOffset * 100;

            /*entriesRectangle.X = (int)entriesRectanglePos.X - menuEntryPadding;
            entriesRectangle.Y = (int)entriesRectanglePos.Y - menuEntryPadding;*/

            spriteBatch.Draw(ScreenManager.Pixel, this.entriesRectangle, backgroundColor);

            spriteBatch.DrawString(font, menuTitle, titlePosition + new Vector2( 20 , 20) , Color.Black * TransitionAlpha, titleRotation,
                                   titleOrigin, titleScale, SpriteEffects.None, 0);

            spriteBatch.DrawString(font, menuTitle, titlePosition + new Vector2(10, 10), titleColor, titleRotation,
                                   titleOrigin, titleScale, SpriteEffects.None, 0);

            spriteBatch.DrawString(font, menuTitle, titlePosition , Color.White * TransitionAlpha, titleRotation,
                                   titleOrigin, titleScale , SpriteEffects.None, 0);

            // Draw each menu entry in turn.
            for (int i = 0; i < menuEntries.Count; i++)
            {
                TouchableEntry menuEntry = menuEntries[i];

                bool isSelected = IsActive && (i == selectedEntry);

                menuEntry.Draw(this, isSelected, gameTime);
            }

            foreach(var imageEntry in imageEntries)
            {
                imageEntry.Draw(this, false, gameTime);
            }

            spriteBatch.End();
        }


        #endregion
    }
}
