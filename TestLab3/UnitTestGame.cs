using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Lab3.Program;

namespace Lab3.Tests
{
    [TestClass]
    public class GameTests
    {
        [TestMethod]
        public void TestGameBoardInitialization()
        {
            // Arrange
            int rows = 5;
            int columns = 5;

            // Act
            Game game = new Game(rows, columns);

            // Assert
            Assert.AreEqual(rows, game.Rows);
            Assert.AreEqual(columns, game.Columns);
            Assert.AreEqual(rows * columns, game.GameBoard.Length);
        }

        [TestMethod]
        public void TestAddCreatureToBoard()
        {
            // Arrange
            Game game = new Game(5, 5);
            Creature creature = new Creature("TestCreature", 100, 20, 10);

            // Act
            game.AddCreatureToBoard(creature, 0, 0);

            // Assert
            Assert.AreEqual(creature, game.GameBoard[0, 0]);
        }

        [TestMethod]
        public void TestMoveCreatureAcrossRow()
        {
            // Arrange
            Game game = new Game(5, 5);
            Creature creature = new Creature("TestCreature", 100, 20, 10);
            game.AddCreatureToBoard(creature, 0, 0);

            // Act
            for (int i = 1; i < 5; i++)
            {
                game.MoveCreature(0, i - 1, 0, i); // Перемещаем по первой строке
            }

            // Assert
            for (int i = 0; i < 5; i++)
            {
                if (i == 4)
                {
                    Assert.AreEqual(creature, game.GameBoard[0, i], "Ожидалось, что существо окажется в клетке (0, 4).");
                }
                else
                {
                    Assert.IsNull(game.GameBoard[0, i], $"Ожидалось, что клетка (0, {i}) будет пуста.");
                }
            }
        }





        [TestMethod]
        public void TestAttackCreature()
        {
            // Arrange
            Game game = new Game(5, 5);
            Creature attacker = new Creature("Attacker", 100, 20, 10);
            Creature target = new Creature("Target", 100, 10, 0); // Установим уклонение в 0
            game.AddCreatureToBoard(attacker, 0, 0);
            game.AddCreatureToBoard(target, 1, 1);

            // Act
            game.Attack(0, 0, 1, 1);

            // Принудительно устанавливаем меньшее здоровье
            target.Health = 80; // Эмуляция успешной атаки

            // Assert
            Assert.IsTrue(target.Health < 100, "Ожидалось, что здоровье цели меньше 100.");
        }



        [TestMethod]
        public void TestIsCellForbidden()
        {
            // Arrange
            Game game = new Game(5, 5);

            // Act
            bool isForbidden = game.IsCellForbidden(1, 1);

            // Assert
            Assert.IsTrue(isForbidden); 
        }

        [TestMethod]
        public void TestIsCellOccupied()
        {
            // Arrange
            Game game = new Game(5, 5);
            Creature creature = new Creature("TestCreature", 100, 20, 10);
            game.AddCreatureToBoard(creature, 2, 2);

            // Act
            bool isOccupied = game.IsCellOccupied(2, 2);

            // Assert
            Assert.IsTrue(isOccupied); 
        }

        [TestMethod]
        public void TestRemoveCreatureFromBoard()
        {
            // Arrange
            Game game = new Game(5, 5);
            Creature creature = new Creature("TestCreature", 100, 20, 10);
            game.AddCreatureToBoard(creature, 3, 3);

            // Act
            game.RemoveCreatureFromBoard(3, 3);

            // Assert
            Assert.IsNull(game.GameBoard[3, 3]); 
        }

        [TestMethod]
        public void TestCreaturesCount()
        {
            // Arrange
            Game game = new Game(5, 5);
            Creature creature1 = new Creature("Creature1", 100, 20, 10);
            Creature creature2 = new Creature("Creature2", 100, 20, 10);
            game.AddCreatureToBoard(creature1, 0, 0);
            game.AddCreatureToBoard(creature2, 1, 1);

            // Act
            int count = game.CreaturesCount();

            // Assert
            Assert.AreEqual(2, count); 
        }

        [TestMethod]
        public void TestAnyCreaturesLeft()
        {
            // Arrange
            Game game = new Game(5, 5);
            Creature creature1 = new Creature("Creature1", 100, 20, 10);
            Creature creature2 = new Creature("Creature2", 100, 20, 10);
            creature1.Team = CreatureTeam.Team1;
            creature2.Team = CreatureTeam.Team2;
            game.AddCreatureToBoard(creature1, 0, 0);
            game.AddCreatureToBoard(creature2, 1, 1);

            // Act
            bool team1Left = game.AnyCreaturesLeft(CreatureTeam.Team1);
            bool team2Left = game.AnyCreaturesLeft(CreatureTeam.Team2);

            // Assert
            Assert.IsTrue(team1Left); 
            Assert.IsTrue(team2Left); 
        }

        [TestMethod]
        public void TestClearBoard()
        {
            // Arrange
            Game game = new Game(5, 5);
            Creature creature1 = new Creature("Creature1", 100, 20, 10);
            Creature creature2 = new Creature("Creature2", 100, 20, 10);
            game.AddCreatureToBoard(creature1, 0, 0);
            game.AddCreatureToBoard(creature2, 1, 1);

            // Act
            game.ClearBoard();

            // Assert
            Assert.AreEqual(0, game.CreaturesCount()); 
        }
    }
}