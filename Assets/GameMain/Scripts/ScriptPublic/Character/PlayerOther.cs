using System;
using UniRx;

namespace GamePlay
{
	public class PlayerOther : Player
	{

		public override void OnShowCard()
		{
			if (state == EPlayerState.End)
			{
				foreach (var card in handCards)
				{
					UnityEngine.Object.Destroy(card.gameObject);
				}

				foreach (var cardData in handCardsData)
				{
					GetCard(cardData);
				}
				
				foreach (var card in handCards)
				{
					card.gameObject.SetActive(true);
					card.SetFrontActive(true);
				}
			}
		}
	}
}
