	using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Models
{
	public abstract class CmdBase
	{
		public string Name { get; set; }

		public string Description { get; set; }

		public string Alias { get; set; }
	}
}
