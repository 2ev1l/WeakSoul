using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Universal;

namespace WeakSoul
{
	public class HelpTextUpdater : HelpUpdater
	{
		#region fields & properties
		public static HelpTextUpdater Instance { get; private set; }

		[SerializeField] private Text bodyText;
		[SerializeField] private LanguageLoader languageLoader;
		private Vector3 defaultMirror;
		#endregion fields & properties

		#region methods
		private void Start()
		{
			languageLoader.Load();
		}
		public override void Init()
		{
			base.Init();
			Instance = this;
			defaultMirror = startMirror;
		}
		public void OpenPanel(Vector3 position, int languageId, TextType textType, string additionalText, bool reverseX)
		{
			startMirror = reverseX ? defaultMirror + Vector3.right * (-2 * defaultMirror.x) : defaultMirror;
			languageLoader.ChangeValues(languageId, textType);
			bodyText.text += additionalText;
			OpenPanel(position);
		}
		#endregion methods
	}
}