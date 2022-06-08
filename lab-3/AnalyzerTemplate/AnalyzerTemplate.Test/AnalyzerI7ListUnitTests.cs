using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VerifyCS = AnalyzerTemplate.Test.CSharpCodeFixVerifier<
    AnalyzerTemplate.AnalyzerI7List,
    AnalyzerTemplate.CodeFixProviderI7List>;

namespace AnalyzerTemplate.Test
{
    [TestClass]
    public class AnalyzerI7ListUnitTests
    {
        [TestMethod]
        public async Task TestIEnumerableMethodReturnsList()
        {
            var test = @"using System.Collections.Generic;
    public class TommyVercetti
    {
        public IEnumerable<double> Method()
        {
            return new List<double>();
        }
    }";

            await VerifyCS.VerifyAnalyzerAsync(test);
        }


        [TestMethod]
        public async Task TestIEnumerableMethodReturnsNull()
        {
            var test = @"using System.Collections.Generic;
    public class AstonVilla
    {
        public IEnumerable<double> Method()
        {
            return [|null|];
        }
    }";
            var fixtest = @"using System.Collections.Generic;
    public class AstonVilla
    {
        public IEnumerable<double> Method()
        {
            return new List<double>();
        }
    }";

            await VerifyCS.VerifyCodeFixAsync(test, fixtest);
        }

        [TestMethod]
        public async Task TestFullListTypeMethodReturnsNull()
        {
            var test = @"using System.Collections.Generic;
    public class Outlaw
    {
        public System.Collections.Generic.List<int> Method()
        {
            return [|null|];
        }
    }";

            var fixtest = @"using System.Collections.Generic;
    public class Outlaw
    {
        public System.Collections.Generic.List<int> Method()
        {
            return new List<int>();
        }
    }";

            await VerifyCS.VerifyCodeFixAsync(test, fixtest);
        }
    }
}