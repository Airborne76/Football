using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

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
        private SpriteFont _soccerFont;
        private int _screenWidth;
        private int _screenHeight;
        private Rectangle _backgroundRectangle;
        private Rectangle _ballRectangle;
        private Vector2 _ballPosition;
        private Vector2 _initialBallPosition;
        private int _goalLinePosition;
        private Rectangle _goalkeeperRectangle;
        private int _goalkeeperPostionX;
        private int _goalkeeperPostionY;
        private int _goalkeeperWidth;
        private int _goalkeeperHight;
        private bool _isBallMoving;
        private TimeSpan _startMovement;
        private double _aCoef;
        private double _deltaCoef;
        private int _scorePosition;
        private string _scoreText;
        private int _userScore;
        private int _computerScore;

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
            LoadContent();
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
            _goalkeeperPostionY = (int)(_screenHeight * 0.12);
            _goalkeeperHight = (int)(_screenWidth * 0.01);
            _goalkeeperWidth = (int)(_screenWidth * 0.05);
            _goalkeeperPostionX = (_screenWidth - _goalkeeperWidth) / 2;
            _scoreText = $"{_userScore} : {_computerScore}";
            var scoreSize = _soccerFont.MeasureString(_scoreText);            
            _scorePosition = (int)((_screenWidth - scoreSize.X)/2.0);
        }
        private void ResetGame()
        {
            _ballPosition = new Vector2(_initialBallPosition.X, _initialBallPosition.Y);
            _goalkeeperPostionX = (_screenWidth - _goalkeeperWidth) / 2;
            _isBallMoving = false;
            _scoreText = $"{_userScore} : {_computerScore}";
            var scoreSize = _soccerFont.MeasureString(_scoreText);
            _scorePosition = (int)((_screenWidth - scoreSize.X) / 2.0);

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
            _soccerFont = Content.Load<SpriteFont>("SoccerFont");
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
            if (!_isBallMoving)
            {
                if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                {
                    _isBallMoving = true;
                    _startMovement = gameTime.TotalGameTime;
                    var rnd = new Random();
                    _aCoef = rnd.NextDouble() * 0.01;
                    _deltaCoef = rnd.NextDouble() * Math.PI / 2;
                }
            }

            if (_isBallMoving)
            {
                _ballPosition.X -= 0.5f;
                _ballPosition.Y -= 3;
                _goalkeeperPostionX = (int)((_screenWidth * 0.11)*Math.Sin(_aCoef*gameTime.TotalGameTime.TotalMilliseconds+_deltaCoef)+(_screenWidth*0.75)/2.0+_screenWidth*0.11);
                if (_goalkeeperRectangle.Intersects(_ballRectangle))
                {
                    _computerScore++;
                    ResetGame();
                }
                if (_ballPosition.Y < _goalLinePosition)
                {
                    _ballPosition = new Vector2(_initialBallPosition.X, _initialBallPosition.Y);
                    _isBallMoving = false;
                    _userScore++;
                    ResetGame();
                }
                _ballRectangle.X = (int)_ballPosition.X;
                _ballRectangle.Y = (int)_ballPosition.Y;
            }
            _goalkeeperRectangle = new Rectangle(_goalkeeperPostionX, _goalkeeperPostionY, _goalkeeperWidth, _goalkeeperHight);
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
            spriteBatch.Draw(_Goalkeeper, _goalkeeperRectangle, Color.White);
            spriteBatch.DrawString(_soccerFont, _scoreText, new Vector2(_scorePosition, _screenHeight * 0.9f), Color.White);
            spriteBatch.End();
            base.Draw(gameTime);
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
