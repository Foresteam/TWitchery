using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using System.Collections.Generic;

namespace TWitchery.Tiles {
	public class TECauldron : TEAbstractStation, IRightClickable {
		private static List<WitcheryRecipe> _recipes = new(new WitcheryRecipe[] {
			new WitcheryRecipe(energyCost: 0)
				.AddIngredient(new Item(ItemID.DirtBlock, 5))
				.SetCatalyst(new Item(ItemID.Wood, 1))
				.AddResult(new WitcheryRecipe.Result.ItemResult(new Item(ItemID.StonePlatform, 10)))
		});
		private WitcheryCrafting _crafting;
		public override StackedInventory Inventory => _crafting;
		public TECauldron() {
			_crafting = new WitcheryCrafting(5, true, true, _recipes);
		}

		public override bool IsValidTile(in Tile tile) => tile.TileType == ModContent.TileType<TestCauldron>();
		protected override void OnPlace(int i, int j) {
			// ass.Add(Main.rand.Next());
			Main.NewText("I exist, therefore i am in the world.");
		}
		public bool RightClick(int i, int j) {
			var ply = Main.LocalPlayer;
			int slot = ply.selectedItem;
			var inv = ply.inventory;
			// no mouse yet
			ref var activeItem = ref inv[slot];
			switch (_crafting.Interract(i, j, ply, inv, slot)) {
				case WitcheryCrafting.Action.Take:
					_crafting.Take(i, j, ply);
					break;
				case WitcheryCrafting.Action.Put:
					_crafting.Put(ref activeItem);
					break;
				case WitcheryCrafting.Action.PutCatalyst:
					_crafting.PutCatalyst(ref activeItem);
					break;
				case WitcheryCrafting.Action.Craft:
					var rs = _crafting.Craft();
					WitcheryCrafting.GiveResult(rs, new Terraria.DataStructures.Point16(i, j), ply, this);
					break;
				default:
					return false;
			}
			return true;
		}
	}
}