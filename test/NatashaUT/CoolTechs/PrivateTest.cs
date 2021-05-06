using Natasha.CSharp;
using NatashaUT.Model;
using System.Collections.Generic;
using System.Net.Http;
using System.Reflection;
using Xunit;

namespace NatashaUT
{
    [Trait("新科技测试", "私有调用")]
    public class PrivateTest : PrepareTest
    {

        [Fact(DisplayName = "私有成员调用")]
        public void Test()
        {
            TPropertyClass test = new TPropertyClass();
            var action = NDelegate
#if !(NET472 || NET461 || NET462)
.RandomDomain()
#else
.DefaultDomain()
#endif

                .SetClass(item=>item.AllowPrivate<TPropertyClass>())
                .Action<TPropertyClass>("obj.publicA = 2; obj.privateA=1;");
            action(test);
            Assert.Equal(1, test.GetD);
        }

        
        [Fact(DisplayName = "私有成员调用0")]
        public void Test0()
        {

            var action = NDelegate
                .RandomDomain(item => item.UseShareLibraries = true)
                .SetClass(item => item.AllowPrivate<List<int>>())
                .Func<int>("return (new List<int>())._size;");
            Assert.Equal(0,action());

        }

        [Fact(DisplayName = "私有成员调用1")]
        public void Test1()
        {

            var domain = DomainComponent.Random;
            domain.AddReferencesFromDllFile(typeof(List<>).Assembly.Location);
            var action = NDelegate
                .UseDomain(domain,item=>item.UseShareLibraries = true)
                .SetClass(item => item.AllowPrivate<List<int>>())
                .Func<int>("return (new List<int>())._size;");
            Assert.Equal(0, action());

        }

        [Fact(DisplayName = "私有成员调用3")]
        public void Test2()
        {
            HttpRequestMessage test = new HttpRequestMessage();
            var action = NDelegate
                .RandomDomain(item => item.UseShareLibraries = true)
                .SetClass(item => item.AllowPrivate<HttpRequestMessage>())
                .Action<HttpRequestMessage>("obj._sendStatus = 4;");
            action(test);
            var _state = typeof(HttpRequestMessage).GetField("_sendStatus", BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.Equal(4, _state.GetValue(test));
        }



    }
}
