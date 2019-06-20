﻿using System;

namespace NatashaUT.Model
{
    public static class StaticTestModel
    {
        public static int Age;
        public static string Name { get; set; }

        public static DateTime Temp;

        public static float Money;
    }

    public class FakeStaticTestModel
    {
        public static int Age;
        public static string Name { get; set; }

        public static DateTime Temp;

        public static float Money;
    }
}