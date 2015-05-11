using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SteeringEntities
{
    public class CameraGameState : GameState
    {
        Player newPlayer;

        Rectangle pos1, pos2, pos3, pos4;
        Color col1, col2, col3, col4;

        public CameraGameState()
            : base("CameraGameState")
        {
            m_BackgroundColour = Color.Red;

            newPlayer = new Player(blankSquare, new Vector2(250, 250), new Vector2(32,32));
            stateCamera.CentreOnPos(new Vector2(250, 250));

            pos1 = new Rectangle((int)(stateCamera.Position.X + Game1.cryptoRand.NextDouble() * stateCamera.ViewportBoundary.X), (int)(stateCamera.Position.Y + Game1.cryptoRand.NextDouble() * stateCamera.ViewportBoundary.Y), 50, 50);
            pos2 = new Rectangle((int)(stateCamera.Position.X + Game1.cryptoRand.NextDouble() * stateCamera.ViewportBoundary.X), (int)(stateCamera.Position.Y + Game1.cryptoRand.NextDouble() * stateCamera.ViewportBoundary.Y), 50, 50);
            pos3 = new Rectangle((int)(stateCamera.Position.X + Game1.cryptoRand.NextDouble() * stateCamera.ViewportBoundary.X), (int)(stateCamera.Position.Y + Game1.cryptoRand.NextDouble() * stateCamera.ViewportBoundary.Y), 50, 50);
            pos4 = new Rectangle((int)(stateCamera.Position.X + Game1.cryptoRand.NextDouble() * stateCamera.ViewportBoundary.X), (int)(stateCamera.Position.Y + Game1.cryptoRand.NextDouble() * stateCamera.ViewportBoundary.Y), 50, 50);

            col1 = Game1.RandomColour();
            col2 = Game1.RandomColour();
            col3 = Game1.RandomColour();
            col4 = Game1.RandomColour();
        }

        public override void Update(GameTime gt)
        {
            newPlayer.Update(gt);
            stateCamera.CentreOnPos(newPlayer.Position);
            base.Update(gt);
        }

        public override void Draw()
        {
            base.Draw();
            newPlayer.Draw(Game1.sb, Game1.sf);

            Game1.sb.Draw(blankSquare, pos1, col1);
            Game1.sb.Draw(blankSquare, pos2, col2);
            Game1.sb.Draw(blankSquare, pos3, col3);
            Game1.sb.Draw(blankSquare, pos4, col4);
        }
    }
}
