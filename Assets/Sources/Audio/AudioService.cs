using System;
using System.Collections.Generic;
using CreatingDust.Shared.Extensions;
using UnityEngine;

namespace CreatingDust.GGJ2017.CrossContext.Services
{
    public class AudioService : IAudioService
    {
        const string KEY_AUDIO_SERVICE_DATA = "audioServiceData";
        const string KEY_FX_ENABLED = "fxEnabled";
        const string KEY_MUSIC_ENABLED = "musicEnabled";

        const bool AUDIO_ENABLED_DEFAULT_VALUE = true;

        AudioServiceComponent _audioServiceComponent;

        public bool fxEnabled { get { return _fxEnabled; } set { _fxEnabled = value; SaveData(); } }
        bool _fxEnabled;

        public bool musicEnabled { get { return _musicEnabled; } set { _musicEnabled = value; SaveData(); CheckBackground(); } }

        bool _musicEnabled;

        IPlayerPrefsService _playerPrefsService;

        public void Setup(IPlayerPrefsService playerPrefsService)
        {
            if (_audioServiceComponent == null)
            {                
                var go = new GameObject();
                go.name = "Audio Service";
                _audioServiceComponent = go.AddComponent<AudioServiceComponent>();
            }

            _fxEnabled = AUDIO_ENABLED_DEFAULT_VALUE;
            _musicEnabled = AUDIO_ENABLED_DEFAULT_VALUE;

            _playerPrefsService = playerPrefsService;

            LoadData();
        }

        void LoadData()
        {
            var data = _playerPrefsService.LoadData(KEY_AUDIO_SERVICE_DATA) as Dictionary<string, object>;
            if (data != null)
            {
                _fxEnabled = data.GetBool(KEY_FX_ENABLED, true);
                _musicEnabled = data.GetBool(KEY_MUSIC_ENABLED, true);
            }
        }

        void SaveData()
        {
            var data = new Dictionary<string, object>
            {
                { KEY_FX_ENABLED, _fxEnabled },
                { KEY_MUSIC_ENABLED, _musicEnabled }
            };
            _playerPrefsService.SaveData(data, KEY_AUDIO_SERVICE_DATA);
        }

        public void Play(AudioId id)
        {
            if (fxEnabled)
            {                
                _audioServiceComponent.Play(id);
            }
        }

        public void SetBackground(AudioId id)
        {
            if (musicEnabled)
            {
                _audioServiceComponent.SetBackground(id);
            }
            else
            {
                _audioServiceComponent.StopBackground();
            }
        }

        void CheckBackground()
        {
            if (!musicEnabled)
            {
                _audioServiceComponent.StopBackground();
            }
        }
    }
}