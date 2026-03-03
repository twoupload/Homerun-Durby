using UnityEngine;

// 타자의 애니메이션을 제어하는 스크립트
// 최초 작성자 : 이상도
// 수정자: 이상도
// 최종 수정일: 2025-04-28

public class BatterAnimation : MonoBehaviour
{
    ////////////////////////////////////////////
    // Component
    ////////////////////////////////////////////

    // Animator 
    public Animator BatterAnimator;

    // animation for end swing
    public VoidEvent SwingFinishedEvent;

    // boolean swing 
    private bool isSwing = false;


    ///////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////
    // Animation Function
    ///////////////////////////////////////////////////////////////

    // set up swing 
    public void EnableSwing()
    {
        isSwing = false;
    }

    // connect swing animation
    public void Swing()
    {
        if (!isSwing)
        {
            BatterAnimator.SetTrigger("swing");
            isSwing = true;
        }
    }

    // end swing animation
    public void SwingFinished()
    {
        SwingFinishedEvent.Raise();
    }
}
