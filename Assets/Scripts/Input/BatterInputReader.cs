using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

// 타자의 입력에 관련된 스크립트
// 스크립터블 오브젝트 및 input action으로 효율적 운영
// 최초 작성자 : 이상도
// 수정자: 이상도
// 최종 수정일: 2025-05-03


[CreateAssetMenu(fileName = "New Battter Input", menuName = "InputReader/Batter")]
public class BatterInputReader : ScriptableObject, GameInput.IBatterActions
{
    ///////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////
    // Action Components
    ///////////////////////////////////////////////////////////////
    public event UnityAction<Vector2> MoveActions;
    public event UnityAction SwingActions;

    ///////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////
    // game input 
    ///////////////////////////////////////////////////////////////
    private GameInput m_GameInput;

    ///////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////
    // Camera Components
    ///////////////////////////////////////////////////////////////
    private Camera m_MainCamera;
    private RectTransform m_CanvasRectTransform;

    ///////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////
    // mouse position components
    ///////////////////////////////////////////////////////////////
    private Vector2 m_PreviousMousePosition;

    ///////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////
    // Unity Function
    ///////////////////////////////////////////////////////////////
    public void Initialize(Camera camera, RectTransform canvasRect)
    {
        m_MainCamera = camera;
        m_CanvasRectTransform = canvasRect;
        m_PreviousMousePosition = Mouse.current.position.ReadValue();
    }

    public void OnEnable()
    {
        if (m_GameInput == null)
        {
            m_GameInput = new GameInput();
            m_GameInput.Batter.SetCallbacks(this);
        }
        m_GameInput.Batter.Enable();
    }

    public void OnDisable()
    {
        m_GameInput.Batter.Disable();
    }

    ///////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////
    // cursor function
    ///////////////////////////////////////////////////////////////

    public void OnMousePosition(InputAction.CallbackContext context)
    {
        if (MoveActions == null || m_MainCamera == null) return;

        Vector2 mousePosition = context.ReadValue<Vector2>();

        if (m_CanvasRectTransform != null)
        {

            Vector2 localPoint;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                m_CanvasRectTransform,
                mousePosition,
                m_MainCamera,
                out localPoint))
            {

                MoveActions.Invoke(localPoint);
            }
        }
        m_PreviousMousePosition = mousePosition;
    }

    ///////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////
    // Hitting function
    ///////////////////////////////////////////////////////////////
    public void OnMouseClick(InputAction.CallbackContext context)
    {
        if (context.performed && SwingActions != null)
        {
            SwingActions.Invoke();
        }
    }

}