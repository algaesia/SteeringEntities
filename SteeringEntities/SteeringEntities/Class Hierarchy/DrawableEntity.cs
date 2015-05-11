using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SteeringEntities
{
    public class DrawableEntity : Entity
    {
        protected Texture2D m_Texture;
        protected Color m_Colour;

        public DrawableEntity(Vector2 a_Position, Vector2 a_Dimensions, Texture2D a_Texture, Color a_Colour)
            : base(a_Position, a_Dimensions)
        {
            m_Texture = a_Texture;
            m_Colour = a_Colour;
        }

        public override void Draw(SpriteBatch a_SB, SpriteFont a_SF)
        {
            if (m_Texture == null)
            {
                Console.WriteLine("TEXTURE IS NULL");
                return;
            }

            if (m_isAlive)
            {
                a_SB.Draw(m_Texture, GetRect, null, m_Colour, m_Rotation, new Vector2(m_Texture.Width * 0.5f, m_Texture.Height * 0.5f), SpriteEffects.None, 0);
            }
        }
    }
}
