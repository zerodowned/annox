using System;
using Server;

namespace Server.Items
{
	public class CRightArm : BaseBodyPart
	{
		[Constructable]
		public CRightArm() : base( AccessLevel.GameMaster )
		{
			ItemID = 7586;
		}

		public CRightArm( Serial serial ) : base( serial )
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