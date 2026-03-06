using BaseLib.Abstracts;
using Godot;

namespace Oddmelt.Character;

public class OddmeltRelicPool : CustomRelicPoolModel
{
    public override string EnergyColorName => "oddmelt";
    public override Color LabOutlineColor => Oddmelt.Color;
}
