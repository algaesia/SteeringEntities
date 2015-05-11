using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SteeringEntities
{
    public class Flock : Enemy
    {
        //keeps track of list of other flocking enemies
        //passed in from game1, not always accurate
        private List<Flock> otherFlocking = new List<Flock>();

        //alignment variables - dist for start move same dir
        Vector2 alignment = Vector2.Zero;
        private float closeEnoughToBeAligned;
        private float maxDistAlignValue = 1000;
        private float alignmentWeight;
        private float alignmentWeightMax = 10;

        //cohesion variables - dist for need to get closer
        Vector2 cohesion = Vector2.Zero;
        private float closeEnoughToBeCohesive;
        private float maxDistCohesionValue = 500;
        private float cohesionWeight;
        private float cohesionWeightMax = 10;

        //separation variables - dist for need to separate
        Vector2 separation = Vector2.Zero;
        private float closeEnoughNeedSeparation;
        private float maxDistSeparationValue = 250;
        private float separationWeight;
        private float separationWeightMax = 10;

        //too far from some centre point
        //when moved too far from centre, 
        //then move back towards centre
        public Vector2 arbCentrePoint = Vector2.Zero;
        private float tooFarFromCentrePoint = 250;
        private float moveToCentreWeight;
        private float moveToCentreWeightMax = 10;

        //wall avoidance
        private float wallRebound = 0;
        private float wallReboundMax = 100;

        //dist to mouse - value for too close to mouse
        private float closeEnoughToMouse;
        private float maxDistCloseValue = 500;
        private float mouseWeight = 25;
        private bool avoidMouse = false;

        public Flock(Texture2D a_Texture, Vector2 a_Position, List<Flock> a_OtherFlocking, float a_MovementSpeed = 0.1f)
            : base(a_Texture, a_Position, 250)
        {
            otherFlocking = a_OtherFlocking;

            //generate random colour
            m_Colour = Game1.RandomColour();

            //generate random direction
            m_Velocity = Game1.RandomDirection();

            //generate random rebound float
            wallRebound = (float)Game1.cryptoRand.NextDouble() * wallReboundMax;

            maxDistCohesionValue = (float)Game1.cryptoRand.NextDouble() * 1000;
            maxDistAlignValue = (float)Game1.cryptoRand.NextDouble() * 1000;
            maxDistSeparationValue = (float)Game1.cryptoRand.NextDouble() * 1000;
            maxDistCloseValue = (float)Game1.cryptoRand.NextDouble() * 1000;

            closeEnoughToBeCohesive = (float)Game1.cryptoRand.NextDouble() * maxDistCohesionValue;
            closeEnoughToBeAligned = (float)Game1.cryptoRand.NextDouble() * maxDistAlignValue;
            closeEnoughNeedSeparation = (float)Game1.cryptoRand.NextDouble() * maxDistSeparationValue;
            closeEnoughToMouse = (float)Game1.cryptoRand.NextDouble() * maxDistCloseValue;

            cohesionWeight = (float)Game1.cryptoRand.NextDouble() * cohesionWeightMax;
            alignmentWeight = (float)Game1.cryptoRand.NextDouble() * alignmentWeightMax;
            separationWeight = (float)Game1.cryptoRand.NextDouble() * separationWeightMax;
            moveToCentreWeight = (float)Game1.cryptoRand.NextDouble() * moveToCentreWeightMax;
        }

        //move away from mouse
        private Vector2 ComputeMouseAvoidance()
        {
            Vector2 point = Vector2.Zero;
            Vector2 mousePos = new Vector2(Mouse.GetState().Position.X, Mouse.GetState().Position.Y);
            Vector2 dirToMouse = Position - mousePos;
            dirToMouse.Normalize();

            if (Vector2.Distance(mousePos, Position) < closeEnoughToMouse)
            {
                //within range, so move away
                point = dirToMouse;
            }

            return point * mouseWeight;
        }

        private Vector2 ComputeMoveTowardsCentre()
        {
            Vector2 point = Vector2.Zero;

            if (Vector2.Distance(Position, arbCentrePoint) > tooFarFromCentrePoint)
            {
                point = arbCentrePoint - Position;

                if (point.Length() > 0)
                {
                    point.Normalize();
                }
            }

            return point * moveToCentreWeight;
        }

        //move away from wall
        private Vector2 ComputeWallAvoidance()
        {
            Vector2 point = Vector2.Zero;

            //left wall
            if (m_Position.X + m_Velocity.X * m_CurrentMovementSpeed < 0)
            {
                //alter pos slightly
                m_Position.X = m_Velocity.X * m_CurrentMovementSpeed;

                //aggregate overall rebound direction
                point += new Vector2(wallRebound, 0);
            }

            //top wall
            if (m_Position.Y + m_Velocity.X * m_CurrentMovementSpeed < 0)
            {
                m_Position.Y = m_Velocity.Y * m_CurrentMovementSpeed;
                point += new Vector2(0, wallRebound);
            }

            //right wall
            if (m_Position.X + m_Velocity.X * m_CurrentMovementSpeed + GetRect.Width > Game1.g_iScreenWidth)
            {
                m_Position.X = Game1.g_iScreenWidth - m_Velocity.X * m_CurrentMovementSpeed - GetRect.Width;
                point += new Vector2(-wallRebound, 0);
            }

            //bottomw wall
            if (m_Position.Y + m_Velocity.Y * m_CurrentMovementSpeed + GetRect.Height > Game1.g_iScreenHeight)
            {
                m_Position.Y = Game1.g_iScreenHeight - m_Velocity.Y * m_CurrentMovementSpeed - GetRect.Height;
                point += new Vector2(0, -wallRebound);
            }

            if (point.Length() > 0)
            {
                point.Normalize();
            }

            return point;
        }

        //alignment, cohesion, separation
        private Vector2 ComputeOverall()
        {
            alignment = Vector2.Zero;
            int alignmentNeighbours = 0;

            cohesion = Vector2.Zero;
            int cohesionNeighbours = 0;

            separation = Vector2.Zero;
            int separationNeighbours = 0;

            int numFlocking = otherFlocking.Count;

            for (int i = 0; i < numFlocking; ++i)
            {
                Flock current = otherFlocking[i];

                //don't check yourself
                if (current == this)
                {
                    continue;
                }

                float dist = Vector2.DistanceSquared(Position, current.Position);

                //alignment check
                if (dist < closeEnoughToBeAligned)
                {
                    //aggregate velo
                    alignment += current.Velocity;
                    alignmentNeighbours++;
                }

                //cohesion check
                if (dist < closeEnoughToBeCohesive)
                {
                    //aggregate pos
                    cohesion += current.Position;
                    cohesionNeighbours++;
                }

                //separation check
                if (dist < closeEnoughNeedSeparation)
                {
                    //aggregate pos
                    separation += current.Position;
                    separationNeighbours++;
                }
            }

            //only calculate if neighbours > 0, since dividing
            //by number of neighbours
            if (alignmentNeighbours > 0)
            {
                //take average velo of neighbours
                alignment /= alignmentNeighbours;
                if (alignment.Length() > 0)
                {
                    alignment.Normalize();
                }
            }

            if (cohesionNeighbours > 0)
            {
                //take average pos of neighbours
                cohesion /= cohesionNeighbours;

                //get dir from current pos to average point
                //move towards point
                cohesion = cohesion - Position;
                if (cohesion.Length() > 0)
                {
                    cohesion.Normalize();
                }
            }

            if (separationNeighbours > 0)
            {
                separation /= separationNeighbours;

                //get dir from average separation
                //point to current pos - move away from point
                separation = Position - separation;

                if (separation.Length() > 0)
                {
                    separation.Normalize();
                }
            }

            //perform calc
            return alignment * alignmentWeight + cohesion * cohesionWeight + separation * separationWeight;
        }

        public override void Update(GameTime gt)
        {
            //avoid mouse when left click is pressed - only recalcs after
            //button has been released after pressed - <bug>
            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                avoidMouse = true;
            }
            else
            {
                avoidMouse = false;
            }

            //add to own direction
            m_Velocity += ComputeOverall() /*+ ComputeWallAvoidance()*/ + (avoidMouse ? ComputeMouseAvoidance() : Vector2.Zero) + ComputeMoveTowardsCentre();

            //normalise direction
            if (m_Velocity.Length() > 0)
            {
                m_Velocity *= m_VeloDecayRate;
                m_Velocity.Normalize();
            }

            base.Update(gt);
        }

        public override void Draw(SpriteBatch sb, SpriteFont sf)
        {
            base.Draw(sb, sf);
            sb.Draw(Game1.whiteSquare, new Rectangle((int)arbCentrePoint.X, (int)arbCentrePoint.Y, 8, 8), Color.White);
        }

        #region Helper functions
        public float AlignmentDist
        {
            get { return closeEnoughToBeAligned; }
            set
            {
                if (value < 0)
                {
                    closeEnoughToBeAligned = 0;
                }
                else if (value > maxDistAlignValue)
                {
                    closeEnoughToBeAligned = maxDistAlignValue;
                }
                else
                {
                    closeEnoughToBeAligned = value;
                }
            }
        }

        public float CohesionDist
        {
            get { return closeEnoughToBeCohesive; }
            set
            {
                if (value < 0)
                {
                    closeEnoughToBeCohesive = 0;
                }
                else if (value > maxDistCohesionValue)
                {
                    closeEnoughToBeCohesive = maxDistCohesionValue;
                }
                else
                {
                    closeEnoughToBeCohesive = value;
                }
            }
        }

        public float SeparationDist
        {
            get { return closeEnoughNeedSeparation; }
            set
            {
                if (value < 0)
                {
                    closeEnoughNeedSeparation = 0;
                }
                else if (value > maxDistSeparationValue)
                {
                    closeEnoughNeedSeparation = maxDistSeparationValue;
                }
                else
                {
                    closeEnoughNeedSeparation = value;
                }
            }
        }

        public float MaxAlignment
        {
            get { return maxDistAlignValue; }
        }

        public float MaxCohesion
        {
            get { return maxDistCohesionValue; }
        }

        public float MaxSeparation
        {
            get { return maxDistSeparationValue; }
        }

        public float MaxAlignmentWeight
        {
            get { return alignmentWeightMax; }
        }

        public float MaxCohesionWeight
        {
            get { return cohesionWeightMax; }
        }

        public float MaxSeparationWeight
        {
            get { return separationWeightMax; }
        }

        public float MaxMoveToCentreWeight
        {
            get { return moveToCentreWeightMax; }
        }

        public float AlignmentWeight
        {
            get { return alignmentWeight; }
            set
            {
                if (value < 0)
                {
                    alignmentWeight = 0;
                }
                else if (value > 1)
                {
                    alignmentWeight = 1;
                }
                else
                {
                    alignmentWeight = value;
                }
            }
        }

        public float CohesionWeight
        {
            get { return cohesionWeight; }
            set
            {
                if (value < 0)
                {
                    cohesionWeight = 0;
                }
                else if (value > 1)
                {
                    cohesionWeight = 1;
                }
                else
                {
                    cohesionWeight = value;
                }
            }
        }

        public float SeparationWeight
        {
            get { return separationWeight; }
            set
            {
                if (value < 0)
                {
                    separationWeight = 0;
                }
                else if (value > 1)
                {
                    separationWeight = 1;
                }
                else
                {
                    separationWeight = value;
                }
            }
        }

        public float MoveToCentreWeight
        {
            get { return moveToCentreWeight; }
            set
            {
                if (value < 0)
                {
                    moveToCentreWeight = 0;
                }
                else if (value > 1)
                {
                    moveToCentreWeight = 1;
                }
                else
                {
                    moveToCentreWeight = value;
                }
            }
        }
        #endregion
    }
}
