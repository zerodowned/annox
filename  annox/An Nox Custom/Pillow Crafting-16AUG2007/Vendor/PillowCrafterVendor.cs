using System;
using System.Collections;
using Server;
using Server.Engines.BulkOrders;

namespace Server.Mobiles
{
    public class PillowCrafter : BaseVendor
    {
        private ArrayList m_SBInfos = new ArrayList();
        protected override ArrayList SBInfos { get { return m_SBInfos; } }

        public override NpcGuild NpcGuild { get { return NpcGuild.TailorsGuild; } }

        [Constructable]
        public PillowCrafter()
            : base("the pillow crafter")
        {
            SetSkill(SkillName.Tailoring, 65.0, 88.0);
        }

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBPillowCrafter());
        }

        public override VendorShoeType ShoeType
        {
            get { return VendorShoeType.Sandals; }
        }

        #region Bulk Orders
        public override Item CreateBulkOrder(Mobile from, bool fromContextMenu)
        {
            PlayerMobile pm = from as PlayerMobile;

            if (pm != null && pm.NextTailorBulkOrder == TimeSpan.Zero && (fromContextMenu || 0.2 > Utility.RandomDouble()))
            {
                double theirSkill = pm.Skills[SkillName.Tailoring].Base;

                if (theirSkill >= 70.1)
                    pm.NextTailorBulkOrder = TimeSpan.FromHours(6.0);
                else if (theirSkill >= 50.1)
                    pm.NextTailorBulkOrder = TimeSpan.FromHours(2.0);
                else
                    pm.NextTailorBulkOrder = TimeSpan.FromHours(1.0);

                //did not want LargeTailorBOD
                //if ( theirSkill >= 70.1 && ((theirSkill - 40.0) / 300.0) > Utility.RandomDouble() )
                //    return new LargeTailorBOD();

                return SmallPillowCraftBOD.CreateRandomFor(from);
            }

            return null;
        }

        public override bool IsValidBulkOrder(Item item)
        {
            return (item is SmallPillowCraftBOD); //|| item is LargeTailorBOD ); //did not want LargeTailorBOD
        }

        public override bool SupportsBulkOrders(Mobile from)
        {
            return (from is PlayerMobile && from.Skills[SkillName.Tailoring].Base > 0);
        }

        public override TimeSpan GetNextBulkOrder(Mobile from)
        {
            if (from is PlayerMobile)
                return ((PlayerMobile)from).NextTailorBulkOrder;

            return TimeSpan.Zero;
        }

        public override void OnSuccessfulBulkOrderReceive(Mobile from)
        {
            if (Core.SE && from is PlayerMobile)
                ((PlayerMobile)from).NextTailorBulkOrder = TimeSpan.Zero;
        }
        #endregion

        public PillowCrafter(Serial serial)
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