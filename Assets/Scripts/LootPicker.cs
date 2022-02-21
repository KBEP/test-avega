using UnityEngine;
using System;

// Компонент ищет есть, что подобрать или нет, и вызывает два соответствующих события HasPickable и HasNoPickable.
// Должен вешаться на объект, на котором висит целевая камера.

public class LootPicker : MonoBehaviour
{
	static bool staticInitDone;//была ли уже сделана инициализация статических данных
	static float maxPickDistance = 1.5f;
	static float pickSphereRadius = 0.1f;//радиус сферы для Physics.SphereCast
	static int layerMask;
	RaycastHit hit;

	public Transform PickableLoot { get; private set; }

	public event Action<Transform> HasPickable;
	public event Action HasNoPickable;

	void Start ()
	{
		if (!staticInitDone)
		{
			int pickableIdx = LayerMask.NameToLayer("Pickable");
			if (pickableIdx == -1) Debug.Log("ERROR");
			else layerMask = 1 << pickableIdx;

			staticInitDone = true;
		}
	}
	
	void Update ()
	{
		//Raycast работает быстрее, но сложнее попадать взглядом на предметы
		//if (Physics.Raycast(transform.position, transform.forward, out hit,
		//  maxPickDistance, layerMask, QueryTriggerInteraction.Collide))
		if (Physics.SphereCast(transform.position, pickSphereRadius, transform.forward, out hit, maxPickDistance,
		  layerMask, QueryTriggerInteraction.Collide))
		{
			PickableLoot = hit.transform;
			if (HasPickable != null) HasPickable(PickableLoot);
		}
		else
		{
			PickableLoot = null;
			if (HasNoPickable != null) HasNoPickable();
		}
	}

	/* void OnDrawGizmos ()
	{
		Gizmos.color = Color.red;
		Ray r = new Ray(transform.position, transform.forward);
		Gizmos.DrawLine(r.origin, r.GetPoint(maxPickDistance));
	} */
}
