using BaseLib.Patches.UI;
using BaseLib.Utils;
using BaseLib.Utils.NodeFactories;
using Godot;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using Oddmelt.OddmeltCode.Extensions;

namespace Oddmelt.OddmeltCode.Cards.Common;

public class Hop : OddmeltCard, ICustomUiModel
{
    public Hop() : base(1, CardType.Skill,
        CardRarity.Common, TargetType.Self)
    {
        WithBlock(9, 3);
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.CardBlock(this, play);
    }

    public void CreateCustomUi(Control toAdd)
    {
        var control = NodeFactory<Control>.CreateFromResource(ResourceLoader.Load<Texture2D>("relics/relic.png".ImagePath()));

        control.Size = new(50, 50);
        control.Position = new(-126, -231);
        var label = new Label { Text = "1" };
        label.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.Center);
        control.AddChild(label);
        
        toAdd.AddChild(control);
    }
}