using GameFramework;
using GameFramework.DataTable;
using GameFramework.Sound;
using UnityGameFramework.Runtime;

namespace BaseModule
{
    public static class SoundExtension
    {
        private const float FadeVolumeDuration = 1f;
        private static int? s_MusicSerialId = null;

        public static int? PlayMusic(this SoundComponent soundComponent, string assetName, object userData = null)
        {
            soundComponent.StopMusic();
            PlaySoundParams playSoundParams = PlaySoundParams.Create();
            playSoundParams.Priority = 64;
            playSoundParams.Loop = true;
            playSoundParams.VolumeInSoundGroup = 1f;
            playSoundParams.FadeInSeconds = FadeVolumeDuration;
            playSoundParams.SpatialBlend = 0f;
            s_MusicSerialId = soundComponent.PlaySound(assetName, "Music", Constant.AssetPriority.MusicAsset, playSoundParams, null, userData);
            return s_MusicSerialId;
        }

        public static void StopMusic(this SoundComponent soundComponent)
        {
            if (!s_MusicSerialId.HasValue)
            {
                return;
            }

            soundComponent.StopSound(s_MusicSerialId.Value, FadeVolumeDuration);
            s_MusicSerialId = null;
        }

        public static void PauseMusic(this SoundComponent soundComponent)
        {
            if (!s_MusicSerialId.HasValue)
            {
                return;
            }

            soundComponent.PauseSound(s_MusicSerialId.Value, FadeVolumeDuration);
        }
        
        public static void ResumeMusic(this SoundComponent soundComponent)
        {
            if (!s_MusicSerialId.HasValue)
            {
                return;
            }

            soundComponent.ResumeSound(s_MusicSerialId.Value, FadeVolumeDuration);
        }

        public static int? PlaySound(this SoundComponent soundComponent, string assetName, Entity bindingEntity = null, object userData = null)
        {
            if (string.IsNullOrEmpty(assetName))
            {
                Log.Warning("Can not load sound '{0}' from data table.", assetName.ToString());
                return null;
            }

            PlaySoundParams playSoundParams = PlaySoundParams.Create();
            playSoundParams.Priority = 0;
            playSoundParams.Loop = false;
            playSoundParams.VolumeInSoundGroup = 1;
            playSoundParams.SpatialBlend = 1;

            string soundAssetName = assetName;
            return soundComponent.PlaySound(soundAssetName, "Sound", Constant.AssetPriority.SoundAsset, playSoundParams,
                bindingEntity != null ? bindingEntity : null, userData);
        }

        public static int? PlayUISound(this SoundComponent soundComponent, string assetName, float volume = 1, int priority = 0, object userData = null)
        {
            PlaySoundParams playSoundParams = PlaySoundParams.Create();
            playSoundParams.Priority = priority;
            playSoundParams.Loop = false;
            playSoundParams.VolumeInSoundGroup = volume;
            playSoundParams.SpatialBlend = 0f;
            return soundComponent.PlaySound(assetName, "UISound", Constant.AssetPriority.UISoundAsset, playSoundParams, userData);
        }

        public static bool IsMuted(this SoundComponent soundComponent, string soundGroupName)
        {
            if (string.IsNullOrEmpty(soundGroupName))
            {
                Log.Warning("Sound group is invalid.");
                return true;
            }

            ISoundGroup soundGroup = soundComponent.GetSoundGroup(soundGroupName);
            if (soundGroup == null)
            {
                Log.Warning("Sound group '{0}' is invalid.", soundGroupName);
                return true;
            }

            return soundGroup.Mute;
        }

        public static void Mute(this SoundComponent soundComponent, string soundGroupName, bool mute)
        {
            if (string.IsNullOrEmpty(soundGroupName))
            {
                Log.Warning("Sound group is invalid.");
                return;
            }

            ISoundGroup soundGroup = soundComponent.GetSoundGroup(soundGroupName);
            if (soundGroup == null)
            {
                Log.Warning("Sound group '{0}' is invalid.", soundGroupName);
                return;
            }

            soundGroup.Mute = mute;
            //todo 临时屏蔽
            //GameModule.Setting.SetBool(Utility.Text.Format(Constant.Setting.SoundGroupMuted, soundGroupName), mute);
            //GameModule.Setting.Save();
        }

        public static float GetVolume(this SoundComponent soundComponent, string soundGroupName)
        {
            if (string.IsNullOrEmpty(soundGroupName))
            {
                Log.Warning("Sound group is invalid.");
                return 0f;
            }

            ISoundGroup soundGroup = soundComponent.GetSoundGroup(soundGroupName);
            if (soundGroup == null)
            {
                Log.Warning("Sound group '{0}' is invalid.", soundGroupName);
                return 0f;
            }

            return soundGroup.Volume;
        }

        public static void SetVolume(this SoundComponent soundComponent, string soundGroupName, float volume)
        {
            if (string.IsNullOrEmpty(soundGroupName))
            {
                Log.Warning("Sound group is invalid.");
                return;
            }

            ISoundGroup soundGroup = soundComponent.GetSoundGroup(soundGroupName);
            if (soundGroup == null)
            {
                Log.Warning("Sound group '{0}' is invalid.", soundGroupName);
                return;
            }

            soundGroup.Volume = volume;

            //todo 临时屏蔽
            //GameModule.Setting.SetFloat(Utility.Text.Format(Constant.Setting.SoundGroupVolume, soundGroupName), volume);
            //GameModule.Setting.Save();
        }

        public static void PauseAllSoundGroup(this SoundComponent soundComponent, string soundGroupName, float fadeOutSeconds=0f)
        {
            if (string.IsNullOrEmpty(soundGroupName))
            {
                ISoundGroup[] soundGroups = soundComponent.GetAllSoundGroups();
                foreach (ISoundGroup soundGroup in soundGroups)
                {
                    //todo 临时屏蔽
                    //soundGroup.PauseAllSound(fadeOutSeconds);
                }
            }
            else
            {
                ISoundGroup group = soundComponent.GetSoundGroup(soundGroupName);
                //todo 临时屏蔽
                // if(group != null)
                //     group.PauseAllSound(fadeOutSeconds);
            }
        }
        
        public static void ResumeAllSoundGroup(this SoundComponent soundComponent, string soundGroupName, float fadeInSeconds=0f)
        {
            if (string.IsNullOrEmpty(soundGroupName))
            {
                ISoundGroup[] soundGroups = soundComponent.GetAllSoundGroups();
                foreach (ISoundGroup soundGroup in soundGroups)
                {
                    //todo 临时屏蔽
                    //soundGroup.ResumeAllSound(fadeInSeconds);
                }
            }
            else
            {
                ISoundGroup group = soundComponent.GetSoundGroup(soundGroupName);
                //todo 临时屏蔽
                // if(group != null)
                //     group.ResumeAllSound(fadeInSeconds);
            }
        }
    }
}