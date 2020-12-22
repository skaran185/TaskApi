using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AllModels
{
	public class Student
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Class { get; set; }
		public Boolean IsDeleted { get; set; }

		public virtual List<Subject> Subjects { get; set; }
	}
}
