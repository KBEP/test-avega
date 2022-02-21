using UnityEngine;

public class DestructionPreventer : MonoBehaviour
{
	void Awake () => DontDestroyOnLoad(gameObject);
}
