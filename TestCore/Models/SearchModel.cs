
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestCore.Models
{
	public class SearchModel
	{
		public int Page { get; set; }
		public int PageSize { get; set; }
		public FilterType Type { get; set; }
		public string SearchString { get; set; }
	}

	public enum FilterType
	{
		Class=1,
		Subject
	}
}
