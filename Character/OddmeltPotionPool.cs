using BaseLib.Abstracts;
using Godot;

namespace Oddmelt.Character;

public class OddmeltPotionPool : CustomPotionPoolModel
{
    public override string EnergyColorName => "oddmelt";
    public override Color LabOutlineColor => Oddmelt.Color;
}
