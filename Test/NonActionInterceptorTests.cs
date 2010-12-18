using System.Web.Mvc;
using JsRouting.Core;
using Xunit;

namespace Test
{
    public class NonActionInterceptorTests
    {
        [Fact]
        public void Intercept_ReturnsFalse_GivenControllerActionWithNonActionAttribute()
        {
            var method = typeof(TestController).GetMethod("NotAcceptable");
            Assert.False(new NonActionInterceptor().Intercept(new ControllerActionDefinition(method)));
        }

        [Fact]
        public void Intercept_ReturnsTrue_GivenControllerActionWithoutNonActionAttribute()
        {
            var method = typeof(TestController).GetMethod("Acceptable");
            Assert.True(new NonActionInterceptor().Intercept(new ControllerActionDefinition(method)));
        }

        private class TestController : Controller
        {
            public ActionResult Acceptable()
            {
                return null;
            }

            [NonAction]
            public ActionResult NotAcceptable()
            {
                return null;
            }
        }
    }
}