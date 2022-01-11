using Pathfinder;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkState : BaseState
{
  //  private Node m_destinationNode;

    public WalkState(AIComponent master): base(master)
    {
      //  m_attachedStates.Add(new AliveState(master));
      //  m_attachedStates.Add(new AggressionState(master));
    }

    protected override void Abort()
    {
        // Transform transform = m_master.Transform;
        //  transform.Movement.Stop();
    }

    protected override void ResetState()
    {

        //   Vector3 _point = Vector3.zero;
        Node findNode = null;
        int maxCicle = 10;

            do
            {
            Vector3 direction = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), 0.0f).normalized;
            float distance = Random.Range(5.0f, 10.0f);
            Vector3 point = m_master.transform.position + direction * distance;
            findNode = Path.Find(m_master.transform.position, point);
            } while (findNode?.Count() <= 1 && maxCicle-- >= 0);

        m_master.Controller.SetTargetNode(findNode);
    }

    protected override bool UpdateState(float deltaTime, out State state)
    {
        if (!m_master.Controller.Moving)
        {
            state = State.Waiting;
            return true;
        }

        state = State.None;
        return false;
    }

   
}
