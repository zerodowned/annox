using System;
using System.Collections;
using Server;
using Server.Gumps;
using Server.Commands;

namespace Arya.Auction
{
	/// <summary>
	/// Staff control gump
	/// </summary>
	public class AuctionControlGump : Gump
	{
		private const int LabelHue = 0x480;
		private const int GreenHue = 0x40;
		private const int RedHue = 0x20;
		private ArrayList m_Buttons;

		Mobile m_User;
		AuctionItem m_Auction;

		public AuctionControlGump( Mobile user, AuctionItem auction, AuctionViewGump view ) : base( 50, 50 )
		{
			m_User = user;
			m_Auction = auction;

			m_User.CloseGump( typeof( AuctionControlGump ) );

			view.X = 400;
			m_User.SendGump( view );

			MakeGump();
		}

		private void MakeGump()
		{
			m_Buttons = new ArrayList();

			this.Closable=true;
			m_Buttons.Add( 0 );

			this.Disposable=true;
			this.Dragable=true;
			this.Resizable=false;
			this.AddPage(0);
			this.AddBackground(0, 0, 300, 385, 9270);
			this.AddAlphaRegion(10, 10, 280, 365);

			this.AddLabel(99, 15, GreenHue, AuctionSystem.ST[ 213 ] );
			this.AddImageTiled(15, 35, 270, 1, 9304);
			this.AddImageTiled(30, 55, 1, 65, 9304);

			// Auction owner
			bool owner = m_Auction.Owner != null;
			this.AddLabel(15, 35, RedHue, string.Format( AuctionSystem.ST[ 216 ], owner ? m_Auction.Owner.Name : AuctionSystem.ST[ 78 ] ));

			// Owner Props : 1
			if ( owner )
			{
				this.AddButton(35, 58, 9702, 9703, 1, GumpButtonType.Reply, 0);
				m_Buttons.Add( 1 );
			}
			this.AddLabel(55, 55, LabelHue, AuctionSystem.ST[ 214 ] );;
			
			// Owner Account: 2
			if ( owner && m_Auction.Owner.Account != null && m_User.AccessLevel == AccessLevel.Administrator )
			{
				this.AddButton(35, 78, 9702, 9703, 2, GumpButtonType.Reply, 0);
				m_Buttons.Add( 2 );
			}
			this.AddLabel(55, 75, LabelHue, string.Format( AuctionSystem.ST[ 215 ], ( owner && m_Auction.Owner.Account != null ) ? ( m_Auction.Owner.Account as Server.Accounting.Account ).Username : AuctionSystem.ST[ 78 ] ) );

			bool online = m_Auction.Owner != null && m_Auction.Owner.NetState != null;
			
			// Client button : 3
			if ( online )
			{
				this.AddButton(35, 98, 9702, 9703, 3, GumpButtonType.Reply, 0);
				m_Buttons.Add( 3 );
			}
			this.AddLabel(55, 95, online ? GreenHue : RedHue, online ? AuctionSystem.ST[ 217 ] : AuctionSystem.ST[ 218 ] );

			this.AddImageTiled(15, 120, 270, 1, 9304);
			this.AddLabel(20, 125, RedHue, AuctionSystem.ST[ 219 ]);
			this.AddImageTiled(30, 145, 1, 65, 9304);

			// Item Properties : 4
			this.AddButton(35, 150, 9702, 9703, 4, GumpButtonType.Reply, 0);
			m_Buttons.Add( 4 );
			this.AddLabel(55, 147, LabelHue, AuctionSystem.ST[ 214 ]);

			bool item = m_Auction.Item != null;

			// Get item : 5
			this.AddButton(35, 170, 9702, 9703, 5, GumpButtonType.Reply, 0);
			m_Buttons.Add( 5 );
			this.AddLabel(55, 167, LabelHue, AuctionSystem.ST[ 220 ] );
            
			// Return item: 6
			this.AddButton(35, 190, 9702, 9703, 6, GumpButtonType.Reply, 0);
			m_Buttons.Add( 6 );
			this.AddLabel(55, 187, LabelHue, AuctionSystem.ST[ 221 ] );

			this.AddImageTiled(15, 210, 270, 1, 9304);
			this.AddLabel(20, 215, RedHue, AuctionSystem.ST[ 222 ] );
			this.AddImageTiled(30, 235, 1, 110, 9304);

			// Auction Properties: 7
			this.AddButton(35, 240, 9702, 9703, 7, GumpButtonType.Reply, 0);
			m_Buttons.Add( 7 );
			this.AddLabel(55, 237, LabelHue, AuctionSystem.ST[ 214 ] );

			// End auction now: 8
			this.AddButton(35, 260, 9702, 9703, 8, GumpButtonType.Reply, 0);
			m_Buttons.Add( 8 );
			this.AddLabel(55, 257, LabelHue, AuctionSystem.ST[ 223 ] );

			// Close and return: 9
			this.AddButton(35, 280, 9702, 9703, 9, GumpButtonType.Reply, 0);
			m_Buttons.Add( 9 );
			this.AddLabel(55, 277, LabelHue, AuctionSystem.ST[ 224 ] );
			
			// Close and get: 10
			this.AddButton(35, 300, 9702, 9703, 10, GumpButtonType.Reply, 0);
			m_Buttons.Add( 10 );
			this.AddLabel(55, 297, LabelHue, AuctionSystem.ST[ 225 ] );

			// Close and delete: 11
			this.AddButton(35, 320, 9702, 9703, 11, GumpButtonType.Reply, 0);
			m_Buttons.Add( 11 );
			this.AddLabel(55, 317, LabelHue, AuctionSystem.ST[ 226 ] );

			this.AddImageTiled(15, 345, 270, 1, 9304);

			// Exit: 0
			this.AddButton(15, 350, 9702, 9703, 0, GumpButtonType.Reply, 0);
			m_Buttons.Add( 0 );
			this.AddLabel(35, 348, LabelHue, AuctionSystem.ST[ 14 ] );
		}

