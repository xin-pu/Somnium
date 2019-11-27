using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MathNet.Numerics.LinearAlgebra.Double;
using Somnium.Core;

namespace Somnium.Data
{
    public class ImageLoad
    {
        public static InputLayer ReadLayerInput(string path, Func<string, InputLayer> func)
        {
            return func(path);
        }

        public static IList<InputLayer> ReadLayerInputs(string folder, Func<string, InputLayer> func)
        {
            var files = new DirectoryInfo(folder).GetFiles();
            return files.Select(a => ReadLayerInput(a.FullName, func)).ToList();
        }


    }
}
