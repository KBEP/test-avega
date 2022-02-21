using UnityEngine;

public class FPSLimiter : MonoBehaviour
{
	public int fps = 60;
	
	void Awake ()
	{
		QualitySettings.vSyncCount = 0;
		Application.targetFrameRate = fps;
	}
}
