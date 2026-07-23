using System.Linq;
using Code.EcsStateMachine.Editor.CodeGeneration.Utils;
using NUnit.Framework;

namespace Tests
{
        public class StableIdTests
    {
        [Test]
        public void SameStringProducesSameId()
        {
            var first = StableId.Get("Test.Type");
            var second = StableId.Get("Test.Type");

            Assert.AreEqual(first, second);
        }

        [Test]
        public void DifferentStringsProduceDifferentIds()
        {
            var first = StableId.Get("Test.FirstType");
            var second = StableId.Get("Test.SecondType");

            Assert.AreNotEqual(first, second);
        }

        [Test]
        public void NamespaceDifferenceProducesDifferentIds()
        {
            var first = StableId.Get("NamespaceA.TestType");
            var second = StableId.Get("NamespaceB.TestType");

            Assert.AreNotEqual(first, second);
        }

        [Test]
        public void EmptyStringProducesValidId()
        {
            Assert.DoesNotThrow(() =>
            {
                StableId.Get(string.Empty);
            });
        }

        [Test]
        public void LongStringProducesValidId()
        {
            var value = new string('A', 10000);

            Assert.DoesNotThrow(() =>
            {
                StableId.Get(value);
            });
        }

        [Test]
        public void SimilarStringsProduceDifferentIds()
        {
            var first = StableId.Get("Test.TypeA");
            var second = StableId.Get("Test.TypeB");

            Assert.AreNotEqual(first, second);
        }

        [Test]
        public void DifferentTypesWithSameNameProduceDifferentIds()
        {
            var types = new[]
            {
                "NamespaceA.MoveSystem",
                "NamespaceB.MoveSystem",
                "NamespaceC.MoveSystem"
            };

            var ids = types.Select(StableId.Get).ToArray();

            Assert.AreEqual(ids.Length, ids.Distinct().Count());
        }

        [Test]
        public void GeneratedIdIsNotZero()
        {
            var id = StableId.Get("Test.Type");

            Assert.AreNotEqual(0, id);
        }
    }
}