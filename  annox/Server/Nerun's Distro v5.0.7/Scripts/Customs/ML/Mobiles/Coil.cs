using System;
using Server.Mobiles;
using Server.Factions;

namespace Server.Mobiles
{
	[CorpseName("a coil corpse")]
	[TypeAlias( "Server.Mobiles.Silverserpant" )]
	public class Coil : BaseCreature
	{
		public override Faction FactionAllegiance{ get{ return TrueBritannians.Instance; } }

		[Constructable]
		public Coil() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Body = 92;
			Name = "a coil";
			BaseSoundID = 219;

			SetStr( 337 );
			SetDex( 364 );
			SetInt( 126 );

			SetHits( 500 );

			SetDamage( 5, 20 );

			SetDamageType( ResistanceType.Physical, 50 );
			SetDamageType( ResistanceType.Poison, 50 );

			SetResistance( ResistanceType.Physical, 59 );
			SetResistance( ResistanceType.Fire, 29 );
			SetResistance( ResistanceType.Cold, 26 );
			SetResistance( ResistanceType.Poison, 100 );
			SetResistance( ResistanceType.Energy, 28 );

			SetSkill( SkillName.Poisoning, 117.8 );
			SetSkill( SkillName.MagicResist, 113.7 );
			SetSkill( SkillName.Tactics, 137.6 );
			SetSkill( SkillName.Wrestling, 131.8 );

			Fame = 7000;
			Karma = -7000;

			VirtualArmor = 40;
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Average );
			AddLoot( LootPack.Gems, 2 );
		}

		public override bool DeathAdderCharmable{ get{ return true; } }

		public override int Meat{ get{ return 1; } }
		public override Poison PoisonImmune{ get{ return Poison.Lethal; } }
		public override Poison HitPoison{ get{ return Poison.Lethal; } }

		public Coil(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int) 0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();

			if ( BaseSoundID == -1 )
				BaseSoundID = 219;
		}
	}
}