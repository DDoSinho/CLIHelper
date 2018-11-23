using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Models
{
	public class Command : CmdBase
	{
		public List<Option> Options { get; set; }

		public List<Argument> Arguments { get; set; }
	}
}
