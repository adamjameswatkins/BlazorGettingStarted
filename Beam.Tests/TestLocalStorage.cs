using Blazored.LocalStorage;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Beam.Tests
{
    public class TestLocalStorage : ILocalStorageService
    {
        private Dictionary<string, object> dictionary = new Dictionary<string, object>();
        public event EventHandler<ChangingEventArgs> Changing;
        public event EventHandler<ChangedEventArgs> Changed;

        public ValueTask ClearAsync()
        {
            throw new NotImplementedException();
        }

        public ValueTask<bool> ContainKeyAsync(string key)
        {
            throw new NotImplementedException();
        }

        public ValueTask<string> GetItemAsStringAsync(string key)
        {
            throw new NotImplementedException();
        }

        public ValueTask<T> GetItemAsync<T>(string key)
        {
            if (dictionary.ContainsKey(key))
            {
                return new ValueTask<T>(Task.FromResult((T)dictionary[key]));
            }
            else
            {
                return new ValueTask<T>(Task.FromResult(default(T)));
            }
        }

        public ValueTask<string> KeyAsync(int index)
        {
            throw new NotImplementedException();
        }

        public ValueTask<int> LengthAsync()
        {
            throw new NotImplementedException();
        }

        public ValueTask RemoveItemAsync(string key)
        {
            throw new NotImplementedException();
        }

        public ValueTask SetItemAsync<T>(string key, T data)
        {
            Changing?.Invoke(this, new ChangingEventArgs() { Key = key, NewValue = data });
            dictionary[key] = data;
            Changed?.Invoke(this, new ChangedEventArgs() { Key = key, NewValue = data });
            return new ValueTask(Task.CompletedTask);
        }
    }
}