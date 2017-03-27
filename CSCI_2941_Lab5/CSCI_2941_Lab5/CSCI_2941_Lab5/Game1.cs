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
    enum Sprite
    {
        Idle, Run, Crouch, Mid_Punch, Kick, Block, Max,
    }
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {        
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        PlayerSonya Sonya = new PlayerSonya();
        PlayerSubZero SubZero = new PlayerSubZero();

        Texture2D background, instructions, title;   //contain the background 
        Rectangle mainFrame;        //conatin the mianFrme

        ClockTimer clock = new ClockTimer();
        SpriteFont font;

        healthBar SonyaGreenBar = new healthBar();
        healthBar SonyaRedBar = new healthBar();
        int SonyaHealth = 550;
        healthBar SubZGreenBar = new healthBar();
        healthBar SubZRedBar = new healthBar();
        int SubZHealth = 550;
        int Health;
        bool DrawHealthBar = true;

        Collision collision = new Collision();

        //menu
        int game = 1;
        private MouseState mouseState, previousMouseState;
        private int mouseX, mouseY;
        int timer = 0;

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
            Sonya.Initialize(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
            SubZero.Initialize(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
            SonyaGreenBar.health(new Vector2(graphics.PreferredBackBufferWidth / 45,
                graphics.PreferredBackBufferHeight / 30), SonyaHealth, Color.LimeGreen);
            SonyaRedBar.health(new Vector2(graphics.PreferredBackBufferWidth / 45, 
                graphics.PreferredBackBufferHeight / 30), SonyaHealth, Color.Red);

            SubZGreenBar.health(new Vector2(graphics.PreferredBackBufferWidth - 580,
                graphics.PreferredBackBufferHeight / 30), SonyaHealth, Color.LimeGreen);
            SubZRedBar.health(new Vector2(graphics.PreferredBackBufferWidth - 580,
                graphics.PreferredBackBufferHeight / 30), SonyaHealth, Color.Red);

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

            background = Content.Load<Texture2D>("background");     //load content for the background   http://wallpapersafari.com/w/r4L80s/
            instructions = Content.Load<Texture2D>("Instructions");     //load content for the background  http://wallpapersafari.com/w/Iy0UTt/
            title = Content.Load<Texture2D>("title");     //https://cdn.gamerant.com/wp-content/uploads/mortal-kombat-web-series-header.jpg and https://i.ytimg.com/vi/DmxaSOD5XxY/maxresdefault.jpg
            mainFrame = new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);     //set the rectangle parameters


            //timer font
            font = Content.Load<SpriteFont>("Courier New");
            //healthbars
            //leftHealthBar = new Texture2D(graphics.GraphicsDevice, 400, 30);
            //rightHealthBar = new Texture2D(graphics.GraphicsDevice, 400, 30);
            //Color[] data = new Color[400 * 30];
            //for (int i = 0; i < data.Length; ++i) data[i] = Color.LimeGreen;
            //leftHealthBar.SetData(data);
            //rightHealthBar.SetData(data);
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
            KeyboardState keyboardState = Keyboard.GetState();      //setting the getstate to keyboardstate
            previousMouseState = Mouse.GetState();       //setting the getstate to the previousMousestate
            mouseState = Mouse.GetState();                 //setting the getstate to the mousestate
            mouseX = mouseState.X;                      //setting the mouse X position
            mouseY = mouseState.Y;                      //setting the mouse Y position
            if (mouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Pressed)
            {
                if (game == 1)
                {  //play
                    if (new Rectangle((graphics.PreferredBackBufferWidth / 2)-200, (2*graphics.PreferredBackBufferHeight / 5), 150, 30).Contains(mouseX, mouseY))
                    {
                        timer = 99;
                        game = 3;
                    }
                    //instructions
                    if (new Rectangle((graphics.PreferredBackBufferWidth / 2)-200, (graphics.PreferredBackBufferHeight / 2), 150, 30).Contains(mouseX, mouseY))
                    {
                        game = 2;
                    }
                    //quit
                    if (new Rectangle((graphics.PreferredBackBufferWidth / 2)-200, (2 * graphics.PreferredBackBufferHeight / 3), 150, 30).Contains(mouseX, mouseY))
                        this.Exit();
                }
                if (game == 2)
                {  //return
                    if (new Rectangle((2*graphics.PreferredBackBufferWidth / 3), (graphics.PreferredBackBufferHeight / 30), 150, 30).Contains(mouseX, mouseY))
                    {
                        game = 1;
                    }
                }
                if (game == 4)
                {  //play
                    if (new Rectangle((graphics.PreferredBackBufferWidth / 2), (graphics.PreferredBackBufferHeight / 2), 150, 30).Contains(mouseX, mouseY))
                    {
                        game = 3;
                    }
                    //quit
                    if (new Rectangle((graphics.PreferredBackBufferWidth / 2), (2 * graphics.PreferredBackBufferHeight / 3), 150, 30).Contains(mouseX, mouseY))
                        this.Exit();
                }


            }
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();

            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            if (game == 3)
            {
                Sonya.Update(gameTime);
                SubZero.Update(gameTime);

                // clock start and update  
                if (clock.isRunning == false)
                {
                    //count 10 seconds down 
                    clock.start(timer);
                }
                else
                {
                    clock.checkTime(gameTime);
                }

            }
            if (game == 3 || game == 4)
            {
                if (keyboardState.IsKeyDown(Keys.P))
                {
                    game = 4;
                }
                if (keyboardState.IsKeyDown(Keys.R))
                {
                    game = 3;
                }
            }

            // Test Sonya's collision //
            Health = collision.TestCollision(Sonya.sonyaHitBox.playerHB, SubZero.subZeroHitBox.playerHB,
                Sonya.currentState, SubZero.currentState);

            if (Health != 0 && SonyaHealth != 0)
            {
                    SonyaHealth -= Health;
                    SonyaGreenBar.update(SonyaHealth);
            }
            Health = collision.TestCollision(SubZero.subZeroHitBox.playerHB, Sonya.sonyaHitBox.playerHB,
                SubZero.currentState, Sonya.currentState);

            if (Health != 0 && SubZHealth != 0)
            {
                SubZHealth -= Health;
                SubZGreenBar.update(SubZHealth);
            }

            if (SonyaHealth <= 0 || SubZHealth <= 0)
                DrawHealthBar = false;

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            //Vector2 leftCoor = new Vector2(graphics.PreferredBackBufferWidth/45, graphics.PreferredBackBufferHeight/30);
            //Vector2 rightCoor = new Vector2(2*graphics.PreferredBackBufferWidth/3, graphics.PreferredBackBufferHeight / 30);
            Vector2 clockSize = font.MeasureString(clock.displayClock);
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();

            //title
            if (game == 1)
            {
                spriteBatch.Draw(title, mainFrame, Color.White);
                spriteBatch.DrawString(font, "Play", new Vector2((graphics.PreferredBackBufferWidth / 2)-200, (2*graphics.PreferredBackBufferHeight / 5)), Color.Red);
                spriteBatch.DrawString(font, "Instructions", new Vector2((graphics.PreferredBackBufferWidth / 2)-200, (graphics.PreferredBackBufferHeight / 2)), Color.Red);
                spriteBatch.DrawString(font, "Quit", new Vector2((graphics.PreferredBackBufferWidth / 2)-200, (2*graphics.PreferredBackBufferHeight / 3)), Color.Red);

            }
            //instructions
            if (game == 2)
            {
                spriteBatch.Draw(instructions, mainFrame, Color.White);

                spriteBatch.DrawString(font, "Return", new Vector2((2 * graphics.PreferredBackBufferWidth / 3), (graphics.PreferredBackBufferHeight / 30)), Color.Red);
            }
            //gamePlay
            if (game == 3)
            {
                spriteBatch.Draw(background, mainFrame, Color.White);


            //timer
            if (!clock.isFinished)
            {
                spriteBatch.DrawString(font, clock.displayClock, new Vector2(graphics.PreferredBackBufferWidth / 2 - (clockSize.X / 2), graphics.PreferredBackBufferHeight / 30), Color.Yellow);
            }
            if (clock.isFinished)
            {
                spriteBatch.DrawString(font, clock.displayClock, new Vector2(graphics.PreferredBackBufferWidth / 2 - (clockSize.X / 2), graphics.PreferredBackBufferHeight / 30), Color.Yellow);
            }
            //spriteBatch.Draw(leftHealthBar, leftCoor, Color.White);
            //spriteBatch.Draw(rightHealthBar, rightCoor, Color.White);
            Sonya.Draw(spriteBatch);
            SubZero.Draw(spriteBatch);

            if (DrawHealthBar)
            {
                SonyaRedBar.Draw(spriteBatch);
                SonyaGreenBar.Draw(spriteBatch);

                SubZRedBar.Draw(spriteBatch);
                SubZGreenBar.Draw(spriteBatch);
            }
            
            }
            //pause
            if (game == 4)
            {
                spriteBatch.DrawString(font, "Paused", new Vector2((graphics.PreferredBackBufferWidth / 2), 50), Color.Red);
                spriteBatch.DrawString(font, "Resume", new Vector2((graphics.PreferredBackBufferWidth / 2), (graphics.PreferredBackBufferHeight / 2)), Color.Red);
                spriteBatch.DrawString(font, "Quit", new Vector2((graphics.PreferredBackBufferWidth / 2), (2 * graphics.PreferredBackBufferHeight / 3)), Color.Red);
            }
            //matchover

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
