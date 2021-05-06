using Microsoft.Extensions.DependencyModel;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Natasha
{

    public static class GlobalUsing
    {

        private readonly static HashSet<string> DefaultNamesapce;
        public readonly static StringBuilder DefaultScript;

        static GlobalUsing()
        {

            DefaultScript = new StringBuilder();
            DefaultNamesapce = new HashSet<string>();
#if !(NET472 || NET461 || NET462)
            var assemblyNames = DependencyContext.Default.GetDefaultAssemblyNames();
            foreach (var name in assemblyNames)
            {

                var assembly = Assembly.Load(name);
                if (assembly != default)
                {

                    try
                    {

                        var types = assembly.ExportedTypes;
                        foreach (var item in types)
                        {

                            if (!DefaultNamesapce.Contains(item.Namespace) && item.Namespace != default)
                            {
                                DefaultNamesapce.Add(item.Namespace);
                            }

                        }

                    }
                    catch (Exception)
                    {



                    }


                }

            }

            var entryTypes = Assembly.GetEntryAssembly().GetTypes();
            foreach (var item in entryTypes)
            {

                if (!DefaultNamesapce.Contains(item.Namespace) && item.Namespace != default)
                {
                    DefaultNamesapce.Add(item.Namespace);
                }

            }

            foreach (var @using in DefaultNamesapce)
            {
                DefaultScript.AppendLine($"using {@using};");
            }
#else
            var assembies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var item in assembies)
            {
                try
                {
                    var entryTypes = item.GetTypes();
                    foreach (var type in entryTypes)
                    {
                        if (!DefaultNamesapce.Contains(type.Namespace) && type.Namespace != default)
                        {
                            DefaultNamesapce.Add(type.Namespace);
                            DefaultScript.AppendLine($"using {type.Namespace};");
                        }

                    }
                }
                catch
                {

 
                }
                

            }
#endif



        }



        public static bool HasElement(string @namespace)
        {
            lock (DefaultNamesapce)
            {
                return DefaultNamesapce.Contains(@namespace);
            }
        }

        public static void Remove(string @namespace)
        {
            lock (DefaultNamesapce)
            {
                if (DefaultNamesapce.Contains(@namespace))
                {
                    DefaultNamesapce.Remove(@namespace);
                    DefaultScript.Replace($"using {@namespace};{Environment.NewLine}", string.Empty);
                }
            }
        }

    }

}
