using BaseLib.Abstracts;
using BaseLib.Utils;
using Oddmelt.OddmeltCode.Character;

namespace Oddmelt.OddmeltCode.Potions;

[Pool(typeof(OddmeltPotionPool))]
public abstract class OddmeltPotion : CustomPotionModel;