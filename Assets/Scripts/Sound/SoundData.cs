using System.Collections.Generic;
using UnityEngine;

// BGM, SFX에 관련된 데이터에 관련된 스크립터블 오브젝트
// 최초 작성자 : 이상도
// 수정자: 이상도
// 최종 수정일: 2025-05-13

[CreateAssetMenu(fileName = "SoundData", menuName = "Sound/SoundData")]


public class SoundData : ScriptableObject
{
    ///////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////
    // Components
    ///////////////////////////////////////////////////////////////
    
    [System.Serializable]
    public class Sound
    {
        public string soundName;
        public AudioClip audioClip;
    }

    public List<Sound> sound = new List<Sound>();
}
