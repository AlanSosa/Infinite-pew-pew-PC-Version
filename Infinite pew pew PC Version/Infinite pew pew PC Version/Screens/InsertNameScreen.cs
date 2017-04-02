using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using TechnicalData;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace GameStateManagement
{
    class InsertNameScreen:GameScreen
    {
        #region Fields

        string message;
        Texture2D gradientTexture;

        public string Text="";

        Texture2D pixel;
        Rectangle cursor;
        Vector2 cursorPosition, mainStringPosition, mainStringSize;
        float cursorWidth = 40, cursorHeight = 10;
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
        public InsertNameScreen(string message)
            : this(message, true)
        { }


        /// <summary>
        /// Constructor lets the caller specify whether to include the standard
        /// "A=ok, B=cancel" usage text prompt.
        /// </summary>
        public InsertNameScreen(string message, bool includeUsageText)
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

            pixel = new Texture2D(ScreenManager.GraphicsDevice, 1,1);
            pixel.SetData(new[] { Color.White });
        }


        #endregion

        #region Handle Input

        /// <summary>
        /// Responds to user input, accepting or cancelling the message box.
        /// </summary>
        public override void HandleInput(InputState input)
        {
            PlayerIndex playerIndex;
            KeyboardState currentState = Keyboard.GetState();

            Keys[] keys = new[] {
            Keys.A, Keys.B, Keys.C, Keys.D ,
            Keys.E,Keys.F,Keys.G,Keys.H,
            Keys.I, Keys.J, Keys.K, Keys.L,
            Keys.M, Keys.N,Keys.O, Keys.P,
            Keys.Q, Keys.R, Keys.S, Keys.T,
            Keys.U, Keys.V, Keys.W, Keys.X,
            Keys.Y, Keys.Z, Keys.Back}.Where(x => currentState.IsKeyDown(x)).ToArray();

            foreach (var key in keys)
            {
                if (key != Keys.Back)
                {

                    if (input.IsNewKeyPress(key, ControllingPlayer, out playerIndex))
                    {

                        if(Text.Count()< 12)
                            Text += key.ToString();

                        message = "New_Highscore!\n";
                        if (GameStatics.sounds)
                            ScreenManager.KeyboardSound.Play();
                    }
                }
                else
                {
                    if (input.IsNewKeyPress(key, ControllingPlayer, out playerIndex))
                    {
                        if (Text.Count() > 0)
                        {
                            Text = Text.Remove(Text.Count() - 1);
                            message = "New_Highscore!\n";

                            if (GameStatics.sounds)
                                ScreenManager.KeyboardSound.Play();
                        }
                    }
                }
            }

            // We pass in our ControllingPlayer, which may either be null (to
            // accept input from any player) or a specific index. If we pass a null
            // controlling player, the InputState helper returns to us which player
            // actually provided the input. We pass that through to our Accepted and
            // Cancelled events, so they can tell which player triggered them.
            if (input.IsMenuSelect(ControllingPlayer, out playerIndex))
            {
                if (GameStatics.sounds)
                    ScreenManager.playPopOff();

                // Raise the accepted event, then exit the message box.
                if (!AcceptedEntry.IsEventNull())
                    AcceptedEntry.OnSelectEntry(playerIndex);

                ExitScreen();
            }

        }

        //15
        #endregion

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            mainStringSize = ScreenManager.Font.MeasureString(message);

            cursorPosition.Y = mainStringPosition.Y + mainStringSize.Y ;
            
            cursorPosition.X = mainStringPosition.X + Text.Count() * 30;
                /*message = message.Remove(14,14+( message.Count()-1 + Text.Count()-1));
                message += Text;*/

        }

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
            Vector2 textSize = font.MeasureString(message + Text);
            textSize.X += AcceptedEntry.GetWidth(this) / 2;
            textSize.Y += AcceptedEntry.GetHeight(this) + yOffset/2;
            Vector2 textPosition = mainStringPosition = (viewportSize - textSize) / 2;

            

            // The background includes a border somewhat larger than the text itself.
            const int hPad = 32;
            const int vPad = 16;

            Rectangle backgroundRectangle = new Rectangle((int)textPosition.X - hPad*2,
                                                          (int)textPosition.Y - vPad,
                                                          (int)textSize.X + hPad * 4,
                                                          (int)textSize.Y + vPad * 2);

            // Fade the popup alpha during transitions.
            Color color = Color.White * TransitionAlpha;

           

            AcceptedEntry.Position = new Vector2(viewportSize.X / 2 - AcceptedEntry.GetWidth(this) / 2 , textPosition.Y + textSize.Y - yOffset);


            spriteBatch.Begin();

            // Draw the background rectangle.
            spriteBatch.Draw(gradientTexture, backgroundRectangle, color);

            // Draw the message box text.
            spriteBatch.DrawString(font, message + Text, textPosition, color);

            // Pulsate the size of the selected menu entry.
            double time = gameTime.TotalGameTime.TotalSeconds;

            float pulsate = (float)Math.Sin(time * 8) + 1;

            spriteBatch.Draw(pixel, new Rectangle((int)cursorPosition.X, (int)cursorPosition.Y, (int)cursorWidth, (int)cursorHeight), Color.Yellow * pulsate);

            if(AcceptedEntry != null)
                AcceptedEntry.Draw(this, false, gameTime);

            spriteBatch.End();
        }


        #endregion
    }
}
