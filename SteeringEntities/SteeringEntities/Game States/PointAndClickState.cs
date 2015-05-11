using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SteeringEntities
{
    public class PointAndClickState : GameState
    {
        Graph levelGrid;
        PointAndClickPathFollower clickPathFollower;

        public PointAndClickState()
            : base("PointAndClickState")
        {
            
        }

        public override void Initialise()
        {
            if (!initialised)
            {
                m_BackgroundColour = Game1.RandomColour();

                //create grid on which the entity will move
                levelGrid = new Graph(31, 31, "costs_2");

                //counter for random num iterations
                //and a max number of iterations
                int counter = 0;
                int maxIter = 10;

                //generate random node on screen
                Node randomNode = levelGrid.GetNearestNode(new Vector2((float)Game1.cryptoRand.NextDouble() * Game1.g_iScreenWidth, (float)Game1.cryptoRand.NextDouble() * Game1.g_iScreenHeight));

                //loop until there's a useable type for the entity to start on
                //or the max iterations are hit
                while (randomNode == null || randomNode.GetNodeType == Node.NODE_TYPE.WALL || randomNode.GetNodeType == Node.NODE_TYPE.NOT_WALKABLE)
                {
                    if (counter > maxIter)
                    {
                        break;
                    }

                    if (randomNode == null || randomNode.GetNodeType == Node.NODE_TYPE.NOT_WALKABLE || randomNode.GetNodeType == Node.NODE_TYPE.WALL)
                    {
                        randomNode = levelGrid.GetNearestNode(new Vector2((float)Game1.cryptoRand.NextDouble() * Game1.g_iScreenWidth, (float)Game1.cryptoRand.NextDouble() * Game1.g_iScreenHeight));
                    }

                    ++counter;
                }

                clickPathFollower = new PointAndClickPathFollower(blankSquare, randomNode.Position, levelGrid);

                stateCamera.CentreOnPos(new Vector2(Game1.g_iScreenWidth * 0.5f, Game1.g_iScreenHeight * 0.5f));
            }

            base.Initialise();
        }

        public override void Update(GameTime gt)
        {
            clickPathFollower.Update(gt);

            base.Update(gt);
        }

        public override void Draw()
        {
            base.Draw();

            levelGrid.Draw(Game1.sb, Game1.sf);
            clickPathFollower.Draw(Game1.sb, Game1.sf);
        }
    }
}
