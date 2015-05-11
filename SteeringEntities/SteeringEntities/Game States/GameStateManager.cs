using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SteeringEntities
{
    //all game states have constructor
    //update and draw functions
    public abstract class GameState
    {
        protected Camera stateCamera = null;

        //Each game state has a ContentManager
        protected ContentManager m_Content;

        //Each game state has a background colour
        //Allowing for each state to be distinguished from the other
        protected Color m_BackgroundColour;

        //blank square loaded and open for use for
        //all that inherit from GameState
        protected Texture2D blankSquare;

        protected bool initialised = false;

        protected string m_Name = "Default";

        public GameState(string a_Name)
        {
            m_Content = GameStateManager.Instance.Content;
            blankSquare = m_Content.Load<Texture2D>("blankSquare");
            stateCamera = new Camera(25);
            m_Name = a_Name;
        }

        public virtual void Initialise()
        {
            initialised = true;
        }

        public virtual void Update(GameTime gt)
        {

        }

        public virtual void Draw()
        {
            //Clear the background with background colour
            GameStateManager.Instance.GfxDevice.Clear(m_BackgroundColour);
        }

        public Camera StateCamera
        {
            get { return stateCamera; }
        }

        public string Name
        {
            get { return m_Name; }
        }
    }

    public class GameStateManager
    {
        //private instance, to be accessed and created through GetInstance property
        private static GameStateManager instance;
        
        //game is only used to get the Content variable
        private static Game m_Game;

        private List<GameState> m_AllGameStates = new List<GameState>();

        private GameState prevState = null;

        private int currentState = 0;
        private int maxStates = 0;

        //public interface for singleton
        public static GameStateManager Instance
        {
            get { return instance == null ? instance = new GameStateManager() : instance; }
        }

        //property for game manager's content variable
        public ContentManager Content
        {
            get { return m_Game.Content; }
        }

        public GraphicsDevice GfxDevice
        {
            get { return m_Game.GraphicsDevice; }
        }

        public GameState CurrentState
        {
            get { return m_AllGameStates[currentState]; }
        }

        public Game CurrentGame
        {
            get 
            { 
                if (m_Game != null) 
                    return m_Game; 
                return null; 
            }

            set 
            { 
                m_Game = value; 
            }
        }

        public void SetGame(Game a_Game)
        {
            m_Game = a_Game;
        }

        public GameState PreviousState
        {
            get { return prevState; }
        }

        public void AddState(GameState a_GameState)
        {
            //adds to the end
            m_AllGameStates.Add(a_GameState);
            ++maxStates;
        }

        public void ChangeCurrentState(string a_Name)
        {
            int counter = 0;
            for (int i = 0; i < m_AllGameStates.Count; ++i)
            {
                if (m_AllGameStates[i].Name == a_Name)
                {
                    prevState = m_AllGameStates[currentState];
                    currentState = counter;
                    return;
                }
                counter++;
            }
        }

        //stop removing if there is only one state
        //if a decrement to the currentState variable
        //places it under zero, then reset it
        //to max states - 1
        private void RemoveCurrentState()
        {
            if (maxStates == 1)
            {
                return;
            }

            m_AllGameStates.RemoveAt(currentState);

            --currentState;
            --maxStates;

            if (currentState < 0)
            {
                currentState = maxStates - 1;
            }
        }

        //incrementing the current state variable, move to the next state
        public void CycleStates()
        {
            ++currentState;
            
            //greater than max number of states,
            //so reset to beginning
            if (currentState > maxStates - 1)
            {
                currentState = 0;
            }
        }

        public void InitialiseState()
        {
            m_AllGameStates[currentState].Initialise();
        }

        //updating the state based on currentState variable
        public void UpdateState(GameTime gt)
        {
            m_AllGameStates[currentState].Update(gt);
        }

        //drawing the state based on currentState variable
        public void DrawState()
        {
            Game1.sb.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, null, null, null, null, m_AllGameStates[currentState].StateCamera.TranslationMatrix);
            m_AllGameStates[currentState].Draw();
            Game1.sb.End();
        }
    }
}
