using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityUI : MonoBehaviour
{
	public AbilitySlot AbilitySlot;
	public Text KeyText;
	public Text NameText;
	public Image IconImage;

	public void RefreshUI()
	{
		KeyText.text = AbilitySlot.ActivateKey.ToString();
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

}
