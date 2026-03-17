using BaseLib.Abstracts;
using Godot;

namespace Oddmelt.OddmeltCode.Character;

public class OddmeltRelicPool : CustomRelicPoolModel
{
    public override string EnergyColorName => Oddmelt.CharacterId;
    public override Color LabOutlineColor => Oddmelt.Color;
}