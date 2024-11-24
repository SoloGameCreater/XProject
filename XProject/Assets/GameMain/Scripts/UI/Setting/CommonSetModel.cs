
public class CommonSetModel : Manager<CommonSetModel>
{
    public void Init()
    {
        //todo 存档 后期实现
        // var storageCook = StorageManager.Instance.GetStorage<StorageGlobal>();
        // AudioSysManager.Instance.MusicClose = storageCook.MusicClose;
        // AudioSysManager.Instance.SoundClose = storageCook.SoundEffectClose;
    }
    //todo 存档 后期实现
    public bool MusicClose;
    // {
    //     get
    //     {
    //         var storageCook = StorageManager.Instance.GetStorage<StorageGlobal>();
    //         return storageCook.MusicClose;
    //     }
    //     set
    //     {
    //         var storageCook = StorageManager.Instance.GetStorage<StorageGlobal>();
    //         storageCook.MusicClose = value;
    //         AudioSysManager.Instance.MusicClose = value;
    //     }
    // }
    //todo 存档 后期实现
    public bool SoundClose;
    // {
    //     get
    //     {
    //         var storageCook = StorageManager.Instance.GetStorage<StorageGlobal>();
    //         return storageCook.SoundEffectClose;
    //     }
    //     set
    //     {
    //         var storageCook = StorageManager.Instance.GetStorage<StorageGlobal>();
    //         storageCook.SoundEffectClose = value;
    //         AudioSysManager.Instance.SoundClose = value;
    //     }
    // }
    //todo 存档 后期实现
    public bool ShakeClose;
    // {
    //     get
    //     {
    //         var storageCook = StorageManager.Instance.GetStorage<StorageGlobal>();
    //         return storageCook.ShakeClose;
    //     }
    //     set
    //     {
    //         var storageCook = StorageManager.Instance.GetStorage<StorageGlobal>();
    //         storageCook.ShakeClose = value;
    //     }
    // }
}
