using System;
using Server;
using Server.Misc;
using System.Collections;
using Server.Network;


namespace Server.Items
{
	public class SmallFishies : Food
	{
		[Constructable]
		public SmallFishies() : this( 1 )
		{
		}

		[Constructable]
		public SmallFishies( int amount ) : base( amount, 3543 )
		{
			Weight = 0.1;
			FillFactor = 3;
			Name = "Small Fishies";
		}

		public SmallFishies( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
}