using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SteeringEntities
{
    //moves relentlessly towards cursor
    public class Chaser : Enemy
    {
        private SteeringBehaviour currentBehaviour;

        public Chaser(Texture2D a_Texture, Vector2 a_Position)
            : base(a_Texture, a_Position, 0.75f)
        {
            m_Colour = Color.Blue;
        }

        public override void Update(GameTime gt)
        {
            Vector2 mousePos = Vector2.Zero;
            mousePos.X = Mouse.GetState().X;
            mousePos.Y = Mouse.GetState().Y;

            currentBehaviour = new Seek(this, mousePos);

            m_Velocity += currentBehaviour.GetForce();

            base.Update(gt);
        }

        public override void Draw(SpriteBatch sb, SpriteFont sf)
        {
            base.Draw(sb, sf);
        }
    }
}