		public override void OnResponse(Server.Network.NetState sender, RelayInfo info)
		{
			if ( ! m_Buttons.Contains( info.ButtonID ) )
			{
				Console.WriteLine( @"The auction system located a potential exploit. 
					Player {0} (Acc. {1}) tried to press an unregistered button in a gump of type: {2}",
					sender.Mobile != null ? sender.Mobile.ToString() : "Unkown",
					sender.Mobile != null && sender.Mobile.Account != null ? ( sender.Mobile.Account as Server.Accounting.Account ).Username : "Unkown",
					this.GetType().Name );

				return;
			}

			switch ( info.ButtonID )
			{
				case 0: // Exit
					break;

				case 1: // Owner Props

					m_User.SendGump( this );
					m_User.SendGump( new PropertiesGump( m_User, m_Auction.Owner ) );
					break;

				case 2: // Owner Account

					m_User.SendGump( this );
					m_User.SendGump( new AdminGump( m_User, AdminGumpPage.AccountDetails_Information, 0, null, "Request from the auction system", m_Auction.Account ) );
					break;

				case 3: // Owner Client

					m_User.SendGump( this );

					if ( m_Auction.Owner.NetState != null )
					{
						m_User.SendGump( new ClientGump( m_User, m_Auction.Owner.NetState ) );
					}
					break;

				case 4: // Item Props

					m_User.SendGump( this );
					m_User.SendGump( new PropertiesGump( m_User, m_Auction.Item ) );
					break;

				case 5: // Get Item

					m_User.SendGump( this );

					if ( m_Auction.Creature && m_Auction.Pet != null )
					{
						m_Auction.Pet.MoveToWorld( m_User.Location, m_User.Map );
					}
					else
					{
						m_User.Backpack.AddItem( m_Auction.Item );
					}
					
					AuctionLog.WriteViewItem( m_Auction, m_User );

					break;

				case 6: // Return Item

					if ( m_Auction.Creature )
					{
						m_Auction.Pet.SetControlMaster( null );
						m_Auction.Pet.ControlOrder = Server.Mobiles.OrderType.Stay;
						m_Auction.Pet.Internalize();
					}
					else
					{
						Item item = m_Auction.Item;

						if ( item != null )
						{
							if ( item.Parent is Mobile)
							{
								( (Mobile) item.Parent ).RemoveItem( item );
							}
							else if ( item.Parent is Item)
							{
								( (Item) item.Parent ).RemoveItem( item );
							}
						}

						AuctionSystem.ControlStone.Items.Add( m_Auction.Item );
						m_Auction.Item.Parent = AuctionSystem.ControlStone;
					}

					AuctionLog.WriteReturnItem( m_Auction, m_User );

					m_User.SendGump( this );

					break;

				case 7: // Auction Props

					m_User.SendGump( this );
					m_User.SendGump( new PropertiesGump( m_User, m_Auction ) );
					break;

				case 8: // End Auction

					m_Auction.End( m_User );
					goto case 0;

				case 9: // End and Return

					m_Auction.StaffDelete( m_User, ItemFate.ReturnToOwner );
					goto case 0;

				case 10: // End and Get
					
					m_Auction.StaffDelete( m_User, ItemFate.ReturnToStaff );
					goto case 0;

				case 11: // End and Delete
					
					m_Auction.StaffDelete( m_User, ItemFate.Delete );
					goto case 0;
			}
		}
	}
}