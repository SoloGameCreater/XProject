using System.Collections.Generic;
public class AOTGenericReferences : UnityEngine.MonoBehaviour
{

	// {{ AOT assemblies
	public static readonly IReadOnlyList<string> PatchedAOTAssemblyList = new List<string>
	{
		"DOTween.dll",
		"Newtonsoft.Json.dll",
		"SDK.dll",
		"SocketIOUnity.dll",
		"StompyRobot.dll",
		"System.Core.dll",
		"System.dll",
		"UniTask.dll",
		"UnityEngine.AndroidJNIModule.dll",
		"UnityEngine.CoreModule.dll",
		"UnityEngine.JSONSerializeModule.dll",
		"UnityEngine.UI.dll",
		"UnityGameFramework.Runtime.dll",
		"mscorlib.dll",
		"protobuf-net.Core.dll",
		"spine-unity.dll",
	};
	// }}

	// {{ constraint implement type
	// }} 

	// {{ AOT generic types
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder<object>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid.<>c<ActivityResHotUpdate.<Download>d__2>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid.<>c<DragonU3DSDK.Asset.ResourcesManager.<LoadResourceAsync>d__2<object>>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid<ActivityResHotUpdate.<Download>d__2>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid<DragonU3DSDK.Asset.ResourcesManager.<LoadResourceAsync>d__2<object>>
	// Cysharp.Threading.Tasks.CompilerServices.IStateMachineRunnerPromise<object>
	// Cysharp.Threading.Tasks.ITaskPoolNode<object>
	// Cysharp.Threading.Tasks.IUniTaskSource<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>>>>>
	// Cysharp.Threading.Tasks.IUniTaskSource<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>>>>
	// Cysharp.Threading.Tasks.IUniTaskSource<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>>>
	// Cysharp.Threading.Tasks.IUniTaskSource<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>>
	// Cysharp.Threading.Tasks.IUniTaskSource<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>
	// Cysharp.Threading.Tasks.IUniTaskSource<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>
	// Cysharp.Threading.Tasks.IUniTaskSource<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>
	// Cysharp.Threading.Tasks.IUniTaskSource<System.ValueTuple<byte,System.ValueTuple<byte,object>>>
	// Cysharp.Threading.Tasks.IUniTaskSource<System.ValueTuple<byte,object>>
	// Cysharp.Threading.Tasks.IUniTaskSource<object>
	// Cysharp.Threading.Tasks.UniTask.Awaiter<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>>>>>
	// Cysharp.Threading.Tasks.UniTask.Awaiter<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>>>>
	// Cysharp.Threading.Tasks.UniTask.Awaiter<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>>>
	// Cysharp.Threading.Tasks.UniTask.Awaiter<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>>
	// Cysharp.Threading.Tasks.UniTask.Awaiter<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>
	// Cysharp.Threading.Tasks.UniTask.Awaiter<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>
	// Cysharp.Threading.Tasks.UniTask.Awaiter<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>
	// Cysharp.Threading.Tasks.UniTask.Awaiter<System.ValueTuple<byte,System.ValueTuple<byte,object>>>
	// Cysharp.Threading.Tasks.UniTask.Awaiter<System.ValueTuple<byte,object>>
	// Cysharp.Threading.Tasks.UniTask.Awaiter<object>
	// Cysharp.Threading.Tasks.UniTask.IsCanceledSource<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>>>>>
	// Cysharp.Threading.Tasks.UniTask.IsCanceledSource<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>>>>
	// Cysharp.Threading.Tasks.UniTask.IsCanceledSource<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>>>
	// Cysharp.Threading.Tasks.UniTask.IsCanceledSource<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>>
	// Cysharp.Threading.Tasks.UniTask.IsCanceledSource<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>
	// Cysharp.Threading.Tasks.UniTask.IsCanceledSource<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>
	// Cysharp.Threading.Tasks.UniTask.IsCanceledSource<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>
	// Cysharp.Threading.Tasks.UniTask.IsCanceledSource<System.ValueTuple<byte,System.ValueTuple<byte,object>>>
	// Cysharp.Threading.Tasks.UniTask.IsCanceledSource<System.ValueTuple<byte,object>>
	// Cysharp.Threading.Tasks.UniTask.IsCanceledSource<object>
	// Cysharp.Threading.Tasks.UniTask.MemoizeSource<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>>>>>
	// Cysharp.Threading.Tasks.UniTask.MemoizeSource<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>>>>
	// Cysharp.Threading.Tasks.UniTask.MemoizeSource<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>>>
	// Cysharp.Threading.Tasks.UniTask.MemoizeSource<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>>
	// Cysharp.Threading.Tasks.UniTask.MemoizeSource<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>
	// Cysharp.Threading.Tasks.UniTask.MemoizeSource<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>
	// Cysharp.Threading.Tasks.UniTask.MemoizeSource<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>
	// Cysharp.Threading.Tasks.UniTask.MemoizeSource<System.ValueTuple<byte,System.ValueTuple<byte,object>>>
	// Cysharp.Threading.Tasks.UniTask.MemoizeSource<System.ValueTuple<byte,object>>
	// Cysharp.Threading.Tasks.UniTask.MemoizeSource<object>
	// Cysharp.Threading.Tasks.UniTask<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>>>>>>
	// Cysharp.Threading.Tasks.UniTask<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>>>>>
	// Cysharp.Threading.Tasks.UniTask<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>>>>
	// Cysharp.Threading.Tasks.UniTask<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>>>
	// Cysharp.Threading.Tasks.UniTask<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>>
	// Cysharp.Threading.Tasks.UniTask<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>
	// Cysharp.Threading.Tasks.UniTask<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>
	// Cysharp.Threading.Tasks.UniTask<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>
	// Cysharp.Threading.Tasks.UniTask<System.ValueTuple<byte,System.ValueTuple<byte,object>>>
	// Cysharp.Threading.Tasks.UniTask<System.ValueTuple<byte,object>>
	// Cysharp.Threading.Tasks.UniTask<object>
	// DG.Tweening.Core.DOGetter<UnityEngine.Color>
	// DG.Tweening.Core.DOGetter<UnityEngine.Vector2>
	// DG.Tweening.Core.DOGetter<UnityEngine.Vector3>
	// DG.Tweening.Core.DOGetter<float>
	// DG.Tweening.Core.DOGetter<int>
	// DG.Tweening.Core.DOGetter<object>
	// DG.Tweening.Core.DOSetter<UnityEngine.Color>
	// DG.Tweening.Core.DOSetter<UnityEngine.Vector2>
	// DG.Tweening.Core.DOSetter<UnityEngine.Vector3>
	// DG.Tweening.Core.DOSetter<float>
	// DG.Tweening.Core.DOSetter<int>
	// DG.Tweening.Core.DOSetter<object>
	// DG.Tweening.Core.TweenerCore<UnityEngine.Vector3,UnityEngine.Vector3,DG.Tweening.Plugins.SpiralOptions>
	// DG.Tweening.Plugins.Core.ABSTweenPlugin<DG.Tweening.Color2,DG.Tweening.Color2,DG.Tweening.Plugins.Options.ColorOptions>
	// DG.Tweening.Plugins.Core.ABSTweenPlugin<UnityEngine.Color,UnityEngine.Color,DG.Tweening.Plugins.Options.ColorOptions>
	// DG.Tweening.Plugins.Core.ABSTweenPlugin<UnityEngine.Quaternion,UnityEngine.Vector3,DG.Tweening.Plugins.Options.QuaternionOptions>
	// DG.Tweening.Plugins.Core.ABSTweenPlugin<UnityEngine.Rect,UnityEngine.Rect,DG.Tweening.Plugins.Options.RectOptions>
	// DG.Tweening.Plugins.Core.ABSTweenPlugin<UnityEngine.Vector2,UnityEngine.Vector2,DG.Tweening.Plugins.Options.VectorOptions>
	// DG.Tweening.Plugins.Core.ABSTweenPlugin<UnityEngine.Vector3,UnityEngine.Vector3,DG.Tweening.Plugins.Options.VectorOptions>
	// DG.Tweening.Plugins.Core.ABSTweenPlugin<UnityEngine.Vector3,UnityEngine.Vector3,DG.Tweening.Plugins.SpiralOptions>
	// DG.Tweening.Plugins.Core.ABSTweenPlugin<UnityEngine.Vector3,object,DG.Tweening.Plugins.Options.Vector3ArrayOptions>
	// DG.Tweening.Plugins.Core.ABSTweenPlugin<UnityEngine.Vector4,UnityEngine.Vector4,DG.Tweening.Plugins.Options.VectorOptions>
	// DG.Tweening.Plugins.Core.ABSTweenPlugin<double,double,DG.Tweening.Plugins.Options.NoOptions>
	// DG.Tweening.Plugins.Core.ABSTweenPlugin<float,float,DG.Tweening.Plugins.Options.FloatOptions>
	// DG.Tweening.Plugins.Core.ABSTweenPlugin<int,int,DG.Tweening.Plugins.Options.NoOptions>
	// DG.Tweening.Plugins.Core.ABSTweenPlugin<long,long,DG.Tweening.Plugins.Options.NoOptions>
	// DG.Tweening.Plugins.Core.ABSTweenPlugin<object,object,DG.Tweening.Plugins.Options.NoOptions>
	// DG.Tweening.Plugins.Core.ABSTweenPlugin<object,object,DG.Tweening.Plugins.Options.StringOptions>
	// DG.Tweening.Plugins.Core.ABSTweenPlugin<uint,uint,DG.Tweening.Plugins.Options.UintOptions>
	// DG.Tweening.Plugins.Core.ABSTweenPlugin<ulong,ulong,DG.Tweening.Plugins.Options.NoOptions>
	// DragonU3DSDK.Network.API.APIManager.<>c__DisplayClass35_0<object,object>
	// DragonU3DSDK.Network.API.APIManager.<>c__DisplayClass35_1<object,object>
	// DragonU3DSDK.Network.API.APIManager.<send>d__35<object,object>
	// DragonU3DSDK.Storage.StorageDictionary<int,byte>
	// DragonU3DSDK.Storage.StorageDictionary<int,int>
	// DragonU3DSDK.Storage.StorageDictionary<int,long>
	// DragonU3DSDK.Storage.StorageDictionary<int,object>
	// DragonU3DSDK.Storage.StorageDictionary<int,ulong>
	// DragonU3DSDK.Storage.StorageDictionary<object,byte>
	// DragonU3DSDK.Storage.StorageDictionary<object,int>
	// DragonU3DSDK.Storage.StorageDictionary<object,object>
	// DragonU3DSDK.Storage.StorageList<int>
	// DragonU3DSDK.Storage.StorageList<long>
	// DragonU3DSDK.Storage.StorageList<object>
	// DragonU3DSDK.Subjects.Subject<object>
	// Manager<object>
	// ProtoBuf.Serializers.IFactory<object>
	// ProtoBuf.Serializers.IRepeatedSerializer<object>
	// ProtoBuf.Serializers.ISerializer<object>
	// Spine.ExposedList.Enumerator<object>
	// Spine.ExposedList<object>
	// System.Action<DG.Tweening.Plugins.Options.PathOptions,object,UnityEngine.Quaternion,object>
	// System.Action<FrameData>
	// System.Action<Loom.DelayedQueueItem>
	// System.Action<OneLine.Pixel>
	// System.Action<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Action<System.Collections.Generic.KeyValuePair<ulong,object>>
	// System.Action<TMatchRewardItemData>
	// System.Action<UnityEngine.EventSystems.RaycastResult>
	// System.Action<UnityEngine.Rect>
	// System.Action<UnityEngine.Vector2>
	// System.Action<UnityEngine.Vector3>
	// System.Action<byte,byte>
	// System.Action<byte,object,object,int>
	// System.Action<byte,object,object>
	// System.Action<byte,object>
	// System.Action<byte>
	// System.Action<float>
	// System.Action<int,UnityEngine.Vector2>
	// System.Action<int,int>
	// System.Action<int,object,object>
	// System.Action<int>
	// System.Action<long>
	// System.Action<object,UnityEngine.Color>
	// System.Action<object,object,object>
	// System.Action<object,object>
	// System.Action<object>
	// System.Action<ulong>
	// System.Buffers.ArrayPool<int>
	// System.Buffers.TlsOverPerCoreLockedStacksArrayPool.LockedStack<int>
	// System.Buffers.TlsOverPerCoreLockedStacksArrayPool.PerCoreLockedStacks<int>
	// System.Buffers.TlsOverPerCoreLockedStacksArrayPool<int>
	// System.ByReference<int>
	// System.Collections.Generic.ArraySortHelper<FrameData>
	// System.Collections.Generic.ArraySortHelper<Loom.DelayedQueueItem>
	// System.Collections.Generic.ArraySortHelper<OneLine.Pixel>
	// System.Collections.Generic.ArraySortHelper<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.ArraySortHelper<System.Collections.Generic.KeyValuePair<ulong,object>>
	// System.Collections.Generic.ArraySortHelper<TMatchRewardItemData>
	// System.Collections.Generic.ArraySortHelper<UnityEngine.EventSystems.RaycastResult>
	// System.Collections.Generic.ArraySortHelper<UnityEngine.Rect>
	// System.Collections.Generic.ArraySortHelper<UnityEngine.Vector2>
	// System.Collections.Generic.ArraySortHelper<UnityEngine.Vector3>
	// System.Collections.Generic.ArraySortHelper<byte>
	// System.Collections.Generic.ArraySortHelper<float>
	// System.Collections.Generic.ArraySortHelper<int>
	// System.Collections.Generic.ArraySortHelper<long>
	// System.Collections.Generic.ArraySortHelper<object,object>
	// System.Collections.Generic.ArraySortHelper<object>
	// System.Collections.Generic.ArraySortHelper<ulong>
	// System.Collections.Generic.Comparer<FrameData>
	// System.Collections.Generic.Comparer<Loom.DelayedQueueItem>
	// System.Collections.Generic.Comparer<OneLine.Pixel>
	// System.Collections.Generic.Comparer<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.Comparer<System.Collections.Generic.KeyValuePair<ulong,object>>
	// System.Collections.Generic.Comparer<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>>>>
	// System.Collections.Generic.Comparer<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>>>
	// System.Collections.Generic.Comparer<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>>
	// System.Collections.Generic.Comparer<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>
	// System.Collections.Generic.Comparer<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>
	// System.Collections.Generic.Comparer<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>
	// System.Collections.Generic.Comparer<System.ValueTuple<byte,System.ValueTuple<byte,object>>>
	// System.Collections.Generic.Comparer<System.ValueTuple<byte,object>>
	// System.Collections.Generic.Comparer<TMatchRewardItemData>
	// System.Collections.Generic.Comparer<UnityEngine.EventSystems.RaycastResult>
	// System.Collections.Generic.Comparer<UnityEngine.Rect>
	// System.Collections.Generic.Comparer<UnityEngine.Vector2>
	// System.Collections.Generic.Comparer<UnityEngine.Vector3>
	// System.Collections.Generic.Comparer<byte>
	// System.Collections.Generic.Comparer<float>
	// System.Collections.Generic.Comparer<int>
	// System.Collections.Generic.Comparer<long>
	// System.Collections.Generic.Comparer<object>
	// System.Collections.Generic.Comparer<ulong>
	// System.Collections.Generic.Dictionary.Enumerator<int,byte>
	// System.Collections.Generic.Dictionary.Enumerator<int,int>
	// System.Collections.Generic.Dictionary.Enumerator<int,long>
	// System.Collections.Generic.Dictionary.Enumerator<int,object>
	// System.Collections.Generic.Dictionary.Enumerator<int,ulong>
	// System.Collections.Generic.Dictionary.Enumerator<long,byte>
	// System.Collections.Generic.Dictionary.Enumerator<long,int>
	// System.Collections.Generic.Dictionary.Enumerator<long,object>
	// System.Collections.Generic.Dictionary.Enumerator<object,UnityEngine.Quaternion>
	// System.Collections.Generic.Dictionary.Enumerator<object,UnityEngine.Vector3>
	// System.Collections.Generic.Dictionary.Enumerator<object,byte>
	// System.Collections.Generic.Dictionary.Enumerator<object,float>
	// System.Collections.Generic.Dictionary.Enumerator<object,int>
	// System.Collections.Generic.Dictionary.Enumerator<object,object>
	// System.Collections.Generic.Dictionary.Enumerator<ulong,byte>
	// System.Collections.Generic.Dictionary.Enumerator<ulong,int>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<int,byte>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<int,int>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<int,long>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<int,object>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<int,ulong>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<long,byte>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<long,int>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<long,object>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<object,UnityEngine.Quaternion>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<object,UnityEngine.Vector3>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<object,byte>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<object,float>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<object,int>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<object,object>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<ulong,byte>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<ulong,int>
	// System.Collections.Generic.Dictionary.KeyCollection<int,byte>
	// System.Collections.Generic.Dictionary.KeyCollection<int,int>
	// System.Collections.Generic.Dictionary.KeyCollection<int,long>
	// System.Collections.Generic.Dictionary.KeyCollection<int,object>
	// System.Collections.Generic.Dictionary.KeyCollection<int,ulong>
	// System.Collections.Generic.Dictionary.KeyCollection<long,byte>
	// System.Collections.Generic.Dictionary.KeyCollection<long,int>
	// System.Collections.Generic.Dictionary.KeyCollection<long,object>
	// System.Collections.Generic.Dictionary.KeyCollection<object,UnityEngine.Quaternion>
	// System.Collections.Generic.Dictionary.KeyCollection<object,UnityEngine.Vector3>
	// System.Collections.Generic.Dictionary.KeyCollection<object,byte>
	// System.Collections.Generic.Dictionary.KeyCollection<object,float>
	// System.Collections.Generic.Dictionary.KeyCollection<object,int>
	// System.Collections.Generic.Dictionary.KeyCollection<object,object>
	// System.Collections.Generic.Dictionary.KeyCollection<ulong,byte>
	// System.Collections.Generic.Dictionary.KeyCollection<ulong,int>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<int,byte>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<int,int>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<int,long>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<int,object>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<int,ulong>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<long,byte>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<long,int>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<long,object>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<object,UnityEngine.Quaternion>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<object,UnityEngine.Vector3>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<object,byte>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<object,float>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<object,int>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<object,object>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<ulong,byte>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<ulong,int>
	// System.Collections.Generic.Dictionary.ValueCollection<int,byte>
	// System.Collections.Generic.Dictionary.ValueCollection<int,int>
	// System.Collections.Generic.Dictionary.ValueCollection<int,long>
	// System.Collections.Generic.Dictionary.ValueCollection<int,object>
	// System.Collections.Generic.Dictionary.ValueCollection<int,ulong>
	// System.Collections.Generic.Dictionary.ValueCollection<long,byte>
	// System.Collections.Generic.Dictionary.ValueCollection<long,int>
	// System.Collections.Generic.Dictionary.ValueCollection<long,object>
	// System.Collections.Generic.Dictionary.ValueCollection<object,UnityEngine.Quaternion>
	// System.Collections.Generic.Dictionary.ValueCollection<object,UnityEngine.Vector3>
	// System.Collections.Generic.Dictionary.ValueCollection<object,byte>
	// System.Collections.Generic.Dictionary.ValueCollection<object,float>
	// System.Collections.Generic.Dictionary.ValueCollection<object,int>
	// System.Collections.Generic.Dictionary.ValueCollection<object,object>
	// System.Collections.Generic.Dictionary.ValueCollection<ulong,byte>
	// System.Collections.Generic.Dictionary.ValueCollection<ulong,int>
	// System.Collections.Generic.Dictionary<int,byte>
	// System.Collections.Generic.Dictionary<int,int>
	// System.Collections.Generic.Dictionary<int,long>
	// System.Collections.Generic.Dictionary<int,object>
	// System.Collections.Generic.Dictionary<int,ulong>
	// System.Collections.Generic.Dictionary<long,byte>
	// System.Collections.Generic.Dictionary<long,int>
	// System.Collections.Generic.Dictionary<long,object>
	// System.Collections.Generic.Dictionary<object,UnityEngine.Quaternion>
	// System.Collections.Generic.Dictionary<object,UnityEngine.Vector3>
	// System.Collections.Generic.Dictionary<object,byte>
	// System.Collections.Generic.Dictionary<object,float>
	// System.Collections.Generic.Dictionary<object,int>
	// System.Collections.Generic.Dictionary<object,object>
	// System.Collections.Generic.Dictionary<ulong,byte>
	// System.Collections.Generic.Dictionary<ulong,int>
	// System.Collections.Generic.EqualityComparer<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>>>>
	// System.Collections.Generic.EqualityComparer<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>>>
	// System.Collections.Generic.EqualityComparer<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>>
	// System.Collections.Generic.EqualityComparer<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>
	// System.Collections.Generic.EqualityComparer<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>
	// System.Collections.Generic.EqualityComparer<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>
	// System.Collections.Generic.EqualityComparer<System.ValueTuple<byte,System.ValueTuple<byte,object>>>
	// System.Collections.Generic.EqualityComparer<System.ValueTuple<byte,object>>
	// System.Collections.Generic.EqualityComparer<UnityEngine.Quaternion>
	// System.Collections.Generic.EqualityComparer<UnityEngine.Vector3>
	// System.Collections.Generic.EqualityComparer<byte>
	// System.Collections.Generic.EqualityComparer<float>
	// System.Collections.Generic.EqualityComparer<int>
	// System.Collections.Generic.EqualityComparer<long>
	// System.Collections.Generic.EqualityComparer<object>
	// System.Collections.Generic.EqualityComparer<ulong>
	// System.Collections.Generic.HashSet.Enumerator<int>
	// System.Collections.Generic.HashSet.Enumerator<object>
	// System.Collections.Generic.HashSet<int>
	// System.Collections.Generic.HashSet<object>
	// System.Collections.Generic.HashSetEqualityComparer<int>
	// System.Collections.Generic.HashSetEqualityComparer<object>
	// System.Collections.Generic.ICollection<FrameData>
	// System.Collections.Generic.ICollection<Loom.DelayedQueueItem>
	// System.Collections.Generic.ICollection<OneLine.Pixel>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<int,byte>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<int,int>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<int,long>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<int,ulong>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<long,byte>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<long,int>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<long,object>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<object,UnityEngine.Quaternion>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<object,UnityEngine.Vector3>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<object,byte>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<object,float>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<object,int>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<ulong,byte>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<ulong,int>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<ulong,object>>
	// System.Collections.Generic.ICollection<TMatchRewardItemData>
	// System.Collections.Generic.ICollection<UnityEngine.Color>
	// System.Collections.Generic.ICollection<UnityEngine.EventSystems.RaycastResult>
	// System.Collections.Generic.ICollection<UnityEngine.Rect>
	// System.Collections.Generic.ICollection<UnityEngine.Vector2>
	// System.Collections.Generic.ICollection<UnityEngine.Vector3>
	// System.Collections.Generic.ICollection<byte>
	// System.Collections.Generic.ICollection<float>
	// System.Collections.Generic.ICollection<int>
	// System.Collections.Generic.ICollection<long>
	// System.Collections.Generic.ICollection<object>
	// System.Collections.Generic.ICollection<ulong>
	// System.Collections.Generic.IComparer<FrameData>
	// System.Collections.Generic.IComparer<Loom.DelayedQueueItem>
	// System.Collections.Generic.IComparer<OneLine.Pixel>
	// System.Collections.Generic.IComparer<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.IComparer<System.Collections.Generic.KeyValuePair<ulong,object>>
	// System.Collections.Generic.IComparer<TMatchRewardItemData>
	// System.Collections.Generic.IComparer<UnityEngine.EventSystems.RaycastResult>
	// System.Collections.Generic.IComparer<UnityEngine.Rect>
	// System.Collections.Generic.IComparer<UnityEngine.Vector2>
	// System.Collections.Generic.IComparer<UnityEngine.Vector3>
	// System.Collections.Generic.IComparer<byte>
	// System.Collections.Generic.IComparer<float>
	// System.Collections.Generic.IComparer<int>
	// System.Collections.Generic.IComparer<long>
	// System.Collections.Generic.IComparer<object>
	// System.Collections.Generic.IComparer<ulong>
	// System.Collections.Generic.IDictionary<object,object>
	// System.Collections.Generic.IEnumerable<FrameData>
	// System.Collections.Generic.IEnumerable<Loom.DelayedQueueItem>
	// System.Collections.Generic.IEnumerable<OneLine.Pixel>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<System.UIntPtr,object>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<int,byte>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<int,int>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<int,long>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<int,ulong>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<long,byte>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<long,int>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<long,object>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<object,UnityEngine.Quaternion>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<object,UnityEngine.Vector3>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<object,byte>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<object,float>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<object,int>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<ulong,byte>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<ulong,int>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<ulong,object>>
	// System.Collections.Generic.IEnumerable<TMatchRewardItemData>
	// System.Collections.Generic.IEnumerable<UnityEngine.Color>
	// System.Collections.Generic.IEnumerable<UnityEngine.EventSystems.RaycastResult>
	// System.Collections.Generic.IEnumerable<UnityEngine.Rect>
	// System.Collections.Generic.IEnumerable<UnityEngine.Vector2>
	// System.Collections.Generic.IEnumerable<UnityEngine.Vector3>
	// System.Collections.Generic.IEnumerable<byte>
	// System.Collections.Generic.IEnumerable<float>
	// System.Collections.Generic.IEnumerable<int>
	// System.Collections.Generic.IEnumerable<long>
	// System.Collections.Generic.IEnumerable<object>
	// System.Collections.Generic.IEnumerable<ulong>
	// System.Collections.Generic.IEnumerator<FrameData>
	// System.Collections.Generic.IEnumerator<Loom.DelayedQueueItem>
	// System.Collections.Generic.IEnumerator<OneLine.Pixel>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<System.UIntPtr,object>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<int,byte>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<int,int>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<int,long>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<int,ulong>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<long,byte>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<long,int>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<long,object>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<object,UnityEngine.Quaternion>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<object,UnityEngine.Vector3>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<object,byte>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<object,float>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<object,int>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<ulong,byte>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<ulong,int>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<ulong,object>>
	// System.Collections.Generic.IEnumerator<TMatchRewardItemData>
	// System.Collections.Generic.IEnumerator<UnityEngine.Color>
	// System.Collections.Generic.IEnumerator<UnityEngine.EventSystems.RaycastResult>
	// System.Collections.Generic.IEnumerator<UnityEngine.Rect>
	// System.Collections.Generic.IEnumerator<UnityEngine.Vector2>
	// System.Collections.Generic.IEnumerator<UnityEngine.Vector3>
	// System.Collections.Generic.IEnumerator<byte>
	// System.Collections.Generic.IEnumerator<float>
	// System.Collections.Generic.IEnumerator<int>
	// System.Collections.Generic.IEnumerator<long>
	// System.Collections.Generic.IEnumerator<object>
	// System.Collections.Generic.IEnumerator<ulong>
	// System.Collections.Generic.IEqualityComparer<int>
	// System.Collections.Generic.IEqualityComparer<long>
	// System.Collections.Generic.IEqualityComparer<object>
	// System.Collections.Generic.IEqualityComparer<ulong>
	// System.Collections.Generic.IList<FrameData>
	// System.Collections.Generic.IList<Loom.DelayedQueueItem>
	// System.Collections.Generic.IList<OneLine.Pixel>
	// System.Collections.Generic.IList<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.IList<System.Collections.Generic.KeyValuePair<ulong,object>>
	// System.Collections.Generic.IList<System.Text.Json.JsonElement>
	// System.Collections.Generic.IList<TMatchRewardItemData>
	// System.Collections.Generic.IList<UnityEngine.EventSystems.RaycastResult>
	// System.Collections.Generic.IList<UnityEngine.Rect>
	// System.Collections.Generic.IList<UnityEngine.Vector2>
	// System.Collections.Generic.IList<UnityEngine.Vector3>
	// System.Collections.Generic.IList<byte>
	// System.Collections.Generic.IList<float>
	// System.Collections.Generic.IList<int>
	// System.Collections.Generic.IList<long>
	// System.Collections.Generic.IList<object>
	// System.Collections.Generic.IList<ulong>
	// System.Collections.Generic.IReadOnlyCollection<UnityEngine.Color>
	// System.Collections.Generic.IReadOnlyCollection<object>
	// System.Collections.Generic.IReadOnlyList<UnityEngine.Color>
	// System.Collections.Generic.IReadOnlyList<object>
	// System.Collections.Generic.KeyValuePair<System.UIntPtr,object>
	// System.Collections.Generic.KeyValuePair<int,byte>
	// System.Collections.Generic.KeyValuePair<int,int>
	// System.Collections.Generic.KeyValuePair<int,long>
	// System.Collections.Generic.KeyValuePair<int,object>
	// System.Collections.Generic.KeyValuePair<int,ulong>
	// System.Collections.Generic.KeyValuePair<long,byte>
	// System.Collections.Generic.KeyValuePair<long,int>
	// System.Collections.Generic.KeyValuePair<long,object>
	// System.Collections.Generic.KeyValuePair<object,UnityEngine.Quaternion>
	// System.Collections.Generic.KeyValuePair<object,UnityEngine.Vector3>
	// System.Collections.Generic.KeyValuePair<object,byte>
	// System.Collections.Generic.KeyValuePair<object,float>
	// System.Collections.Generic.KeyValuePair<object,int>
	// System.Collections.Generic.KeyValuePair<object,object>
	// System.Collections.Generic.KeyValuePair<ulong,byte>
	// System.Collections.Generic.KeyValuePair<ulong,int>
	// System.Collections.Generic.KeyValuePair<ulong,object>
	// System.Collections.Generic.LinkedList.Enumerator<object>
	// System.Collections.Generic.LinkedList<object>
	// System.Collections.Generic.LinkedListNode<object>
	// System.Collections.Generic.List.Enumerator<FrameData>
	// System.Collections.Generic.List.Enumerator<Loom.DelayedQueueItem>
	// System.Collections.Generic.List.Enumerator<OneLine.Pixel>
	// System.Collections.Generic.List.Enumerator<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.List.Enumerator<System.Collections.Generic.KeyValuePair<ulong,object>>
	// System.Collections.Generic.List.Enumerator<TMatchRewardItemData>
	// System.Collections.Generic.List.Enumerator<UnityEngine.EventSystems.RaycastResult>
	// System.Collections.Generic.List.Enumerator<UnityEngine.Rect>
	// System.Collections.Generic.List.Enumerator<UnityEngine.Vector2>
	// System.Collections.Generic.List.Enumerator<UnityEngine.Vector3>
	// System.Collections.Generic.List.Enumerator<byte>
	// System.Collections.Generic.List.Enumerator<float>
	// System.Collections.Generic.List.Enumerator<int>
	// System.Collections.Generic.List.Enumerator<long>
	// System.Collections.Generic.List.Enumerator<object>
	// System.Collections.Generic.List.Enumerator<ulong>
	// System.Collections.Generic.List<FrameData>
	// System.Collections.Generic.List<Loom.DelayedQueueItem>
	// System.Collections.Generic.List<OneLine.Pixel>
	// System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<ulong,object>>
	// System.Collections.Generic.List<TMatchRewardItemData>
	// System.Collections.Generic.List<UnityEngine.EventSystems.RaycastResult>
	// System.Collections.Generic.List<UnityEngine.Rect>
	// System.Collections.Generic.List<UnityEngine.Vector2>
	// System.Collections.Generic.List<UnityEngine.Vector3>
	// System.Collections.Generic.List<byte>
	// System.Collections.Generic.List<float>
	// System.Collections.Generic.List<int>
	// System.Collections.Generic.List<long>
	// System.Collections.Generic.List<object>
	// System.Collections.Generic.List<ulong>
	// System.Collections.Generic.ObjectComparer<FrameData>
	// System.Collections.Generic.ObjectComparer<Loom.DelayedQueueItem>
	// System.Collections.Generic.ObjectComparer<OneLine.Pixel>
	// System.Collections.Generic.ObjectComparer<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.ObjectComparer<System.Collections.Generic.KeyValuePair<ulong,object>>
	// System.Collections.Generic.ObjectComparer<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>>>
	// System.Collections.Generic.ObjectComparer<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>>
	// System.Collections.Generic.ObjectComparer<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>
	// System.Collections.Generic.ObjectComparer<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>
	// System.Collections.Generic.ObjectComparer<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>
	// System.Collections.Generic.ObjectComparer<System.ValueTuple<byte,System.ValueTuple<byte,object>>>
	// System.Collections.Generic.ObjectComparer<System.ValueTuple<byte,object>>
	// System.Collections.Generic.ObjectComparer<TMatchRewardItemData>
	// System.Collections.Generic.ObjectComparer<UnityEngine.EventSystems.RaycastResult>
	// System.Collections.Generic.ObjectComparer<UnityEngine.Rect>
	// System.Collections.Generic.ObjectComparer<UnityEngine.Vector2>
	// System.Collections.Generic.ObjectComparer<UnityEngine.Vector3>
	// System.Collections.Generic.ObjectComparer<byte>
	// System.Collections.Generic.ObjectComparer<float>
	// System.Collections.Generic.ObjectComparer<int>
	// System.Collections.Generic.ObjectComparer<long>
	// System.Collections.Generic.ObjectComparer<object>
	// System.Collections.Generic.ObjectComparer<ulong>
	// System.Collections.Generic.ObjectEqualityComparer<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>>>
	// System.Collections.Generic.ObjectEqualityComparer<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>>
	// System.Collections.Generic.ObjectEqualityComparer<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>
	// System.Collections.Generic.ObjectEqualityComparer<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>
	// System.Collections.Generic.ObjectEqualityComparer<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>
	// System.Collections.Generic.ObjectEqualityComparer<System.ValueTuple<byte,System.ValueTuple<byte,object>>>
	// System.Collections.Generic.ObjectEqualityComparer<System.ValueTuple<byte,object>>
	// System.Collections.Generic.ObjectEqualityComparer<UnityEngine.Quaternion>
	// System.Collections.Generic.ObjectEqualityComparer<UnityEngine.Vector3>
	// System.Collections.Generic.ObjectEqualityComparer<byte>
	// System.Collections.Generic.ObjectEqualityComparer<float>
	// System.Collections.Generic.ObjectEqualityComparer<int>
	// System.Collections.Generic.ObjectEqualityComparer<long>
	// System.Collections.Generic.ObjectEqualityComparer<object>
	// System.Collections.Generic.ObjectEqualityComparer<ulong>
	// System.Collections.Generic.Queue.Enumerator<object>
	// System.Collections.Generic.Queue<object>
	// System.Collections.Generic.SortedDictionary.<>c__DisplayClass34_0<ulong,object>
	// System.Collections.Generic.SortedDictionary.<>c__DisplayClass34_1<ulong,object>
	// System.Collections.Generic.SortedDictionary.Enumerator<ulong,object>
	// System.Collections.Generic.SortedDictionary.KeyCollection.<>c__DisplayClass5_0<ulong,object>
	// System.Collections.Generic.SortedDictionary.KeyCollection.<>c__DisplayClass6_0<ulong,object>
	// System.Collections.Generic.SortedDictionary.KeyCollection.Enumerator<ulong,object>
	// System.Collections.Generic.SortedDictionary.KeyCollection<ulong,object>
	// System.Collections.Generic.SortedDictionary.KeyValuePairComparer<ulong,object>
	// System.Collections.Generic.SortedDictionary.ValueCollection.<>c__DisplayClass5_0<ulong,object>
	// System.Collections.Generic.SortedDictionary.ValueCollection.<>c__DisplayClass6_0<ulong,object>
	// System.Collections.Generic.SortedDictionary.ValueCollection.Enumerator<ulong,object>
	// System.Collections.Generic.SortedDictionary.ValueCollection<ulong,object>
	// System.Collections.Generic.SortedDictionary<ulong,object>
	// System.Collections.Generic.SortedSet.<>c__DisplayClass52_0<System.Collections.Generic.KeyValuePair<ulong,object>>
	// System.Collections.Generic.SortedSet.<>c__DisplayClass53_0<System.Collections.Generic.KeyValuePair<ulong,object>>
	// System.Collections.Generic.SortedSet.Enumerator<System.Collections.Generic.KeyValuePair<ulong,object>>
	// System.Collections.Generic.SortedSet.Node<System.Collections.Generic.KeyValuePair<ulong,object>>
	// System.Collections.Generic.SortedSet.TreeSubSet.<>c__DisplayClass9_0<System.Collections.Generic.KeyValuePair<ulong,object>>
	// System.Collections.Generic.SortedSet.TreeSubSet<System.Collections.Generic.KeyValuePair<ulong,object>>
	// System.Collections.Generic.SortedSet<System.Collections.Generic.KeyValuePair<ulong,object>>
	// System.Collections.Generic.Stack.Enumerator<byte>
	// System.Collections.Generic.Stack.Enumerator<object>
	// System.Collections.Generic.Stack<byte>
	// System.Collections.Generic.Stack<object>
	// System.Collections.Generic.TreeSet<System.Collections.Generic.KeyValuePair<ulong,object>>
	// System.Collections.Generic.TreeWalkPredicate<System.Collections.Generic.KeyValuePair<ulong,object>>
	// System.Collections.Generic.ValueListBuilder<int>
	// System.Collections.ObjectModel.ReadOnlyCollection<FrameData>
	// System.Collections.ObjectModel.ReadOnlyCollection<Loom.DelayedQueueItem>
	// System.Collections.ObjectModel.ReadOnlyCollection<OneLine.Pixel>
	// System.Collections.ObjectModel.ReadOnlyCollection<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.ObjectModel.ReadOnlyCollection<System.Collections.Generic.KeyValuePair<ulong,object>>
	// System.Collections.ObjectModel.ReadOnlyCollection<TMatchRewardItemData>
	// System.Collections.ObjectModel.ReadOnlyCollection<UnityEngine.EventSystems.RaycastResult>
	// System.Collections.ObjectModel.ReadOnlyCollection<UnityEngine.Rect>
	// System.Collections.ObjectModel.ReadOnlyCollection<UnityEngine.Vector2>
	// System.Collections.ObjectModel.ReadOnlyCollection<UnityEngine.Vector3>
	// System.Collections.ObjectModel.ReadOnlyCollection<byte>
	// System.Collections.ObjectModel.ReadOnlyCollection<float>
	// System.Collections.ObjectModel.ReadOnlyCollection<int>
	// System.Collections.ObjectModel.ReadOnlyCollection<long>
	// System.Collections.ObjectModel.ReadOnlyCollection<object>
	// System.Collections.ObjectModel.ReadOnlyCollection<ulong>
	// System.Comparison<FrameData>
	// System.Comparison<Loom.DelayedQueueItem>
	// System.Comparison<OneLine.Pixel>
	// System.Comparison<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Comparison<System.Collections.Generic.KeyValuePair<ulong,object>>
	// System.Comparison<TMatchRewardItemData>
	// System.Comparison<UnityEngine.EventSystems.RaycastResult>
	// System.Comparison<UnityEngine.Rect>
	// System.Comparison<UnityEngine.Vector2>
	// System.Comparison<UnityEngine.Vector3>
	// System.Comparison<byte>
	// System.Comparison<float>
	// System.Comparison<int>
	// System.Comparison<long>
	// System.Comparison<object>
	// System.Comparison<ulong>
	// System.Converter<object,int>
	// System.EventHandler<object>
	// System.Func<LobbyTaskSystem.TaskResult>
	// System.Func<Loom.DelayedQueueItem,byte>
	// System.Func<System.Collections.Generic.KeyValuePair<object,object>,object>
	// System.Func<System.Threading.Tasks.VoidTaskResult>
	// System.Func<UILoadingController.LoadingData>
	// System.Func<byte>
	// System.Func<float,byte>
	// System.Func<float>
	// System.Func<int,byte>
	// System.Func<int,float>
	// System.Func<int,object>
	// System.Func<int>
	// System.Func<object,System.Threading.Tasks.VoidTaskResult>
	// System.Func<object,byte>
	// System.Func<object,double>
	// System.Func<object,float>
	// System.Func<object,int>
	// System.Func<object,long>
	// System.Func<object,object,byte>
	// System.Func<object,object,object>
	// System.Func<object,object>
	// System.Func<object>
	// System.IComparable<object>
	// System.IEquatable<OneLine.Pixel>
	// System.Lazy<object>
	// System.Linq.Buffer<UnityEngine.Color>
	// System.Linq.Buffer<float>
	// System.Linq.Buffer<object>
	// System.Linq.Enumerable.<OfTypeIterator>d__97<object>
	// System.Linq.Enumerable.<RepeatIterator>d__117<UnityEngine.Color>
	// System.Linq.Enumerable.Iterator<Loom.DelayedQueueItem>
	// System.Linq.Enumerable.Iterator<float>
	// System.Linq.Enumerable.Iterator<int>
	// System.Linq.Enumerable.Iterator<object>
	// System.Linq.Enumerable.WhereArrayIterator<Loom.DelayedQueueItem>
	// System.Linq.Enumerable.WhereArrayIterator<object>
	// System.Linq.Enumerable.WhereEnumerableIterator<Loom.DelayedQueueItem>
	// System.Linq.Enumerable.WhereEnumerableIterator<float>
	// System.Linq.Enumerable.WhereEnumerableIterator<object>
	// System.Linq.Enumerable.WhereListIterator<Loom.DelayedQueueItem>
	// System.Linq.Enumerable.WhereListIterator<object>
	// System.Linq.Enumerable.WhereSelectArrayIterator<int,float>
	// System.Linq.Enumerable.WhereSelectArrayIterator<int,object>
	// System.Linq.Enumerable.WhereSelectArrayIterator<object,object>
	// System.Linq.Enumerable.WhereSelectEnumerableIterator<int,float>
	// System.Linq.Enumerable.WhereSelectEnumerableIterator<int,object>
	// System.Linq.Enumerable.WhereSelectEnumerableIterator<object,object>
	// System.Linq.Enumerable.WhereSelectListIterator<int,float>
	// System.Linq.Enumerable.WhereSelectListIterator<int,object>
	// System.Linq.Enumerable.WhereSelectListIterator<object,object>
	// System.Nullable<UnityEngine.Color>
	// System.Nullable<UnityEngine.Vector3>
	// System.Nullable<float>
	// System.Nullable<int>
	// System.Predicate<FrameData>
	// System.Predicate<Loom.DelayedQueueItem>
	// System.Predicate<OneLine.Pixel>
	// System.Predicate<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Predicate<System.Collections.Generic.KeyValuePair<ulong,object>>
	// System.Predicate<TMatchRewardItemData>
	// System.Predicate<UnityEngine.EventSystems.RaycastResult>
	// System.Predicate<UnityEngine.Rect>
	// System.Predicate<UnityEngine.Vector2>
	// System.Predicate<UnityEngine.Vector3>
	// System.Predicate<byte>
	// System.Predicate<float>
	// System.Predicate<int>
	// System.Predicate<long>
	// System.Predicate<object>
	// System.Predicate<ulong>
	// System.ReadOnlySpan<int>
	// System.Runtime.CompilerServices.AsyncTaskMethodBuilder<System.Threading.Tasks.VoidTaskResult>
	// System.Runtime.CompilerServices.AsyncTaskMethodBuilder<byte>
	// System.Runtime.CompilerServices.AsyncTaskMethodBuilder<object>
	// System.Runtime.CompilerServices.ConfiguredTaskAwaitable.ConfiguredTaskAwaiter<System.Threading.Tasks.VoidTaskResult>
	// System.Runtime.CompilerServices.ConfiguredTaskAwaitable.ConfiguredTaskAwaiter<byte>
	// System.Runtime.CompilerServices.ConfiguredTaskAwaitable.ConfiguredTaskAwaiter<int>
	// System.Runtime.CompilerServices.ConfiguredTaskAwaitable.ConfiguredTaskAwaiter<object>
	// System.Runtime.CompilerServices.ConfiguredTaskAwaitable<System.Threading.Tasks.VoidTaskResult>
	// System.Runtime.CompilerServices.ConfiguredTaskAwaitable<byte>
	// System.Runtime.CompilerServices.ConfiguredTaskAwaitable<int>
	// System.Runtime.CompilerServices.ConfiguredTaskAwaitable<object>
	// System.Runtime.CompilerServices.TaskAwaiter<System.Threading.Tasks.VoidTaskResult>
	// System.Runtime.CompilerServices.TaskAwaiter<byte>
	// System.Runtime.CompilerServices.TaskAwaiter<int>
	// System.Runtime.CompilerServices.TaskAwaiter<object>
	// System.Span<int>
	// System.Threading.Tasks.ContinuationTaskFromResultTask<System.Threading.Tasks.VoidTaskResult>
	// System.Threading.Tasks.ContinuationTaskFromResultTask<byte>
	// System.Threading.Tasks.ContinuationTaskFromResultTask<int>
	// System.Threading.Tasks.ContinuationTaskFromResultTask<object>
	// System.Threading.Tasks.Task<System.Threading.Tasks.VoidTaskResult>
	// System.Threading.Tasks.Task<byte>
	// System.Threading.Tasks.Task<int>
	// System.Threading.Tasks.Task<object>
	// System.Threading.Tasks.TaskFactory.<>c__DisplayClass35_0<System.Threading.Tasks.VoidTaskResult>
	// System.Threading.Tasks.TaskFactory.<>c__DisplayClass35_0<byte>
	// System.Threading.Tasks.TaskFactory.<>c__DisplayClass35_0<int>
	// System.Threading.Tasks.TaskFactory.<>c__DisplayClass35_0<object>
	// System.Threading.Tasks.TaskFactory<System.Threading.Tasks.VoidTaskResult>
	// System.Threading.Tasks.TaskFactory<byte>
	// System.Threading.Tasks.TaskFactory<int>
	// System.Threading.Tasks.TaskFactory<object>
	// System.Tuple<object,int,object,object,object>
	// System.Tuple<object,object,object>
	// System.Tuple<object,object>
	// System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>>>>>
	// System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>>>>
	// System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>>>
	// System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>>
	// System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>
	// System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>
	// System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>
	// System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>
	// System.ValueTuple<byte,System.ValueTuple<byte,object>>
	// System.ValueTuple<byte,int,int>
	// System.ValueTuple<byte,object>
	// System.ValueTuple<int,int>
	// System.ValueTuple<object,object>
	// SystemCollectionsExtension.<>c__4<object,object>
	// Unity.Collections.NativeArray.Enumerator<Unity.Mathematics.float4>
	// Unity.Collections.NativeArray.Enumerator<Unity.Mathematics.int2>
	// Unity.Collections.NativeArray.Enumerator<Unity.Mathematics.int3>
	// Unity.Collections.NativeArray.Enumerator<float>
	// Unity.Collections.NativeArray.ReadOnly.Enumerator<Unity.Mathematics.float4>
	// Unity.Collections.NativeArray.ReadOnly.Enumerator<Unity.Mathematics.int2>
	// Unity.Collections.NativeArray.ReadOnly.Enumerator<Unity.Mathematics.int3>
	// Unity.Collections.NativeArray.ReadOnly.Enumerator<float>
	// Unity.Collections.NativeArray.ReadOnly<Unity.Mathematics.float4>
	// Unity.Collections.NativeArray.ReadOnly<Unity.Mathematics.int2>
	// Unity.Collections.NativeArray.ReadOnly<Unity.Mathematics.int3>
	// Unity.Collections.NativeArray.ReadOnly<float>
	// Unity.Collections.NativeArray<Unity.Mathematics.float4>
	// Unity.Collections.NativeArray<Unity.Mathematics.int2>
	// Unity.Collections.NativeArray<Unity.Mathematics.int3>
	// Unity.Collections.NativeArray<float>
	// UnityEngine.EventSystems.ExecuteEvents.EventFunction<object>
	// UnityEngine.Events.InvokableCall<UnityEngine.Vector2>
	// UnityEngine.Events.InvokableCall<byte>
	// UnityEngine.Events.InvokableCall<float>
	// UnityEngine.Events.InvokableCall<int>
	// UnityEngine.Events.InvokableCall<object>
	// UnityEngine.Events.UnityAction<UnityEngine.Vector2>
	// UnityEngine.Events.UnityAction<byte>
	// UnityEngine.Events.UnityAction<float>
	// UnityEngine.Events.UnityAction<int>
	// UnityEngine.Events.UnityAction<object>
	// UnityEngine.Events.UnityEvent<UnityEngine.Vector2>
	// UnityEngine.Events.UnityEvent<byte>
	// UnityEngine.Events.UnityEvent<float>
	// UnityEngine.Events.UnityEvent<int>
	// UnityEngine.Events.UnityEvent<object>
	// UnityEngine.Pool.CollectionPool.<>c<object,object>
	// UnityEngine.Pool.CollectionPool<object,object>
	// }}

