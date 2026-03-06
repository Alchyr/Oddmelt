using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using Oddmelt.Patches;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Oddmelt.Cards.Common;

public class Wirecasting() : OddmeltCard(1, CardType.Attack, CardRarity.Common, TargetType.AllEnemies)
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => [OddmeltKeywords.Stitch];
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar(4, ValueProp.Move)
        ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await CommonActions.CardAttack(this, play).Execute(choiceContext);
        await StitchHelper.StitchOnPlay(this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(2m);
    }
}
