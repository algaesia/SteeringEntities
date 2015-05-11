using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SteeringEntities
{
    public class Obstacle : DrawableEntity
    {
        public bool Processed
        {
            get;
            set;
        }

        public bool Remove
        {
            get;
            set;
        }

        public Obstacle(Vector2 a_Position, Vector2 a_Dimensions, Texture2D a_Texture, Color a_Colour)
            : base(a_Position, a_Dimensions, a_Texture, a_Colour)
        {
            Processed = false;
            Remove = false;
        }

        public override void Update(GameTime gt)
        {
            base.Update(gt);
        }

        public override void Draw(SpriteBatch a_SB, SpriteFont a_SF)
        {
            base.Draw(a_SB, a_SF);
        }

        public override void OnCollide(float damage)
        {
            base.OnCollide(damage);
        }
    }
}
