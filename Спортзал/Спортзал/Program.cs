using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.RegularExpressions;
using System.Transactions;
using System.Xml.Linq;

namespace Sportzal
{
    class Trainer
    {
        public string FullName;
        public string Specialization;
        public string Qualification;
        public int Fee;
        public double Salary;
        public double ClassDuration;

        public Trainer(string FullName, string Specialization, string Qualification, int Fee)
        {
            this.FullName = FullName;
            this.Specialization = Specialization;
            this.Qualification = Qualification;
            this.Fee = Fee;
        }

        public void Add()
        {
            StreamWriter sw = new StreamWriter("trainer.txt", true);
            sw.WriteLine(FullName);
            sw.WriteLine(Specialization);
            sw.WriteLine(Qualification);
            sw.WriteLine(Fee);
            sw.WriteLine("");
            sw.Close();
        }
    }

    class Group
    {
        public int GroupNumber;
        public string TypeOfOccupation;
        public int Age;
        public string Status;

        public Group(int GroupNumber, string TypeOfOccupation, int Age, string Status)
        {
            this.GroupNumber = GroupNumber;
            this.TypeOfOccupation = TypeOfOccupation;
            this.Age = Age;
            this.Status = Status;
        }

        public void Add()
        {
            StreamWriter sw = new StreamWriter("group.txt", true);
            sw.WriteLine(GroupNumber);
            sw.WriteLine(TypeOfOccupation);
            sw.WriteLine(Age);
            sw.WriteLine(Status);
            sw.WriteLine("");
            sw.Close();
        }

    }

    class PinningGroups
    {
        public string TrainerFullName;
        public int GroupNumber;
        public int TreatyNumber;
        public string FixingDate;
        public double ClassDuration;

        public PinningGroups(string TrainerFullName, int GroupNumber, int TreatyNumber, string FixingDate, int ClassDuration)
        {
            this.TrainerFullName = TrainerFullName;
            this.GroupNumber = GroupNumber;
            this.TreatyNumber = TreatyNumber;
            this.FixingDate = FixingDate;
            this.ClassDuration = ClassDuration;
        }

        public void ShowPinningGroup()
        {
            var ts = TimeSpan.FromMinutes(ClassDuration);
            Console.WriteLine("---------------------------------------------\n" +
                              $"Имя тренера: {TrainerFullName}\n" +
                              $"№ группы: {GroupNumber}\n" +
                              $"№ договора: {TreatyNumber}\n" +
                              $"Дата закрепления: {FixingDate}\n" +
                              "Длительность занятий: {0} ч. {1} мин.", ts.Hours, ts.Minutes);
            Console.WriteLine("---------------------------------------------");
        }

    }

    class Program
    {
        static void Main()
        {
            Console.Clear();
            Console.WindowHeight = Console.LargestWindowHeight;
            Console.WindowWidth = Console.LargestWindowWidth;
            Console.WriteLine("---------------------------------------------------------------------------------\n" +
                              "|  Тренера(1)  |  Группы(2)  |  Закрепление групп(3)  |  Выход из программы(4)  |\n" +
                              "---------------------------------------------------------------------------------");
            Console.Write("Введите код операции: ");
            char Code = '0';
            while (Code != '4')
            {
                Code = Console.ReadKey(true).KeyChar;
                if (Code == '1')
                {
                    TrainMenu();
                    break;
                }
                if (Code == '2')
                {
                    GroupMenu();
                    break;
                }
                if (Code == '3')
                {
                    PinningGroupMenu();
                    break;
                }
            }
        }

