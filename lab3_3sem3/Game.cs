using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Lab3.Program;

namespace Lab3
{
    [Serializable]
    internal class Game
    {
        public Creature[,] GameBoard { get; set; }
        public int Rows { get; set; }
        public int Columns { get; set; }

        public string ForbiddenCellSymbol { get; set; } = "Болото"; // Символ для вывода на запрещенных клетках


        // Список координат запрещенных клеток
        private List<Tuple<int, int>> forbiddenCells;

        public Game(int rows, int columns)
        {
            Rows = rows;
            Columns = columns;
            GameBoard = new Creature[Rows, Columns];

            forbiddenCells = new List<Tuple<int, int>>
            {
                Tuple.Create(1, 1),
                Tuple.Create(3, 1),
                Tuple.Create(1, 3),
                Tuple.Create(3, 3)
            };
        }

        // Добавим метод для подсчета количества существ на игровом поле
        public int CreaturesCount()
        {
            int count = 0;
            foreach (Creature creature in GameBoard)
            {
                if (creature != null)
                {
                    count++;
                }
            }
            return count;
        }


        public void AddCreatureToBoard(Creature creature, int row, int column)
        {
            if (row >= 0 && row < Rows && column >= 0 && column < Columns)
            {
                GameBoard[row, column] = creature;
            }
            else
            {
                Console.WriteLine("Неверные координаты для добавления существа на поле!");
            }
        }

        public Creature GetCreatureAtPosition(int row, int column)
        {
            if (row >= 0 && row < Rows && column >= 0 && column < Columns)
            {
                return GameBoard[row, column];
            }
            else
            {
                Console.WriteLine("Неверные координаты для получения существа с поля!");
                return null;
            }
        }

        public void MoveCreature(int initialRow, int initialColumn, int targetRow, int targetColumn)
        {
            Creature creatureToMove = GetCreatureAtPosition(initialRow, initialColumn);
            if (creatureToMove != null)
            {
                // Проверяем, что целевая клетка не является запрещенной
                if (!IsCellForbidden(targetRow, targetColumn))
                {
                    // Проверяем, что целевая клетка находится на расстоянии одной клетки от начальной
                    if (Math.Abs(targetRow - initialRow) <= 1 && Math.Abs(targetColumn - initialColumn) <= 1)
                    {
                        if (GetCreatureAtPosition(targetRow, targetColumn) == null)
                        {
                            GameBoard[targetRow, targetColumn] = creatureToMove;
                            GameBoard[initialRow, initialColumn] = null;
                            Console.WriteLine($"{creatureToMove.Name} переместился на ({targetRow}, {targetColumn})");
                        }
                        else
                        {
                            Console.WriteLine("Невозможно переместить, клетка уже занята!");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Невозможно переместить, существо может перемещаться только на соседние клетки!");
                    }
                }
                else
                {
                    Console.WriteLine("Невозможно переместить, клетка является Болотом!");
                }
            }
            else
            {
                Console.WriteLine("Невозможно переместить, существо не найдено!");
            }
        }

        // Метод для проверки, является ли клетка запрещенной
        public bool IsCellForbidden(int row, int column)
        {
            return forbiddenCells.Any(cell => cell.Item1 == row && cell.Item2 == column);
        }

        public void Attack(int attackerRow, int attackerColumn, int targetRow, int targetColumn)
        {
            Creature attacker = GetCreatureAtPosition(attackerRow, attackerColumn);
            Creature target = GetCreatureAtPosition(targetRow, targetColumn);

            if (attacker is Mage || attacker is Priest)
            {
                // Для мага и священника проверяем возможность атаки как на соседней клетке, так и через одну клетку
                if ((Math.Abs(attackerRow - targetRow) <= 1 && Math.Abs(attackerColumn - targetColumn) <= 1))
                {
                    // Вызываем метод атаки
                    if (attacker != null && target != null)
                    {
                        attacker.AttackCreature(target);
                        if (target.Health <= 0)
                        {
                            GameBoard[targetRow, targetColumn] = null;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Невозможно совершить атаку, существо не найдено!");
                    }
                }
                else
                {
                    Console.WriteLine("Невозможно совершить атаку, цель слишком далеко!");
                }
            }
            else if (attacker is Rogue || attacker is Warrior)
            {
                // Для разбойника и рыцаря атака возможна только на соседней клетке
                if ((Math.Abs(attackerRow - targetRow) == 1 && Math.Abs(attackerColumn - targetColumn) <= 1) || (Math.Abs(attackerColumn - targetColumn) == 1 && Math.Abs(attackerRow - targetRow) <= 1))
                {
                    // Вызываем метод атаки
                    if (attacker != null && target != null)
                    {
                        attacker.AttackCreature(target);
                        if (target.Health <= 0)
                        {
                            GameBoard[targetRow, targetColumn] = null;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Невозможно совершить атаку, существо не найдено!");
                    }
                }
                else
                {
                    Console.WriteLine("Невозможно совершить атаку, цель слишком далеко!");
                }
            }
        }



    public bool IsGameOver()
        {
            foreach (Creature creature in GameBoard)
            {
                if (creature != null)
                {
                    return false;
                }
            }
            return true;
        }

        public void PrintGameBoard()
        {
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    if (GameBoard[i, j] != null)
                    {
                        Console.Write($"{GameBoard[i, j].Name} ");
                    }
                    else
                    {
                        if (IsCellForbidden(i, j))
                        {
                            Console.Write($"{ForbiddenCellSymbol} ");
                        }
                        else
                        {
                            Console.Write("Пусто ");
                        }
                    }
                }
                Console.WriteLine();
            }
        }

        public bool AnyCreaturesLeft(CreatureTeam team)
        {
            foreach (Creature creature in GameBoard)
            {
                if (creature != null && creature.Team == team)
                {
                    return true;
                }
            }
            return false;
        }

        public void ClearBoard()
        {
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    GameBoard[i, j] = null;
                }
            }
            Console.WriteLine("Игровое поле очищено!");
        }

        public void RemoveCreatureFromBoard(int row, int column)
        {
            if (row >= 0 && row < Rows && column >= 0 && column < Columns)
            {
                GameBoard[row, column] = null;
            }
            else
            {
                Console.WriteLine("Неверные координаты для удаления существа с поля!");
            }
        }

        public bool IsCellOccupied(int row, int column)
        {
            if (row >= 0 && row < Rows && column >= 0 && column < Columns)
            {
                return GameBoard[row, column] != null;
            }
            else
            {
                Console.WriteLine("Неверные координаты для проверки клетки на занятость!");
                return false;
            }
        }
    }
}
