using UnityEngine;
using System.Collections.Generic;

public static class EM_Transform
{
	public static T GetChildComponent<T> (this Transform t, string path) where T : Component
	{
		Transform child = t.Find(path);
		return child != null ? child.GetComponent<T>() : null;
	}

	public static void DestroyChildren (this Transform t)
	{
		int i = t.childCount;
		while (i != 0) GameObject.Destroy(t.GetChild(--i).gameObject);
		t.DetachChildren();
	}

	public static void ActivateChildren (this Transform t)
	{
		int childCount = t.childCount;
		for (int i = 0; i < childCount; ++i) t.GetChild(i).gameObject.SetActive(true);
	}

	public static void DeactivateChildren (this Transform t)
	{
		int childCount = t.childCount;
		for (int i = 0; i < childCount; ++i) t.GetChild(i).gameObject.SetActive(false);
	}

	public static List<T> GetChildrenComponents<T> (this Transform t) where T : Component
	{
		List<T> result = new List<T>();
		int childCount = t.childCount;
		for (int i = 0; i < childCount; ++i)
		{
			T c = t.GetChild(i).GetComponent<T>();
			if (c != null) result.Add(c);
		}
		return result;
	}
}
