using System.Collections.Generic;
using UnityEngine;

// 투수 데이터를 정의하는 스크립터블 오브젝트 스크립트
// 최초 작성자 : 이상도
// 수정자: 이상도
// 최종 수정일: 2025-05-04

namespace Pitcher
{
    ///////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////
    // new asset
    ///////////////////////////////////////////////////////////////
    [CreateAssetMenu(fileName = "PitcherData", menuName = "ScriptableObjects/PitcherData", order = 1)]
    public class PitcherDataSO : ScriptableObject
    {
        ///////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////
        // components - pitcher character
        ///////////////////////////////////////////////////////////////
        [Range(50,100)] public float Stamina;
        [Range(50,100)] public int Control;
        public List<PitchTypeSO> PitchTypes = new List<PitchTypeSO>();
    }
}

