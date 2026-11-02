using System;
using System.Collections.Generic;

namespace GameDevModule1
{
    // Base Class: Could be a character in an FPS or a Unit/Card on a board!
    public class Entity
    {
        public string Name { get; private set; }
        public int Health { get; protected set; }

        // Constructor: runs automatically when we spawn a new Entity
        public Entity(string name, int startingHealth)
        {
            Name = name;
            Health = startingHealth;
        }

        // 'virtual' means child classes are allowed to customize this behavior!
        public virtual void PerformAction(Entity target)
        {
            Console.WriteLine($"{Name} stares menacingly at {target.Name}.");
        }

        public void TakeDamage(int dmg)
        {
            Health = Math.Max(0, Health - dmg);
            Console.WriteLine($"  -> {Name} took {dmg} damage! Remaining HP: {Health}");
        }
    }

    // Child Class 1: Inherits from Entity using ':'
    public class FpsSoldier : Entity
    {
        public int Ammo { get; private set; }

        public FpsSoldier(string name, int health, int ammo) : base(name, health)
        {
            Ammo = ammo;
        }

        // 'override' replaces the base PerformAction with FPS shooting logic!
        public override void PerformAction(Entity target)
        {
            if (Ammo > 0)
            {
                Ammo--;
                Console.WriteLine($"[FPS] {Name} fires a burst at {target.Name}! (Ammo left: {Ammo})");
                target.TakeDamage(25);
            }
            else
            {
                Console.WriteLine($"[FPS] *Click!* {Name} is out of ammo and reloads!");
                Ammo = 5;
            }
        }
    }

    // Child Class 2: A Card Mage who casts spells using Mana
    public class CardSummoner : Entity
    {
        public int Mana { get; private set; }

        public CardSummoner(string name, int health, int mana) : base(name, health)
        {
            Mana = mana;
        }

        public override void PerformAction(Entity target)
        {
            if (Mana >= 3)
            {
                Mana -= 3;
                Console.WriteLine($"[CARD] {Name} plays 'Arcane Bolt' (Cost: 3 Mana) on {target.Name}!");
                target.TakeDamage(40);
            }
            else
            {
                Console.WriteLine($"[CARD] {Name} draws a card and channels +2 Mana.");
                Mana += 2;
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // Polymorphism in action: a single List<Entity> holds different child classes!
            List<Entity> arena = new List<Entity>
            {
                new FpsSoldier("Vanguard-7", 100, 2),
                new CardSummoner("Archmage Lyra", 80, 5)
            };
            Console.WriteLine("Vanguard has 100 health and 2 ammo.");
            Console.WriteLine("Archmage Lyra has 80 health and 5 mana");
            
            for(int i=1; i<=3; i++){
            Console.WriteLine($"=== ROUND {i} FIGHT ===");
            arena[0].PerformAction(arena[1]);
            arena[1].PerformAction(arena[0]);

        }
    }
}
}