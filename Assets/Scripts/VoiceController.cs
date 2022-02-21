using System.Collections.Generic;
using UnityEngine;

public class VoiceController : MonoBehaviour
{
	static List<AudioClip> hurts;//клипы для проигрывания при получении урона
	
	Health health;
	AudioSource audioSource;

	void Start ()
	{
		if (hurts == null)//ещё не инициализирован
		{
			hurts = new List<AudioClip>();
			for (int i = 1; i <= 7; ++i)
			{
				AudioClip clip = Resources.Load<AudioClip>("Sounds/hurt" + i);
				if (clip != null) hurts.Add(clip);
				else Debug.Log("ERROR");
			}
		}
		
		health = transform.GetChildComponent<Health>("Player");
		if (health == null) Debug.Log("ERROR");
		else health.Damaged += OnDamaged;//подписываемся, чтобы реагировать на потерю здоровья

		audioSource = transform.GetChildComponent<AudioSource>("Player");
		if (audioSource == null) Debug.Log("ERROR");
	}

	void OnDamaged ()//проигрываем случайный звук БОЛИ
	{
		if (hurts != null && hurts.Count > 0) audioSource?.PlayOneShot(hurts[Random.Range(0, hurts.Count)]);
	}
}
