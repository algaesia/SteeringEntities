using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SteeringEntities
{
    //Moves away from target
    public class Avoider : Enemy
    {
        private SteeringBehaviour currentBehaviour;
        private Vector2 m_StartingPos;
        private float m_FarAwayEnough = 500;
        public MovableEntity currentTarget;

        public Avoider(Texture2D a_Texture, Vector2 a_Position, MovableEntity a_CurrentTarget)
            : base(a_Texture, a_Position)
        {
            currentTarget = a_CurrentTarget;
            m_StartingPos = a_Position;
            m_Colour = Color.Gray;
        }

        public override void Update(GameTime gt)
        {
            if (currentTarget != null)
            {
                currentBehaviour = new Evade(this, currentTarget);
            }

            //Target is far away enough, so sit still
            if (Vector2.Distance(m_Position, currentTarget.Position) > m_FarAwayEnough)
            {
                m_Velocity = Vector2.Zero;
            }
            //otherwise move away
            else
            {
                m_Velocity += currentBehaviour.GetForce();
            }            

            if (m_Velocity.Length() > 0)
            {
                m_Velocity.Normalize();
            }

            base.Update(gt);
        }

        public override void Draw(SpriteBatch sb, SpriteFont sf)
        {
            base.Draw(sb, sf);
        }
    }
}
