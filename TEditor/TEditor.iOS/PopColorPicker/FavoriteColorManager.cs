﻿using Foundation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;


namespace PopColorPicker.iOS
{
    public class FavoriteColorManager
    {
        private readonly string path;

        public FavoriteColorManager()
        {
            var documents = NSFileManager.DefaultManager.GetUrls(NSSearchPathDirectory.DocumentDirectory, NSSearchPathDomain.User)[0];
            path = Path.Combine(documents.Path, "FavoriteColors.txt");
        }

        public void Add(string colorText, bool rewriter = false)
        {
            var fileModel = rewriter == true ? FileMode.Create : FileMode.Append;

            using (var file = new FileStream(path, fileModel, FileAccess.Write, FileShare.ReadWrite))
            {
                using (var writer = new StreamWriter(file))
                {
                    writer.WriteLine(colorText);
                    writer.Flush();
                }
            }
        }

        public List<string> List()
        {
            List<string> list;

            using (var file = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                using (var reader = new StreamReader(file))
                {
                    var content = reader.ReadToEnd();
                    list = content.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList();
                }
            }

            return list;
        }

        public void Delete(string colorText)
        {
            List<string> list;

            using (var file = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                using (var reader = new StreamReader(file))
                {
                    var content = reader.ReadToEnd();

                    list = content.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    list.Remove(colorText);
                }
            }

            Add(string.Join(Environment.NewLine, list), true);
        }
    }
}