//http://en.wikipedia.org/wiki/Wicce
using System;
using System.Collections;
using Server;

namespace Server.Mobiles
{
    public class Wicce : BaseVendor
    {
        private ArrayList m_SBInfos = new ArrayList();
        protected override ArrayList SBInfos { get { return m_SBInfos; } }

        //adding guild required changes to playermobile.cs and I don't like distro changes.
        //public override NpcGuild NpcGuild{ get{ return NpcGuild.WicceGuild; } }

        [Constructable]
        public Wicce()
            : base("the Wicce")
        {
            SetSkill(SkillName.EvalInt, 65.0, 88.0);
            SetSkill(SkillName.Inscribe, 60.0, 83.0);
            SetSkill(SkillName.Magery, 64.0, 100.0);
            SetSkill(SkillName.Meditation, 60.0, 83.0);
            SetSkill(SkillName.MagicResist, 65.0, 88.0);
            SetSkill(SkillName.Wrestling, 36.0, 68.0);

            VirtualArmor = 30;
            Female = true;
        }

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBWicce());
        }

        //public override VendorShoeType ShoeType
        //{
        //    get{ return Utility.RandomBool() ? VendorShoeType.Shoes : VendorShoeType.Sandals; }
        //}

        public override void InitOutfit()
        {
            //base.InitOutfit();
            //
            //AddItem( new Server.Items.Robe( Utility.RandomBlueHue() ) );
            AddItem(new Server.Items.Robe(0));
            AddItem(new Server.Items.Shoes(37));
            AddItem(new Server.Items.MagicWizardsHat(53));
        }

        public Wicce(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }
}