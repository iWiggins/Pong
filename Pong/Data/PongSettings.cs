namespace Pong.Data;
/// <summary>
/// The game settings.
/// </summary>
internal class PongSettings
{
	/// <summary>
	/// Volume level (1-10)
	/// </summary>
	public int Volume { get; set; }
	/// <summary>
	/// What is controlling the left paddle.
	/// </summary>
	public Controller LeftPaddle { get; set; }
	/// <summary>
	/// What is controlling the right paddle.
	/// </summary>
	public Controller RightPaddle { get; set; }
}
