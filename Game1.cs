﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace football
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    ///    
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private Texture2D _backgroundTexture;
        private Texture2D _ballTexture;
        private Texture2D _Goalkeeper;
        private int _screenWidth;
        private int _screenHeight;
        private Rectangle _backgroundRectangle;
        private Rectangle _ballRectangle;
        private Vector2 _ballPosition;
        private Vector2 _initialBallPosition;
        private int _goalLinePosition;
        private bool _isBallMoving;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            ResetWindowsSize();
            Window.ClientSizeChanged += (s, e) => ResetWindowsSize();
            base.Initialize();
        }
        private void ResetWindowsSize()
        {
            _screenWidth = Window.ClientBounds.Width;
            _screenHeight = Window.ClientBounds.Height;
            _backgroundRectangle = new Rectangle(0, 0, _screenWidth, _screenHeight);
            _initialBallPosition = new Vector2(_screenWidth * 0.5f, _screenHeight * 0.8f);
            var ballDimension = (_screenWidth > _screenHeight) ?
                (int)(_screenWidth * 0.02) :
                (int)(_screenHeight * 0.035);
            _ballPosition.X = (int)_initialBallPosition.X;
            _ballPosition.Y = (int)_initialBallPosition.Y;
            _ballRectangle = new Rectangle((int)_initialBallPosition.X, (int)_initialBallPosition.Y, ballDimension, ballDimension);
            _goalLinePosition = (int)(_screenHeight * 0.05);
        }
        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            _backgroundTexture = Content.Load<Texture2D>("monogame_img03");
            _ballTexture = Content.Load<Texture2D>("monogame_img04");
            _Goalkeeper = Content.Load<Texture2D>("monogame_img06");
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            // TODO: Add your update logic here
            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                _isBallMoving = true;
            }
            if (_isBallMoving)
            {
                _ballPosition.X -= 0.5f;
                _ballPosition.Y -= 3;
                if (_ballPosition.Y < _goalLinePosition)
                {
                    _ballPosition = new Vector2(_initialBallPosition.X, _initialBallPosition.Y);
                    _isBallMoving = false;
                }
                _ballRectangle.X = (int)_ballPosition.X;
                _ballRectangle.Y = (int)_ballPosition.Y;               
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Green);
            //var screenWidth = Window.ClientBounds.Width;
            //var screenHeight = Window.ClientBounds.Height;
            var rectangle = new Rectangle(0, 0, _screenWidth, _screenHeight);
            spriteBatch.Begin();
            //绘制背景（材质，矩形数据，颜色）
            spriteBatch.Draw(_backgroundTexture, rectangle, Color.White);

            //球的初始坐标
            //var initialBallPositionX = screenWidth / 2;
            //var initialBallPositionY = (int)(screenHeight * 0.8);
            //球的宽度
            //var ballDimension = (screenWidth > screenHeight) ?
            //    (int)(screenWidth * 0.02) :
            //    (int)(screenHeight * 0.035);
            //var ballRectangle = new Rectangle(initialBallPositionX, initialBallPositionY, ballDimension, ballDimension);
            spriteBatch.Draw(_ballTexture, _ballRectangle, Color.White);
            spriteBatch.End();
            base.Draw(gameTime);
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
