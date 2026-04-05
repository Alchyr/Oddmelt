using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;
using Oddmelt.OddmeltCode.Powers;

namespace Oddmelt.OddmeltCode.Cards.Uncommon;

public class Promise : OddmeltCard
{
    public Promise() : base(0,
        CardType.Skill, CardRarity.Uncommon,
        TargetType.Self)
    {
        WithPower<StrengthPower>(1);
        WithPower<Bind>(6);
    }


    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.ApplySelf<StrengthPower>(this, DynamicVars.Strength.BaseValue);
        await CommonActions.ApplySelf<Bind>(this, DynamicVars["Bind"].BaseValue);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Strength.UpgradeValueBy(1);
    }
}