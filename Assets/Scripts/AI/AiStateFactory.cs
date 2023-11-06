using AI.Stats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace AI
{
    public class AiStateFactory
    {
        private DiContainer m_container;

        [Inject]
        private void Construct(DiContainer container) 
        {
            m_container = container;
        }

        public BaseState CreateState(AIComponent component, State state)
        {
            BaseState baseState = state switch
            {
                State.Waiting => new IdleState(component),
                State.Walk => new WalkState(component),
                _ => null
            };

            if(baseState == null) 
            {
                Debug.LogError($"Failed to create state behavior");
                return baseState;
            }

            m_container.Inject(baseState);

            return baseState;
        }
    }
}
