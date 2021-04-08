// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using FluentAssertions;
using Tiledriver.Core.FormatModels.Uwmf;

namespace Tiledriver.Core.Tests.FormatModels.Uwmf.Parsing
{
    public static partial class UwmfComparison
    {
        public static void AssertEqual(UnknownProperty actual, UnknownProperty expected)
        {
            Func<UnknownProperty, string> toString =
                up => $"{(string)up.Name} = {up.Value}";
            toString(actual).Should().Be(toString(expected));
        }

        public static void AssertEqual(IEnumerable<UnknownProperty> actual, IEnumerable<UnknownProperty> expected)
        {
            Func<IEnumerable<UnknownProperty>, string> toString =
                list => string.Join("\n", list.Select(up => $"{(string)up.Name} = {up.Value}"));
            toString(actual).Should().Be(toString(expected));
        }

        public static void AssertEqual(UnknownBlock actual, UnknownBlock expected)
        {
            actual.Name.Should().Be(expected.Name, "unknown block name should match");

            AssertEqual(actual.Properties, expected.Properties);
        }
    }
}