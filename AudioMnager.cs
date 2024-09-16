
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;
    public static AudioManager Instance { get { return _instance; } }

    [SerializeField] private AudioSource _sfxAudio;
    [SerializeField] private AudioSource _musicAudio;
    [SerializeField] private AudioClip _themeMusicClip, _pickUpAudio;
    [SerializeField] private AudioClip[] _bloodSqueezeAudio;
    private void Awake()
    {

        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        PlayMusic(_themeMusicClip, true);
    }
    public void PlayBloodSFX(float volume)
    {
        int i = Random.Range(0, _bloodSqueezeAudio.Length);
        PlaySFX(_bloodSqueezeAudio[i], volume);
    }
    public void PlaySFX(AudioClip clip, float volume)
    {
        _sfxAudio.PlayOneShot(clip, volume);
        //Debug.Log("Play");
    }
    public void PlayMusic(AudioClip audioClip, bool loop)
    {
        _musicAudio.clip = audioClip;
        _musicAudio.loop = loop;
        _musicAudio.Play();
    }
    public void PlayPickUpSFX()
    {
        PlaySFX(_pickUpAudio, 1);
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
