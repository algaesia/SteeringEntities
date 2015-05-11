using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SteeringEntities
{
    public class ShootingGameState : GameState
    {
        LevelManager testManager;

        public ShootingGameState()
            : base("ShootingGameState")
        {
            m_BackgroundColour = Game1.RandomColour();
        }

        public override void Initialise()
        {
            if (!initialised)
            {
                testManager = new LevelManager(m_Content.Load<Texture2D>("costs_3"));
            }

            base.Initialise();
        }

        public override void Update(GameTime gt)
        {
            stateCamera.CentreOnPos(testManager.PlayerRef.Position);

            testManager.Update(gt);

            ProjectileManager.Instance.Update(gt);

            base.Update(gt);
        }

        public override void Draw()
        {
            base.Draw();

            ProjectileManager.Instance.Draw(Game1.sb, Game1.sf);

            testManager.Draw();
        }
    }
}
