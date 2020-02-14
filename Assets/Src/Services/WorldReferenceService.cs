using UnityEngine;
using UnityEngine.InputSystem.UI;

namespace Services
{
	/// <summary>
	/// TODO:
	/// </summary>
	public interface IWorldReferenceService
	{
		/// <summary>
		/// The game's current main active camera
		/// </summary>
		Camera MainCamera { get; }

		/// <summary>
		/// Disables all the game's input. The player will not be able to touch or interact with anything in the game
		/// </summary>
		void DisableInput();

		/// <summary>
		/// Enables all the game's input. The player will not be able to touch or interact with anything in the game
		/// </summary>
		void EnableInput();
	}
	
	/// <inheritdoc />
	public class WorldReferenceService : IWorldReferenceService
	{
		private InputSystemUIInputModule _inputSystem;
		
		/// <inheritdoc />
		public Camera MainCamera { get; }

		private WorldReferenceService() {}

		public WorldReferenceService(Camera mainCamera, InputSystemUIInputModule inputSystem)
		{
			_inputSystem = inputSystem;
			
			MainCamera = mainCamera;
		}

		/// <inheritdoc />
		public void DisableInput()
		{
			_inputSystem.DisableAllActions();
		}

		/// <inheritdoc />
		public void EnableInput()
		{
			_inputSystem.EnableAllActions();
		}
	}
}