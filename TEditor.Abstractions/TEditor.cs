﻿using System;
using System.Reflection;
using System.IO;

namespace TEditor.Abstractions
{
    public partial class TEditor
    {
        public TEditor()
        {
            EditorLoaded = false;
            FormatHTML = false;
            InternalHTML = string.Empty;
            AutoFocusInput = false;
        }

        public string InternalHTML { get; set; }

        public bool EditorLoaded { get; set; }

        public bool FormatHTML { get; set; }

        public bool AutoFocusInput { get; set; }

        public string LoadResources()
        {
            var assembly = typeof(TEditor).GetTypeInfo().Assembly;
            var stream = assembly.GetManifestResourceStream("TEditor.Abstractions.EditorResources.editor.html");
            var htmlData = "";
            using (var reader = new StreamReader(stream, System.Text.Encoding.UTF8))
            {
                htmlData = reader.ReadToEnd();
            }
            var jsData = "";
            stream = assembly.GetManifestResourceStream("TEditor.Abstractions.EditorResources.ZSSRichTextEditor.js");
            using (var reader = new StreamReader(stream, System.Text.Encoding.UTF8))
            {
                jsData = reader.ReadToEnd();
            }
            return htmlData.Replace("<!--editor-->", jsData);
        }
    }
}

