using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MathNet.Numerics.LinearAlgebra.Double;
using Somnium.Core;

namespace Somnium.Data
{
    public abstract class DataReader
    {
        public string FilePath { set; get; }
        public abstract string GetActualLabel(string path);
        public abstract Matrix GetMatrixData(string path);

        public StreamData ReadStreamData(string path)
        {
            FilePath = path;
            return new StreamData(GetMatrixData(path), GetActualLabel(path));
        }

        public static List<Type> GetDataReaders()
        {
            var types = Assembly.GetExecutingAssembly().GetTypes()
                .Where(a => a.BaseType == typeof(DataReader)&&
                            a.IsPublic).ToList();
            return types;
        }

    }
}