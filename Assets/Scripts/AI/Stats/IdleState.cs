using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI.Stats
{
    public class IdleState : BaseState
    {
        private float waitTime = 0;

        public IdleState(AIComponent master) : base(master)
        {
        }

        protected override void Abort()
        {
        }

        protected override void ResetState()
        {
            waitTime = Random.Range(1.0f, 3.0f);
        }

        protected override bool UpdateState(float deltaTime, out State state)
        {
            waitTime -= deltaTime;
            if (waitTime <= 0)
            {
                state = State.Walk;
                return true;
            }
            state = State.None;
            return false;
        }
    }
}