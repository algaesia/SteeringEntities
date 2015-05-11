using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SteeringEntities
{
    public class Flee : SteeringBehaviour
    {
        public Vector2 target;

        public Flee(MovableEntity a_Owner, Vector2 a_Target)
        {
            behaviourType = BehaviourType.FLEE;
            target = a_Target;
            owner = a_Owner;
        }

        public override Vector2 GetForce()
        {
            //reverse of seek, no account of chaser's velo
            Vector2 force = Vector2.Normalize(owner.Position - target);
            force *= owner.MaxVelocity;
            return (force - owner.Velocity);
        }

        public override void Draw(SpriteBatch sb, Texture2D a_Texture)
        {
            
        }
    }
}
