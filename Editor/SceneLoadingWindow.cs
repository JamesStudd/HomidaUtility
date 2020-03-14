using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.Collections.Generic;
using System.Linq;

namespace HomidaUtility.Utility.Editor
{
	public class SceneLoadingWindow : EditorWindow
	{
		private GUIStyle _titleStyle;

		private EditorBuildSettingsScene[] _scenes;
		private List<string> _scenePaths;
		private List<bool> _scenesSelected;

		private bool _playMode;
		private bool _loadWithSharedUi;
		private bool _loadMultiple;

		private void OnEnable()
		{
			_titleStyle = new GUIStyle
			{
				fontStyle = FontStyle.Bold
			};

			_scenes = EditorBuildSettings.scenes;
			_scenePaths = _scenes.Select(scene => scene.path).ToList();
			_scenesSelected = new List<bool>();
			for (int i = 0; i < _scenePaths.Count; i++)
			{
				_scenesSelected.Add(false);
			}
		}

		[MenuItem("HomidaUtility/SceneLoader")]
		private static void ShowWindow()
		{
			var window = GetWindow<SceneLoadingWindow>();
			window.titleContent = new GUIContent("SceneLoadingWindow");
			window.Show();
		}

		private void OnGUI()
		{
			GUILayout.Label("Setup", _titleStyle);
			_loadMultiple = GUILayout.Toggle(_loadMultiple, "Load Multiple Scenes?");
			_playMode = GUILayout.Toggle(_playMode, "Play Scenes When Loaded?");
			EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

			GUILayout.Label("Select Scenes to Load", _titleStyle);
			for (int i = 0; i < _scenePaths.Count; i++)
			{
				string path = _scenePaths[i];
				int lastSlash = path.LastIndexOf("/") + 1;
				string sceneName = path.Substring(lastSlash, path.LastIndexOf(".") - lastSlash);

				_scenesSelected[i] = GUILayout.Toggle(_scenesSelected[i], sceneName);
				if (_scenesSelected[i] && !_loadMultiple)
				{
					for (int j = 0; j < _scenePaths.Count; j++)
					{
						if (i == j) continue;
						_scenesSelected[j] = false;
					}
				}
			}
			if (GUILayout.Button("Load"))
			{
				LoadScenes();
			}
		}

		private void LoadScenes()
		{
			bool loadedFirst = false;
			for (int i = 0; i < _scenePaths.Count; i++)
			{
				if (_scenesSelected[i])
				{
					EditorSceneManager.OpenScene(_scenePaths[i], loadedFirst ? OpenSceneMode.Additive : OpenSceneMode.Single);
					loadedFirst = true;
				}
			}
			if (_playMode)
			{
				EditorApplication.ExecuteMenuItem("Edit/Play");
			}
		}
	}
}