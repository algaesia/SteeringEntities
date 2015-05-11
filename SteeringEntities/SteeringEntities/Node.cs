using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SteeringEntities
{
    //used to represent nodes within the level
    public class Node
    {
        public enum NODE_TYPE
        {
            NOT_WALKABLE = -1,
            WALL,
            WATER,
            WALKABLE,
        }

        // to help identify what node it is
        NODE_TYPE nodeType;

        Vector2 m_Position;
        Vector2 m_Dimensions;

        //to link nodes in the path from one to the next
        Node m_PreviousNode;

        //node to get to
        Node m_GoalNode;
        public Color colour;

        //GCost = cost to get to current node
        float m_gCost;

        //HCost = heuristic cost
        float m_hCost;
        //FCost = final cost
        float m_fCost;

        int m_NodeNumber;

        bool m_bVisited;

        public List<Edge> edges;

        public Node(int a_NodeNumber, Vector2 a_Position, NODE_TYPE a_Type = NODE_TYPE.WALKABLE)
        {
            nodeType = a_Type;
            m_NodeNumber = a_NodeNumber;
            m_Position = a_Position;
            m_PreviousNode = null;
            m_GoalNode = null;
            m_bVisited = false;
            edges = new List<Edge>();

            //red = walls
            //blue = water
            //black = unwalkable
            //white = walkable
            switch (a_Type)
            {
                case NODE_TYPE.WALKABLE:
                    colour = Color.White;
                    m_Dimensions = new Vector2(10, 10);
                    break;
                case NODE_TYPE.WALL:
                    colour = Color.Red;
                    m_Dimensions = new Vector2(8, 8);
                    break;
                case NODE_TYPE.WATER:
                    colour = Color.Blue;
                    m_Dimensions = new Vector2(7, 7);
                    break;
                case NODE_TYPE.NOT_WALKABLE:
                    colour = Color.Black;
                    m_Dimensions = new Vector2(5, 5);
                    break;
                default:
                    colour = Color.Gray;
                    m_Dimensions = new Vector2(4, 4);
                    break;
            }
        }

        public bool HasEdge(Node a_NodeToCheck)
        {
            foreach (Edge n in edges)
            {
                if (n.end == a_NodeToCheck)
                {
                    return true;
                }
            }
            return false;
        }

        public void Draw(SpriteBatch a_SB, SpriteFont a_SF, bool drawNodeNum = false)
        {
            a_SB.Draw(Game1.whiteSquare, GetRectangle, colour);
            if (drawNodeNum)
            {
                a_SB.DrawString(a_SF, m_NodeNumber.ToString(), m_Position, Color.Black);
            }

            foreach (Edge n in edges)
            {
                if ((nodeType == NODE_TYPE.NOT_WALKABLE || n.end.GetNodeType == NODE_TYPE.NOT_WALKABLE) || (nodeType == NODE_TYPE.WALL || n.end.GetNodeType == NODE_TYPE.WALL))
                {
                    continue;
                }

                n.Draw(a_SB, a_SF, Game1.whiteSquare, true);
            }
        }

        public int NodeNumber
        {
            get { return m_NodeNumber; }
        }

        public Vector2 Position
        {
            get { return m_Position; }
        }

        public Rectangle GetRectangle
        {
            get
            {
                return new Rectangle((int)(m_Position.X), (int)(m_Position.Y), (int)m_Dimensions.X, (int)m_Dimensions.Y);
            }
        }

        public NODE_TYPE GetNodeType
        {
            get { return nodeType; }
        }

        public float GCost
        {
            get { return m_gCost; }
            set { m_gCost = value; }
        }

        public float FCost
        {
            get { return m_fCost; }
            set { m_fCost = value; }
        }

        public float HCost
        {
            get { return m_hCost; }
            set { m_hCost = value; }
        }

        public void ResetCosts()
        {
            m_fCost = float.MaxValue;
            m_hCost = float.MaxValue;
            m_gCost = float.MaxValue;
        }

        public bool Visited
        {
            get { return m_bVisited; }
            set { m_bVisited = value; }
        }

        public Node PreviousNode
        {
            get { return m_PreviousNode; }
            set { m_PreviousNode = value; }
        }

        public Node GoalNode
        {
            get { return m_GoalNode; }
            set { m_GoalNode = value; }
        }

        public float GetDistanceHeuristic()
        {
            if (m_GoalNode != null)
            {
                return Vector2.Distance(m_Position, m_GoalNode.m_Position) * 0.1f;
            }
            return float.MaxValue;
        }

        public float GetManhattanHeuristic()
        {
            if (m_GoalNode != null)
            {
                return (m_GoalNode.m_Position.X - m_Position.X + m_GoalNode.m_Position.Y - m_Position.Y);
            }
            return float.MaxValue;
        }

        public void AddEdge(Node a_OtherNode)
        {
            if (HasEdge(a_OtherNode))
            {
                return;
            }

            Edge temp = new Edge();

            temp.start = this;
            temp.end = a_OtherNode;

            switch (a_OtherNode.GetNodeType)
            {
                case NODE_TYPE.WALKABLE:
                    temp.cost = 1;
                    break;
                case NODE_TYPE.NOT_WALKABLE:
                    temp.cost = float.MaxValue;
                    break;
                case NODE_TYPE.WALL:
                    temp.cost = float.MaxValue;
                    break;
                case NODE_TYPE.WATER:
                    temp.cost = 5;
                    break;
            }

            edges.Add(temp);
        }

        public void RemoveEdge(Node a_OtherNode)
        {
            if (!HasEdge(a_OtherNode))
            {
                return;
            }

            //Current node has other node as edge end
            for (int i = 0; i < edges.Count; ++i)
            {
                if (edges[i].end == a_OtherNode)
                {
                    edges.Remove(edges[i]);
                    return;
                }
            }
        }
    }
}
