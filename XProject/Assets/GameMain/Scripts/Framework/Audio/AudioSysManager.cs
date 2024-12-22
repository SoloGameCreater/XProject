using BaseModule;
using Framework;
using GameFramework.Sound;

public enum AudioType
{
    MUSIC,
    SOUND,
    PATH
}

public class AudioSysManager : GlobalSystem<AudioSysManager>
{
    public bool MusicClose
    {
        get { return GameModule.Sound.IsMuted("Music"); }
        set { GameModule.Sound.Mute("Music", value); }
    }

    public bool SoundClose
    {
        get { return GameModule.Sound.IsMuted("Sound"); }
        set { GameModule.Sound.Mute("Sound", value); }
    }

    public int? PlayMusic(string musicName, bool loop = true, float volume = 1)
    {
        string path = GetAudioPath(AudioType.MUSIC, musicName);
        bool checkPath = true;//todo GameModule.Resource.CheckLocationValid(path);
        if (!checkPath)
        {
            UnityEngine.Debug.LogError("找不到音效资源:" + path);
            return -1;
        }

        return GameModule.Sound.PlayMusic(GetAudioPath(AudioType.MUSIC, musicName));
    }

    private int InternalPlayAudio(string soundName, string groupName, float volume = 1f, bool loop = false,
        AudioType audioType = AudioType.SOUND)
    {
        PlaySoundParams playSoundParams = PlaySoundParams.Create();
        playSoundParams.Priority = 64;
        playSoundParams.Loop = loop;
        playSoundParams.VolumeInSoundGroup = volume;
        playSoundParams.FadeInSeconds = 0f;
        playSoundParams.SpatialBlend = 0f;
        string path = GetAudioPath(audioType, soundName);
        bool checkPath = true;//todo GameModule.Resource.CheckLocationValid(path);
        if (!checkPath)
        {
            UnityEngine.Debug.LogError("找不到音效资源:" + path);
            return -1;
        }

        return GameModule.Sound.PlaySound(path, groupName, playSoundParams);
    }

    /// <summary>
    /// 通过音效名字播放音效
    /// </summary>
    /// <param name="soundName"></param>
    /// <param name="volume"></param>
    public int PlaySound(string soundName, float volume = 1)
    {
        return InternalPlayAudio(soundName, "Sound", volume);
    }

    /// <summary>
    /// 通过音效名字播放音效
    /// </summary>
    /// <param name="soundName"></param>
    /// <param name="loop"></param>
    /// <returns></returns>
    public int PlaySound(string soundName, bool loop)
    {
        return InternalPlayAudio(soundName, "Sound", 1f, loop);
    }

    /// <summary>
    /// 通过路径播放音效
    /// </summary>
    /// <param name="soundPath">相对与Export的路径</param>
    /// <param name="loop"></param>
    /// <returns></returns>
    public int PlaySoundByPath(string soundPath, bool loop)
    {
        return InternalPlayAudio(soundPath, "Sound", 1f, loop, AudioType.PATH);
    }

    public void StopSoundById(int soundId)
    {
        GameModule.Sound.StopSound(soundId);
    }

    public void StopAllMusic()
    {
        GameModule.Sound.StopMusic();
    }

    public void StopAllSound()
    {
        GameModule.Sound.StopAllLoadedSounds();
    }

    public void PauseAllMusic()
    {
        GameModule.Sound.PauseMusic();
    }

    public void ResumeAllMusic()
    {
        if (MusicClose)
            return;
        GameModule.Sound.ResumeMusic();
    }

    public void PauseAllSounds(float fadeOutSeconds = 0f)
    {
        GameModule.Sound.PauseAllSoundGroup("Sound", fadeOutSeconds);
    }

    public void ResumeAllSound(float fadeInSeconds = 0f)
    {
        GameModule.Sound.ResumeAllSoundGroup("Sound", fadeInSeconds);
    }

    private string GetAudioPath(AudioType type, string audioName)
    {
        if (type == AudioType.PATH)
            return $"Assets/ExtraRes/{audioName}";

        string prefix = "BGM";
        switch (type)
        {
            case AudioType.MUSIC:
                prefix = "BGM";
                break;
            case AudioType.SOUND:
                prefix = "Sound";
                break;
        }

        var audioPath = $"Assets/ExtraRes/Audio/{prefix}/{audioName}";
        return audioPath;
    }
}