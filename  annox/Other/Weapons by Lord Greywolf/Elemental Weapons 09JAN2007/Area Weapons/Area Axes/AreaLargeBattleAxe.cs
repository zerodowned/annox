using System;
using Server;
using Server.Mobiles;
using Server.Targeting;

namespace Server.Items
{
	public class AreaLargeBattleAxe : LargeBattleAxe
	{
		public override WeaponAbility PrimaryAbility{ get{ return WeaponAbility.WhirlwindAttack; } }
		public override WeaponAbility SecondaryAbility{ get{ return WeaponAbility.BleedAttack; } }

		public override int InitMinHits{ get{ return 255; } }
		public override int InitMaxHits{ get{ return 255; } }
		[Constructable]
		public AreaLargeBattleAxe()
		{
			Name = "Large Battle Axe";
		}

		public AreaLargeBattleAxe(Serial serial) : base( serial ) { }
		public override void Serialize( GenericWriter writer ) { base.Serialize( writer ); writer.Write( (int) 0 ); }
		public override void Deserialize(GenericReader reader) { base.Deserialize( reader ); int version = reader.ReadInt(); }
	}
}
