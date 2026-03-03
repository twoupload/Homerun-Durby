using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

// 메인 메뉴 입력에 관련된 스크립트
// 스크립터블 오브젝트 및 input action으로 효율적 운영
// 최초 작성자 : 이상도
// 수정자: 이상도
// 최종 수정일: 2025-05-03


[CreateAssetMenu(fileName = "MenuInputReader", menuName = "InputReader/Menu", order = 1)]
public class MenuInputReader : ScriptableObject, MenuInput.IMenuActions
{
    ///////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////
    // Components
    ///////////////////////////////////////////////////////////////
    public event UnityAction StartActions;  

    private MenuInput m_MenuInput;

    ///////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////
    // Unity Function
    ///////////////////////////////////////////////////////////////
    void OnEnable()
    {
        if (m_MenuInput == null)
        {
            m_MenuInput = new MenuInput();
            m_MenuInput.Menu.SetCallbacks(this);
        }
        m_MenuInput.Menu.Enable();
    }

    void OnDisable()
    {
        m_MenuInput.Menu.Disable();
    }

    ///////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////
    // starting menu function
    ///////////////////////////////////////////////////////////////
    public void OnStartGame(InputAction.CallbackContext context)
    {
        if(StartActions != null && context.canceled) 
        {
            StartActions.Invoke();    
        }
    }
}
