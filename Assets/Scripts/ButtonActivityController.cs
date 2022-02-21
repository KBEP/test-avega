using UnityEngine;
using UnityEngine.UI;

// В MFPC нет возможности управлять активностью кнопки (FP_Button.Interactable не работает).
// Этот компонент позволяет это сделать, не влезая в код FP_Button. Компонент вещается туда же, где висит FP_Button.

public class ButtonActivityController : MonoBehaviour
{
	//элементы кнопки которые нужно включать/выключать, чтобы активировать/деактивировать кнопку
	CanvasGroup canvasGroup;
	Image btnImage;
	Image iconImage;

	void Awake ()
	{
		canvasGroup = GetComponent<CanvasGroup>();
		if (canvasGroup == null) Debug.Log("ERROR");

		btnImage = GetComponent<Image>();
		if (btnImage == null) Debug.Log("ERROR");

		iconImage = transform.GetChildComponent<Image>("Icon");
		if (iconImage == null) Debug.Log("ERROR");
	}

	public void SetButtonActivity (bool isActive)
	{
		if (canvasGroup != null) canvasGroup.blocksRaycasts = isActive;
		if (btnImage != null) btnImage.enabled = isActive;
		if (iconImage != null) iconImage.enabled = isActive;
	}
}
