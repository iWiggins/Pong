using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
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

	public static PongSettings Default => new()
	{
		Volume = 5,
		LeftPaddle = Controller.WASD,
		RightPaddle = Controller.CPU
	};

	public static string SettingsFile => "pong.json";

	public static PongSettings Load()
	{
		if(File.Exists(SettingsFile))
		{
			using FileStream stream = new(SettingsFile, FileMode.Open);
			PongSettings? settings =
				JsonSerializer.Deserialize(
					stream,
					SettingsGenerationContext.Default.PongSettings);

			if(settings is not null) return settings;

			// if is null, continue to generate defaults and overwrite the existing file.
		}

		PongSettings defaultSettings = Default;

		defaultSettings.Save();

		return defaultSettings;
	}

	public void Save()
	{
		using FileStream stream = new(SettingsFile, FileMode.OpenOrCreate);
		JsonSerializer.Serialize(stream, this, typeof(PongSettings), SettingsGenerationContext.Default);
	}
}

[JsonSourceGenerationOptions(WriteIndented = true)]
[JsonSerializable(typeof(PongSettings))]
internal partial class SettingsGenerationContext : JsonSerializerContext { }