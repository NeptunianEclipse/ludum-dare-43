using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AbilityPedestalState {
	Inactive,
	Animating,
	WaitingForSacrifice,
	WaitingForChoice
}

[RequireComponent(typeof(Collider2D))]
public class AbilityPedestal : MonoBehaviour 
{
	
	

	public PedestalStateInfo Inactive;
	public PedestalStateInfo Dispensing;
	public PedestalStateInfo WaitingForSacrificeUnselected;
	public PedestalStateInfo WaitingForSacrificeSelected;
	public PedestalStateInfo Sacrificing;
	public PedestalStateInfo Bestowing;
	public PedestalStateInfo WaitingForChoiceUnselected;
	public PedestalStateInfo WaitingForChoiceSelected;

	[HideInInspector]
	public PlayerAbilityController PlayerAbilityController;
	[HideInInspector]
	public SacrificialChamber SacrificialChamber;

	public AbilitySlot SacrificeSlot { get; private set; }
	public AbilityBase GiftAbility { get; private set; }
	public AbilityPedestalState State;
	public SacrificialChamberState ChamberState => SacrificialChamber.State;

	






	public KeyCode SelectKey;

	public SpriteRenderer IconRenderer;
	public Transform Floater;
	public SpriteRenderer Rays;

	public LineRenderer SacrificeLine;
	public Gradient SacrificeLineGradient;
	public float LineGradientFrequency;
	public float RayAlphaRate;

	public Transform InactivePosition;
	public Transform SelectedPosition;
	public Transform OffscreenPosition;

	public float SelectRate;
	public float OtherFlyingRate;
	public float AnimationFinishDistance;

	private float baseRaysAlpha;
	private bool selected;

	private Transform floaterTarget;




	private void Awake()
	{
		baseRaysAlpha = Rays.color.a;
		floaterTarget = InactivePosition;
	}

	private void Update()
	{
		if (State == AbilityPedestalState.WaitingForChoice || State == AbilityPedestalState.WaitingForSacrifice)
		{
			Floater.position = Vector3.Lerp(Floater.position, floaterTarget.position, SelectRate * Time.deltaTime);
		}

		if(selected && Input.GetKeyDown(SelectKey))
		{
			if(State == AbilityPedestalState.WaitingForSacrifice)
			{
				StartCoroutine(SacrificialChamber.SacrificeSelected(SacrificeSlot));
			}
			else if(State == AbilityPedestalState.WaitingForChoice)
			{
				StartCoroutine(SacrificialChamber.GiftSelected(GiftAbility));
			}
		}

		if (ChamberState == SacrificialChamberState.Inactive)
		{
			SacrificeLine.enabled = false;
		}
		else
		{
			SacrificeLine.enabled = true;
			for (int i = 0; i < SacrificeLine.positionCount; i++)
			{
				float t = i / ((float)SacrificeLine.positionCount - 1);
				SacrificeLine.SetPosition(i, Vector3.Lerp(Floater.position, PlayerAbilityController.transform.position, t));
			}
		}

	}


	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag(Tags.Player))
		{
			floaterTarget = SelectedPosition;
			selected = true;
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.CompareTag(Tags.Player))
		{
			floaterTarget = InactivePosition;
			selected = false;
		}
		
	}

	

	public void SetAbilitySlot(AbilitySlot slot)
	{
		SacrificeSlot = slot;
		IconRenderer.sprite = SacrificeSlot.Ability.Sprite;
	}

	public void UnsetAbilitySlot()
	{
		SacrificeSlot = null;
		IconRenderer.sprite = null;
	}

	public void SetGiftAbility(AbilityBase ability)
	{
		GiftAbility = ability;
		IconRenderer.sprite = ability.Sprite;
	}



	public void PlayerEnteredChamber()
	{
		StartCoroutine(LeavePlayer());
	}

	public void PlayerExitedChamber()
	{

	}

	public IEnumerator BestowGifts()
	{
		yield return DescendFromAbove();
	}
	
	public IEnumerator PlayerSelectedSacrifice(AbilitySlot slot)
	{
		selected = false;
		if(slot == SacrificeSlot)
		{
			yield return Sacrifice();
		}
		else
		{
			yield return ReturnToPlayer();
		}
	}

	public IEnumerator PlayerSelectedGift(AbilityBase ability)
	{
		yield return null;
	}

	private IEnumerator LeavePlayer()
	{
		State = AbilityPedestalState.Animating;
		Floater.position = PlayerAbilityController.transform.position;
		yield return FlyToPedestal();
		State = AbilityPedestalState.WaitingForSacrifice;
	}

	private IEnumerator FlyToPedestal()
	{
		yield return FlyTo(InactivePosition.position, OtherFlyingRate);
	}

	private IEnumerator Sacrifice()
	{
		State = AbilityPedestalState.Animating;
		yield return FlyTo(OffscreenPosition.position, OtherFlyingRate);
		State = AbilityPedestalState.Inactive;
		UnsetAbilitySlot();
	}

	private IEnumerator ReturnToPlayer()
	{
		State = AbilityPedestalState.Animating;
		yield return FlyToPlayer();
		State = AbilityPedestalState.Inactive;
		UnsetAbilitySlot();
	}

	private IEnumerator DescendFromAbove()
	{
		State = AbilityPedestalState.Animating;
		yield return FlyToPedestal();
		State = AbilityPedestalState.WaitingForChoice;
	}

	private IEnumerator FlyToPlayer()
	{
		yield return FlyTo(PlayerAbilityController.transform, OtherFlyingRate);
	}

	private IEnumerator FlyTo(Vector3 position, float rate)
	{
		while (Vector3.Distance(position, Floater.position) >= AnimationFinishDistance)
		{
			Floater.position = Vector3.Lerp(Floater.position, position, rate * Time.deltaTime);
			yield return null;
		}
	}

	private IEnumerator FlyTo(Transform targetTransform, float rate)
	{
		while (Vector3.Distance(Floater.position, targetTransform.position) > AnimationFinishDistance)
		{
			Floater.position = Vector3.Lerp(Floater.position, targetTransform.position, rate * Time.deltaTime);
			yield return null;
		}
	}

	#region Visuals

	private Vector2 SpinnyOffset(Vector2 amplitude, float frequency, float t)
	{
		return new Vector2(Mathf.Sin(t * frequency) * amplitude.x, Mathf.Cos(t * frequency) * amplitude.y);
	}

	private Gradient PulsingGradient(int numKeys, float frequency, Gradient sampleGradient, float alpha, float t)
	{
		var gradient = new Gradient();
		var colorKeys = new GradientColorKey[numKeys];
		var alphaKeys = new GradientAlphaKey[numKeys];

		for (int i = 0; i < numKeys; i++)
		{
			float proportion = i / (float)numKeys;
			Color color = sampleGradient.Evaluate((t + proportion * frequency) % 1);
			colorKeys[i] = new GradientColorKey(color, proportion);
			alphaKeys[i] = new GradientAlphaKey(alpha, proportion);
		}

		gradient.SetKeys(colorKeys, alphaKeys);
		return gradient;
	}

	#endregion


}

[System.Serializable]
public class PedestalStateInfo
{
	public Transform IconTargetPosition;
	public float IconMoveRate;
	public float IconMaxSpeed;
	public Color TargetRaysColor;
	public float LineAlpha;
	public bool IconVisible;
	public Vector2 SpinnyAmplitude;
	public float SpinnyFrequency;
}