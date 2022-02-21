using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner
{
	public static readonly string defaultColor = "Red";

	static readonly string path = "Prefabs/Bullets/";
	static readonly string[] colorNames = new string[] { defaultColor, "Green", "Yellow" };
	static readonly Dictionary<string, GameObject> prefabs = new Dictionary<string, GameObject>();

	public static bool Spawn (string colorName, Transform muzzle)
	  => muzzle != null && Spawn(GetPrefab(path, colorName), muzzle.position, muzzle.rotation);

	public static bool IsColorValid (string colorName) => System.Array.IndexOf<string>(colorNames, colorName) != -1;

	static GameObject GetPrefab (string path, string colorName)
	{
		if (!IsColorValid(colorName)) return null;
		GameObject prefab;
		if (prefabs.TryGetValue(colorName, out prefab)) return prefab;
		
		prefab = Resources.Load<GameObject>(path + colorName);
		if (prefab == null) return null;
		prefabs.Add(colorName, prefab);
		return prefab;
	}

	static bool Spawn (GameObject prefab, Vector3 pos, Quaternion rot)
	{
		if (prefab == null) return false;
		GameObject go = GameObject.Instantiate<GameObject>(prefab, pos, rot);
		if (go == null) return false;
		go.name = prefab.name;
		return true;
	}
}
