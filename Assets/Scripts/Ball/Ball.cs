using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Result;


// 공의 움직임을 제어하는 스크립트
// 최초 작성자 : 이상도
// 수정자: 이상도
// 최종 수정일: 2026-03-03

public class Ball : MonoBehaviour
{

    ///////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////
    // Components
    ///////////////////////////////////////////////////////////////
    public Vector3Event BallTravelEvent;
    public IntEvent BallFinishEvent;
    public TransformEvent BallHitEvent;
    public FloatEvent DistanceTrackerEvent;

    //public float SPEED_CONSTANT = 1000000f;
    public int PointCounts = 100;

    private Rigidbody m_RB; 
    private Bezier m_BallPath;

    private Vector3 m_HitPos;

    private float m_Time  = 0f;
    private int   m_Index = 0;
    private float m_Speed = 0.0f; 
    private bool  m_Start = false;
    private bool  m_IsHit = false;

    private const int NUM_CTRL_PTS = 4;
    private const int EXTRA_PTS = 4; 


    private float m_BallSpeed = 0f;


    private const float VERT_MULTIPLIER = 0.02f;
    private const float HORI_MULTIPLIER = 0.02f;

    private Result.ResultState m_ResultState = Result.ResultState.Ground;


    // Debug
    private Vector3 m_DebugDirection = Vector3.zero;
    private bool m_IsDebugHomeRun = false;
    private float m_HitTime = 0f;

    LineRenderer line;


    ///////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////
    // Unity Function
    ///////////////////////////////////////////////////////////////
    public void Awake()
    {
        m_Start = false;
        m_IsHit = false;
        m_RB = GetComponent<Rigidbody>();
    }

    public void Update()
    {
        if (!m_Start) return;

        // value of hit statement
        if (m_IsHit)
        {
            // cal distance feating actually ball position (debug mode)
            DistanceTrackerEvent.Raise((transform.position - m_HitPos).magnitude);


            float flightTime = Time.time - m_HitTime;

            if (DerbyManager.IsDebugHomeRunMode)
            {
                
                if (flightTime > 2.0f && m_RB.velocity.magnitude < 1.0f)
                {
                    m_ResultState = Result.ResultState.HR;
                    Destroy(gameObject);
                    return;
                }
            }
            else if (m_RB.velocity.magnitude < 0.55f)
            {
                m_ResultState = Result.ResultState.Ground;
                Destroy(gameObject);
            }

            return;
        }

        m_Time += Time.deltaTime;

        if (m_Index < m_BallPath.PathPoints.Count && m_Time > m_Speed)
        {
            transform.position = m_BallPath.PathPoints[m_Index++];
            BallTravelEvent.Raise(transform.position);
            m_Time = 0.0f; 
        }
        else if(m_Index >= m_BallPath.PathPoints.Count)
        {

            m_ResultState = Result.ResultState.StrikeOut;
            Destroy(gameObject);
        }
    }

    public void OnTriggerEnter(Collider col)
    {

        // 디버그 모드인 경우 홈런 바운드에만 반응하고 다른 충돌은 무시
        if (DerbyManager.IsDebugHomeRunMode)
        {
            if (col.CompareTag("HomeRunBound"))
            {
                // 홈런 바운드에 닿으면 홈런으로 설정하고 공 파괴
                m_ResultState = Result.ResultState.HR;

                // 홈런 환호 사운드
                if (SoundManager.Instance != null)
                {
                    SoundManager.Instance.PlaySound("homerun_impact_call", false, 1.0f);
                }
                Destroy(gameObject);
            }
            // 다른 모든 충돌은 무시 (계속 날아감)
            return;
        }

        // 기존 로직 (디버그 모드가 아닐 경우)
        if (col.CompareTag("OutOfBound"))
        {
            m_ResultState = Result.ResultState.Foul;

            if (SoundManager.Instance != null)
            {
                SoundManager.Instance.PlaySound("foul", false, 1.0f);
            }
        }
        else if (col.CompareTag("HomeRunBound"))
        {
            m_ResultState = Result.ResultState.HR;

            if (SoundManager.Instance != null)
            {
                SoundManager.Instance.PlaySound("homerun_impact_call", false, 1.0f);
            }
        }
        else
        {
            m_ResultState = Result.ResultState.Ground;

            if (SoundManager.Instance != null)
            {
                SoundManager.Instance.PlaySound("hit_call", false, 1.0f);
            }
            return;
        }
        Destroy(gameObject);
    }

    public void OnDestroy()
    {
        BallFinishEvent.Raise((int)m_ResultState);

        if (m_ResultState == Result.ResultState.StrikeOut)
        {
            if (SoundManager.Instance != null)
            {
                SoundManager.Instance.PlaySound("catching", false, 1.0f);
            }

            var derbyManager = FindObjectOfType<DerbyManager>();
            if (derbyManager != null)
            {
                derbyManager.PlayStrikeSound();
            }
        }
    }

    ///////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////
    // Getter Function
    ///////////////////////////////////////////////////////////////
    public bool IsHit()
    {
        return m_IsHit;
    }

    public ResultState GetResultState()
    {
        return m_ResultState;
    }
    

    ///////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////
    // Physical and Position Function
    ///////////////////////////////////////////////////////////////
    public void StartMove()
    {
        m_Start = true;
    }


