using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SteeringEntities
{
    public abstract class Entity
    {
        //Shared vars
        protected Vector2 m_Position;
        protected Vector2 m_Dimensions;
        protected Vector2 m_StartingPosition;
        protected bool m_isAlive = true;
        protected float m_Rotation;
        protected string m_Name;

        public Entity(Vector2 a_Position, Vector2 a_Dimensions)
        {
            m_Position = a_Position;
            m_Dimensions = a_Dimensions;
            m_Name = "Default";
            m_Rotation = 0;
            m_StartingPosition = m_Position;
        }

        public virtual void Update(GameTime gt)
        {
            
        }

        public virtual void Draw(SpriteBatch a_SB, SpriteFont a_SF)
        {

        }

        public virtual void TakeDamage(float damage)
        {

        }

        public virtual void OnCollide(float damage)
        {

        }

        public void ResetPosition(bool randomPos = true)
        {
            m_Position = randomPos ? new Vector2(Game1.cryptoRand.Next(0, Game1.g_iScreenWidth), Game1.cryptoRand.Next(0, Game1.g_iScreenHeight)) : m_StartingPosition;
        }

        public Rectangle GetRect
        {
            get { return new Rectangle((int)(m_Position.X), (int)(m_Position.Y), (int)m_Dimensions.X, (int)m_Dimensions.Y); }
        }

        public Vector2 Dimensions
        {
            get { return m_Dimensions; }
            set { m_Dimensions = value; }
        }

        public Vector2 Position
        {
            get { return m_Position; }
            set { m_Position = value; }
        }

        public Vector2 StartingPosition
        {
            get { return m_StartingPosition; }
            set { m_StartingPosition = value; }
        }

        public float Rotation
        {
            get { return m_Rotation; }
            set { m_Rotation = value; }
        }

        public bool IsAlive
        {
            get { return m_isAlive; }
            set { m_isAlive = value; }
        }

        public bool CheckCollision(Rectangle a_OtherRectangle)
        {
            return GetRect.Intersects(a_OtherRectangle);
        }
    }
}
