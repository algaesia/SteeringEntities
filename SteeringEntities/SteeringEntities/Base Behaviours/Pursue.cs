using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SteeringEntities
{
    public class Pursue : SteeringBehaviour
    {
        //pursue algorithm takes into account
        //user velo, so need whole entity
        //over simply vector for pos
        public MovableEntity target;

        public Pursue(MovableEntity a_Owner, MovableEntity a_Target)
        {
            behaviourType = BehaviourType.PURSUE;
            owner = a_Owner;
            target = a_Target;
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

            //force req includes velo
            Vector2 force = Vector2.Normalize((target.Position + target.Velocity) - owner.Position);
            force *= owner.MaxVelocity;
            return (force - owner.Velocity);
        }
    }
}
