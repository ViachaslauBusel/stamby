using System.Collections.Generic;


namespace AI
{
    [System.Serializable]
    public abstract class BaseState
    {
        protected AIComponent m_master;

        public BaseState(AIComponent master)
        {
            m_master = master;
        }

        public void Reset()
        {
            ResetState();
        }

        public bool Update(float deltaTime, out State state)
        {
            return UpdateState(deltaTime, out state);
        }

        /// <summary>
        /// Reset the script to the initial state, called after selecting this state
        /// </summary>
        protected abstract void ResetState();

        /// <summary>
        /// Update script state
        /// </summary>
        /// <param name="deltaTime">Time elapsed since the last update was performed</param>
        /// <param name="state">State change variable</param>
        /// <returns></returns>
        protected abstract bool UpdateState(float deltaTime, out State state);

        /// <summary>
        /// Called if the state has been changed from attached scripts, used to cancel this state
        /// </summary>
        protected abstract void Abort();
    }
}