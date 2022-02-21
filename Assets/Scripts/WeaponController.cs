using UnityEngine;

public class WeaponController : MonoBehaviour
{	
	Transform muzzle;//трансформ, откуда вылетает пуля, также её forward задаёт направление полёта пули
	AudioSource audioSource;
	AudioClip shotClip;
	string bulletColor = BulletSpawner.defaultColor;
	float shotDelay = 0.5f;//задержка между выстрелами (сек)
	float canShotTime = float.MinValue;//время в которое уже можно сделать выстрел
	
	public string BulletColor
	{
		get => bulletColor;
		set => this.bulletColor = BulletSpawner.IsColorValid(value) ? value : BulletSpawner.defaultColor;
	}

	bool CanShot => Time.time >= canShotTime;

	void Start ()
	{
		muzzle = transform;//пока сделал так, пусть пуля вылетает из центра оружия

		audioSource = GetComponent<AudioSource>();
		if (audioSource == null) Debug.Log("ERROR");

		shotClip = Resources.Load<AudioClip>("Sounds/pb_shoot");
		if (shotClip == null) Debug.Log("ERROR");
	}
	
	public bool TryShootOnce () => CanShot && ShootOnce();

	bool ShootOnce ()
	{
		canShotTime = Time.time + shotDelay;
		bool wasShot = BulletSpawner.Spawn(bulletColor, muzzle);
		if (wasShot && shotClip != null) audioSource?.PlayOneShot(shotClip);
		return wasShot;
	}
}
