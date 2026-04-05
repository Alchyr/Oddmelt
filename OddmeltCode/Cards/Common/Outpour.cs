using BaseLib.Abstracts;
using BaseLib.Utils;
using Godot;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Oddmelt.OddmeltCode.Cards.Common;

public class Outpour : OddmeltCard
{
    public Outpour() : base(1,
        CardType.Attack, CardRarity.Common,
        TargetType.AnyEnemy)
    {
        
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        /*byte[] data = Godot.FileAccess.GetFileAsBytes("res://Oddmelt/audio/squelch.ogg");
        if (data.Length == 0)
        {
            MainFile.Logger.Info("Loaded file");
        }
        else
        {
            MainFile.Logger.Info("Failed to load: " + Godot.FileAccess.GetOpenError());
        }*/
        //BaseLib.Utils.FmodAudio.PlayFile("res://Oddmelt/audio/squelch.ogg");
    }

    protected override void OnUpgrade()
    {
        
    }

    public override List<(string, string)> Localization => new CardLoc("ODOBADJA", "ooooooOOOooooOoooooOoo");

    public override ShaderMaterial? CreateCustomFrameMaterial => ShaderUtils.GenerateHsv(0.3f, 1f, 0.8f);
}