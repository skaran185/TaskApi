using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestCore
{
    public class StudentModel
    {
		public int Id { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Class { get; set; }
		public string Subject { get; set; }
		public string Marks { get; set; }
	}
}
