using NUnit.Framework;
using System.Linq;
using TwitterHashtag;

namespace TwitterHashtagTest
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void AddOnlyAddsOnePerUniqueTag()
        {
            HashTags tags = new HashTags();
            for (int i = 0; i < 5; i++)
            {
                tags.Add("test");
            }
            var vals = tags.GetTopTen();
            Assert.IsTrue(vals.Count() == 1);
            Assert.Pass();
        }

        [Test]
        public void LowerCountsWillGetBumped()
        {
            HashTags tags = new HashTags();
            for (int i = 0; i < 11; i++)
            {
                tags.Add("test" + i);
            }
            var vals = tags.GetTopTen();
            Assert.IsTrue(vals.Any(x=> x == "test0"));
            for (int i = 1; i < 11; i++)
            {
                tags.Add("test" + i);
            }
            vals = tags.GetTopTen();
            Assert.IsFalse(vals.Any(x => x == "test0"));
            Assert.Pass();
        }

        [Test]
        public void NonTopTenGetPromotedInOrder()
        {
            HashTags tags = new HashTags();
            for (int i = 0; i < 11; i++)
            {
                tags.Add("test" + i);
            }
            var vals = tags.GetTopTen();
            Assert.IsTrue(vals.Any(x => x == "test0"));
            tags.Add("test10");
            vals = tags.GetTopTen();
            Assert.IsTrue(vals.First() == "test10");
            Assert.Pass();
        }
    }
}