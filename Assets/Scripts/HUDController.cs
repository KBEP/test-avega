using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Контролирует показания здоровья игрока и количества собраных кубов.

public class HUDController : MonoBehaviour
{
	Text hpText;
	Health health;
	Dictionary<string, Text> scores;
	ButtonActivityController pickUpBtn;
	
	void Start ()
	{
		health = transform.parent.GetChildComponent<Health>("Player");
		if (health == null) Debug.Log("ERROR");
		else
		{
			health.Dead += OnHPChanged;
			health.Damaged += OnHPChanged;
		}

		hpText = transform.GetChildComponent<Text>("HP");
		if (hpText == null) Debug.Log("ERROR");
		
		scores = new Dictionary<string, Text>();
		foreach (var c in LootSpawner.ColorNames)
		{
			Text t = transform.GetChildComponent<Text>("Score/" + c + "/Text");
			if (t != null) scores.Add(c, t);
			else Debug.Log("ERROR");
		}

		pickUpBtn = transform.parent?.GetChildComponent<ButtonActivityController>("Canvas/PickUpButton");
		if (pickUpBtn == null) Debug.Log("ERROR");
	}

	void OnHPChanged ()
	{
		if (hpText != null && health != null) hpText.text = health.Value.ToString();
	}

	public void IncreaseScore (string lootName)
	{
		if (scores == null || lootName == null) return;
		Text t;
		int current;
		if (scores.TryGetValue(lootName, out t) && int.TryParse(t.text, out current)) t.text = (++current).ToString();
	}

	public void SetPickUpButtonActivity (bool isActive)
	{
		pickUpBtn?.SetButtonActivity(isActive);
	}
}
