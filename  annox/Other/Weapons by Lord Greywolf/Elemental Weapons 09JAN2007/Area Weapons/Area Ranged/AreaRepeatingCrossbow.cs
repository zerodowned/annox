using System;
using Server;
using Server.Mobiles;
using Server.Targeting;

namespace Server.Items
{
	public class AreaRepeatingCrossbow : RepeatingCrossbow
	{
		public override WeaponAbility PrimaryAbility{ get{ return WeaponAbility.DoubleStrike; } }
		public override WeaponAbility SecondaryAbility{ get{ return WeaponAbility.MovingShot; } }

		public override int InitMinHits{ get{ return 255; } }
		public override int InitMaxHits{ get{ return 255; } }
		[Constructable]
		public AreaRepeatingCrossbow()
		{
			Name = "Repeating Crossbow";
		}

		public AreaRepeatingCrossbow(Serial serial) : base( serial ) { }
		public override void Serialize( GenericWriter writer ) { base.Serialize( writer ); writer.Write( (int) 0 ); }
		public override void Deserialize(GenericReader reader) { base.Deserialize( reader ); int version = reader.ReadInt(); }
	}
}
