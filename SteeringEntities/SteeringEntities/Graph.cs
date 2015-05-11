using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SteeringEntities
{
    //to keep track of node set up
    public class Graph
    {
        float currentPathTotalCost = 0;
        bool pathCalculated = false;
        public List<Node> nodes = new List<Node>();
        public List<Node> openList = new List<Node>();

        int gridWidth, gridHeight, nodeWidth, nodeHeight;
        int levelWidth, levelHeight;
        Node[,] levelData;
        Texture2D levelTexture;

        //grid width and height doesn't really work as intended yet
        public Graph(int a_GridWidth, int a_GridHeight, string a_LevelName = "costs")
        {
            levelTexture = GameStateManager.Instance.Content.Load<Texture2D>(a_LevelName);
            gridWidth = a_GridWidth;
            gridHeight = a_GridHeight;

            nodeWidth = Game1.g_iScreenWidth / gridWidth;
            nodeHeight = Game1.g_iScreenHeight / gridHeight;

            GenerateGridFromFile();
        }

        public void AddNode(Node a_Node)
        {
            //check that the node doesnt already exist
            if (IsInNodeList(a_Node))
            {
                return;
            }

            nodes.Add(a_Node);
        }

        //get node in list based on index
        public Node At(int index)
        {
            //make sure number passed in is within correct bounds
            if (index >= 0 && index < nodes.Count)
            {
                return nodes[index];
            }
            return null;
        }

        //get node in list based on node ID
        public Node AtID(int ID)
        {
            foreach (Node n in nodes)
            {
                if (n.NodeNumber == ID)
                {
                    return n;
                }
            }
            return null;
        }

        //remove edges of passed in node if it has any
        //remove node from list
        public void RemoveNode(Node a_Node)
        {
            for (int i = 0; i < nodes.Count; ++i)
            {
                for (int j = 0; j < nodes[i].edges.Count; ++j)
                {
                    if (nodes[i].edges[j].end == a_Node)
                    {
                        nodes[i].edges.Remove(nodes[i].edges[j]);
                    }
                }
            }

            for (int i = 0; i < nodes.Count; ++i)
            {
                if (nodes[i] == a_Node)
                {
                    nodes.Remove(a_Node);
                    return;
                }
            }
        }

        public bool IsInNodeList(Node a_Node)
        {
            foreach (Node n in nodes)
            {
                if (n == a_Node)
                {
                    return true;
                }
            }
            return false;
        }

        public void AddEdge(Node a_StartNode, Node a_EndNode)
        {
            //make sure that both the start and end node exists
            if (!IsInNodeList(a_StartNode) && !IsInNodeList(a_EndNode))
            {
                return;
            }

            foreach (Node n in nodes)
            {
                //make a link from start to end node
                if (n == a_StartNode)
                {
                    n.AddEdge(a_EndNode);
                    return;
                }
            }
        }

        public void RemoveEdge(Node a_StartNode, Node a_EndNode)
        {
            if (!IsInNodeList(a_StartNode))
            {
                return;
            }

            for (int i = 0; i < nodes.Count; ++i)
            {
                if (nodes[i] == a_StartNode)
                {
                    nodes[i].RemoveEdge(a_EndNode);
                    return;
                }
            }
        }

        public void Draw(SpriteBatch sb, SpriteFont sf)
        {
            foreach (Node n in nodes)
            {
                n.Draw(sb, sf);
            }
        }

        public float GetPathTotalCost
        {
            get { return pathCalculated ? currentPathTotalCost : 0.0f; }
        }

        public bool BuildAStarPath(Node start, Node end, ref Queue<Node> pathQueue)
        {
            if (AStar(start, end))
            {
                Node currentNode = end;

                while (currentNode != start && currentNode.PreviousNode != null)
                {
                    //start at current node,
                    //project towards grandparent
                    //if no intersection occurs,
                    //add grandparent, otherwise
                    //add parent

                    int lineWidth = 16;
                    int lineHeight = 16;

                    float multiplier = 0;
                    float maxMultiplier = 50;
                    float multiplierIncr = 5;

                    Vector2 currentPos = currentNode.Position;
                    Vector2 direction = currentNode.PreviousNode.PreviousNode.Position - currentPos;
                    direction.Normalize();

                    while (true)
                    {
                        //multiplier going too high,
                        //haven't hit anything yet,
                        //so leave the loop
                        if (Math.Abs(multiplier - maxMultiplier) <= 0.5f)
                        {
                            //hit end of the line, no intersection
                            //so just add the parent instead
                            pathQueue.Enqueue(currentNode);
                            currentNode = currentNode.PreviousNode;
                            break;
                        }

                        //shoot ray towards grandparent (currentNode->prevNode->prevNode)
                        currentPos += direction * multiplier;
                        Rectangle line = new Rectangle((int)currentPos.X + (int)(lineWidth * 0.5f), (int)currentPos.Y + (int)(lineHeight * 0.5f), lineWidth, lineHeight);

                        //check collision with grandparent
                        //&& (currentNode.GetNodeType != Node.NODE_TYPE.WALL || currentNode.GetNodeType != Node.NODE_TYPE.NOT_WALKABLE)
                        if (line.Intersects(currentNode.PreviousNode.PreviousNode.GetRectangle))
                        {
                            //collision, so add grandparent, leave loop
                            pathQueue.Enqueue(currentNode.PreviousNode.PreviousNode);
                            currentNode = currentNode.PreviousNode.PreviousNode;
                            break;
                        }

                        //progress multiplier
                        multiplier += multiplierIncr;
                    }
                }

                pathQueue.Reverse();

                pathCalculated = true;
                return true;
            }
            pathCalculated = false;
            return false;
        }

        void GenerateGrid()
        {
            for (int i = 0; i < gridWidth; ++i)
            {
                for (int j = 0; j < gridHeight; ++j)
                {
                    AddNode(new Node(gridWidth * i + j, new Vector2(nodeWidth * (i + 0.5f), nodeHeight * (j + 0.5f)), 
                            Game1.cryptoRand.NextDouble() < 0.9 ? (Node.NODE_TYPE)(Game1.cryptoRand.NextDouble() * Enum.GetNames(typeof(Node.NODE_TYPE)).Length) : (Node.NODE_TYPE.NOT_WALKABLE)));
                }
            }

            for (int i = 0; i < gridHeight - 1; ++i)
            {
                for (int j = 0; j < gridWidth - 1; ++j)
                {
                    //wall or not walkable, don't make edge to it
                    if (AtID(gridWidth * i + j).GetNodeType == Node.NODE_TYPE.NOT_WALKABLE || AtID(gridWidth * i + j).GetNodeType == Node.NODE_TYPE.WALL)
                    {
                        continue;
                    }

                    if (i - 1 >= 0)
                    {
                        int distToNodeBottom = (int)Vector2.Distance(AtID(gridWidth * i + j).Position,
                                                                  AtID(gridHeight * (i - 1) + j).Position);

                        int distToNodeTop = (int)Vector2.Distance(AtID(gridWidth * i + j).Position,
                                                                  AtID(gridHeight * (i + 1) + j).Position);

                        if (distToNodeBottom <= nodeWidth)
                        {
                            AddEdge(AtID(gridWidth * i + j), AtID(gridHeight * (i - 1) + j));
                        }

                        if (distToNodeTop <= nodeWidth)
                        {
                            AddEdge(AtID(gridWidth * i + j), AtID(gridHeight * (i + 1) + j));
                        }
                    }

                    if (j - 1 >= 0)
                    {
                        int distToNodeLeft = (int)Vector2.Distance(AtID(gridWidth * i + j).Position,
                                                                  AtID(gridHeight * i + j - 1).Position);

                        int distToNodeRight = (int)Vector2.Distance(AtID(gridWidth * i + j).Position,
                                                                  AtID(gridHeight * i + j + 1).Position);

                        if (distToNodeLeft <= nodeHeight)
                        {
                            AddEdge(AtID(gridWidth * i + j), AtID(gridHeight * i + j - 1));
                        }

                        if (distToNodeRight <= nodeHeight)
                        {
                            AddEdge(AtID(gridWidth * i + j), AtID(gridHeight * i + j + 1));
                        }
                    }
                }
            }
        }

        void GenerateGridFromFile()
        {
            //red = walls
            //blue = water
            //black = unwalkable
            //white = walkable

            //numbers based on raw dimensions of texture
            levelWidth = levelTexture.Width;
            levelHeight = levelTexture.Height;

            //create jagged array based on raw dimensions of texture
            levelData = new Node[levelWidth, levelHeight];

            //create 1D array of colours
            Color[] colours = new Color[levelWidth * levelHeight];

            //get the data from the texture2d variable,
            //assigning the values to the colours array
            levelTexture.GetData(colours);

            //create jagged array based on raw dimensions of texture
            Color[,] mapData = new Color[levelWidth, levelHeight];

            //converting linear array to 2d array,
            //for easier traversal
            for (int i = 0; i < levelWidth; ++i)
            {
                for (int j = 0; j < levelHeight; ++j)
                {
                    //1D index maps to 2D index based on i + j * levelWidth
                    mapData[i, j] = colours[i + j * levelWidth];
                }
            }

            Color currentColour = Color.White;

            for (int i = 0; i < levelWidth; ++i)
            {
                for (int j = 0; j < levelHeight; ++j)
                {
                    currentColour = mapData[i, j];

                    //not walkable
                    if (currentColour.PackedValue == Color.Black.PackedValue)
                    {
                        AddNode(new Node(levelWidth * i + j, new Vector2(nodeWidth * (i), nodeHeight * (j)), Node.NODE_TYPE.NOT_WALKABLE));
                    }
                    //walls
                    else if (currentColour.PackedValue == Color.Red.PackedValue)
                    {
                        AddNode(new Node(levelWidth * i + j, new Vector2(nodeWidth * (i), nodeHeight * (j)), Node.NODE_TYPE.WALL));
                    }
                    //water
                    else if (currentColour.PackedValue == Color.Blue.PackedValue)
                    {
                        AddNode(new Node(levelWidth * i + j, new Vector2(nodeWidth * (i), nodeHeight * (j)), Node.NODE_TYPE.WATER));
                    }
                    //walkable
                    else if (currentColour.PackedValue == Color.White.PackedValue)
                    {
                        AddNode(new Node(levelWidth * i + j, new Vector2(nodeWidth * (i), nodeHeight * (j)), Node.NODE_TYPE.WALKABLE));
                    }
                    else
                    {
                        AddNode(new Node(levelWidth * i + j, new Vector2(nodeWidth * (i), nodeHeight * (j)), Node.NODE_TYPE.NOT_WALKABLE));
                    }
                }
            }

            for (int i = 0; i < levelHeight - 1; ++i)
            {
                for (int j = 0; j < levelWidth - 1; ++j)
                {
                    //wall or not walkable, don't make edge to it
                    if (AtID(levelWidth * i + j).GetNodeType == Node.NODE_TYPE.NOT_WALKABLE || AtID(levelWidth * i + j).GetNodeType == Node.NODE_TYPE.WALL)
                    {
                        continue;
                    }

                    int nodeIDCurrent = levelWidth * i + j;
                    int nodeIDBottom = levelWidth * (i + 1) + j;
                    int nodeIDTop = levelWidth * (i - 1) + j;
                    int nodeIDLeft = levelWidth * i + j - 1;
                    int nodeIDRight = levelWidth * i + j + 1;

                    if (i - 1 >= 0)
                    {
                        int distToNodeBottom = (int)Vector2.Distance(AtID(nodeIDCurrent).Position,
                                                                  AtID(nodeIDBottom).Position);

                        int distToNodeTop = (int)Vector2.Distance(AtID(nodeIDCurrent).Position,
                                                                  AtID(nodeIDTop).Position);

                        if (distToNodeBottom <= nodeWidth)
                        {
                            AddEdge(AtID(nodeIDCurrent), AtID(nodeIDBottom));
                        }

                        if (distToNodeTop <= nodeWidth)
                        {
                            AddEdge(AtID(nodeIDCurrent), AtID(nodeIDTop));
                        }
                    }

                    if (j - 1 >= 0)
                    {
                        int distToNodeLeft = (int)Vector2.Distance(AtID(nodeIDCurrent).Position,
                                                                  AtID(nodeIDLeft).Position);

                        int distToNodeRight = (int)Vector2.Distance(AtID(nodeIDCurrent).Position,
                                                                  AtID(nodeIDRight).Position);

                        if (distToNodeLeft <= nodeHeight)
                        {
                            AddEdge(AtID(nodeIDCurrent), AtID(nodeIDLeft));
                        }

                        if (distToNodeRight <= nodeHeight)
                        {
                            AddEdge(AtID(nodeIDCurrent), AtID(nodeIDRight));
                        }
                    }
                }
            }
        }

        public Vector2 NodeSize()
        {
            return new Vector2(nodes[0].GetRectangle.Width, nodes[0].GetRectangle.Height);
        }

        public int NumNodes
        {
            get { return nodes.Count; }
        }

        void ResetVisited()
        {
            foreach (Node n in nodes)
            {
                n.Visited = false;
            }
        }

        void ResetPreviousNode()
        {
            foreach (Node n in nodes)
            {
                n.PreviousNode = null;
            }
        }

        void ResetCosts()
        {
            foreach (Node n in nodes)
            {
                n.ResetCosts();
            }
        }

        bool AStar(Node start, Node end)
        {
            //reset costs to float max
            ResetCosts();

            //reset bool visited
            ResetVisited();

            //reset prev node to be null
            ResetPreviousNode();

            //stores nodes to be processed
            List<Node> nodeList = new List<Node>();

            start.PreviousNode = start;

            //set goal node
            start.GoalNode = end;

            //GCost = cost to get to current node
            //HCost = heuristic cost
            //FCost = final cost

            //Start, so GCost is zero
            start.GCost = 0;
            start.HCost = start.GetDistanceHeuristic();
            start.FCost = start.GCost + start.HCost;

            nodeList.Add(start);

            while (nodeList.Count > 0)
            {
                nodeList = nodeList.OrderBy(a => a.FCost).ToList();

                Node currentNode = nodeList.First<Node>();
                nodeList.Remove(nodeList.First<Node>());

                if (!currentNode.Visited)
                {
                    if (currentNode == end)
                    {
                        return true;
                    }

                    currentNode.Visited = true;

                    //Loop through all the edges, see which edge is the lowest cost to travel along
                    for (int i = 0; i < currentNode.edges.Count; ++i)
                    {
                        if (!currentNode.edges[i].end.Visited)
                        {
                            if (currentNode.PreviousNode != null)
                            {
                                float currentFCost = currentNode.GCost + currentNode.HCost;

                                if (currentFCost < currentNode.edges[i].end.FCost)
                                {
                                    currentNode.edges[i].end.PreviousNode = currentNode;
                                    currentNode.edges[i].end.GoalNode = end;
                                    currentNode.edges[i].end.GCost = currentNode.GCost + currentNode.edges[i].cost;
                                    currentNode.edges[i].end.HCost = currentNode.edges[i].end.GetDistanceHeuristic();
                                    currentNode.edges[i].end.FCost = currentNode.edges[i].end.GCost + currentNode.edges[i].end.HCost;

                                    if (!nodeList.Contains(currentNode.edges[i].end))
                                    {
                                        nodeList.Add(currentNode.edges[i].end);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return false;
        }

        public Node GetNearestNode(Vector2 a_Position)
        {
            foreach (Node n in nodes)
            {
                if (Vector2.Distance(n.Position, a_Position) < n.GetRectangle.Width * 2)
                {
                    return n;
                }
            }
            return null;
        }

        public List<Node> GetNodes(Node.NODE_TYPE a_Type)
        {
            List<Node> nodeOfType = new List<Node>();

            foreach (Node n in nodes)
            {
                if (n.GetNodeType == a_Type)
                {
                    nodeOfType.Add(n);
                }
            }

            return nodeOfType;
        }
    }
}
