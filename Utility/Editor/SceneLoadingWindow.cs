using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.Collections.Generic;
using System.Linq;

namespace Homida.Utility.Editor
{
	public class SceneLoadingWindow : EditorWindow
	{
		private const string SHARED_UI_PATH = "Assets/Gameplay/UI/SharedUI/SharedUI.unity";

		private bool _playMode;
		private bool _loadWithSharedUi;
		private EditorBuildSettingsScene[] _scenes;
		private List<string> _scenePaths;

		private void OnEnable()
		{
			_scenes = EditorBuildSettings.scenes;
			_scenePaths = _scenes.Select(scene => scene.path).ToList();
		}

		[MenuItem("Window/General/SceneLoader")]
		private static void ShowWindow()
		{
			var window = GetWindow<SceneLoadingWindow>();
			window.titleContent = new GUIContent("SceneLoadingWindow");
			window.Show();
		}

		private void OnGUI()
		{
			foreach (string path in _scenePaths)
			{
				// Get just the name of the scene
				int lastSlash = path.LastIndexOf("/") + 1;
				string name = path.Substring(lastSlash, path.LastIndexOf(".") - lastSlash);
				if (GUILayout.Button(name))
				{
					LoadScene(path);
				}
			}
			_loadWithSharedUi = GUILayout.Toggle(_loadWithSharedUi, "Load with Shared UI");
			_playMode = GUILayout.Toggle(_playMode, "Play Mode");
		}

		private void LoadScene(string path)
		{
			EditorSceneManager.OpenScene(path);

			if (_loadWithSharedUi)
			{
				EditorSceneManager.OpenScene(SHARED_UI_PATH, OpenSceneMode.Additive);
			}

			if (_playMode)
			{
				EditorApplication.ExecuteMenuItem("Edit/Play");
			}
		}
	}
}