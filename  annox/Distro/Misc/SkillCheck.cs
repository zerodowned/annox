//26OCT2007 Disable AntiMacroCode
//31OCT2007 Hourly Skill Boost
//27FEB2008 Found area to fix STR/DEX/INT (Two Places Time & Amount)

//Peek Hours of An Nox
// 2:00 -  3:59 AM
//11:00 - 11:59 AM
// 2:00 -  4:59 PM
//11:00 - 12:59 PM

using System;
using Server;
using Server.Mobiles;

namespace Server.Misc
{
    public class SkillCheck
    {
        //26OCT2007 Disable AntiMacroCode *** START ***
        //private static readonly bool AntiMacroCode = !Core.ML;		//Change this to false to disable anti-macro code
        private static readonly bool AntiMacroCode = false;		//Change this to false to disable anti-macro code
        //26OCT2007 Disable AntiMacroCode *** END   ***

        public static TimeSpan AntiMacroExpire = TimeSpan.FromMinutes(5.0); //How long do we remember targets/locations?
        public const int Allowance = 3;	//How many times may we use the same location/target for gain
        private const int LocationSize = 5; //The size of eeach location, make this smaller so players dont have to move as far
        private static bool[] UseAntiMacro = new bool[]
		{
			// true if this skill uses the anti-macro code, false if it does not
			false,// Alchemy = 0,
			true,// Anatomy = 1,
			true,// AnimalLore = 2,
			true,// ItemID = 3,
			true,// ArmsLore = 4,
			false,// Parry = 5,
			true,// Begging = 6,
			false,// Blacksmith = 7,
			false,// Fletching = 8,
			true,// Peacemaking = 9,
			true,// Camping = 10,
			false,// Carpentry = 11,
			false,// Cartography = 12,
			false,// Cooking = 13,
			true,// DetectHidden = 14,
			true,// Discordance = 15,
			true,// EvalInt = 16,
			true,// Healing = 17,
			true,// Fishing = 18,
			true,// Forensics = 19,
			true,// Herding = 20,
			true,// Hiding = 21,
			true,// Provocation = 22,
			false,// Inscribe = 23,
			true,// Lockpicking = 24,
			true,// Magery = 25,
			true,// MagicResist = 26,
			false,// Tactics = 27,
			true,// Snooping = 28,
			true,// Musicianship = 29,
			true,// Poisoning = 30,
			false,// Archery = 31,
			true,// SpiritSpeak = 32,
			true,// Stealing = 33,
			false,// Tailoring = 34,
			true,// AnimalTaming = 35,
			true,// TasteID = 36,
			false,// Tinkering = 37,
			true,// Tracking = 38,
			true,// Veterinary = 39,
			false,// Swords = 40,
			false,// Macing = 41,
			false,// Fencing = 42,
			false,// Wrestling = 43,
			true,// Lumberjacking = 44,
			true,// Mining = 45,
			true,// Meditation = 46,
			true,// Stealth = 47,
			true,// RemoveTrap = 48,
			true,// Necromancy = 49,
			false,// Focus = 50,
			true,// Chivalry = 51
			true,// Bushido = 52
			true,//Ninjitsu = 53
			true // Spellweaving
		};

        public static void Initialize()
        {
            Mobile.SkillCheckLocationHandler = new SkillCheckLocationHandler(Mobile_SkillCheckLocation);
            Mobile.SkillCheckDirectLocationHandler = new SkillCheckDirectLocationHandler(Mobile_SkillCheckDirectLocation);

            Mobile.SkillCheckTargetHandler = new SkillCheckTargetHandler(Mobile_SkillCheckTarget);
            Mobile.SkillCheckDirectTargetHandler = new SkillCheckDirectTargetHandler(Mobile_SkillCheckDirectTarget);
        }

        public static bool Mobile_SkillCheckLocation(Mobile from, SkillName skillName, double minSkill, double maxSkill)
        {
            Skill skill = from.Skills[skillName];

            if (skill == null)
                return false;

            double value = skill.Value;

            if (value < minSkill)
                return false; // Too difficult
            else if (value >= maxSkill)
                return true; // No challenge

            double chance = (value - minSkill) / (maxSkill - minSkill);

            Point2D loc = new Point2D(from.Location.X / LocationSize, from.Location.Y / LocationSize);
            return CheckSkill(from, skill, loc, chance);
        }

        public static bool Mobile_SkillCheckDirectLocation(Mobile from, SkillName skillName, double chance)
        {
            Skill skill = from.Skills[skillName];

            if (skill == null)
                return false;

            if (chance < 0.0)
                return false; // Too difficult
            else if (chance >= 1.0)
                return true; // No challenge

            Point2D loc = new Point2D(from.Location.X / LocationSize, from.Location.Y / LocationSize);
            return CheckSkill(from, skill, loc, chance);
        }

