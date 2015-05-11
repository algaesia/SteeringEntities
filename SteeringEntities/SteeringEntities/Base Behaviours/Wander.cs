using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SteeringEntities
{
    public class Wander : SteeringBehaviour
    {
        public Vector2 target;
        public float wanderRad = 100;
        public float wanderDist = 25;
        public float wanderJitter = 50;

        public Wander(MovableEntity a_Owner)
        {
            owner = a_Owner;
            target = owner.Position;
            behaviourType = BehaviourType.WANDER;
        }

        public override void Draw(SpriteBatch sb, Texture2D a_Texture)
        {
            
        }

        public override Vector2 GetForce()
        {
            //get angle, using current rotation
            float wanderAngle = owner.Rotation + (float)Game1.cryptoRand.NextDouble() * wanderJitter - wanderJitter * 0.5f;

            //using current pos, project forward by wander dist
            Vector2 circlePos = owner.Position + (owner.Velocity * wanderDist);

            //construct vector from angle using sin, cos
            //extend that vector using wander radius
            Vector2 circleTarget = owner.Position + new Vector2((float)Math.Cos(wanderAngle), (float)Math.Sin(wanderAngle)) * wanderRad;

            //calculate direction to calculated target
            Vector2 point = circleTarget - owner.Position;
            point.Normalize();
            point *= owner.MaxVelocity;
            return point;
        }
    }
}
