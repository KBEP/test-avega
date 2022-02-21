using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// Генерирует и кэширует позиции пригодные для спауна на навмеше.

public class BotPositionProvider : MonoBehaviour
{
	List<Vector3> cachedPos = new List<Vector3>();//кэш для хранения найденых позиций пригодных для спауна
	int limit = 20;//лимит кэша
	int tryCount = 30;//количество попыток сгенерировать позицию "форсировано", если нет подходяшей позиции в cachedPos
	Bounds bounds;//примерные границы навмеша в пределах которых будет генерироваться позиция
	float maxDistance = 4.0f;//для NavMesh.SamplePosition
	int mask;//для NavMesh.SamplePosition

	void Awake ()
	{
		//устанавливаем маску
		int areaIdx = NavMesh.GetAreaFromName("Walkable");
		if (areaIdx == -1) Debug.Log("ERROR");
		else mask = 1 << areaIdx;
		
		//устанавливаем границы навмеша на основе объекта NavMeshBounds на сцене
		GameObject navMeshBounds = GameObject.Find("NavMeshBounds");
		if (navMeshBounds == null) Debug.Log("ERROR");
		else
		{
			Renderer r = navMeshBounds.GetComponent<Renderer>();
			if (r == null) Debug.Log("ERROR");
			else
			{
				bounds = r.bounds;
				GameObject.Destroy(navMeshBounds);//больше не нужен
			}
		}

		//устанвливаем maxDistance, ориентируясь на размер врага, она должна быть больше высоты NavMashAgent'а в 2 раза
		GameObject enemyPrefab = Resources.Load<GameObject>("Prefabs/Enemies/Enemy");
		if (enemyPrefab == null)
		{
			Debug.Log("WARNING");
			maxDistance = 2.0f * 2.0f;//2 - высота NavMashAgent по умолчанию
		}
		else
		{
			NavMeshAgent a = enemyPrefab.GetComponent<NavMeshAgent>();
			if (a == null)
			{
				Debug.Log("WARNING");
				maxDistance = 2.0f * 2.0f;//2 - высота NavMashAgent по умолчанию
			}
			else maxDistance = a.height * 2.0f;
		}
	}

	//каждый кадр пробуем сгенерировать по одной годной позиции и сохранить её для будущего использования,
	//но не более установленого лимита
	void Update ()
	{
		if (cachedPos.Count >= limit) return;
		Vector3 pos;
		if (TryGenPos(out pos)) cachedPos.Add(pos);
	}

	//попытаться получить позицию пригодную для спауна
	//eyePos - позиция трансформации камеры игрока
	//forward - направление взгляда камеры (вектор forward из трансформации камеры)
	public bool TryGetRandomPos (in Vector3 eyePos, in Vector3 forward, out Vector3 result)
	{
		//пробуем найти подходящую позицию в кэше
		for (int i = 0; i < cachedPos.Count; i++)
		{
			if (IsBehind(cachedPos[i], eyePos, forward))
			{
				result = cachedPos[i];
				cachedPos.RemoveAt(i);
				return true;
			}
		}
		//"форсированый" поиск подходящей позиции, потенциально нагружает систему
		int tryCount = this.tryCount;
		while (--tryCount >= 0)
		{
			if (TryGenPos(out result))
			{
				if (IsBehind(result, eyePos, forward)) return true;
				//позиция не подходит, т. к. видима, но сгодится на будущее
				else if (cachedPos.Count < limit) cachedPos.Add(result);
			}
		}
		result = Vector3.zero;
		return false;
	}

	//сделан на основе примера из официальной документации
	bool TryGenPos (out Vector3 pos)
	{
		NavMeshHit hit;
		if (NavMesh.SamplePosition(GenRandomPointInBounds(), out hit, maxDistance, mask))
		{
			pos = hit.position;
			return true;
		}
		else
		{
			pos = Vector3.zero;
			return false;
		}
	}

	Vector3 GenRandomPointInBounds ()
	{
    	float x = Random.Range(bounds.min.x, bounds.max.x);
		float y = Random.Range(bounds.min.y, bounds.max.y);
		float z = Random.Range(bounds.min.z, bounds.max.z);
		
		return new Vector3(x, y, z);
	}

	//позиция за спиной?
	bool IsBehind (in Vector3 queryPos, in Vector3 eyePos, in Vector3 forward)
	  => !new Plane(forward, eyePos).GetSide(queryPos);
}