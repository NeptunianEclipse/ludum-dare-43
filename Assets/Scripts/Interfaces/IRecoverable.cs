using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// An ability that has a cooldown
public interface IRecoverable
{
	float RecoveryPercent { get; }
}
