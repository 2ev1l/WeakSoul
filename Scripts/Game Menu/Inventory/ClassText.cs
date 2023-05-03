using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal;

namespace WeakSoul.GameMenu.Inventory
{
    public class ClassText : MonoBehaviour
    {
        #region fields & properties
        [SerializeField] private LanguageLoader classLanguage;
        [SerializeField] private ShowTextHelp help;
		#endregion fields & properties

		#region methods
		private void OnEnable()
		{
			UpdateValues();
		}
		private void UpdateValues()
		{
			PlayerClass pClass = GameData.Data.PlayerData.Stats.Class;
			bool isTutorialCompleted = GameData.Data.TutorialData.IsCompleted;
			help.Id = isTutorialCompleted ? LanguageLoader.GetPlayerClassDescriptionIdByType(pClass) : -1;
			classLanguage.Id = isTutorialCompleted ? LanguageLoader.GetPlayerClassTextIdByType(pClass) : -1;
		}
		#endregion methods
	}
}