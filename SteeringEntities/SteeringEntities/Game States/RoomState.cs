using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SteeringEntities
{
    public enum Direction
    {
        LEFT,
        RIGHT,
        UP,
        DOWN
    }

    public struct Passageway
    {
        public Room start;
        public Room end;
        public bool processed;

        public Passageway()
        {
            start = end = null;
            processed = false;
        }

        public Passageway(Room a_Start, Room a_End)
        {
            processed = false;
            start = a_Start;
            end = a_End;
        }
    }

    public class Room
    {
        Passageway[] connections;
        static int RoomID;
        int number;
        int numConnections;

        public Room()
        {
            connections = new Passageway[4];
            number = 0;
            numConnections = 0;
            number = RoomID;
            RoomID++;

            for (int i = 0; i < MaxConnections; ++i)
            {
                connections[i] = new Passageway(this, null);
            }
        }

        public Room GetRoom(Direction a_Direction)
        {
            return connections[(int)a_Direction].end;
        }

        public Passageway GetPassageway(Direction a_Direction)
        {
            return connections[(int)a_Direction];
        }

        public void SetPassageway(Passageway a_Passageway, Direction a_Direction)
        {
            connections[(int)a_Direction] = a_Passageway;
        }

        public int MaxConnections
        {
            get { return 4; }
        }
    }

    public class RoomState : GameState
    {
        

        public override void Update(GameTime gt)
        {
            base.Update(gt);
        }

        public override void Draw()
        {
            base.Draw();
        }
    }
}
