using System.Collections.Generic;
using UnityEngine;

// 커서의 움직임을 제어하는 스크립트
// 최초 작성자 : 이상도
// 수정자: 이상도
// 최종 수정일: 2025-04-28

public class BaseCursor : MonoBehaviour
{
    ////////////////////////////////////////////
    // Component
    ////////////////////////////////////////////
    
    public List<Vector3> PathPoints;
    private int m_Segments;
    private const int m_TotalPoints = 100;
    public float OutBoundLimit = 0f;
    public RectTransform BorderRectT;

    public float Speed = 500f;

    // direction 
    protected RectTransform m_Cursor;
    protected Vector2 m_Dir;
    protected Vector3 m_CenterPosition;


    // box boundry
    private Vector2 m_xBoundry;
    private Vector2 m_yBoundry;

    // out boundry
    private Vector2 m_xOutBoundry;
    private Vector2 m_yOutBoundry;


    ///////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////
    // Unity Function
    ///////////////////////////////////////////////////////////////
    public void Awake()
    {
        m_Cursor = GetComponent<RectTransform>();

        // set up cur's origin position
        m_CenterPosition = m_Cursor.localPosition;

        // cal boundry
        Vector2 halfsize = BorderRectT.sizeDelta / 2;
        m_xBoundry = new Vector2(BorderRectT.transform.localPosition.x - halfsize.x, BorderRectT.transform.localPosition.x + halfsize.x);
        m_yBoundry = new Vector2(BorderRectT.transform.localPosition.y - halfsize.y, BorderRectT.transform.localPosition.y + halfsize.y);


        m_xOutBoundry = new Vector2(m_xBoundry.x - OutBoundLimit, m_xBoundry.y + OutBoundLimit);
        m_yOutBoundry = new Vector2(m_yBoundry.x - OutBoundLimit, m_yBoundry.y + OutBoundLimit);
    }

    public void Update()
    {
        m_Cursor.localPosition += new Vector3(m_Dir.x, m_Dir.y) * Speed * Time.deltaTime;
        Vector2 local = m_Cursor.localPosition;
        local.x = Mathf.Clamp(local.x, m_xOutBoundry.x, m_xOutBoundry.y);
        local.y = Mathf.Clamp(local.y, m_yOutBoundry.x, m_yOutBoundry.y);
        m_Cursor.localPosition = local;
    }


    ///////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////
    // Cursor function
    ///////////////////////////////////////////////////////////////
    protected void MoveCursor(Vector2 dir)
    {
        m_Dir = dir;
    }


    // check inside boundry
    protected bool InsideTheBound()
    {
        Vector2 pos = m_Cursor.localPosition;
        return (pos.x >= m_xBoundry.x && pos.x <= m_xBoundry.y && pos.y >= m_yBoundry.x && pos.y <= m_yBoundry.y);
    }
}
