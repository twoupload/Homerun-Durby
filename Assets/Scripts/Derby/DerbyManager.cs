using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Result;

// 홈런 더비 제어와 관련된 스크립트
// 최초 작성자 : 이상도
// 수정자: 이상도
// 최종 수정일: 2025-05-19

public class DerbyManager : MonoBehaviour
{
    ///////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////
    // Components
    ///////////////////////////////////////////////////////////////
    
    public Text RemainCountText;
    public VoidEvent GameOverEvent;
    public MenuInputReader InputReader;

    private static bool s_HasPlayedEntranceMusic = false;

    // life count
    private int m_Count = 10;

    // Debug Mode
    [Header("디버그 설정")]
    [Tooltip("활성화하면 모든 타구가 홈런이 된다.")]
    public bool DebugHR = false;

    [Tooltip("디버그 모드에서 홈런의 비거리 (미터)")]
    public int DebugHomeRunDistance = 120;

    [Tooltip("디버그 모드에서 사용할 공의 속도")]
    public float DebugBallSpeed = 40f;

    // set the debug bool button
    public static bool IsDebugHomeRunMode = false;
    public static int DebugDistance = 120;
    public static float DebugBallSpeedValue = 25f;

    ///////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////
    // Unity Function
    ///////////////////////////////////////////////////////////////
    public void Awake()
    {
        IsDebugHomeRunMode = DebugHR;
        DebugDistance = DebugHomeRunDistance;
        DebugBallSpeedValue = DebugBallSpeed;
    }

    public void Start()
    {
        if (!s_HasPlayedEntranceMusic)
        {
            if (SoundManager.Instance != null)
            {
                SoundManager.Instance.PlaySound("appear", false, 1.0f);
                s_HasPlayedEntranceMusic = true;
            }
        }

        if (RemainCountText != null)
        {
            RemainCountText.text = "" + m_Count;
        }
    }

    ///////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////
    // text sound and game over and restart logic function
    ///////////////////////////////////////////////////////////////
    public void DecrementCount(bool isStrike = true)
    {
        if (isStrike)
        {
            --m_Count;
            RemainCountText.text = "" + m_Count;

            if (m_Count == 0)
            {
                //GameOver
                //Display Result
                GameOverEvent.Raise();
                InputReader.StartActions += Restart;
            }
        }
    }
    public void PlayStrikeSound()
    {
        if (SoundManager.Instance != null)
        {
            int randomSound = Random.Range(1, 4);
            string soundName = "s" + randomSound;
            SoundManager.Instance.PlaySound(soundName);
        }
    }

    private void Restart()
    {
        InputReader.StartActions -= Restart;
        SceneManager.LoadScene("DerbyScene",LoadSceneMode.Single); // change scene
    }

    ///////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////
    // Debug toggle fundtion
    ///////////////////////////////////////////////////////////////
    public void ToggleDebugHomeRun()
    {
        DebugHR = !DebugHR;
        IsDebugHomeRunMode = DebugHR;
        Debug.Log("홈런 디버그 모드: " + (DebugHR ? "활성화" : "비활성화"));
    }
}
