using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Lab3.Program;

namespace Lab3
{
    [Serializable]
    internal class Creature
    {
        public string Name { get; set; }
        public int Health { get; set; }
        public int Attack { get; set; }
        public int Evasion { get; set; } // Новое свойство для шанса уклонения

        public CreatureTeam Team { get; set; }

        public Creature(string name, int health, int attack, int evasion)
        {
            Name = name;
            Health = health;
            Attack = attack;
            Evasion = evasion; // Инициализируем шанс уклонения

        }

        public virtual void AttackCreature(Creature target)
        {

            // Проверяем шанс уклонения
            Random random = new Random();
            if (random.Next(1, 101) <= target.Evasion)
            {

                if (target is Rogue)
                {
                    // Если атакуемое существо разбойник, то разбойник восстанавливает здоровье
                    target.Health += 25;
                    Console.WriteLine($"{Name} атакует {target.Name}, но {target.Name} уклонился и восстановил {"25"} единиц здоровья!");
                    Console.WriteLine($"У {target.Name} теперь {target.Health} единиц здоровья!");
                    return;
                }


                Console.WriteLine($"{target.Name} уклонился от атаки {Name}!");
                return; // Прекращаем выполнение атаки

            }

            if (target is Warrior)
            {
                // Если атакуемое существо воин, то атакующий получает урон
                target.Health -= Attack - 10 ;
                Console.WriteLine($"{Name} атакует {target.Name}!");
                Console.WriteLine($"{target.Name} используеь Щит! Урон уменьшен на 10 единиц");
                Console.WriteLine($"{target.Name} потерял {Attack - 10} единиц здоровья.");

                if (target.Health > 0)
                {
                    Console.WriteLine($"У {target.Name} осталось {target.Health} единиц здоровья!");
                }

                return;
            }
            // Атака успешна, наносим урон
            target.Health -= Attack;
            Console.WriteLine($"{Name} атакует {target.Name}!");
            Console.WriteLine($"{target.Name} потерял {Attack} единиц здоровья.");

            if (target.Health > 0 ){
                Console.WriteLine($"У {target.Name} осталось {target.Health} единиц здоровья!");
            }
        }

        public virtual void PassiveAbility()
        {
            // Реализация пассивной способности по умолчанию
            Console.WriteLine($"{Name} использует пассивную способность!");
        }
    }
}
