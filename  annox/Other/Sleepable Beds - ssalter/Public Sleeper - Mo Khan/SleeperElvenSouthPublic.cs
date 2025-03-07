// Script Package: Sleepable Beds
// Version: 1.0
// Author: Oak
// Servers: RunUO 2.0
// Date: 7/10/2006 ( elven south)
// History: 
//  Written for RunUO 1.0 shard, Sylvan Dreams,  in February 2005. Based largely on work by David on his Sleepable NPCs scripts.
//  Modified for RunUO 2.0, removed shard specific customizations (wing layers, etc.)
//  

using System;
using System.Collections;
using Server;
using Server.Network;
using Server.Mobiles;
using Server.Gumps;
using Server.Multis;

namespace Server.Items
{
    // version 1.1.1 Bed coordinates of 0,0,0 will cause npc to sleep and wake at it's current location.
    // version 1.0 initial release.

    public class SleeperElvenSouthPublic : BaseAddon, IChopable
    {
        public override BaseAddonDeed Deed
        {
            get
            {
                return new SleeperElvenSouthPublicDeed();
            }
        }

        public SleeperElvenSouthPublic(Serial serial)
            : base(serial)
        {
        }

        [Constructable]
        public SleeperElvenSouthPublic()
        {
            Visible = true;
            Name = "Sleeper";
            AddComponent(new SleeperElvenSouthPiecePublic(this, 0x3050), 0, 0, 0);
            AddComponent(new SleeperElvenSouthPiecePublic(this, 0x3051), 0, -1, 0);

        }

        private SleeperElvenSouthPublic m_Sleeper;

        private SleeperBedBody m_SleeperBedBody;
        private bool m_Active = false;
        private Mobile m_Player;
        private Point3D m_Location;
        private Point3D m_PlayerLocation;
        private bool m_Sleeping = false;
        private bool m_Debug = false;
        private Mobile m_Owner;

        //wtry is the "wake try" counter. After two attempts to wake someone else up, you get zapped
        private int wtry;

        [CommandProperty(AccessLevel.GameMaster)]
        public Point3D Bed
        {
            get { return m_Location; }
            set { m_Location = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool Debug
        {
            get { return m_Debug; }
            set { m_Debug = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool Active
        {
            get { return m_Active; }
            set { m_Active = value; InvalidateProperties(); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool Asleep
        {
            get { return m_Sleeping; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public Mobile Mobile
        {
            get { return m_Player; }
            set
            {
                if (value == null)
                    m_Active = false;
                else
                    m_Active = true;

                m_Player = value;

                InvalidateProperties();
            }
        }


        [CommandProperty(AccessLevel.GameMaster)]
        public SleeperElvenSouthPublic Sleeper
        {
            get { return m_Sleeper; }
            set { }
        }

        private void Sleep()
        {
            if (m_Sleeping) return;

        }

        public void DoubleClick(Mobile from)
        {
            Mobile m_Player = from as PlayerMobile;
            if (m_Player.CantWalk && !m_Sleeping)
            {
                m_Player.LocalOverheadMessage(MessageType.Regular, 0x33, true, "You are already sleeping somewhere!");
            }
            else
            {
                if (!m_Sleeping)
                {
                    wtry = 0;
                    m_Owner = m_Player;
                    m_Player.Hidden = true;
                    m_Player.CantWalk = true;
                    m_Sleeping = true;
                    m_SleeperBedBody = new SleeperBedBody(m_Player, false, false);
                    Point3D m_Location = new Point3D(this.Location.X, this.Location.Y + 1, this.Location.Z + 8);
                    m_SleeperBedBody.Direction = Direction.South;
                    m_SleeperBedBody.MoveToWorld(m_Location, this.Map);

                }
                else
                {
                    if (m_Owner == m_Player)
                    {
                        m_Sleeping = false;
                        m_Player.Hidden = false;
                        m_Player.CantWalk = false;
                        if (m_SleeperBedBody != null)
                            m_SleeperBedBody.Delete();
                        m_SleeperBedBody = null;
                        switch (Utility.RandomMinMax(1, 3))
                        {
                            case 1:
                            m_Player.LocalOverheadMessage(MessageType.Regular, 0x33, true, "You wake up and feel rested and strong.");
                            break;
                            case 2:
                            m_Player.LocalOverheadMessage(MessageType.Regular, 0x33, true, "You spring out of bed, ready for another day!");
                            break;
                            case 3:
                            m_Player.LocalOverheadMessage(MessageType.Regular, 0x33, true, "You fall out of bed and blearily reach for the coffee pot.");
                            break;
                        }
                    }
                    else
                    {
                        switch (wtry)
                        {
                            case 0:
                            m_Player.LocalOverheadMessage(MessageType.Regular, 0x33, true, "Shhh, don't wake them up. They really need their beauty rest!");
                            wtry = wtry + 1;
                            break;
                            case 1:
                            m_Player.LocalOverheadMessage(MessageType.Regular, 0x33, true, "You really should NOT bother someone that is sleeping. Bad things might happen.");
                            wtry = wtry + 1;
                            break;
                            case 2:
                            m_Player.LocalOverheadMessage(MessageType.Regular, 0x33, true, "You were warned!! Now leave them alone.");
                            m_Player.FixedParticles(0x3709, 10, 30, 5052, EffectLayer.Head);
                            m_Player.PlaySound(0x208);
                            m_Player.Hits = m_Player.Hits - 40;
                            break;
                        }
                    }
                }
            }
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);

            string tmp = String.Format("{0}: {1}", this.Name, (m_Player != null ? m_Player.Name : "unassigned"));
            list.Add(tmp);

            if (m_Active)
                list.Add(1060742); // active
            else
                list.Add(1060743); // inactive
        }



        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
            writer.Write((Item)m_SleeperBedBody);
            writer.Write((Mobile)m_Player);
            writer.Write(m_Active);
            writer.Write(m_Location);
            writer.Write(m_Sleeping);
            writer.Write((Mobile)m_Owner);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_SleeperBedBody = (SleeperBedBody)reader.ReadItem();
            m_Player = reader.ReadMobile();
            m_Active = reader.ReadBool();
            m_Location = reader.ReadPoint3D();
            m_Sleeping = reader.ReadBool();
            m_Owner = reader.ReadMobile();

            m_Debug = false;
        }



    }

    public class SleeperElvenSouthPiecePublic : AddonComponent
    {

        private SleeperElvenSouthPublic m_Sleeper;

        public SleeperElvenSouthPiecePublic(SleeperElvenSouthPublic sleeper, int itemid)
            : base(itemid)
        {
            m_Sleeper = sleeper;
        }

        public override void OnDoubleClick(Mobile from)
        {
            m_Sleeper.DoubleClick(from);
        }

        public SleeperElvenSouthPiecePublic(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write((Item)m_Sleeper);

        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_Sleeper = (SleeperElvenSouthPublic)reader.ReadItem();

        }

    }
    public class SleeperElvenSouthPublicDeed : BaseAddonDeed
    {
        public override BaseAddon Addon
        {
            get
            {
                return new SleeperElvenSouthPublic();
            }
        }

        [Constructable]
        public SleeperElvenSouthPublicDeed()
        {
            Name = "a sleepable bed facing south deed.";
        }

        public SleeperElvenSouthPublicDeed(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // Version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
