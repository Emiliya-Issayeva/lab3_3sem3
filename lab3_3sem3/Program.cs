using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
#pragma warning disable SYSLIB0011

namespace Lab3
{
 
    internal class Program
    {
        static Game game; // Объявляем переменную для доступа к игре
        static string autosaveFilename = "autosave.dat";

        static int GetCoordinate(string coordinateName, int maxRows, int maxColumns)
        {
            int coordinate;
            while (true)
            {
                Console.Write($"Введите {coordinateName}: ");
                string input = Console.ReadLine();
                if (int.TryParse(input, out coordinate))
                {
                    if (coordinate >= 0 && coordinate < maxRows && coordinate < maxColumns)
                    {
                        return coordinate;
                    }
                    else
                    {
                        Console.WriteLine($"Ошибка ввода. {coordinateName} должен быть в диапазоне от 0 до {maxRows - 1} для строк и от 0 до {maxColumns - 1} для столбцов.");
                    }
                }
                else
                {
                    Console.WriteLine("Ошибка ввода. Пожалуйста, введите целое число.");
                }
            }
        }

        static void Main(string[] args)
        {
            AppContext.SetSwitch("System.Runtime.Serialization.BinaryFormatter.Enable", true);

            // Проверка наличия файла автосохранения
            if (File.Exists(autosaveFilename))
            {
                Console.WriteLine("Обнаружено автосохранение. Хотите загрузить экстренно завершенную игру? (y/n)");
                string choice = Console.ReadLine();
                if (choice?.ToLower() == "y")
                {
                    game = LoadGame(autosaveFilename) ?? new Game(5, 5);
                }
                else
                {
                    game = new Game(5, 5);
                }
            }
            else
            {
                game = new Game(5, 5);
            }

            Console.WriteLine("Добро пожаловать в игру!");

            AppDomain.CurrentDomain.ProcessExit += (s, e) => AutoSaveOnExit(); // Автосохранение при закрытии программы

            while (true)
            {
                Console.WriteLine("Выберите действие:");
                Console.WriteLine("1. Начать игру");
                Console.WriteLine("2. Добавить существо");
                Console.WriteLine("3. Вывести текущее состояние игрового поля");
                Console.WriteLine("4. Загрузить сохраненную игру");
                Console.WriteLine("5. Завершить игру");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        PlayGame();
                        break;
                    case "2":
                        AddCreature();
                        break;
                    case "3":
                        PrintGameBoard();
                        break;
                    case "4":
                        LoadSavedGame();
                        break;
                    case "5":
                        Console.WriteLine("Сохраняем игру и завершаем.");
                        SaveGame(game, autosaveFilename);
                        Environment.Exit(0); // Завершение программы
                        break;
                    default:
                        Console.WriteLine("Некорректный ввод. Пожалуйста, выберите действие из списка.");
                        break;
                }
            }
        }

        static void PlayGame()

        {
            if (game.CreaturesCount() == 0)
            {
                Console.WriteLine("Нельзя начать игру без существ. Пожалуйста, добавьте существ перед началом игры.");
                return;
            }

            ;

            while (game.AnyCreaturesLeft(Program.CreatureTeam.Team1) || game.AnyCreaturesLeft(Program.CreatureTeam.Team2))
            {
                // Проверяем победителя
                if (!game.AnyCreaturesLeft(Program.CreatureTeam.Team1))
                {
                    Console.WriteLine("Команда 2 победила!");
                    Console.WriteLine("Игра окончена!");
                    game = new Game(5, 5);
                    return;
                }
                else if (!game.AnyCreaturesLeft(Program.CreatureTeam.Team2))
                {
                    Console.WriteLine("Команда 1 победила!");
                    Console.WriteLine("Игра окончена!");
                    game = new Game(5, 5);
                    return;
                }

                Console.WriteLine("Выберите действие:");
                Console.WriteLine("1. Переместить существо");
                Console.WriteLine("2. Атаковать существо");
                Console.WriteLine("3. Вывести текущее состояние игрового поля");
                Console.WriteLine("4. Сохранить игру");
                Console.WriteLine("5. Закончить игру");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        MoveCreature();
                        break;
                    case "2":
                        AttackCreature();
                        break;
                    case "3":
                        PrintGameBoard();
                        break;
                    case "4":
                        SaveGame();
                        break;
                    case "5":
                        Console.WriteLine("Игра завершена.");
                        game = new Game(5, 5);
                        return;
                    default:
                        Console.WriteLine("Некорректный ввод. Пожалуйста, выберите действие из списка.");
                        break;
                }
            }

            

        }


        public enum CreatureTeam
    {
        Team1,
        Team2
        }


        
