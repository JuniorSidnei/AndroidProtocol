using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameToBeNamed.Utils;
using UnityEngine;
using UnityEngine.Audio;

namespace GameToBeNamed.Utils.Sound {
    public class AudioController : Singleton<AudioController> {
        #region Editor Attributes

        public int SoundEffectChannel = 1;

        #endregion

        #region Enum

        public enum SoundType {
            Music,
            SoundEffect2D,
            Speech
        }

        #endregion

        #region Constants

        const string musicSettings = "musicsettings";
        const string soundSettings = "soundsettings";
        const string unMuteLabel = "unmute";
        const string muteLabel = "mute";
        const string soundVolume = "soundVolume";
        const string musicVolume = "musicVolume";
        const string narrationVolume = "narrationVolume";

        #endregion

        #region Variables

        Dictionary<string, AudioClip> m_cachedSounds;

        private float m_soundVolume;
        private float m_narrationVolume;
        private float m_musicVolume;

        #endregion

        #region Awake

        public void Awake() {

            m_cachedSounds = new Dictionary<string, AudioClip>();
            _soundEffectChannels = new List<AudioSource>();

            _musicChannel = gameObject.AddComponent<AudioSource>();

            _exclusiveSounds = new List<PlayAndRemove>();

            for (int i = 0; i < SoundEffectChannel; i++)
                _soundEffectChannels.Add(gameObject.AddComponent<AudioSource>());

            LoadGameSettings();

            UnMuteSounds();
            UnMuteMusic();

            DontDestroyOnLoad(this);
        }

        private void LoadGameSettings() {
            string music = "unmute";
            string audio = "unmute";

            if (PlayerPrefs.HasKey(musicSettings))
                music = PlayerPrefs.GetString(musicSettings).ToLower();

            if (PlayerPrefs.HasKey(soundSettings))
                audio = PlayerPrefs.GetString(soundSettings).ToLower();

            m_soundVolume = SoundVolume();
            m_musicVolume = MusicVolume();
            m_narrationVolume = NarrationVolume();

            if (music.Equals(muteLabel))
                MuteMusic();
            else
                UnMuteMusic();

            if (audio.Equals(muteLabel))
                MuteSounds();
            else
                UnMuteSounds();
        }

        #endregion

        #region Attributes

        /// <summary>
        /// Controller that handle sounds effects channels 
        /// </summary>
        private List<AudioSource> _soundEffectChannels;

        /// <summary>
        /// Controller that handle music channel
        /// </summary>
        private AudioSource _musicChannel;

        /// <summary>
        /// Handle used channel
        /// </summary>
        private int _nextChannel;

        private bool _muteMusic;

        private bool _muteAudio;

        #endregion

        #region Properties

        public bool IsMusicMuted {
            get { return _muteMusic; }
        }

        public bool IsSoundMuted {
            get { return _muteAudio; }
        }

        List<PlayAndRemove> _exclusiveSounds;

        #endregion

        #region Play

        /// <summary>
        /// Play any sound 
        /// </summary>
        /// <param name="audio">Audio to be played</param>
        /// <param name="type">Type of sound</param>
        //public void Play(string resourcePath, SoundType type, float volume = 1f, bool switchMusicSmooth = true, bool loop = false, bool exclusiveSound = false, float duration = 0, float pitch = 1f)
        //{
        //    if (m_cachedSounds.ContainsKey(resourcePath))
        //    {
        //        Play(m_cachedSounds[resourcePath], type, volume, loop, switchMusicSmooth, exclusiveSound, duration, pitch);
        //    }
        //    else
        //    {
        //        AudioClip clip = Resources.Load<AudioClip>(resourcePath);
        //        m_cachedSounds.Add(resourcePath, clip);
        //        Play(m_cachedSounds[resourcePath], type, volume, loop, switchMusicSmooth, exclusiveSound, duration, pitch);
        //    }
        //}


