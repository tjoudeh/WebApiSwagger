using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using WebApiSwagger.Models;

namespace WebApiSwagger.Controllers
{
    [RoutePrefix("api/Students")]
    public class StudentsController : ApiController
    {

        private static List<Student> StudentsList;

        public StudentsController()
        {
            if (StudentsList == null)
            {
                StudentsList = StudentsData.CreateStudents();
            }

        }

        /// <summary>
        /// Get all students
        /// </summary>
        /// <remarks>Get an array of all students</remarks>
        /// <response code="500">Internal Server Error</response>
        [Route("")]
        [ResponseType(typeof(List<Student>))]
        public IHttpActionResult Get()
        {
            return Ok(StudentsList);
        }

        /// <summary>
        /// Get student
        /// </summary>
        /// <param name="userName">Unique username</param>
        /// <remarks>Get signle student by providing username</remarks>
        /// <response code="404">Not found</response>
        /// <response code="500">Internal Server Error</response>
        [Route("{userName:alpha}", Name = "GetStudentByUserName")]
        [ResponseType(typeof(Student))]
        public IHttpActionResult Get(string userName)
        {

            var student = StudentsList.Where(s => s.UserName == userName).FirstOrDefault();

            if (student == null)
            {
                return NotFound();
            }

            return Ok(student);
        }

        /// <summary>
        /// Add new student
        /// </summary>
        /// <param name="student">Student Model</param>
        /// <remarks>Insert new student</remarks>
        /// <response code="400">Bad request</response>
        /// <response code="500">Internal Server Error</response>
        [Route("")]
        [ResponseType(typeof(Student))]
        public IHttpActionResult Post(Student student)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (StudentsList.Any(s => s.UserName == student.UserName))
            {
                return BadRequest("Username already exists");
            }

            StudentsList.Add(student);

            string uri = Url.Link("GetStudentByUserName", new { userName = student.UserName });

            return Created(uri, student);
        }

        /// <summary>
        /// Delete student
        /// </summary>
        /// <param name="userName">Unique username</param>
        /// <remarks>Delete existing student</remarks>
        /// <response code="404">Not found</response>
        /// <response code="500">Internal Server Error</response>
        [Route("{userName:alpha}")]
        public HttpResponseMessage Delete(string userName)
        {

            var student = StudentsList.Where(s => s.UserName == userName).FirstOrDefault();

            if (student == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            StudentsList.Remove(student);

            return Request.CreateResponse(HttpStatusCode.NoContent);

        }

    }

    public class StudentsData
    {
        public static List<Student> CreateStudents()
        {

            List<Student> studentsList = new List<Student>();

            for (int i = 0; i < studentNames.Length; i++)
            {
                var nameGenderMail = SplitValue(studentNames[i]);
                var student = new Student()
                {
                    Email = String.Format("{0}.{1}@{2}", nameGenderMail[0], nameGenderMail[1], nameGenderMail[3]),
                    UserName = String.Format("{0}{1}", nameGenderMail[0], nameGenderMail[1]),
                    FirstName = nameGenderMail[0],
                    LastName = nameGenderMail[1],
                    DateOfBirth = DateTime.UtcNow.AddDays(-new Random().Next(7000, 8000)),
                };

                studentsList.Add(student);
            }

            return studentsList;
        }

        static string[] studentNames = 
        { 
            "Taiseer,Joudeh,Male,hotmail.com", 
            "Hasan,Ahmad,Male,mymail.com", 
            "Moatasem,Ahmad,Male,outlook.com", 
            "Salma,Tamer,Female,outlook.com", 
            "Ahmad,Radi,Male,gmail.com", 
            "Bill,Gates,Male,yahoo.com", 
            "Shareef,Khaled,Male,gmail.com", 
            "Aram,Naser,Male,gmail.com", 
            "Layla,Ibrahim,Female,mymail.com", 
            "Rema,Oday,Female,hotmail.com",
            "Fikri,Husein,Male,gmail.com",
            "Zakari,Husein,Male,outlook.com",
            "Taher,Waleed,Male,mymail.com",
            "Tamer,Wesam,Male,yahoo.com",
            "Khaled,Hasaan,Male,gmail.com",
            "Asaad,Ibrahim,Male,hotmail.com",
            "Tareq,Nassar,Male,outlook.com",
            "Diana,Lutfi,Female,outlook.com",
            "Tamara,Malek,Female,gmail.com",
            "Arwa,Kamal,Female,yahoo.com",
            "Jana,Ahmad,Female,yahoo.com",
            "Nisreen,Tamer,Female,gmail.com",
            "Noura,Ahmad,Female,outlook.com"
        };

        private static string[] SplitValue(string val)
        {
            return val.Split(',');
        }
    }
}