        public static void TrainMenu()
        {
            Console.Clear();
            Console.WriteLine("--------------------------------------------------------------------------------------------------------------------------\n" +
                              "|  Список тренеров(1)  |  Добавить тренера(2)  |  Удалить тренера(3)  |  Редактировать тренера(4)  |  В главное меню(5)  |\n" +
                              "--------------------------------------------------------------------------------------------------------------------------");
            Console.Write("Введите код операции: ");
            bool Exit = true; char Code;
            while (Exit)
            {
                Code = Console.ReadKey(true).KeyChar;
                if (Code == '1')
                {
                    ShowTrainer();
                    TrainMenu();
                    Exit = false;
                }
                if (Code == '2')
                {
                    AddTrainer();
                    TrainMenu();
                    Exit = false;
                }
                if (Code == '3')
                {
                    RemoveTrainer();
                    TrainMenu();
                    Exit = false;
                }
                if (Code == '4')
                {
                    EditTrainer();
                    TrainMenu();
                    Exit = false;
                }
                if (Code == '5')
                {
                    Main();
                    Exit = false;
                }
            }
        }
        public static void AddTrainer()
        {
        C0:
            Console.Write("\nВведите ФИО тренера: ");
            string FullName = Console.ReadLine();
            if (FullName.Any(c => char.IsNumber(c)) || FullName.Any(c => char.IsPunctuation(c)))
            {
                Console.WriteLine("В данной строке не может быть цифр или спец. символов\n");
                goto C0;
            }
        C1:
            Console.Write("Введите специализацию тренера: ");
            string Specialization = Console.ReadLine();
            if (Specialization.Any(c => char.IsNumber(c)) || Specialization.Any(c => char.IsPunctuation(c)))
            {
                Console.WriteLine("В данной строке не может быть цифр или спец. символов\n");
                goto C1;
            }
        C2:
            Console.Write("Введите квалификацию тренера: ");
            string Qualification = Console.ReadLine();
            if (Qualification.Any(c => char.IsNumber(c)) || Qualification.Any(c => char.IsPunctuation(c)))
            {
                Console.WriteLine("В данной строке не может быть цифр или спец. символов\n");
                goto C2;
            }
        Check:
            Console.Write("Введите гонорар тренера: ");
            string Check = Console.ReadLine();
            if (Check.Any(c => char.IsLetter(c)) || Check.Any(c => char.IsPunctuation(c)) || Check.Any(c => char.IsSeparator(c)))
            {
                Console.WriteLine("Некорректно введённое число\n");
                goto Check;
            }
            int Fee = int.Parse(Check);
            if (Fee < 0)
            {
                Console.WriteLine("Введённое число не может быть отрицательным\n");
                goto Check;
            }
            Trainer trainer = new Trainer(FullName, Specialization, Qualification, Fee);
            if (File.Exists("trainer.txt"))
            {
                StreamWriter sw = new StreamWriter("trainer.txt", true);
                sw.WriteLine(FullName);
                sw.WriteLine(Specialization);
                sw.WriteLine(Qualification);
                sw.WriteLine(Fee);
                sw.WriteLine("");
                sw.Close();
            }
            else
            {
                using (FileStream fs = File.Create("trainer.txt")) ;
                StreamWriter sw = new StreamWriter("trainer.txt", true);
                sw.WriteLine(FullName);
                sw.WriteLine(Specialization);
                sw.WriteLine(Qualification);
                sw.WriteLine(Fee);
                sw.WriteLine("");
                sw.Close();
            }
        }

        public static List<Trainer> GetAllTrainers()
        {
            StreamReader reader = new StreamReader("trainer.txt");
            List<Trainer> trainers = new List<Trainer>();
            List<string> info = new List<string>();
            while (!reader.EndOfStream)
            {
                string infoLine = reader.ReadLine();
                if (infoLine == "")
                {
                    trainers.Add(new Trainer(info[0], info[1], info[2], int.Parse(info[3])));
                    info.Clear();
                }
                else
                {
                    info.Add(infoLine);
                }
            }
            reader.Close();
            return trainers;
        }

