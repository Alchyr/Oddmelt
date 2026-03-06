using BaseLib.Abstracts;
using Godot;
using MegaCrit.Sts2.Core.Entities.Characters;
using MegaCrit.Sts2.Core.Models;
using Oddmelt.Cards.Basic;
using Oddmelt.Relics;
using System.Collections.Generic;

namespace Oddmelt.Character;

public class Oddmelt : PlaceholderCharacterModel
{
    public static readonly Color Color = new("c4278a");

    public override string PlaceholderID => "necrobinder";
    public override Color NameColor => Color;

    public override int StartingHp => 75;
    public override int StartingGold => 99;

    public override CharacterGender Gender => CharacterGender.Neutral;

    /*public override IEnumerable<IReadOnlyList<CardModel>> CardBundles => [
        [
            ModelDb.Card<StrikeOddmelt>(),
            ModelDb.Card<StrikeOddmelt>(),
            ModelDb.Card<StrikeOddmelt>()
        ],
        [
            ModelDb.Card<DefendOddmelt>(),
            ModelDb.Card<DefendOddmelt>(),
            ModelDb.Card<DefendOddmelt>()
        ]
    ];*/

    public override CardPoolModel CardPool => ModelDb.CardPool<OddmeltCardPool>();
    public override RelicPoolModel RelicPool => ModelDb.RelicPool<OddmeltRelicPool>();
    public override PotionPoolModel PotionPool => ModelDb.PotionPool<OddmeltPotionPool>();

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

    public override IReadOnlyList<RelicModel> StartingRelics => [ModelDb.Relic<CorrodedRibbon>()];


    //Visuals
    public override string CustomVisualPath => "res://scenes/oddmelt/oddmelt.tscn";
    public override string CustomIconTexturePath => "res://images/oddmelt/character_icon_oddmelt.png";
    public override string CustomCharacterSelectIconPath => "res://images/oddmelt/char_select_oddmelt.png";
    public override string CustomCharacterSelectLockedIconPath => "res://images/oddmelt/char_select_oddmelt_locked.png";
    public override string CustomMapMarkerPath => "res://images/oddmelt/map_marker_oddmelt.png";
}
