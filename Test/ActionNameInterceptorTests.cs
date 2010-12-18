using System.Web.Mvc;
using JsRouting.Core;
using Xunit;
using Xunit.Extensions;

namespace Test
{
    public class ActionNameInterceptorTests
    {
        private const string NewActionName = "NewName";

        [Theory]
        [InlineData("Renamed")]
        [InlineData("NonRenamed")]
        public void Intercept_ReturnsTrue_GivenContollerControllerAction(string action)
        {
            var method = typeof(TestController).GetMethod(action);
            Assert.True(new ActionNameInterceptor().Intercept(new ControllerActionDefinition(method)));
        }

        [Fact]
        public void Intercept_RenamedActionNameInDefinition_WhenAttributeSpecifiesNewName()
        {
            var method = typeof(TestController).GetMethod("Renamed");
            var definition = new ControllerActionDefinition(method);
            new ActionNameInterceptor().Intercept(definition);
            Assert.Equal(NewActionName, definition.ActionName);
        }

        [Fact]
        public void Intercept_KeepsActionNameInDefinition_WhenAttributeMissing()
        {
            string methodName = "NonRenamed";
            var method = typeof(TestController).GetMethod(methodName);
            var definition = new ControllerActionDefinition(method);
            new ActionNameInterceptor().Intercept(definition);
            Assert.Equal(methodName, definition.ActionName);
        }

        private class TestController : Controller
        {
            [ActionName(NewActionName)]
            public ActionResult Renamed()
            {
                return null;
            }

            public ActionResult NonRenamed()
            {
                return null;
            }
        }
    }
}