using UnityEngine;
using Cinemachine;

// 카메라를 조작하는 스크립트
// 최초 작성자 : 이상도
// 수정자: 이상도
// 최종 수정일: 2025-04-29

public class CameraManager : MonoBehaviour
{
    ///////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////
    // Components
    ///////////////////////////////////////////////////////////////

    // virture camera
    public CinemachineVirtualCamera VC_Main;
    public CinemachineVirtualCamera VC_TrackBall;


    ///////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////
    // Camera function
    ///////////////////////////////////////////////////////////////


    // camera follow after hitting
    public void FollowBall(Transform ballTrans)
    {
        VC_TrackBall.LookAt = ballTrans;
        SwitchBallCam(true);
    }

    // trans ball camera
    // 높을 수록 우선순위가 높음
    // 높을 수록 카메라가 우선적으로 찍음
    public void SwitchBallCam(bool yes)
    {
        //switching priority
        //higher the number, higher the priority
        if (yes)
        {
            VC_TrackBall.Priority = 1;
            VC_Main.Priority = 0;
        }
        else
        {
            VC_TrackBall.Priority = 0;
            VC_Main.Priority = 1;
        }
    }
}
