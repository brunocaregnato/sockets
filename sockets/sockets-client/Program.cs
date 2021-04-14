using System;
using System.IO;
using System.Net.Sockets;

namespace sockets_client
{
    class Program
    {
        static void Main(string[] args)
        {
            int option;

            using (var client = new TcpClient("localhost", 9000))
            {
                var output = client.GetStream();

                using (var receive = new BinaryReader(output))
                using (var send = new BinaryWriter(output))
                {
                    var run = true;

                    while (run)
                    {
                        PrintMenu();
                        while (true)
                        {
                            try
                            {
                                option = int.Parse(Console.ReadLine());
                                break;
                            }
                            catch (Exception)
                            {
                                Console.WriteLine("Digite uma opção válida!");
                            }
                        }

                        string title, author, message = string.Empty;
                        int year, edition;

                        switch (option)
                        {
                        case 1:
                            Console.WriteLine("Título do Livro:");
                            title = Console.ReadLine();

                            Console.WriteLine("Autor do Livro:");
                            author = Console.ReadLine();

                            Console.WriteLine("Ano do Livro:");
                            year = int.Parse(Console.ReadLine());

                            Console.WriteLine("Edição do Livro:");
                            edition = int.Parse(Console.ReadLine());

                            send.Write(option);
                            send.Write(title);
                            send.Write(author);
                            send.Write(year);
                            send.Write(edition);

                            break;

                        case 2:
                            Console.WriteLine("Título do Livro:");
                            title = Console.ReadLine();

                            send.Write(option);
                            send.Write(title);

                            break;

                        case 3:
                            Console.WriteLine("Autor do Livro:");
                            author = Console.ReadLine();

                            send.Write(option);
                            send.Write(author);

                            break;

                        case 4:
                            Console.WriteLine("Ano do Livro:");
                            year = int.Parse(Console.ReadLine());

                            send.Write(option);
                            send.Write(year);

                            break;

                        case 5:
                            Console.WriteLine("Edição do Livro:");
                            edition = int.Parse(Console.ReadLine());

                            send.Write(option);
                            send.Write(edition);

                            break;

                        case 6:
                            Console.WriteLine("Título do Livro para remover:");
                            title = Console.ReadLine();

                            send.Write(option);
                            send.Write(title);

                            break;

                        case 7:
                            Console.WriteLine("Título do Livro:");
                            var oldTitle = Console.ReadLine();

                            send.Write(option);
                            send.Write(oldTitle);

                            message = receive.ReadString();

                            if (message.Equals(string.Empty))
                            {
                                Console.WriteLine("Novo título do Livro:");
                                var newTitle = Console.ReadLine();

                                Console.WriteLine("Novo autor do Livro:");
                                author = Console.ReadLine();

                                Console.WriteLine("Novo ano do Livro:");
                                year = int.Parse(Console.ReadLine());

                                Console.WriteLine("Nova edição do Livro:");
                                edition = int.Parse(Console.ReadLine());

                                send.Write(9);
                                send.Write(oldTitle);
                                send.Write(newTitle);
                                send.Write(author);
                                send.Write(year);
                                send.Write(edition);
                            }

                            break;

                        case 8:
                            send.Write(option);
                            run = false;
                            break;

                        default:
                            Console.WriteLine("Opção não existente");
                            break;
                        }

                        Console.WriteLine(option != 8 ? receive.ReadString() : message);
                    }

                }

            }
        }

        private static void PrintMenu()
        {
            Console.WriteLine("-------------------------------");
            Console.WriteLine("1 - Criar livro");
            Console.WriteLine("2 - Consultar livro por título");
            Console.WriteLine("3 - Consultar livro por autor");
            Console.WriteLine("4 - Consultar livro por ano");
            Console.WriteLine("5 - Consultar livro por edição");
            Console.WriteLine("6 - Remover livro");
            Console.WriteLine("7 - Atualizar livro");
            Console.WriteLine("8 - Sair");
            Console.WriteLine("-------------------------------");
            Console.Write("Digite sua opção: ");
        }
    }
}