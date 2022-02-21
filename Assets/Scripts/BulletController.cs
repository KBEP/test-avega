using UnityEngine;

public class BulletController : MonoBehaviour
{
	static bool staticInitDone;//была ли уже сделана инициализация статических данных
	static int layerMask;//маска коллайдеров, с которыми пуля взаимодействует
	static float speed = 100.0f;
	static int damage = -50;
	static float lifeTime = 3.0f;//сколько будет существовать пуля пока не будет уничтожена
	
	float timeToDestroy;//время, когда пуля будет уничтожена

	void Start ()
	{
		if (!staticInitDone)
		{
			int solidIdx = LayerMask.NameToLayer("Solid");
			if (solidIdx == -1) Debug.Log("ERROR");
			else layerMask = 1 << solidIdx;

			staticInitDone = true;
		}
		
		timeToDestroy = Time.time + lifeTime;
	}

	void Update ()
	{
		if (Time.time >= timeToDestroy) GameObject.Destroy(gameObject);
		else//летим вперёд
		{
			Vector3 prevPos = transform.position;
			transform.Translate(Vector3.forward * Time.deltaTime * speed);
			//проверка на столкновение
			RaycastHit hit;
			if (Physics.Linecast(prevPos, transform.position, out hit, layerMask, QueryTriggerInteraction.Collide))
			{
				//наносим урон, если есть чему
				Health health = hit.transform.GetComponent<Health>();
				if (health != null) health.Value += damage;
				GameObject.Destroy(gameObject);
			}
		}
	}

}
