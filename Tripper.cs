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

            LocalInit();

            if (!player.refs.view.IsMine)
            {
                return;
            }
            if (PhotonNetwork.IsMasterClient)
            {
                MyceliumNetwork.RPC(modId, nameof(Init), ReliableType.Reliable, DontTrip.Duration.Value.ToString(), DontTrip.ChanceToTrip.Value.ToString(), DontTrip.DoesDamage.Value.ToString(), DontTrip.DamageAmount.Value.ToString());
            }

        }

        [CustomRPC]
        public void Init(string du, string ch, string doe, string da)
        {
            Duration = float.Parse(du);
            Chance = float.Parse(ch);
            DoesDamage = bool.Parse(doe);

            if (DoesDamage)
            {
                DamageAmount = float.Parse(da);
            }
            else
            {
                DamageAmount = 0f;
            }

        }

        void LocalInit()
        {
            Duration = DontTrip.Duration.Value;
            Chance = DontTrip.ChanceToTrip.Value;
            DoesDamage = DontTrip.DoesDamage.Value;

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

        }


    }
}
