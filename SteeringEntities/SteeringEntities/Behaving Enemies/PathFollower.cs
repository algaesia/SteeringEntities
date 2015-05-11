using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SteeringEntities
{
    //this entity simply finds a path every 1.5 seconds (getNewPath var)
    //moves to the beginning of that path and moves along it
    public class PathFollower : Enemy
    {
        public Queue<Node> pathQueue = new Queue<Node>();
        float distCloseEnough = 10;
        float newPathTimer = 0;
        float getNewPath = 1.5f;

        List<Node> pathList = new List<Node>();
        Graph m_LevelGrid;

        public PathFollower(Texture2D a_Texture, Vector2 a_Position, Graph a_LevelGrid)
            : base(a_Texture, a_Position)
        {
            m_LevelGrid = a_LevelGrid;

            //rand nums in the node list
            int randNum1 = Game1.cryptoRand.Next(0, m_LevelGrid.NumNodes);
            int randNum2 = Game1.cryptoRand.Next(0, m_LevelGrid.NumNodes);

            //make sure we are not generating numbers that are the same as the wall node type
            //counter to keep track of how many iterations have occurred
            int counter = 0;
            while (randNum1 == (int)Node.NODE_TYPE.WALL || randNum2 == (int)Node.NODE_TYPE.WALL)
            {
                counter++;
                //stop after 10 iterations - magic number
                if (counter > 10)
                {
                    break;
                }

                randNum1 = Game1.cryptoRand.Next(0, m_LevelGrid.NumNodes);
                randNum2 = Game1.cryptoRand.Next(0, m_LevelGrid.NumNodes);
            }

            //build a path from rand num 1 to rand num 2
            m_LevelGrid.BuildAStarPath(m_LevelGrid.AtID(randNum1), m_LevelGrid.AtID(randNum2), ref pathQueue);
            pathList = pathQueue.ToList();
        }

        public override void Update(GameTime gt)
        {
            //as long as there are nodes in the queue
            if (pathQueue.Count > 0)
            {
                //look at the next node along
                Node currentTarget = pathQueue.Peek();

                //calculate direction to target
                m_Velocity = Vector2.Normalize(currentTarget.Position - m_Position);

                //close enough, remove node from queue
                if (Vector2.Distance(currentTarget.Position, m_Position) < distCloseEnough)
                {
                    pathQueue.Dequeue();
                }
            }
            else
            {
                //progress timer til new path is generated
                newPathTimer += (float)(gt.ElapsedGameTime.TotalSeconds);
                if (newPathTimer > getNewPath)
                {
                    m_LevelGrid.BuildAStarPath(m_LevelGrid.AtID(Game1.cryptoRand.Next(0, m_LevelGrid.NumNodes)), m_LevelGrid.AtID(Game1.cryptoRand.Next(0, m_LevelGrid.NumNodes)), ref pathQueue);
                    pathList = pathQueue.ToList();
                    newPathTimer = 0;
                }

                //no path, so sit still
                m_Velocity = Vector2.Zero;
            }

            base.Update(gt);
        }

        public override void Draw(SpriteBatch sb, SpriteFont sf)
        {
            //draw generated path
            for (int i = 0; i < pathList.Count - 1; ++i)
            {
                Game1.DrawLine(pathList[i].Position, pathList[i + 1].Position, Color.Black);
            }

            base.Draw(sb, sf);
        }
    }
}
