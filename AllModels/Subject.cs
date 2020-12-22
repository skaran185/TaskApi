using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AllModels
{
	public class Subject
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		public int StudentId { get; set; }
		public string SubjectName { get; set; }
		public string Marks { get; set; }
		public Boolean IsDeleted { get; set; }

		public virtual Student Student { get; set; }
	}
}
