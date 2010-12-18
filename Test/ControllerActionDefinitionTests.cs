using System;
using System.Reflection;
using System.Web.Mvc;
using JsRouting.Core;
using Xunit;

namespace Test
{
    public class ControllerActionDefinitionTests
    {
        private static readonly MethodInfo method = typeof(TestController).GetMethod("MyAction");

        [Fact]
        public void Ctr_ThrowsException_GivenNullMethod()
        {
            Assert.Throws<ArgumentNullException>(() => new ControllerActionDefinition(null));
        }

        [Fact]
        public void Ctr_SetsMethodInfoProperty()
        {
            Assert.Equal(method, new ControllerActionDefinition(method).Method);
        }

        [Fact]
        public void Ctr_SetsActionNameToMethodName()
        {
            Assert.Equal("MyAction", new ControllerActionDefinition(method).ActionName);
        }

        [Fact]
        public void Ctr_SetsControllerNameToTypeName_WithoutController()
        {
            Assert.Equal("Test", new ControllerActionDefinition(method).ControllerName);
        }

        [Fact]
        public void Ctr_SetsParametersForMethodToEmpty()
        {
            Assert.Empty(new ControllerActionDefinition(method).Parameters);
        }

        private class TestController : Controller
        {
            public ActionResult MyAction()
            {
                return null;
            }
        }
    }
}