        public static bool CheckSkill(Mobile from, Skill skill, object amObj, double chance)
        {
            if (from.Skills.Cap == 0)
                return false;

            bool success = (chance >= Utility.RandomDouble());
            double gc = (double)(from.Skills.Cap - from.Skills.Total) / from.Skills.Cap;
            //Hourly Skill Boost *** START ***
            //gc += ( skill.Cap - skill.Base ) / skill.Cap;
            //gc /= 2;

            //gc += ( 1.0 - chance ) * ( success ? 0.5 : (Core.AOS ? 0.0 : 0.2) );
            //gc /= 2;

            //Peek Hours of An Nox
            // 2:00 -  3:59 AM
            //11:00 - 11:59 AM
            // 2:00 -  4:59 PM
            //11:00 - 12:59 PM

            DateTime now = DateTime.Now;

            if (now.Hour == 2 || now.Hour == 3 || now.Hour == 11 || now.Hour == 14 || now.Hour == 15 || now.Hour == 16 || now.Hour == 23)
            {
                gc += (skill.Cap - skill.Base) / skill.Cap;
                gc /= .75;

                gc += (1.0 - chance) * (success ? 0.5 : (Core.AOS ? 0.0 : 0.2));
                gc /= .75;
            }
            else
            {
                gc += (skill.Cap - skill.Base) / skill.Cap;
                gc /= 1;

                gc += (1.0 - chance) * (success ? 0.5 : (Core.AOS ? 0.0 : 0.2));
                gc /= 1;
            }
            //Hourly Skill Boost *** END   ***

            gc *= skill.Info.GainFactor;

            if (gc < 0.01)
                gc = 0.01;

            if (from is BaseCreature && ((BaseCreature)from).Controlled)
                gc *= 2;

            if (from.Alive && ((gc >= Utility.RandomDouble() && AllowGain(from, skill, amObj)) || skill.Base < 10.0))
                Gain(from, skill);

            return success;
        }

        public static bool Mobile_SkillCheckTarget(Mobile from, SkillName skillName, object target, double minSkill, double maxSkill)
        {
            Skill skill = from.Skills[skillName];

            if (skill == null)
                return false;

            double value = skill.Value;

            if (value < minSkill)
                return false; // Too difficult
            else if (value >= maxSkill)
                return true; // No challenge

            double chance = (value - minSkill) / (maxSkill - minSkill);

            return CheckSkill(from, skill, target, chance);
        }

        public static bool Mobile_SkillCheckDirectTarget(Mobile from, SkillName skillName, object target, double chance)
        {
            Skill skill = from.Skills[skillName];

            if (skill == null)
                return false;

            if (chance < 0.0)
                return false; // Too difficult
            else if (chance >= 1.0)
                return true; // No challenge

            return CheckSkill(from, skill, target, chance);
        }

        private static bool AllowGain(Mobile from, Skill skill, object obj)
        {
            if (AntiMacroCode && from is PlayerMobile && UseAntiMacro[skill.Info.SkillID])
                return ((PlayerMobile)from).AntiMacroCheck(skill, obj);
            else
                return true;
        }

        public enum Stat { Str, Dex, Int }

        public static void Gain(Mobile from, Skill skill)
        {
            if (from.Region.IsPartOf(typeof(Regions.Jail)))
                return;

            if (from is BaseCreature && ((BaseCreature)from).IsDeadPet)
                return;

            if (skill.SkillName == SkillName.Focus && from is BaseCreature)
                return;

            if (skill.Base < skill.Cap && skill.Lock == SkillLock.Up)
            {
                int toGain = 1;

                if (skill.Base <= 10.0)
                    toGain = Utility.Random(4) + 1;

                Skills skills = from.Skills;

                if (from.Player && (skills.Total / skills.Cap) >= Utility.RandomDouble())//( skills.Total >= skills.Cap )
                {
                    for (int i = 0; i < skills.Length; ++i)
                    {
                        Skill toLower = skills[i];

                        if (toLower != skill && toLower.Lock == SkillLock.Down && toLower.BaseFixedPoint >= toGain)
                        {
                            toLower.BaseFixedPoint -= toGain;
                            break;
                        }
                    }
                }

                if (!from.Player || (skills.Total + toGain) <= skills.Cap)
                {
                    skill.BaseFixedPoint += toGain;
                }
            }

            if (skill.Lock == SkillLock.Up)
            {
                SkillInfo info = skill.Info;

                if (from.StrLock == StatLockType.Up && (info.StrGain / 33.3) > Utility.RandomDouble())
                    GainStat(from, Stat.Str);
                else if (from.DexLock == StatLockType.Up && (info.DexGain / 33.3) > Utility.RandomDouble())
                    GainStat(from, Stat.Dex);
                else if (from.IntLock == StatLockType.Up && (info.IntGain / 33.3) > Utility.RandomDouble())
                    GainStat(from, Stat.Int);

                //Peek Hours of An Nox
                // 2:00 -  3:59 AM
                //11:00 - 11:59 AM
                // 2:00 -  4:59 PM
                //11:00 - 12:59 PM

                DateTime now = DateTime.Now;

                if (now.Hour == 2 || now.Hour == 3 || now.Hour == 11 || now.Hour == 14 || now.Hour == 15 || now.Hour == 16 || now.Hour == 23)
                {
                    if (from.StrLock == StatLockType.Up && (info.StrGain / 16.65) > Utility.RandomDouble())
                        GainStat(from, Stat.Str);
                    else if (from.DexLock == StatLockType.Up && (info.DexGain / 16.65) > Utility.RandomDouble())
                        GainStat(from, Stat.Dex);
                    else if (from.IntLock == StatLockType.Up && (info.IntGain / 16.65) > Utility.RandomDouble())
                        GainStat(from, Stat.Int);
                }
                else
                {

                    if (from.StrLock == StatLockType.Up && (info.StrGain / 33.3) > Utility.RandomDouble())
                        GainStat(from, Stat.Str);
                    else if (from.DexLock == StatLockType.Up && (info.DexGain / 33.3) > Utility.RandomDouble())
                        GainStat(from, Stat.Dex);
                    else if (from.IntLock == StatLockType.Up && (info.IntGain / 33.3) > Utility.RandomDouble())
                        GainStat(from, Stat.Int);
                }
            }
        }

