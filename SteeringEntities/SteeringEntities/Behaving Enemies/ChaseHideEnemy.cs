using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SteeringEntities
{
    //doesn't really work yet
    public class ChaseHideEnemy : Enemy
    {
        private SteeringBehaviour currentBehaviour;
        private MovableEntity currentTarget;
        private float closeEnough = 50;

        public ChaseHideEnemy(Texture2D a_Texture, Vector2 a_Position, MovableEntity a_Target)
            : base(a_Texture, a_Position)
        {
            m_Colour = Color.Azure;
            currentBehaviour = null;
            currentTarget = a_Target;
        }

        public override void Update(GameTime gt)
        {
            if (currentBehaviour == null)
            {
                if (Vector2.Distance(Position, currentTarget.Position) < closeEnough)
                {
                    currentBehaviour = new Evade(this, currentTarget);
                }
                else
                {
                    currentBehaviour = new Pursue(this, currentTarget);
                }
            }

            if (Vector2.Distance(Position, currentTarget.Position) < closeEnough)
            {
                currentBehaviour = null;
            }

            if (m_Velocity.Length() > 0)
            {
                m_Velocity.Normalize();
            }

            if (currentBehaviour != null)
            {
                m_Velocity += currentBehaviour.GetForce();
            }

            base.Update(gt);
        }

        public override void Draw(SpriteBatch sb, SpriteFont sf)
        {
            base.Draw(sb, sf);
        }
    }
}
