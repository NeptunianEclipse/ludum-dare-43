using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityUI : MonoBehaviour
{
	public AbilitySlot AbilitySlot;
	public Text KeyText;
	public Text NameText;
	public RectTransform IconMask;
	public Image IconImage;

	public void RefreshUI()
	{
		if(AbilitySlot is ActivableAbilitySlot)
		{
			KeyText.text = ((ActivableAbilitySlot)AbilitySlot).ActivateKey.ToString();
		} else
		{
			KeyText.text = "";
		}
		
		if(AbilitySlot.Ability != null)
		{
			NameText.text = AbilitySlot.Ability.Name;
			IconImage.sprite = AbilitySlot.Ability.Sprite;
		}
		else
		{
			NameText.text = "-";
			IconImage.sprite = null;
		}
	}

	private void Update()
	{
		if(AbilitySlot.Ability is IRecoverable)
		{
			IRecoverable recoverable = (IRecoverable)AbilitySlot.Ability;
			float y = (1 - recoverable.RecoveryPercent) * IconImage.rectTransform.sizeDelta.y;
			IconMask.sizeDelta = new Vector2(IconMask.sizeDelta.x, y);
		}
	}

}
