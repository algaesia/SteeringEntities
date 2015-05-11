using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SteeringEntities
{
    public class PathfindingState : GameState
    {
        Graph levelGrid;
        List<Enemy> enemies = new List<Enemy>();

        public PathfindingState()
            : base("PathfindingState")
        {
            
        }

        public override void Initialise()
        {
            if (!initialised)
            {
                m_BackgroundColour = Color.Green;
                levelGrid = new Graph(31, 31);

                enemies.Add(new PointAndClickPathFollower(blankSquare,
                                                          new Vector2((float)(Game1.cryptoRand.NextDouble() * Game1.g_iScreenWidth),
                                                                      (float)(Game1.cryptoRand.NextDouble() * Game1.g_iScreenHeight)),
                                                          levelGrid));
                for (int i = 0; i < 25; ++i)
                {
                    enemies.Add(new PathFollower(blankSquare,
                                new Vector2((float)(Game1.cryptoRand.NextDouble() * Game1.g_iScreenWidth),
                                            (float)(Game1.cryptoRand.NextDouble() * Game1.g_iScreenHeight)), levelGrid));
                }

                stateCamera.CentreOnPos(stateCamera.ViewportCentre);
            }

            base.Initialise();
        }

        public override void Update(GameTime gt)
        {
            foreach (Enemy n in enemies)
            {
                n.Update(gt);
            }

            stateCamera.HandleInput();

            base.Update(gt);
        }

        public override void Draw()
        {
            //graphics clear first
            base.Draw();

            //draw grid
            levelGrid.Draw(Game1.sb, Game1.sf);

            //draw enemies
            foreach (Enemy n in enemies)
            {
                n.Draw(Game1.sb, Game1.sf);
            }
        }
    }
}