        public static bool CanLower(Mobile from, Stat stat)
        {
            switch (stat)
            {
                case Stat.Str: return (from.StrLock == StatLockType.Down && from.RawStr > 10);
                case Stat.Dex: return (from.DexLock == StatLockType.Down && from.RawDex > 10);
                case Stat.Int: return (from.IntLock == StatLockType.Down && from.RawInt > 10);
            }

            return false;
        }

        public static bool CanRaise(Mobile from, Stat stat)
        {
            if (!(from is BaseCreature && ((BaseCreature)from).Controlled))
            {
                if (from.RawStatTotal >= from.StatCap)
                    return false;
            }

            switch (stat)
            {
                case Stat.Str: return (from.StrLock == StatLockType.Up && from.RawStr < 125);
                case Stat.Dex: return (from.DexLock == StatLockType.Up && from.RawDex < 125);
                case Stat.Int: return (from.IntLock == StatLockType.Up && from.RawInt < 125);
            }

            return false;
        }

        public static void IncreaseStat(Mobile from, Stat stat, bool atrophy)
        {
            atrophy = atrophy || (from.RawStatTotal >= from.StatCap);

            switch (stat)
            {
                case Stat.Str:
                    {
                        if (atrophy)
                        {
                            if (CanLower(from, Stat.Dex) && (from.RawDex < from.RawInt || !CanLower(from, Stat.Int)))
                                --from.RawDex;
                            else if (CanLower(from, Stat.Int))
                                --from.RawInt;
                        }

                        if (CanRaise(from, Stat.Str))
                            ++from.RawStr;

                        break;
                    }
                case Stat.Dex:
                    {
                        if (atrophy)
                        {
                            if (CanLower(from, Stat.Str) && (from.RawStr < from.RawInt || !CanLower(from, Stat.Int)))
                                --from.RawStr;
                            else if (CanLower(from, Stat.Int))
                                --from.RawInt;
                        }

                        if (CanRaise(from, Stat.Dex))
                            ++from.RawDex;

                        break;
                    }
                case Stat.Int:
                    {
                        if (atrophy)
                        {
                            if (CanLower(from, Stat.Str) && (from.RawStr < from.RawDex || !CanLower(from, Stat.Dex)))
                                --from.RawStr;
                            else if (CanLower(from, Stat.Dex))
                                --from.RawDex;
                        }

                        if (CanRaise(from, Stat.Int))
                            ++from.RawInt;

                        break;
                    }
            }
        }

        //Hourly Stat Boost *** START   ***
        private static TimeSpan m_StatGainDelay; // = TimeSpan.FromMinutes(15.0);

        public static void GainStat(Mobile from, Stat stat)
        {
            DateTime now = DateTime.Now;

            if (now.Hour == 2 || now.Hour == 3 || now.Hour == 11 || now.Hour == 14 || now.Hour == 15 || now.Hour == 16 || now.Hour == 23)
            {
                m_StatGainDelay = TimeSpan.FromMinutes(7.5);
            }
            else
            {
                m_StatGainDelay = TimeSpan.FromMinutes(15.0);
            }
            //Hourly Skill Boost *** END   ***

            switch (stat)
            {
                case Stat.Str:
                    {
                        if ((from.LastStrGain + m_StatGainDelay) >= DateTime.Now)
                            return;

                        from.LastStrGain = DateTime.Now;
                        break;
                    }
                case Stat.Dex:
                    {
                        if ((from.LastDexGain + m_StatGainDelay) >= DateTime.Now)
                            return;

                        from.LastDexGain = DateTime.Now;
                        break;
                    }
                case Stat.Int:
                    {
                        if ((from.LastIntGain + m_StatGainDelay) >= DateTime.Now)
                            return;

                        from.LastIntGain = DateTime.Now;
                        break;
                    }
            }

            bool atrophy = ((from.RawStatTotal / (double)from.StatCap) >= Utility.RandomDouble());

            IncreaseStat(from, stat, atrophy);
        }
    }
}