using Microsoft.CodeAnalysis.CSharp;
using Microsoft.Extensions.DependencyModel;
using Natasha.CSharpEngine;
using Natasha.Framework;
using System;
using System.Linq;

public static class NatashaComponentRegister
{

    public static void RegistCompiler<TCompiler>() where TCompiler : CompilerBase<CSharpCompilation, CSharpCompilationOptions>, new()
    {
        CompilerComponent.RegisterDefault<TCompiler>();
    }

    public static void RegistDomain<TDomain>(bool initializeReference) where TDomain : DomainBase
    {
        DomainComponent.CreateDomain = DomainComponent.RegisterDefault<TDomain>();
        DomainComponent.Default = DomainComponent.CreateDomain("Default");

        if (initializeReference)
        {

#if (NET472 || NET461 || NET462)
                        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
                        foreach (var item in assemblies)
                        {
                            try
                            {
                                if (!string.IsNullOrEmpty(item.Location))
                                {
                                    DomainComponent.Default.AddReferencesFromDllFile(item.Location);
                                }
                            }
                            catch (Exception ex)
                            {

                            }

                        }
#else
            foreach (var asm in DependencyContext
           .Default
           .CompileLibraries
           .SelectMany(cl => cl.ResolveReferencePaths()))
            {
                DomainComponent.Default.AddReferencesFromDllFile(asm);
            }
#endif

        }
    }

    public static void RegistSyntax<TSyntax>() where TSyntax : SyntaxBase, new()
    {
        SyntaxComponent.RegisterDefault<TSyntax>();
    }

}

