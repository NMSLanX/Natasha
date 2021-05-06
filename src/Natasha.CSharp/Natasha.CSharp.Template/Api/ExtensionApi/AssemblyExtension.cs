using Natasha.CSharp;
using Natasha.Framework;
using System.Reflection;
#if !(NET472 || NET461 || NET462)
using System.Runtime.Loader;
#endif


public static class AssemblyExtension
{

    /// <summary>
    /// 创建一个程序集编译类
    /// </summary>
    /// <param name="name">程序集名字</param>
    /// <returns></returns>
    public static NAssembly CreateAssembly(this DomainBase domain, string name = default)
    {

        NAssembly result = new NAssembly(name);
        result.AssemblyBuilder.Compiler.Domain = domain;
        return result;

    }




    public static DomainBase GetDomain(this Assembly assembly)
    {
#if (NET472 || NET461 || NET462)
        return DomainComponent.Default;
#else
        var assemblyDomain = AssemblyLoadContext.GetLoadContext(assembly);
        if (assemblyDomain == AssemblyLoadContext.Default)
        {
            return DomainComponent.Default;
        }
        return (DomainBase)assemblyDomain;
#endif
    }




    public static void RemoveReferences(this Assembly assembly)
    {

        GetDomain(assembly).Remove(assembly);

    }



    public static void DisposeDomain(this Assembly assembly)
    {

        GetDomain(assembly).Dispose();

    }

}

