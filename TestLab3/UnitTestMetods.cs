using Lab3;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lab3.Tests
{
    [TestClass]
    public class CreatureTests
    {
        [TestMethod]
        public void TestCreatureCreation()
        {
            // Arrange
            string name = "TestCreature";
            int health = 100;
            int attack = 20;
            int evasion = 30;

            // Act
            Creature creature = new Creature(name, health, attack, evasion);

            // Assert
            Assert.AreEqual(name, creature.Name);
            Assert.AreEqual(health, creature.Health);
            Assert.AreEqual(attack, creature.Attack);
            Assert.AreEqual(evasion, creature.Evasion);
        }

        [TestMethod]
        public void TestCreatureAttack()
        {
            // Arrange
            Creature attacker = new Creature("Attacker", 100, 20, 10);
            Creature target = new Creature("Target", 100, 10, 0); // ”станавливаем шанс уклонени€ цели в 0

            // Act
            attacker.AttackCreature(target);

            // Assert
            Assert.IsTrue(target.Health < 100, "ќжидалось, что здоровье цели меньше 100.");
        }


        [TestMethod]
        public void TestCreatureEvasion()
        {
            // Arrange
            Creature attacker = new Creature("Attacker", 100, 20, 10);
            Creature target = new Creature("Target", 100, 10, 100); 

            // Act
            attacker.AttackCreature(target);

            // Assert
            Assert.AreEqual(100, target.Health); 
        }
    }
}