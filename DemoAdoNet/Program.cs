using DemoAdoNet;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;
using System.Transactions;
using Microsoft.EntityFrameworkCore;

class Program
{
    private static String connectionString = "Data Source=DESKTOP-RG5G9U4;Initial Catalog=SMS;User ID=sa;Password=pa$$word; TrustServerCertificate = True";
    static void Main()
    {

        var context = new TeacherContext();
        var teachers = context.Teachers.ToList();

        foreach (var teacher in teachers)
        {
            Console.WriteLine("ID:" +teacher.TeacherId+ "Name:" +teacher.Name+ "Subject: " + teacher.Nationality);
        }

        
        StudentManager studentManager = new StudentManager();
        TeacherManager teacherManager = new TeacherManager();

        bool repeat = true;
        while (repeat)
        {

            Console.WriteLine("Select an option: ");
            Console.WriteLine("1. Student Operation");
            Console.WriteLine("2. Teacher Operation");
            Console.WriteLine("3. Exit");

            int choice = Convert.ToInt32(Console.ReadLine());

            switch (choice)
            {
                case 1:
                    Console.WriteLine("Select an option: ");
                    Console.WriteLine("1. Insert Student");
                    Console.WriteLine("2. Update Student");
                    Console.WriteLine("3. Delete Student");
                    Console.WriteLine("4. GelAll Student");
                    Console.WriteLine("5. Filter Student");
                    Console.WriteLine("6. Exit");

                    int choice1 = Convert.ToInt32(Console.ReadLine());


                    switch (choice1)
                    {
                        case 1:
                            AddNewStudent(studentManager);
                            Console.WriteLine("Enter any key to ontinue_");
                            Console.ReadKey();
                            break;

                        case 2:

                            UpdateStudent(studentManager);
                            Console.WriteLine("Enter any key to ontinue_");
                            Console.ReadKey();
                            break;

                        case 3:
                            // Delete
                            Console.WriteLine("Enter the ID : ");
                            int studentIdToDelete = Convert.ToInt32(Console.ReadLine());

                            studentManager.DeleteStudent(studentIdToDelete);
                            Console.WriteLine("Enter any key to ontinue_");
                            Console.ReadKey();

                            break;

                        case 4:
                            //GetAll
                            List<Student> s = studentManager.GetAllStudents();
                            foreach (Student student in s)
                            {
                                Console.WriteLine(student.Name);

                            }
                            Console.WriteLine("Enter any key to ontinue_");
                            Console.ReadKey();
                            break;

                        case 5:
                            //Filter
                            Console.WriteLine("Enter Name: ");
                            string name = Console.ReadLine();

                            List<Student> std = studentManager.GetStudentsByFilter(name);
                            foreach (Student student in std)
                            {
                                Console.WriteLine(student.StudentId);
                                Console.WriteLine(student.Name);
                            }
                            Console.WriteLine("Enter any key to ontinue_");
                            Console.ReadKey();

                            break;

                        case 6:
                            Console.WriteLine("Exited");
                            break;
                        default:
                            Console.WriteLine("Error");
                            break;

                    }
                    break;

                case 2:
                    Console.WriteLine("Select an option: ");
                    Console.WriteLine("1. Insert ");
                    Console.WriteLine("2. Update ");
                    Console.WriteLine("3. Join ");
                    Console.WriteLine("4. GelAll ");
                    Console.WriteLine("5. Filter ");
                    Console.WriteLine("6. Add Random Teacher");
                    Console.WriteLine("7. Pagination");
                    Console.WriteLine("8. Exit");

                    int choice2 = Convert.ToInt32(Console.ReadLine());
                    switch (choice2)
                    {
                        case 1:
                            AddTeacher(teacherManager);
                            Console.WriteLine("Enter any key to ontinue_");
                            Console.ReadKey();
                            break;

                        case 2:

                            UpdateTeacher(teacherManager);
                            Console.WriteLine("Enter any key to ontinue_");
                            Console.ReadKey();
                            break;

                        case 3:
                            List<Class> teac = studentManager.GetClassDetails();
                            Console.WriteLine("Class ID\tCourse Name\tNumberOfStudent\tTeacher Id\tTeacher Name\tTime");
                            Console.ResetColor();
                            foreach (Class tech in teac)
                            {
                                Console.WriteLine(tech.ClassId + "\t" + tech.CourseName + "\t" +tech.NumberOfStudents +"\t" + tech.TeacherId+ "\t"+tech.TeacherName+ "\t" +tech.Time);

                            }
                            Console.WriteLine("Enter any key to ontinue_");
                            Console.ReadKey();
                            break;

                        case 4:
                            //GetAll
                            List<Teacher> cls = teacherManager.GetAllTeacher();
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("TeacherID\tName\tPhone\tAddress\tbloodgroup\tNationality\tJoinDate");
                            Console.ResetColor();
                            foreach (Teacher tech in cls)
                            {
                                Console.WriteLine(tech.TeacherId + "\t" + tech.Name + "\t" + tech.Phone + "\t" + tech.Address);

                            }
                            Console.WriteLine("Enter any key to ontinue_");
                            Console.ReadKey();
                            break;

                        case 5:
                            //Filter
                            Console.WriteLine("Enter Name: ");
                            string name = Console.ReadLine();

                            List<Teacher> t = teacherManager.FilterByTeacher(name);
                            foreach (Teacher teacher1 in t)
                            {
                                Console.WriteLine("TeacherID\tName\tPhone\tAddress\tbloodgroup\tNationality\tJoinDate");
                                Console.WriteLine(teacher1.TeacherId+"\n"+teacher1.Name+"\n"+teacher1.Phone+"\n"+teacher1.Address);
                            }
                            Console.WriteLine("Enter any key to ontinue_");
                            Console.ReadKey();


                            break;

                        case 6:
                            AddRandomTeacher(teacherManager);
                            break;
                        case 7:
                            bool rpt = true;

                            while (rpt)
                            {
                                Console.WriteLine("Press 'n' to next");
                                var key = Console.ReadKey(intercept: true).Key;
                                if (key == ConsoleKey.N)
                                {
                                    Console.WriteLine("Enter pageSize:");
                                    int page = Convert.ToInt32(Console.ReadLine());
                                    int pageSize = 10;
                                    List<Teacher> cls1 = teacherManager.GetLimitTeacher(page, pageSize);
                                    Console.ForegroundColor = ConsoleColor.Yellow;
                                    Console.WriteLine("TeacherID\tName\tPhone\tAddress\tbloodgroup\tNationality\tJoinDate");
                                    Console.ResetColor();
                                    foreach (Teacher tech in cls1)
                                    {
                                        Console.WriteLine(tech.TeacherId + "\t" + tech.Name + "\t" + tech.Phone + "\t" + tech.Address);

                                    }
                                }
                                else
                                {
                                    throw new Exception("Error key");
                                }
                                //rpt = false;
                            }

                            //Pagination

                            //int pageSize = 10;

                            Console.WriteLine("Enter any key to ontinue_");
                            Console.ReadKey();

                            break;
                        case 8:
                            Console.WriteLine("Exited");
                            break;

                        default :
                            Console.WriteLine("Error");
                            break;
                    }
                break;
                    
                case 3:
                    Console.WriteLine("Exited");
                    repeat = false;
                    break;

                default:
                    Console.WriteLine("Error");
                    break;
            }

        }
        

    }

