using System.Collections.Generic;
using GGJ2017.Shared.Logging;
using UnityEngine;
using DG.Tweening;

namespace GGJ2017.CrossContext.Services
{
    class AudioServiceComponent : MonoBehaviour
    {
        const float FADE_DURATION = 0.6f;

        AudioSource _audioSource;
        AudioSource _backgroundSource;
        AudioLibrary _audioLibrary;
        System.Random _random = new System.Random();

        void Awake()
        {
            if (_audioSource == null)
            {
                _audioSource = GetComponent<AudioSource>();

                if (_audioSource == null)
                {
                    _audioSource = gameObject.AddComponent<AudioSource>();
                }
            }

            if (_backgroundSource == null)
            {
                _backgroundSource = gameObject.AddComponent<AudioSource>();
                _backgroundSource.loop = true;
                _backgroundSource.bypassEffects = true;
                _backgroundSource.bypassListenerEffects = true;
                _backgroundSource.bypassReverbZones = true;
            }

            if (_audioLibrary == null)
            {
                var prefab = Resources.Load("Prefabs/AudioLibrary");
                var go = Instantiate(prefab) as GameObject;
                _audioLibrary = go.GetComponent<AudioLibrary>();
            }
        }

        public void Play(AudioId id)
        {
            var items = _audioLibrary.items.FindAll((obj) => obj.id == id);

            if (items.Count == 0)
            {
                Log.WarningFormat("[AudioServiceComponent] Could not find audio clip with id={0}", id);
                return;
            }

            var item = items[_random.Next(0, items.Count)];
            _audioSource.PlayOneShot(item.clip, item.volume);
        }

        public void SetBackground(AudioId id)
        {
            var items = _audioLibrary.items.FindAll((obj) => obj.id == id);

            if (items.Count > 0)
            {
                var item = items[0];
                var clip = item.clip;

                if (!_backgroundSource.isPlaying)
                {
                    _backgroundSource.clip = clip;
                    _backgroundSource.volume = item.volume;
                    _backgroundSource.Play();
                }
                else if(_backgroundSource.isPlaying && _backgroundSource.clip != clip)
                {
                    DOTween.Kill(_backgroundSource);
                    _backgroundSource.DOFade(0f, FADE_DURATION).OnComplete(()=>
                    {
                        _backgroundSource.clip = clip;
                        _backgroundSource.Play();
                        _backgroundSource.DOFade(item.volume, FADE_DURATION);
                    });
                }
            }
            else
            {
                StopBackground();
                Log.WarningFormat("[AudioServiceComponent] Could not find audio clip with id={0}", id);
            }
        }

        public void StopBackground()
        {
            if (_backgroundSource.isPlaying)
            {
                _backgroundSource.Stop();
            }
        }
    }        
}