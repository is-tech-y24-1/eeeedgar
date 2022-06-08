using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VerifyCS = AnalyzerTemplate.Test.CSharpCodeFixVerifier<
    AnalyzerTemplate.AnalyzerI7Array,
    AnalyzerTemplate.CodeFixProviderI7Array>;

namespace AnalyzerTemplate.Test
{
    [TestClass]
    public class AnalyzerI7ArrayUnitTests
    {
        [TestMethod]
        public async Task TestArrayMethodReturnsArray()
        {
            var test = @"using System;
    public class Englishman
    {
        public int[] Method()
        {
            return new int[5];
        }
    }";

            await VerifyCS.VerifyAnalyzerAsync(test);
        }


        [TestMethod]
        public async Task TestArrayMethodReturnsNull()
        {
            var test = @"using System;
    public class NewYork
    {
        public string[] Method()
        {
            return [|null|];
        }
    }";
            var fixtest = @"using System;
    public class NewYork
    {
        public string[] Method()
        {
            return Array.Empty<string>();
        }
    }";

            await VerifyCS.VerifyCodeFixAsync(test, fixtest);
        }
    }
}