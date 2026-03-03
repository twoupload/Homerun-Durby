using UnityEngine;

// 배팅 커서와 관련된 스크립트
// 최초 작성자 : 이상도
// 수정자: 이상도
// 최종 수정일: 2025-04-28


/**
동작 원리

초기화: 게임 시작 시, 필요한 컴포넌트와 참조를 설정
입력 감지: 사용자 입력(이동 및 스윙)을 감지하고 적절한 함수를 호출
커서 이동: 방향 입력에 따라 커서가 이동

회전과 공의 타격

커서 회전: 타격 지점과 기준점 사이의 각도에 따라 커서의 회전을 계산
충돌 처리: 스윙 시 일정 시간 동안 콜라이더를 활성화하고, 공과 충돌하면 공의 속도를 계산
=> 공의 속도와 회전에 따른 방향으로 홈런, 그라운드 볼, 헛스윙(미트)를 계산

이벤트

이벤트 처리: 스윙 및 공 충돌 시 적절한 이벤트를 발생시켜 다른 시스템에 알림
**/

namespace Batter
{
    public class BatCuror : BaseCursor
    {
        ///////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////
        // Components
        ///////////////////////////////////////////////////////////////

        // enum statement for batter cursor
        private enum CursorState { FOCUS, MEAT};

        // batter
        public BatterInputReader BatterInput;

        // swing 
        public VoidEvent SwingEvent;
        public Vector3Event BallExitVelEvent;

        // hitting point cursor
        public GameObject MeatCursor;

        // point for bat react
        public RectTransform PivotRectT;

        // cursor 
        private CursorState m_CurrentState;
        private float m_Distance;

        // hitting
        [SerializeField] private bool m_CheckCollision = false;
        [SerializeField] private bool m_AnimationFinisehd = true;
        private float m_SwingDelay = 0.01f;
        private float m_SwingDone = 0.5f;
        private float m_Timer = 0.0f;

        private CapsuleCollider m_Collision;

        private Vector3 BAT_CONTACT = Vector3.one;

        private Camera m_MainCamera;
        private bool m_MouseControlEnabled = true;

        ///////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////
        // Unity Function
        ///////////////////////////////////////////////////////////////

        public new void Awake()
        {
            base.Awake();
            m_MainCamera = Camera.main;
            m_Collision = GetComponent<CapsuleCollider>();
            m_Collision.enabled = false;
            m_Distance = m_Cursor.localPosition.x - PivotRectT.localPosition.x; // distance between fix point


            if (BatterInput != null)
            {
                var canvasRT = m_Cursor.GetComponentInParent<Canvas>().GetComponent<RectTransform>();
                BatterInput.Initialize(m_MainCamera, canvasRT);
            }
        }

        public new void Update()
        {

            if (m_Dir.magnitude > 0)
            {
                // moving cursor distance
                m_Cursor.localPosition += new Vector3(m_Dir.x, m_Dir.y) * Speed * Time.deltaTime;
            }

            base.Update();

            // distance amouont of change
            float dx = m_Cursor.localPosition.x - PivotRectT.localPosition.x;
            float dy = m_Cursor.localPosition.y - PivotRectT.localPosition.y;
            float angle = Mathf.Rad2Deg * Mathf.Atan(dy / dx);
            m_Cursor.localRotation = new Quaternion
            {
                eulerAngles = new Vector3(0, 0, angle)
            };


            // turning cursor
            if (m_CheckCollision)
            {
                m_Timer += Time.deltaTime;
                if (m_Timer > m_SwingDelay)
                {
                    m_Collision.enabled = true;
                }
                if (m_Timer > m_SwingDone)
                {
                    m_Collision.enabled = false;
                    m_CheckCollision = false;
                }
            }
        }

        // trigger system
        public void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Ball"))
            {
                if (SoundManager.Instance != null)
                {
                    SoundManager.Instance.PlaySound("hit");
                }
                Vector3 ballPos = other.transform.position;
                Vector3 delta = ballPos - transform.position;

                BallExitVelEvent.Raise(delta * 150);
            }
        }

        public void OnEnable()
        {
            BatterInput.SwingActions += OnSwing;
            BatterInput.MoveActions += MoveCursor;
        }

        public void OnDisable()
        {
            BatterInput.SwingActions -= OnSwing;
            BatterInput.MoveActions -= MoveCursor;

        }

        ///////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////
        // Unity Function
        ///////////////////////////////////////////////////////////////

        protected new void MoveCursor(Vector2 dir)
        {

            if (dir.magnitude <= 1.01f)
            {
                base.MoveCursor(dir);
            }
            else
            {
                Vector3 targetPosition = new Vector3(dir.x, dir.y, m_Cursor.localPosition.z);

                float lerpFactor = 0.1f;

                Vector3 newPosition = Vector3.Lerp(m_Cursor.localPosition, targetPosition, lerpFactor);
                m_Cursor.localPosition = newPosition;

                m_Dir = Vector2.zero;
            }
        }

        // animation about swing 
        public void OnSwingFinished()
        {
            m_AnimationFinisehd = true;
        }

        private void OnSwing()
        {
            if (!m_CheckCollision && m_AnimationFinisehd)
            {
                SwingEvent.Raise();

                m_CheckCollision = true;
                m_AnimationFinisehd = false;
                m_Timer = 0.0f;
            }
        }
    }
}