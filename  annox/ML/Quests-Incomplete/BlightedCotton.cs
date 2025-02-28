using System;
using Server;

namespace Server.Items
{
	public class BlightedCotton : Item
	{	
		//public override int Lifespan{ get{ return 21600; } }
		//public override int LabelNumber{ get{ return 1074331; } } // blighted cotton
	
		[Constructable]
		public BlightedCotton() : base( 0x2DB )
		{
			Weight = 1;
			Hue = 0x35; // TODO check
		}

		public BlightedCotton( Serial serial ) : base( serial )
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