        public static void ShowTrainer()
        {
            Console.Clear();
            if (File.Exists("trainer.txt"))
            {
                List<Trainer> trainers = GetAllTrainers();
                if (trainers.Count == 0)
                {
                    Console.WriteLine("Нет тренеров");
                    Console.WriteLine("\nНажмите любую клавишу, чтобы продолжить...");
                    Console.ReadKey();
                }
                else
                {
                    Console.WriteLine("---------------------------------------\n" +
                                      "|  По гонорару(1)  |  По алфавиту(2)  |\n" +
                                      "---------------------------------------");
                    Console.WriteLine("Введите код операции\n");
                    Console.WriteLine("Список тренеров:");
                    char Code = Console.ReadKey(true).KeyChar;
                    if (Code == '1')
                    {
                        trainers.Sort(delegate (Trainer x, Trainer y) {
                            return x.Fee.CompareTo(y.Fee);
                        });
                    }
                    if (Code == '2')
                    {
                        trainers.Sort(delegate (Trainer x, Trainer y) {
                            return x.FullName.CompareTo(y.FullName);
                        });
                    }
                    if (File.Exists("PinningGroup.txt"))
                    {
                        List<PinningGroups> pinningGroups = GetAllPinningGroups();
                        foreach (PinningGroups pinningGroup in pinningGroups)
                        {
                            foreach (Trainer trainer in trainers)
                            {
                                if (pinningGroup.TrainerFullName == trainer.FullName)
                                {
                                    trainer.ClassDuration = trainer.ClassDuration + pinningGroup.ClassDuration;
                                    trainer.Salary = (trainer.ClassDuration / 60) * trainer.Fee;
                                }
                            }
                        }
                    }
                    foreach (Trainer trainer in trainers)
                    {
                        Console.WriteLine("---------------------------------------------\n" +
                                         $"Имя: {trainer.FullName}\n" +
                                         $"Специализация: {trainer.Specialization}\n" +
                                         $"Квалификация: {trainer.Qualification}\n" +
                                         $"Гонорар: {trainer.Fee} руб.\n" +
                                         $"Зарплата: {trainer.Salary:F2} руб.\n" +
                                         "---------------------------------------------");
                    }
                    Console.WriteLine("Нажмите любую клавишу, чтобы продолжить...");
                    Console.ReadKey();
                }
            }
            else
            {
                Console.WriteLine("Нет тренеров для просмотра\n\nНажмите любую клавишу, чтобы продолжить...");
                Console.ReadKey();
            }
        }

        public static void RemoveTrainer()
        {
            Console.Clear();
            List<Trainer> trainers = GetAllTrainers();
        C0:
            Console.Write("Введите ФИО тренера для удаления: ");
            string Name = Console.ReadLine();
            if (Name.Any(c => char.IsNumber(c)) || Name.Any(c => char.IsPunctuation(c)))
            {
                Console.WriteLine("В данной строке не может быть цифр или спец. символов\n");
                goto C0;
            }
            Trainer searchtrainers = trainers.Find(trainer => trainer.FullName == Name);
            if (searchtrainers != null)
            {
                trainers.Remove(searchtrainers);
                StreamWriter sw = new StreamWriter("trainer.txt", false);
                sw.Close();
                foreach (Trainer trainer in trainers)
                {
                    trainer.Add();
                }
            }
            else
            {
                Console.WriteLine("Нет тренеров для удаления");
                Console.WriteLine("\nНажмите любую клавишу, чтобы продолжить...");
                Console.ReadKey();
            }
        }

