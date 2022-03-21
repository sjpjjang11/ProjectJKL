using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectK
{
	public class Layer
	{
		/*public static int CollisionLayerMask(csBattleManager.CollisionLayer _layer)
		{
			string MyLayerMask = _layer.ToString();

			int CollisionLayerMask = 0;

			switch (MyLayerMask)
			{
				case csBattleManager.m_strPlayerLayer:

					CollisionLayerMask = (1 << LayerMask.NameToLayer(csBattleManager.CollisionLayer.Monster.ToString())
						| (1 << LayerMask.NameToLayer(csBattleManager.CollisionLayer.Map.ToString()))
						| (1 << LayerMask.NameToLayer(csBattleManager.CollisionLayer.Floor.ToString())));

					break;

				case csBattleManager.m_strBotLayer:

					CollisionLayerMask = (1 << LayerMask.NameToLayer(csBattleManager.CollisionLayer.Player.ToString())
						| (1 << LayerMask.NameToLayer(csBattleManager.CollisionLayer.Map.ToString()))
						| (1 << LayerMask.NameToLayer(csBattleManager.CollisionLayer.Floor.ToString())));

					break;
			}

			/*CollisionLayerMask = (1 << LayerMask.NameToLayer(csBattleManager.CollisionLayer.Hero.ToString())
						| (1 << LayerMask.NameToLayer(csBattleManager.CollisionLayer.Map.ToString())));

			return CollisionLayerMask;
		}*/

		public static int CollisionLayerMask(eCollisionLayerType _layer)
		{
			int CollisionLayerMask = 0;

			CollisionLayerMask = 1 << LayerMask.NameToLayer(_layer.ToString());

			return CollisionLayerMask;
		}

		public static int CollisionLayerMask(eCollisionLayerType _layerA, eCollisionLayerType _layerB)
		{
			int CollisionLayerMask = 0;

			CollisionLayerMask = (1 << LayerMask.NameToLayer(_layerA.ToString())
						| (1 << LayerMask.NameToLayer(_layerB.ToString())));

			return CollisionLayerMask;
		}

		public static int CollisionLayerMask(eCollisionLayerType _layerA, eCollisionLayerType _layerB, eCollisionLayerType _layerC)
		{
			int CollisionLayerMask = 0;

			CollisionLayerMask = (1 << LayerMask.NameToLayer(_layerA.ToString())
						| (1 << LayerMask.NameToLayer(_layerB.ToString()))
						| (1 << LayerMask.NameToLayer(_layerC.ToString())));

			return CollisionLayerMask;
		}
	}	
}
