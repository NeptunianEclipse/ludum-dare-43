using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityUI : MonoBehaviour
{
	public AbilitySlot AbilitySlot;
	public Text KeyText;
	public CanvasGroup KeyGroup;
	public RectTransform IconMask;
	public Image IconImage;
	public Sprite NoIconImage;

	public void RefreshUI()
	{
		if(AbilitySlot is ActivableAbilitySlot)
		{
			KeyText.text = ((ActivableAbilitySlot)AbilitySlot).ActivateKey.ToString();
			KeyGroup.alpha = 1;
		} else
		{
			KeyText.text = "";
			KeyGroup.alpha = 0;
		}
		
		if(AbilitySlot.Ability != null)
		{
			IconImage.sprite = AbilitySlot.Ability.Sprite;
		}
		else
		{
			IconImage.sprite = NoIconImage;
		}
	}

	private void Update()
	{
		if (AbilitySlot.Ability != null && AbilitySlot.Ability is IRecoverable)
		{
			IRecoverable recoverable = (IRecoverable)AbilitySlot.Ability;
			float y = (1 - recoverable.RecoveryPercent) * IconImage.rectTransform.sizeDelta.y;
			IconMask.sizeDelta = new Vector2(IconMask.sizeDelta.x, y);
		}
		else
		{
			IconMask.sizeDelta = Vector2.zero;
		}
	}
}