        public static void EditTrainer()
        {
            Console.Clear();
            List<Trainer> trainers = GetAllTrainers();
        C0:
            Console.Write("Введите ФИО тренера: ");
            string Name = Console.ReadLine();
            if (Name.Any(c => char.IsNumber(c)) || Name.Any(c => char.IsPunctuation(c)))
            {
                Console.WriteLine("В данной строке не может быть цифр или спец. символов\n");
                goto C0;
            }
            Trainer search = trainers.Find(trainer => trainer.FullName == Name);
            if (search != null)
            {
                int index = trainers.IndexOf(search);
                Console.WriteLine("|——————————————————————————————————————————————————————————————————--------------|\n" +
                                  "|  ФИО(1)  |  Специализация(2)  |  Квалификация(3)  |  Гонорар(4)  |  В меню(5)  |\n" +
                                  "|——————————————————————————————————————————————————————————————————--------------|");
                Console.Write("Введите код опреации: \n");
                bool Exit = true; char Code;
                while (Exit)
                {
                    Code = Console.ReadKey(true).KeyChar;
                    if (Code == '1')
                    {
                    C1:
                        Console.Write("Введите ФИО тренера: ");
                        search.FullName = Console.ReadLine();
                        if (search.FullName.Any(c => char.IsNumber(c)) || search.FullName.Any(c => char.IsPunctuation(c)))
                        {
                            Console.WriteLine("В данной строке не может быть цифр или спец. символов\n");
                            goto C1;
                        }
                        Exit = false;
                    }
                    if (Code == '2')
                    {
                    C2:
                        Console.Write("Введите специализацию тренера: ");
                        search.Specialization = Console.ReadLine();
                        if (search.Specialization.Any(c => char.IsNumber(c)) || search.Specialization.Any(c => char.IsPunctuation(c)))
                        {
                            Console.WriteLine("В данной строке не может быть цифр или спец. символов\n");
                            goto C2;
                        }
                        Exit = false;
                    }
                    if (Code == '3')
                    {
                    C3:
                        Console.Write("Введите квалификацию тренера: ");
                        search.Qualification = Console.ReadLine();
                        if (search.Qualification.Any(c => char.IsNumber(c)) || search.Qualification.Any(c => char.IsPunctuation(c)))
                        {
                            Console.WriteLine("В данной строке не может быть цифр или спец. символов\n");
                            goto C3;
                        }
                        Exit = false;
                    }
                    if (Code == '4')
                    {
                    Check:
                        Console.Write("Введите гонорар тренера: ");
                        string Check = Console.ReadLine();
                        if (Check.Any(c => char.IsLetter(c)) || Check.Any(c => char.IsPunctuation(c)) || Check.Any(c => char.IsSeparator(c)))
                        {
                            Console.WriteLine("Некорректно введённое число\n");
                            goto Check;
                        }
                        search.Fee = int.Parse(Check);
                        if (search.Fee < 0)
                        {
                            Console.WriteLine("Введённое число не может быть отрицательным\n");
                            goto Check;
                        }
                        Exit = false;
                    }
                    if (Code == '5')
                    {
                        Exit = false;
                    }
                }
                trainers[index] = search;
                StreamWriter sw = new StreamWriter("trainer.txt", false);
                sw.Close();
                foreach (Trainer trainer in trainers)
                {
                    trainer.Add();
                }
            }
            else
            {
                Console.WriteLine("Данного тренера нет в списке, попробуйте ещё раз");
                Console.ReadKey();
                TrainMenu();
            }
        }

        public static void GroupMenu()
        {
            Console.Clear();
            Console.WriteLine("--------------------------------------------------------------------------------------------------------------------\n" +
                              "|  Список групп(1)  |  Добавить группу(2)  |  Удалить группу(3)  |  Редактировать группу(4)  |  В главное меню(5)  |\n" +
                              "--------------------------------------------------------------------------------------------------------------------");
            Console.Write("Введите код операции: \n");
            bool Exit = true; char Code;
            while (Exit)
            {
                Code = Console.ReadKey(true).KeyChar;
                if (Code == '1')
                {
                    ShowGroup();
                    GroupMenu();
                    Exit = false;
                }
                if (Code == '2')
                {
                    AddGroup();
                    GroupMenu();
                    Exit = false;
                }
                if (Code == '3')
                {
                    RemoveGroup();
                    GroupMenu();
                    Exit = false;
                }
                if (Code == '4')
                {
                    EditGroup();
                    GroupMenu();
                    Exit = false;
                }
                if (Code == '5')
                {
                    Main();
                    Exit = false;
                }
            }
        }

