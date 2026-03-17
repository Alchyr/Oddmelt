using BaseLib.Abstracts;
using Godot;
using Oddmelt.OddmeltCode.Extensions;

namespace Oddmelt.OddmeltCode.Character;

public class OddmeltPotionPool : CustomPotionPoolModel
{
    public override Color LabOutlineColor => Oddmelt.Color;

    public override string BigEnergyIconPath => "charui/big_energy.png".ImagePath();
    public override string TextEnergyIconPath => "charui/text_energy.png".ImagePath();
}