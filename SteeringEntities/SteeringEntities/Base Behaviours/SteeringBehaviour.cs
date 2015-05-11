using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SteeringEntities
{
    //enum to identify different types of entities
    public enum BehaviourType
    {
        SEEK,
        FLEE,
        WANDER,
        PURSUE,
        EVADE,
        ARRIVAL,
    };

    public abstract class SteeringBehaviour
    {
        //the entity that owns the current behaviour
        protected MovableEntity owner;

        //behaviour type identifier
        protected BehaviourType behaviourType;

        //calculate the force required for specific behaviour
        public abstract Vector2 GetForce();

        //draw function to help with debugging
        public abstract void Draw(SpriteBatch sb, Texture2D a_Texture);
        public BehaviourType type
        {
            get { return behaviourType; }
        }
    }
}
