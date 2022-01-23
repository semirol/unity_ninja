using UnityEngine;

namespace Managers
{
    public class AudioManager : UnitySingleton<AudioManager>
    {
        private AudioSource _bgm;
        private AudioSource _effect;
        
        public override void Awake()
        {
            base.Awake();
            _bgm = gameObject.AddComponent<AudioSource>();
            _bgm.loop = true;
            _bgm.volume = 0.2f;

            _effect = gameObject.AddComponent<AudioSource>();
            _effect.loop = false;

        }

        public void PlayBgm(string clipName)
        {
            _bgm.clip = GetClipByName(clipName);
            _bgm.Play();
        }

        private AudioClip GetClipByName(string clipName)
        {
            return ResourceManager.Instance.GetAssetFromShortPath<AudioClip>("Sounds/" + clipName);
        }
    }
}