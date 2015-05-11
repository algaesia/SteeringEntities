using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SteeringEntities
{
    public class RandomWalkerState : GameState
    {
        List<RandomWalker> walkers = new List<RandomWalker>();

        public RandomWalkerState()
            : base("RandomWalkerState")
        {
            
        }

        public override void Initialise()
        {
            if (initialised) return;

            for (int i = 0; i < 250; ++i)
            {
                walkers.Add(new RandomWalker(new Vector2(Game1.g_iScreenWidth * 0.5f, Game1.g_iScreenHeight * 0.5f), blankSquare));
            }

            stateCamera.CentreOnPos(new Vector2(Game1.g_iScreenWidth * 0.5f, Game1.g_iScreenHeight * 0.5f));

            base.Initialise();
        }

        public override void Update(GameTime gt)
        {
            foreach (RandomWalker walker in walkers)
            {
                walker.Update(gt);
            }

            if (Game1.IsKeyDown(Keys.R))
            {
                foreach (RandomWalker walker in walkers)
                {
                    walker.ResetPosition(false);
                }
            }

            stateCamera.HandleInput();

            base.Update(gt);
        }

        public override void Draw()
        {
            base.Draw();
            foreach (RandomWalker walker in walkers)
            {
                walker.Draw(Game1.sb, Game1.sf);
            }
        }
    }
}
