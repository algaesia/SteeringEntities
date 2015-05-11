using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SteeringEntities
{
    public class DrawnCharacter : MovableEntity
    {
        private float m_MaxDistFromStart = Game1.g_iScreenHeight * 1.5f;
        private char m_Value = 'a';
        private float fade = 0;

        public DrawnCharacter(Vector2 a_Position)
            : base(a_Position, new Vector2(8, 8), null, new Color(0, (int)(Game1.cryptoRand.NextDouble() * 255), 0, 255), 200, 250, 0.85f)
        {
            m_Value = (char)(Game1.cryptoRand.Next(33, 126));
            m_Velocity = new Vector2(0, 1);
            m_CurrentMovementSpeed = (float)(Game1.cryptoRand.NextDouble() * m_MaxVelo);
        }

        public override void Update(GameTime gt)
        {
            fade = 1 - (Vector2.Distance(Position, m_StartingPosition) / m_MaxDistFromStart);

            if (Vector2.Distance(Position, m_StartingPosition) > m_MaxDistFromStart || m_Velocity.Length() <= 0.01f ||
                GetRect.Intersects(new Rectangle((int)Mouse.GetState().Position.X, (int)Mouse.GetState().Position.Y, 16, 16)))
            {
                m_isAlive = false;
            }

            base.Update(gt);
        }

        public override void Draw(SpriteBatch sb, SpriteFont sf)
        {
            if (m_isAlive)
            {
                sb.DrawString(sf, m_Value.ToString(), Position, m_Colour * fade);
            }
        }

        public void Reset()
        {
            ResetPosition(false);
            m_Value = (char)(Game1.cryptoRand.Next(33, 126));
            m_CurrentMovementSpeed = (float)(Game1.cryptoRand.NextDouble() * m_MaxVelo);
            m_Colour = new Color(0, (int)(Game1.cryptoRand.NextDouble() * 255), 0, 255);
            m_isAlive = true;
        }
    }
}
