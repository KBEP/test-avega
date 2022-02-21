using UnityEngine;

public class PlayerController : MonoBehaviour
{
	FP_Input fpInput;
	WeaponController wpnCtr;
	HUDController hudCtr;
	LootPicker lootPicker;
	AudioSource audioSource;
	AudioClip pickUpClip;

	public Health Health { get; private set; }
	public Transform CameraT { get; private set; }//трансформ камеры игрока, чтобы знать, куда он смотрит
	
	void Start ()
	{
		fpInput = transform.GetChildComponent<FP_Input>("Player");
		if (fpInput == null) Debug.Log("ERROR");
		
		wpnCtr = transform.GetChildComponent<WeaponController>("Player/PlayerHead/ItemsCamera/Weapon");
		if (wpnCtr == null) Debug.Log("ERROR");

		hudCtr = transform.GetChildComponent<HUDController>("HUD");
		if (hudCtr == null) Debug.Log("ERROR");

		lootPicker = transform.GetChildComponent<LootPicker>("Player/PlayerHead/Main Camera");
		if (lootPicker == null) Debug.Log("ERROR");
		else
		{
			lootPicker.HasPickable += OnHasPickable;
			lootPicker.HasNoPickable += OnHasNoPickable;
		}

		audioSource = transform.GetChildComponent<AudioSource>("Player");
		if (audioSource == null) Debug.Log("ERROR");
		
		pickUpClip = Resources.Load<AudioClip>("Sounds/inv_slot");
		if (pickUpClip == null) Debug.Log("ERROR");

		Health = transform.GetChildComponent<Health>("Player");
		if (Health == null) Debug.Log("ERROR");
		else Health.Dead += OnDead;

		CameraT = transform.GetChildComponent<Transform>("Player/PlayerHead/Main Camera");
		if (CameraT == null) Debug.Log("ERROR");

		BotNet.Target = this;//чтобы боты знали, кого атаковать
	}

	void Update ()
	{
		if (fpInput != null)
		{
			if (fpInput.Shoot()) TryShoot();
			if (fpInput.PickUp()) TryPickUpLoot();
		}
	}

	void TryShoot ()
	{
		wpnCtr?.TryShootOnce();
	}

	void TryPickUpLoot ()
	{
		if (lootPicker != null && lootPicker.PickableLoot != null)
		{
			hudCtr.IncreaseScore(lootPicker.PickableLoot.name);
			if (wpnCtr != null) wpnCtr.BulletColor = lootPicker.PickableLoot.name;
			if (pickUpClip != null) audioSource?.PlayOneShot(pickUpClip);
			GameObject.Destroy(lootPicker.PickableLoot.gameObject);
		}
	}

	void OnDead ()
	{
		Loader.Instance?.CmdReloadScene();
	}

	void OnHasPickable (Transform pickable)
	{
		hudCtr?.SetPickUpButtonActivity(true);
	}

	void OnHasNoPickable ()
	{
		hudCtr?.SetPickUpButtonActivity(false);
	}
}
