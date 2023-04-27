using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Json;


namespace Lab5
{
    internal class University
    {
        public List<Teacher> Teachers { get; set; }
        public List<Student> Students { get; set; }
        public List<Subject> Subjects { get; set; }

        public University()
        {
            Teachers = new List<Teacher>();
            Students = new List<Student>();
            Subjects = new List<Subject>();
        }

        // метод періоду вступної компанії
        public void StartAdmissionCamping()
        {
            int countOfStudents = 25;
            AddStudents(countOfStudents);   // зарахування студентів
            ExpelStudents();                // відрахування студентів
        }

        // метод періоду відпусток
        public void StartVacationPeriod() 
        {
            int countOfSubjects = 2;
            RemoveSubjects();
            AddSubjects(countOfSubjects);
            ExpelTeachers();         
        }

        // метод періоду навчання
        public void StartClasses()
        {
            int countOfTeachers = 2;
            AddTeachers(countOfTeachers);
        }

        // метод для додавання випадкових даних про студентів
        public void AddStudents(int count)
        {
            // список існуючих імен для рандомної вибірки
            var names = new List<string>()
            {
                "Adam", "Alex", "Aaron", "Benjamin", "Connor", "Cameron", "David", "Daniel",
                "Ethan", "Edward", "Fred", "Frank", "George", "Gerald", "Hank", "Henry",
                "Ivan", "Isaac", "Jerry", "Jack", "Kevin", "Kenneth", "Larry", "Leonard",
                "Michael", "Matthew", "Mark", "Nathan", "Nicholas", "Oliver", "Oscar", "Peter",
                "Patrick", "Paul", "Quentin", "Robert", "Richard", "Roger", "Samuel", "Seth",
                "Thomas", "Timothy", "Ulysses", "Victor", "Vincent", "William", "Wesley", "Xavier",
                "Zachary", "Zeke"
            };

            var rnd = new Random();

            // створення потрібної кількості студентів
            for (int i = 0; i < count; i++)
            {
                var name = names[rnd.Next(names.Count)];
                var student = new Student(name);

                var numSubjects = rnd.Next(1, Subjects.Count);
                var subjects = Subjects.OrderBy(x => rnd.Next()).Take(numSubjects).ToList();
                student.Subjects.AddRange(subjects);

                Students.Add(student);
            }
        }

        // метод для відрахування студентів
        public void ExpelStudents()
        {
            if (Students.Count > 1)
            {
                Random random = new Random();
                int numberToExpel = random.Next(1, 20);

                for (int i = 0; i < numberToExpel; i++)
                {
                    Students.RemoveAt(random.Next(Students.Count));
                }
            }
        }


        // метод для додавання випадкових даних про викладачів
        public void AddTeachers(int count)
        {
            // список існуючих імен для рандомної вибірки
            var names = new List<string>()
            {
                "Adam", "Alex", "Aaron", "Benjamin", "Connor", "Cameron", "David", "Daniel",
                "Ethan", "Edward", "Fred", "Frank", "George", "Gerald", "Hank", "Henry",
                "Ivan", "Isaac", "Jerry", "Jack", "Kevin", "Kenneth", "Larry", "Leonard",
                "Michael", "Matthew", "Mark", "Nathan", "Nicholas", "Oliver", "Oscar", "Peter",
                "Patrick", "Paul", "Quentin", "Robert", "Richard", "Roger", "Samuel", "Seth",
                "Thomas", "Timothy", "Ulysses", "Victor", "Vincent", "William", "Wesley", "Xavier",
                "Zachary", "Zeke"
            };

            var rnd = new Random();

            // створення потрібної кількості викладачів
            for (int i = 0; i < count; i++)
            {
                var name = names[rnd.Next(names.Count)];
                var teacher = new Teacher(name);

                Teachers.Add(teacher);
            }
        }

        // метод для звільнення викладачів
        public void ExpelTeachers()
        {
            if (Teachers.Count > 1)
            {
                Random random = new Random();
                int numberToExpel = random.Next(1, 2);

                for (int i = 0; i < numberToExpel; i++)
                {
                    Teachers.RemoveAt(random.Next(Teachers.Count));
                }
            }
        }

        // метод для додавання випадкових даних про предмети
        public void AddSubjects(int count)
        {
            // список існуючих назв для рандомної вибірки
            var names = new List<string>()
            {
                "Mathematics", "Physics", "Chemistry", "Biology", "History", "Geography", "English",
                "French", "German", "Spanish", "Computer Science", "Art", "Music", "Physical Education"
            };

            var rnd = new Random();

            // створення потрібної кількості предметів
            for (int i = 0; i < count; i++)
            {
                var name = names[rnd.Next(names.Count)];
                var subject = new Subject(name);

                Subjects.Add(subject);
            }
        }

        // метод видалення предметів
        public void RemoveSubjects()
        {
            if (Subjects.Count > 3)
            {
                Random random = new Random();
                int numberToExpel = random.Next(1, 2);

                for (int i = 0; i < numberToExpel; i++)
                {
                    Subjects.RemoveAt(random.Next(Subjects.Count));
                }
            }
        }


        // метод для збереження логу стану університету у файл
        public void SaveStateLog(string filePath)
        {
            // налаштування логування в файл в форматі JSON
            Log.Logger = new LoggerConfiguration().WriteTo.File(new JsonFormatter(), 
                filePath, rollingInterval: RollingInterval.Day).CreateLogger();

            // запис наявних даних у файл
            foreach (var student in Students)
            {
                Log.Information("Student: " + student.Name);
            }
            foreach (var teacher in Teachers)
            {
                Log.Information("Teacher: " + teacher.Name);
            }
            foreach (var subject in Subjects)
            {
                Log.Information("Subject: " + subject.Name);
            }

            // закриття логера 
            Log.CloseAndFlush();
        }
    }

    public class Teacher
    {
        public string Name { get; set; }
        public List<Subject> Subjects { get; set; }

        public Teacher(string name)
        {
            Name = name;
            Subjects = new List<Subject>();
        }
    }

    public class Student
    {
        public string Name { get; set; }
        public List<Subject> Subjects { get; set; }

        public Student(string name)
        {
            Name = name;
            Subjects = new List<Subject>();
        }
    }

    public class Subject
    {
        public string Name { get; set; }

        public Subject(string name)
        {
            Name = name;
        }
    }
}
