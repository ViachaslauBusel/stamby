using GameField;
using Pathfinder;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace AI.Stats
{
    public class WalkState : BaseState
    {
        private GameGrid m_grid;


        [Inject]
        private void Construct(GameGrid grid)
        {
            m_grid = grid;
        }

        public WalkState(AIComponent master) : base(master)
        {
        }

        protected override void Abort()
        {
        }

        protected override void ResetState()
        {
            Node findNode = null;
            int maxCicle = 10;

            do
            {
                Vector3 direction = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), 0.0f).normalized;
                float distance = Random.Range(5.0f, 10.0f);
                Vector3 point = m_master.transform.position + direction * distance;
                findNode = m_grid.Find(m_master.transform.position, point);
            } while (findNode?.Count() <= 1 && maxCicle-- >= 0);

            m_master.Controller.MoveTo(findNode);
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
}