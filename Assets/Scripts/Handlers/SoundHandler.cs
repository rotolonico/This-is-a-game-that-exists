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
        public AudioClip pa;
        public AudioClip qa;
        public AudioClip qb;
        public AudioClip ra;
        public AudioClip rb;
        public AudioClip s;
        public AudioClip ta;
        public AudioClip taa;
        public AudioClip tb;
        public AudioClip tba;
        public AudioClip u;
        public AudioClip ua;
        public AudioClip v;
        public AudioClip w;
        public AudioClip wa;
        public AudioClip xa;
        public AudioClip xaa;
        public AudioClip xb;
        public AudioClip xba;
        public AudioClip y;

        public AudioClip loadingSound;
        public AudioClip squareWhite;
        public AudioClip squareColor;
        public AudioClip laser;

        public AudioSource audioSource;
        public AudioSource secondaryAudioSource;

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
            sound = GetComponent<SoundHandler>();
        }
    
        public void Play(AudioClip audioClip)
        {
            audioSource.clip = audioClip;
            audioSource.Play();
        }
        
        public void Stop()
        {
            audioSource.Stop();
        }
        
        public void PlaySecondary(AudioClip audioClip)
        {
            secondaryAudioSource.clip = audioClip;
            secondaryAudioSource.Play();
        }

        public void StopSecondary()
        {
            secondaryAudioSource.Stop();
        }
    }
}
