using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Oddmelt.OddmeltCode;

public class HealOnCardPlay() : CustomSingletonModel(true, false)
{
    public override async Task AfterCardPlayedLate(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.Heal(cardPlay.Card.Owner.Creature, 1);
    }
}