    // init ball path
    /**
    public Vector3 InstantiateBallPath(Pitcher.PitchTypeSO selectedType, Vector3 releasePt, Vector3 targetPt)
    {
        // create bezier curve path
        m_BallPath = CreatePath(selectedType, releasePt, targetPt);

        // making path points (feat.breaking ball)
        PointCounts = m_BallPath.PathPoints.Count;

        // set up ball max speed
        m_BallSpeed = selectedType.MaxSpeed;

        // making ball speed
        // set up ball max speed
        m_BallSpeed = selectedType.MaxSpeed;

        
        float distance = (targetPt - releasePt).magnitude;


        float speedMs = selectedType.MaxSpeed;
        // MaxSpeed가 km/h면 이걸로:
        // float speedMs = selectedType.MaxSpeed / 3.6f;

        float duration = distance / Mathf.Max(0.1f, speedMs);

        speedMs *= selectedType.SpeedMultiplier;

        m_Speed = duration / Mathf.Max(1, (m_BallPath.PathPoints.Count - 1));

        return m_BallPath.PathPoints.Last();
    }
    **/

    public Vector3 InstantiateBallPath(Pitcher.PitchTypeSO selectedType, Vector3 releasePt, Vector3 targetPt)
    {
        // 1) create bezier curve path
        m_BallPath = CreatePath(selectedType, releasePt, targetPt);
        PointCounts = m_BallPath.PathPoints.Count;

        // 2) set up ball max speed
        m_BallSpeed = selectedType.MaxSpeed;

        // 3) speed (duration-based)
        float distance = (targetPt - releasePt).magnitude;

        // MaxSpeed 단위가 km/h라면 아래 줄로 바꾸는 걸 고려
        float speedMs = selectedType.MaxSpeed;
        // float speedMs = selectedType.MaxSpeed / 3.6f;

        // 방법 A: 구종별 속도 배율
        speedMs *= selectedType.SpeedMultiplier;

        float duration = distance / Mathf.Max(0.1f, speedMs);
        m_Speed = duration / Mathf.Max(1, (m_BallPath.PathPoints.Count - 1));

        // 4) debug log (여기가 위치!)
#if UNITY_EDITOR
    Debug.Log(
        $"[PITCH] type={selectedType.name} " +
        $"maxSpeed={selectedType.MaxSpeed:F1} speedMul={selectedType.SpeedMultiplier:F2} " +
        $"curve={selectedType.CurveOffset:F2} drop={selectedType.DropOffset:F2} breakMul={selectedType.BreakMultiplier:F2} " +
        $"points={m_BallPath.PathPoints.Count} stepTime={m_Speed:F4}s " +
        $"release={releasePt} target={targetPt}"
    );
#endif

        // 5) return last ball path
        return m_BallPath.PathPoints.Last();
    }

    // init bezier path
    public static Bezier CreatePath(Pitcher.PitchTypeSO type, Vector3 releasePt, Vector3 targetPt)
    {
        Vector3 dir = targetPt - releasePt;
        float distance = dir.magnitude;
        Vector3 forward = dir.normalized;

        // 투구 방향 기준 right/up (월드축 의존 제거)
        Vector3 right = Vector3.Cross(Vector3.up, forward).normalized;
        Vector3 up = Vector3.Cross(forward, right).normalized;

        // ⚠️ 여기 값이 “각”을 결정함 (처음엔 과감히 크게 잡고 줄이는 게 빠름)
        // type.CurveOffset / DropOffset가 어떤 단위인지 모르니 스케일로 조절
        float curve = -type.CurveOffset * 0.02f * type.BreakMultiplier;
        float drop = -type.DropOffset * 0.02f * type.BreakMultiplier;

        Vector3 p0 = releasePt;
        Vector3 p3 = targetPt;

        // “막판에 확 꺾이게” 만들기: p2를 강하게
        Vector3 p1 = releasePt + forward * (distance * 0.33f) + right * (curve * 0.2f) + up * (drop * 0.2f);
        Vector3 p2 = releasePt + forward * (distance * 0.66f) + right * (curve * 1.2f) + up * (drop * 1.2f);

        return new Bezier(new List<Vector3> { p0, p1, p2, p3 });
    }

    ///////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////
    // Collision And Hitting Function
    ///////////////////////////////////////////////////////////////
    public void OnHit(Vector3 vel)
    {
        m_IsHit = true;
        m_HitTime = Time.time;

        if (m_RB == null)
        {
            Debug.Log("m_RB is undefined for some reason");
        }
        else
        {

            m_HitPos = transform.position;

            // checking the debug mode
            if (DerbyManager.IsDebugHomeRunMode)
            {
                m_IsDebugHomeRun = true;

                float angle = Random.Range(30f, 50f) * Mathf.Deg2Rad;
                float horizontalAngle = Random.Range(-30f, 30f) * Mathf.Deg2Rad;

                float power = DerbyManager.DebugBallSpeedValue;

                Vector3 direction = new Vector3(
                    Mathf.Sin(horizontalAngle),
                    Mathf.Sin(angle),
                    Mathf.Abs(Mathf.Cos(angle) * Mathf.Cos(horizontalAngle)) * -1
                ).normalized;

                vel = direction * power;

                m_DebugDirection = direction;

                Debug.Log("디버그 모드: 홈런 발사! 각도: " + (angle * Mathf.Rad2Deg) + "도");
            }

            else
            {
                
                float distanceMultiplier = 1.5f; 
                vel *= distanceMultiplier;
            }

            m_RB.useGravity = true;
            m_RB.velocity = vel;
            BallHitEvent.Raise(transform);
        }
    }

    public float GetBallSpeed()
    {
        return m_BallSpeed;
    }
}

