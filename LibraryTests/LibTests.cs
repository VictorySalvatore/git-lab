using Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace LibraryTests
{
    [TestFixture]
    public class LibraryManagerTests
    {
        private LibraryManager m;
        [SetUp]
        public void Setup()
        {
            m = new LibraryManager();
            m.InitializeDefaultData();
        }
        [Test]
        public void AddBookDuplicateUdc()
        {
            Book b = new Book("004.4", "X", "Y", 2020, 1);
            Assert.Throws<LibraryException>(() => m.AddBook(b));
        }

        [Test]
        public void IssueLoanDecreasesAvailable()
        {
            int before = m.FindBook("53.01").Now;
            m.IssueLoan("53.01", 1);
            Assert.That(m.FindBook("53.01").Now, Is.EqualTo(before - 1));
        }
        [Test]
        public void ReturnLoanIncreasesAvailable()
        {
            Loan loan = m.IssueLoan("82-31", 1);
            m.ReturnLoan(loan.Id);
            Assert.That(m.FindBook("82-31").Now, Is.EqualTo(4));
        }
    }
}
