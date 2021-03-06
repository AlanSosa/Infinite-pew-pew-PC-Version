#region File Description
//-----------------------------------------------------------------------------
// MessageBoxScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using TechnicalData;
#endregion

namespace GameStateManagement
{
    /// <summary>
    /// A popup message box screen, used to display "are you sure?"
    /// confirmation messages.
    /// </summary>
    class MessageBoxScreen : GameScreen
    {
        #region Fields

        string message;
        Texture2D gradientTexture;

        #endregion

        #region Events
        public TouchableEntry AcceptedEntry;
        public TouchableEntry CancelledEntry;
        #endregion

        #region Initialization


        /// <summary>
        /// Constructor automatically includes the standard "A=ok, B=cancel"
        /// usage text prompt.
        /// </summary>
        public MessageBoxScreen(string message)
            : this(message, true)
        { }


        /// <summary>
        /// Constructor lets the caller specify whether to include the standard
        /// "A=ok, B=cancel" usage text prompt.
        /// </summary>
        public MessageBoxScreen(string message, bool includeUsageText)
        {

            const string usageText = "";

            /*const string usageText = "\nA button, Space, Enter = ok" +
                                     "\nB button, Esc = cancel"; */

            if (includeUsageText)
                this.message = message + usageText;
            else
                this.message = message;

            IsPopup = true;

            TransitionOnTime = TimeSpan.FromSeconds(0.2);
            TransitionOffTime = TimeSpan.FromSeconds(0.2);
            
        }

        public void ReWriteMessage(string message)
        {
            this.message = message;
        }

        /// <summary>
        /// Loads graphics content for this screen. This uses the shared ContentManager
        /// provided by the Game class, so the content will remain loaded forever.
        /// Whenever a subsequent MessageBoxScreen tries to load this same content,
        /// it will just get back another reference to the already loaded data.
        /// </summary>
        public override void LoadContent()
        {
            ContentManager content = ScreenManager.Game.Content;

            gradientTexture = content.Load<Texture2D>("gradient");
            if (GameStatics.sounds)
                ScreenManager.playPopUp();
        }


        #endregion

        #region Handle Input


        /// <summary>
        /// Responds to user input, accepting or cancelling the message box.
        /// </summary>
        public override void HandleInput(InputState input)
        {
            PlayerIndex playerIndex;

            // We pass in our ControllingPlayer, which may either be null (to
            // accept input from any player) or a specific index. If we pass a null
            // controlling player, the InputState helper returns to us which player
            // actually provided the input. We pass that through to our Accepted and
            // Cancelled events, so they can tell which player triggered them.
            if (input.IsMenuSelect(ControllingPlayer, out playerIndex))
            {
                // Raise the accepted event, then exit the message box.
                if (!AcceptedEntry.IsEventNull())
                    AcceptedEntry.OnSelectEntry(playerIndex);

                if (GameStatics.sounds)
                    ScreenManager.playPopOff();
                ExitScreen();
            }
            else if (input.IsMenuCancel(ControllingPlayer, out playerIndex))
            {
                // Raise the cancelled event, then exit the message box.
                if (!CancelledEntry.IsEventNull())
                    CancelledEntry.OnSelectEntry(playerIndex);

                if (GameStatics.sounds)
                    ScreenManager.playPopOff();
                ExitScreen();
                
            }


            /*
            // look for any taps that occurred and select any entries that were tapped
            foreach (GestureSample gesture in input.Gestures)
            {
                if (gesture.GestureType == GestureType.Tap)
                {
                    // convert the position to a Point that we can test against a Rectangle
                    Point tapLocation = new Point((int)gesture.Position.X, (int)gesture.Position.Y);

                    if (AcceptedEntry != null)
                    {
                        if (AcceptedEntry.GetMenuEntryHitBounds(this).Contains(tapLocation))
                        {
                            if (!AcceptedEntry.IsEventNull())
                            {
                                AcceptedEntry.OnSelectEntry(playerIndex);
                            }
                            else
                            {
                                if (GameStatics.sounds)
                                    ScreenManager.playPopOff();
                                ExitScreen();
                            }
                        }
                    }

                    if (CancelledEntry != null)
                    {
                        if (CancelledEntry.GetMenuEntryHitBounds(this).Contains(tapLocation))
                        {
                            if (!CancelledEntry.IsEventNull())
                            {
                                CancelledEntry.OnSelectEntry(playerIndex);
                            }
                            else
                            {
                                if (GameStatics.sounds)
                                    ScreenManager.playPopOff();
                                ExitScreen();
                            }
                        }
                    }
                    

                }
            }*/
        }


        #endregion

        #region Draw
        /// <summary>
        /// Draws the message box.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            SpriteFont font = ScreenManager.Font;

            // Darken down any other screens that were drawn beneath the popup.
            ScreenManager.FadeBackBufferToBlack(TransitionAlpha * 2 / 3);

            float yOffset = 40;
            float xOffset = 20;

            // Center the message text in the viewport.
            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            Vector2 viewportSize = new Vector2(viewport.Width, viewport.Height);
            Vector2 textSize = font.MeasureString(message);
            textSize.X += AcceptedEntry.GetWidth(this) / 2 + CancelledEntry.GetWidth(this) / 2;
            textSize.Y += AcceptedEntry.GetHeight(this) + yOffset/2;
            Vector2 textPosition = (viewportSize - textSize) / 2;

            

            // The background includes a border somewhat larger than the text itself.
            const int hPad = 32;
            const int vPad = 16;

            Rectangle backgroundRectangle = new Rectangle((int)textPosition.X - hPad*2,
                                                          (int)textPosition.Y - vPad,
                                                          (int)textSize.X + hPad * 4,
                                                          (int)textSize.Y + vPad * 2);

            // Fade the popup alpha during transitions.
            Color color = Color.White * TransitionAlpha;

            AcceptedEntry.Position = new Vector2(viewportSize.X / 2 - AcceptedEntry.GetWidth(this) - xOffset * 1.5f, textPosition.Y + textSize.Y  - yOffset);

            CancelledEntry.Position = new Vector2(viewportSize.X / 2 + xOffset * 1.5f, textPosition.Y + textSize.Y - yOffset);


            spriteBatch.Begin();

            // Draw the background rectangle.
            spriteBatch.Draw(gradientTexture, backgroundRectangle, color);

            // Draw the message box text.
            spriteBatch.DrawString(font, message, textPosition, color);

            if(AcceptedEntry != null)
                AcceptedEntry.Draw(this, false, gameTime);

            if (CancelledEntry != null)
                CancelledEntry.Draw(this, false, gameTime);

            spriteBatch.End();
        }


        #endregion
    }
}
