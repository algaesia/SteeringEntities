using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SteeringEntities
{
    public class MovableEntity : AnimatedEntity
    {
        protected Vector2 m_Velocity;
        protected float m_CurrentMovementSpeed;
        protected float m_MaxVelo;
        protected float m_VeloDecayRate;

        public MovableEntity(Vector2 a_Position, Vector2 a_Dimensions, Texture2D a_Texture, Color a_Colour, float a_MovementSpeed, float a_MaxVelo, float a_VeloDecayRate)
            : base(a_Position, a_Dimensions, a_Texture, a_Colour)
        {
            m_Velocity = Vector2.Zero;
            m_CurrentMovementSpeed = a_MovementSpeed;
            m_MaxVelo = a_MaxVelo;
            m_VeloDecayRate = a_VeloDecayRate;
            m_Rotation = 0;
        }

        public override void Update(GameTime gt)
        {
            if (m_isAlive)
            {
                //m_Rotation = (float)(Math.Atan2(m_Velocity.Y, m_Velocity.X));

                m_Position += m_Velocity * m_CurrentMovementSpeed * (float)(gt.ElapsedGameTime.TotalSeconds);
            }

            base.Update(gt);
        }

        public override void Draw(SpriteBatch a_SB, SpriteFont a_SF)
        {
            base.Draw(a_SB, a_SF);
        }

        public Vector2 CurrentTarget
        {
            get;
            set;
        }

        public void MoveLeft()
        {
            m_Velocity += new Vector2(-1, 0);
        }

        public void MoveRight()
        {
            m_Velocity += new Vector2(1, 0);
        }

        public void MoveUp()
        {
            m_Velocity += new Vector2(0, -1);
        }

        public void MoveDown()
        {
            m_Velocity += new Vector2(0, 1);
        }

        public Vector2 Velocity
        {
            get { return m_Velocity; }
        }

        public float MaxVelocity
        {
            get { return m_MaxVelo; }
        }

        public virtual void Shoot()
        {
            
        }

        public float MovementSpeed
        {
            get { return m_CurrentMovementSpeed; }
        }
    }
}
