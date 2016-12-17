// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using System.Text;

namespace Tiledriver.Metadata
{
    public sealed class IndentedWriter
    {
        private readonly StringBuilder _builder = new StringBuilder();
        public int IndentionLevel { get; private set; }
        public string CurrentIndent => new string(' ', IndentionLevel*4);

        public IndentedWriter IncreaseIndent()
        {
            IndentionLevel++;
            return this;
        }

        public IndentedWriter DecreaseIndent()
        {
            if (IndentionLevel == 0)
                throw new InvalidOperationException();
            IndentionLevel--;
            return this;
        }

        public IndentedWriter OpenParen()
        {
            return Line("{").IncreaseIndent();
        }

        public IndentedWriter CloseParen()
        {
            return DecreaseIndent().Line("}");
        }

        public IndentedWriter Line(string line)
        {
            _builder.Append(CurrentIndent);
            _builder.AppendLine(line);
            return this;
        }

        public IndentedWriter Line()
        {
            _builder.AppendLine();
            return this;
        }

        public string GetString()
        {
            if (IndentionLevel != 0)
            {
                throw new InvalidOperationException("Indention level is screwed up.");
            }
            return _builder.ToString();
        }
    }
}