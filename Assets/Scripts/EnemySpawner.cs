using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
	WaitForSeconds spawnDelay = new WaitForSeconds(3.0f);
	Coroutine crtSpawn;
	GameObject enemyPrefab;
	int limit = 5;//лимит врагов на сцене
	int count;//сколько всего врагов на сцене

	BotPositionProvider posProvider;//отсюда берем свободные позиции для спауна
	
	void Awake ()
	{
		enemyPrefab = Resources.Load<GameObject>("Prefabs/Enemies/Enemy");
		if (enemyPrefab == null) Debug.Log("ERROR");

		posProvider = GetComponent<BotPositionProvider>();
		if (posProvider == null) Debug.Log("ERROR");
	}

	void OnEnable ()
	{
		if (crtSpawn != null) StopCoroutine(crtSpawn);//остановить спаун врагов, если он происходит
		if (enemyPrefab == null) Debug.Log("ERROR");
		else crtSpawn = StartCoroutine(CrtSpawn());//начать спаун врагов
	}

	void OnDisable ()
	{
		if (crtSpawn != null) StopCoroutine(crtSpawn);//остановить спаун врагов
	}

	IEnumerator CrtSpawn ()//эта корутина спаунит врагов
	{
		for (;;)
		{
			yield return spawnDelay;
			if (count >= limit) continue;
			if (enemyPrefab == null)
			{
				Debug.Log("ERROR");
				continue;
			}
			Vector3 randomPos;
			if (!TryGetRandomPos(out randomPos))//нет возможности заспаунить врага, игрок полностью видит сцену
			{
				//Debug.Log("No spawning position.");
				continue;
			}
			GameObject enemy = GameObject.Instantiate<GameObject>(enemyPrefab, randomPos, Quaternion.identity);
			if (enemy == null)
			{
				Debug.Log("ERROR");
				continue;
			}
			enemy.name = enemyPrefab.name;
			EnemyController ctr = enemy.GetComponent<EnemyController>();
			if (ctr == null)
			{
				Debug.Log("ERROR");
				continue;
			}
			ctr.Destroyed += OnEnemyDestroyed;//для контроля поголовья врагов
			count++;
		}
	}

	//попытаться получить позицию для спауна врага за спиной игрока, чтобы он его не видел
	bool TryGetRandomPos (out Vector3 result)
	{
		if (posProvider == null)
		{
			result = Vector3.zero;
			Debug.Log("ERROR");
			return false;
		}
		Transform tCam = BotNet.Target?.CameraT;
		if (tCam == null)
		{
			result = Vector3.zero;
			Debug.Log("ERROR");
			return false;
		}
		return posProvider.TryGetRandomPos(tCam.position, tCam.forward, out result);
	}

	void OnEnemyDestroyed ()
	{
		if (--count < 0) count = 0;
	}
}
