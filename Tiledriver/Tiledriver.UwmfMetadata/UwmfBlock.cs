/*
** UwmfBlock.cs
**
**---------------------------------------------------------------------------
** Copyright (c) 2016, David Aramant
** All rights reserved.
**
** Redistribution and use in source and binary forms, with or without
** modification, are permitted provided that the following conditions
** are met:
**
** 1. Redistributions of source code must retain the above copyright
**    notice, this list of conditions and the following disclaimer.
** 2. Redistributions in binary form must reproduce the above copyright
**    notice, this list of conditions and the following disclaimer in the
**    documentation and/or other materials provided with the distribution.
** 3. The name of the author may not be used to endorse or promote products
**    derived from this software without specific prior written permission.
**
** THIS SOFTWARE IS PROVIDED BY THE AUTHOR ``AS IS'' AND ANY EXPRESS OR
** IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES
** OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED.
** IN NO EVENT SHALL THE AUTHOR BE LIABLE FOR ANY DIRECT, INDIRECT,
** INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT
** NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
** DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY
** THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
** (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF
** THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
**---------------------------------------------------------------------------
**
**
*/

using System.Collections.Generic;
using System.Linq;

namespace Tiledriver.UwmfMetadata
{
    public sealed class UwmfBlock : NamedItem
    {
        private readonly List<UwmfProperty> _properties = new List<UwmfProperty>();
        private readonly List<NamedItem> _subBlocks = new List<NamedItem>();

        public IEnumerable<UwmfProperty> Properties => _properties;
        public IEnumerable<NamedItem> SubBlocks => _subBlocks;
        public bool IsSubBlock { get; private set; } = true;
        public bool NormalWriting { get; private set; } = true;
        public bool NormalReading { get; private set; } = true;
        public bool CanHaveUnknownProperties { get; private set; } = true;
        public bool CanHaveUnknownBlocks => !IsSubBlock;

        public UwmfBlock(string name) : base(name,name)
        {
        }

        public UwmfBlock HasSubBlocks(params string[] names)
        {
            _subBlocks.AddRange(names.Select(_ => new NamedItem(_,_)));
            return this;
        }

        public UwmfBlock IsTopLevel()
        {
            IsSubBlock = false;
            return this;
        }

        public UwmfBlock DisableNormalWriting()
        {
            NormalWriting = false;
            return this;
        }

        public UwmfBlock DisableNormalReading()
        {
            NormalReading = false;
            return this;
        }

        public UwmfBlock CannotHaveUnknownProperties()
        {
            CanHaveUnknownProperties = false;
            return this;
        }

        public UwmfBlock HasRequiredIntegerNumber(string name)
        {
            _properties.Add(new UwmfProperty(name, name, PropertyType.IntegerNumber, defaultValue: null));
            return this;
        }

        public UwmfBlock HasRequiredFloatingPointNumber(string name)
        {
            _properties.Add(new UwmfProperty(name, name, PropertyType.FloatingPointNumber, defaultValue: null));
            return this;
        }

        public UwmfBlock HasRequiredString(string name, string uwmfName = null)
        {
            // 'namespace' is currently the only name that needs special handling.
            _properties.Add(new UwmfProperty(name, uwmfName ?? name, PropertyType.String, defaultValue: null));
            return this;
        }

        public UwmfBlock HasRequiredBoolean(string name)
        {
            _properties.Add(new UwmfProperty(name, name, PropertyType.Boolean, defaultValue: null));
            return this;
        }

        public UwmfBlock HasOptionalIntegerNumber(string name, int defaultValue)
        {
            _properties.Add(new UwmfProperty(name, name, PropertyType.IntegerNumber, defaultValue: defaultValue));
            return this;
        }

        public UwmfBlock HasOptionalFloatingPointNumber(string name, double defaultValue)
        {
            _properties.Add(new UwmfProperty(name, name, PropertyType.FloatingPointNumber, defaultValue: defaultValue));
            return this;
        }

        public UwmfBlock HasOptionalString(string name, string defaultValue)
        {
            _properties.Add(new UwmfProperty(name, name, PropertyType.String, defaultValue: defaultValue));
            return this;
        }

        public UwmfBlock HasOptionalBoolean(string name, bool defaultValue)
        {
            _properties.Add(new UwmfProperty(name, name, PropertyType.Boolean, defaultValue: defaultValue));
            return this;
        }
    }
}