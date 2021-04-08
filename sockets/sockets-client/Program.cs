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

            var client = new TcpClient("localhost", 9000);
            var output = client.GetStream();
            var receive = new BinaryReader(output);
            var send = new BinaryWriter(output);

            while (true)
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

                string title, author;
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

                        using (var ms = new MemoryStream())
                        {
                            using var writer = new BinaryWriter(ms);
                            send.Write(option);
                            send.Write(title);
                            send.Write(author);
                            send.Write(year);
                            send.Write(edition);
                        }
                        break;

                    case 2:
                        Console.WriteLine("Título do Livro:");
                        title = Console.ReadLine();
                        using (var ms = new MemoryStream())
                        {
                            using var writer = new BinaryWriter(ms);
                            send.Write(option);
                            send.Write(title);
                        }
                        break;

                    case 3:
                        Console.WriteLine("Autor do Livro:");
                        author = Console.ReadLine();
                        using (var ms = new MemoryStream())
                        {
                            using var writer = new BinaryWriter(ms);
                            send.Write(option);
                            send.Write(author);
                        }
                        break;

                    case 4:
                        Console.WriteLine("Ano do Livro:");
                        year = int.Parse(Console.ReadLine());
                        using (var ms = new MemoryStream())
                        {
                            using var writer = new BinaryWriter(ms);
                            send.Write(option);
                            send.Write(year);
                        }
                        break;

                    case 5:
                        Console.WriteLine("Edição do Livro:");
                        edition = int.Parse(Console.ReadLine());
                        using (var ms = new MemoryStream())
                        {
                            using var writer = new BinaryWriter(ms);
                            send.Write(option);
                            send.Write(edition);
                        }
                        break;

                    case 6:
                        Console.WriteLine("Título do Livro para remover:");
                        title = Console.ReadLine();
                        using (var ms = new MemoryStream())
                        {
                            using var writer = new BinaryWriter(ms);
                            send.Write(option);
                            send.Write(title);
                        }
                        break;

                    case 7:
                        //Update();
                        break;

                    case 8:
                        send.Write(option);
                        output.Close();
                        receive.Close();
                        send.Close();
                        client.Close();
                        Environment.Exit(0);
                        break;

                    default:
                        Console.WriteLine("Opção não existente");
                        break;
                }

                Console.WriteLine("Waiting Server response...");
                Console.WriteLine(receive.ReadString());
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