#if UNITY_EDITOR
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuTools : MonoBehaviour {
	[MenuItem("Assets/Save All Assets")]
	static void SaveAllAssets() {
		AssetDatabase.SaveAssets();
		AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
	}
}
#endif