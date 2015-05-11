using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SteeringEntities
{
    public class InputHandler
    {
        int[] playerKeys = new int[] { (int)Keys.W, (int)Keys.A, (int)Keys.D, (int)Keys.S, (int)MouseButton.Left };

        private Dictionary<int, Command> possiblePlayerInputs = new Dictionary<int, Command>();

        private static InputHandler instance;

        public static InputHandler Instance
        {
            get { return instance == null ? instance = new InputHandler() : instance; }
        }

        private InputHandler()
        {
            possiblePlayerInputs.Add(playerKeys[0], new MoveUpCommand());
            possiblePlayerInputs.Add(playerKeys[1], new MoveLeftCommand());
            possiblePlayerInputs.Add(playerKeys[2], new MoveRightCommand());
            possiblePlayerInputs.Add(playerKeys[3], new MoveDownCommand());
            possiblePlayerInputs.Add(playerKeys[4], new ShootCommand());
        }

        public int PlayerMoveLeftKey
        {
            get { return playerKeys[1]; }
        }

        public int PlayerMoveRightKey
        {
            get { return playerKeys[2]; }
        }

        public int PlayerMoveUpKey
        {
            get { return playerKeys[0]; }
        }

        public int PlayerMoveDownKey
        {
            get { return playerKeys[3]; }
        }

        public int PlayerShootKey
        {
            get { return playerKeys[4]; }
        }

        public List<Command> HandlePlayerInput()
        {
            List<Command> actions = new List<Command>();
            if (Game1.currentKeyboardState.IsKeyDown((Keys)PlayerMoveUpKey))
            {
                actions.Add(possiblePlayerInputs[PlayerMoveUpKey]);
            }

            if (Game1.currentKeyboardState.IsKeyDown((Keys)PlayerMoveDownKey))
            {
                actions.Add(possiblePlayerInputs[PlayerMoveDownKey]);
            }

            if (Game1.currentKeyboardState.IsKeyDown((Keys)PlayerMoveLeftKey))
            {
                actions.Add(possiblePlayerInputs[PlayerMoveLeftKey]);
            }

            if (Game1.currentKeyboardState.IsKeyDown((Keys)PlayerMoveRightKey))
            {
                actions.Add(possiblePlayerInputs[PlayerMoveRightKey]);
            }

            if (CheckMouseButtonDown((MouseButton)PlayerShootKey))
            {
                actions.Add(possiblePlayerInputs[PlayerShootKey]);
            }

            return actions;
        }

        private bool CheckMouseButtonDown(MouseButton a_MouseButton)
        {
            switch (a_MouseButton)
            {
                case MouseButton.Left:
                    return Mouse.GetState().LeftButton == ButtonState.Pressed;
                case MouseButton.Right:
                    return Mouse.GetState().RightButton == ButtonState.Pressed;
                case MouseButton.Middle:
                    return Mouse.GetState().MiddleButton == ButtonState.Pressed;
            }
            return false;
        }
    }
}
