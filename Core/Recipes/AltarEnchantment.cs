using System.Linq;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace TWitchery.Recipes;
using RecipeItems;
using Liquids;
using Terraria.ID;

/// <summary>No actual difference. Just fits best</summary>
#nullable enable
class AltarEnchantment : WitcheryRecipe {
	private EnchantmentData? _enchantment = null;
	public AltarEnchantment(float energyCost, float failedWorkedChance = 0, float matchThreshold = .75f) : base(energyCost, failedWorkedChance, matchThreshold) { }
	public AltarEnchantment SetTarget(Item target) => (AltarEnchantment)base.SetCatalyst(target);
	public AltarEnchantment SetTarget(Recipes.RecipeItems.RecipeItem target) => (AltarEnchantment)base.SetCatalyst(target);
	public override AltarEnchantment AddIngredient(Item ingredient) => (AltarEnchantment)base.AddIngredient(ingredient);
	public override AltarEnchantment AddIngredient(Recipes.RecipeItems.RecipeItem ingredient) => (AltarEnchantment)base.AddIngredient(ingredient);
	public AltarEnchantment Enchant(EnchantmentData data) {
		// we want to accumulate enchantments to allow chain .notation
		if (_enchantment == null)
			_enchantment = data;
		else
			_enchantment *= data;
		return this;
	}
	public AltarEnchantment AddResult() => (AltarEnchantment)AddResult(new Item());
	public override void GetResult(RecipeItem[] ritems, int? xAmount, Item[] items, Item catalyst, List<Liquid> liquids, ref Result result) {
		if (xAmount == null)
			return;
		result.items[0] = catalyst;
		if (_enchantment != null) 
			result.items[0].GetGlobalItem<Enchantment>().Apply((EnchantmentData)_enchantment);
		foreach (var item in result.items)
			item.stack *= (int)xAmount;
		foreach (var liquid in result.liquids)
			liquid.Volume *= (int)xAmount;
		result.energyCost *= (int)xAmount;
	}
}