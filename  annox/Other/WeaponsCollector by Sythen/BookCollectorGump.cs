using System;
using Server;
using Server.Commands;
using Server.Commands.Generic;
using Server.Gumps;
using Server.Network;
using Server.Items;
using Server.Mobiles;

namespace Server.Gumps
{

    public class BookCollectorGump : Gump
    {
        public static void Initialize()
        {

            CommandSystem.Register("BookCollectorGump", AccessLevel.GameMaster, new CommandEventHandler(BookCollectorGump_OnCommand));

        }

        private static void BookCollectorGump_OnCommand(CommandEventArgs e)
        {

            e.Mobile.SendGump(new BookCollectorGump(e.Mobile));

        }

        public BookCollectorGump(Mobile owner)
            : base(50, 50)
        {

            //----------------------------------------------------------------------------------------------------//

            AddPage(0);

            AddImageTiled(54, 33, 369, 400, 2624);

            AddAlphaRegion(54, 33, 369, 400);

            AddImageTiled(416, 39, 44, 389, 203);

            //--------------------------------------Window size bar-----------------------------------------------//

            AddImage(97, 49, 9005);

            AddImageTiled(58, 39, 29, 390, 10460);

            AddImageTiled(412, 37, 31, 389, 10460);

            AddLabel(140, 60, 0x34, "Ancient Book Trade");

            AddHtml(107, 140, 300, 230, "<BODY>" +

//----------------------------------------------------------------------------------------------------//

"<BASEFONT COLOR=YELLOW>I am a Book Collector! Would you like to sell me your book? <BR><BR>If you have any you'd like to sell, please hand me them to me and I will give you some gold in return." + "</BODY>", false, true);

            //----------------------------------------------------------------------------------------------------//

            AddImage(430, 9, 10441);

            AddImageTiled(40, 38, 17, 391, 9263);

            AddImage(6, 25, 10421);

            AddImage(34, 12, 10420);

            AddImageTiled(94, 25, 342, 15, 10304);

            AddImageTiled(40, 427, 415, 16, 10304);

            AddImage(-10, 314, 10402);

            AddImage(56, 150, 10411);

            AddImage(136, 84, 96);

            AddButton(225, 390, 0xF7, 0xF8, 0, GumpButtonType.Reply, 0);

            //----------------------------------------------------------------------------------------------------//

        }

        public override void OnResponse(NetState state, RelayInfo info) //Function for GumpButtonType.Reply Buttons 
        {
            Mobile from = state.Mobile;

            switch (info.ButtonID)
            {
                case 0: //Case uses the ActionIDs defenied above. Case 0 defenies the actions for the button with the action id 0 
                    {

                        from.SendMessage("Please return to any Book Collector, should you wish to sell more book to us.");
                        break;

                    }

            }
        }
    }
}