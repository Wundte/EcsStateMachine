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
    }
}