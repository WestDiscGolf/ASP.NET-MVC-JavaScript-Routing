using JsRouting.Core;
using Xunit;

namespace Test
{
    public class OutputTests
    {
        [Fact]
        public void Output_IsNonEmpty()
        {
            Assert.NotEmpty(Output.ManagerJs);
        }
    }
}