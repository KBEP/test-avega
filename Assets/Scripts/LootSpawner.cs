using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public static class LootSpawner
{
	public static readonly string defaultColor = "Red";

	static readonly string lootPath = "Prefabs/Loot/";
	static readonly string[] colorNames = new string[] { defaultColor, "Green", "Yellow" };
	static readonly Dictionary<string, GameObject> prefabs = new Dictionary<string, GameObject>();

	public static readonly ReadOnlyCollection<string> ColorNames = new ReadOnlyCollection<string>(colorNames);

	public static bool SpawnRandomLoot (Vector3 pos)
	{
		string randomName = colorNames[Random.Range(0, colorNames.Length)];
		return Spawn(GetPrefab(lootPath, randomName), pos);
	}

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

	static bool Spawn (GameObject prefab, Vector3 pos)
	{
		if (prefab == null) return false;
		GameObject go = GameObject.Instantiate<GameObject>(prefab, pos, Quaternion.identity);
		if (go == null) return false;
		go.name = prefab.name;
		return true;
	}
}
