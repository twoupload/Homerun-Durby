using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 사운드 제어와 관련된 스크립트
// 최초 작성자 : 이상도
// 수정자: 이상도
// 최종 수정일: 2025-05-13

public class SoundManager : MonoBehaviour
{
    ///////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////
    // Components
    ///////////////////////////////////////////////////////////////
    public static SoundManager Instance;

    private Dictionary<string, AudioClip> soundDictionary;
    private List<AudioSource> audioSources;
    private int maxAudioSources = 10;
    public int bgmIndex = -1;
    public int bgmOffsetIndex = 0;
    public bool isEndingBGM = false;

    ///////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////
    // Unity Function
    ///////////////////////////////////////////////////////////////

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        soundDictionary = new Dictionary<string, AudioClip>();
        audioSources = new List<AudioSource>();

        // Initialize a pool of AudioSources
        for (int i = 0; i < maxAudioSources; i++)
        {
            AudioSource newAudioSource = gameObject.AddComponent<AudioSource>();
            audioSources.Add(newAudioSource);
        }
    }

    ///////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////
    // Load Function
    ///////////////////////////////////////////////////////////////
    
    public void LoadSoundData(SoundData soundData)
    {
        // clear
        foreach(var sound in soundData.sound)
        {
            if(!soundDictionary.ContainsKey(sound.soundName))
            {
                soundDictionary.Add(sound.soundName, sound.audioClip);
            }
            else
            {
                print("Already exists : " + sound.soundName);
            }
        }
    }

    ///////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////
    // Using Function
    ///////////////////////////////////////////////////////////////
    public int PlaySound(string soundName, bool loop = false, float volume = 1.0f)
    {
        if (!soundDictionary.ContainsKey(soundName))
        {
            Debug.LogWarning($"Sound: {soundName} not found!");
            return -1; // Return -1 if sound is not found
        }

        // Try to find an available AudioSource using a more efficient method (avoid Find)
        AudioSource availableAudioSource = null;

        // Try to find an available source from the pool (or reuse from the list)
        foreach (AudioSource source in audioSources)
        {
            if (!source.isPlaying)
            {
                availableAudioSource = source;
                break;
            }
        }

        if (availableAudioSource != null)
        {
            availableAudioSource.clip = soundDictionary[soundName];
            availableAudioSource.loop = loop;
            availableAudioSource.volume = volume;
            availableAudioSource.Play();
            return audioSources.IndexOf(availableAudioSource);
        }
        else
        {
            // If no available AudioSource, create a new one
            AudioSource tempSource = gameObject.AddComponent<AudioSource>();
            tempSource.clip = soundDictionary[soundName];
            tempSource.loop = loop;
            tempSource.volume = volume;
            tempSource.Play();
            audioSources.Add(tempSource); // Add to list to manage it

            int tempIndex = audioSources.IndexOf(tempSource);

            if (!loop)
            {
                // Remove temporary source when it's done
                StartCoroutine(RemoveAudioSourceWhenDone(tempSource, tempSource.clip.length));
            }

            if (soundName.Contains("BGM"))
            {
                bgmIndex = tempIndex;
                if (soundName[soundName.Length - 1] != 'M')
                {
                    bgmOffsetIndex = soundName[soundName.Length - 1];
                }
            }

            return tempIndex;
        }
    }

    // Stop the sound at a specific AudioSource index
    public void StopSound(int index)
    {
        if (index >= 0 && index < audioSources.Count && audioSources[index].isPlaying)
        {
            audioSources[index].Stop();
        }
        else
        {
            Debug.LogWarning("Invalid AudioSource index or AudioSource is not playing.");
        }
    }

    // Coroutine to remove AudioSource after it finishes playing
    private IEnumerator RemoveAudioSourceWhenDone(AudioSource source, float duration)
    {
        yield return new WaitForSeconds(duration);
        audioSources.Remove(source);
        Destroy(source);
    }

    public void PlayBGMWithTransition(string beforeSoundName, string bgmSoundName, bool loop = true, float volume = 1.0f)
    {
        StartCoroutine(PlayBGMWithTransitionCoroutine(beforeSoundName, bgmSoundName, loop, volume));
    }


    private IEnumerator PlayBGMWithTransitionCoroutine(string beforeSoundName, string bgmSoundName, bool loop, float volume)
    {
        // 이전 사운드 재생
        int beforeSoundIndex = PlaySound(beforeSoundName, false, volume);
        AudioSource beforeSoundSource = audioSources[beforeSoundIndex];
        float beforeSoundLength = beforeSoundSource.clip.length;

        Debug.LogWarning(beforeSoundLength);

        // BGM 클립을 미리 로드
        AudioClip bgmClip = soundDictionary[bgmSoundName];

        // 이전 BGM이 끝날 때까지 대기
        yield return new WaitForSecondsRealtime(beforeSoundLength);

        // BGM 시작
        PlaySound(bgmSoundName, loop, volume);
    }

    public void StopAllSound()
    {
        foreach (AudioSource source in audioSources)
        {
            if (source.isPlaying)
            {
                source.Stop();
            }
        }
    }
}
