using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using UnityEngine;
using static UnityEngine.ParticleSystem.PlaybackState;

namespace DontTrip
{
    public class Tripper : MonoBehaviour
    {
        public Player player { get; set; }

        float maxTimerTime = 5f;
        float TimerTime = 0f;
        Vector3 TripForce;
        


        void Awake()
        {
            TripForce = Vector3.forward;
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
