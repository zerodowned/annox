// Created by Script Creator
// From Aries at Revenge of the Gods
using System;
using Server;
namespace Server.Items
{
    public class DaemonDeath : WarMace
    {
        public override int InitMinHits { get { return 250; } }
        public override int InitMaxHits { get { return 250; } }
        [Constructable]
        public DaemonDeath()
        {
            Name = "Daemon Death";
            Attributes.WeaponDamage = 100;
            Attributes.AttackChance = 20;
            WeaponAttributes.HitLeechHits = 20;
            WeaponAttributes.HitLeechMana = 25;
            WeaponAttributes.HitLeechStam = 35;
            WeaponAttributes.HitMagicArrow = 100;
            WeaponAttributes.SelfRepair = 75;
            Attributes.BonusDex = 10;
            Attributes.BonusStam = 100;
            Attributes.Luck = 150;
            Attributes.ReflectPhysical = 35;
            Attributes.RegenHits = 20;
            Attributes.RegenMana = 20;
            Attributes.RegenStam = 20;
            Attributes.SpellChanneling = 1;
            Attributes.WeaponSpeed = 100;
            LootType = LootType.Blessed;
        }
        public override void GetDamageTypes(Mobile wielder, out int phys, out int fire, out int cold, out int pois, out int nrgy, out int chaos, out int direct )
        {
            phys = 100;
            cold = 0;
            fire = 0;
            nrgy = 0;
            pois = 0;
            chaos = direct = 0;
        }
        public DaemonDeath(Serial serial)
            : base(serial)
        {
        }
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }
        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
