using System.Collections.Immutable;
using Tiledriver.Core.FormatModels.Common;
using Tiledriver.Core.FormatModels.Common.Reading;

namespace Tiledriver.Core.FormatModels.Uwmf.Reading.AbstractSyntaxTree;

public sealed record IntTuple(FilePosition StartLocation, ImmutableArray<IntegerToken> Values);
