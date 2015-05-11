using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SteeringEntities
{
    public class Evade : SteeringBehaviour
    {
        public MovableEntity target;

        public Evade(MovableEntity a_Owner, MovableEntity a_Target)
        {
            behaviourType = BehaviourType.EVADE;
            target = a_Target;
            owner = a_Owner;
        }

        public override void Draw(SpriteBatch sb, Texture2D a_Texture)
        {
            
        }

        public override Vector2 GetForce()
        {
            if (target == null)
            {
                return Vector2.Zero;
            }

            //takes into account chaser's velo
            Vector2 force = Vector2.Normalize(owner.Position - (target.Position + target.Velocity));
            force *= owner.MaxVelocity;
            return (force - owner.Velocity);
        }
    }
}
