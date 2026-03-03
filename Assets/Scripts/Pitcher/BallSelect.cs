using System.Collections.Generic;
using UnityEngine;

// 투수가 던질 구종을 선택하는 스크립트
// 최초 작성자 : 이상도
// 수정자: 이상도
// 최종 수정일: 2025-05-04

namespace Pitcher
{
    public class BallSelect : MonoBehaviour
    {
        ///////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////
        // Components - AI and pitcher data info
        ///////////////////////////////////////////////////////////////

        private bool m_IsAI = true;

        // AI mode
        public PitcherAI AIReader;

        // AI pitcher input component
        public PitcherDataSO PitcherData;

        // pitcher data scriptableObject and pitch type
        private Dictionary<BallDir, GameObject>  m_PitchArrowUIDict = new Dictionary<BallDir, GameObject>();
        private Dictionary<BallDir, PitchTypeSO> m_PitchTypeDict    = new Dictionary<BallDir, PitchTypeSO>();

        private BallDir m_SelectedBallDir;

        public PitchConfirmEvent _OnConfirmPitchEvent;

        ///////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////
        // Unity Function
        ///////////////////////////////////////////////////////////////
        public void OnEnable()
        {
            m_IsAI = true;
            if (m_IsAI)
            {
                AIReader.PitchSelectActions += Select;
                AIReader.PitchConfirmActions += Confirm;
            }
        }

        public void OnDisable()
        {
            AIReader.PitchSelectActions -= Select;
            AIReader.PitchConfirmActions -= Confirm;
        }

        public void Override(bool isAI) { m_IsAI = isAI; }

        public void Start()
        {
            for (int i = 0; i < transform.childCount; ++i)
            {
                GameObject arrowUI = transform.GetChild(i).gameObject;
                ArrowDirection arrowDir = arrowUI.GetComponent<ArrowDirection>();

                arrowUI.SetActive(false); 
   

                
                foreach (PitchTypeSO pitchType in PitcherData.PitchTypes)
                {

                    if (arrowDir.Dir == pitchType.Dir)
                    {

                        arrowUI.SetActive(true);

                        if(!m_PitchTypeDict.ContainsKey(arrowDir.Dir))     m_PitchTypeDict.Add(arrowDir.Dir, pitchType);
                        if(!m_PitchArrowUIDict.ContainsKey(arrowDir.Dir)) m_PitchArrowUIDict.Add(arrowDir.Dir, arrowUI);
                        break;
                    }
                }
                
            }
            
            // basic ball
            m_SelectedBallDir = BallDir.SLOW;
            SelectBall(BallDir.SLOW);
        }

        ///////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////
        // breaking  ball type function
        ///////////////////////////////////////////////////////////////
        public void SelectBall(BallDir dir)
        {
            if (m_PitchArrowUIDict.ContainsKey(dir))
            {
                m_SelectedBallDir = dir;
            }  
            else
            {
                m_SelectedBallDir = BallDir.SLOW;
            }
        }

        ///////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////
        // process event AI
        ///////////////////////////////////////////////////////////////
        private void Select(Vector2 dir)
        {
            SelectBall(PitchTypeSO.WhatBallType(dir));
        }

        private void Confirm()
        {
            PitchTypeSO selectedType = m_PitchTypeDict[m_SelectedBallDir];

            _OnConfirmPitchEvent.Raise(selectedType);

            gameObject.SetActive(false);
        }
    }
}