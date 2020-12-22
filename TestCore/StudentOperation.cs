
using AllModels;
using Localdb;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using TestCore.Models;

namespace TestCore
{

	public class StudentOperation
	{
		private readonly StudentContext _context;
		public StudentOperation(StudentContext context)
		{
			_context = context;
		}

		#region Public methods

		/// <summary>
		///
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public ResponseModel AddStudent(StudentModel model)
		{
			ResponseModel response = new ResponseModel
			{
				Action = false
			};
			try
			{
				Student student = new Student
				{
					FirstName = model.FirstName,
					LastName = model.LastName,
					Class = model.Class,
					IsDeleted = false
				};

				student = SaveSubjectsData(model, student);

				_context.Student.Add(student);
				_context.SaveChanges();

				response.Action = true;
				response.Message = "Student added successfully";
				response.StatusCode = 200;
			}
			catch (Exception ex)
			{
				response.Message = ex.Message;
				response.StatusCode = 500;
			}
			return response;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>

		public ResponseModel UpdateStudent(StudentModel model)
		{
			ResponseModel response = new ResponseModel
			{
				Action = false
			};
			try
			{
				Student student = _context.Student.Where(b => b.Id == model.Id).Include(b => b.Subjects).FirstOrDefault();
				if (student != null)
				{
					student.FirstName = model.FirstName;
					student.LastName = model.LastName;
					student.Class = model.Class;

					foreach (var item in student.Subjects)
					{
						if (item != null)
						{
							_context.Subject.Remove(item);
						}
					}

					student = SaveSubjectsData(model, student);

					_context.SaveChanges();

					response.Action = true;
					response.Message = "Student updated successfully";
					response.StatusCode = 200;
				}
				else
				{
					response.Action = false;
					response.Message = "Data not found";
					response.StatusCode = 404;
				}
			}
			catch (Exception ex)
			{
				response.Message = ex.Message;
				response.StatusCode = 500;
			}
			return response;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="model"></param>
		/// <param name="student"></param>
		/// <returns></returns>
		private Student SaveSubjectsData(StudentModel model, Student student)
		{
			var subjects = model.Subject.Split(',');
			var marks = model.Marks.Split(',');
			if (subjects.Length > 0)
			{
				student.Subjects = new List<Subject>();
				for (int i = 0; i < subjects.Length; i++)
				{
					if (!string.IsNullOrEmpty(subjects[i]))
					{
						Subject subject = new Subject();
						subject.StudentId = student.Id;
						subject.SubjectName = subjects[i];
						subject.Marks = marks.Length > i ? marks[i] : subject.Marks = "0";
						subject.IsDeleted = false;
						student.Subjects.Add(subject);
					}
				}
			}
			return student;
		}
		#endregion
	}
}
