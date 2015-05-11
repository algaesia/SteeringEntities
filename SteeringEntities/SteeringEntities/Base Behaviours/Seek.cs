using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SteeringEntities
{
    public class Seek : SteeringBehaviour
    {
        public Vector2 target;

        public Seek(MovableEntity a_Owner, Vector2 a_Target)
        {
            behaviourType = BehaviourType.SEEK;
            target = a_Target;
            owner = a_Owner;
        }

        public override Vector2 GetForce()
        {
            //Get direction to target, return it
            Vector2 force = target - owner.Position;
            force.Normalize();

            force *= owner.MaxVelocity;

            return (force - owner.Velocity);
        }

        public override void Draw(SpriteBatch sb, Texture2D a_Texture)
        {
            
        }
    }
}
