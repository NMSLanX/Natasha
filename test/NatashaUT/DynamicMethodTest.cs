﻿using Natasha.CSharp;
using Natasha.CSharp.Operator;
using NatashaUT.Model;
using System;
using Xunit;

namespace NatashaUT
{
    [Trait("快速构建", "函数")]
    public class DynamicMethodTest
    {
        [Fact(DisplayName = "手动强转委托")]
        public static void RunDelegate1()
        {
            var delegateAction = FastMethodOperator
                .Default()
                        .Param<string>("str1")
                        .Param<string>("str2")
                        .Body(@"
                            string result = str1 +"" ""+ str2;
                            Console.WriteLine(result);
                            return result;")
                        .Return<string>()
                .Compile();

            string result = ((Func<string, string, string>)delegateAction)?.Invoke("Hello", "World1!");
            Assert.Equal("Hello World1!", result);
        }

        [Fact(DisplayName = "内部类委托")]
        public static void RunInnerDelegate()
        {
            var delegateAction = FastMethodOperator
                .Default()
                        .Body(@"
                           OopTestModel.InnerClass a = new OopTestModel.InnerClass();
                            a.Name =""abc"";
                            return a;")
                        .Return<OopTestModel.InnerClass>()
                .Compile();

            var result = ((Func<OopTestModel.InnerClass>)delegateAction)?.Invoke();
            Assert.Equal("abc", result.Name);
        }

        [Fact(DisplayName = "NFunc委托")]
        public static void RunDelegate5()
        {
            NSucceedLog.Enabled = true;
            Func<string, string, string> action = NDelegate.Random(builder => builder.UseFileCompile())
                .UnsafeFunc<string, string, string>(@"
                            string result = arg1 +"" ""+ arg2;
                            Console.WriteLine(result);
                            return result;");

            Func<string, string, string> action2 = NDelegate.Random(builder => builder.UseFileCompile())
                .UnsafeFunc<string, string, string>(@"
                            string result = arg1 + "" "" + arg2 + ""1"";
                            Console.WriteLine(result);
                            return result;");

            Assert.NotEqual(action.Method.GetHashCode(), action2.Method.GetHashCode());
            // Assert.NotEqual(action.GetHashCode(), action2.GetHashCode());

            Func<string, string, string> action3 = (s1, s2) => s1 + s2;

            string result = action("Hello", "World1!");
            Assert.Equal("Hello World1!", result);

            Assert.NotEqual(action3.Method.GetHashCode(), action2.Method.GetHashCode());
            //Assert.NotEqual(action3.GetHashCode(), action2.GetHashCode());
        }

        [Fact(DisplayName = "自动泛型委托")]
        public static void RunDelegate2()
        {
            var delegateAction = FastMethodOperator
                .Default()
                        .Param<string>("str1")
                        .Param<string>("str2")
                        .Body(@"
                            string result = str1 +"" ""+ str2;
                            Console.WriteLine(result);
                            return result;")
                        .Return<string>()
                .Compile<Func<string, string, string>>();

            string result = delegateAction?.Invoke("Hello", "World2!");
            Assert.Equal("Hello World2!", result);
        }
    }
}