using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VerifyCS = AnalyzerTemplate.Test.CSharpCodeFixVerifier<
    AnalyzerTemplate.AnalyzerO8,
    AnalyzerTemplate.CodeFixProviderO8>;

namespace AnalyzerTemplate.Test
{
    [TestClass]
    public class AnalyzerO8UnitTests
    {
        [TestMethod]
        public async Task TestMethodIsDeclaredInTheClass()
        {
            var test = @"using System;

public class Human
{
    public override string ToString()
    {
        return base.ToString();
    }
}

public class Runner
{
    public void Run()
    {
        var human = new Human();
        var s = human.ToString();
    }
}";

            await VerifyCS.VerifyAnalyzerAsync(test);
        }

        [TestMethod]
        public async Task TestMethodIsDeclaredInBaseClass()
        {
            var test = @"using System;

public class Human
{
    public override string ToString()
    {
        return base.ToString();
    }
}

public class Nagibator3001 : Human
{
    private short aboltus = 2;
}

public class Runner
{
    public void Run()
    {
        var nagiev = new Nagibator3001();
        var s = nagiev.ToString();
    }
}";

            await VerifyCS.VerifyAnalyzerAsync(test);
        }
        
        [TestMethod]
        public async Task TestMethodIsNotDeclaredAndBaseTypeIsObject()
        {
            var test = @"using System;

public class Human
{
}

public class Nagibator3001 : Human
{
    private short aboltus = 2;
}

public class Runner
{
    public void Run()
    {
        var nagiev = new Nagibator3001();
        var s = nagiev.[|ToString|]();
    }
}";

            var fixtest = @"using System;

public class Human
{
}

public class Nagibator3001 : Human
{
    private short aboltus = 2;
    public override string ToString()
    {
        return base.ToString();
    }
}

public class Runner
{
    public void Run()
    {
        var nagiev = new Nagibator3001();
        var s = nagiev.ToString();
    }
}";
            await VerifyCS.VerifyCodeFixAsync(test, fixtest);
        }
        
        [TestMethod]
        public async Task TestMethodIsNotDeclaredAndBaseTypeIsObjectButClassIsEmpty()
        {
            var test = @"using System;

class Human
{
}

class Nagibator3001 : Human
{
}

public class Runner
{
    public void Run()
    {
        var human = new Human();
        var s = human.[|ToString|]();
    }
}";

            var fixtest = @"using System;

class Human
{
    public override string ToString()
    {
        return base.ToString();
    }
}

class Nagibator3001 : Human
{
}

public class Runner
{
    public void Run()
    {
        var human = new Human();
        var s = human.ToString();
    }
}";
            await VerifyCS.VerifyCodeFixAsync(test, fixtest);
        }
    }
}