using System;
using System.Collections.Generic;

namespace SaveInfoFramework
{
    internal class Program
    {
        static void Main(string[] args)
        {
#if DEBUG
            Console.Title = Properties.Settings.Default.ApplicationNameDebug;
#else
            Console.Title = Properties.Settings.Default.ApplicationName;
#endif
            if (string.IsNullOrEmpty(Properties.Settings.Default.Fio) || Properties.Settings.Default.Age <= 0)
            {
                Console.Write("Введите ФИО: ");
                Properties.Settings.Default.Fio = Console.ReadLine();
                Console.Write("Введите возраст: ");
                if (int.TryParse(Console.ReadLine(), out int age))
                    Properties.Settings.Default.Age = age;
                else
                {
                    Properties.Settings.Default.Age = 0;
                }
                Properties.Settings.Default.Save();
            }

            Console.WriteLine($"ФИО: {Properties.Settings.Default.Fio}");
            Console.WriteLine($"Возраст: {Properties.Settings.Default.Age}");

            /*ConnectionString connectionString1 = new ConnectionString()
            {
                DatabaseName = "DB1",
                Host = "localhost",
                Password = "12345",
                UserName = "User1",
            };

            ConnectionString connectionString2 = new ConnectionString()
            {
                DatabaseName = "DB2",
                Host = "localhost",
                Password = "12345",
                UserName = "User2",
            };

            List<ConnectionString> connections = new List<ConnectionString>();
            connections.Add(connectionString1);
            connections.Add(connectionString2);

            CacheProvider cacheProvider = new CacheProvider();
            cacheProvider.CacheConnection(connections);*/

            CacheProvider cacheProvider = new CacheProvider();
            List<ConnectionString> connections = cacheProvider.GetconnectionFromCache();

            foreach (ConnectionString connection in connections)
            {
                Console.WriteLine($"{connection.Host} {connection.DatabaseName} {connection.UserName} {connection.Password}");
            }

            Console.ReadKey();
        }
    }
}
