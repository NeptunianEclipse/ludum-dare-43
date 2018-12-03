public interface ITurnable
{
	bool IsTurning { get; }

	void StartTurning();

	void StartTurning(float degrees);
}

