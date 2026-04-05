using BaseLib.Abstracts;
using BaseLib.Utils;
using Oddmelt.OddmeltCode.Extensions;
using Godot;
using MegaCrit.Sts2.Core.Entities.Characters;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Combat;
using Oddmelt.OddmeltCode.Cards.Basic;
using Oddmelt.OddmeltCode.Relics;

namespace Oddmelt.OddmeltCode.Character;

public class Oddmelt : PlaceholderCharacterModel
{
    public const string CharacterId = "Oddmelt";

    public override string PlaceholderID => "necrobinder";
    
    public static readonly Color Color = new("c4278a");

    public override Color NameColor => Color;
    public override CharacterGender Gender => CharacterGender.Neutral;
    public override int StartingHp => 75;

    public override IEnumerable<CardModel> StartingDeck => [
        ModelDb.Card<StrikeOddmelt>(),
        ModelDb.Card<StrikeOddmelt>(),
        ModelDb.Card<StrikeOddmelt>(),
        ModelDb.Card<StrikeOddmelt>(),
        ModelDb.Card<StrikeOddmelt>(),
        ModelDb.Card<DefendOddmelt>(),
        ModelDb.Card<DefendOddmelt>(),
        ModelDb.Card<DefendOddmelt>(),
        ModelDb.Card<DefendOddmelt>(),
        ModelDb.Card<Splatter>()
    ];

    public override IReadOnlyList<RelicModel> StartingRelics => [ModelDb.Relic<CorrodedNeedle>()];

    public override CardPoolModel CardPool => ModelDb.CardPool<OddmeltCardPool>();
    public override RelicPoolModel RelicPool => ModelDb.RelicPool<OddmeltRelicPool>();
    public override PotionPoolModel PotionPool => ModelDb.PotionPool<OddmeltPotionPool>();

    /*  PlaceholderCharacterModel will utilize placeholder basegame assets for most of your character assets until you
        override all the other methods that define those assets.
        These are just some of the simplest assets, given some placeholders to differentiate your character with.
        You don't have to, but you're suggested to rename these images. */
    public override string CustomVisualPath => "res://Oddmelt/scenes/oddmelt.tscn";
    /*public override NCreatureVisuals? CreateCustomVisuals()
    {
        return GodotUtils.CreatureVisualsFromImage("res://Oddmelt/images/character/placeholder.png");
    }*/

    public override string CustomIconTexturePath => "character_icon_char_name.png".CharacterUiPath();
    public override string CustomCharacterSelectIconPath => "char_select_char_name.png".CharacterUiPath();
    public override string CustomCharacterSelectLockedIconPath => "char_select_char_name_locked.png".CharacterUiPath();
    public override string CustomMapMarkerPath => "map_marker_char_name.png".CharacterUiPath();
}