using System; 
using Server; 
using Server.Items; 

namespace Server.Mobiles 
{ 
   [CorpseName( "a ridable horn corpse" )] 
   public class RidableDreadHorn : BaseMount 
   { 
      [Constructable] 
      public RidableDreadHorn() : this( "a Ridable Dread Horn" ) 
      { 
      } 

      [Constructable] 
      public RidableDreadHorn( string name ) : base( name, 257, 0x3f09, AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 ) 
      { 
         Body = 257; 
         BaseSoundID = 0xA3; 

         SetStr( 116, 140 ); 
         SetDex( 81, 105 ); 
         SetInt( 26, 50 ); 

         SetHits( 70, 84 ); 
         SetMana( 0 ); 

         SetDamage( 7, 12 ); 

         SetDamageType( ResistanceType.Physical, 100 ); 

         SetResistance( ResistanceType.Physical, 25, 35 ); 
         SetResistance( ResistanceType.Cold, 60, 80 ); 
         SetResistance( ResistanceType.Poison, 15, 25 ); 
         SetResistance( ResistanceType.Energy, 10, 15 ); 

         SetSkill( SkillName.MagicResist, 45.1, 60.0 ); 
         SetSkill( SkillName.Tactics, 60.1, 90.0 ); 
         SetSkill( SkillName.Wrestling, 45.1, 70.0 ); 

         Fame = 1500; 
         Karma = 0; 

         VirtualArmor = 18; 

         Tamable = true; 
         ControlSlots = 1; 
         MinTameSkill = 110.3; 
      } 

      public override int Meat{ get{ return 2; } } 
      public override int Hides{ get{ return 16; } } 
      public override FoodType FavoriteFood{ get{ return FoodType.Fish | FoodType.FruitsAndVegies | FoodType.Meat; } } 
      public override PackInstinct PackInstinct{ get{ return PackInstinct.Bear; } } 

      public RidableDreadHorn( Serial serial ) : base( serial ) 
      { 
      } 

      public override void Serialize( GenericWriter writer ) 
      { 
         base.Serialize( writer ); 

         writer.Write( (int) 0 ); 
      } 

      public override void Deserialize( GenericReader reader ) 
      { 
         base.Deserialize( reader ); 

         int version = reader.ReadInt(); 
      } 
   } 
}