    private static void UpdateTeacher(TeacherManager teacherManager)
    {
        var tcr = new Teacher();

        Console.WriteLine("Enter Id: ");
        string tid = Console.ReadLine();
        if (int.TryParse(tid, out int x))
        {
            tcr.TeacherId = x;
        }
        else
        {
            Console.WriteLine("Invallid identity");
        }

        Console.WriteLine("Enter Name: ");
        tcr.Name = Console.ReadLine();

        Console.WriteLine("Enter Phone: ");
        tcr.Phone = Console.ReadLine();

        Console.WriteLine("Enter Phone: ");
        tcr.Phone = Console.ReadLine();

        Console.WriteLine("Enter Address: ");
        tcr.Address = Console.ReadLine();

        Console.WriteLine("Enter BloodGroup: ");
        tcr.BloodGroup = Console.ReadLine();

        Console.WriteLine("Enter Nationality: ");
        tcr.Nationality = Console.ReadLine();

        Console.WriteLine("Enter Date of Join: ");
        string joindate1 = Console.ReadLine();
        if (DateTime.TryParse(joindate1, out DateTime joinDate1))
        {
            tcr.JoinDate = joinDate1;
        }
        else
        {
            Console.WriteLine("Invallid date" + joindate1 + "");

        }

        teacherManager.UpdateTeacher(tcr);
    }

    private static void AddTeacher(TeacherManager teacherManager)
    {
        var teacher = new Teacher();

        Console.WriteLine("Enter Id: ");
        string id = Console.ReadLine();
        if (int.TryParse(id, out int i))
        {
            teacher.TeacherId = i;
        }
        else
        {
            Console.WriteLine("Invallid identity");
        }

        Console.WriteLine("Enter Name: ");
        teacher.Name = Console.ReadLine();

        Console.WriteLine("Enter Phone: ");
        teacher.Phone = Console.ReadLine();

        Console.WriteLine("Enter Address: ");
        teacher.Address = Console.ReadLine();

        Console.WriteLine("Enter BloodGroup: ");
        teacher.BloodGroup = Console.ReadLine();

        Console.WriteLine("Enter Nationality: ");
        teacher.Nationality = Console.ReadLine();

        Console.WriteLine("Enter Date of Join: ");
        string joindate = Console.ReadLine();
        if (DateTime.TryParse(joindate, out DateTime joinDate))
        {
            teacher.JoinDate = joinDate;
        }
        else
        {
            Console.WriteLine("Invallid date" + joindate + "");

        }

        teacherManager.AddTeacher(teacher);
    }

