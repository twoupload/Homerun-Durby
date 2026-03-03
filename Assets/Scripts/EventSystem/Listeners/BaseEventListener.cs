using UnityEngine;
using UnityEngine.Events;

// 기초적인 이벤트를 수신하는 리스너와 관련된 스크립트
// 최초 작성자 : 이상도
// 수정자: 이상도
// 최종 수정일: 2025-05-03


public abstract class BaseEventListener<T, E, UER> : MonoBehaviour,

     ///////////////////////////////////////////////////////////////
     ///////////////////////////////////////////////////////////////
     //  event listener Components
     ///////////////////////////////////////////////////////////////
     IEventListener<T> where E : BaseEvent<T> where UER : UnityEvent<T>
{
    [SerializeField] private E m_GameEvent;
    public E GameEvent { get { return m_GameEvent; } set { m_GameEvent = value; } }

    [SerializeField] private UER m_UnityEventResponse;


    ///////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////
    // Unity Functon
    ///////////////////////////////////////////////////////////////
    private void OnEnable()
    {
        if(m_GameEvent == null) { return; }

        GameEvent.AddListner(this);
    }

    private void OnDisable()
    {
       if(m_GameEvent == null) { return; }

        GameEvent.RemoveListener(this);
    }

    ///////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////
    // react about event (listener)
    ///////////////////////////////////////////////////////////////
    public void OnEventRaised(T data)
    {
        if (m_UnityEventResponse != null)
        {
            m_UnityEventResponse.Invoke(data);
        }
    }
}

