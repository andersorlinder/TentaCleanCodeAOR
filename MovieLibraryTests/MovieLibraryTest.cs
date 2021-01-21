using Microsoft.VisualStudio.TestTools.UnitTesting;
using MovieLibrary.Controllers;
using System.Collections.Generic;

namespace MovieLibraryTests
{
    [TestClass]
    public class MovieLibraryTest
    {
        [TestMethod]
        public void GetTopListTest()
        {
            var controller = new MovieController();
            List<string> actual = controller.GetToplist(true);
            List<string> expected = new List<string>(MovieLibraryMockup.ExpectedTopListAscending);
            CollectionAssert.AreEqual(actual, expected);
        }
    }
}