        /// <summary>
        /// Play any sound 
        /// </summary>
        /// <param name="audio">Audio to be played</param>
        /// <param name="type">Type of sound</param>
        public void Play(AudioClip audioClip, SoundType type, float volume = 1f, bool loop = false,
            bool exclusiveSound = true,
            float duration = 0, float pitch = 1f, float panStereo = 0f, float startFrom = 0f,
            AudioMixerGroup audioMixerGroup = null, bool switchMusicSmooth = false) {



            if (audioClip == null) return;

            switch (type) {
                case SoundType.Music:

                    if (_musicChannel != null && _musicChannel.clip != null &&
                        _musicChannel.clip.name.Equals(audioClip.name) && _musicChannel.isPlaying) {
                        if (_musicChannel.clip.name.Equals(audioClip.name))
                            _musicChannel.volume = volume;
                        return;
                    }

                    if (_musicChannel.clip != null && switchMusicSmooth)
                        StartCoroutine(SwitchMusic(audioClip, volume));
                    else {
                        //volume = m_musicVolume;
                        _musicChannel.volume = volume;
                        _musicChannel.clip = audioClip;
                        _musicChannel.loop = true;
                        _musicChannel.Play();
                    }

                    break;
                case SoundType.SoundEffect2D:

                    if (_muteAudio) return;

                    // volume = type == SoundType.SoundEffect2D ? m_soundVolume : m_narrationVolume;

                    if (exclusiveSound) {
                        for (int i = 0; i < _exclusiveSounds.Count; i++) {
                            if (!_exclusiveSounds[i].IsPlaying) {
                                _exclusiveSounds[i].Play(audioClip, loop, volume, duration, pitch, panStereo, startFrom,
                                    audioMixerGroup);
                                return;
                            }
                        }

                        GameObject go = new GameObject("ExclusiveSound");
                        var removeAfterPlay = go.AddComponent<PlayAndRemove>();
                        removeAfterPlay.Play(audioClip, loop, volume, duration, pitch, panStereo, startFrom,
                            audioMixerGroup);
                        _exclusiveSounds.Add(removeAfterPlay);
                        removeAfterPlay.transform.SetParent(transform);
                        return;
                    }

                    bool _freeChannel = false;

                    for (int i = 0; i < SoundEffectChannel; i++) {
                        if (_soundEffectChannels[i].clip == null || !_soundEffectChannels[i].isPlaying) {
                            _soundEffectChannels[i].clip = audioClip;
                            _soundEffectChannels[i].loop = loop;
                            _soundEffectChannels[i].volume = volume;
                            _soundEffectChannels[i].Play();

                            _freeChannel = true;
                            break;
                        }
                    }

                    if (!_freeChannel) {
                        //Check last index used

                        if (_soundEffectChannels.Any(x => !x.loop)) {
                            var channel = _soundEffectChannels.FirstOrDefault(x => !x.loop);
                            _nextChannel = _soundEffectChannels.IndexOf(channel);
                        }

                        _soundEffectChannels[_nextChannel].clip = audioClip;
                        _soundEffectChannels[_nextChannel].loop = loop;
                        _soundEffectChannels[_nextChannel].volume = volume;
                        _soundEffectChannels[_nextChannel].Play();

                        _nextChannel++;
                        if (_nextChannel > _soundEffectChannels.Count - 1)
                            _nextChannel = 0;
                    }

                    break;
                case SoundType.Speech:
                    break;
                default:
                    break;
            }
        }

        private IEnumerator SwitchMusic(AudioClip audioClip, float volume) {
            float targetTime = Time.time + 0.5f;
            float lastVolume = _musicChannel.volume;
            //reduce the current music volume to 0
            do {
                _musicChannel.volume = Mathf.Lerp(lastVolume, 0, Time.time / targetTime);
                yield return null;
            } while (Time.time < targetTime);

            volume = m_musicVolume;
            _musicChannel.clip = audioClip;
            _musicChannel.loop = true;

            targetTime = Time.time + 0.5f;
            _musicChannel.volume = 0;
            do {
                _musicChannel.volume = Mathf.Lerp(0, volume, Time.time / targetTime);
                yield return null;
            } while (Time.time < targetTime);

            _musicChannel.volume = volume;
            if (!_muteMusic) {
                _musicChannel.Play();
            }
        }

        /// <summary>
        /// Play sound or music attached to object!
        /// </summary>
        /// <param name="audio">Audio to be played</param>
        /// <param name="type">Type of audio</param>
        /// <param name="attachedObject">Object to attche audio</param>
        public void Play(AudioClip audioClip, SoundType type, GameObject attachedObject) {
            Play(audioClip, type);
        }

        #endregion

        #region Stop

        public void Stop(SoundType type = SoundType.Music) {
            Debug.Log("Stop " + Time.time);
            switch (type) {
                case SoundType.Music:
                    _musicChannel.Stop();
                    break;
                case SoundType.SoundEffect2D:
                    foreach (var item in _soundEffectChannels) {
                        item.Stop();
                    }

                    foreach (var item in _exclusiveSounds) {
                        item.audioSource.Stop();
                    }

                    break;
                case SoundType.Speech:
                    break;
                default:
                    break;
            }
        }

        public void StopSound(AudioClip sound, bool exclusive = false) {
            AudioSource currentSoud = null;

            if (!exclusive)
                currentSoud = _soundEffectChannels.FirstOrDefault(x => x.clip == sound);
            else {
                var soundsToStop = _exclusiveSounds.Where(x => x.audioSource.clip == sound).ToList();

                for (int i = 0; i < soundsToStop.Count(); i++) {
                    soundsToStop[i].audioSource.Stop();
                }
            }

            if (currentSoud != null)
                currentSoud.Stop();
        }

        #endregion

        #region Pause

        public void Pause(SoundType type = SoundType.Music) {
            switch (type) {
                case SoundType.Music:
                    _musicChannel.Pause();
                    break;
                case SoundType.SoundEffect2D:
                    break;
                case SoundType.Speech:
                    break;
                default:
                    break;
            }
        }

        #endregion

        #region Resume

        public void Resume(SoundType type = SoundType.Music) {
            switch (type) {
                case SoundType.Music:
                    _musicChannel.Play();
                    break;
                case SoundType.SoundEffect2D:
                    break;
                case SoundType.Speech:
                    break;
                default:
                    break;
            }
        }

