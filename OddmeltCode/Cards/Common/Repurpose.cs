using BaseLib.Utils;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Oddmelt.OddmeltCode.Cards.Common;

public class Repurpose : OddmeltCard
{
    public Repurpose() : base(0,
        CardType.Skill, CardRarity.Common,
        TargetType.Self)
    {
        WithVars(new CardsVar(1));
        WithKeywords(CardKeyword.Exhaust);
    }
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        int cardCount = DynamicVars.Cards.IntValue;
        
        var cardModel = await CommonActions.SelectSingleCard(this, SelectionScreenPrompt, choiceContext, PileType.Discard);
        if (cardModel != null)
        {
            await CardPileCmd.Add(cardModel, PileType.Hand);
        }
        
        await CardCmd.Discard(choiceContext, await CardSelectCmd.FromHandForDiscard(choiceContext, Owner, new CardSelectorPrefs(CardSelectorPrefs.DiscardSelectionPrompt, cardCount), null, this));
    }

    protected override void OnUpgrade()
    {
        RemoveKeyword(CardKeyword.Exhaust);
    }
}