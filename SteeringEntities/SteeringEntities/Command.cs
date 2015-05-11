using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SteeringEntities
{
    public abstract class Command
    {
        public abstract void Execute(MovableEntity a_Entity);
    }

    public class MoveLeftCommand : Command
    {
        public override void Execute(MovableEntity a_Entity)
        {
            a_Entity.MoveLeft();
        }
    }

    public class MoveRightCommand : Command
    {
        public override void Execute(MovableEntity a_Entity)
        {
            a_Entity.MoveRight();
        }
    }

    public class MoveUpCommand : Command
    {
        public override void Execute(MovableEntity a_Entity)
        {
            a_Entity.MoveUp();
        }
    }

    public class MoveDownCommand : Command
    {
        public override void Execute(MovableEntity a_Entity)
        {
            a_Entity.MoveDown();
        }
    }

    public class ShootCommand : Command
    {
        public override void Execute(MovableEntity a_Entity)
        {
            a_Entity.Shoot();
        }
    }
}
