using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 사운드 데이터에 관련된 스크립트
// 최초 작성자 : 이상도
// 수정자: 이상도
// 최종 수정일: 2025-05-15

public class SoundDataLoader : MonoBehaviour
{
    ///////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////
    // Components
    ///////////////////////////////////////////////////////////////

    public SoundData strikeData;
    public SoundData sfxData;
    public SoundData bgmData;

    ///////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////
    // Unity Function
    ///////////////////////////////////////////////////////////////

    private void Start()
    {

        /*
        if (SoundManager.Instance != null && soundData != null)
        {
            SoundManager.Instance.LoadSoundData(soundData);
        }
        */

        if (SoundManager.Instance != null)
        {
            if (sfxData != null)
            {
                SoundManager.Instance.LoadSoundData(sfxData);
            }

            if (bgmData != null)
            {
                SoundManager.Instance.LoadSoundData(bgmData);
            }

            // StrikeData 로드 (아직 로드되지 않는 경우)
            if (strikeData != null)
            {
                SoundManager.Instance.LoadSoundData(strikeData);
            }
        }

    }
}
