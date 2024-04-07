using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using Photon.Pun;
using UnityEngine;
using MyceliumNetworking;

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
        bool DoesDamage;
        float DamageAmount;

        const uint modId = 817816524;


        void Awake()
        {
            TripForce = Vector3.forward;
            
        }

        void Start()
        {
            MyceliumNetwork.RegisterNetworkObject(this, modId);

            if (!player.refs.view.IsMine)
            {
                return;
            }
            if (PhotonNetwork.IsMasterClient)
            {
                MyceliumNetwork.RPC(modId, nameof(Init), ReliableType.Reliable, DontTrip.Duration.Value, DontTrip.ChanceToTrip.Value, DontTrip.DoesDamage.Value, DontTrip.DamageAmount.Value);
            }
        }

        [CustomRPC]
        public void Init(float du, float ch, bool doe, float da)
        {
            Duration = du;
            Chance = ch;
            DoesDamage = doe;
            DamageAmount = da;
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
            if (randomNumber < DontTrip.ChanceToTrip.Value)
            {
                MakeTrip();
            }
        }
         
        void MakeTrip()
        {

            float Damage = DontTrip.DoesDamage.Value ? DontTrip.DamageAmount.Value : 0f;

            player.CallTakeDamageAndAddForceAndFall(Damage, TripForce, DontTrip.Duration.Value);

        }


    }
}
