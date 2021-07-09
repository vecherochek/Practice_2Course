using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace problem_3
{
    class Cache<T>
    {
        private TimeSpan    timeLife;
        private int         maxSize;
        private Dictionary<string, KeyValuePair<T, DateTime>> data; 
        public Cache(TimeSpan time, int size)
        {
            timeLife    = time;
            maxSize     = size;
            data        = new Dictionary<string, KeyValuePair<T, DateTime>>();
        }
        public void Save(string key, T _data)
        {
            KeyValuePair<T, DateTime> tmp;

            TimeLife();

            if (data.TryGetValue(key, out tmp))
                throw new ArgumentException("Такой ключ уже существует =)");

            TimeLife();
            if (data.Count == maxSize) DeleteOld();
            data.Add(key, new KeyValuePair<T, DateTime>(_data, DateTime.Now));
        }
        public T Get(string key)
        {
            KeyValuePair<T, DateTime> tmp;

            TimeLife();
            if (data.TryGetValue(key, out tmp))
                return tmp.Key;
            else throw new KeyNotFoundException("Нет такого ключа =(");
        }
        public void TimeLife()
        {
            DateTime time = DateTime.Now;

            foreach (var pair in data.ToArray())
            {
                if (time - pair.Value.Value > timeLife)
                    data.Remove(pair.Key);
            }
        }
        public void DeleteOld()
        {
            string key = "";
            DateTime time = DateTime.Now;
            TimeSpan interval = new TimeSpan(0, 0, 0);

            foreach (var pair in data)
            {
                if (time - pair.Value.Value > interval)
                {
                    interval = time - pair.Value.Value;
                    key = pair.Key;
                }
            }
            data.Remove(key);
        }
        public void Print()
        {
            TimeLife();
            foreach (var pair in data)
            {
                Console.WriteLine($"Ключ: {pair.Key} Данные: {pair.Value.Key} Время: {pair.Value.Value}");
            }
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            int a;
            TimeSpan timelife = new TimeSpan(0, 0, 10);
            Cache<string> cache = new Cache<string>(timelife, 5);

            try
            {
                cache.Save("123", "qwerty");
                cache.Save("135", "asd");
                cache.Save("136", "gfdsgfd");
                cache.Save("16", "рпарпнр");
                cache.Save("169", "икип");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            do
            {
                Menu();
                if (int.TryParse(Console.ReadLine(), out a) && a > 0 && a < 5)
                {
                    MenuSwitch(a, cache);
                }
                else
                {
                    Console.WriteLine("Введите число от 0 до 3.");
                }
            } while (a != 4);
        }

        static void Menu()
        {
            Console.WriteLine("1. Добавить данные по ключу\n" +
                              "2. Получить данные из кеша по ключу\n" +
                              "3. Показать кеш\n" +
                              "4. Выход");
        }
        static void MenuSwitch(int a, Cache<string> cache)
        {
            string key, data;
            switch (a)
            {
                case 1:
                    {
                        Console.WriteLine("Введите ключ: ");
                        key = Console.ReadLine();

                        Console.WriteLine("Введите данные: ");
                        data = Console.ReadLine();

                        try
                        {
                            cache.Save(key, data);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        break;
                    }
                case 2:
                    {
                        Console.WriteLine("Введите ключ: ");
                        key = Console.ReadLine();

                        try
                        {
                            Console.WriteLine(cache.Get(key));
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        break;
                    }
                case 3:
                    {
                        cache.Print();
                        break;
                    }
                case 4:
                    {
                        break;
                    }
                default: break;
            }
        }
    }
}
