using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models;
using Oddmelt.Character;
using System.Threading.Tasks;

namespace Oddmelt.Cards;

[Pool(typeof(OddmeltCardPool))]
public abstract class OddmeltCard(int baseCost, CardType type, CardRarity rarity, TargetType target, bool showInCardLibrary = true, bool autoAdd = true) : CustomCardModel(baseCost, type, rarity, target, showInCardLibrary, autoAdd)
{
    public bool CanDissolve => DynamicVars.TryGetValue(DissolveVar.KEY, out var dissolve) && Owner != null && Owner.Creature.Block >= dissolve.IntValue;
    public static async Task<bool> Dissolve(CardModel card)
    {
        if (card.DynamicVars.TryGetValue(DissolveVar.KEY, out var dissolve) && card.Owner != null && card.Owner.Creature.Block >= dissolve.IntValue)
        {
            await CreatureCmd.LoseBlock(card.Owner.Creature, dissolve.IntValue);
            return true;
        }
        return false;
    }
}
