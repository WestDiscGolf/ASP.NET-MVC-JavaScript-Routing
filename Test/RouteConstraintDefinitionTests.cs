using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using JsRouting.Core;

namespace Test
{
    public class RouteConstraintDefinitionTests
    {
        [Fact]
        public void ToJavaScript_AcceptanceTest()
        {
            var constraint = new RouteConstraintDefinition(constraintName: "notEmpty", parameterName: "myParam", data: "{'parameter':'foo'}");
            Assert.Equal("$.routeManager.constraintTypeDefs.notEmpty('myParam', {'parameter':'foo'})", constraint.ToJavaScript());
        }
    }
}
