// Copyright (c) 2017, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;

namespace Tiledriver.Core.Utils
{
    public static class ResetableLazy
    {
        public static ResetableLazy<T> Create<T>(Func<T> valueFactory) => new ResetableLazy<T>(valueFactory);
    }

    public sealed class ResetableLazy<T>
    {
        private readonly Func<T> _valueFactory;
        private Lazy<T> _lazy;

        public ResetableLazy(Func<T> valueFactory)
        {
            _valueFactory = valueFactory;
            _lazy = new Lazy<T>(valueFactory);
        }

        public T Value => _lazy.Value;

        public void Reset()
        {
            _lazy = new Lazy<T>(_valueFactory);
        }
    }
}