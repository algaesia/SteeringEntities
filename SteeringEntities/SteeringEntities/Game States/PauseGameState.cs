using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SteeringEntities
{
    public class PauseGameState : GameState
    {
        Rectangle mouseRectangle;

        Rectangle firstRect = new Rectangle(-1000, 100, 50, 50);
        Rectangle secondRect = new Rectangle(1000, -1000, 50, 50);

        Color firstColour = Color.Blue;
        Color secondColour = Color.White;

        public PauseGameState()
            : base("PauseGameState")
        {
            m_BackgroundColour = Color.Red;
        }

        public override void Initialise()
        {
            if (!initialised)
            {
                mouseRectangle = new Rectangle((int)Mouse.GetState().X, (int)Mouse.GetState().Y, 32, 32);
            }

            base.Initialise();
        }

        public override void Update(GameTime gt)
        {
            Vector2 mouseWorld = stateCamera.ScreenToWorld(new Vector2(Mouse.GetState().X, Mouse.GetState().Y));

            mouseRectangle.X = (int)mouseWorld.X;
            mouseRectangle.Y = (int)mouseWorld.Y;

            if (firstRect.Intersects(mouseRectangle))
            {
                firstColour = Color.Yellow;
            }
            else
            {
                firstColour = Color.Blue;
            }

            if (secondRect.Intersects(mouseRectangle))
            {
                secondColour = Color.Violet;
            }
            else
            {
                secondColour = Color.White;
            }

            stateCamera.HandleInput();

            base.Update(gt);
        }

        public override void Draw()
        {
            Vector2 camPos = stateCamera.ScreenToWorld(new Vector2(-(int)stateCamera.Position.X, -(int)stateCamera.Position.Y));
            Game1.sb.Draw(blankSquare, mouseRectangle, Color.White);
            Game1.sb.Draw(blankSquare, firstRect, firstColour);
            Game1.sb.Draw(blankSquare, secondRect, secondColour);
            Game1.sb.Draw(blankSquare, new Rectangle((int)camPos.X, (int)camPos.Y, Game1.g_iScreenWidth, Game1.g_iScreenHeight), Color.Black);

            base.Draw();
        }
    }
}
