using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using Photon.Pun;
using UnityEngine;
using MyceliumNetworking;
using Steamworks;

namespace DontTrip
{
    public class Tripper : MonoBehaviour
    {
        public Player player { get; set; }

        float maxTimerTime = 5f;
        float TimerTime = 0f;
        Vector3 TripForce;

        float Duration;
        float Chance;
        float DamageAmount;
        bool DoesDamage;
        bool DropItem;

        const uint modId = 817816524;


        void Awake()
        {
            TripForce = Vector3.forward;
            MyceliumNetwork.RegisterLobbyDataKey("K_Duration");
            MyceliumNetwork.RegisterLobbyDataKey("K_Chance");
            MyceliumNetwork.RegisterLobbyDataKey("K_DamageAmount");
            MyceliumNetwork.RegisterLobbyDataKey("K_DoesDamage");
            MyceliumNetwork.RegisterLobbyDataKey("K_DropItem");


        }

        void Start()
        {
            MyceliumNetwork.RegisterNetworkObject(this, modId);

            LocalInit();

            if (!player.refs.view.IsMine)
            {
                return;
            }
            if (MyceliumNetwork.IsHost)
            {
                MyceliumNetwork.SetLobbyData("K_Duration", DontTrip.Duration.Value);
                MyceliumNetwork.SetLobbyData("K_Chance", DontTrip.ChanceToTrip.Value);
                MyceliumNetwork.SetLobbyData("K_DamageAmount", DontTrip.DamageAmount.Value);
                MyceliumNetwork.SetLobbyData("K_DoesDamage", DontTrip.DoesDamage.Value);
                MyceliumNetwork.SetLobbyData("K_DropItem", DontTrip.DropItem.Value);

            }
            else
            {
                Duration = MyceliumNetwork.GetLobbyData<float>("K_Duration");
                Chance = MyceliumNetwork.GetLobbyData<float>("K_Chance");
                DamageAmount = MyceliumNetwork.GetLobbyData<float>("K_DamageAmount");
                DoesDamage = MyceliumNetwork.GetLobbyData<bool>("K_DoesDamage");
                DropItem = MyceliumNetwork.GetLobbyData<bool>("K_DropItem");
            }

        }

   void LocalInit()
   {
       Duration = DontTrip.Duration.Value;
       Chance = DontTrip.ChanceToTrip.Value;
       DoesDamage = DontTrip.DoesDamage.Value;
       DropItem = DontTrip.DropItem.Value;

       if (DoesDamage)
       {
           DamageAmount = DontTrip.DamageAmount.Value;
       }
       else
       {
           DamageAmount = 0f;
       }

   }



   public void Update()
   {

       if (player.data.dead)
       {
           return;
       }
       Timer();





   }


   void Timer()
   {
       if (TimerTime >= maxTimerTime) 
       {
           if (player.data.isSprinting && player.data.fallTime <= 0f)
           {
               CheckTrip();
           }
           ResetTimer();
       }
       else
       {
           TimerTime += Time.deltaTime;
       }
   }

   void ResetTimer()
   {
       TimerTime = 0f;
   }

   void CheckTrip()
   {
       float randomNumber = Random.Range(0, 100);
       if (randomNumber < Chance)
       {
           MakeTrip();
       }
   }

   void MakeTrip()
   {
       player.CallTakeDamageAndAddForceAndFall(DamageAmount, TripForce, Duration);

       if (DropItem) player.refs.items.DropItem(player.data.selectedItemSlot);
   }


}
}
