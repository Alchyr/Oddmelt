using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using Oddmelt.OddmeltCode.Character;
using Oddmelt.OddmeltCode.Extensions;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models;

namespace Oddmelt.OddmeltCode.Cards;

[Pool(typeof(OddmeltCardPool))]
public abstract class OddmeltCard(int cost, CardType type, CardRarity rarity, TargetType target) :
    ConstructedCardModel(cost, type, rarity, target)
{
    //Image size:
    //Normal art: 1000x760 (Using 500x380 should also work, it will simply be scaled.)
    //Full art: 606x852
    public override string CustomPortraitPath
    {
        get
        {
            var path = $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".BigCardImagePath();
            return ResourceLoader.Exists(path) ? path : "card.png".BigCardImagePath();
        }
    }

    //Smaller variants of card images for efficiency:
    //Smaller variant of fullart: 250x350
    //Smaller variant of normalart: 250x190

    //Uses card_portraits/card_name.png as image path. These should be smaller images.
    public override string PortraitPath
    {
        get
        {
            var path = $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".CardImagePath();
            return ResourceLoader.Exists(path) ? path : "card.png".CardImagePath();
        }
    }

    //Optional and I'm not sure it's functional yet.
    public override string BetaPortraitPath
    {
        get
        {
            var path = $"beta/{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".CardImagePath();
            return ResourceLoader.Exists(path) ? path : "beta/card.png".CardImagePath();
        }
    }
    
    protected void WithDissolve(int baseVal)
    {
        WithVars(new DissolveVar(baseVal));
    }
    
    public bool CanDissolve => DynamicVars.TryGetValue(DissolveVar.Key, out var dissolve) && Owner != null && Owner.Creature.Block >= dissolve.IntValue;
    public static async Task<bool> Dissolve(CardModel card)
    {
        if (!card.DynamicVars.TryGetValue(DissolveVar.Key, out var dissolve)
            || card.Owner == null || card.Owner.Creature.Block < dissolve.IntValue) return false;
        
        await CreatureCmd.LoseBlock(card.Owner.Creature, dissolve.IntValue);
        return true;
    }
}