    private static void AddRandomTeacher(TeacherManager teacherManager)
    {
        for (int i = 0; i < 50; i++)
        {
            var teacher = new Teacher
            {
                Name = "New Teacher " + i,
                Address = "New Teacher Address " + i,
                BloodGroup = "A+ " + i,
                JoinDate = DateTime.Now.AddDays(i),
                Nationality = "BD " + i,
                Phone = "019171" + i
            };

            teacherManager.AddTeacher(teacher);

            Console.WriteLine("Teacher created: " + (i + 1));
        }
    }

    private static void UpdateStudent(StudentManager studentManager)
    {
 
        var newStd = new Student();
        Console.WriteLine("Enter Id: ");
        string id = Console.ReadLine();
        if (int.TryParse(id, out int i))
        {
            newStd.StudentId = i;
        }
        else
        {
            Console.WriteLine("Invallid identity");
        }

        Console.WriteLine("Enter Name: ");
        newStd.Name = Console.ReadLine();

        Console.WriteLine("Enter Email: ");
        newStd.Email = Console.ReadLine();

        Console.WriteLine("Enter Phone: ");
        newStd.Phone = Console.ReadLine();

        Console.WriteLine("Enter fatherName: ");
        newStd.FatherName = Console.ReadLine();

        Console.WriteLine("Enter Name: ");
        newStd.MotherName = Console.ReadLine();

        Console.WriteLine("Enter Date of Birth: ");
        string dob = Console.ReadLine();
        if (DateTime.TryParse(dob, out DateTime date))
        {
            newStd.Dob = date;
        }
        else
        {
            Console.WriteLine("Invallid date" + date + "");
        }

        Console.WriteLine("Enter Nationality: ");
        newStd.Nationality = Console.ReadLine();

        Console.WriteLine("Enter Date of Join: ");
        string joindate = Console.ReadLine();
        if (DateTime.TryParse(joindate, out DateTime joinDate))
        {
            newStd.JoinDate = joinDate;
        }
        else
        {
            Console.WriteLine("Invallid date" + date + "");

        }

        //newStd.JoinDate = Convert.ToDateTime(Console.ReadLine());
        Console.WriteLine("Enter Status: ");
        string status = Console.ReadLine();
        if (bool.TryParse(status, out bool sts))
        {
            newStd.Status = sts;
        }
        else
        {
            Console.WriteLine("Invalid Status");
        }
    }

    private static void AddNewStudent(StudentManager studentManager)
    {
        var newStd = new Student();

        Console.WriteLine("Enter Id: ");
        string id = Console.ReadLine();
        if (int.TryParse(id, out int i))
        {
            newStd.StudentId = i;
        }
        else
        {
            Console.WriteLine("Invallid identity");
        }

        Console.WriteLine("Enter Name: ");
        newStd.Name = Console.ReadLine();

        Console.WriteLine("Enter Email: ");
        newStd.Email = Console.ReadLine();

        Console.WriteLine("Enter Phone: ");
        newStd.Phone = Console.ReadLine();

        Console.WriteLine("Enter fatherName: ");
        newStd.FatherName = Console.ReadLine();

        Console.WriteLine("Enter Name: ");
        newStd.MotherName = Console.ReadLine();

        Console.WriteLine("Enter Date of Birth: ");
        string dob = Console.ReadLine();
        if (DateTime.TryParse(dob, out DateTime date))
        {
            newStd.Dob = date;
        }
        else
        {
            Console.WriteLine("Invallid date" + date + "");
        }

        Console.WriteLine("Enter Nationality: ");
        newStd.Nationality = Console.ReadLine();

        Console.WriteLine("Enter Date of Join: ");
        string joindate = Console.ReadLine();
        if (DateTime.TryParse(joindate, out DateTime joinDate))
        {
            newStd.JoinDate = joinDate;
        }
        else
        {
            Console.WriteLine("Invallid date" + date + "");

        }

        //newStd.JoinDate = Convert.ToDateTime(Console.ReadLine());
        Console.WriteLine("Enter Status: ");
        string status = Console.ReadLine();
        if (bool.TryParse(status, out bool sts))
        {
            newStd.Status = sts;
        }
        else
        {
            Console.WriteLine("Invalid Status");
        }
    }
}