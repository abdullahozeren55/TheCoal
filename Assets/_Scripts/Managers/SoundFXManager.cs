using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundFXManager : MonoBehaviour
{
    public static SoundFXManager instance;

    private int rand;
    private int previousRand;

    private float pitch;
    private float previousPitch;

    [SerializeField] private AudioSource soundFXObject;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    public void PlaySoundFXClip(AudioClip audioClip, Transform spawnTransform, float volume)
    {
        //spawn in gameobject
        AudioSource audioSource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity);

        //assign the clip
        audioSource.clip = audioClip;

        //assign volume
        audioSource.volume = volume;

        //play sound
        audioSource.Play();

        //get length of sound clip
        float clipLength = audioSource.clip.length;

        //destroy after done
        Destroy(audioSource.gameObject, clipLength);
    }

    public void PlaySoundFXClip(AudioClip audioClip, Transform spawnTransform, float volume, float minRandPitch, float maxRandPitch)
    {
        SetRandomPitch(minRandPitch, maxRandPitch);
        //spawn in gameobject
        AudioSource audioSource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity);

        //assign the clip
        audioSource.clip = audioClip;

        //change pitch
        audioSource.pitch = pitch;

        //assign volume
        audioSource.volume = volume;

        //play sound
        audioSource.Play();

        //get length of sound clip
        float clipLength = audioSource.clip.length;

        //destroy after done
        Destroy(audioSource.gameObject, clipLength);
    }

    public void PlaySoundFXClip(AudioClip[] audioClip, Transform spawnTransform, float volume, float minRandPitch, float maxRandPitch)
    {

        //assign a random index
        SetRandomClip(audioClip.Length);

        SetRandomPitch(minRandPitch, maxRandPitch);
        //spawn in gameobject
        AudioSource audioSource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity);

        //assign the clip
        audioSource.clip = audioClip[rand];

        //change pitch
        audioSource.pitch = pitch;

        //assign volume
        audioSource.volume = volume;

        //play sound
        audioSource.Play();

        //get length of sound clip
        float clipLength = audioSource.clip.length;

        //destroy after done
        Destroy(audioSource.gameObject, clipLength);
    }

    public void PlaySoundFXClip(AudioClip[] audioClip, Transform spawnTransform, float volume)
    {

        //assign a random index
        SetRandomClip(audioClip.Length);

        //spawn in gameobject
        AudioSource audioSource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity);

        //assign the clip
        audioSource.clip = audioClip[rand];

        //assign volume
        audioSource.volume = volume;

        //play sound
        audioSource.Play();

        //get length of sound clip
        float clipLength = audioSource.clip.length;

        //destroy after done
        Destroy(audioSource.gameObject, clipLength);
    }

    private void SetRandomClip(int length)
    {
        rand = Random.Range(0, length);
        if(rand == previousRand)
        {
            SetRandomClip(length);
        }
        else
        {
            previousRand = rand;
        }
    }

    private void SetRandomPitch(float minPitch, float maxPitch)
    {
        pitch = Random.Range(minPitch, maxPitch);
        if(pitch == previousPitch)
        {
            SetRandomPitch(minPitch, maxPitch);
        }
        else
        {
            previousPitch = pitch;
        }
    }
}
