using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SteeringEntities
{
    //to link two nodes together
    public class Edge
    {
        public Node start;
        public Node end;

        //the cost to travel along edge
        public float cost;

        //draw visualisation of edge
        public void Draw(SpriteBatch sb, SpriteFont sf, Texture2D t, bool drawEdgeCost = false)
        {
            float xHalf = (end.Position.X + start.Position.X) * 0.5f;
            float yHalf = (end.Position.Y + start.Position.Y) * 0.5f;

            //start and end have same Y value, go long on X
            //start and end have same X value, go long on Y
            sb.Draw(t, new Rectangle((int)xHalf, (int)yHalf, end.Position.Y == start.Position.Y ? 7 : 2, end.Position.X == start.Position.X ? 7 : 2), Color.Black);

            if (drawEdgeCost)
            {
                sb.DrawString(sf, ((int)cost).ToString(), new Vector2(xHalf, yHalf), Color.Yellow);
            }
        }
    }
}
