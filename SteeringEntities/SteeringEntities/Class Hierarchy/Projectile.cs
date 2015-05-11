using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SteeringEntities
{
    public class Projectile : MovableEntity
    {
        float m_MaxDistFromStart;

        float m_LifeTimer = 0;
        float m_MaxLifeTime = 1;

        public Projectile(Vector2 a_Position, Vector2 a_Dimensions, Texture2D a_Texture, Color a_Colour, float a_MovementSpeed, float a_MaxVelo, float a_VeloDecayRate)
            : base(a_Position, a_Dimensions, a_Texture, a_Colour, a_MovementSpeed, a_MaxVelo, a_VeloDecayRate)
        {
            Target = Vector2.Zero;
            m_isAlive = false;
            m_MaxDistFromStart = 250;
        }

        public override void Update(GameTime gt)
        {
            m_LifeTimer += (float)(gt.ElapsedGameTime.TotalSeconds);

            m_Velocity = Target - m_Position;

            if (Vector2.Distance(m_Position, m_StartingPosition) > m_MaxDistFromStart || m_LifeTimer > m_MaxLifeTime)
            {
                m_isAlive = false;
                m_LifeTimer = 0;
            }

            if (m_Velocity.Length() < 0.01f)
            {
                m_Velocity = Vector2.Zero;
            }

            m_Velocity *= m_VeloDecayRate;

            if (m_Velocity.Length() != 0)
            {
                float mag = m_Velocity.Length();

                m_Velocity = Vector2.Normalize(m_Velocity) * Math.Min(mag, MaxVelocity);
            }

            base.Update(gt);
        }

        public override void Draw(SpriteBatch a_SB, SpriteFont a_SF)
        {
            //m_Colour *= 1 - (Vector2.Distance(m_Position, m_StartingPosition) / m_MaxDistFromStart);

            if (!IsAlive)
            {
                return;
            }

            base.Draw(a_SB, a_SF);
        }

        public Vector2 Target
        {
            get;
            set;
        }

        public bool IsPlayerBullet
        {
            get;
            set;
        }

        public override void OnCollide(float damage)
        {
            base.OnCollide(damage);
        }
    }
}
