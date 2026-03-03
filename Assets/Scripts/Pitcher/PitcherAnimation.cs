using UnityEngine;

// 투수의 애니메이션을 제어하는 스크립트
// 최초 작성자 : 이상도
// 수정자: 이상도
// 최종 수정일: 2025-05-19

public class PitcherAnimation : MonoBehaviour
{

    ///////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////
    // Components
    ///////////////////////////////////////////////////////////////
    public VoidEvent ThrowEvent;
    private enum State {IDLE,WINDUP,THROW,ROLLUP};
    private State m_State = State.IDLE;

    private Animator m_Animator;

    private float m_Delay = 0.0f;
    private float m_Time  = 0.0f;

    ///////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////
    // Unity Function
    ///////////////////////////////////////////////////////////////
    public void Awake()
    {
        m_Animator = GetComponent<Animator>();
    }

    public void Update()
    {
        switch (m_State)
        {
            case State.WINDUP:
                if (m_Time > m_Delay)
                {
                    m_Animator.SetTrigger("throw");
                    SetState(State.THROW, 0.0f);
                }
                m_Time += Time.deltaTime;
                break;
            case State.THROW:
            case State.IDLE:
            default:
                break;
        }
    }


    ///////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////
    // pitching and Release point Function
    ///////////////////////////////////////////////////////////////
    /*
     * Called when the throw animation reached certain keyframe (ie. reached throwing release point)
     *  On reach, this function will be called from ThrowRelease func exist in attached component(at least I think).
     */
    public void ThrowRelease()
    {
        Debug.Log("Reached release point");

        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlaySound("pitching", false, 1.0f);
        }

        ThrowEvent.Raise();
        SetState(State.IDLE, 0.0f);
    }

    // start throw
    public void StartThrow()
    {
        SetState(State.WINDUP,1.5f);
    }

    // set the throw statement
    private void SetState(State state, float delay)
    {
        m_State = state;
        m_Delay = delay;
        m_Time = 0f;
    }
}
