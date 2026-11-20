List<Character> characters = new List<Character>();
characters.Add(new Hero("Spiderman",100,30));
characters.Add(new Hero("Ironman",120,25));
characters.Add(new Enemy("Orc",80,10));
characters.Add(new Enemy("Goblin",60,15));
characters.Add(new Enemy("Zombie",50,15));
Boss boss = new Boss("Thanos",200,40);
List<Hero> heroes = new List<Hero>();
List<Enemy> enemies = new List<Enemy>();
heroes.Add(new Hero("Spiderman",100,30));
heroes.Add(new Hero("Ironman",120,25));

enemies.Add(new Enemy("Orc",80,10));
enemies.Add(new Enemy("Goblin",60,15));
enemies.Add(new Enemy("Zombie",50,15));
Console.WriteLine("All Character are: ");
foreach(Character character in characters)
{
    Console.WriteLine(character.GetName());
}
Console.WriteLine("All Heroes are: ");
foreach(Hero hero in heroes)
{
    Console.WriteLine(hero.GetName());
}
Console.WriteLine("All Enemies are: ");
foreach(Enemy enemy in enemies)
{
    Console.WriteLine(enemy.GetName());
}

int RemainingHeroes=heroes.Count;
int RemainingEnemies=enemies.Count;
int round =1;
while(RemainingHeroes > 0 && RemainingEnemies > 0)
{
    Console.WriteLine("============ Round " + round + " ============ ");
    round++;
    RemainingHeroes=0;
    RemainingEnemies=0;
    foreach(Hero hero in heroes)
    {
        if (hero.GetHealth() > 0)
        {
            if(enemies[0].GetHealth() > 0)
            {
                hero.Attack(enemies[0]);
            }
        }
    }
    foreach(Enemy enemy in enemies)
    {
        if(enemy.GetHealth() > 0)
        {
            if(heroes[0].GetHealth() > 0)
            {
                enemy.Attack(heroes[0]);
            }
        }
    }
    foreach(Hero hero in heroes)
    {
        if (hero.GetHealth() > 0)
        {
            if(enemies[1].GetHealth() > 0)
            {
                hero.Attack(enemies[1]);
            }
        }
    }
    foreach(Enemy enemy in enemies)
    {
        if(enemy.GetHealth() > 0)
        {
            if(heroes[1].GetHealth() > 0)
            {
                enemy.Attack(heroes[1]);
            }
        }
    }
    foreach(Hero hero in heroes)
    {
        if (hero.GetHealth() > 0)
        {
            if(enemies[2].GetHealth() > 0)
            {
                hero.Attack(enemies[2]);
            }
        }
    }
    foreach(Hero hero in heroes)
    {
        if(hero.GetHealth() > 0)
        {
            RemainingHeroes++;
        }
    }
    foreach(Enemy enemy in enemies)
    {
        if(enemy.GetHealth() > 0)
        {
            RemainingEnemies++;
        }
    }
    Console.WriteLine("Remaining Heroes: "+ RemainingHeroes);
    Console.WriteLine("Remaining enemies: "+ RemainingEnemies);
}
if(RemainingHeroes > 0)
{
Console.WriteLine("Boss Appears!");
Console.WriteLine("=============== Boss Fight ==============");
}
while(RemainingHeroes>0 && boss.GetHealth()>0)
    {
    if(RemainingEnemies == 0)
    {
    foreach(Hero hero in heroes)
    {
        if (hero.GetHealth() > 0)
        {
            if(boss.GetHealth() > 0)
            {
                hero.Attack(boss);
            }
        }
    }
    if(heroes[0].GetHealth()>0 && boss.GetHealth()>0)
    {
        boss.Attack(heroes[0]);
    }
    if(heroes[1].GetHealth()>0 && boss.GetHealth()>0)
    {
        boss.Attack(heroes[1]);
    }
    }
    RemainingHeroes=0;
    foreach(Hero hero in heroes)
    {
        if(hero.GetHealth() > 0)
        {
            RemainingHeroes++;
        }
    }
}
if (RemainingHeroes > 0)
{
    Console.WriteLine("Heroes won!");
}
else
{
   Console.WriteLine("Boss won!"); 
}
class Character
{
    protected string Name="";
    protected int Health;
    protected int Damage;
    public string GetName()
    {
    return Name;
    }   

    public int GetHealth()
    {
    return Health;
    }
    public int GetDamage()
    {
    return Damage;
    }
    public Character( string name , int health , int damage)
    {
        Name=name;
        Health=health;
        Damage=damage;
        
    }
    public void TakeDamage(int damage)
    {
        Health=Health-damage;
        if (Health < 0)
        {
            Health=0;
        }
        Console.WriteLine(Name +" Health: "+ Health);
        if (Health <= 0)
        {
            Console.WriteLine(Name + " Has been defeated!");
        }
    }
    public virtual void Attack(Character Target)
    {
        Console.WriteLine(Name + " Attacks " + Target.Name);
        Target.TakeDamage(Damage);
    }
    
    
}
class Hero : Character
{
    public Hero(string name , int health , int damage)
        : base(name, health , damage)
    {    
    }
    public void Heal(int amount)
    {
        Health=Health+amount;
        Console.WriteLine(Name + " Healed!");
        Console.WriteLine("Health: "+ Health);
    }
    public override void Attack(Character target)
    {
    Console.WriteLine(Name + " uses Heat Blast");

    target.TakeDamage(Damage*2);
    }
}
class Enemy : Character
{
    public Enemy(string name , int health , int damage)
        : base(name, health ,  damage)
    {    
    }
    public override void Attack(Character target)
    {
    Console.WriteLine(Name + " uses Sword Slash! ");

    target.TakeDamage(Damage);
    }
}
class Boss : Character
{
    public Boss(string name , int health , int damage)
        :base(name,health , damage)
    {
    }
    public override void Attack(Character target)
    {
    Console.WriteLine(Name + " uses Infinity Blast!");

    target.TakeDamage(Damage * 2);
    }
}