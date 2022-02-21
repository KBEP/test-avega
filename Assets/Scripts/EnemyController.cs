using System;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
	Health health;
	NavMeshAgent agent;
	
	static int damage = -5;
	static float attackDelay = 1.0f;//сек
	static float attackDistance = 2.0f;
	static float sqrAttackDistance = attackDistance * attackDistance;
	
	float timeToAttack = float.MinValue;

	public event Action Destroyed;
	
	void Start ()
	{
		health = GetComponent<Health>();
		if (health == null) Debug.Log("ERROR");
		else health.Dead += OnDead;

		agent = GetComponent<NavMeshAgent>();
		if (agent == null) Debug.Log("ERROR");
		else agent.stoppingDistance = attackDistance;
	}

	void Update ()
	{
		Health targetHealth = BotNet.Target?.Health;
		if (targetHealth == null || targetHealth.IsDead)//нет цели или цель мертва
		{
			if (agent != null) agent.isStopped = true;
		}
		else
		{
			agent?.SetDestination(targetHealth.transform.position);//преследуем цель
			if (CanAttack(targetHealth.transform.position))//атакуем если можем
			{
				targetHealth.Value += damage;
				timeToAttack = Time.time + attackDelay;
			}
		}
	}

	void OnDestroy ()
	{
		if (health != null) health.Dead -= OnDead;
		if (Destroyed != null) Destroyed();
	}

	void OnDead ()
	{
		LootSpawner.SpawnRandomLoot(transform.position);
		GameObject.Destroy(gameObject);
	}

	bool CanAttack (in Vector3 pos)//таймаут атаки кончился и бот достаточно близок к цели
	  => Time.time >= timeToAttack && AgentIsCloseEnough(pos);
	
	bool AgentIsCloseEnough (in Vector3 pos)//агент достаточно близок к позиции
	  => agent != null && (agent.transform.position - pos).sqrMagnitude <= sqrAttackDistance;
}
