using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace CSCI_2941_Lab5
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        PlayerSonya Sonya = new PlayerSonya();
        PlayerSubZero SubZero = new PlayerSubZero();

        Texture2D background;   //contain the background 
        Rectangle mainFrame;        //conatin the mianFrme

        ClockTimer clock = new ClockTimer();
        SpriteFont font;


        Texture2D leftHealthBar, rightHealthBar;             //for the health bar

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
            //graphics.IsFullScreen = true;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            Sonya.Initialize();
            SubZero.Initialize();

            base.Initialize();
            this.IsMouseVisible = true;
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            Sonya.LoadContent(Content);
            SubZero.LoadContent(Content);

            background = Content.Load<Texture2D>("background");     //load content for the background
            mainFrame = new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);     //set the rectangle parameters


            //timer font
            font = Content.Load<SpriteFont>("Courier New");
            //healthbars
            leftHealthBar = new Texture2D(graphics.GraphicsDevice, 400, 30);
            rightHealthBar = new Texture2D(graphics.GraphicsDevice, 400, 30);
            Color[] data = new Color[400 * 30];
            for (int i = 0; i < data.Length; ++i) data[i] = Color.LimeGreen;
            leftHealthBar.SetData(data);
            rightHealthBar.SetData(data);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            Sonya.Dispose();
            SubZero.Dispose();
            spriteBatch.Dispose();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();

            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            Sonya.Update(gameTime);
            SubZero.Update(gameTime);

            // clock start and update  
            if (clock.isRunning == false)
            {
                //count 10 seconds down 
                clock.start(11);
            }
            else
            {
                clock.checkTime(gameTime);
            }

            base.Update(gameTime);


            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            Vector2 leftCoor = new Vector2(graphics.PreferredBackBufferWidth/45, graphics.PreferredBackBufferHeight/30);
            Vector2 rightCoor = new Vector2(2*graphics.PreferredBackBufferWidth/3, graphics.PreferredBackBufferHeight / 30);

            GraphicsDevice.Clear(Color.Purple);

            spriteBatch.Begin();

            spriteBatch.Draw(background, mainFrame, Color.White);

            //timer
            if (!clock.isFinished)
            {
                Vector2 clockSize = font.MeasureString(clock.displayClock);
                spriteBatch.DrawString(font, clock.displayClock, new Vector2(graphics.PreferredBackBufferWidth/2 - (clockSize.X / 2), graphics.PreferredBackBufferHeight / 30), Color.Yellow);
            }
            spriteBatch.Draw(leftHealthBar, leftCoor, Color.White);
            spriteBatch.Draw(rightHealthBar, rightCoor, Color.White);
            Sonya.Draw(spriteBatch);
            SubZero.Draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
