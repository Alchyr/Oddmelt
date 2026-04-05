using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace Oddmelt.OddmeltCode.Cards.Uncommon;

public class BlankSlate : OddmeltCard
{
    public BlankSlate() : base(2, CardType.Skill,
        CardRarity.Uncommon, TargetType.Self)
    {
        WithBlock(4);
        WithKeywords(CardKeyword.Retain, CardKeyword.Exhaust);
    }

    public override void AfterCreated()
    {
        BaseReplayCount += 3; //Needs save/load testing
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.CardBlock(this, play);
    }

    protected override void OnUpgrade()
    {
        BaseReplayCount += 1;
    }
}