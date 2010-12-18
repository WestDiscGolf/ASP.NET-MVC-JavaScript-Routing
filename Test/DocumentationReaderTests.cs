using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using JsRouting.Core;

namespace Test
{
    public class DocumentationReaderTests
    {
        [Fact]
        public void ParameterDescription_AcceptanceTest()
        {
            var docReader = new DocumentationReader(typeof(NonActionInterceptor).Assembly);
            var desc = docReader.ParameterDescription(typeof(NonActionInterceptor).GetMethod("Intercept"), "definition");
            Assert.NotEmpty(desc);
        }

        [Fact]
        public void MethodDescription_AcceptanceTest()
        {
            var docReader = new DocumentationReader(typeof(NonActionInterceptor).Assembly);
            var desc = docReader.MethodDescription(typeof(NonActionInterceptor).GetMethod("Intercept"));
            Assert.NotEmpty(desc);
        }
    }
}