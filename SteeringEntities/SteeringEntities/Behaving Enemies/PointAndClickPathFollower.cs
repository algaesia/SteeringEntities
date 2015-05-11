using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SteeringEntities
{
    //click on a node and this enemy will path from
    //where it currently is to where you clicked
    public class PointAndClickPathFollower : Enemy
    {
        Queue<Node> pathQueue = new Queue<Node>();
        float distCloseEnough;

        //each entity has own reference to grid
        Graph m_LevelGrid;

        //list to debug nodes in generated path
        List<Node> pathList = new List<Node>();

        public PointAndClickPathFollower(Texture2D a_Texture, Vector2 a_Position, Graph a_LevelGrid)
            : base(a_Texture, a_Position)
        {
            m_LevelGrid = a_LevelGrid;
            m_Colour = Color.Blue;
            m_Dimensions = new Vector2(16, 16);
            distCloseEnough = m_LevelGrid.NodeSize().X / 2.0f;
        }

        public override void Update(GameTime gt)
        {
            //get mouse pos
            Vector2 mousePos = Vector2.Zero;
            mousePos.X = Mouse.GetState().X;
            mousePos.Y = Mouse.GetState().Y;

            //clicked mouse?
            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                Node nodeToMoveTo = m_LevelGrid.GetNearestNode(Position);
                Node startingNode = m_LevelGrid.GetNearestNode(mousePos);

                //make sure current node and node that was clicked on are valid
                if (nodeToMoveTo != null && startingNode != null)
                {
                    //clear previous path
                    pathQueue.Clear();

                    //calculate new one, using new args
                    m_LevelGrid.BuildAStarPath(startingNode, nodeToMoveTo, ref pathQueue);

                    //clear debugging path
                    pathList.Clear();

                    //convert queue to list
                    pathList = pathQueue.ToList();
                }
            }

            //are there nodes to move to?
            if (pathQueue.Count > 0)
            {
                //look at first node and process it, without
                //removing it
                Node currentTarget = pathQueue.Peek();
                m_Velocity = Vector2.Normalize(currentTarget.Position - m_Position);

                //close enough to current target?
                if (Vector2.Distance(currentTarget.Position, m_Position) < distCloseEnough)
                {
                    pathQueue.Dequeue();
                }
            }
            else
            {
                //nothing left to do, so just sit there
                m_Velocity = Vector2.Zero;
            }

            base.Update(gt);
        }

        public override void Draw(SpriteBatch sb, SpriteFont sf)
        {
            //for path debugging
            for (int i = 0; i < pathList.Count - 1; ++i)
            {
                Game1.DrawLine(pathList[i].Position, pathList[i + 1].Position, Color.Yellow);
            }

            base.Draw(sb, sf);
        }
    }
}
