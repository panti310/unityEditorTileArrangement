using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR

public class SampleWindow : EditorWindow
{
	const string Default_Icon_Path = "Icons/None.png";
	const string Sample_Icon_Root_Path = "Icons/Sample/";
	const string Sample_Icon_Extension = ".png";
	const float Button_Margin = 6f;
	const float Button_Min_Size = 32f;
	const float Button_Max_Size = 128f;
	const int Button_Num = 20;

	GUIContent m_btnContent = new GUIContent ();
	Vector2 m_scrollPos = Vector2.zero;
	float m_buttonSize = 64f;
	Texture m_default_tex;
	Dictionary<string, Texture> m_texCacheDict = new Dictionary<string, Texture> ();

	[MenuItem ("Window/SampleWindow")]
	static void Open ()
	{
		GetWindow<SampleWindow> ("SampleWindow");
	}

	void OnEnable ()
	{
		Init ();
	}

	void Init ()
	{
		Preload ();
		m_btnContent.text = "";
		m_btnContent.tooltip = "";
	}

	/// <summary>
	/// Preload icon
	/// </summary>
	void Preload ()
	{
		m_default_tex = EditorGUIUtility.Load (Default_Icon_Path) as Texture2D;
		SetTexCacheDict (Sample_Icon_Root_Path, Sample_Icon_Extension, Button_Num);
	}

	/// <summary>
	/// Set a texture dictionary
	/// </summary>
	/// <param name="rootPath">Root path.</param>
	/// <param name="extension">Extension.</param>
	/// <param name="num">Number.</param>
	void SetTexCacheDict (string rootPath, string extension, int num)
	{
		for (int i = 0, c = num; i < c; i++) {
			string path = rootPath + (i + 1) + extension;
			if (m_texCacheDict.ContainsKey (path)) {
				continue;
			}
			Texture2D texture = EditorGUIUtility.Load (path) as Texture2D;
			if (texture == null) {
				continue;
			}
			m_texCacheDict [path] = EditorGUIUtility.Load (path) as Texture2D;
		}
	}

	void OnGUI ()
	{
		DrawButtonSizeSlider ();

		m_scrollPos = EditorGUILayout.BeginScrollView (m_scrollPos);
		{
			DrawButtonGroup ();
		}
		EditorGUILayout.EndScrollView ();
	}

	/// <summary>
	/// Draw a button size slider
	/// </summary>
	void DrawButtonSizeSlider ()
	{
		EditorGUILayout.BeginVertical (GUI.skin.box);
		{
			EditorGUILayout.PrefixLabel ("Button size");
			m_buttonSize = EditorGUILayout.Slider (m_buttonSize, Button_Min_Size, Button_Max_Size);
		}
		EditorGUILayout.EndVertical ();
	}

	/// <summary>
	/// Draw a button group
	/// </summary>
	void DrawButtonGroup ()
	{
		EditorGUILayout.BeginVertical (GUI.skin.box);
		{
			EditorGUILayout.PrefixLabel ("Button group title");
			DrawThumbnailButtons (Button_Num, OnClickButton, Sample_Icon_Root_Path, Sample_Icon_Extension);
		}
		EditorGUILayout.EndVertical ();
	}

	/// <summary>
	/// Ons the click button.
	/// </summary>
	/// <param name="num">Number.</param>
	void OnClickButton (int num)
	{
		Debug.Log ("Tap Button = " + num);
	}

	/// <summary>
	/// Draw thumbnail buttons
	/// </summary>
	/// <param name="itemNum">Item number.</param>
	/// <param name="onClick">On click.</param>
	void DrawThumbnailButtons (int itemNum, System.Action<int> onClick, string rootPath, string extension)
	{
		float windowWidth = position.width;
		int horizontalMaxNumber = Mathf.FloorToInt (windowWidth / (m_buttonSize + Button_Margin));
		bool inHorizontalLayout = false;

		for (int i = 0; i < itemNum; i++) {
			if (i % horizontalMaxNumber == 0) {
				EditorGUILayout.BeginHorizontal ();
				inHorizontalLayout = true;
			}

			// Set from preloaded icon.
			string path = rootPath + (i + 1) + extension;
			if (m_texCacheDict.ContainsKey (path)) {
				m_btnContent.image = m_texCacheDict [path];
			} else {
				m_btnContent.image = m_default_tex;
			}
			if (GUILayout.Button (m_btnContent, GUILayout.Width (m_buttonSize), GUILayout.Height (m_buttonSize))) {
				if (onClick != null) {
					onClick (i);
				}
			}

			if (i % horizontalMaxNumber == horizontalMaxNumber - 1) {
				EditorGUILayout.EndHorizontal ();
				inHorizontalLayout = false;
			}
		}

		if (inHorizontalLayout) {
			EditorGUILayout.EndHorizontal ();
		}
	}
}
#endif