using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SteeringEntities
{
    public enum Status
    {
        INVALID,
        SUCCESS,
        FAILURE,
        RUNNING,
    }

    public abstract class BTBehaviour
    {
        protected abstract Status Update(Entity a_Entity, List<Entity> a_Entities);

        protected virtual void onInitialise() { }
        protected virtual void OnTerminate(Status a_Status) { }
        protected int m_Counter;

        private Status m_Status;
        private BTComposite m_Parent;

        public BTBehaviour()
        {
            m_Status = Status.INVALID;
            m_Counter = 0;
        }

        public Status Tick(Entity a_Entity, List<Entity> a_Entities)
        {
            if (m_Status == Status.INVALID)
            {
                onInitialise();
            }
            m_Status = Update(a_Entity, a_Entities);

            if (m_Status != Status.RUNNING)
            {
                OnTerminate(m_Status);
            }

            return m_Status;
        }

        public BTComposite End()
        {
            return m_Parent;
        }

        public void SetParen(BTComposite a_Parent)
        {
            m_Parent = a_Parent;
        }
    };

    public class BTComposite : BTBehaviour
    {
        protected List<BTBehaviour> m_Children;

        protected override Status Update(Entity a_Entity, List<Entity> a_Entities)
        {
            return Status.FAILURE;
        }

        public BTComposite Add(BTBehaviour a_Child)
        {
            a_Child.SetParen(this);

            m_Children.Add(a_Child);

            BTComposite compositeBehaviour = a_Child as BTComposite;

            return compositeBehaviour;
        }
    }

    //AND equivalent
    //visits children, returns on any fail
    public class BTSequence : BTComposite
    {
        private int currentIndex = 0;

        protected override void onInitialise()
        {
            currentIndex = 0;

            base.onInitialise();
        }

        protected override void OnTerminate(Status a_Status)
        {
            currentIndex = 0;

            base.OnTerminate(a_Status);
        }

        protected override Status Update(Entity a_Entity, List<Entity> a_Entities)
        {
            while (true)
            {
                Status currentStatus = m_Children[currentIndex].Tick(a_Entity, a_Entities);

                if (currentStatus != Status.SUCCESS)
                {
                    return currentStatus;
                }

                if (++currentIndex == m_Children.Count - 1)
                {
                    return Status.SUCCESS;
                }
            }
        }
    }

    //OR equivalent
    //visits children, returns on any success
    public class BTSelector : BTComposite
    {
        private int currentIndex = 0;

        protected override void onInitialise()
        {
            currentIndex = 0;

            base.onInitialise();
        }

        protected override void OnTerminate(Status a_Status)
        {
            currentIndex = 0;

            base.OnTerminate(a_Status);
        }

        protected override Status Update(Entity a_Entity, List<Entity> a_Entities)
        {
            while (true)
            {
                Status s = m_Children[currentIndex].Tick(a_Entity, a_Entities);

                if (s != Status.FAILURE)
                {
                    return s;
                }

                if (++currentIndex == m_Children.Count - 1)
                {
                    return Status.FAILURE;
                }
            }
        }
    }
}
