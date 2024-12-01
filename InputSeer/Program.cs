using Raylib_cs.BleedingEdge;
using Raylib_cs.BleedingEdge.Interop;

namespace InputSeer;

class Program
{
	unsafe static void Main(string[] args)
	{
		int GamepadCount = 4;
		float time = 0;
		Raylib.SetConfigFlags(ConfigFlags.VSyncHint | ConfigFlags.Msaa4XHint);
		Raylib.InitWindow(1280, 720, "Input Seer");
		Raylib.SetTargetFPS(60);
		List<KeyboardKey> keyboardKeys = new List<KeyboardKey>();
		List<GamepadButton> AllGamepadButtons = new List<GamepadButton>();
		List<GamepadButton>[] GamepadButtonsPerPad = new List<GamepadButton>[GamepadCount];
		float[] GamePadTimer = new float[GamepadCount];
		for (int i = 0; i < GamepadCount; i++)
		{
			GamepadButtonsPerPad[i] = new List<GamepadButton>();
		}
		string str = "";
		while (true)
		{
			Raylib.BeginDrawing();
			Raylib.ClearBackground(Color.Black);
			int DY = 20;
			int PH = 5;
			{

				int HOffset = 50;
				int X = 200;
				while (true)
				{
					var key = Raylib.GetKeyPressed();
					if (key == KeyboardKey.Null) break;
					keyboardKeys.Add(key);
				}
				for (int keyI = keyboardKeys.Count - 1; keyI >= 0; keyI--)
				{
					KeyboardKey key = keyboardKeys[keyI];
					if (Raylib.IsKeyDown(key))
					{
						Raylib.DrawText($"{key.ToString()}({(int)key})", X, HOffset, DY, Color.White);
						HOffset += DY + PH;
					}
					else
					{
						keyboardKeys.RemoveAt(keyI);
					}
				}
				if (keyboardKeys.Count == 0)
				{
					Raylib.DrawText("No key is pressed.", X, HOffset, DY, Color.White);
					HOffset += DY + PH;
					time = 0;
				}
				else
				{
					time += Raylib.GetFrameTime();
					if (time > 5f)
					{
						break;
					}
				}
				{
					Raylib.DrawText($"Mouse:{Raylib.GetMouseX()},{Raylib.GetMouseY()}", X, HOffset, DY, Color.White);
					HOffset += DY + PH;
				}
			}
			{
				int HOffset = 10;
				int X = 300;
				while (true)
				{

					var _char = Raylib.GetCharPressed();
					if (_char == 0)
					{
						break;
					}
					str+=(char)_char;
				}
				if (Raylib.IsKeyPressed(KeyboardKey.Escape))
				{
					str="";
				}
				{
					Raylib.DrawText(str, X, HOffset, DY, Color.White);
					HOffset += DY + PH;
				}
			}
			{
				int HOffset = 50;
				int X = 500;
				var Lkey = Raylib.GetGamepadButtonPressed();
				for (int gamepadID = 0; gamepadID < GamepadCount; gamepadID++)
				{
					if ((bool)Raylib.IsGamepadAvailable(gamepadID))
					{
						Raylib.DrawText($"Gamepad {gamepadID} conneted:", X, HOffset, DY, Color.Green);
						HOffset += DY + PH;
						Raylib.DrawText(Raylib.GetGamepadName(gamepadID), X, HOffset, DY, Color.Green);
						HOffset += DY + PH;
						var axis = Raylib.GetGamepadAxisCount(gamepadID);
						for (int axisID = 0; axisID < axis; axisID++)
						{
							var mv = Raylib.GetGamepadAxisMovement(gamepadID, (GamepadAxis)axisID);
							Raylib.DrawText($"Axis: {(GamepadAxis)axisID} >> {mv}", X, HOffset, DY, Color.White);
							HOffset += DY + PH;
						}
						if (Raylib.IsGamepadButtonPressed(gamepadID, Lkey))
						{
							GamepadButtonsPerPad[gamepadID].Add(Lkey);
						}
						for (int i = GamepadButtonsPerPad[gamepadID].Count - 1; i >= 0; i--)
						{
							GamepadButton key = GamepadButtonsPerPad[gamepadID][i];
							if (Raylib.IsGamepadButtonDown(gamepadID, key))
							{
								Raylib.DrawText($"{key}({(int)key})", X, HOffset, DY, Color.White);
								HOffset += DY + PH;
							}
							else
							{
								GamepadButtonsPerPad[gamepadID].RemoveAt(i);
							}
						}
						if (GamepadButtonsPerPad[gamepadID].Count == 0)
						{
							Raylib.DrawText("No button is pressed.", X, HOffset, DY, Color.White);
							HOffset += DY + PH;
							GamePadTimer[gamepadID] = 0;
						}
						else
						{
							GamePadTimer[gamepadID] += Raylib.GetFrameTime();
							if (GamePadTimer[gamepadID] > 5f)
							{
								goto CLEAN_UP;
							}
						}
					}
					else
					{
						Raylib.DrawText($"Gamepad {gamepadID} not conneted.", X, HOffset, DY, Color.White);
						HOffset += DY + PH;
					}
				}
			}
			Raylib.DrawFPS(10, 10);
			Raylib.EndDrawing();
		}
	CLEAN_UP:
		Raylib.CloseWindow();
	}
}