        public static void AddGroup()
        {
        C0:
            Console.Write("Введите вид занятий: ");
            string TypeOfOccupation = Console.ReadLine();
            if (TypeOfOccupation.Any(c => char.IsNumber(c)) || TypeOfOccupation.Any(c => char.IsPunctuation(c)))
            {
                Console.WriteLine("В данной строке не может быть цифр или спец. символов\n");
                goto C0;
            }
            Console.Write("Введите целевой возраст для группы: ");
        Check:
            string Check = Console.ReadLine();
            if (Check.Any(c => char.IsLetter(c)) || Check.Any(c => char.IsPunctuation(c)) || Check.Any(c => char.IsSeparator(c)))
            {
                Console.WriteLine("Некорректно введённое число\n");
                goto Check;
            }
            int Age = int.Parse(Check);
            if (Age < 0)
            {
                Console.WriteLine("Введённое число не может быть отрицательным\n");
                goto Check;
            }
        C1:
            Console.Write("Введите статус группы: ");
            string Status = Console.ReadLine();
            if (Status.Any(c => char.IsNumber(c)) || Status.Any(c => char.IsPunctuation(c)))
            {
                Console.WriteLine("В данной строке не может быть цифр или спец. символов\n");
                goto C1;
            }
            List<Group> groups = GetAllGroups();
            int GroupNumber = 1;
            if (groups.Count != 0)
            {
                int i = 1;
                foreach (Group group in groups)
                {
                    if (groups.Count == i)
                        GroupNumber = i + 1;
                    if (group.GroupNumber == i)
                        i++;
                    else
                        GroupNumber = i;
                }
            }
            if (File.Exists("group.txt"))
            {
                StreamWriter sw = new StreamWriter("group.txt", true);
                sw.WriteLine(GroupNumber);
                sw.WriteLine(TypeOfOccupation);
                sw.WriteLine(Age);
                sw.WriteLine(Status);
                sw.WriteLine("");
                sw.Close();
            }
            else
            {
                using (FileStream fs = File.Create("group.txt")) ;
                StreamWriter sw = new StreamWriter("group.txt", true);
                sw.WriteLine(GroupNumber);
                sw.WriteLine(TypeOfOccupation);
                sw.WriteLine(Age);
                sw.WriteLine(Status);
                sw.WriteLine("");
                sw.Close();
            }
        }

        public static List<Group> GetAllGroups()
        {
            StreamReader reader = new StreamReader("group.txt");
            List<Group> groups = new List<Group>();
            List<string> info = new List<string>();
            while (!reader.EndOfStream)
            {
                string infoLine = reader.ReadLine();
                if (infoLine == "")
                {
                    groups.Add(new Group(int.Parse(info[0]), info[1], int.Parse(info[2]), info[3]));
                    info.Clear();
                }
                else
                {
                    info.Add(infoLine);
                }
            }
            reader.Close();
            return groups;
        }

        public static void ShowGroup()
        {
            Console.Clear();
            if (File.Exists("group.txt"))
            {
                List<Group> groups = GetAllGroups();
                if (groups.Count == 0)
                {
                    Console.WriteLine("Нету записанных групп");
                    Console.WriteLine("\nНажмите любую клавишу, чтобы продолжить...");
                    Console.ReadKey();
                }
                else
                {
                    Console.WriteLine("---------------------------------------\n" +
                                      "|  По возрасту(1)  |  По алфавиту(2)  |\n" +
                                      "---------------------------------------");
                    Console.WriteLine("Введите код операции\n");
                    Console.WriteLine("Список тренеров:");
                    char Code = Console.ReadKey(true).KeyChar;
                    if (Code == '1')
                    {
                        groups.Sort(delegate (Group x, Group y)
                        {
                            return x.Age.CompareTo(y.Age);
                        });
                    }
                    if (Code == '2')
                    {
                        groups.Sort(delegate (Group x, Group y)
                        {
                            return x.TypeOfOccupation.CompareTo(y.TypeOfOccupation);
                        });
                    }
                    foreach (Group group in groups)
                    {
                        Console.WriteLine("---------------------------------------------\n" +
                                         $"№ группы: {group.GroupNumber}\n" +
                                         $"Вид занятий: {group.TypeOfOccupation}\n" +
                                         $"Целевой возраст: {group.Age}\n" +
                                         $"Статус: {group.Status}\n" +
                                          "---------------------------------------------");
                    }
                    Console.WriteLine("Нажмите любую клавишу, чтобы продолжить...");
                    Console.ReadKey();
                }
            }
            else
            {
                Console.WriteLine("Нет групп для просмотра\n\nНажмите любую клавишу, чтобы продолжить...");
                Console.ReadKey();
            }
        }

