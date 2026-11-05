using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using BankApp;

namespace BankTests
{
    [TestFixture]
    public class BankAccountTests
    {
        private Mock<TransactionLogger> mockLogger;
        [SetUp]
        public void SetUp()
        {
            mockLogger = new Mock<TransactionLogger>();
        }
        [Test] // Создание объекта с корректным начальным балансом
        public void TestCreateValid()
        {
            var acc = new BankAccount("ACC-001", 100f, mockLogger.Object);
            Assert.That(acc.AccountNumber, Is.EqualTo("ACC-001"));
            Assert.That(acc.GetBalance(), Is.EqualTo(100f));
        }
        [Test] // Создание объекта с балансом по умолчанию (0)
        public void TestCreateDefault()
        {
            var acc = new BankAccount("ACC-002");
            Assert.That(acc.GetBalance(), Is.EqualTo(0f));
        }
        [Test] // Создание объекта с некорректным (отрицательным) балансом
        public void TestCreateNegative()
        {
            Assert.Throws<ArgumentException>(() => new BankAccount("ACC-003", -10f));
        }
        [Test] // Создание объекта с пустым номером счёта
        public void TestCreateEmptyNumber()
        {
            Assert.Throws<ArgumentException>(() => new BankAccount(""));
        }
        [Test] // Успешное добавление средств
        public void TestDepositSuccess()
        {
            var acc = new BankAccount("ACC-004", 50f, mockLogger.Object);
            acc.Deposit(25f);
            Assert.That(acc.GetBalance(), Is.EqualTo(75f));
            mockLogger.Verify(l => l.Log(It.IsAny<string>()), Times.Once); // Ровно 1 вызов
        }
        [TestCase(0f)] // Попытка пополнить некорректную сумму (0 или отрицательное значение)
        [TestCase(-5f)]
        public void TestDepositInvalid(float amount)
        {
            var acc = new BankAccount("ACC-005", 10f, mockLogger.Object);
            Assert.Throws<ArgumentException>(() => acc.Deposit(amount));
            Assert.That(acc.GetBalance(), Is.EqualTo(10f));
            mockLogger.Verify(l => l.Log(It.IsAny<string>()), Times.Never);
        }
        [Test] // Успешное снятие, если сумма не превышает баланс
        public void TestWithdrawSuccess()
        {
            var acc = new BankAccount("ACC-006", 100f, mockLogger.Object);
            acc.Withdraw(40f);
            Assert.That(acc.GetBalance(), Is.EqualTo(60f));
            mockLogger.Verify(l => l.Log(It.IsAny<string>()), Times.Once);
        }
        [Test] // Ошибка при попытке снять сумму больше текущего баланса
        public void TestWithdrawTooMuch()
        {
            var acc = new BankAccount("ACC-007", 30f, mockLogger.Object);
            var ex = Assert.Throws<InvalidOperationException>(() => acc.Withdraw(50f));
            Assert.That(ex.Message, Is.EqualTo("Insufficient funds"));
            Assert.That(acc.GetBalance(), Is.EqualTo(30f));
            mockLogger.Verify(l => l.Log(It.IsAny<string>()), Times.Never);
        }
        [Test] // Снятие всей суммы со счёта (граничный случай)
        public void TestWithdrawAll()
        {
            var acc = new BankAccount("ACC-008", 20f, mockLogger.Object);
            acc.Withdraw(20f);
            Assert.That(acc.GetBalance(), Is.EqualTo(0f));
        }
        [TestCase(0f)] // Ошибка при попытке снять некорректную сумму (0 или отрицательное значение)
        [TestCase(-1f)]
        public void TestWithdrawInvalid(float amount)
        {
            var acc = new BankAccount("ACC-009", 50f, mockLogger.Object);
            Assert.Throws<ArgumentException>(() => acc.Withdraw(amount));
            Assert.That(acc.GetBalance(), Is.EqualTo(50f));
        }
        [Test] // Проверка текущего баланса после нескольких операций
        public void TestBalanceAfterOperations()
        {
            var acc = new BankAccount("ACC-010", 100f, mockLogger.Object);
            acc.Deposit(50f);
            acc.Withdraw(30f);
            acc.Deposit(10f);
            Assert.That(acc.GetBalance(), Is.EqualTo(130f));
            mockLogger.Verify(l => l.Log(It.IsAny<string>()), Times.Exactly(3));
        }
        [Test] // Доп: проверка сообщения, передаваемого в логгер
        public void TestLogMessage()
        {
            var acc = new BankAccount("ACC-011", 0f, mockLogger.Object);
            acc.Deposit(15f);
            mockLogger.Verify(l => l.Log(It.Is<string>(s =>
                s.Contains("15") && s.Contains("ACC-011"))), Times.Once);
        }
    }
}
