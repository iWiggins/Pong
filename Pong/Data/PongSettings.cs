using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
namespace Pong.Data;
/// <summary>
/// The game settings.
/// </summary>
/// <remarks>
/// This class is designed to read from and write to a JSON file.
/// That's why all of its values are public properties.
/// </remarks>
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

	/// <summary>
	/// Load the settings from a JSON file.
	/// If the file is not found, load default settings.
	/// </summary>
	/// <returns>A populated PongSettings object.</returns>
	public static PongSettings Load()
	{
		// If the settings file exists, read it.
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

	/// <summary>
	/// Save the settings to a JSON file.
	/// </summary>
	public void Save()
	{
		using FileStream stream = new(SettingsFile, FileMode.OpenOrCreate);
		JsonSerializer.Serialize(stream, this, typeof(PongSettings), SettingsGenerationContext.Default);
	}
}

/// <summary>
/// This context is needed because the game compiles to native code.
/// The native compilation disables reflection that would have been used
/// to automatically analyze this class at runtime, preventing serialization.
/// Adding this triggers that analysis at compile time instead.
/// </summary>
[JsonSourceGenerationOptions(WriteIndented = true)]
[JsonSerializable(typeof(PongSettings))]
internal partial class SettingsGenerationContext : JsonSerializerContext { }