using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using static Tutan.Functional.F;

namespace Tutan.Functional.Tests
{
    [TestFixture]
    public class ErrorTests
    {
        [Test]
        public void Constructor_WithStringMessage_SetsMessage()
        {
            var error = new Error("something went wrong");

            Assert.AreEqual("something went wrong", error.Message);
        }

        [Test]
        public void FactoryMethod_ErrorString_CreatesErrorWithMessage()
        {
            Error error = Error("factory message");

            Assert.AreEqual("factory message", error.Message);
        }

        [Test]
        public void FactoryMethod_ErrorEnumerable_CreatesCompositeError()
        {
            var inner = new List<Error> { Error("first"), Error("second") };

            Error composite = Error((IEnumerable<Error>)inner);

            Assert.AreEqual("first; second", composite.Message);
            Assert.AreEqual(2, composite.InnerErrors.Count);
        }

        [Test]
        public void ImplicitConversion_FromString_CreatesError()
        {
            Error error = "implicit error";

            Assert.AreEqual("implicit error", error.Message);
        }

        [Test]
        public void Inner_WhenSetViaInit_ReturnsInnerError()
        {
            var inner = new Error("inner cause");
            var outer = new Error("outer problem") { Inner = inner };

            Assert.IsNotNull(outer.Inner);
            Assert.AreEqual("inner cause", outer.Inner.Message);
        }

        [Test]
        public void InnerErrors_DefaultsToEmpty()
        {
            var error = new Error("simple");

            Assert.IsNotNull(error.InnerErrors);
            Assert.AreEqual(0, error.InnerErrors.Count);
        }

        [Test]
        public void InnerErrors_CompositeConstructor_PopulatedWithErrors()
        {
            var errors = new[] { new Error("a"), new Error("b"), new Error("c") };

            var composite = new Error(errors);

            Assert.AreEqual(3, composite.InnerErrors.Count);
            Assert.AreEqual("a", composite.InnerErrors[0].Message);
            Assert.AreEqual("b", composite.InnerErrors[1].Message);
            Assert.AreEqual("c", composite.InnerErrors[2].Message);
        }

        [Test]
        public void AsEnumerable_SingleError_ReturnsSelf()
        {
            var error = new Error("only one");

            var result = error.AsEnumerable().ToList();

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(error, result[0]);
        }

        [Test]
        public void AsEnumerable_CompositeError_ReturnsInnerErrors()
        {
            var inner1 = new Error("err1");
            var inner2 = new Error("err2");
            var composite = new Error(new[] { inner1, inner2 });

            var result = composite.AsEnumerable().ToList();

            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(inner1, result[0]);
            Assert.AreEqual(inner2, result[1]);
        }

        [Test]
        public void ToString_SingleError_ReturnsMessage()
        {
            var error = new Error("single message");

            Assert.AreEqual("single message", error.ToString());
        }

        [Test]
        public void ToString_CompositeError_JoinsInnerMessages()
        {
            var composite = new Error(new[] { new Error("alpha"), new Error("beta") });

            Assert.AreEqual("alpha; beta", composite.ToString());
        }

        [Test]
        public void RecordEquality_SameMessage_AreEqual()
        {
            var error1 = new Error("duplicate");
            var error2 = new Error("duplicate");

            Assert.AreEqual(error1, error2);
            Assert.IsTrue(error1 == error2);
        }
    }
}