static void AddCreature()
        {
            Console.WriteLine("Введите имя существа:");
            string name = Console.ReadLine();


            // Выбор команды
            CreatureTeam team;
            do
            {

            Console.WriteLine("Выберите команду существа:");
            Console.WriteLine("1. Команда 1");
            Console.WriteLine("2. Команда 2");
                string teamChoice = Console.ReadLine();
                if (teamChoice == "1")
                {
                    team = CreatureTeam.Team1;
                    break;
                }
                else if (teamChoice == "2")
                {
                    team = CreatureTeam.Team2;
                    break;
                }
                else
                {
                    Console.WriteLine("Некорректный выбор команды. Пожалуйста, выберите команду 1 или 2.");
                }
            } while (true);


            int health;
            int attack;

            Console.WriteLine("Выберите класс существа:");
            Console.WriteLine("");

            Console.WriteLine("1. Воин");
            Console.WriteLine("Количество здоровья существа: 90");
            Console.WriteLine("Сила атаки существа: 30");
            Console.WriteLine("Шанс уклонения существа: 20");
            Console.WriteLine("Пассивная способность: уменьшает получаемый урон на 10 единиц");
            Console.WriteLine("");
           
            Console.WriteLine("2. Маг");
            Console.WriteLine("Количество здоровья существа: 70");
            Console.WriteLine("Сила атаки существа: 40");
            Console.WriteLine("Шанс уклонения существа: 30");
            Console.WriteLine("Пассивная способность: в два раза быстрее восстанавливает активную способность");
            Console.WriteLine("");

            Console.WriteLine("3. Разбойник");
            Console.WriteLine("Количество здоровья существа: 60");
            Console.WriteLine("Сила атаки существа: 50");
            Console.WriteLine("Шанс уклонения существа: 40");
            Console.WriteLine("Пассивная способность: при успешном уклонении восстанавливает 25 единиц здоровья");
            Console.WriteLine("");

            Console.WriteLine("4. Священник"); 
            Console.WriteLine("Количество здоровья существа: 80");
            Console.WriteLine("Сила атаки существа: 20");
            Console.WriteLine("Шанс уклонения существа: 50");
            Console.WriteLine("Пассивная способность: лечение на 15 единиц здоровья при каждой обычной атаке");
            Console.WriteLine("");

            string classChoice = Console.ReadLine();

            int evasion = 10; // Значение шанса уклонения по умолчанию

            switch (classChoice)
            {
                case "1":

                    health = int.Parse("90");
                    attack = int.Parse("30");
                    int armor = int.Parse("20");
                    evasion = int.Parse("20");
                    Creature warrior = new Warrior(name, health, attack, armor, evasion, team);
                    AddCreatureToBoard(warrior);
                    break;

                case "2":


                    health = int.Parse("70");
                    attack = int.Parse("40");
                    int mana = int.Parse("100");
                    evasion = int.Parse("30");
                    Creature mage = new Mage(name, health, attack, mana, evasion, team);
                    AddCreatureToBoard(mage);
                    break;

                case "3":


                    health = int.Parse("60");
                    attack = int.Parse("50");
                    int evasionValue = int.Parse("30");
                    evasion = int.Parse("40");
                    Creature rogue = new Rogue(name, health, attack, evasionValue, evasion, team);
                    AddCreatureToBoard(rogue);
                    break;


                case "4":


                    health = int.Parse("80");
                    attack = int.Parse("20");
                    int healingPower = int.Parse("20");
                    evasion = int.Parse("50");
                    Creature priest = new Priest(name, health, attack, healingPower, evasion, team);
                    AddCreatureToBoard(priest);
                    break;
                default:
                    Console.WriteLine("Некорректный ввод. Пожалуйста, выберите класс из списка.");
                    break;
            }
        }

        static void AddCreatureToBoard(Creature creature)
        {
            int maxRows = game.GameBoard.GetLength(0);
            int maxColumns = game.GameBoard.GetLength(1);

            Console.WriteLine("Введите координаты, куда хотите поместить существо (строка и столбец)");

            int row;
            int column;
            do
            {
                row = GetCoordinate("Cтроку", maxRows, maxColumns);
                column = GetCoordinate("Столбец", maxRows, maxColumns);

                if (game.IsCellForbidden(row, column))
                {
                    Console.WriteLine("Невозможно поместить существо на Болото!");
                }
                else
                {
                    break;
                }
            } while (true);

            game.AddCreatureToBoard(creature, row, column);
        }

        static void MoveCreature()
        {
            int maxRows = game.GameBoard.GetLength(0);
            int maxColumns = game.GameBoard.GetLength(1);

            Console.WriteLine("Введите начальные координаты существа (строка и столбец)");
            int initialRow = GetCoordinate("начальную строку", maxRows, maxColumns);
            int initialColumn = GetCoordinate("начальный столбец", maxRows, maxColumns);

            Console.WriteLine("Введите конечные координаты (строка и столбец)");
            int targetRow = GetCoordinate("конечную строку", maxRows, maxColumns);
            int targetColumn = GetCoordinate("конечный столбец", maxRows, maxColumns);
 

            game.MoveCreature(initialRow, initialColumn, targetRow, targetColumn);
        }

        static void AttackCreature()

        {
            int maxRows = game.GameBoard.GetLength(0);
            int maxColumns = game.GameBoard.GetLength(1);

            Console.WriteLine("Введите координаты атакующего существа (строка и столбец):");
            int attackerRow = GetCoordinate("строку атакующего существа", maxRows, maxColumns);
            int attackerColumn = GetCoordinate("столбец атакующего существа", maxRows, maxColumns);

            Console.WriteLine("Введите координаты цели атаки (строка и столбец):");
            int targetRow = GetCoordinate("строку цели атаки", maxRows, maxColumns);
            int targetColumn = GetCoordinate("столбец цели атаки", maxRows, maxColumns);

            Creature attacker = game.GetCreatureAtPosition(attackerRow, attackerColumn);
            Creature target = game.GetCreatureAtPosition(targetRow, targetColumn);

            // Проверяем, что обе цели существуют и принадлежат разным командам
            if (attacker != null && target != null && attacker.Team != target.Team)
            {
                game.Attack(attackerRow, attackerColumn, targetRow, targetColumn);
                if (target.Health <= 0)
                {
                  

                    Console.WriteLine($"{target.Name} погиб!");
                }
            }
            else
            {
                Console.WriteLine("Невозможно совершить атаку, некорректные цели или существа принадлежат одной команде!");
            }
        }

        static void PrintGameBoard()
        {
            Console.WriteLine("Текущее состояние игрового поля:");
            game.PrintGameBoard();
        }

        static void AutoSaveOnExit()
        {
            Console.WriteLine("Автосохранение текущего состояния игры...");
            SaveGame(game, autosaveFilename);
        }

        static void SaveGame()
        {
            Console.WriteLine("Введите имя файла для сохранения:");
            string filename = Console.ReadLine();
            SaveGame(game, filename);
        }

        static void LoadGame()
        {
            Console.WriteLine("Введите имя файла для загрузки:");
            string filename = Console.ReadLine();
            game = LoadGame(filename);
        }

        static void LoadSavedGame()
        {
            Console.WriteLine("Введите имя файла для загрузки:");
            string filename = Console.ReadLine();
            Game loadedGame = LoadGame(filename);

            if (loadedGame != null)
            {
                game = loadedGame;
                
            }
            else
            {
                Console.WriteLine("Не удалось загрузить игру.");
            }
        }

        static void SaveGame(Game game, string filename)
        {
            try
            {
                IFormatter formatter = new BinaryFormatter();
                using (Stream stream = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    formatter.Serialize(stream, game);
                }
                Console.WriteLine($"Игра сохранена в файл {filename}");
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Ошибка сохранения игры: {ex.Message}");
            }
        }

        static Game LoadGame(string filename)
        {
            try
            {
                Game game;
                IFormatter formatter = new BinaryFormatter();
                using (Stream stream = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    game = (Game)formatter.Deserialize(stream);
                }
                Console.WriteLine("Игра успешно загружена!");
                return game;
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Ошибка загрузки игры: {ex.Message}");
                return null;
            }
            catch (SerializationException ex)
            {
                Console.WriteLine($"Ошибка загрузки игры: {ex.Message}");
                return null;
            }
        }

    }
}
