using System.Collections.Immutable;
using Tiledriver.FormatModels.Common.Reading;
using Tiledriver.FormatModels.Common.Reading.AbstractSyntaxTree;

namespace Tiledriver.FormatModels.Uwmf.Reading.AbstractSyntaxTree;

public sealed record IntTupleBlock(IdentifierToken Name, ImmutableArray<IntTuple> Tuples) : IExpression;
