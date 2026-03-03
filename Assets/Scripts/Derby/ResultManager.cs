using UnityEngine;
using UnityEngine.UI;

// 결과 매니저 스크립트
// 최초 작성자 : 이상도
// 수정자: 이상도
// 최종 수정일: 2025-05-19
namespace Result
{
    // enum about result
    public enum ResultState { StrikeOut, Ground, Foul, HR };
    
    public class ResultManager : MonoBehaviour
    {
        ///////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////
        // Components
        ///////////////////////////////////////////////////////////////
        
        // text 
        public Text ScoreText;
        public Text TempDistanceText;
        public Text HRCountText;

        // count
        private uint m_HRCount = 0;
        private int m_Score = 0;
        private int m_Distance = 0;

        // state
        private ResultState m_CurrentResult = ResultState.Ground;

        // boolean strike and ball
        private bool isStrike = false;


        ///////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////
        // unity function
        ///////////////////////////////////////////////////////////////
        
        public void Start()
        {
            ScoreText.text = "" + m_Score;
            HRCountText.text = "" + m_HRCount;
        }


        ///////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////
        // count hit state function
        ///////////////////////////////////////////////////////////////
        public void UpdateHR(int result)
        {
            m_CurrentResult = (ResultState)result;

            if (m_CurrentResult == ResultState.HR)
            {
                HRCountText.text = "" + ++m_HRCount;

                m_Score += m_Distance * 2;
            }
            else if (m_CurrentResult == ResultState.Ground)
            {
                m_Score += m_Distance;
            }

            ScoreText.text = "" + m_Score;
        }

        ///////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////
        // Determine strike and ball function
        ///////////////////////////////////////////////////////////////

        public void DetermineStrikeOrBall(bool inStrikeZone)
        {
            isStrike = inStrikeZone;

            if (isStrike)
            {
                m_CurrentResult = ResultState.StrikeOut;
            }
            else
            {
                //m_CurrentResult = ResultState.Ball;
            }
        }

        ///////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////
        // Result function
        ///////////////////////////////////////////////////////////////
        public ResultState GetCurrentResult()
        {
            return m_CurrentResult;
        }

        ///////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////
        // distance function
        ///////////////////////////////////////////////////////////////

        public void UpdateDistance(float distance)
        {
            m_Distance = Mathf.RoundToInt(distance);
            TempDistanceText.text = m_Distance + "m";
        }

        ///////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////
        // reset function
        ///////////////////////////////////////////////////////////////
        public void Reset()
        {
            m_HRCount = 0;
            m_Score = 0;
            ScoreText.text = "" + m_Score;
        }
    }
}
