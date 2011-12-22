﻿using System.IO;

namespace Bddify.Reporters
{
    public static class LazyFileLoader
    {
        static string _cssFile;
        static string _jqueryFile;
        static string _myJsFile;

        public static string BddifyCssFile
        {
            get
            {
                if (_cssFile == null)
                {
                    _cssFile = GetEmbeddedFileResource("Bddify.Reporters.bddify.css");
                }

                return _cssFile;
            }
        }

        public static string JQueryFile
        {
            get
            {
                if (_jqueryFile == null)
                {
                    _jqueryFile = GetEmbeddedFileResource("Bddify.Reporters.jquery-1.7.1.min.js");
                }
                return _jqueryFile;
            }
        }

        public static string BddifyJsFile
        {
            get
            {
                if (_myJsFile == null)
                {
                    _myJsFile = GetEmbeddedFileResource("Bddify.Reporters.bddify.js");
                }

                return _myJsFile;
            }
        }

        static string GetEmbeddedFileResource(string fileResourceName)
        {
            string fileContent;
            var templateResourceStream = typeof(LazyFileLoader).Assembly.GetManifestResourceStream(fileResourceName);
            using (var sr = new StreamReader(templateResourceStream))
            {
                fileContent = sr.ReadToEnd();
            }

            return fileContent;
        }
    }
}