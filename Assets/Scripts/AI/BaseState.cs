using System.Collections.Generic;

[System.Serializable]
public abstract class BaseState
{
    // protected List<AttachState> m_attachedStates { get; } = new List<AttachState>();
    protected AIComponent m_master;
    public BaseState(AIComponent master)
    {
        m_master = master;
    }
    public void Reset()
    {
     //   foreach (AttachState _state in m_attachedStates) _state.ResetState();
        ResetState();
    }

    public bool Update(float deltaTime, out State state)
    {
      /*  foreach (AttachState _state in m_attachedStates)
        {
            if (_state.UpdateState(deltaTime, out state))
            {
                Abort();
                return true;
            }
        }*/
        return UpdateState(deltaTime, out state);
    }
    /// <summary>
    /// Сброс скрипта в начальное состояния, вызываеться после выбора этого состояния
    /// </summary>
    protected abstract void ResetState();
    /// <summary>
    /// Обновление состояния скрипта
    /// </summary>
    /// <param name="deltaTime">Время прошедшее с момента выполнения последнего обновление</param>
    /// <param name="state">Переменная для смены состояния</param>
    /// <returns></returns>
    protected abstract bool UpdateState(float deltaTime, out State state);
    /// <summary>
    /// Вызывается если состояние было изменено из прикрепляемых скриптов, используется для отмены этого состояния
    /// </summary>
    protected abstract void Abort();
}