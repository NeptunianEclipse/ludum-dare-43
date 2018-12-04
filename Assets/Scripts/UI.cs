using LudumDare43.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : Singleton<UI> {

	public Text SacrificeMessage;
	public Text GiftMessage;
	public Text WinMesssage;
	public GameObject StoryDialogue;
	public Text HealthText;
	public Text Gameover;

	public void SetText(object sender, System.EventArgs args)
	{
		var comp = Player.Instance.GetComponent<Damageable>();
		HealthText.text = $"Health: {comp.CurrentHealth} / {comp.MaxHealth}";
	}

	public void DismissStoryClicked()
	{
		StoryDialogue.SetActive(false);
	}


}
