﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Model.Models;

namespace Model.Parser
{
    public class CliCommandParser : ICliCommandParser
    {
        public IList<Command> Deserialize(string jsonFilename)
        {
            string jsonFilePath = Environment.CurrentDirectory + @"\" + jsonFilename + ".json";
            string json = File.ReadAllText(jsonFilePath);

            var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(json));
            var ser = new DataContractJsonSerializer(typeof(List<Command>));
            var deserializedCommands = ser.ReadObject(memoryStream) as List<Command>;
            memoryStream.Close();

            return deserializedCommands;
        }
    }
}