using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Lab3.Program;

namespace Lab3
{
    [Serializable]
    internal class Warrior : Creature
    {
        public int Armor { get; set; }
        private int turnsSinceLastActiveAbility = 2;

        public Warrior(string name, int health, int attack, int armor, int evasion, CreatureTeam team) : base(name, health, attack, evasion)
        {
            Armor = armor;
            Team = team;
        }

        public override void AttackCreature(Creature target)
        {
            Console.WriteLine($"Выберите действие для {Name}:");
            Console.WriteLine("1. Обычная атака");
            if (turnsSinceLastActiveAbility >= 2)
                Console.WriteLine("2. Использовать активную способность: Разрушительный удар!");
            else
                Console.WriteLine("2. Использовать активную способность: Разрушительный удар! (не готов)");
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
                    // Проверяем, прошло ли уже два хода с момента последнего использования активной способности
                    if (turnsSinceLastActiveAbility >= 2)
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
                Console.WriteLine($"{Name} использует активную способность: Разрушительный удар!");
                // Допустим, активная способность мага наносит дополнительный урон
                target.Health -= Attack;
                Console.WriteLine($"{Name} наносит дополнительный урон {target.Name} с помощью Разрушиткльного удара!");
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
            Console.WriteLine($"{Name} использует активную способность: Разрушительный удар!");
            // Допустим, активная способность мага наносит дополнительный урон
            target.Health -= Attack + 10;
            Console.WriteLine($"{Name} наносит дополнительный урон {target.Name} с помощью Разрушительного удара!");
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
            // Переопределение метода пассивной способности для влина

            turnsSinceLastActiveAbility++;
        }
    }
}
