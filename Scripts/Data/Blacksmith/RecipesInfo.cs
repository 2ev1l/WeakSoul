using Data.ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Data
{
	public class RecipesInfo : MonoBehaviour
	{
		#region fields & properties
		public static RecipesInfo Instance { get; private set; }
		[field: SerializeField] public List<CraftRecipeSO> Recipes { get; private set; }
		#endregion fields & properties

		#region methods
		public void Init()
		{
			Instance = this;
		}
		public CraftRecipe GetRecipe(int recipeId) => Recipes[recipeId].Recipe;
		public Sprite GetRecipeTexture(int recipeId) => ItemsInfo.Instance.GetItem(Recipes[recipeId].Recipe.ItemId).Texture;

		[ContextMenu("Get all")]
		private void Get()
		{
			Recipes = new();
			Recipes = Resources.FindObjectsOfTypeAll<CraftRecipeSO>().OrderBy(x => x.Recipe.Id).ToList();
			foreach (var el in Recipes)
			{
				if (Recipes.Where(x => x.Recipe.Id == el.Recipe.Id).Count() > 1)
					Debug.LogError($"Error - Recipe id {el.Recipe.Id} at {el.name}");
				List<CraftSet> order = el.Recipe.GetItems();
				if (order.Where(x => x.Id != 0 && x.Thing == CraftingThing.None).Count() > 0)
					Debug.LogError($"Error - Recipe <none> thing at {el.name}");
				if (order.Where(x => x.Id > 4 && x.Thing == CraftingThing.Soul).Count() > 0)
					Debug.LogError($"Error - Recipe <soul> thing at {el.name}");
			}
		}

		[ContextMenu("Add opened recipes")]
		private void AOR()
		{
			for (int i = startId; i < endId; ++i)
			{
				GameData.Data.BlacksmithData.TryOpenRecipe(i);
			}
		}
		[SerializeField] private int startId;
		[SerializeField] private int endId;
		#endregion methods
	}
}