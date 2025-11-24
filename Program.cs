
namespace SimpleJobPortal
{
    class Program
    {
        struct Member
        {
            public string userid;
            public string name;
            public string email;
            public string phone;
            public string designation;
        }

        static Member[] members = new Member[100];
        static int memberCount = 0;
        static int userIdCounter = 1;

        static string validEmail = "admin@123.com";
        static string validPassword = "admin123";

        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("1 - Login");

                Console.Write("Enter your choice: ");
                string option = Console.ReadLine();

                if (option == "1")
                {

                    Console.Write("Enter email: ");
                    string mail = Console.ReadLine();

                    Console.Write("Enter password: ");
                    string password = Console.ReadLine();
                    if (mail == validEmail && password == validPassword)
                    {
                        Console.WriteLine("Login successful!");




                        while (true)
                        {
                            Console.WriteLine("\n1 - List all company members");
                            Console.WriteLine("2 - Add company member");
                            Console.WriteLine("3 - Logout");
                            string choice = Console.ReadLine();

                            if (choice == "1")
                            {
                                Console.WriteLine("UserID\t\t\tDesignation\t\t\tName\t\t\tEmail\t\t\tPhone");

                                for (int i = 0; i < memberCount; i++)
                                {
                                    Console.WriteLine($"{members[i].userid}\t{members[i].designation}\t{members[i].name}\t{members[i].email}\t{members[i].phone}");
                                }
                            }
                            else if (choice == "2")
                            {
                                Member m;
                                m.userid = "U" + userIdCounter.ToString("D3");
                                userIdCounter++;

                                Console.Write("Enter name: ");
                                m.name = Console.ReadLine();

                                Console.Write("Enter email: ");
                                m.email = Console.ReadLine();

                                Console.Write("Enter phone: ");
                                m.phone = Console.ReadLine();

                                Console.Write("Enter designation: ");
                                m.designation = Console.ReadLine();

                                members[memberCount] = m;
                                memberCount++;

                                Console.WriteLine("Member added successfully!");
                            }
                            else if (choice == "3")
                            {
                                Console.WriteLine("Logged out.");
                                break;
                            }
                            else
                            {
                                Console.WriteLine("Invalid choice.");
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Login failed! Email or password is incorrect.\n");
                    }
                }

                else
                {
                    Console.WriteLine("Invalid option.\n");

                }
            }
        }
    }
        }
    
    

                                 









