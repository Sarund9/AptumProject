using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Veldrid;

namespace AptumEngine.Core
{
    public class ShaderLoader
    {
        #region SINGLETON
        static ShaderLoader _i;
        private ShaderLoader(){}
        static ShaderLoader I
        {
            get
            {
                if (_i is null)
                    _i = new ShaderLoader();
                return _i;
            }
        }
        #endregion

        const string FILE_EX = "spv";

        string s_ShaderPath;

        public static string ShaderPath
        {
            get => I.s_ShaderPath;
            set => I.s_ShaderPath = value;
        }

        public static string LoadFrom(string relPath)
        {
            string fullPath = $"{I.s_ShaderPath}/{relPath}/.{FILE_EX}";

            if (File.Exists(fullPath))
            {
                return File.ReadAllText(fullPath);
            }
            else return null;
        }

    }

    static class Assets
    {
        static Stream GetEmbbededResourceStream(string name) => typeof(Assets).Assembly.GetManifestResourceStream(name);

        public static byte[] ReadEmbeddedAssetBytes(string name)
        {
            using (Stream stream = GetEmbbededResourceStream(name))
            {
                byte[] bytes = new byte[stream.Length];
                using (MemoryStream ms = new MemoryStream(bytes))
                {
                    stream.CopyTo(ms);
                    return bytes;
                }
            }
        }
    }
}
