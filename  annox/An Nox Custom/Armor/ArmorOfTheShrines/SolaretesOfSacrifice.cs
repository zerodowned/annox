//03APR2007 Armor of the Shrines by RavonTUS.  Play at An Nox, the cure for the UO addiction.
//Values are that of Bone Armor, if you know the correct values please pass them along.

using System;
using Server.Items;

namespace Server.Items
{
    [Flipable(0x2B12, 0x2B13)]
    public class SolaretesOfSacrifice : BaseShoes, IArcaneEquip
    {
        #region Arcane Impl
        private int m_MaxArcaneCharges, m_CurArcaneCharges;

        [CommandProperty(AccessLevel.GameMaster)]
        public int MaxArcaneCharges
        {
            get { return m_MaxArcaneCharges; }
            set { m_MaxArcaneCharges = value; InvalidateProperties(); Update(); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int CurArcaneCharges
        {
            get { return m_CurArcaneCharges; }
            set { m_CurArcaneCharges = value; InvalidateProperties(); Update(); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool IsArcane
        {
            get { return (m_MaxArcaneCharges > 0 && m_CurArcaneCharges >= 0); }
        }

        public override void OnSingleClick(Mobile from)
        {
            base.OnSingleClick(from);

            if (IsArcane)
                LabelTo(from, 1061837, String.Format("{0}\t{1}", m_CurArcaneCharges, m_MaxArcaneCharges));
        }

        public void Update()
        {
            if (IsArcane)
                ItemID = 0x26AF;
            else if (ItemID == 0x26AF)
                ItemID = 0x1711;

            if (IsArcane && CurArcaneCharges == 0)
                Hue = 0;
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);

            if (IsArcane)
                list.Add(1061837, "{0}\t{1}", m_CurArcaneCharges, m_MaxArcaneCharges); // arcane charges: ~1_val~ / ~2_val~
        }

        public void Flip()
        {
            if (ItemID == 0x1711)
                ItemID = 0x1712;
            else if (ItemID == 0x1712)
                ItemID = 0x1711;
        }
        #endregion

        public override CraftResource DefaultResource { get { return CraftResource.RegularLeather; } }

        [Constructable]
        public SolaretesOfSacrifice()
            : base(0x2B12)
        {
            Name = "Solaretes of Sacrifice";
            Weight = 4.0;
        }

        public SolaretesOfSacrifice(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)1); // version

            if (IsArcane)
            {
                writer.Write(true);
                writer.Write((int)m_CurArcaneCharges);
                writer.Write((int)m_MaxArcaneCharges);
            }
            else
            {
                writer.Write(false);
            }
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            switch (version)
            {
                case 1:
                    {
                        if (reader.ReadBool())
                        {
                            m_CurArcaneCharges = reader.ReadInt();
                            m_MaxArcaneCharges = reader.ReadInt();

                            if (Hue == 2118)
                                Hue = ArcaneGem.DefaultArcaneHue;
                        }

                        break;
                    }
            }
        }
    }
}
