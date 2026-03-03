using UnityEngine;
using UnityEngine.UI;

// 결과 애니메이션에 관련한 스크립트
// 최초 작성자 : 이상도
// 수정자: 이상도
// 최종 수정일: 2025-05-20

namespace Result
{
    public class ResultAnimation : MonoBehaviour
    {
        ///////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////
        // component
        ///////////////////////////////////////////////////////////////

        public VoidEvent AtBatResetEvent;

        private Animation m_ResultAnim;
        private Text m_ResultText;


        ///////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////
        // unity function
        ///////////////////////////////////////////////////////////////

        private void Start()
        {
            m_ResultAnim = GetComponent<Animation>();
            m_ResultText = GetComponent<Text>();
            m_ResultText.enabled = false;
        }

        ///////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////
        // result function
        ///////////////////////////////////////////////////////////////

        // display
        public void DisplayResult(int state)
        {
            // play title hit result animation
            string anim_str = "GroundNFoulNStrikeUI"; // connect animation

            // change color and scale
            switch ((ResultState)state)
            {
                case ResultState.HR:
                    m_ResultText.text = "HOMERUN!!";
                    m_ResultText.color = Color.yellow;
                    anim_str = "HomeRunUI";
                    break;
                case ResultState.Foul:
                    m_ResultText.text = "FOUL";
                    m_ResultText.color = Color.green;
                    break;
                case ResultState.StrikeOut:
                    m_ResultText.text = "STRIKE";
                    m_ResultText.color = Color.red;
                    break;
                case ResultState.Ground:
                    m_ResultText.text = "GROUND BALL";
                    m_ResultText.color = Color.black;
                    break;
                default:
                    m_ResultText.text = "BALL";
                    m_ResultText.color = Color.blue;
                    break;
            }
            m_ResultText.enabled = true; // set
            m_ResultAnim.Play(anim_str); // play
        }

        // animation
        public void OnAnimationFinisehd(string str)
        {
            m_ResultText.enabled = false;
            AtBatResetEvent.Raise();
        }
    }
}
