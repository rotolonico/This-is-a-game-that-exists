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
        public AudioClip g;
        public AudioClip ha;
        public AudioClip hba;
        public AudioClip hbb;
        public AudioClip i;
        public AudioClip ja;
        public AudioClip jb;
        public AudioClip k;
        public AudioClip la;
        public AudioClip lb;
        public AudioClip m;
        public AudioClip na;
        public AudioClip nb;
        public AudioClip o;
        public AudioClip p;

        public AudioClip loadingSound;
        public AudioClip squareWhite;
        public AudioClip squareColor;

        public AudioSource audioSource;
        public AudioSource secondaryAudioSource;

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
        
        public void PlaySecondary(AudioClip audioClip)
        {
            secondaryAudioSource.clip = audioClip;
            secondaryAudioSource.Play();
        }
    }
}
