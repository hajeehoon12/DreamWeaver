using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] Sound[] sfx = null;
    [SerializeField] Sound[] bgm = null;
    [SerializeField] Sound[] UI = null;
    [SerializeField] Sound[] Wolf = null;
    [SerializeField] Sound[] Samurai = null;
    [SerializeField] Sound[] Holy = null;
    [SerializeField] Sound[] Monster = null;

    [SerializeField] public AudioSource bgmPlayer = null;
    [SerializeField] public AudioSource bgmPlayer2 = null;
    [SerializeField] AudioSource[] sfxPlayer = null;
    //[SerializeField] AudioSource[] UIPlayer = null;
    AudioSource myAudioSource;

    private string audioName;
    public float masterVolumeSFX = 1f;

    private void Awake()
    {
        myAudioSource = GetComponent<AudioSource>();
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        
    }

    private void Start()
    {
        //PlayBGM("SadStory", 0.05f);
    }

    public void PlayBGM(string p_bgmName)
    {
        for (int i = 0; i < bgm.Length; i++)
        {
            if (p_bgmName == bgm[i].name)
            {
                bgmPlayer.clip = bgm[i].clip;
                bgmPlayer.Play();
                return;
            }
        }
        Debug.Log("No name of BGM :" + p_bgmName);
    }
    public void PlayBGM(string p_bgmName, float _volume)
    {
        for (int i = 0; i < bgm.Length; i++)
        {
            if (p_bgmName == bgm[i].name)
            {
                bgmPlayer.clip = bgm[i].clip;
                bgmPlayer.Play();
                bgmPlayer.volume = _volume;
                return;
            }
        }
        Debug.Log("No name of BGM :" + p_bgmName);
    }

    public void PlayBGM2(string p_bgmName, float _volume)
    {
        for (int i = 0; i < bgm.Length; i++)
        {
            if (p_bgmName == bgm[i].name)
            {
                bgmPlayer2.clip = bgm[i].clip;
                bgmPlayer2.Play();
                bgmPlayer2.volume = _volume;
                return;
            }
        }
        Debug.Log("No name of BGM :" + p_bgmName);
    }

    public void StopBGM2()
    {
        bgmPlayer2.Stop();
    }


    public void StopBGM()
    {
        bgmPlayer.Stop();
    }

    public void PlaySFX(string p_sfxName)
    {
        
        for (int i = 0; i < sfx.Length; i++)
        {
            if (p_sfxName == sfx[i].name)
            {
                sfxPlayer[i].pitch = 1f;
                myAudioSource.PlayOneShot(sfx[i].clip);
                return;
            }
        }
        Debug.Log("No name of SFX :" + p_sfxName);
        return;
    }

    public void PlaySFX(string p_sfxName, float _volume) // overloading
    {

        for (int i = 0; i < sfx.Length; i++)
        {
            if (p_sfxName == sfx[i].name)
            {
                for (int j = 0; j < sfxPlayer.Length; j++)
                {
                    
                    if (!sfxPlayer[j].isPlaying)
                    {
                        sfxPlayer[j].clip = sfx[i].clip;
                        sfxPlayer[j].pitch = 1f;
                        sfxPlayer[j].PlayOneShot(sfx[i].clip, _volume);
                        return;
                    }
                }
                Debug.Log("All Audio Player is Used it Needs More");
                return;
            }
        }
        Debug.Log("No name of SFX :" + p_sfxName);
        return;
    }

    public void PlayWolf(string p_sfxName)
    {

        for (int i = 0; i < sfx.Length; i++)
        {
            if (p_sfxName == Wolf[i].name)
            {
                myAudioSource.PlayOneShot(Wolf[i].clip);
                return;
            }
        }
        Debug.Log("No name of Wolf :" + p_sfxName);
        return;
    }

    public void PlayWolf(string p_sfxName, float _volume) // overloading
    {

        for (int i = 0; i < sfx.Length; i++)
        {
            if (p_sfxName == Wolf[i].name)
            {
                for (int j = 0; j < sfxPlayer.Length; j++)
                {

                    if (!sfxPlayer[j].isPlaying)
                    {
                        sfxPlayer[j].clip = Wolf[i].clip;
                        sfxPlayer[j].pitch = 1f;
                        sfxPlayer[j].PlayOneShot(Wolf[i].clip, _volume);
                        return;
                    }
                }
                Debug.Log("All Audio Player is Used it Needs More");
                return;
            }
        }
        Debug.Log("No name of Wolf :" + p_sfxName);
        return;
    }

    public void PlaySamurai(string p_sfxName)
    {

        for (int i = 0; i < sfx.Length; i++)
        {
            if (p_sfxName == Samurai[i].name)
            {
                sfxPlayer[i].pitch = 1f;
                myAudioSource.PlayOneShot(Samurai[i].clip);
                return;
            }
        }
        Debug.Log("No name of Samurai :" + p_sfxName);
        return;
    }

    public void PlaySamurai(string p_sfxName, float _volume) // overloading
    {

        for (int i = 0; i < sfx.Length; i++)
        {
            if (p_sfxName == Samurai[i].name)
            {
                for (int j = 0; j < sfxPlayer.Length; j++)
                {

                    if (!sfxPlayer[j].isPlaying)
                    {
                        sfxPlayer[j].clip = Samurai[i].clip;
                        sfxPlayer[j].pitch = 1f;
                        sfxPlayer[j].PlayOneShot(Samurai[i].clip, _volume);
                        return;
                    }
                }
                Debug.Log("All Audio Player is Used it Needs More");
                return;
            }
        }
        Debug.Log("No name of Samurai :" + p_sfxName);
        return;
    }

    public void PlayHoly(string p_sfxName)
    {

        for (int i = 0; i < sfx.Length; i++)
        {
            if (p_sfxName == Holy[i].name)
            {
                sfxPlayer[i].pitch = 1f;
                myAudioSource.PlayOneShot(Holy[i].clip);
                return;
            }
        }
        Debug.Log("No name of Holy :" + p_sfxName);
        return;
    }

    public void PlayHoly(string p_sfxName, float _volume) // overloading
    {

        for (int i = 0; i < sfx.Length; i++)
        {
            if (p_sfxName == Holy[i].name)
            {
                for (int j = 0; j < sfxPlayer.Length; j++)
                {

                    if (!sfxPlayer[j].isPlaying)
                    {
                        sfxPlayer[j].clip = Holy[i].clip;
                        sfxPlayer[j].pitch = 1f;
                        sfxPlayer[j].PlayOneShot(Holy[i].clip, _volume);
                        return;
                    }
                }
                Debug.Log("All Audio Player is Used it Needs More");
                return;
            }
        }
        Debug.Log("No name of Holy :" + p_sfxName);
        return;
    }

    public void PlayHoly(string p_sfxName, float _volume, float pitch) // overloading
    {

        for (int i = 0; i < sfx.Length; i++)
        {
            if (p_sfxName == Holy[i].name)
            {
                for (int j = 0; j < sfxPlayer.Length; j++)
                {

                    if (!sfxPlayer[j].isPlaying)
                    {
                        sfxPlayer[j].clip = Holy[i].clip;
                        sfxPlayer[j].pitch = pitch;
                        sfxPlayer[j].PlayOneShot(Holy[i].clip, _volume);
                        return;
                    }
                }
                Debug.Log("All Audio Player is Used it Needs More");
                return;
            }
        }
        Debug.Log("No name of Holy :" + p_sfxName);
        return;
    }

    public void PlayHolyPitch(string p_sfxName, float _volume) // overloading
    {

        for (int i = 0; i < sfx.Length; i++)
        {
            if (p_sfxName == Holy[i].name)
            {
                for (int j = 0; j < sfxPlayer.Length; j++)
                {

                    if (!sfxPlayer[j].isPlaying)
                    {
                        sfxPlayer[j].clip = Holy[i].clip;
                        sfxPlayer[j].pitch = 1f + Random.Range(-0.2f, 0.2f);
                        sfxPlayer[j].PlayOneShot(Holy[i].clip, _volume);
                        return;
                    }
                }
                Debug.Log("All Audio Player is Used it Needs More");
                return;
            }
        }
        Debug.Log("No name of Holy :" + p_sfxName);
        return;
    }

    public void PlayMonster(string p_sfxName)
    {

        for (int i = 0; i < sfx.Length; i++)
        {
            if (p_sfxName == Monster[i].name)
            {

                sfxPlayer[i].pitch = 1f;
                myAudioSource.PlayOneShot(Monster[i].clip);
                return;
            }
        }
        Debug.Log("No name of Monster :" + p_sfxName);
        return;
    }

    public void PlayMonster(string p_sfxName, float _volume) // overloading
    {

        for (int i = 0; i < sfx.Length; i++)
        {
            if (p_sfxName == Monster[i].name)
            {
                for (int j = 0; j < sfxPlayer.Length; j++)
                {

                    if (!sfxPlayer[j].isPlaying)
                    {
                        sfxPlayer[j].clip = Monster[i].clip;
                        sfxPlayer[j].pitch = 1f;
                        sfxPlayer[j].PlayOneShot(Monster[i].clip, _volume);
                        return;
                    }
                }
                Debug.Log("All Audio Player is Used it Needs More");
                return;
            }
        }
        Debug.Log("No name of Monster :" + p_sfxName);
        return;
    }

    public void PlayMonster(string p_sfxName, float _volume, float pitch) // overloading
    {

        for (int i = 0; i < sfx.Length; i++)
        {
            if (p_sfxName == Monster[i].name)
            {
                for (int j = 0; j < sfxPlayer.Length; j++)
                {

                    if (!sfxPlayer[j].isPlaying)
                    {
                        sfxPlayer[j].clip = Monster[i].clip;
                        sfxPlayer[j].pitch = pitch;
                        sfxPlayer[j].PlayOneShot(Monster[i].clip, _volume);
                        return;
                    }
                }
                Debug.Log("All Audio Player is Used it Needs More");
                return;
            }
        }
        Debug.Log("No name of Monster :" + p_sfxName);
        return;
    }

    public void PlayMonsterPitch(string p_sfxName, float _volume) // overloading
    {

        for (int i = 0; i < sfx.Length; i++)
        {
            if (p_sfxName == Monster[i].name)
            {
                for (int j = 0; j < sfxPlayer.Length; j++)
                {

                    if (!sfxPlayer[j].isPlaying)
                    {
                        sfxPlayer[j].clip = Monster[i].clip;
                        sfxPlayer[j].pitch = 1f + Random.Range(-0.2f, 0.2f);
                        sfxPlayer[j].PlayOneShot(Monster[i].clip, _volume);
                        return;
                    }
                }
                Debug.Log("All Audio Player is Used it Needs More");
                return;
            }
        }
        Debug.Log("No name of Monster :" + p_sfxName);
        return;
    }


    public void PlayUI(string p_sfxName)
    {

        for (int i = 0; i < sfx.Length; i++)
        {
            if (p_sfxName == UI[i].name)
            {
                sfxPlayer[i].pitch = 1f;
                myAudioSource.PlayOneShot(UI[i].clip);
                return;
            }
        }
        Debug.Log("No name of UI :" + p_sfxName);
        return;
    }

    public void PlayUI(string p_sfxName, float _volume) // overloading
    {

        for (int i = 0; i < sfx.Length; i++)
        {
            if (p_sfxName == UI[i].name)
            {
                for (int j = 0; j < sfxPlayer.Length; j++)
                {

                    if (!sfxPlayer[j].isPlaying)
                    {
                        sfxPlayer[j].clip = UI[i].clip;
                        sfxPlayer[j].pitch = 1f;
                        sfxPlayer[j].PlayOneShot(UI[i].clip, _volume);
                        return;
                    }
                }
                Debug.Log("All Audio Player is Used it Needs More");
                return;
            }
        }
        Debug.Log("No name of UI :" + p_sfxName);
        return;
    }

    public void PlayPitchSFX(string p_sfxName, float _volume) // overloading Change Pitch
    {

        for (int i = 0; i < sfx.Length; i++)
        {
            if (p_sfxName == sfx[i].name)
            {
                for (int j = 0; j < sfxPlayer.Length; j++)
                {
                    
                    if (!sfxPlayer[j].isPlaying)
                    {
                        sfxPlayer[j].clip = sfx[i].clip;
                        sfxPlayer[j].pitch = 1f + Random.Range(-0.2f, 0.2f);
                        sfxPlayer[j].PlayOneShot(sfx[i].clip, _volume);

                        return;
                    }
                }
                Debug.Log("All Audio Player is Used it Needs More");
                return;
            }
        }
        Debug.Log("No name of SFX :" + p_sfxName);
        return;
    }


}