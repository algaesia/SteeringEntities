using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SteeringEntities
{
    //this entity uses the wander behaviour to move around the map
    public class Explorer : Enemy
    {
        private SteeringBehaviour currentBehaviour;

        public Explorer(Texture2D a_Texture, Vector2 a_Position)
            : base(a_Texture, a_Position)
        {
            m_Colour = Color.Yellow;
            currentBehaviour = new Wander(this);
        }

        public override void Update(GameTime gt)
        {
            //go outside level, reset pos
            if (m_Position.X < 0 || m_Position.X > Game1.g_iScreenWidth || m_Position.Y < 0 || m_Position.Y > Game1.g_iScreenHeight)
            {
                ResetPosition();
            }

            if (m_Velocity.Length() > 0)
            {
                m_Velocity.Normalize();
            }

            m_Velocity += currentBehaviour.GetForce();

            base.Update(gt);
        }

        public override void Draw(SpriteBatch sb, SpriteFont sf)
        {
            base.Draw(sb, sf);
        }
    }
}
