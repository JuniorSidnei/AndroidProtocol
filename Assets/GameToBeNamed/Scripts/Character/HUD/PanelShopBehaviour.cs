using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameToBeNamed.Character {

    public class PanelShopBehaviour : BaseUIBehavior {
        //todo aqui vão estar as funções de mostar o painel da loja e os items e ativar os eventos de compra e tal

        
        public  override void HandlePlayingMode() {
            base.HandlePlayingMode();
        }
        
        public  override void HandleConversationMode() {
            base.HandleConversationMode();
        }
        
        public  override void HandleShopMode() {
            base.HandleShopMode();
        }
        
        public  override void HandleCinematicMode() {
            base.HandleCinematicMode();
        }
        

        //active hud shop message and erevything else
        public void SetShop() {
            
        }
    }
}