        public static void RemoveGroup()
        {
            Console.Clear();
            List<Group> groups = GetAllGroups();
            if (groups.Count == 0)
            {
                Console.WriteLine("Нету записанных групп");
                Console.WriteLine("\nНажмите любую клавишу, чтобы продолжить...");
                Console.ReadKey();
            }
            else
            {
            Check:
                Console.Write("Введите № группы для удаления: ");
                string Check = Console.ReadLine();
                if (Check.Any(c => char.IsLetter(c)) || Check.Any(c => char.IsPunctuation(c)) || Check.Any(c => char.IsSeparator(c)))
                {
                    Console.WriteLine("Некорректно введённое число\n");
                    goto Check;
                }
                int Number = int.Parse(Check);
                if (Number < 0)
                {
                    Console.WriteLine("Введённое число не может быть отрицательным\n");
                    goto Check;
                }
                Group searchgroups = groups.Find(group => group.GroupNumber == Number);
                if (searchgroups != null)
                {
                    groups.Remove(searchgroups);
                    StreamWriter sw = new StreamWriter("group.txt", false);
                    sw.Close();
                    foreach (Group group in groups)
                    {
                        group.Add();
                    }
                }
            }
        }

        public static void EditGroup()
        {
            Console.Clear();
            List<Group> groups = GetAllGroups();
        Check:
            Console.Write("Введите номер группы: ");
            int Number = int.Parse(Console.ReadLine());
            if (Number < 0)
            {
                Console.WriteLine("Введённое число не может быть отрицательным\n");
                goto Check;
            }
            Group search = groups.Find(group => group.GroupNumber == Number);
            if (search != null)
            {
                int index = groups.IndexOf(search);
                Console.WriteLine("|——————————————————————————————————————————————————————————————————---|\n" +
                                  "|  Вид занятий(1)  |  Целевой возраст(2)  |  Статус(3)  |  В меню(4)  |\n" +
                                  "|—————————————————————————————————————————————————————————------------|");
                Console.Write("Введите код опреации: \n");
                bool Exit = true; char Code;
                while (Exit)
                {
                    Code = Console.ReadKey(true).KeyChar;
                    if (Code == '1')
                    {
                    C0:
                        Console.Write("Введите вид занятий: ");
                        search.TypeOfOccupation = Console.ReadLine();
                        if (search.TypeOfOccupation.Any(c => char.IsNumber(c)) || search.TypeOfOccupation.Any(c => char.IsPunctuation(c)))
                        {
                            Console.WriteLine("В данной строке не может быть цифр или спец. символов\n");
                            goto C0;
                        }
                        Exit = false;
                    }
                    if (Code == '2')
                    {
                    check:
                        Console.Write("Введите целевой возраст: ");
                        string Check = Console.ReadLine();
                        if (Check.Any(c => char.IsLetter(c)) || Check.Any(c => char.IsPunctuation(c)) || Check.Any(c => char.IsSeparator(c)))
                        {
                            Console.WriteLine("Некорректно введённое число\n");
                            goto Check;
                        }
                        search.Age = int.Parse(Check);
                        if (search.Age < 0)
                        {
                            Console.WriteLine("Введённое число не может быть отрицательным\n");
                            goto check;
                        }
                        Exit = false;
                    }
                    if (Code == '3')
                    {
                    C1:
                        Console.Write("Введите статус группы: ");
                        search.Status = Console.ReadLine();
                        if (search.Status.Any(c => char.IsNumber(c)) || search.Status.Any(c => char.IsPunctuation(c)))
                        {
                            Console.WriteLine("В данной строке не может быть цифр или спец. символов\n");
                            goto C1;
                        }
                        Exit = false;
                    }
                    if (Code == '4')
                    {
                        Exit = false;
                    }
                }
                groups[index] = search;
                StreamWriter sw = new StreamWriter("group.txt", false);
                sw.Close();
                foreach (Group group in groups)
                {
                    group.Add();
                }
            }
            else
            {
                Console.WriteLine("Данного номера группы нет в списке, попробуйте ещё раз");
                Console.ReadKey();
                GroupMenu();
            }
        }

