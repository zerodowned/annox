using System;

namespace Server.Items
{

    [Flipable(0x2684, 0x2683)]
    public class DeathsRobe : BaseOuterTorso
    {
        [Constructable]
        public DeathsRobe()
            : this(0x455)
        {
        }

        [Constructable]
        public DeathsRobe(int hue)
            : base(0x2684, hue)
        {
            LootType = LootType.Blessed;
            Weight = 3.0;
            Name = "Deaths Robe";
            Hue = 1175;
        }

        public override bool Dye(Mobile from, DyeTub sender)
        {
            from.SendLocalizedMessage(sender.FailMessage);
            return false;
        }

        public DeathsRobe(Serial serial)
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
