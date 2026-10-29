
using System;


class Player
{
 private int health;

public int Health
{
    get { return health; }
}
    public string Name { get; }

    public Player(string playerName, int startingHealth)
    {

        if (startingHealth < 0)
    {
        startingHealth = 0;
    } 

        Name = playerName;
        health = startingHealth;
    }

    public virtual void Introduce()
    {
        Console.WriteLine("I am " + Name);
    }

    public void ShowHealth()
    {
        Console.WriteLine("Health: " + health);
    }

    public void TakeDamage(int damage)
    {
        if (damage < 0)
        {
            Console.WriteLine("Damage cannot be negative.");
            return;
        }

        health = health - damage;

        if (health < 0)
        {
            health = 0;
        }
    }
}


class Avengers : Player
{
    public Avengers(string playerName, int startingHealth)
        : base(playerName, startingHealth)
    {
    }

    public override void Introduce()
    {
        Console.WriteLine("I am the best Avenger named " + Name);
    }

    public void Fight(Enemy p1,int attack)
    {
        if (attack <= 0)
        {
            Console.WriteLine("The player blocks it");
            return;
        }

        p1.TakeDamage(attack);
        Console.WriteLine("Thanos was hurt by " + attack);
    }
}

class Enemy : Player
{
    public Enemy(string enemyName,int startingHealth)
    : base(enemyName, startingHealth)
    {
    }

    public override void Introduce()
    {
        Console.WriteLine("I will kill you in the name of " + Name);
    }

    public void Givedamage(Avengers hero ,int damage)
    {
      if(damage <= 0)
        {
            Console.WriteLine("The hero blocks the damage");
            return;
        }

      hero.TakeDamage(damage);
      Console.WriteLine("The hero took " + damage + " damage.");
}

}





class Program
{
    
static void Main()
{
    Player player = new Player("Vedant", 100);
    Avengers hero = new Avengers ("Rakhi Sawant", 120);
    Enemy p1 = new Enemy("Thanos",150);

    Console.WriteLine("--- Normal Player ---");
    player.Introduce();
    player.ShowHealth();

    Console.WriteLine("--- Avenger ---");
    hero.Introduce();
    hero.ShowHealth();

    

    Console.WriteLine("------Enemy------");
    p1.Introduce();
    p1.ShowHealth();

   


       Console.WriteLine("----Fight Begins----");
        
       while(hero.Health>0 && p1.Health > 0)
        {
           hero.Fight(p1,70);
           if(p1.Health>0){
           p1.Givedamage(hero,80);
           }
            
        }
        p1.ShowHealth();
        hero.ShowHealth();

        if (hero.Health <= 0)
        {
            Console.WriteLine("Thanos end the world in his million possibilities");
        }
        else
        {
            Console.WriteLine("The 1 Future comes true");
        }

}
}