        public static void PinningGroupMenu()
        {
            Console.Clear();
            Console.WriteLine("------------------------------------------------------------------------\n" +
                              "|  Список договоров(1)  |  Составить договор(2)  |  В главное меню(3)  |\n" +
                              "------------------------------------------------------------------------");
            Console.Write("Введите код операции: \n");
            bool Exit = true; char Code;
            while (Exit)
            {
                Code = Console.ReadKey(true).KeyChar;
                if (Code == '1')
                {
                    ShowPinningGroups();
                    PinningGroupMenu();
                    Exit = false;
                }
                if (Code == '2')
                {
                    AddPinningGroups();
                    PinningGroupMenu();
                    Exit = false;
                }
                if (Code == '3')
                {
                    Main();
                    Exit = false;
                }
            }
        }

        public static void AddPinningGroups()
        {
            List<Group> groups = GetAllGroups();
            List<Trainer> trainers = GetAllTrainers();
        Check:
            bool flag = true;
            bool Checked = false;
            string Specialization;
            string TypeOfOccupation;
        C0:
            Console.Write("Введите ФИО тренера: ");
            string TrainerFullName = Console.ReadLine();
            if (TrainerFullName.Any(c => char.IsNumber(c)) || TrainerFullName.Any(c => char.IsPunctuation(c)))
            {
                Console.WriteLine("В данной строке не может быть цифр или спец. символов\n");
                goto C0;
            }
        check:
            Console.Write("Введите номер группы: ");
            string Check = Console.ReadLine();
            if (Check.Any(c => char.IsLetter(c)) || Check.Any(c => char.IsPunctuation(c)) || Check.Any(c => char.IsSeparator(c)))
            {
                Console.WriteLine("Некорректно введённое число\n");
                goto check;
            }
            int GroupNumber = int.Parse(Check);
            if (GroupNumber < 0)
            {
                Console.WriteLine("Введённое число не может быть отрицательным\n");
                goto check;
            }
            while (flag)
            {
                foreach (Group group in groups)
                {
                    foreach (Trainer trainer in trainers)
                    {
                        if (TrainerFullName == trainer.FullName && GroupNumber == group.GroupNumber)
                        {
                            flag = false;
                            Checked = true;
                            TypeOfOccupation = group.TypeOfOccupation;
                            Specialization = trainer.Specialization;
                            if (Specialization != TypeOfOccupation)
                            {
                                Console.WriteLine("Специализация тренера не подходит к группе \n");
                                goto Check;
                            }
                        }
                    }
                }
                flag = false;
                if (Checked == false)
                {
                    Console.WriteLine("Нет такой группы или тренера \n");
                    goto Check;
                }
            }
        CheckDate:
            Console.Write("Введите дату закрепления: ");
            string FixingDate = Console.ReadLine();
            DateTime dDate;
            if (DateTime.TryParse(FixingDate, out dDate))
            {
                String.Format("{0:d/MM/yyyy}", dDate);
            }
            else
            {
                Console.WriteLine("Некорректная дата\n");
                goto CheckDate;
            }
        Check1:
            Console.Write("Введите длительность занятий (мин.): ");
            string Check1 = Console.ReadLine();
            if (Check1.Any(c => char.IsLetter(c)) || Check1.Any(c => char.IsPunctuation(c)) || Check1.Any(c => char.IsSeparator(c)))
            {
                Console.WriteLine("Некорректно введённое число\n");
                goto Check1;
            }
            int ClassDuration = int.Parse(Check1);
            if (ClassDuration < 0)
            {
                Console.WriteLine("Введённое число не может быть отрицательным\n");
                goto Check1;
            }
            List<PinningGroups> pinningGroups = GetAllPinningGroups();
            int TreatyNumber = 1;
            if (pinningGroups.Count != 0)
            {
                int i = 1;
                foreach (PinningGroups group in pinningGroups)
                {
                    if (pinningGroups.Count == i)
                        TreatyNumber = i + 1;
                    if (group.TreatyNumber == i)
                        i++;
                    else
                        TreatyNumber = i;
                }
            }
            if (File.Exists("PinningGroup.txt"))
            {
                StreamWriter sw = new StreamWriter("PinningGroup.txt", true);
                sw.WriteLine(TrainerFullName);
                sw.WriteLine(GroupNumber);
                sw.WriteLine(TreatyNumber);
                sw.WriteLine(FixingDate);
                sw.WriteLine(ClassDuration);
                sw.WriteLine("");
                sw.Close();
            }
            else
            {
                using (FileStream fs = File.Create("PinningGroup.txt")) ;
                StreamWriter sw = new StreamWriter("PinningGroup.txt", true);
                sw.WriteLine(TrainerFullName);
                sw.WriteLine(GroupNumber);
                sw.WriteLine(TreatyNumber);
                sw.WriteLine(FixingDate);
                sw.WriteLine(ClassDuration);
                sw.WriteLine("");
                sw.Close();
            }
        }

