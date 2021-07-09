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

            IFTimeLifeOK(data);

            if (data.TryGetValue(key, out tmp))
                throw new ArgumentException("Такой ключ уже существует.");

            IFTimeLifeOK(data);
            if (data.Count == maxSize)   
                data.Add(key, new KeyValuePair<T, DateTime>(_data, DateTime.Now));
            else
                data.Add(key, new KeyValuePair<T, DateTime>(_data, DateTime.Now));
        }
        public T Get(string key)
        {
            KeyValuePair<T, DateTime> tmp;

            IFTimeLifeOK(data);
            if (!data.TryGetValue(key, out tmp))
                throw new KeyNotFoundException();
            return tmp.Key;
        }
        public void IFTimeLifeOK(Dictionary<string, KeyValuePair<T, DateTime>> _data)
        {
            foreach (var i in _data)
            {
                if (DateTime.Now - i.Value.Value > timeLife)
                    _data.Remove(i.Key);
            }
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            TimeSpan timelife = new TimeSpan(0, 0, 30);
            Cache<string> cache = new Cache<string>(timelife, 5);

            cache.Save("123", "qwerty");
            cache.Save("135", "asd");
            cache.Save("135", "gfdsgfd");
            cache.Save("135gg", "64646");
        }
    }
}
