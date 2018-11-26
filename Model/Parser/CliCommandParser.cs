using System;
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
        public CLITool Deserialize(string jsonFilename)
        {
			string jsonFilePath = $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\CLIHelper\\{jsonFilename}.json";
            string json = File.ReadAllText(jsonFilePath);

            var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(json));
            var ser = new DataContractJsonSerializer(typeof(CLITool));
            var deserializedCommands = ser.ReadObject(memoryStream) as CLITool;
            memoryStream.Close();

            return deserializedCommands;
        }
    }
}