	public void RefMethods()
	{
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder<object>.Start<UnityGameFramework.Runtime.ResourceComponent.<LoadAssetAsync>d__91<object>>(UnityGameFramework.Runtime.ResourceComponent.<LoadAssetAsync>d__91<object>&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter<object>,DragonU3DSDK.Asset.ResourcesManager.<LoadResourceAsync>d__2<object>>(Cysharp.Threading.Tasks.UniTask.Awaiter<object>&,DragonU3DSDK.Asset.ResourcesManager.<LoadResourceAsync>d__2<object>&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.YieldAwaitable.Awaiter,ActivityResHotUpdate.<Download>d__2>(Cysharp.Threading.Tasks.YieldAwaitable.Awaiter&,ActivityResHotUpdate.<Download>d__2&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.Start<ActivityResHotUpdate.<Download>d__2>(ActivityResHotUpdate.<Download>d__2&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.Start<DragonU3DSDK.Asset.ResourcesManager.<LoadResourceAsync>d__2<object>>(DragonU3DSDK.Asset.ResourcesManager.<LoadResourceAsync>d__2<object>&)
		// DG.Tweening.Core.TweenerCore<UnityEngine.Color,UnityEngine.Color,DG.Tweening.Plugins.Options.ColorOptions> DG.Tweening.Core.Extensions.Blendable<UnityEngine.Color,UnityEngine.Color,DG.Tweening.Plugins.Options.ColorOptions>(DG.Tweening.Core.TweenerCore<UnityEngine.Color,UnityEngine.Color,DG.Tweening.Plugins.Options.ColorOptions>)
		// object DG.Tweening.Core.Extensions.SetSpecialStartupMode<object>(object,DG.Tweening.Core.Enums.SpecialStartupMode)
		// DG.Tweening.Core.TweenerCore<UnityEngine.Vector3,UnityEngine.Vector3,DG.Tweening.Plugins.SpiralOptions> DG.Tweening.Core.TweenManager.GetTweener<UnityEngine.Vector3,UnityEngine.Vector3,DG.Tweening.Plugins.SpiralOptions>()
		// DG.Tweening.Core.TweenerCore<UnityEngine.Vector3,UnityEngine.Vector3,DG.Tweening.Plugins.SpiralOptions> DG.Tweening.DOTween.ApplyTo<UnityEngine.Vector3,UnityEngine.Vector3,DG.Tweening.Plugins.SpiralOptions>(DG.Tweening.Core.DOGetter<UnityEngine.Vector3>,DG.Tweening.Core.DOSetter<UnityEngine.Vector3>,UnityEngine.Vector3,float,DG.Tweening.Plugins.Core.ABSTweenPlugin<UnityEngine.Vector3,UnityEngine.Vector3,DG.Tweening.Plugins.SpiralOptions>)
		// DG.Tweening.Core.TweenerCore<UnityEngine.Vector3,UnityEngine.Vector3,DG.Tweening.Plugins.SpiralOptions> DG.Tweening.DOTween.To<UnityEngine.Vector3,UnityEngine.Vector3,DG.Tweening.Plugins.SpiralOptions>(DG.Tweening.Plugins.Core.ABSTweenPlugin<UnityEngine.Vector3,UnityEngine.Vector3,DG.Tweening.Plugins.SpiralOptions>,DG.Tweening.Core.DOGetter<UnityEngine.Vector3>,DG.Tweening.Core.DOSetter<UnityEngine.Vector3>,UnityEngine.Vector3,float)
		// DG.Tweening.Plugins.Core.ABSTweenPlugin<UnityEngine.Vector3,UnityEngine.Vector3,DG.Tweening.Plugins.SpiralOptions> DG.Tweening.Plugins.Core.PluginsManager.GetDefaultPlugin<UnityEngine.Vector3,UnityEngine.Vector3,DG.Tweening.Plugins.SpiralOptions>()
		// object DG.Tweening.TweenExtensions.Pause<object>(object)
		// object DG.Tweening.TweenExtensions.Play<object>(object)
		// object DG.Tweening.TweenSettingsExtensions.From<object>(object)
		// object DG.Tweening.TweenSettingsExtensions.From<object>(object,bool)
		// object DG.Tweening.TweenSettingsExtensions.OnComplete<object>(object,DG.Tweening.TweenCallback)
		// object DG.Tweening.TweenSettingsExtensions.OnKill<object>(object,DG.Tweening.TweenCallback)
		// object DG.Tweening.TweenSettingsExtensions.OnPlay<object>(object,DG.Tweening.TweenCallback)
		// object DG.Tweening.TweenSettingsExtensions.OnRewind<object>(object,DG.Tweening.TweenCallback)
		// object DG.Tweening.TweenSettingsExtensions.OnStart<object>(object,DG.Tweening.TweenCallback)
		// object DG.Tweening.TweenSettingsExtensions.OnStepComplete<object>(object,DG.Tweening.TweenCallback)
		// object DG.Tweening.TweenSettingsExtensions.OnUpdate<object>(object,DG.Tweening.TweenCallback)
		// object DG.Tweening.TweenSettingsExtensions.SetAutoKill<object>(object,bool)
		// object DG.Tweening.TweenSettingsExtensions.SetDelay<object>(object,float)
		// object DG.Tweening.TweenSettingsExtensions.SetEase<object>(object,DG.Tweening.Ease)
		// object DG.Tweening.TweenSettingsExtensions.SetEase<object>(object,UnityEngine.AnimationCurve)
		// object DG.Tweening.TweenSettingsExtensions.SetId<object>(object,string)
		// object DG.Tweening.TweenSettingsExtensions.SetLoops<object>(object,int)
		// object DG.Tweening.TweenSettingsExtensions.SetLoops<object>(object,int,DG.Tweening.LoopType)
		// object DG.Tweening.TweenSettingsExtensions.SetRelative<object>(object)
		// object DG.Tweening.TweenSettingsExtensions.SetRelative<object>(object,bool)
		// object DG.Tweening.TweenSettingsExtensions.SetSpeedBased<object>(object)
		// object DG.Tweening.TweenSettingsExtensions.SetTarget<object>(object,object)
		// object DG.Tweening.TweenSettingsExtensions.SetUpdate<object>(object,bool)
		// bool DG.Tweening.Tweener.Setup<UnityEngine.Vector3,UnityEngine.Vector3,DG.Tweening.Plugins.SpiralOptions>(DG.Tweening.Core.TweenerCore<UnityEngine.Vector3,UnityEngine.Vector3,DG.Tweening.Plugins.SpiralOptions>,DG.Tweening.Core.DOGetter<UnityEngine.Vector3>,DG.Tweening.Core.DOSetter<UnityEngine.Vector3>,UnityEngine.Vector3,float,DG.Tweening.Plugins.Core.ABSTweenPlugin<UnityEngine.Vector3,UnityEngine.Vector3,DG.Tweening.Plugins.SpiralOptions>)
		// System.Void DragonU3DSDK.Network.API.APIManager.Send<object,object>(object,System.Action<object>,System.Action<DragonU3DSDK.Network.API.Protocol.ErrorCode,string,object>)
		// System.Collections.IEnumerator DragonU3DSDK.Network.API.APIManager.send<object,object>(object,System.Action<object>,System.Action<DragonU3DSDK.Network.API.Protocol.ErrorCode,string,object>)
		// object DragonU3DSDK.Network.API.ProtocolNetExtensions.Deserialize<object>(byte[])
		// object DragonU3DSDK.Storage.StorageManager.GetStorage<object>()
		// bool DragonU3DSDK.Subjects.SubjectAggregation.Subscribe<object>(DragonU3DSDK.Subjects.IEventHandler<object>)
		// object DragonU3DSDK.Subjects.SubjectAggregation.Trigger<object>()
		// bool DragonU3DSDK.Subjects.SubjectAggregation.Unsubscribe<object>(DragonU3DSDK.Subjects.IEventHandler<object>)
		// object GameFramework.Resource.IResourceManager.LoadAsset<object>(string,string)
		// object GameObject_MonoBehaviour.GetOrCreateComponent<object>(UnityEngine.GameObject)
		// BiUtil.ItemChangeReasonArgs Newtonsoft.Json.JsonConvert.DeserializeObject<BiUtil.ItemChangeReasonArgs>(string)
		// BiUtil.ItemChangeReasonArgs Newtonsoft.Json.JsonConvert.DeserializeObject<BiUtil.ItemChangeReasonArgs>(string,Newtonsoft.Json.JsonSerializerSettings)
		// PlayerExtraInfo Newtonsoft.Json.JsonConvert.DeserializeObject<PlayerExtraInfo>(string)
		// PlayerExtraInfo Newtonsoft.Json.JsonConvert.DeserializeObject<PlayerExtraInfo>(string,Newtonsoft.Json.JsonSerializerSettings)
		// PlayerRankExtraInfo Newtonsoft.Json.JsonConvert.DeserializeObject<PlayerRankExtraInfo>(string)
		// PlayerRankExtraInfo Newtonsoft.Json.JsonConvert.DeserializeObject<PlayerRankExtraInfo>(string,Newtonsoft.Json.JsonSerializerSettings)
		// TeamExtraInfo Newtonsoft.Json.JsonConvert.DeserializeObject<TeamExtraInfo>(string)
		// TeamExtraInfo Newtonsoft.Json.JsonConvert.DeserializeObject<TeamExtraInfo>(string,Newtonsoft.Json.JsonSerializerSettings)
		// object Newtonsoft.Json.JsonConvert.DeserializeObject<object>(string)
		// object Newtonsoft.Json.JsonConvert.DeserializeObject<object>(string,Newtonsoft.Json.JsonSerializerSettings)
		// byte Newtonsoft.Json.Linq.JToken.ToObject<byte>()
		// int Newtonsoft.Json.Linq.JToken.ToObject<int>()
		// object Newtonsoft.Json.Linq.JToken.ToObject<object>()
		// object ProtoBuf.Meta.TypeModel.ActivatorCreate<object>()
		// object ProtoBuf.Meta.TypeModel.CreateInstance<object>(ProtoBuf.ISerializationContext,ProtoBuf.Serializers.ISerializer<object>)
		// object ProtoBuf.Meta.TypeModel.Deserialize<object>(System.IO.Stream,object,object)
		// ProtoBuf.Serializers.ISerializer<object> ProtoBuf.Meta.TypeModel.GetSerializer<object>()
		// ProtoBuf.Serializers.ISerializer<object> ProtoBuf.Meta.TypeModel.GetSerializer<object>(ProtoBuf.Meta.TypeModel,ProtoBuf.CompatibilityLevel)
		// ProtoBuf.Serializers.ISerializer<object> ProtoBuf.Meta.TypeModel.GetSerializerCore<object>(ProtoBuf.CompatibilityLevel)
		// ProtoBuf.Serializers.ISerializer<object> ProtoBuf.Meta.TypeModel.NoSerializer<object>(ProtoBuf.Meta.TypeModel)
		// ProtoBuf.Serializers.ISerializer<object> ProtoBuf.Meta.TypeModel.TryGetSerializer<object>(ProtoBuf.Meta.TypeModel)
		// object ProtoBuf.ProtoReader.State.<ReadAsRoot>g__ReadFieldOne|102_0<object>(ProtoBuf.ProtoReader.State&,ProtoBuf.Serializers.SerializerFeatures,object,ProtoBuf.Serializers.ISerializer<object>)
		// object ProtoBuf.ProtoReader.State.CreateInstance<object>(ProtoBuf.Serializers.ISerializer<object>)
		// object ProtoBuf.ProtoReader.State.DeserializeRoot<object>(object,ProtoBuf.Serializers.ISerializer<object>)
		// object ProtoBuf.ProtoReader.State.DeserializeRootImpl<object>(object)
		// object ProtoBuf.ProtoReader.State.ReadAny<object>(ProtoBuf.Serializers.SerializerFeatures,object,ProtoBuf.Serializers.ISerializer<object>)
		// object ProtoBuf.ProtoReader.State.ReadAsRoot<object>(object,ProtoBuf.Serializers.ISerializer<object>)
		// object ProtoBuf.ProtoReader.State.ReadMessage<object,object>(ProtoBuf.Serializers.SerializerFeatures,object,object&)
		// object ProtoBuf.ProtoReader.State.ReadMessage<object>(ProtoBuf.Serializers.SerializerFeatures,object,ProtoBuf.Serializers.ISerializer<object>)
		// object SRF.Helpers.SRReflection.GetAttribute<object>(System.Reflection.MemberInfo)
		// byte SocketIOClient.JsonSerializer.IJsonSerializer.Deserialize<byte>(string,System.Collections.Generic.IList<byte[]>)
		// object SocketIOClient.JsonSerializer.IJsonSerializer.Deserialize<object>(string,System.Collections.Generic.IList<byte[]>)
		// byte SocketIOClient.SocketIOResponse.GetValue<byte>(int)
		// object SocketIOClient.SocketIOResponse.GetValue<object>(int)
		// object System.Activator.CreateInstance<object>()
		// object[] System.Array.Empty<object>()
		// int System.Array.IndexOf<object>(object[],object)
		// int System.Array.IndexOfImpl<object>(object[],object,int,int)
		// System.Void System.Array.Sort<object,object>(object[],object[],System.Collections.Generic.IComparer<object>)
		// System.Void System.Array.Sort<object,object>(object[],object[],int,int,System.Collections.Generic.IComparer<object>)
		// System.Void System.Array.Sort<object>(object[])
		// System.Void System.Array.Sort<object>(object[],int,int,System.Collections.Generic.IComparer<object>)
		// System.Collections.Generic.List<int> System.Collections.Generic.List<object>.ConvertAll<int>(System.Converter<object,int>)
		// bool System.Enum.TryParse<int>(string,bool,int&)
		// bool System.Enum.TryParse<int>(string,int&)
		// object System.Linq.Enumerable.Aggregate<object,object>(System.Collections.Generic.IEnumerable<object>,object,System.Func<object,object,object>)
		// bool System.Linq.Enumerable.All<object>(System.Collections.Generic.IEnumerable<object>,System.Func<object,bool>)
		// bool System.Linq.Enumerable.Contains<object>(System.Collections.Generic.IEnumerable<object>,object)
		// bool System.Linq.Enumerable.Contains<object>(System.Collections.Generic.IEnumerable<object>,object,System.Collections.Generic.IEqualityComparer<object>)
		// object System.Linq.Enumerable.First<object>(System.Collections.Generic.IEnumerable<object>)
		// object System.Linq.Enumerable.FirstOrDefault<object>(System.Collections.Generic.IEnumerable<object>)
		// int System.Linq.Enumerable.Last<int>(System.Collections.Generic.IEnumerable<int>)
		// object System.Linq.Enumerable.Last<object>(System.Collections.Generic.IEnumerable<object>)
		// System.Collections.Generic.IEnumerable<object> System.Linq.Enumerable.OfType<object>(System.Collections.IEnumerable)
		// System.Collections.Generic.IEnumerable<object> System.Linq.Enumerable.OfTypeIterator<object>(System.Collections.IEnumerable)
		// System.Collections.Generic.IEnumerable<UnityEngine.Color> System.Linq.Enumerable.Repeat<UnityEngine.Color>(UnityEngine.Color,int)
		// System.Collections.Generic.IEnumerable<UnityEngine.Color> System.Linq.Enumerable.RepeatIterator<UnityEngine.Color>(UnityEngine.Color,int)
		// System.Collections.Generic.IEnumerable<float> System.Linq.Enumerable.Select<int,float>(System.Collections.Generic.IEnumerable<int>,System.Func<int,float>)
		// System.Collections.Generic.IEnumerable<object> System.Linq.Enumerable.Select<int,object>(System.Collections.Generic.IEnumerable<int>,System.Func<int,object>)
		// System.Collections.Generic.IEnumerable<object> System.Linq.Enumerable.Select<object,object>(System.Collections.Generic.IEnumerable<object>,System.Func<object,object>)
		// bool System.Linq.Enumerable.SequenceEqual<object>(System.Collections.Generic.IEnumerable<object>,System.Collections.Generic.IEnumerable<object>)
		// bool System.Linq.Enumerable.SequenceEqual<object>(System.Collections.Generic.IEnumerable<object>,System.Collections.Generic.IEnumerable<object>,System.Collections.Generic.IEqualityComparer<object>)
		// UnityEngine.Color[] System.Linq.Enumerable.ToArray<UnityEngine.Color>(System.Collections.Generic.IEnumerable<UnityEngine.Color>)
		// float[] System.Linq.Enumerable.ToArray<float>(System.Collections.Generic.IEnumerable<float>)
		// object[] System.Linq.Enumerable.ToArray<object>(System.Collections.Generic.IEnumerable<object>)
		// System.Collections.Generic.List<int> System.Linq.Enumerable.ToList<int>(System.Collections.Generic.IEnumerable<int>)
		// System.Collections.Generic.List<object> System.Linq.Enumerable.ToList<object>(System.Collections.Generic.IEnumerable<object>)
		// System.Collections.Generic.IEnumerable<Loom.DelayedQueueItem> System.Linq.Enumerable.Where<Loom.DelayedQueueItem>(System.Collections.Generic.IEnumerable<Loom.DelayedQueueItem>,System.Func<Loom.DelayedQueueItem,bool>)
		// System.Collections.Generic.IEnumerable<object> System.Linq.Enumerable.Where<object>(System.Collections.Generic.IEnumerable<object>,System.Func<object,bool>)
		// System.Collections.Generic.IEnumerable<float> System.Linq.Enumerable.Iterator<int>.Select<float>(System.Func<int,float>)
		// System.Collections.Generic.IEnumerable<object> System.Linq.Enumerable.Iterator<int>.Select<object>(System.Func<int,object>)
		// System.Collections.Generic.IEnumerable<object> System.Linq.Enumerable.Iterator<object>.Select<object>(System.Func<object,object>)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,BreakEggMainPopup.<DelayToOpenRewardUI>d__31>(System.Runtime.CompilerServices.TaskAwaiter&,BreakEggMainPopup.<DelayToOpenRewardUI>d__31&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,CGVideoManager.<PlayVideo>d__30>(System.Runtime.CompilerServices.TaskAwaiter&,CGVideoManager.<PlayVideo>d__30&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,CGVideoManager.<TryStartCG>d__35>(System.Runtime.CompilerServices.TaskAwaiter&,CGVideoManager.<TryStartCG>d__35&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,CollectCardMainView.<TaskShowExchange>d__32>(System.Runtime.CompilerServices.TaskAwaiter&,CollectCardMainView.<TaskShowExchange>d__32&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,CollectCoinView.<OnViewClose>d__11>(System.Runtime.CompilerServices.TaskAwaiter&,CollectCoinView.<OnViewClose>d__11&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,CollectDiamondView.<OnViewClose>d__15>(System.Runtime.CompilerServices.TaskAwaiter&,CollectDiamondView.<OnViewClose>d__15&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,CommonUtils.<PlayAnimationAsync>d__100>(System.Runtime.CompilerServices.TaskAwaiter&,CommonUtils.<PlayAnimationAsync>d__100&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,ContactUsController.<OnViewClose>d__54>(System.Runtime.CompilerServices.TaskAwaiter&,ContactUsController.<OnViewClose>d__54&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,DragonPlus.PrivacyModel.<TryToShowPrivacyAsync>d__8>(System.Runtime.CompilerServices.TaskAwaiter&,DragonPlus.PrivacyModel.<TryToShowPrivacyAsync>d__8&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,EndlessGiftPackMainItem.<DoDiappear>d__25>(System.Runtime.CompilerServices.TaskAwaiter&,EndlessGiftPackMainItem.<DoDiappear>d__25&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,EndlessGiftPackMainPopup.<ClaimedSuccess>d__14>(System.Runtime.CompilerServices.TaskAwaiter&,EndlessGiftPackMainPopup.<ClaimedSuccess>d__14&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,GuideArrowView.<OnViewClose>d__5>(System.Runtime.CompilerServices.TaskAwaiter&,GuideArrowView.<OnViewClose>d__5&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,GuideFingerView.<OnViewClose>d__4>(System.Runtime.CompilerServices.TaskAwaiter&,GuideFingerView.<OnViewClose>d__4&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,LoadingTransitionController.<OnViewClose>d__16>(System.Runtime.CompilerServices.TaskAwaiter&,LoadingTransitionController.<OnViewClose>d__16&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,LoginController.<OnViewClose>d__19>(System.Runtime.CompilerServices.TaskAwaiter&,LoginController.<OnViewClose>d__19&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,OneVOneDuelMain.<RefreshAniHeads>d__55>(System.Runtime.CompilerServices.TaskAwaiter&,OneVOneDuelMain.<RefreshAniHeads>d__55&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,PVPActivityController.<EnterPvp>d__24>(System.Runtime.CompilerServices.TaskAwaiter&,PVPActivityController.<EnterPvp>d__24&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,PVPActivityController.<UpdateLeaderBoard>d__30>(System.Runtime.CompilerServices.TaskAwaiter&,PVPActivityController.<UpdateLeaderBoard>d__30&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,PVPRankView.<PlayFailAni>d__50>(System.Runtime.CompilerServices.TaskAwaiter&,PVPRankView.<PlayFailAni>d__50&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,PVPRankView.<PlayScoreUpAni>d__46>(System.Runtime.CompilerServices.TaskAwaiter&,PVPRankView.<PlayScoreUpAni>d__46&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,PVPSaveWinstreakView.<DelayToClose>d__11>(System.Runtime.CompilerServices.TaskAwaiter&,PVPSaveWinstreakView.<DelayToClose>d__11&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,PointToPoint.GameMain.<PlayWinAni>d__84>(System.Runtime.CompilerServices.TaskAwaiter&,PointToPoint.GameMain.<PlayWinAni>d__84&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,PvPMatchEntranceView.<RefreshAniHeads>d__44>(System.Runtime.CompilerServices.TaskAwaiter&,PvPMatchEntranceView.<RefreshAniHeads>d__44&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,RateUsController.<OnViewClose>d__9>(System.Runtime.CompilerServices.TaskAwaiter&,RateUsController.<OnViewClose>d__9&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,TMatchCreateState.<Enter>d__2>(System.Runtime.CompilerServices.TaskAwaiter&,TMatchCreateState.<Enter>d__2&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,UIMailboxController.<OnViewClose>d__18>(System.Runtime.CompilerServices.TaskAwaiter&,UIMailboxController.<OnViewClose>d__18&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,UIPopup.<OnViewClose>d__13>(System.Runtime.CompilerServices.TaskAwaiter&,UIPopup.<OnViewClose>d__13&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,UITMatchBuyProps.<OnViewClose>d__8>(System.Runtime.CompilerServices.TaskAwaiter&,UITMatchBuyProps.<OnViewClose>d__8&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,UITMatchFailController.<OnViewClose>d__9>(System.Runtime.CompilerServices.TaskAwaiter&,UITMatchFailController.<OnViewClose>d__9&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,UITMatchMainController.<OnViewClose>d__36>(System.Runtime.CompilerServices.TaskAwaiter&,UITMatchMainController.<OnViewClose>d__36&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,UITMatchMainTaskItem.<Destory>d__3>(System.Runtime.CompilerServices.TaskAwaiter&,UITMatchMainTaskItem.<Destory>d__3&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,UITeamDialogController.<OnViewClose>d__3>(System.Runtime.CompilerServices.TaskAwaiter&,UITeamDialogController.<OnViewClose>d__3&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,UIView.<OnViewClose>d__14>(System.Runtime.CompilerServices.TaskAwaiter&,UIView.<OnViewClose>d__14&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,UiLoadingCutainsController.<OnViewClose>d__13>(System.Runtime.CompilerServices.TaskAwaiter&,UiLoadingCutainsController.<OnViewClose>d__13&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,UiOutLivesController.<OnViewClose>d__6>(System.Runtime.CompilerServices.TaskAwaiter&,UiOutLivesController.<OnViewClose>d__6&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,UiTeamDetailController.<OnViewClose>d__16>(System.Runtime.CompilerServices.TaskAwaiter&,UiTeamDetailController.<OnViewClose>d__16&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,UiTeamReplaceController.<OnViewClose>d__7>(System.Runtime.CompilerServices.TaskAwaiter&,UiTeamReplaceController.<OnViewClose>d__7&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.YieldAwaitable.YieldAwaiter,PVPRankView.<DoMove>d__49>(System.Runtime.CompilerServices.YieldAwaitable.YieldAwaiter&,PVPRankView.<DoMove>d__49&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.YieldAwaitable.YieldAwaiter,TMatchCreateState.<CorrectItemPosition>d__6>(System.Runtime.CompilerServices.YieldAwaitable.YieldAwaiter&,TMatchCreateState.<CorrectItemPosition>d__6&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<System.Threading.Tasks.VoidTaskResult>.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,BreakEggMainPopup.<DelayToOpenRewardUI>d__31>(System.Runtime.CompilerServices.TaskAwaiter&,BreakEggMainPopup.<DelayToOpenRewardUI>d__31&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<System.Threading.Tasks.VoidTaskResult>.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,CGVideoManager.<PlayVideo>d__30>(System.Runtime.CompilerServices.TaskAwaiter&,CGVideoManager.<PlayVideo>d__30&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<System.Threading.Tasks.VoidTaskResult>.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,CGVideoManager.<TryStartCG>d__35>(System.Runtime.CompilerServices.TaskAwaiter&,CGVideoManager.<TryStartCG>d__35&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<System.Threading.Tasks.VoidTaskResult>.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,CollectCardMainView.<TaskShowExchange>d__32>(System.Runtime.CompilerServices.TaskAwaiter&,CollectCardMainView.<TaskShowExchange>d__32&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<System.Threading.Tasks.VoidTaskResult>.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,CollectCoinView.<OnViewClose>d__11>(System.Runtime.CompilerServices.TaskAwaiter&,CollectCoinView.<OnViewClose>d__11&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<System.Threading.Tasks.VoidTaskResult>.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,CollectDiamondView.<OnViewClose>d__15>(System.Runtime.CompilerServices.TaskAwaiter&,CollectDiamondView.<OnViewClose>d__15&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<System.Threading.Tasks.VoidTaskResult>.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,CommonUtils.<PlayAnimationAsync>d__100>(System.Runtime.CompilerServices.TaskAwaiter&,CommonUtils.<PlayAnimationAsync>d__100&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<System.Threading.Tasks.VoidTaskResult>.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,ContactUsController.<OnViewClose>d__54>(System.Runtime.CompilerServices.TaskAwaiter&,ContactUsController.<OnViewClose>d__54&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<System.Threading.Tasks.VoidTaskResult>.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,DragonPlus.PrivacyModel.<TryToShowPrivacyAsync>d__8>(System.Runtime.CompilerServices.TaskAwaiter&,DragonPlus.PrivacyModel.<TryToShowPrivacyAsync>d__8&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<System.Threading.Tasks.VoidTaskResult>.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,EndlessGiftPackMainItem.<DoDiappear>d__25>(System.Runtime.CompilerServices.TaskAwaiter&,EndlessGiftPackMainItem.<DoDiappear>d__25&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<System.Threading.Tasks.VoidTaskResult>.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,EndlessGiftPackMainPopup.<ClaimedSuccess>d__14>(System.Runtime.CompilerServices.TaskAwaiter&,EndlessGiftPackMainPopup.<ClaimedSuccess>d__14&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<System.Threading.Tasks.VoidTaskResult>.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,GuideArrowView.<OnViewClose>d__5>(System.Runtime.CompilerServices.TaskAwaiter&,GuideArrowView.<OnViewClose>d__5&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<System.Threading.Tasks.VoidTaskResult>.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,GuideFingerView.<OnViewClose>d__4>(System.Runtime.CompilerServices.TaskAwaiter&,GuideFingerView.<OnViewClose>d__4&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<System.Threading.Tasks.VoidTaskResult>.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,LoadingTransitionController.<OnViewClose>d__16>(System.Runtime.CompilerServices.TaskAwaiter&,LoadingTransitionController.<OnViewClose>d__16&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<System.Threading.Tasks.VoidTaskResult>.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,LoginController.<OnViewClose>d__19>(System.Runtime.CompilerServices.TaskAwaiter&,LoginController.<OnViewClose>d__19&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<System.Threading.Tasks.VoidTaskResult>.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,OneVOneDuelMain.<RefreshAniHeads>d__55>(System.Runtime.CompilerServices.TaskAwaiter&,OneVOneDuelMain.<RefreshAniHeads>d__55&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<System.Threading.Tasks.VoidTaskResult>.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,PVPActivityController.<EnterPvp>d__24>(System.Runtime.CompilerServices.TaskAwaiter&,PVPActivityController.<EnterPvp>d__24&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<System.Threading.Tasks.VoidTaskResult>.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,PVPActivityController.<UpdateLeaderBoard>d__30>(System.Runtime.CompilerServices.TaskAwaiter&,PVPActivityController.<UpdateLeaderBoard>d__30&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<System.Threading.Tasks.VoidTaskResult>.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,PVPRankView.<PlayFailAni>d__50>(System.Runtime.CompilerServices.TaskAwaiter&,PVPRankView.<PlayFailAni>d__50&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<System.Threading.Tasks.VoidTaskResult>.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,PVPRankView.<PlayScoreUpAni>d__46>(System.Runtime.CompilerServices.TaskAwaiter&,PVPRankView.<PlayScoreUpAni>d__46&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<System.Threading.Tasks.VoidTaskResult>.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,PVPSaveWinstreakView.<DelayToClose>d__11>(System.Runtime.CompilerServices.TaskAwaiter&,PVPSaveWinstreakView.<DelayToClose>d__11&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<System.Threading.Tasks.VoidTaskResult>.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,PointToPoint.GameMain.<PlayWinAni>d__84>(System.Runtime.CompilerServices.TaskAwaiter&,PointToPoint.GameMain.<PlayWinAni>d__84&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<System.Threading.Tasks.VoidTaskResult>.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,PvPMatchEntranceView.<RefreshAniHeads>d__44>(System.Runtime.CompilerServices.TaskAwaiter&,PvPMatchEntranceView.<RefreshAniHeads>d__44&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<System.Threading.Tasks.VoidTaskResult>.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,RateUsController.<OnViewClose>d__9>(System.Runtime.CompilerServices.TaskAwaiter&,RateUsController.<OnViewClose>d__9&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<System.Threading.Tasks.VoidTaskResult>.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,TMatchCreateState.<Enter>d__2>(System.Runtime.CompilerServices.TaskAwaiter&,TMatchCreateState.<Enter>d__2&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<System.Threading.Tasks.VoidTaskResult>.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,UIMailboxController.<OnViewClose>d__18>(System.Runtime.CompilerServices.TaskAwaiter&,UIMailboxController.<OnViewClose>d__18&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<System.Threading.Tasks.VoidTaskResult>.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,UIPopup.<OnViewClose>d__13>(System.Runtime.CompilerServices.TaskAwaiter&,UIPopup.<OnViewClose>d__13&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<System.Threading.Tasks.VoidTaskResult>.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,UITMatchBuyProps.<OnViewClose>d__8>(System.Runtime.CompilerServices.TaskAwaiter&,UITMatchBuyProps.<OnViewClose>d__8&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<System.Threading.Tasks.VoidTaskResult>.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,UITMatchFailController.<OnViewClose>d__9>(System.Runtime.CompilerServices.TaskAwaiter&,UITMatchFailController.<OnViewClose>d__9&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<System.Threading.Tasks.VoidTaskResult>.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,UITMatchMainController.<OnViewClose>d__36>(System.Runtime.CompilerServices.TaskAwaiter&,UITMatchMainController.<OnViewClose>d__36&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<System.Threading.Tasks.VoidTaskResult>.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,UITMatchMainTaskItem.<Destory>d__3>(System.Runtime.CompilerServices.TaskAwaiter&,UITMatchMainTaskItem.<Destory>d__3&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<System.Threading.Tasks.VoidTaskResult>.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,UITeamDialogController.<OnViewClose>d__3>(System.Runtime.CompilerServices.TaskAwaiter&,UITeamDialogController.<OnViewClose>d__3&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<System.Threading.Tasks.VoidTaskResult>.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,UIView.<OnViewClose>d__14>(System.Runtime.CompilerServices.TaskAwaiter&,UIView.<OnViewClose>d__14&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<System.Threading.Tasks.VoidTaskResult>.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,UiLoadingCutainsController.<OnViewClose>d__13>(System.Runtime.CompilerServices.TaskAwaiter&,UiLoadingCutainsController.<OnViewClose>d__13&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<System.Threading.Tasks.VoidTaskResult>.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,UiOutLivesController.<OnViewClose>d__6>(System.Runtime.CompilerServices.TaskAwaiter&,UiOutLivesController.<OnViewClose>d__6&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<System.Threading.Tasks.VoidTaskResult>.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,UiTeamDetailController.<OnViewClose>d__16>(System.Runtime.CompilerServices.TaskAwaiter&,UiTeamDetailController.<OnViewClose>d__16&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<System.Threading.Tasks.VoidTaskResult>.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,UiTeamReplaceController.<OnViewClose>d__7>(System.Runtime.CompilerServices.TaskAwaiter&,UiTeamReplaceController.<OnViewClose>d__7&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<System.Threading.Tasks.VoidTaskResult>.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.YieldAwaitable.YieldAwaiter,PVPRankView.<DoMove>d__49>(System.Runtime.CompilerServices.YieldAwaitable.YieldAwaiter&,PVPRankView.<DoMove>d__49&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<System.Threading.Tasks.VoidTaskResult>.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.YieldAwaitable.YieldAwaiter,TMatchCreateState.<CorrectItemPosition>d__6>(System.Runtime.CompilerServices.YieldAwaitable.YieldAwaiter&,TMatchCreateState.<CorrectItemPosition>d__6&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<byte>.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,FileUtils.<SaveTextureAsync>d__0>(System.Runtime.CompilerServices.TaskAwaiter&,FileUtils.<SaveTextureAsync>d__0&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<byte>.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,Framework.SubSystem.<DelayCareDestory>d__1>(System.Runtime.CompilerServices.TaskAwaiter&,Framework.SubSystem.<DelayCareDestory>d__1&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<byte>.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,PVPSystemBase.<DelayCareDestory>d__7>(System.Runtime.CompilerServices.TaskAwaiter&,PVPSystemBase.<DelayCareDestory>d__7&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<byte>.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter<byte>,Framework.Fsm.<doChangeToStateAsync>d__13>(System.Runtime.CompilerServices.TaskAwaiter<byte>&,Framework.Fsm.<doChangeToStateAsync>d__13&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<byte>.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter<byte>,StateDecoration.<Framework-IFsmState-PreEnterAsync>d__3>(System.Runtime.CompilerServices.TaskAwaiter<byte>&,StateDecoration.<Framework-IFsmState-PreEnterAsync>d__3&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<byte>.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.YieldAwaitable.YieldAwaiter,ColorFul.Gameplay.StateColorfulGame.<Framework-IFsmState-PreEnterAsync>d__5>(System.Runtime.CompilerServices.YieldAwaitable.YieldAwaiter&,ColorFul.Gameplay.StateColorfulGame.<Framework-IFsmState-PreEnterAsync>d__5&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<byte>.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.YieldAwaitable.YieldAwaiter,OneLine.StateOneLineGame.<Framework-IFsmState-PreEnterAsync>d__4>(System.Runtime.CompilerServices.YieldAwaitable.YieldAwaiter&,OneLine.StateOneLineGame.<Framework-IFsmState-PreEnterAsync>d__4&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<byte>.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.YieldAwaitable.YieldAwaiter,Pipe.GamePlay.StatePipeGame.<Framework-IFsmState-PreEnterAsync>d__6>(System.Runtime.CompilerServices.YieldAwaitable.YieldAwaiter&,Pipe.GamePlay.StatePipeGame.<Framework-IFsmState-PreEnterAsync>d__6&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<object>.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter<int>,FileUtils.<LoadTextureAsync>d__1>(System.Runtime.CompilerServices.TaskAwaiter<int>&,FileUtils.<LoadTextureAsync>d__1&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.Start<ASMR.UIMain.<OnViewClose>d__15>(ASMR.UIMain.<OnViewClose>d__15&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.Start<BreakEggMainPopup.<DelayToOpenRewardUI>d__31>(BreakEggMainPopup.<DelayToOpenRewardUI>d__31&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.Start<CGVideoManager.<PlayVideo>d__30>(CGVideoManager.<PlayVideo>d__30&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.Start<CGVideoManager.<TryStartCG>d__35>(CGVideoManager.<TryStartCG>d__35&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.Start<ChooseProgressController.<OnViewClose>d__28>(ChooseProgressController.<OnViewClose>d__28&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.Start<CollectCardMainView.<TaskShowExchange>d__32>(CollectCardMainView.<TaskShowExchange>d__32&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.Start<CollectCoinView.<OnViewClose>d__11>(CollectCoinView.<OnViewClose>d__11&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.Start<CollectDiamondView.<OnViewClose>d__15>(CollectDiamondView.<OnViewClose>d__15&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.Start<CommonUtils.<PlayAnimationAsync>d__100>(CommonUtils.<PlayAnimationAsync>d__100&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.Start<ContactUsController.<OnViewClose>d__54>(ContactUsController.<OnViewClose>d__54&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.Start<DragonPlus.PrivacyModel.<TryToShowPrivacyAsync>d__8>(DragonPlus.PrivacyModel.<TryToShowPrivacyAsync>d__8&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.Start<EndlessGiftPackMainItem.<DoDiappear>d__25>(EndlessGiftPackMainItem.<DoDiappear>d__25&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.Start<EndlessGiftPackMainPopup.<ClaimedSuccess>d__14>(EndlessGiftPackMainPopup.<ClaimedSuccess>d__14&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.Start<GuideArrowView.<OnViewClose>d__5>(GuideArrowView.<OnViewClose>d__5&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.Start<GuideFingerView.<OnViewClose>d__4>(GuideFingerView.<OnViewClose>d__4&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.Start<Launching.<LoadConfigs>d__21>(Launching.<LoadConfigs>d__21&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.Start<LoadingTransitionController.<OnViewClose>d__16>(LoadingTransitionController.<OnViewClose>d__16&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.Start<LoginController.<OnViewClose>d__19>(LoginController.<OnViewClose>d__19&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.Start<OneVOneDuelMain.<RefreshAniHeads>d__55>(OneVOneDuelMain.<RefreshAniHeads>d__55&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.Start<PVPActivityController.<EnterPvp>d__24>(PVPActivityController.<EnterPvp>d__24&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.Start<PVPActivityController.<UpdateLeaderBoard>d__30>(PVPActivityController.<UpdateLeaderBoard>d__30&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.Start<PVPRankView.<DoMove>d__49>(PVPRankView.<DoMove>d__49&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.Start<PVPRankView.<PlayFailAni>d__50>(PVPRankView.<PlayFailAni>d__50&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.Start<PVPRankView.<PlayScoreUpAni>d__46>(PVPRankView.<PlayScoreUpAni>d__46&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.Start<PVPSaveWinstreakView.<DelayToClose>d__11>(PVPSaveWinstreakView.<DelayToClose>d__11&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.Start<PointToPoint.GameMain.<PlayWinAni>d__84>(PointToPoint.GameMain.<PlayWinAni>d__84&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.Start<PvPMatchEntranceView.<RefreshAniHeads>d__44>(PvPMatchEntranceView.<RefreshAniHeads>d__44&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.Start<RateUsController.<OnViewClose>d__9>(RateUsController.<OnViewClose>d__9&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.Start<TMatchCreateState.<CorrectItemPosition>d__6>(TMatchCreateState.<CorrectItemPosition>d__6&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.Start<TMatchCreateState.<Enter>d__2>(TMatchCreateState.<Enter>d__2&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.Start<TMatchCreateState.<Exit>d__5>(TMatchCreateState.<Exit>d__5&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.Start<TMatchDestoryState.<Enter>d__2>(TMatchDestoryState.<Enter>d__2&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.Start<TMatchDestoryState.<Exit>d__4>(TMatchDestoryState.<Exit>d__4&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.Start<TMatchFinishState.<Enter>d__2>(TMatchFinishState.<Enter>d__2&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.Start<TMatchFinishState.<Exit>d__4>(TMatchFinishState.<Exit>d__4&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.Start<TMatchPlayState.<Enter>d__5>(TMatchPlayState.<Enter>d__5&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.Start<TMatchPlayState.<Exit>d__7>(TMatchPlayState.<Exit>d__7&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.Start<TMatchPrepareState.<Enter>d__2>(TMatchPrepareState.<Enter>d__2&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.Start<TMatchPrepareState.<Exit>d__4>(TMatchPrepareState.<Exit>d__4&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.Start<UIMailboxController.<OnViewClose>d__18>(UIMailboxController.<OnViewClose>d__18&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.Start<UIPopup.<OnViewClose>d__13>(UIPopup.<OnViewClose>d__13&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.Start<UITMatchBuyProps.<OnViewClose>d__8>(UITMatchBuyProps.<OnViewClose>d__8&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.Start<UITMatchFailController.<OnViewClose>d__9>(UITMatchFailController.<OnViewClose>d__9&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.Start<UITMatchMainController.<OnViewClose>d__36>(UITMatchMainController.<OnViewClose>d__36&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.Start<UITMatchMainTaskItem.<Destory>d__3>(UITMatchMainTaskItem.<Destory>d__3&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.Start<UITeamDialogController.<OnViewClose>d__3>(UITeamDialogController.<OnViewClose>d__3&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.Start<UIView.<OnViewClose>d__14>(UIView.<OnViewClose>d__14&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.Start<UiLoadingCutainsController.<OnViewClose>d__13>(UiLoadingCutainsController.<OnViewClose>d__13&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.Start<UiOutLivesController.<OnViewClose>d__6>(UiOutLivesController.<OnViewClose>d__6&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.Start<UiTeamDetailController.<OnViewClose>d__16>(UiTeamDetailController.<OnViewClose>d__16&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.Start<UiTeamReplaceController.<OnViewClose>d__7>(UiTeamReplaceController.<OnViewClose>d__7&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<byte>.Start<ColorFul.Gameplay.StateColorfulGame.<Framework-IFsmState-PreEnterAsync>d__5>(ColorFul.Gameplay.StateColorfulGame.<Framework-IFsmState-PreEnterAsync>d__5&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<byte>.Start<FileUtils.<SaveTextureAsync>d__0>(FileUtils.<SaveTextureAsync>d__0&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<byte>.Start<Framework.Fsm.<doChangeToStateAsync>d__13>(Framework.Fsm.<doChangeToStateAsync>d__13&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<byte>.Start<Framework.SubSystem.<DelayCareDestory>d__1>(Framework.SubSystem.<DelayCareDestory>d__1&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<byte>.Start<Gameplay.StateASMRGame.<PreEnterAsync>d__5>(Gameplay.StateASMRGame.<PreEnterAsync>d__5&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<byte>.Start<OneLine.StateOneLineGame.<Framework-IFsmState-PreEnterAsync>d__4>(OneLine.StateOneLineGame.<Framework-IFsmState-PreEnterAsync>d__4&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<byte>.Start<PVPSystemBase.<DelayCareDestory>d__7>(PVPSystemBase.<DelayCareDestory>d__7&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<byte>.Start<Pipe.GamePlay.StatePipeGame.<Framework-IFsmState-PreEnterAsync>d__6>(Pipe.GamePlay.StatePipeGame.<Framework-IFsmState-PreEnterAsync>d__6&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<byte>.Start<PointToPoint.StatePointToPoint.<Framework-IFsmState-PreEnterAsync>d__7>(PointToPoint.StatePointToPoint.<Framework-IFsmState-PreEnterAsync>d__7&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<byte>.Start<StateCG.<PreEnterAsync>d__2>(StateCG.<PreEnterAsync>d__2&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<byte>.Start<StateDecoration.<Framework-IFsmState-PreEnterAsync>d__3>(StateDecoration.<Framework-IFsmState-PreEnterAsync>d__3&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<byte>.Start<StateDecoration.<loadWorldAsync>d__5>(StateDecoration.<loadWorldAsync>d__5&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<byte>.Start<StateLaunch.<PreEnterAsync>d__3>(StateLaunch.<PreEnterAsync>d__3&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<byte>.Start<StateLogin.<PreEnterAsync>d__2>(StateLogin.<PreEnterAsync>d__2&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<byte>.Start<StatePVP.<PreEnterAsync>d__4>(StatePVP.<PreEnterAsync>d__4&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<byte>.Start<StateTMatch.<PreEnterAsync>d__4>(StateTMatch.<PreEnterAsync>d__4&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<byte>.Start<StateTransition.<PreEnterAsync>d__2>(StateTransition.<PreEnterAsync>d__2&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<object>.Start<FileUtils.<LoadTextureAsync>d__1>(FileUtils.<LoadTextureAsync>d__1&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,ASMR.UIGameSuccess.<FlyCallback>d__17>(System.Runtime.CompilerServices.TaskAwaiter&,ASMR.UIGameSuccess.<FlyCallback>d__17&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,ASMRLittleGameSuccess.<FlyCallback>d__17>(System.Runtime.CompilerServices.TaskAwaiter&,ASMRLittleGameSuccess.<FlyCallback>d__17&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,BackGiftMainView.<OnClickClaim>d__11>(System.Runtime.CompilerServices.TaskAwaiter&,BackGiftMainView.<OnClickClaim>d__11&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,BeginnerRebateHelpView.<WaitToClose>d__5>(System.Runtime.CompilerServices.TaskAwaiter&,BeginnerRebateHelpView.<WaitToClose>d__5&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,BeginnerRebateRewardView.<WaitToClose>d__13>(System.Runtime.CompilerServices.TaskAwaiter&,BeginnerRebateRewardView.<WaitToClose>d__13&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,CollectCardFullRewardView.<WaitForClose>d__13>(System.Runtime.CompilerServices.TaskAwaiter&,CollectCardFullRewardView.<WaitForClose>d__13&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,CollectCardGoldCardRewardView.<FlyItem>d__10>(System.Runtime.CompilerServices.TaskAwaiter&,CollectCardGoldCardRewardView.<FlyItem>d__10&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,CollectCardGoldCardRewardView.<OnClickButton>d__9>(System.Runtime.CompilerServices.TaskAwaiter&,CollectCardGoldCardRewardView.<OnClickButton>d__9&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,CollectCardGoldCardRewardView.<Show>d__8>(System.Runtime.CompilerServices.TaskAwaiter&,CollectCardGoldCardRewardView.<Show>d__8&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,CollectCardHelpView.<WaitToClose>d__5>(System.Runtime.CompilerServices.TaskAwaiter&,CollectCardHelpView.<WaitToClose>d__5&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,CollectCardMainView.<TryAutoExchange>d__31>(System.Runtime.CompilerServices.TaskAwaiter&,CollectCardMainView.<TryAutoExchange>d__31&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,CollectCardRewardView.<DoCardOpenAnimation>d__14>(System.Runtime.CompilerServices.TaskAwaiter&,CollectCardRewardView.<DoCardOpenAnimation>d__14&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,CollectCardRewardView.<FlyItemToGate>d__19>(System.Runtime.CompilerServices.TaskAwaiter&,CollectCardRewardView.<FlyItemToGate>d__19&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,CollectCardRewardView.<OnClaimButtonClicked>d__17>(System.Runtime.CompilerServices.TaskAwaiter&,CollectCardRewardView.<OnClaimButtonClicked>d__17&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,CollectCoinGateView.<OnTMatchResultExecute>d__15>(System.Runtime.CompilerServices.TaskAwaiter&,CollectCoinGateView.<OnTMatchResultExecute>d__15&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,CollectDiamondGateView.<OnTMatchResultExecute>d__15>(System.Runtime.CompilerServices.TaskAwaiter&,CollectDiamondGateView.<OnTMatchResultExecute>d__15&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,CollectGuildGateView.<OnTMatchResultExecute>d__16>(System.Runtime.CompilerServices.TaskAwaiter&,CollectGuildGateView.<OnTMatchResultExecute>d__16&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,CollectGuildOpeningView.<OnViewOpen>d__3>(System.Runtime.CompilerServices.TaskAwaiter&,CollectGuildOpeningView.<OnViewOpen>d__3&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,EndlessGiftPackMainItem.<TryToOpenBox>d__23>(System.Runtime.CompilerServices.TaskAwaiter&,EndlessGiftPackMainItem.<TryToOpenBox>d__23&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,FlyNoticeView.<AddNotice>d__8>(System.Runtime.CompilerServices.TaskAwaiter&,FlyNoticeView.<AddNotice>d__8&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,GoldenPass2GateView.<OnTMatchResultExecute>d__20>(System.Runtime.CompilerServices.TaskAwaiter&,GoldenPass2GateView.<OnTMatchResultExecute>d__20&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,GoldenPass2HelpPopup.<WaitToClose>d__3>(System.Runtime.CompilerServices.TaskAwaiter&,GoldenPass2HelpPopup.<WaitToClose>d__3&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,GoldenPass2MainPopup.<RefreshTableViewAndScroll>d__46>(System.Runtime.CompilerServices.TaskAwaiter&,GoldenPass2MainPopup.<RefreshTableViewAndScroll>d__46&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,GoldenPass3GateView.<OnTMatchResultExecute>d__20>(System.Runtime.CompilerServices.TaskAwaiter&,GoldenPass3GateView.<OnTMatchResultExecute>d__20&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,GoldenPass3HelpPopup.<WaitToClose>d__3>(System.Runtime.CompilerServices.TaskAwaiter&,GoldenPass3HelpPopup.<WaitToClose>d__3&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,GoldenPass3MainPopup.<RefreshTableViewAndScroll>d__46>(System.Runtime.CompilerServices.TaskAwaiter&,GoldenPass3MainPopup.<RefreshTableViewAndScroll>d__46&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,GoldenPass4GateView.<OnTMatchResultExecute>d__20>(System.Runtime.CompilerServices.TaskAwaiter&,GoldenPass4GateView.<OnTMatchResultExecute>d__20&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,GoldenPass4HelpPopup.<WaitToClose>d__3>(System.Runtime.CompilerServices.TaskAwaiter&,GoldenPass4HelpPopup.<WaitToClose>d__3&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,GoldenPass4MainPopup.<RefreshTableViewAndScroll>d__46>(System.Runtime.CompilerServices.TaskAwaiter&,GoldenPass4MainPopup.<RefreshTableViewAndScroll>d__46&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,GoldenPass5GateView.<OnTMatchResultExecute>d__23>(System.Runtime.CompilerServices.TaskAwaiter&,GoldenPass5GateView.<OnTMatchResultExecute>d__23&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,GoldenPass5HelpPopup.<WaitToClose>d__3>(System.Runtime.CompilerServices.TaskAwaiter&,GoldenPass5HelpPopup.<WaitToClose>d__3&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,GoldenPass5MainPopup.<RefreshTableViewAndScroll>d__46>(System.Runtime.CompilerServices.TaskAwaiter&,GoldenPass5MainPopup.<RefreshTableViewAndScroll>d__46&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,GoldenPassGateView.<OnTMatchResultExecute>d__23>(System.Runtime.CompilerServices.TaskAwaiter&,GoldenPassGateView.<OnTMatchResultExecute>d__23&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,GoldenPassHelpPopup.<WaitToClose>d__3>(System.Runtime.CompilerServices.TaskAwaiter&,GoldenPassHelpPopup.<WaitToClose>d__3&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,GoldenPassMainPopup.<RefreshTableViewAndScroll>d__46>(System.Runtime.CompilerServices.TaskAwaiter&,GoldenPassMainPopup.<RefreshTableViewAndScroll>d__46&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,HalloweenShibaInuGateView.<OnTMatchResultExecute>d__13>(System.Runtime.CompilerServices.TaskAwaiter&,HalloweenShibaInuGateView.<OnTMatchResultExecute>d__13&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,HalloweenShibaInuHelpPopup.<WaitToClose>d__5>(System.Runtime.CompilerServices.TaskAwaiter&,HalloweenShibaInuHelpPopup.<WaitToClose>d__5&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,Launching.<startLaunchSequence>d__15>(System.Runtime.CompilerServices.TaskAwaiter&,Launching.<startLaunchSequence>d__15&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,LeaderBoardManager.<startTimerTick>d__6>(System.Runtime.CompilerServices.TaskAwaiter&,LeaderBoardManager.<startTimerTick>d__6&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,LobbyTaskSystem.<CheckResultLastFinsih>d__16>(System.Runtime.CompilerServices.TaskAwaiter&,LobbyTaskSystem.<CheckResultLastFinsih>d__16&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,PVPActivityController.<GetRankInfo>d__60>(System.Runtime.CompilerServices.TaskAwaiter&,PVPActivityController.<GetRankInfo>d__60&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,PVPBaseItem.<Retract>d__49>(System.Runtime.CompilerServices.TaskAwaiter&,PVPBaseItem.<Retract>d__49&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,PVPBoostSystem.<StopPropEffect>d__15>(System.Runtime.CompilerServices.TaskAwaiter&,PVPBoostSystem.<StopPropEffect>d__15&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,PVPLoadingView.<DoHeadFlyAndClose>d__16>(System.Runtime.CompilerServices.TaskAwaiter&,PVPLoadingView.<DoHeadFlyAndClose>d__16&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,PVPSystem.<CheckAllPrepared>d__26>(System.Runtime.CompilerServices.TaskAwaiter&,PVPSystem.<CheckAllPrepared>d__26&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,PVPSystem.<GameBegin>d__27>(System.Runtime.CompilerServices.TaskAwaiter&,PVPSystem.<GameBegin>d__27&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,PVPSystem.<GameTimeOut>d__32>(System.Runtime.CompilerServices.TaskAwaiter&,PVPSystem.<GameTimeOut>d__32&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,PVPSystem.<ShowGameResult>d__33>(System.Runtime.CompilerServices.TaskAwaiter&,PVPSystem.<ShowGameResult>d__33&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,PVPTaskItemView.<DoMoveXAnimation>d__7>(System.Runtime.CompilerServices.TaskAwaiter&,PVPTaskItemView.<DoMoveXAnimation>d__7&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,PVPTaskItemView.<RefreshShow>d__6>(System.Runtime.CompilerServices.TaskAwaiter&,PVPTaskItemView.<RefreshShow>d__6&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,PiggyBankGateView.<OnTMatchResultExecute>d__13>(System.Runtime.CompilerServices.TaskAwaiter&,PiggyBankGateView.<OnTMatchResultExecute>d__13&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,RankModel.<GetRankInfo>d__12>(System.Runtime.CompilerServices.TaskAwaiter&,RankModel.<GetRankInfo>d__12&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,RechargeGiftPackActivityModel.<>c__DisplayClass13_0.<<ShowGetRewardView>b__0>d>(System.Runtime.CompilerServices.TaskAwaiter&,RechargeGiftPackActivityModel.<>c__DisplayClass13_0.<<ShowGetRewardView>b__0>d&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,RechargeGiftPackMainItem.<PlayBuySuccessAni>d__21>(System.Runtime.CompilerServices.TaskAwaiter&,RechargeGiftPackMainItem.<PlayBuySuccessAni>d__21&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,RechargeGiftPackMainItem.<PlayUnlockAni>d__22>(System.Runtime.CompilerServices.TaskAwaiter&,RechargeGiftPackMainItem.<PlayUnlockAni>d__22&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,RechargeGiftPackMainPopup.<OnBuySuccess>d__16>(System.Runtime.CompilerServices.TaskAwaiter&,RechargeGiftPackMainPopup.<OnBuySuccess>d__16&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,RoseJourneyGateView.<FlyAddItem>d__17>(System.Runtime.CompilerServices.TaskAwaiter&,RoseJourneyGateView.<FlyAddItem>d__17&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,RoseJourneyGateView.<JumpToType>d__19>(System.Runtime.CompilerServices.TaskAwaiter&,RoseJourneyGateView.<JumpToType>d__19&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,RoseJourneyHelpView.<WaitToClose>d__3>(System.Runtime.CompilerServices.TaskAwaiter&,RoseJourneyHelpView.<WaitToClose>d__3&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,ShibaInuGateView.<OnTMatchResultExecute>d__13>(System.Runtime.CompilerServices.TaskAwaiter&,ShibaInuGateView.<OnTMatchResultExecute>d__13&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,ShibaInuHelpPopup.<WaitToClose>d__5>(System.Runtime.CompilerServices.TaskAwaiter&,ShibaInuHelpPopup.<WaitToClose>d__5&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,SpeedRaceHelpPopup.<WaitToClose>d__5>(System.Runtime.CompilerServices.TaskAwaiter&,SpeedRaceHelpPopup.<WaitToClose>d__5&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,SpeedRaceMainItem.<>c__DisplayClass24_0.<<PlayScoreAin>g__UpdateScoreOnce|0>d>(System.Runtime.CompilerServices.TaskAwaiter&,SpeedRaceMainItem.<>c__DisplayClass24_0.<<PlayScoreAin>g__UpdateScoreOnce|0>d&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,SpeedRaceMainItem.<WaitToPlayWinAni>d__23>(System.Runtime.CompilerServices.TaskAwaiter&,SpeedRaceMainItem.<WaitToPlayWinAni>d__23&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,SpeedRaceMainPopup.<PlayMoveAnimation>d__26>(System.Runtime.CompilerServices.TaskAwaiter&,SpeedRaceMainPopup.<PlayMoveAnimation>d__26&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,SuperLightGateView.<OnTMatchResultExecute>d__12>(System.Runtime.CompilerServices.TaskAwaiter&,SuperLightGateView.<OnTMatchResultExecute>d__12&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,SuperLightHelpView.<WaitToClose>d__4>(System.Runtime.CompilerServices.TaskAwaiter&,SuperLightHelpView.<WaitToClose>d__4&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,SuperWinstreakGateView.<OnTMatchResultExecute>d__13>(System.Runtime.CompilerServices.TaskAwaiter&,SuperWinstreakGateView.<OnTMatchResultExecute>d__13&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,TMatchBaseItem.<Retract>d__48>(System.Runtime.CompilerServices.TaskAwaiter&,TMatchBaseItem.<Retract>d__48&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,TMatchGoldenHatterSystem.<OnGameStartEvt>d__5>(System.Runtime.CompilerServices.TaskAwaiter&,TMatchGoldenHatterSystem.<OnGameStartEvt>d__5&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,TMatchLevelBoostSystem.<UseLevelBoostClock>d__3>(System.Runtime.CompilerServices.TaskAwaiter&,TMatchLevelBoostSystem.<UseLevelBoostClock>d__3&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,TMatchLevelBoostSystem.<UseLevelBoostLighting>d__4>(System.Runtime.CompilerServices.TaskAwaiter&,TMatchLevelBoostSystem.<UseLevelBoostLighting>d__4&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,TMatchNormalLevelController.<OnWin>d__13>(System.Runtime.CompilerServices.TaskAwaiter&,TMatchNormalLevelController.<OnWin>d__13&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,TMatchStateSystem.<ChangeStateAsync>d__8>(System.Runtime.CompilerServices.TaskAwaiter&,TMatchStateSystem.<ChangeStateAsync>d__8&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,TeamManager.<startTimerTick>d__12>(System.Runtime.CompilerServices.TaskAwaiter&,TeamManager.<startTimerTick>d__12&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,TeamPassGift.<WaitToSetHelpButtonEnabled>d__14>(System.Runtime.CompilerServices.TaskAwaiter&,TeamPassGift.<WaitToSetHelpButtonEnabled>d__14&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,TeamTreasureGateView.<OnTMatchResultExecute>d__15>(System.Runtime.CompilerServices.TaskAwaiter&,TeamTreasureGateView.<OnTMatchResultExecute>d__15&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,TeamTreasureOpeningView.<OnViewOpen>d__3>(System.Runtime.CompilerServices.TaskAwaiter&,TeamTreasureOpeningView.<OnViewOpen>d__3&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,TreasureHuntGateView.<OnTMatchResultExecute>d__15>(System.Runtime.CompilerServices.TaskAwaiter&,TreasureHuntGateView.<OnTMatchResultExecute>d__15&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,UILevelChestPopup.<DelayShowText>d__6>(System.Runtime.CompilerServices.TaskAwaiter&,UILevelChestPopup.<DelayShowText>d__6&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,UILobbyMainViewLevelButton.<ShowGoldenHatter>d__13>(System.Runtime.CompilerServices.TaskAwaiter&,UILobbyMainViewLevelButton.<ShowGoldenHatter>d__13&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,UILobbyMainViewStarChest.<OnTMatchResultExecute>d__16>(System.Runtime.CompilerServices.TaskAwaiter&,UILobbyMainViewStarChest.<OnTMatchResultExecute>d__16&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,UIPVPCountdownView.<PlayAniAndClose>d__1>(System.Runtime.CompilerServices.TaskAwaiter&,UIPVPCountdownView.<PlayAniAndClose>d__1&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,UIPVPFailedPopup.<RefreshCoinWhenAniEnd>d__12>(System.Runtime.CompilerServices.TaskAwaiter&,UIPVPFailedPopup.<RefreshCoinWhenAniEnd>d__12&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,UIPVPSpaceOutPopup.<ContinueOnClick>d__12>(System.Runtime.CompilerServices.TaskAwaiter&,UIPVPSpaceOutPopup.<ContinueOnClick>d__12&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,UIPVPWinPopup.<OnContinueButtonClicked>d__3>(System.Runtime.CompilerServices.TaskAwaiter&,UIPVPWinPopup.<OnContinueButtonClicked>d__3&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,UIPVPWinPopup.<RefreshCoinWhenAniEnd>d__2>(System.Runtime.CompilerServices.TaskAwaiter&,UIPVPWinPopup.<RefreshCoinWhenAniEnd>d__2&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,UISaveYourProgressController.<RefreshButtonTextAndReward>d__22>(System.Runtime.CompilerServices.TaskAwaiter&,UISaveYourProgressController.<RefreshButtonTextAndReward>d__22&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,UISignInController.<OnClaimButtonClicked>d__21>(System.Runtime.CompilerServices.TaskAwaiter&,UISignInController.<OnClaimButtonClicked>d__21&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,UITMatchBuyProps.<BuyOnClick>d__9>(System.Runtime.CompilerServices.TaskAwaiter&,UITMatchBuyProps.<BuyOnClick>d__9&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,UITMatchLevelBoostClock.<WaitToDoClose>d__5>(System.Runtime.CompilerServices.TaskAwaiter&,UITMatchLevelBoostClock.<WaitToDoClose>d__5&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,UITMatchMainController.<OnLowSpaceEvt>d__48>(System.Runtime.CompilerServices.TaskAwaiter&,UITMatchMainController.<OnLowSpaceEvt>d__48&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,UITMatchMainController.<OnTriple>d__56>(System.Runtime.CompilerServices.TaskAwaiter&,UITMatchMainController.<OnTriple>d__56&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,UITMatchSpaceOutController.<ContinueOnClick>d__24>(System.Runtime.CompilerServices.TaskAwaiter&,UITMatchSpaceOutController.<ContinueOnClick>d__24&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,UITMatchTimeOutController.<ContinueOnClick>d__24>(System.Runtime.CompilerServices.TaskAwaiter&,UITMatchTimeOutController.<ContinueOnClick>d__24&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,UITreasureHuntHelp.<WaitToClose>d__10>(System.Runtime.CompilerServices.TaskAwaiter&,UITreasureHuntHelp.<WaitToClose>d__10&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,UITreasureHuntMain.<OnClickAtGrid>d__48>(System.Runtime.CompilerServices.TaskAwaiter&,UITreasureHuntMain.<OnClickAtGrid>d__48&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,UIViewSystem.<OnClose>d__10>(System.Runtime.CompilerServices.TaskAwaiter&,UIViewSystem.<OnClose>d__10&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,UiLeaderBoardMainView.<autoRecoverPullingCoolDown>d__54>(System.Runtime.CompilerServices.TaskAwaiter&,UiLeaderBoardMainView.<autoRecoverPullingCoolDown>d__54&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,UiLeaderBoardMainView.<showGlobalTooltip>d__105>(System.Runtime.CompilerServices.TaskAwaiter&,UiLeaderBoardMainView.<showGlobalTooltip>d__105&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,UiLeaderBoardMainView.<showTeamTooltip>d__194>(System.Runtime.CompilerServices.TaskAwaiter&,UiLeaderBoardMainView.<showTeamTooltip>d__194&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,UiOutLivesController.<DelayShowText>d__5>(System.Runtime.CompilerServices.TaskAwaiter&,UiOutLivesController.<DelayShowText>d__5&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,UiOutLivesController.<OnRefillButtonClicked>d__10>(System.Runtime.CompilerServices.TaskAwaiter&,UiOutLivesController.<OnRefillButtonClicked>d__10&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,UiTeamDetailController.<autoRecoverJoinButton>d__28>(System.Runtime.CompilerServices.TaskAwaiter&,UiTeamDetailController.<autoRecoverJoinButton>d__28&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,UiTeamDetailController.<showDetailTooltip>d__29>(System.Runtime.CompilerServices.TaskAwaiter&,UiTeamDetailController.<showDetailTooltip>d__29&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,UiTeamMainController.<autoRecoverAskHelpButton>d__43>(System.Runtime.CompilerServices.TaskAwaiter&,UiTeamMainController.<autoRecoverAskHelpButton>d__43&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,UiTeamMainController.<autoRecoverCreateButton>d__78>(System.Runtime.CompilerServices.TaskAwaiter&,UiTeamMainController.<autoRecoverCreateButton>d__78&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,UiTeamMainController.<autoRecoverPullingCoolDown>d__192>(System.Runtime.CompilerServices.TaskAwaiter&,UiTeamMainController.<autoRecoverPullingCoolDown>d__192&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,UiTeamMainController.<autoRecoverSaveButton>d__172>(System.Runtime.CompilerServices.TaskAwaiter&,UiTeamMainController.<autoRecoverSaveButton>d__172&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,UiTeamMainController.<autoRecoverSearchButton>d__214>(System.Runtime.CompilerServices.TaskAwaiter&,UiTeamMainController.<autoRecoverSearchButton>d__214&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,UiTeamMainController.<showChatTooltip>d__42>(System.Runtime.CompilerServices.TaskAwaiter&,UiTeamMainController.<showChatTooltip>d__42&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,UiTeamMainController.<showCreateLevelTooltip>d__76>(System.Runtime.CompilerServices.TaskAwaiter&,UiTeamMainController.<showCreateLevelTooltip>d__76&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,UiTeamMainController.<showCreateNameTooltip>d__77>(System.Runtime.CompilerServices.TaskAwaiter&,UiTeamMainController.<showCreateNameTooltip>d__77&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,UiTeamMainController.<showModifyTooltip>d__173>(System.Runtime.CompilerServices.TaskAwaiter&,UiTeamMainController.<showModifyTooltip>d__173&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,UiTeamMainController.<showSearchTooltip>d__213>(System.Runtime.CompilerServices.TaskAwaiter&,UiTeamMainController.<showSearchTooltip>d__213&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,WeeklyChallengeController.<CheckStarNextWeek>d__9>(System.Runtime.CompilerServices.TaskAwaiter&,WeeklyChallengeController.<CheckStarNextWeek>d__9&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,WeeklyChallengeController.<OnGameStartEvt>d__7>(System.Runtime.CompilerServices.TaskAwaiter&,WeeklyChallengeController.<OnGameStartEvt>d__7&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,WeeklyChallengeController.<SyncWithServer>d__8>(System.Runtime.CompilerServices.TaskAwaiter&,WeeklyChallengeController.<SyncWithServer>d__8&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,WeeklyChallengeGetRewardView.<CliamOnClick>d__9>(System.Runtime.CompilerServices.TaskAwaiter&,WeeklyChallengeGetRewardView.<CliamOnClick>d__9&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,WeeklyChallengeGetRewardView.<OnViewOpen>d__8>(System.Runtime.CompilerServices.TaskAwaiter&,WeeklyChallengeGetRewardView.<OnViewOpen>d__8&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,WinStreakMainPopup.<Jump>d__47>(System.Runtime.CompilerServices.TaskAwaiter&,WinStreakMainPopup.<Jump>d__47&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,WinStreakMainPopup.<PlayJumpAni>d__44>(System.Runtime.CompilerServices.TaskAwaiter&,WinStreakMainPopup.<PlayJumpAni>d__44&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,WinStreakMainPopup.<PlayJumpAniEnd>d__48>(System.Runtime.CompilerServices.TaskAwaiter&,WinStreakMainPopup.<PlayJumpAniEnd>d__48&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,WinStreakMatchPopup.<DoHeadShowAni>d__14>(System.Runtime.CompilerServices.TaskAwaiter&,WinStreakMatchPopup.<DoHeadShowAni>d__14&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,WinStreakMatchPopup.<PlayHeadMatchAnimation>d__11>(System.Runtime.CompilerServices.TaskAwaiter&,WinStreakMatchPopup.<PlayHeadMatchAnimation>d__11&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,WinStreakSuccessPopup.<WaitToSetButtonInteractable>d__12>(System.Runtime.CompilerServices.TaskAwaiter&,WinStreakSuccessPopup.<WaitToSetButtonInteractable>d__12&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,asmr_new.AsmrLevel.<PlayDragTip>d__32>(System.Runtime.CompilerServices.TaskAwaiter&,asmr_new.AsmrLevel.<PlayDragTip>d__32&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter<byte>,ColorFul.Gameplay.ColorfulGameView.<SaveResultTexture>d__42>(System.Runtime.CompilerServices.TaskAwaiter<byte>&,ColorFul.Gameplay.ColorfulGameView.<SaveResultTexture>d__42&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter<byte>,Framework.Fsm.<>c__DisplayClass12_0.<<changeStateAsync>b__1>d>(System.Runtime.CompilerServices.TaskAwaiter<byte>&,Framework.Fsm.<>c__DisplayClass12_0.<<changeStateAsync>b__1>d&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter<byte>,Framework.Fsm.<changeStateAsync>d__12>(System.Runtime.CompilerServices.TaskAwaiter<byte>&,Framework.Fsm.<changeStateAsync>d__12&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter<byte>,PVPBoostSystem.<UseBroom>d__11>(System.Runtime.CompilerServices.TaskAwaiter<byte>&,PVPBoostSystem.<UseBroom>d__11&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter<byte>,PVPBoostSystem.<UseMagnet>d__7>(System.Runtime.CompilerServices.TaskAwaiter<byte>&,PVPBoostSystem.<UseMagnet>d__7&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter<byte>,PVPBoostSystem.<UseWindmill>d__14>(System.Runtime.CompilerServices.TaskAwaiter<byte>&,PVPBoostSystem.<UseWindmill>d__14&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter<byte>,TMatchBoostSystem.<PlayOneSuperLight>d__21>(System.Runtime.CompilerServices.TaskAwaiter<byte>&,TMatchBoostSystem.<PlayOneSuperLight>d__21&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter<byte>,TMatchBoostSystem.<UseBroom>d__12>(System.Runtime.CompilerServices.TaskAwaiter<byte>&,TMatchBoostSystem.<UseBroom>d__12&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter<byte>,TMatchBoostSystem.<UseFrozen>d__18>(System.Runtime.CompilerServices.TaskAwaiter<byte>&,TMatchBoostSystem.<UseFrozen>d__18&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter<byte>,TMatchBoostSystem.<UseLighting>d__19>(System.Runtime.CompilerServices.TaskAwaiter<byte>&,TMatchBoostSystem.<UseLighting>d__19&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter<byte>,TMatchBoostSystem.<UseMagnet>d__8>(System.Runtime.CompilerServices.TaskAwaiter<byte>&,TMatchBoostSystem.<UseMagnet>d__8&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter<byte>,TMatchBoostSystem.<UseSuperLighting>d__20>(System.Runtime.CompilerServices.TaskAwaiter<byte>&,TMatchBoostSystem.<UseSuperLighting>d__20&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter<byte>,TMatchBoostSystem.<UseWindmill>d__15>(System.Runtime.CompilerServices.TaskAwaiter<byte>&,TMatchBoostSystem.<UseWindmill>d__15&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter<object>,LittleGameManager.<LoadLevelIcon>d__34>(System.Runtime.CompilerServices.TaskAwaiter<object>&,LittleGameManager.<LoadLevelIcon>d__34&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.YieldAwaitable.YieldAwaiter,CollectGuildOpeningView.<OnViewOpen>d__3>(System.Runtime.CompilerServices.YieldAwaitable.YieldAwaiter&,CollectGuildOpeningView.<OnViewOpen>d__3&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.YieldAwaitable.YieldAwaiter,DebugOptions.<SendTest>d__277>(System.Runtime.CompilerServices.YieldAwaitable.YieldAwaiter&,DebugOptions.<SendTest>d__277&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.YieldAwaitable.YieldAwaiter,GoldenPass2ItemCell.<WaitToPlayAni>d__52>(System.Runtime.CompilerServices.YieldAwaitable.YieldAwaiter&,GoldenPass2ItemCell.<WaitToPlayAni>d__52&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.YieldAwaitable.YieldAwaiter,GoldenPass3ItemCell.<WaitToPlayAni>d__52>(System.Runtime.CompilerServices.YieldAwaitable.YieldAwaiter&,GoldenPass3ItemCell.<WaitToPlayAni>d__52&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.YieldAwaitable.YieldAwaiter,GoldenPass4ItemCell.<WaitToPlayAni>d__52>(System.Runtime.CompilerServices.YieldAwaitable.YieldAwaiter&,GoldenPass4ItemCell.<WaitToPlayAni>d__52&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.YieldAwaitable.YieldAwaiter,GoldenPass5ItemCell.<WaitToPlayAni>d__52>(System.Runtime.CompilerServices.YieldAwaitable.YieldAwaiter&,GoldenPass5ItemCell.<WaitToPlayAni>d__52&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.YieldAwaitable.YieldAwaiter,GoldenPassItemCell.<WaitToPlayAni>d__52>(System.Runtime.CompilerServices.YieldAwaitable.YieldAwaiter&,GoldenPassItemCell.<WaitToPlayAni>d__52&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.YieldAwaitable.YieldAwaiter,Launching.<DelayNotifyCheckUpdateEvent>d__24>(System.Runtime.CompilerServices.YieldAwaitable.YieldAwaiter&,Launching.<DelayNotifyCheckUpdateEvent>d__24&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.YieldAwaitable.YieldAwaiter,LoginController.<AutoPlay>d__7>(System.Runtime.CompilerServices.YieldAwaitable.YieldAwaiter&,LoginController.<AutoPlay>d__7&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.YieldAwaitable.YieldAwaiter,NetWaittingView.<DelayClose>d__2>(System.Runtime.CompilerServices.YieldAwaitable.YieldAwaiter&,NetWaittingView.<DelayClose>d__2&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.YieldAwaitable.YieldAwaiter,PVPItemSystem.<CreateItemAndSetOriginalPosition>d__18>(System.Runtime.CompilerServices.YieldAwaitable.YieldAwaiter&,PVPItemSystem.<CreateItemAndSetOriginalPosition>d__18&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.YieldAwaitable.YieldAwaiter,PVPItemSystem.<CreateItems>d__15>(System.Runtime.CompilerServices.YieldAwaitable.YieldAwaiter&,PVPItemSystem.<CreateItems>d__15&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.YieldAwaitable.YieldAwaiter,PVPLoadingView.<WaitPlayAudio>d__14>(System.Runtime.CompilerServices.YieldAwaitable.YieldAwaiter&,PVPLoadingView.<WaitPlayAudio>d__14&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.YieldAwaitable.YieldAwaiter,TMatchCreateState.<BoostGuide>d__3>(System.Runtime.CompilerServices.YieldAwaitable.YieldAwaiter&,TMatchCreateState.<BoostGuide>d__3&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.YieldAwaitable.YieldAwaiter,TeamTreasureOpeningView.<OnViewOpen>d__3>(System.Runtime.CompilerServices.YieldAwaitable.YieldAwaiter&,TeamTreasureOpeningView.<OnViewOpen>d__3&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.YieldAwaitable.YieldAwaiter,TripleLineFlyEffect.<MoveAlongBezier>d__6>(System.Runtime.CompilerServices.YieldAwaitable.YieldAwaiter&,TripleLineFlyEffect.<MoveAlongBezier>d__6&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.YieldAwaitable.YieldAwaiter,UILobbyMainViewLevelButton.<RefreshMainBg>d__17>(System.Runtime.CompilerServices.YieldAwaitable.YieldAwaiter&,UILobbyMainViewLevelButton.<RefreshMainBg>d__17&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.YieldAwaitable.YieldAwaiter,UILobbyMainViewLevelButton.<ShowGoldenHatter>d__13>(System.Runtime.CompilerServices.YieldAwaitable.YieldAwaiter&,UILobbyMainViewLevelButton.<ShowGoldenHatter>d__13&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.YieldAwaitable.YieldAwaiter,UILobbyView.<AdaptRootGridLayout>d__21>(System.Runtime.CompilerServices.YieldAwaitable.YieldAwaiter&,UILobbyView.<AdaptRootGridLayout>d__21&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.YieldAwaitable.YieldAwaiter,UIPVPFailedPopup.<RefreshCoinWhenAniEnd>d__12>(System.Runtime.CompilerServices.YieldAwaitable.YieldAwaiter&,UIPVPFailedPopup.<RefreshCoinWhenAniEnd>d__12&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.YieldAwaitable.YieldAwaiter,UIPVPWinPopup.<RefreshCoinWhenAniEnd>d__2>(System.Runtime.CompilerServices.YieldAwaitable.YieldAwaiter&,UIPVPWinPopup.<RefreshCoinWhenAniEnd>d__2&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.YieldAwaitable.YieldAwaiter,UITMatchMainCollectItemView.<OnTripleBoost>d__8>(System.Runtime.CompilerServices.YieldAwaitable.YieldAwaiter&,UITMatchMainCollectItemView.<OnTripleBoost>d__8&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.YieldAwaitable.YieldAwaiter,UITMatchMainController.<FrozenTime>d__70>(System.Runtime.CompilerServices.YieldAwaitable.YieldAwaiter&,UITMatchMainController.<FrozenTime>d__70&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.YieldAwaitable.YieldAwaiter,WaittingView.<DelayClose>d__14>(System.Runtime.CompilerServices.YieldAwaitable.YieldAwaiter&,WaittingView.<DelayClose>d__14&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.YieldAwaitable.YieldAwaiter,WeeklyChallengeController.<CheckStarNextWeek>d__9>(System.Runtime.CompilerServices.YieldAwaitable.YieldAwaiter&,WeeklyChallengeController.<CheckStarNextWeek>d__9&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.YieldAwaitable.YieldAwaiter,WeeklyChallengeView.<MoveTo>d__18>(System.Runtime.CompilerServices.YieldAwaitable.YieldAwaiter&,WeeklyChallengeView.<MoveTo>d__18&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<ASMR.UIGameSuccess.<FlyCallback>d__17>(ASMR.UIGameSuccess.<FlyCallback>d__17&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<ASMRLittleGameSuccess.<FlyCallback>d__17>(ASMRLittleGameSuccess.<FlyCallback>d__17&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<BackGiftMainView.<OnClickClaim>d__11>(BackGiftMainView.<OnClickClaim>d__11&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<BeginnerRebateHelpView.<WaitToClose>d__5>(BeginnerRebateHelpView.<WaitToClose>d__5&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<BeginnerRebateRewardView.<WaitToClose>d__13>(BeginnerRebateRewardView.<WaitToClose>d__13&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<ChooseProgressController.<ReloadUI>d__26>(ChooseProgressController.<ReloadUI>d__26&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<CollectCardFullRewardView.<WaitForClose>d__13>(CollectCardFullRewardView.<WaitForClose>d__13&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<CollectCardGoldCardRewardView.<FlyItem>d__10>(CollectCardGoldCardRewardView.<FlyItem>d__10&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<CollectCardGoldCardRewardView.<OnClickButton>d__9>(CollectCardGoldCardRewardView.<OnClickButton>d__9&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<CollectCardGoldCardRewardView.<Show>d__8>(CollectCardGoldCardRewardView.<Show>d__8&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<CollectCardHelpView.<WaitToClose>d__5>(CollectCardHelpView.<WaitToClose>d__5&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<CollectCardMainView.<TryAutoExchange>d__31>(CollectCardMainView.<TryAutoExchange>d__31&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<CollectCardRewardView.<DoCardOpenAnimation>d__14>(CollectCardRewardView.<DoCardOpenAnimation>d__14&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<CollectCardRewardView.<FlyItemToGate>d__19>(CollectCardRewardView.<FlyItemToGate>d__19&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<CollectCardRewardView.<OnClaimButtonClicked>d__17>(CollectCardRewardView.<OnClaimButtonClicked>d__17&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<CollectCoinGateView.<OnLobbyRefreshShow>d__13>(CollectCoinGateView.<OnLobbyRefreshShow>d__13&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<CollectCoinGateView.<OnTMatchResultExecute>d__15>(CollectCoinGateView.<OnTMatchResultExecute>d__15&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<CollectDiamondGateView.<OnLobbyRefreshShow>d__13>(CollectDiamondGateView.<OnLobbyRefreshShow>d__13&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<CollectDiamondGateView.<OnTMatchResultExecute>d__15>(CollectDiamondGateView.<OnTMatchResultExecute>d__15&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<CollectGuildGateView.<OnTMatchResultExecute>d__16>(CollectGuildGateView.<OnTMatchResultExecute>d__16&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<CollectGuildMainPopup.<>c__DisplayClass32_0.<<OnContinueButtonClicked>b__0>d>(CollectGuildMainPopup.<>c__DisplayClass32_0.<<OnContinueButtonClicked>b__0>d&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<CollectGuildOpeningView.<OnViewOpen>d__3>(CollectGuildOpeningView.<OnViewOpen>d__3&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<ColorFul.Gameplay.ColorfulGameSuccessView.<FlyCallback>d__17>(ColorFul.Gameplay.ColorfulGameSuccessView.<FlyCallback>d__17&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<ColorFul.Gameplay.ColorfulGameView.<SaveResultTexture>d__42>(ColorFul.Gameplay.ColorfulGameView.<SaveResultTexture>d__42&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<DebugOptions.<SendTest>d__277>(DebugOptions.<SendTest>d__277&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<DebugOptions.<createMatchSession>d__272>(DebugOptions.<createMatchSession>d__272&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<EndlessGiftPackMainItem.<TryToOpenBox>d__23>(EndlessGiftPackMainItem.<TryToOpenBox>d__23&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<FBModel.<>c.<<TryAutoOpenFBLikeReward>b__21_0>d>(FBModel.<>c.<<TryAutoOpenFBLikeReward>b__21_0>d&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<FlyNoticeView.<AddNotice>d__8>(FlyNoticeView.<AddNotice>d__8&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<Framework.Fsm.<>c__DisplayClass12_0.<<changeStateAsync>b__1>d>(Framework.Fsm.<>c__DisplayClass12_0.<<changeStateAsync>b__1>d&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<Framework.Fsm.<changeStateAsync>d__12>(Framework.Fsm.<changeStateAsync>d__12&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<GoldenPass2GateView.<OnTMatchResultExecute>d__20>(GoldenPass2GateView.<OnTMatchResultExecute>d__20&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<GoldenPass2GateView.<UpdateProgress>d__15>(GoldenPass2GateView.<UpdateProgress>d__15&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<GoldenPass2HelpPopup.<WaitToClose>d__3>(GoldenPass2HelpPopup.<WaitToClose>d__3&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<GoldenPass2ItemCell.<WaitToPlayAni>d__52>(GoldenPass2ItemCell.<WaitToPlayAni>d__52&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<GoldenPass2MainPopup.<RefreshTableViewAndScroll>d__46>(GoldenPass2MainPopup.<RefreshTableViewAndScroll>d__46&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<GoldenPass3GateView.<OnTMatchResultExecute>d__20>(GoldenPass3GateView.<OnTMatchResultExecute>d__20&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<GoldenPass3GateView.<UpdateProgress>d__15>(GoldenPass3GateView.<UpdateProgress>d__15&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<GoldenPass3HelpPopup.<WaitToClose>d__3>(GoldenPass3HelpPopup.<WaitToClose>d__3&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<GoldenPass3ItemCell.<WaitToPlayAni>d__52>(GoldenPass3ItemCell.<WaitToPlayAni>d__52&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<GoldenPass3MainPopup.<RefreshTableViewAndScroll>d__46>(GoldenPass3MainPopup.<RefreshTableViewAndScroll>d__46&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<GoldenPass4GateView.<OnTMatchResultExecute>d__20>(GoldenPass4GateView.<OnTMatchResultExecute>d__20&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<GoldenPass4GateView.<UpdateProgress>d__15>(GoldenPass4GateView.<UpdateProgress>d__15&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<GoldenPass4HelpPopup.<WaitToClose>d__3>(GoldenPass4HelpPopup.<WaitToClose>d__3&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<GoldenPass4ItemCell.<WaitToPlayAni>d__52>(GoldenPass4ItemCell.<WaitToPlayAni>d__52&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<GoldenPass4MainPopup.<RefreshTableViewAndScroll>d__46>(GoldenPass4MainPopup.<RefreshTableViewAndScroll>d__46&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<GoldenPass5GateView.<OnTMatchResultExecute>d__23>(GoldenPass5GateView.<OnTMatchResultExecute>d__23&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<GoldenPass5GateView.<UpdateProgress>d__18>(GoldenPass5GateView.<UpdateProgress>d__18&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<GoldenPass5HelpPopup.<WaitToClose>d__3>(GoldenPass5HelpPopup.<WaitToClose>d__3&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<GoldenPass5ItemCell.<WaitToPlayAni>d__52>(GoldenPass5ItemCell.<WaitToPlayAni>d__52&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<GoldenPass5MainPopup.<RefreshTableViewAndScroll>d__46>(GoldenPass5MainPopup.<RefreshTableViewAndScroll>d__46&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<GoldenPassGateView.<OnTMatchResultExecute>d__23>(GoldenPassGateView.<OnTMatchResultExecute>d__23&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<GoldenPassGateView.<UpdateProgress>d__18>(GoldenPassGateView.<UpdateProgress>d__18&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<GoldenPassHelpPopup.<WaitToClose>d__3>(GoldenPassHelpPopup.<WaitToClose>d__3&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<GoldenPassItemCell.<WaitToPlayAni>d__52>(GoldenPassItemCell.<WaitToPlayAni>d__52&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<GoldenPassMainPopup.<RefreshTableViewAndScroll>d__46>(GoldenPassMainPopup.<RefreshTableViewAndScroll>d__46&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<HalloweenShibaInuGateView.<OnTMatchResultExecute>d__13>(HalloweenShibaInuGateView.<OnTMatchResultExecute>d__13&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<HalloweenShibaInuHelpPopup.<WaitToClose>d__5>(HalloweenShibaInuHelpPopup.<WaitToClose>d__5&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<Launching.<DelayNotifyCheckUpdateEvent>d__24>(Launching.<DelayNotifyCheckUpdateEvent>d__24&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<Launching.<startLaunchSequence>d__15>(Launching.<startLaunchSequence>d__15&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<LeaderBoardManager.<startTimerTick>d__6>(LeaderBoardManager.<startTimerTick>d__6&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<LittleGameManager.<LoadLevelIcon>d__34>(LittleGameManager.<LoadLevelIcon>d__34&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<LobbyTaskSystem.<CheckResultLastFinsih>d__16>(LobbyTaskSystem.<CheckResultLastFinsih>d__16&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<LoginController.<AutoPlay>d__7>(LoginController.<AutoPlay>d__7&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<NetWaittingView.<DelayClose>d__2>(NetWaittingView.<DelayClose>d__2&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<OneLine.OneLineGameSuccessView.<FlyCallback>d__17>(OneLine.OneLineGameSuccessView.<FlyCallback>d__17&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<PVPActivityController.<CreateMatchSession>d__40>(PVPActivityController.<CreateMatchSession>d__40&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<PVPActivityController.<GetRankInfo>d__60>(PVPActivityController.<GetRankInfo>d__60&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<PVPBaseItem.<Retract>d__49>(PVPBaseItem.<Retract>d__49&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<PVPBoostSystem.<StopPropEffect>d__15>(PVPBoostSystem.<StopPropEffect>d__15&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<PVPBoostSystem.<UseBroom>d__11>(PVPBoostSystem.<UseBroom>d__11&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<PVPBoostSystem.<UseMagnet>d__7>(PVPBoostSystem.<UseMagnet>d__7&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<PVPBoostSystem.<UseWindmill>d__14>(PVPBoostSystem.<UseWindmill>d__14&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<PVPItemSystem.<CreateItemAndSetOriginalPosition>d__18>(PVPItemSystem.<CreateItemAndSetOriginalPosition>d__18&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<PVPItemSystem.<CreateItems>d__15>(PVPItemSystem.<CreateItems>d__15&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<PVPLoadingView.<DoHeadFlyAndClose>d__16>(PVPLoadingView.<DoHeadFlyAndClose>d__16&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<PVPLoadingView.<WaitPlayAudio>d__14>(PVPLoadingView.<WaitPlayAudio>d__14&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<PVPNetWork.<CreateFightSession>d__7>(PVPNetWork.<CreateFightSession>d__7&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<PVPSystem.<CheckAllPrepared>d__26>(PVPSystem.<CheckAllPrepared>d__26&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<PVPSystem.<GameBegin>d__27>(PVPSystem.<GameBegin>d__27&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<PVPSystem.<GameTimeOut>d__32>(PVPSystem.<GameTimeOut>d__32&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<PVPSystem.<ShowGameResult>d__33>(PVPSystem.<ShowGameResult>d__33&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<PVPTaskItemView.<DoMoveXAnimation>d__7>(PVPTaskItemView.<DoMoveXAnimation>d__7&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<PVPTaskItemView.<RefreshShow>d__6>(PVPTaskItemView.<RefreshShow>d__6&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<PiggyBankGateView.<OnTMatchResultExecute>d__13>(PiggyBankGateView.<OnTMatchResultExecute>d__13&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<PipeSuccessView.<FlyCallback>d__17>(PipeSuccessView.<FlyCallback>d__17&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<PointToPointSuccessView.<FlyCallback>d__17>(PointToPointSuccessView.<FlyCallback>d__17&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<RankModel.<GetRankInfo>d__12>(RankModel.<GetRankInfo>d__12&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<RechargeGiftPackActivityModel.<>c__DisplayClass13_0.<<ShowGetRewardView>b__0>d>(RechargeGiftPackActivityModel.<>c__DisplayClass13_0.<<ShowGetRewardView>b__0>d&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<RechargeGiftPackMainItem.<PlayBuySuccessAni>d__21>(RechargeGiftPackMainItem.<PlayBuySuccessAni>d__21&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<RechargeGiftPackMainItem.<PlayUnlockAni>d__22>(RechargeGiftPackMainItem.<PlayUnlockAni>d__22&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<RechargeGiftPackMainPopup.<OnBuySuccess>d__16>(RechargeGiftPackMainPopup.<OnBuySuccess>d__16&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<RoseJourneyGateView.<FlyAddItem>d__17>(RoseJourneyGateView.<FlyAddItem>d__17&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<RoseJourneyGateView.<JumpToType>d__19>(RoseJourneyGateView.<JumpToType>d__19&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<RoseJourneyGateView.<OnTMatchResultExecute>d__16>(RoseJourneyGateView.<OnTMatchResultExecute>d__16&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<RoseJourneyHelpView.<WaitToClose>d__3>(RoseJourneyHelpView.<WaitToClose>d__3&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<ShibaInuGateView.<OnTMatchResultExecute>d__13>(ShibaInuGateView.<OnTMatchResultExecute>d__13&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<ShibaInuHelpPopup.<WaitToClose>d__5>(ShibaInuHelpPopup.<WaitToClose>d__5&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<SpeedRaceGateView.<OnTMatchResultExecute>d__15>(SpeedRaceGateView.<OnTMatchResultExecute>d__15&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<SpeedRaceHelpPopup.<WaitToClose>d__5>(SpeedRaceHelpPopup.<WaitToClose>d__5&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<SpeedRaceMainItem.<>c__DisplayClass24_0.<<PlayScoreAin>g__UpdateScoreOnce|0>d>(SpeedRaceMainItem.<>c__DisplayClass24_0.<<PlayScoreAin>g__UpdateScoreOnce|0>d&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<SpeedRaceMainItem.<WaitToPlayWinAni>d__23>(SpeedRaceMainItem.<WaitToPlayWinAni>d__23&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<SpeedRaceMainPopup.<PlayMoveAnimation>d__26>(SpeedRaceMainPopup.<PlayMoveAnimation>d__26&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<StarChallengeGateView.<OnTMatchResultExecute>d__16>(StarChallengeGateView.<OnTMatchResultExecute>d__16&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<StarChallengeMainPopup.<>c.<<OnClaimRewardClicked>b__28_0>d>(StarChallengeMainPopup.<>c.<<OnClaimRewardClicked>b__28_0>d&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<StateDecoration.<Framework-IFsmState-EnterFinish>d__4>(StateDecoration.<Framework-IFsmState-EnterFinish>d__4&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<SuperLightGateView.<OnTMatchResultExecute>d__12>(SuperLightGateView.<OnTMatchResultExecute>d__12&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<SuperLightHelpView.<WaitToClose>d__4>(SuperLightHelpView.<WaitToClose>d__4&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<SuperWinstreakGateView.<OnTMatchResultExecute>d__13>(SuperWinstreakGateView.<OnTMatchResultExecute>d__13&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<TMatchBaseItem.<Retract>d__48>(TMatchBaseItem.<Retract>d__48&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<TMatchBoostSystem.<PlayOneSuperLight>d__21>(TMatchBoostSystem.<PlayOneSuperLight>d__21&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<TMatchBoostSystem.<UseBroom>d__12>(TMatchBoostSystem.<UseBroom>d__12&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<TMatchBoostSystem.<UseClock>d__22>(TMatchBoostSystem.<UseClock>d__22&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<TMatchBoostSystem.<UseFrozen>d__18>(TMatchBoostSystem.<UseFrozen>d__18&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<TMatchBoostSystem.<UseLighting>d__19>(TMatchBoostSystem.<UseLighting>d__19&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<TMatchBoostSystem.<UseMagnet>d__8>(TMatchBoostSystem.<UseMagnet>d__8&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<TMatchBoostSystem.<UseSuperLighting>d__20>(TMatchBoostSystem.<UseSuperLighting>d__20&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<TMatchBoostSystem.<UseWindmill>d__15>(TMatchBoostSystem.<UseWindmill>d__15&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<TMatchCreateState.<BoostGuide>d__3>(TMatchCreateState.<BoostGuide>d__3&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<TMatchGoldenHatterSystem.<OnGameStartEvt>d__5>(TMatchGoldenHatterSystem.<OnGameStartEvt>d__5&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<TMatchLevelBoostSystem.<OnGameStartEvt>d__2>(TMatchLevelBoostSystem.<OnGameStartEvt>d__2&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<TMatchLevelBoostSystem.<UseLevelBoostClock>d__3>(TMatchLevelBoostSystem.<UseLevelBoostClock>d__3&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<TMatchLevelBoostSystem.<UseLevelBoostLighting>d__4>(TMatchLevelBoostSystem.<UseLevelBoostLighting>d__4&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<TMatchNormalLevelController.<OnWin>d__13>(TMatchNormalLevelController.<OnWin>d__13&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<TMatchStateSystem.<ChangeStateAsync>d__8>(TMatchStateSystem.<ChangeStateAsync>d__8&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<TeamManager.<startTimerTick>d__12>(TeamManager.<startTimerTick>d__12&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<TeamPassGift.<WaitToSetHelpButtonEnabled>d__14>(TeamPassGift.<WaitToSetHelpButtonEnabled>d__14&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<TeamTreasureGateView.<OnTMatchResultExecute>d__15>(TeamTreasureGateView.<OnTMatchResultExecute>d__15&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<TeamTreasureMainPopup.<>c__DisplayClass31_0.<<OnContinueButtonClicked>b__0>d>(TeamTreasureMainPopup.<>c__DisplayClass31_0.<<OnContinueButtonClicked>b__0>d&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<TeamTreasureOpeningView.<OnViewOpen>d__3>(TeamTreasureOpeningView.<OnViewOpen>d__3&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<TeamTreasureSliderView.<>c__DisplayClass8_0.<<OnOpenButtonClicked>b__0>d>(TeamTreasureSliderView.<>c__DisplayClass8_0.<<OnOpenButtonClicked>b__0>d&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<TreasureHuntGateView.<OnTMatchResultExecute>d__15>(TreasureHuntGateView.<OnTMatchResultExecute>d__15&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<TripleLineFlyEffect.<MoveAlongBezier>d__6>(TripleLineFlyEffect.<MoveAlongBezier>d__6&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<UILevelChestPopup.<DelayShowText>d__6>(UILevelChestPopup.<DelayShowText>d__6&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<UILobbyMainViewAsmr.<OnLobbyRefreshShow>d__14>(UILobbyMainViewAsmr.<OnLobbyRefreshShow>d__14&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<UILobbyMainViewLevelButton.<OnLobbyRefreshShow>d__15>(UILobbyMainViewLevelButton.<OnLobbyRefreshShow>d__15&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<UILobbyMainViewLevelButton.<RefreshMainBg>d__17>(UILobbyMainViewLevelButton.<RefreshMainBg>d__17&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<UILobbyMainViewLevelButton.<ShowGoldenHatter>d__13>(UILobbyMainViewLevelButton.<ShowGoldenHatter>d__13&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<UILobbyMainViewLevelChest.<OnTMatchResultExecute>d__15>(UILobbyMainViewLevelChest.<OnTMatchResultExecute>d__15&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<UILobbyMainViewLevelChest.<UpdateLevelChestProgress>d__10>(UILobbyMainViewLevelChest.<UpdateLevelChestProgress>d__10&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<UILobbyMainViewLittleGame.<OnLobbyRefreshShow>d__16>(UILobbyMainViewLittleGame.<OnLobbyRefreshShow>d__16&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<UILobbyMainViewSignButton.<OnLobbyRefreshShow>d__11>(UILobbyMainViewSignButton.<OnLobbyRefreshShow>d__11&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<UILobbyMainViewStarChest.<OnTMatchResultExecute>d__16>(UILobbyMainViewStarChest.<OnTMatchResultExecute>d__16&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<UILobbyView.<AdaptRootGridLayout>d__21>(UILobbyView.<AdaptRootGridLayout>d__21&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<UIPVPCountdownView.<PlayAniAndClose>d__1>(UIPVPCountdownView.<PlayAniAndClose>d__1&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<UIPVPFailedPopup.<OnContinueButtonClicked>d__14>(UIPVPFailedPopup.<OnContinueButtonClicked>d__14&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<UIPVPFailedPopup.<RefreshCoinWhenAniEnd>d__12>(UIPVPFailedPopup.<RefreshCoinWhenAniEnd>d__12&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<UIPVPSpaceOutPopup.<ContinueOnClick>d__12>(UIPVPSpaceOutPopup.<ContinueOnClick>d__12&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<UIPVPWinPopup.<OnContinueButtonClicked>d__3>(UIPVPWinPopup.<OnContinueButtonClicked>d__3&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<UIPVPWinPopup.<RefreshCoinWhenAniEnd>d__2>(UIPVPWinPopup.<RefreshCoinWhenAniEnd>d__2&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<UIRemoveAdPopup.<DelayRefreshBuyText>d__15>(UIRemoveAdPopup.<DelayRefreshBuyText>d__15&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<UIRemoveAdPopup.<OnCloseButtonClicked>d__17>(UIRemoveAdPopup.<OnCloseButtonClicked>d__17&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<UISaveYourProgressController.<RefreshButtonTextAndReward>d__22>(UISaveYourProgressController.<RefreshButtonTextAndReward>d__22&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<UISignInController.<>c.<<ShowGetRewardView>b__27_0>d>(UISignInController.<>c.<<ShowGetRewardView>b__27_0>d&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<UISignInController.<OnClaimButtonClicked>d__21>(UISignInController.<OnClaimButtonClicked>d__21&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<UITMatchBuyProps.<BuyOnClick>d__9>(UITMatchBuyProps.<BuyOnClick>d__9&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<UITMatchLevelBoostClock.<WaitToDoClose>d__5>(UITMatchLevelBoostClock.<WaitToDoClose>d__5&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<UITMatchMainCollectItemView.<OnTripleBoost>d__8>(UITMatchMainCollectItemView.<OnTripleBoost>d__8&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<UITMatchMainController.<FrozenTime>d__70>(UITMatchMainController.<FrozenTime>d__70&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<UITMatchMainController.<OnLowSpaceEvt>d__48>(UITMatchMainController.<OnLowSpaceEvt>d__48&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<UITMatchMainController.<OnTriple>d__56>(UITMatchMainController.<OnTriple>d__56&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<UITMatchSpaceOutController.<ContinueOnClick>d__24>(UITMatchSpaceOutController.<ContinueOnClick>d__24&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<UITMatchTimeOutController.<ContinueOnClick>d__24>(UITMatchTimeOutController.<ContinueOnClick>d__24&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<UITreasureHuntHelp.<WaitToClose>d__10>(UITreasureHuntHelp.<WaitToClose>d__10&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<UITreasureHuntMain.<OnClickAtGrid>d__48>(UITreasureHuntMain.<OnClickAtGrid>d__48&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<UIViewSystem.<OnClose>d__10>(UIViewSystem.<OnClose>d__10&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<UiLeaderBoardMainView.<autoRecoverPullingCoolDown>d__54>(UiLeaderBoardMainView.<autoRecoverPullingCoolDown>d__54&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<UiLeaderBoardMainView.<onPullGlobalCallback>d__102>(UiLeaderBoardMainView.<onPullGlobalCallback>d__102&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<UiLeaderBoardMainView.<onPullTeamCallback>d__192>(UiLeaderBoardMainView.<onPullTeamCallback>d__192&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<UiLeaderBoardMainView.<showGlobalTooltip>d__105>(UiLeaderBoardMainView.<showGlobalTooltip>d__105&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<UiLeaderBoardMainView.<showTeamTooltip>d__194>(UiLeaderBoardMainView.<showTeamTooltip>d__194&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<UiOutLivesController.<DelayShowText>d__5>(UiOutLivesController.<DelayShowText>d__5&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<UiOutLivesController.<OnRefillButtonClicked>d__10>(UiOutLivesController.<OnRefillButtonClicked>d__10&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<UiTeamDetailController.<autoRecoverJoinButton>d__28>(UiTeamDetailController.<autoRecoverJoinButton>d__28&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<UiTeamDetailController.<showDetailTooltip>d__29>(UiTeamDetailController.<showDetailTooltip>d__29&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<UiTeamMainController.<autoRecoverAskHelpButton>d__43>(UiTeamMainController.<autoRecoverAskHelpButton>d__43&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<UiTeamMainController.<autoRecoverCreateButton>d__78>(UiTeamMainController.<autoRecoverCreateButton>d__78&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<UiTeamMainController.<autoRecoverPullingCoolDown>d__192>(UiTeamMainController.<autoRecoverPullingCoolDown>d__192&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<UiTeamMainController.<autoRecoverSaveButton>d__172>(UiTeamMainController.<autoRecoverSaveButton>d__172&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<UiTeamMainController.<autoRecoverSearchButton>d__214>(UiTeamMainController.<autoRecoverSearchButton>d__214&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<UiTeamMainController.<onChatUIShow>d__30>(UiTeamMainController.<onChatUIShow>d__30&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<UiTeamMainController.<onPullCallback>d__191>(UiTeamMainController.<onPullCallback>d__191&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<UiTeamMainController.<showChatTooltip>d__42>(UiTeamMainController.<showChatTooltip>d__42&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<UiTeamMainController.<showCreateLevelTooltip>d__76>(UiTeamMainController.<showCreateLevelTooltip>d__76&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<UiTeamMainController.<showCreateNameTooltip>d__77>(UiTeamMainController.<showCreateNameTooltip>d__77&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<UiTeamMainController.<showModifyTooltip>d__173>(UiTeamMainController.<showModifyTooltip>d__173&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<UiTeamMainController.<showSearchTooltip>d__213>(UiTeamMainController.<showSearchTooltip>d__213&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<WaittingView.<DelayClose>d__14>(WaittingView.<DelayClose>d__14&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<WeeklyChallengeController.<CheckStarNextWeek>d__9>(WeeklyChallengeController.<CheckStarNextWeek>d__9&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<WeeklyChallengeController.<OnGameStartEvt>d__7>(WeeklyChallengeController.<OnGameStartEvt>d__7&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<WeeklyChallengeController.<SyncWithServer>d__8>(WeeklyChallengeController.<SyncWithServer>d__8&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<WeeklyChallengeGetRewardView.<CliamOnClick>d__9>(WeeklyChallengeGetRewardView.<CliamOnClick>d__9&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<WeeklyChallengeGetRewardView.<OnViewOpen>d__8>(WeeklyChallengeGetRewardView.<OnViewOpen>d__8&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<WeeklyChallengeView.<MoveTo>d__18>(WeeklyChallengeView.<MoveTo>d__18&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<WinStreakGateView.<UpdateProgress>d__10>(WinStreakGateView.<UpdateProgress>d__10&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<WinStreakMainPopup.<Jump>d__47>(WinStreakMainPopup.<Jump>d__47&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<WinStreakMainPopup.<PlayJumpAni>d__44>(WinStreakMainPopup.<PlayJumpAni>d__44&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<WinStreakMainPopup.<PlayJumpAniEnd>d__48>(WinStreakMainPopup.<PlayJumpAniEnd>d__48&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<WinStreakMatchPopup.<DoHeadShowAni>d__14>(WinStreakMatchPopup.<DoHeadShowAni>d__14&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<WinStreakMatchPopup.<PlayHeadMatchAnimation>d__11>(WinStreakMatchPopup.<PlayHeadMatchAnimation>d__11&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<WinStreakSuccessPopup.<OnCloseButtonClicked>d__16>(WinStreakSuccessPopup.<OnCloseButtonClicked>d__16&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<WinStreakSuccessPopup.<WaitToSetButtonInteractable>d__12>(WinStreakSuccessPopup.<WaitToSetButtonInteractable>d__12&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<asmr_new.AsmrLevel.<PlayDragTip>d__32>(asmr_new.AsmrLevel.<PlayDragTip>d__32&)
		// object& System.Runtime.CompilerServices.Unsafe.As<object,object>(object&)
		// System.Void* System.Runtime.CompilerServices.Unsafe.AsPointer<object>(object&)
		// System.Collections.Generic.Dictionary<object,object> SystemCollectionsExtension.Merge<object,object>(System.Collections.Generic.Dictionary<object,object>,System.Collections.Generic.Dictionary<object,object>[])
		// System.Void* Unity.Collections.LowLevel.Unsafe.UnsafeUtility.AddressOf<TriJob>(TriJob&)
		// System.Void* Unity.Collections.LowLevel.Unsafe.UnsafeUtility.AddressOf<VtJob>(VtJob&)
		// Unity.Jobs.JobHandle Unity.Jobs.IJobParallelForExtensions.Schedule<TriJob>(TriJob,int,int,Unity.Jobs.JobHandle)
		// Unity.Jobs.JobHandle Unity.Jobs.IJobParallelForExtensions.Schedule<VtJob>(VtJob,int,int,Unity.Jobs.JobHandle)
		// object UnityEngine.AndroidJNIHelper.ConvertFromJNIArray<object>(System.IntPtr)
		// System.IntPtr UnityEngine.AndroidJNIHelper.GetFieldID<object>(System.IntPtr,string,bool)
		// System.IntPtr UnityEngine.AndroidJNIHelper.GetMethodID<object>(System.IntPtr,string,object[],bool)
		// object UnityEngine.AndroidJavaObject.Call<object>(string,object[])
		// object UnityEngine.AndroidJavaObject.FromJavaArrayDeleteLocalRef<object>(System.IntPtr)
		// object UnityEngine.AndroidJavaObject.GetStatic<object>(string)
		// object UnityEngine.AndroidJavaObject._Call<object>(string,object[])
		// object UnityEngine.AndroidJavaObject._GetStatic<object>(string)
		// object UnityEngine.Component.GetComponent<object>()
		// object UnityEngine.Component.GetComponentInChildren<object>()
		// object UnityEngine.Component.GetComponentInParent<object>()
		// object[] UnityEngine.Component.GetComponents<object>()
		// System.Void UnityEngine.Component.GetComponentsInChildren<object>(System.Collections.Generic.List<object>)
		// System.Void UnityEngine.Component.GetComponentsInChildren<object>(bool,System.Collections.Generic.List<object>)
		// object[] UnityEngine.Component.GetComponentsInChildren<object>()
		// object[] UnityEngine.Component.GetComponentsInChildren<object>(bool)
		// bool UnityEngine.EventSystems.ExecuteEvents.Execute<object>(UnityEngine.GameObject,UnityEngine.EventSystems.BaseEventData,UnityEngine.EventSystems.ExecuteEvents.EventFunction<object>)
		// UnityEngine.GameObject UnityEngine.EventSystems.ExecuteEvents.ExecuteHierarchy<object>(UnityEngine.GameObject,UnityEngine.EventSystems.BaseEventData,UnityEngine.EventSystems.ExecuteEvents.EventFunction<object>)
		// System.Void UnityEngine.EventSystems.ExecuteEvents.GetEventList<object>(UnityEngine.GameObject,System.Collections.Generic.IList<UnityEngine.EventSystems.IEventSystemHandler>)
		// bool UnityEngine.EventSystems.ExecuteEvents.ShouldSendToComponent<object>(UnityEngine.Component)
		// object UnityEngine.GameObject.AddComponent<object>()
		// object UnityEngine.GameObject.GetComponent<object>()
		// object UnityEngine.GameObject.GetComponentInChildren<object>()
		// object UnityEngine.GameObject.GetComponentInChildren<object>(bool)
		// object UnityEngine.GameObject.GetComponentInParent<object>()
		// object UnityEngine.GameObject.GetComponentInParent<object>(bool)
		// System.Void UnityEngine.GameObject.GetComponents<object>(System.Collections.Generic.List<object>)
		// object[] UnityEngine.GameObject.GetComponents<object>()
		// System.Void UnityEngine.GameObject.GetComponentsInChildren<object>(bool,System.Collections.Generic.List<object>)
		// object[] UnityEngine.GameObject.GetComponentsInChildren<object>()
		// object[] UnityEngine.GameObject.GetComponentsInChildren<object>(bool)
		// object UnityEngine.JsonUtility.FromJson<object>(string)
		// object[] UnityEngine.Object.FindObjectsOfType<object>()
		// object UnityEngine.Object.Instantiate<object>(object)
		// object UnityEngine.Object.Instantiate<object>(object,UnityEngine.Transform)
		// object UnityEngine.Object.Instantiate<object>(object,UnityEngine.Transform,bool)
		// object UnityEngine.Object.Instantiate<object>(object,UnityEngine.Vector3,UnityEngine.Quaternion)
		// object UnityEngine.Object.Instantiate<object>(object,UnityEngine.Vector3,UnityEngine.Quaternion,UnityEngine.Transform)
		// object[] UnityEngine.Resources.ConvertObjects<object>(UnityEngine.Object[])
		// object UnityEngine.Resources.Load<object>(string)
		// object UnityEngine._AndroidJNIHelper.ConvertFromJNIArray<object>(System.IntPtr)
		// System.IntPtr UnityEngine._AndroidJNIHelper.GetFieldID<object>(System.IntPtr,string,bool)
		// System.IntPtr UnityEngine._AndroidJNIHelper.GetMethodID<object>(System.IntPtr,string,object[],bool)
		// string UnityEngine._AndroidJNIHelper.GetSignature<object>(object[])
		// object UnityGameFramework.Runtime.ResourceComponent.LoadAsset<object>(string,string)
		// Cysharp.Threading.Tasks.UniTask<object> UnityGameFramework.Runtime.ResourceComponent.LoadAssetAsync<object>(string,System.Threading.CancellationToken,string)
		// string string.Join<int>(string,System.Collections.Generic.IEnumerable<int>)
		// string string.JoinCore<int>(System.Char*,int,System.Collections.Generic.IEnumerable<int>)
	}
}