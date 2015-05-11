using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SteeringEntities
{
    //generic enemy that doesn't really do much
    public class Enemy : MovableEntity
    {
        public Enemy(Texture2D a_Texture, Vector2 a_Position, float a_MovementSpeed = 50)
            : base(a_Position, new Vector2(16, 16), a_Texture, Color.Red, a_MovementSpeed, 100, 0.85f)
        {
            CurrentTarget = Game1.RandomPosition();
        }

        public override void Update(GameTime gt)
        {
            base.Update(gt);
        }

        public override void Draw(SpriteBatch sb, SpriteFont sf)
        {
            base.Draw(sb, sf);
        }

        public override void TakeDamage(float damage)
        {
            base.TakeDamage(damage);
        }

        public override void Shoot()
        {
            ProjectileManager.Instance.Shoot(m_Position, CurrentTarget, false);
        }
    }
}
