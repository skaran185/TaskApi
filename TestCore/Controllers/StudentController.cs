using AllModels;
using Localdb;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using TestCore.Models;

namespace TestCore.Controllers
{
	[Route("api/student")]
	[ApiController]
	public class StudentController : ControllerBase
	{
		private readonly StudentContext context;
		public StudentOperation _studentOperation;
		public StudentController(StudentContext studentContext)
		{
			context = studentContext;
			_studentOperation = new StudentOperation(context);
		}

		/// <summary>
		///
		/// </summary>
		/// <returns></returns>
		[Route("Test")]
		public IActionResult TestApi()
		{
			return Ok("Api working. . .");
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="filters"></param>
		/// <returns></returns>
		[HttpPost]
		[Route("GetAllStudents")]
		public IActionResult GetAllStudents(SearchModel filters)
		{
			int skipData = filters.PageSize * (filters.Page - 1);

			ResponseModel response = new ResponseModel
			{
				Action = false
			};
			try
			{
				List<Student> data = context.Student.AsNoTracking().AsQueryable().Where(b => !b.IsDeleted).Include(b => b.Subjects).OrderByDescending(b => b.Id).ToList();

				filters.SearchString = filters.SearchString.ToLower();

				switch (filters.Type)
				{
					case FilterType.Class:
						{
							if (!string.IsNullOrEmpty(filters.SearchString))
								data = data.Where(v => v.Class.ToLower().Contains(filters.SearchString)).ToList();
							break;
						}
					case FilterType.Subject:
						{
							if (!string.IsNullOrEmpty(filters.SearchString))
							{
								data = data.Where(v => v.Subjects.Any(c => c.SubjectName.ToLower().Contains(filters.SearchString))).ToList();
							}
							break;
						}
					default:
						{
							if (!string.IsNullOrEmpty(filters.SearchString))
								data = data.Where(v => v.FirstName.ToLower().Contains(filters.SearchString) || v.LastName.ToLower().Contains(filters.SearchString)).ToList();
							break;
						}
				}

				var res = new
				{
					data = data.Skip(skipData).Take(filters.PageSize).ToList(),
					count = data.Count
				};

				return Ok(res);
			}
			catch (Exception ex)
			{
				response.Message = ex.Message;
				response.StatusCode = 500;
			}
			return Ok(response);
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpGet]
		[Route("GetStudent")]
		public IActionResult GetById(int id)
		{
			ResponseModel response = new ResponseModel
			{
				Action = false
			};
			try
			{
				var data = context.Student.Where(b => b.Id == id && !b.IsDeleted).Include(v => v.Subjects).Select(c => new StudentModel
				{
					Class = c.Class,
					FirstName = c.FirstName,
					Id = c.Id,
					LastName = c.LastName,
					Marks = string.Join(",", c.Subjects.Where(b => !b.IsDeleted).Select(v => v.Marks).ToList()),
					Subject = string.Join(",", c.Subjects.Where(b => !b.IsDeleted).Select(v => v.SubjectName).ToList()),
				}).FirstOrDefault();
				return Ok(data);

			}
			catch (Exception ex)
			{
				response.Message = ex.Message;
				response.StatusCode = 500;
			}
			return Ok(response);
		}


		/// <summary>
		///
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpPost]
		[Route("PostStudent")]
		public IActionResult PostStudent(StudentModel model)
		{
			return model.Id > 0 ? Ok(_studentOperation.UpdateStudent(model)) : Ok(_studentOperation.AddStudent(model));
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpDelete]
		[Route("DeleteStudent")]
		public IActionResult DeleteStudent(int id)
		{
			ResponseModel response = new ResponseModel
			{
				Action = false
			};
			try
			{
				Student student = context.Student.Where(b => b.Id == id).Include(v => v.Subjects).FirstOrDefault();
				if (student != null)
				{
					student.IsDeleted = true;

					foreach (var item in student.Subjects)
					{
						item.IsDeleted = true;
					}
					context.SaveChanges();

					response.Action = true;
					response.Message = "Student deleted successfully";
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
			return Ok(response);
		}

	}

}