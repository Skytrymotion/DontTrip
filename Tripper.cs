using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using UnityEngine;

namespace DontTrip
{
    public class Tripper : MonoBehaviour
    {
        public Player player { get; set; }

        float maxTimerTime = 5f;
        float TimerTime = 0f;
        


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
            player.data.fallTime = DontTrip.Duration.Value;
            if (DontTrip.DoesDamage.Value)
            {
                TakeDamage();
            }
        }

        void TakeDamage()
        {
            if (player.ai)
            {
                return;
            }
            if (player.data.dead)
            {
                return;
            }
            if (!player.refs.view.IsMine)
            {
                return;
            }
            player.data.health -= DontTrip.DamageAmount.Value;
            TakeDamagePost.instance.TakeDamageFeedback();
            UI_Feedback.instance.TakeDamage(false);

            if (player.data.health <= 0f)
            {
                player.Die();
            }
        }
    }
}