        #endregion

        #region Mute

        public void MuteMusic() {
            //AnalyticsManager.Instance.Music(true);

            _muteMusic = true;

            ChangeMusicState(muteLabel);

            Pause();
        }

        public void MuteSounds() {
            //AnalyticsManager.Instance.Music(true);

            ChangeSoundState(muteLabel);

            _muteAudio = true;
        }

        public void MuteAll() {
            //AnalyticsManager.Instance.Music(true);

            MuteSounds();
            MuteMusic();
        }

        public bool IsSoundPlaying(AudioClip sound) {
            if (_soundEffectChannels.Any(x => x.name == sound.name)) {
                return _soundEffectChannels.First(x => x.name == sound.name).isPlaying;
            }
            else if (_exclusiveSounds.Any(x => x.name == sound.name)) {
                return _exclusiveSounds.Last(x => x.name == sound.name).IsPlaying;
            }

            return false;
        }

        public bool isSoundPlaying() {
            var audio = _soundEffectChannels.Where(x => x.isPlaying == true).FirstOrDefault();
            return audio == null ? false : true;
        }

        public float GetSoundProgress(AudioClip sound) {
            if (!IsSoundPlaying(sound)) return 0;

            float progress = 0;

            if (_soundEffectChannels.Any(x => x.name == sound.name)) {
                progress = _soundEffectChannels.First(x => x.name == sound.name).time;
            }
            else {
                progress = _exclusiveSounds.First(x => x.name == sound.name).audioSource.time;
            }

            return progress;
        }

        public float GetSoundLength(AudioClip sound, float pitch, AudioMixerGroup group) {
            if (pitch == 0)
                pitch = float.MinValue;

            if (group == null) {
                return sound.length / pitch;
            }
            else {
                float groupPitch = 0;
                group.audioMixer.GetFloat("pitch", out groupPitch);

                float pitchShifter = 0;
                group.audioMixer.GetFloat("pitch_shifter", out pitchShifter);
                if (groupPitch == 0)
                    groupPitch = float.MinValue;

                if (_exclusiveSounds.Count > 0 && _exclusiveSounds.Any(x => x.name == sound.name)) {
                    var last = _exclusiveSounds.Last(x => x.name == sound.name).audioSource.clip;
                    var samples = (float) last.samples;
                    var freq = (float) last.frequency;
                    print(samples / freq);
                    return samples / freq;
                }

                return sound.length / (pitch * groupPitch * pitchShifter);
            }
        }


        #endregion

        #region UnMute

        public void UnMuteMusic() {
            //AnalyticsManager.Instance.Music(false);

            ChangeMusicState(unMuteLabel);

            _muteMusic = false;
            Resume();

        }

        public void UnMuteSounds() {
            //AnalyticsManager.Instance.Music(false);

            ChangeSoundState(unMuteLabel);
            _muteAudio = false;
        }

        public void UnMuteAll() {
            //AnalyticsManager.Instance.Music(false);

            UnMuteMusic();
            UnMuteSounds();
        }


        public float MusicVolume() {
            if (PlayerPrefs.HasKey(musicVolume))
                return PlayerPrefs.GetFloat(musicVolume);

            return 1;
        }

        public float SoundVolume() {
            if (PlayerPrefs.HasKey(soundVolume))
                return PlayerPrefs.GetFloat(soundVolume);

            return 1;
        }

        public float NarrationVolume() {
            if (PlayerPrefs.HasKey(narrationVolume))
                return PlayerPrefs.GetFloat(narrationVolume);

            return 1;
        }

        public IEnumerator ClearExclusiveSounds() {
            int remainingSounds = _exclusiveSounds.Count;
            int finalQuantity = remainingSounds / 3;
            for (int i = remainingSounds - 1; i > finalQuantity; i--) {
                Destroy(_exclusiveSounds[i].gameObject);
                _exclusiveSounds.RemoveAt(i);
                yield return null;
            }

            m_cachedSounds.Clear();
        }

        #endregion

        #region Private Methods

        private void ChangeMusicState(string newState) {
            PlayerPrefs.SetString(musicSettings, newState);
            PlayerPrefs.SetString(soundSettings, _muteAudio ? muteLabel : unMuteLabel);

        }

        private void ChangeSoundState(string newState) {
            PlayerPrefs.SetString(soundSettings, newState);
            PlayerPrefs.SetString(musicSettings, _muteMusic ? muteLabel : unMuteLabel);

        }

        void OnDestroy() {
            //PlayerPrefs.Save();
        }

        public void ChangeVolume(SoundType currentType, float volume) {
            switch (currentType) {
                case SoundType.Music:
                    m_musicVolume = volume;
                    _musicChannel.volume = m_musicVolume;
                    PlayerPrefs.SetFloat(musicVolume, m_musicVolume);

                    break;
                case SoundType.SoundEffect2D:
                    m_soundVolume = volume;

                    foreach (AudioSource item in _soundEffectChannels) {
                        item.clip = null;
                    }

                    PlayerPrefs.SetFloat(soundVolume, m_soundVolume);
                    break;
            }
        }

        #endregion
    }
}