﻿// ***********************************************************************
// Assembly         : BancosBrasileiros.MergeTool
// Author           : Guilherme Branco Stracini
// Created          : 19/05/2020
//
// Last Modified By : Guilherme Branco Stracini
// Last Modified On : 05-05-2021
// ***********************************************************************
// <copyright file="Worker.cs" company="Guilherme Branco Stracini ME">
//     Copyright (c) Guilherme Branco Stracini ME. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace BancosBrasileiros.MergeTool
{
    using Helpers;
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Runtime.Serialization.Formatters.Binary;
    using CrispyWaffle.Extensions;

    /// <summary>
    /// Class Worker.
    /// </summary>
    internal class Worker
    {

        /// <summary>
        /// Works this instance.
        /// </summary>
        public void Work()
        {
            Console.WriteLine("Reading data files");

            var reader = new Reader();

            var source = reader.LoadBase();
            var str = reader.LoadStr();
            var slc = reader.LoadSlc();
            var pix = reader.LoadPix();

            var original = DeepCopy(source);

            Console.WriteLine($"Source: {source.Count} | STR: {str.Count} | SLC: {slc.Count} | PIX: {pix.Count} \r\n");

            new Seeder(source)
                .GenerateMissingDocument()
                .SeedStr(str)
                .SeedSlc(slc)
                .SeedPix(pix);

            foreach (var bank in source)
            {
                bank.DateRegistered ??= DateTimeOffset.Now;
                bank.DateUpdated ??= DateTimeOffset.Now;
            }

            var types = source.GroupBy(b => b.Type);

            foreach (var type in types.OrderBy(g => g.Key))
            {
                Console.WriteLine($"Type: {(string.IsNullOrWhiteSpace(type.Key) ? "-" : type.Key)} | Total: {type.Count()}");
            }

            source = source.Where(b => b.Ispb != 0 || b.Compe == 1).ToList();


            var except = source.Except(original).ToList();

            if (!except.Any())
            {
                Console.WriteLine("No new data or updated information");
                return;
            }

            Console.WriteLine($"\r\nUpdated items: {except.Count}\r\n");

            foreach (var exc in except)
            {
                Console.WriteLine($"Updated: {exc}");
            }

            Console.WriteLine("\r\nSaving result files");

            Writer.Save(source);

            Console.WriteLine($"Merge done. Banks: {source.Count} | Check 'result' folder in 'bin' directory!");

            var binDirectory = Directory.GetCurrentDirectory();
            var resultDirectory = Path.Combine(binDirectory, "result");
            Process.Start("explorer.exe", resultDirectory);
        }

        public static T DeepCopy<T>(T item)
        {
            var formatter = new BinaryFormatter();
            var stream = new MemoryStream();
            formatter.Serialize(stream, item);
            stream.Seek(0, SeekOrigin.Begin);
            var result = (T)formatter.Deserialize(stream);
            stream.Close();
            return result;
        }
    }
}
