using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Models
{
	public class CLITool
	{
		public string Name { get; set; }

		public string ExecutableFile { get; set; }

		public List<Command> Commands { get; set; }
	}
}
