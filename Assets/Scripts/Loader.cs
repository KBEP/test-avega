using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

// Компонент для загрузки и перезагрузки основной сцены. Вещается на любой объект инициализирующей сцены.

public class Loader : MonoBehaviour
{
	public static Loader Instance { get; private set; }

	void Awake ()
	{
		if (Instance != null) Destroy(this);
		else
		{
			Instance = this;
			CmdReloadScene();
		}
	}
	
	public void CmdReloadScene ()
	{
		StartCoroutine(CrtLoadLevel());
	}

	IEnumerator CrtLoadLevel ()
	{
		AsyncOperation op = null;
		try
		{
			op = SceneManager.LoadSceneAsync("Test", LoadSceneMode.Single);
		}
		catch
		{
			Debug.Log("ERROR");
			yield break;
		}
		//проверяем, т. к. SceneManager.LoadSceneAsync не выбрасывает исключения, если сцена не добавлена в билд,
		//а просто возвращает null
		if (op == null)
		{
			Debug.Log("ERROR");
			yield break;
		}
		yield return StartCoroutine(CrtWaiting(op));
		GC.Collect();
		Resources.UnloadUnusedAssets();
	}

	IEnumerator CrtWaiting (AsyncOperation op)
	{
		while (!op.isDone) yield return null;
	}
}
