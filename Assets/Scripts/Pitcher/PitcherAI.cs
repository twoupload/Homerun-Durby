using UnityEngine;
using UnityEngine.Events;

// AI 투수의 행동을 제어하는 스크립트
// AI 투수의 구종 선택, 확정, 커서 움직임을 제어한다.
// 최초 작성자 : 이상도
// 수정자: 이상도
// 최종 수정일: 2026-03-03

namespace Pitcher {
    public class PitcherAI : MonoBehaviour
    {
        ///////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////
        // Components - about event Action
        ///////////////////////////////////////////////////////////////
        public UnityAction<Vector2> PitchSelectActions;
        public UnityAction          PitchConfirmActions;
        public UnityAction<Vector2> PitchCursorActions;

        // checking box for only breaking ball
        [SerializeField] private bool onlyBreakingBalls = false;

        // management AI statement
        enum State { IDLE, PICKING, THROWING, CLOSING };

        private State m_CurrentState = State.IDLE;
        [SerializeField] private float m_WaitTime = 5.0f;
        private float m_Time = 0.0f;
        private bool isReady;

        // Update is called once per frame
        void Update()
        {
            /*
             투수 제어 방법
            1. 게임으로부터 준비 신호 대기
            2. 무작위로 구종 선택 (가중치 적용)
            3. 투구 위치 선택
             */
            switch (m_CurrentState)
            {
                case State.IDLE:
                    if (m_Time > m_WaitTime)
                    {
                        m_CurrentState = State.PICKING;
                        m_Time = 0.0f;
                    }
                    else
                    {
                        m_Time += Time.deltaTime;
                    }
                    break;
                case State.PICKING:
                    // 구종 선택 및 확정
                    SelectNConfirm();
                    m_CurrentState = State.THROWING;
                    break;
                case State.THROWING:
                    // 커서 이동
                    CursorMove();
                    break;
                case State.CLOSING:
                    //wait for the response from the other part of the game for pitch to start to throw

                    //TEMP - just back to idle for now
                    m_CurrentState = State.IDLE;
                    break;
            }
        }

        ///////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////
        // call the function when finished the pitch
        ///////////////////////////////////////////////////////////////
        public void Throwed()
        {
            m_CurrentState = State.CLOSING;
        }

        ///////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////
        // select the breaking ball Function
        ///////////////////////////////////////////////////////////////
        ///

        private static readonly Vector2[] MixedPitchCandidates = new Vector2[]
        {
            new Vector2(0, 1),   // CENTER (직구로 사용한다고 가정)
            new Vector2(-1, 0),  // LEFT
            new Vector2(1, 0),   // RIGHT
            new Vector2(0, -1),  // DOWN
            new Vector2(-1, -1), // LOWERLEFT
            new Vector2(1, -1),  // LOWERRIGHT
            new Vector2(0, 0),   // SLOW (체인지업/느린 공 등으로 사용 가능)
        };

        private static readonly Vector2[] BreakingOnlyCandidates = new Vector2[]
        {
            new Vector2(-1, 0),  // LEFT
            new Vector2(1, 0),   // RIGHT
            new Vector2(0, -1),  // DOWN
            new Vector2(-1, -1), // LOWERLEFT
            new Vector2(1, -1),  // LOWERRIGHT
            new Vector2(0, 0),   // SLOW
        };

        private void SelectNConfirm()
        {
            Vector2[] pool = onlyBreakingBalls ? BreakingOnlyCandidates : MixedPitchCandidates;
            Vector2 selected = pool[Random.Range(0, pool.Length)];

            if (PitchConfirmActions != null && PitchSelectActions != null)
            {
                PitchSelectActions.Invoke(selected);
                PitchConfirmActions.Invoke();
            }
        }

        ///////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////
        // move the cursor function
        ///////////////////////////////////////////////////////////////
        private void CursorMove()
        {
            if(PitchCursorActions != null)
            {
                Vector2 dir = new Vector2(Random.value* 2 - 1, Random.value * 2 - 1 );
                PitchCursorActions.Invoke(dir);
            }
        }
    }

}