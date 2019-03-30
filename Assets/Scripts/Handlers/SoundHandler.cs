using UnityEngine;

namespace Handlers
{
    public class SoundHandler : MonoBehaviour
    {
        public static SoundHandler sound;
    
        public AudioClip a;
        public AudioClip ba;
        public AudioClip bba;
        public AudioClip bbb;
        public AudioClip ca;
        public AudioClip cb;
        public AudioClip cc;
        public AudioClip da;
        public AudioClip ea;
        public AudioClip eb;
        public AudioClip fa;
        public AudioClip fba;
        public AudioClip fbba;
        public AudioClip fbbb;
        public AudioClip fbbc;
        public AudioClip fbc;

        public AudioClip loadingSound;

        public AudioSource audioSource;

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
            sound = gameObject.GetComponent<SoundHandler>();
            gameObject.GetComponent<Main>().Initialize();
        }
    
        public void Play(AudioClip audioClip)
        {
            audioSource.clip = audioClip;
            audioSource.Play();
        }
    }
}
