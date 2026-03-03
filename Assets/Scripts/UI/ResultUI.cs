using System.Collections;
using UnityEngine;
using UnityEngine.UI;


// 투구 결과와 구속을 UI에 표시하는 스크립트
// 최초 작성자 : 이상도
// 수정자: 이상도
// 최종 수정일: 2025-05-04

public class ResultUI : MonoBehaviour
{
    ///////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////
    // Components
    ///////////////////////////////////////////////////////////////
    public VoidEvent ResetEvent; //TODO - will display speed/ball type later so have to move this later

    // text component that show velocity
    public Text m_SpeedText;

    private Text m_DisplayText;
    private float m_DisplayTime = 1.5f;

    // current ball velocity
    private float m_CurrentBallSpeed = 0f;

    ///////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////
    // Unity Function
    ///////////////////////////////////////////////////////////////
    public void Start()
    {
        m_DisplayText = GetComponent<Text>();
        m_DisplayText.enabled = false;

        // unabled velocity text
        if (m_SpeedText != null)
        {
            m_SpeedText.enabled = false;
        }
    }

    ///////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////
    // velocity function
    ///////////////////////////////////////////////////////////////
    
    // set the velocity
    public void SetBallSpeed(float speed)
    {
        m_CurrentBallSpeed = speed;

        Debug.Log("구속 설정: " + speed + " km/h");

        if (m_SpeedText == null)
        {
            Debug.LogError("SpeedText가 null입니다! UI 요소를 할당해주세요.");
            return;
        }

        m_SpeedText.text = Mathf.RoundToInt(speed) + " km/h";
        m_SpeedText.enabled = true;
        Debug.Log("구속 UI 설정 완료: " + m_SpeedText.text);
    }

    // show the velocity function
    private void DisplayBallSpeed()
    {
        if (m_SpeedText == null)
        {
            Debug.LogError("SpeedText가 null입니다! ShowResult에서 구속을 표시할 수 없습니다.");
            return;
        }

        m_SpeedText.text = Mathf.RoundToInt(m_CurrentBallSpeed) + " km/h";
        m_SpeedText.enabled = true;
        Debug.Log("구속 표시됨: " + m_SpeedText.text);
    }


    ///////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////
    // event couroutain function
    ///////////////////////////////////////////////////////////////
    private IEnumerator Show()
    {
        yield return new WaitForSeconds(m_DisplayTime);
        m_DisplayText.enabled = false;

        if (m_SpeedText != null)
        {
            m_SpeedText.enabled = false;
        }

        ResetEvent.Raise();
    }



}