        public static List<PinningGroups> GetAllPinningGroups()
        {
            StreamReader reader = new StreamReader("PinningGroup.txt");
            List<PinningGroups> pinningGroups = new List<PinningGroups>();
            List<string> info = new List<string>();
            while (!reader.EndOfStream)
            {
                string infoLine = reader.ReadLine();
                if (infoLine == "")
                {
                    pinningGroups.Add(new PinningGroups(info[0], int.Parse(info[1]), int.Parse(info[2]), info[3], int.Parse(info[4])));
                    info.Clear();
                }
                else
                {
                    info.Add(infoLine);
                }
            }
            reader.Close();
            return pinningGroups;
        }

        public static void ShowPinningGroups()
        {
            Console.Clear();
            if (File.Exists("PinningGroup.txt"))
            {
                List<PinningGroups> pinningGroups = GetAllPinningGroups();
                if (pinningGroups.Count == 0)
                {
                    Console.WriteLine("Нет записанных договоров");
                    Console.WriteLine("\nНажмите любую клавишу, чтобы продолжить...");
                    Console.ReadKey();
                }
                else
                {
                    Console.WriteLine("---------------------------------------------------\n" +
                                      "|  По длительности занятий(1)  |  По алфавиту(2)  |\n" +
                                      "---------------------------------------------------");
                    Console.WriteLine("Введите код операции\n");
                    Console.WriteLine("Список договоров:");
                    char Code = Console.ReadKey(true).KeyChar;
                    if (Code == '1')
                    {
                        pinningGroups.Sort(delegate (PinningGroups x, PinningGroups y)
                        {
                            return x.TrainerFullName.CompareTo(y.TrainerFullName);
                        });
                    }
                    if (Code == '2')
                    {
                        pinningGroups.Sort(delegate (PinningGroups x, PinningGroups y)
                        {
                            return x.ClassDuration.CompareTo(y.ClassDuration);
                        });
                    }
                    foreach (PinningGroups group in pinningGroups)
                    {
                        group.ShowPinningGroup();
                    }
                }
                Console.WriteLine("Нажмите любую клавишу, чтобы продолжить");
                Console.ReadKey();
            }
            else
            {
                Console.WriteLine("Нету закрепленных групп для просмотра");
                Console.ReadKey();
            }
        }
    }
}