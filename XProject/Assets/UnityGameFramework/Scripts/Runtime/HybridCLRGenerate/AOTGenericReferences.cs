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
	public void RefMethods()
	{
	}
	
}
