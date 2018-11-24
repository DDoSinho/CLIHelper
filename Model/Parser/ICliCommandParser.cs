using Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Parser
{
    public interface ICliCommandParser
    {
        IList<Command> Deserialize(string jsonFilename);
    }
}
