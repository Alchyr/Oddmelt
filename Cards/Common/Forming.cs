using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.ValueProps;
using Oddmelt.Patches;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Oddmelt.Cards.Common;

public class Forming() : OddmeltCard(1, CardType.Skill, CardRarity.Common, TargetType.Self)
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => [OddmeltKeywords.Stitch];
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new BlockVar(4, ValueProp.Move),
        ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await CommonActions.CardBlock(this, play);
        await StitchHelper.StitchOnPlay(this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["Block"].UpgradeValueBy(2m);
    }
}
