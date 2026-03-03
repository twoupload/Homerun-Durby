using UnityEngine;
using UnityEngine.SceneManagement;

// 메뉴 버튼 입력을 처리하는 스크립트
// 최초 작성자 : 이상도
// 수정자: 이상도
// 최종 수정일: 2025-05-19

public class MenuControl : MonoBehaviour
{
    ///////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////
    // Components
    ///////////////////////////////////////////////////////////////
    public MenuInputReader InputReader;

    ///////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////
    // Unity Function
    ///////////////////////////////////////////////////////////////
    private void OnEnable()
    {
        InputReader.StartActions += OnStartGame;
    }

    private void OnDisable()
    {
        InputReader.StartActions -= OnStartGame;
    }

    ///////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////
    // Game Start Scene Function
    ///////////////////////////////////////////////////////////////
    ///
    private void OnStartGame()
    {
        Debug.Log("OnStartGame 호출됨");

        if (SoundManager.Instance != null)
        {
            Debug.Log("SoundManager.Instance 존재함");
            SoundManager.Instance.PlaySound("playball", false, 1.0f);
        }
        else
        {
            Debug.LogError("SoundManager.Instance가 null입니다!");
        }

        Invoke("StartGame", 0.1f);
    }

    private void StartGame()
    {
        InputReader.StartActions -= OnStartGame; //remove the function from the action incase of double tap to avoid calling multiple time
        SceneManager.LoadScene("DerbyScene",LoadSceneMode.Single);
    }
}
