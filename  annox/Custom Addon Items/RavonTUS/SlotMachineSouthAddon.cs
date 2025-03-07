
////////////////////////////////////////
//                                    //
//   Generated by CEO's YAAAG - V1.2  //
// (Yet Another Arya Addon Generator) //
//                                    //
////////////////////////////////////////
using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class SlotMachineSouthAddon : BaseAddon
	{
        private static int[,] m_AddOnSimpleComponents = new int[,] {
			  {10076, 1, 0, 10}, {10082, 1, 0, 10}, {10079, 0, 1, 10}// 1	2	3	
			, {10076, -1, -1, 0}, {10076, 1, 1, 10}// 5	7	
		};

 
            
		public override BaseAddonDeed Deed
		{
			get
			{
				return new SlotMachineSouthAddonDeed();
			}
		}

		[ Constructable ]
		public SlotMachineSouthAddon()
		{

            for (int i = 0; i < m_AddOnSimpleComponents.Length / 4; i++)
                AddComponent( new AddonComponent( m_AddOnSimpleComponents[i,0] ), m_AddOnSimpleComponents[i,1], m_AddOnSimpleComponents[i,2], m_AddOnSimpleComponents[i,3] );


			AddComplexComponent( (BaseAddon) this, 10069, 1, 1, 10, 0, -1, "Slot Machine", 1);// 4
			AddComplexComponent( (BaseAddon) this, 10757, 1, 1, 10, 0, -1, "Slot Machine", 1);// 6
			AddComplexComponent( (BaseAddon) this, 12221, 0, 1, 12, 0, -1, "Lever", 1);// 8
			AddComplexComponent( (BaseAddon) this, 6855, 1, 1, 30, 0, -1, "Slot Machine", 1);// 9

		}

		public SlotMachineSouthAddon( Serial serial ) : base( serial )
		{
		}

        private static void AddComplexComponent(BaseAddon addon, int item, int xoffset, int yoffset, int zoffset, int hue, int lightsource)
        {
            AddComplexComponent(addon, item, xoffset, yoffset, zoffset, hue, lightsource, null, 1);
        }

        private static void AddComplexComponent(BaseAddon addon, int item, int xoffset, int yoffset, int zoffset, int hue, int lightsource, string name, int amount)
        {
            AddonComponent ac;
            ac = new AddonComponent(item);
            if (name != null && name.Length > 0)
                ac.Name = name;
            if (hue != 0)
                ac.Hue = hue;
            if (amount > 1)
            {
                ac.Stackable = true;
                ac.Amount = amount;
            }
            if (lightsource != -1)
                ac.Light = (LightType) lightsource;
            addon.AddComponent(ac, xoffset, yoffset, zoffset);
        }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( 0 ); // Version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}

	public class SlotMachineSouthAddonDeed : BaseAddonDeed
	{
		public override BaseAddon Addon
		{
			get
			{
				return new SlotMachineSouthAddon();
			}
		}

		[Constructable]
		public SlotMachineSouthAddonDeed()
		{
			Name = "SlotMachineSouth";
		}

		public SlotMachineSouthAddonDeed( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( 0 ); // Version
		}

		public override void	Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}
}