using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Lab3.Program;

namespace Lab3
{
    [Serializable]
    internal class Mage : Creature
    {
        public int Mana { get; set; }
        private int turnsSinceLastActiveAbility = 2; // Добавляем счетчик ходов с момента последнего использования активной способности

        public Mage(string name, int health, int attack, int mana, int evasion, CreatureTeam team) : base(name, health, attack, evasion)
        {
            Mana = mana;
            Team = team;
        }

        public override void AttackCreature(Creature target)
        {
            Console.WriteLine($"Выберите действие для {Name}:");
            Console.WriteLine("1. Обычная атака");

            // Дополнительная проверка для определения доступности активной способности
            if (this is Mage && turnsSinceLastActiveAbility >= 1)
                Console.WriteLine("2. Использовать активную способность: Огненный шар!");
            else if (!(this is Mage) && turnsSinceLastActiveAbility >= 2)
                Console.WriteLine("2. Использовать активную способность: Огненный шар!");
            else
                Console.WriteLine("2. Использовать активную способность: Огненный шар! (не готов)");

            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    // Вызываем пассивную способность перед атакой
                    PassiveAbility();
                    // Вызываем метод обычной атаки
                    base.AttackCreature(target);
                    break;
                case "2":
                    // Проверяем, прошло ли уже один ход для мага или два для остальных с момента последнего использования активной способности
                    if ((this is Mage && turnsSinceLastActiveAbility >= 1) || (!(this is Mage) && turnsSinceLastActiveAbility >= 2))
                    {
                        // Вызываем метод активной способности
                        ActiveAbility(target);
                        turnsSinceLastActiveAbility = 0; // Сбрасываем счетчик ходов
                    }
                    else
                    {
                        Console.WriteLine("Вы не можете использовать активную способность. Прошло недостаточно времени с момента предыдущего использования.");
                        // Вызываем пассивную способность перед атакой
                        PassiveAbility();
                        // Вызываем метод обычной атаки
                        base.AttackCreature(target);
                    }
                    break;
                default:
                    Console.WriteLine("Некорректный ввод. Будет выполнена обычная атака.");
                    // Вызываем пассивную способность перед атакой
                    PassiveAbility();
                    // Вызываем метод обычной атаки
                    base.AttackCreature(target);
                    break;
            }
        }


        public void ActiveAbility(Creature target)
        {
            if (target is Warrior)
            {
                // Переопределяем метод активной способности для мага
                Console.WriteLine($"{Name} использует активную способность: Огненный шар!");
                // Допустим, активная способность мага наносит дополнительный урон
                target.Health -= Attack;
                Console.WriteLine($"{Name} наносит дополнительный урон {target.Name} с помощью Огненного шара!");
                Console.WriteLine($"{target.Name} используеь Щит! Урон уменьшен на 10 единиц");
                Console.WriteLine($"{target.Name} потерял {Attack} единиц здоровья.");

                if (target.Health > 0)
                {
                    Console.WriteLine($"У {target.Name} осталось {target.Health} единиц здоровья!");
                }

                // Увеличиваем счетчик ходов с момента последнего использования активной способности
                turnsSinceLastActiveAbility++;
                return;
            }

            // Переопределяем метод активной способности для мага
            Console.WriteLine($"{Name} использует активную способность: Огненный шар!");
            // Допустим, активная способность мага наносит дополнительный урон
            target.Health -= Attack + 10;
            Console.WriteLine($"{Name} наносит дополнительный урон {target.Name} с помощью Огненного шара!");
            Console.WriteLine($"{target.Name} потерял {Attack + 10} единиц здоровья.");

            if (target.Health > 0)
            {
                Console.WriteLine($"У {target.Name} осталось {target.Health} единиц здоровья!");
            }

            // Увеличиваем счетчик ходов с момента последнего использования активной способности
            turnsSinceLastActiveAbility++;
        }

        public override void PassiveAbility()
        {
            turnsSinceLastActiveAbility++;
        }
    }
}
