using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Island
{
    public class PlayerStats : MonoBehaviour
    {
        private readonly float tirednessDecline = 0.15f;
        private readonly float healthDecline = 0.4f;
        private readonly float hungerDecline = 0.45f;
        private readonly float healthRestore = 3f;

        private const string I_AM_HUNGRY = "I am hungry";
        private const string I_AM_WOUNDED = "I am wounded";
        private const string I_AM_TIRED = "I am tired";

        public event EventHandler OnDied;

        [SerializeField] private int maxHealth;
        [SerializeField] private int maxTiredness;
        [SerializeField] private int maxHunger;
        [SerializeField] private PlayerSpeechBubble speechBubble;

        public int MaxHealth => maxHealth;
        public int MaxTiredness => maxTiredness;
        public int MaxHunger => maxHunger;

        public float Health { get; private set; } = 100;
        public float Tiredness { get; private set; } = 100;
        public float Hunger { get; private set; } = 100;
        public float Armor { get; private set; } = 1;
        public bool IsDead { get; private set; }

        private void Update()
        {     
            if (Tiredness > 0)
                Tiredness -= Time.deltaTime * tirednessDecline;

            if (Tiredness <= 0)
            {
                if (Health > 10)
                    GetDamage(Time.deltaTime * healthDecline);
            }
            if (Hunger > 0)
                Hunger -= Time.deltaTime * hungerDecline;

            if (Hunger <= 0)
            {
                if (Health > 10)
                    GetDamage(Time.deltaTime * healthDecline);
            }

            if (Hunger > 50 && Tiredness > 50 && Health < 100)
                Health += Time.deltaTime * healthRestore;

            if ((int)Hunger == 15 || (int)Hunger == 5)
                speechBubble.SetMessage(I_AM_HUNGRY);

            if ((int)Tiredness == 15 || (int)Tiredness == 5)
                speechBubble.SetMessage(I_AM_TIRED);

        }

        public void UpdateStats(PlayerStatsSeralizable statsSeralizable)
        {
            Health = statsSeralizable.health;
            Tiredness = statsSeralizable.tiredness;
            Hunger = statsSeralizable.hunger;
            Armor = statsSeralizable.armor;
        }

        public void UpdateArmor(Armor armor)
        {
            Armor = 1;
            if (armor.helmet?.itemSO != null)
                Armor += armor.helmet.itemSO.armor;
            if (armor.body?.itemSO != null)
                Armor += armor.body.itemSO.armor;
            if (armor.hand?.itemSO != null)
                Armor += armor.hand.itemSO.armor;
            if (armor.shoes?.itemSO != null)
                Armor += armor.shoes.itemSO.armor;
        }

        public void ChangeHunger(int eatValue)
        {
            Hunger += eatValue;
            if (Hunger > maxHunger) Hunger = maxHunger;
            if (Hunger < 0) Hunger = 0;
        }

        public void Sleep()
        {
            Tiredness = maxTiredness;
            ChangeHunger(-40);
        }

        public void GetDamage(float damage)
        {
            Health -= damage / Armor;
            InventoryManager.Instance.Armor.ReduceArmorStrength();

            if ((int)Health == 10 || (int)Health == 2)
                speechBubble.SetMessage(I_AM_WOUNDED);

            if (Health <= 0 && !IsDead)
            {
                IsDead = true;
                OnDied?.Invoke(this, EventArgs.Empty);
            }
        }

        public void RestartStatistic()
        {
            Health = maxHealth;
            Tiredness = maxTiredness;
            Hunger = maxHunger;
            IsDead = false;
        }

    }

    [System.Serializable]
    public struct PlayerStatsSeralizable
    {
        public float health;
        public float tiredness;
        public float hunger;
        public float armor;

        public PositionSerializableStruct position;

        public PlayerStatsSeralizable(PlayerController player)
        {
            health = player.PlayerStats.Health;
            tiredness = player.PlayerStats.Tiredness;
            hunger = player.PlayerStats.Hunger;
            armor = player.PlayerStats.Armor;

            position = new PositionSerializableStruct(player.transform.position);
        }
    }
}

