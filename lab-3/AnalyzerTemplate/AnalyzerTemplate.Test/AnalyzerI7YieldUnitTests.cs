using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VerifyCS = AnalyzerTemplate.Test.CSharpCodeFixVerifier<
    AnalyzerTemplate.AnalyzerI7Yield,
    AnalyzerTemplate.CodeFixProviderI7Yield>;

namespace AnalyzerTemplate.Test
{
    [TestClass]
    public class AnalyzerI7YieldUnitTests
    {
        [TestMethod]
        public async Task TestYieldReturnsInt()
        {
            var test = @"using System.Collections.Generic;
public class Bangladesh
{
    public IEnumerator<int> Method()
    {
        yield return 0;
    }
}";

            await VerifyCS.VerifyAnalyzerAsync(test);
        }


        [TestMethod]
        public async Task TestYieldReturnsNull()
        {
            var test = @"using System;
using System.Collections.Generic;
public class Tehran
{
    public IEnumerator<Object> Method()
    {
        yield return [|null|];
    }
}";
            var fixtest = @"using System;
using System.Collections.Generic;
public class Tehran
{
    public IEnumerator<Object> Method()
    {
        yield break;
    }
}";

            await VerifyCS.VerifyCodeFixAsync(test, fixtest);
        }
    }
}