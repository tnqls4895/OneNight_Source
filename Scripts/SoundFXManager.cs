using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ESound
{
    ECheckin,


    EScreem,
    EGhostFrameClick,
    EDoorKnock,
    ERing,
    EUnknownSound,
    EBubble
};

public class SoundFXManager : MonoBehaviour
{
    public static SoundFXManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this.gameObject);
        }

    }

    public AudioClip[] SoundFXs;
    public AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlaySound(ESound sound)
    {
        AudioSource.PlayClipAtPoint(SoundFXs[(int)sound], Vector3.zero);
    }
}
