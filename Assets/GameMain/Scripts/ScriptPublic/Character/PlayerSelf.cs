using System;
using UniRx;

namespace GamePlay
{
	
	public class PlayerSelf : Player
	{
		public override void OnSeat()
		{
			base.OnSeat();
			
			tableUI.BtnSeat.interactable = false;
		}
		public override void OnShowCard()
		{
		   ShowCard();
		
		}
	}
}

