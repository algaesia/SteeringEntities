#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
#endregion

namespace SteeringEntities
{
    public class Game1 : Game
    {
        //static vars for whole game
        public static int g_iScreenWidth = 1920;
        public static int g_iScreenHeight = 1080;
        public static CryptoRandom cryptoRand = new CryptoRandom();

        public static PerlinNoise perlinNoise = new PerlinNoise();

        //blank square texture, can be used everywhere
        public static Texture2D whiteSquare;

        public static SpriteBatch sb;
        public static SpriteFont sf;

        //kb states
        public static KeyboardState currentKeyboardState;
        public static KeyboardState previousKeyboardState;

        GraphicsDeviceManager graphics;

        public Game1()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferWidth = g_iScreenWidth;
            graphics.PreferredBackBufferHeight = g_iScreenHeight;
        }

        protected override void Initialize()
        {
            IsMouseVisible = true;

            GameStateManager.Instance.SetGame(this);

            GameStateManager.Instance.AddState(new RogueLikeState());
            GameStateManager.Instance.AddState(new ShootingGameState());
            GameStateManager.Instance.AddState(new RandomWalkerState());
            GameStateManager.Instance.AddState(new PathfindingState());
            GameStateManager.Instance.AddState(new CameraGameState());
            GameStateManager.Instance.AddState(new FlockingState());
            GameStateManager.Instance.AddState(new PointAndClickState());
            GameStateManager.Instance.AddState(new MatrixState());
            GameStateManager.Instance.AddState(new PauseGameState());

            graphics.IsFullScreen = false;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            sb = new SpriteBatch(GraphicsDevice);
            sf = Content.Load<SpriteFont>("spriteFont1");

            whiteSquare = Content.Load<Texture2D>("blankSquare");
        }

        protected override void UnloadContent() { Content.Unload(); }

        #region Update
        protected override void Update(GameTime gameTime)
        {
            currentKeyboardState = Keyboard.GetState();

            //Check for input, process things, then move to initialise and update everything
            #region Keyboard input
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            if (IsKeyDown(Keys.Enter))
            {
                GameStateManager.Instance.CycleStates();
            }

            if (IsKeyDown(Keys.P))
            {
                if (GameStateManager.Instance.CurrentState.Name == "PauseGameState")
                {
                    GameStateManager.Instance.ChangeCurrentState(GameStateManager.Instance.PreviousState.Name);
                }
                else
                {
                    GameStateManager.Instance.ChangeCurrentState("PauseGameState");
                }
            }
            #endregion

            //Initialise state if needed, created in order to separate camera
            //positioning from being executed in the constructor, only being
            //executed when entering the state
            GameStateManager.Instance.InitialiseState();

            //update current game state
            GameStateManager.Instance.UpdateState(gameTime);

            previousKeyboardState = currentKeyboardState;

            base.Update(gameTime);
        }
        #endregion

        #region Draw
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Coral);

            GameStateManager.Instance.DrawState();

            base.Draw(gameTime);
        }
        #endregion

        #region Helper functions
        public static void DrawLine(Vector2 begin, Vector2 end, Color color, int width = 1)
        {
            //generate a rectangle that encompasses the specified start and end
            //width is dist from start to end + specified width.
            //height is specified width
            Rectangle r = new Rectangle((int)begin.X, (int)begin.Y, (int)(end - begin).Length() + width, width);

            //need angle, construct vector
            Vector2 v = Vector2.Normalize(begin - end);

            //calculate dot prod between specified vector and (-1, 0)
            //gives similarity between two vectors as a float
            //use acos to calc angle between those two vectors
            float angle = (float)Math.Acos(Vector2.Dot(v, -Vector2.UnitX));
            if (begin.Y > end.Y)
            {
                angle = MathHelper.TwoPi - angle;
            }
            sb.Draw(whiteSquare, r, null, color, angle, Vector2.Zero, SpriteEffects.None, 0);
        }

        public static Color RandomColour()
        {
            return new Color((float)cryptoRand.NextDouble(), (float)cryptoRand.NextDouble(), (float)cryptoRand.NextDouble(), 1);
        }

        public static Vector2 RandomDirection()
        {
            return new Vector2((float)Game1.cryptoRand.NextDouble(), (float)Game1.cryptoRand.NextDouble());
        }

        public static Vector2 RandomPosition()
        {
            return new Vector2((float)Game1.cryptoRand.NextDouble() * GameStateManager.Instance.CurrentState.StateCamera.ViewportBoundary.Width, (float)Game1.cryptoRand.NextDouble() * GameStateManager.Instance.CurrentState.StateCamera.ViewportBoundary.Height);
        }

        //not held
        public static bool IsKeyDown(Keys a_Key)
        {
            return currentKeyboardState.IsKeyDown(a_Key) && currentKeyboardState != previousKeyboardState;
        }
        #endregion
    }
}
