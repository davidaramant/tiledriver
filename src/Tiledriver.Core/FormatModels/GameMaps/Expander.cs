// Copyright (c) 2013, Braden Obrzut
// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

namespace Tiledriver.Core.FormatModels.GameMaps;

/// <summary>
/// Methods for decompressing RLEW encoding and Carmack compression.
/// </summary>
public static class Expander
{
	public static byte[] DecompressRlew(ushort marker, byte[] input, int outputSize)
	{
		var result = new byte[outputSize];
		var resultPos = 0;
		int inputPos = 0;

		while (inputPos < input.Length)
		{
			var word = BitConverter.ToUInt16(input, inputPos);
			if (word == marker)
			{
				inputPos += 2;
				var repeatCount = BitConverter.ToUInt16(input, inputPos);
				inputPos += 2;

				for (int i = 0; i < repeatCount; i++)
				{
					result[resultPos++] = input[inputPos];
					result[resultPos++] = input[inputPos + 1];
				}
			}
			else
			{
				result[resultPos++] = input[inputPos];
				result[resultPos++] = input[inputPos + 1];
			}
			inputPos += 2;
		}

		return result;
	}

	/// <remarks>
	/// This method was transcribed from wolfmapcommon.cpp in the ECWolf source code.
	/// </remarks>
	public static byte[] DecompressCarmack(byte[] input)
	{
		const byte nearTag = 0xA7;
		const byte farTag = 0xA8;

		var outputLength = BitConverter.ToUInt16(input, 0);

		var outputPos = 0;
		var output = new byte[outputLength];

		var inputPos = 2;

		while (outputPos < outputLength)
		{
			int copyPos;
			var length = input[inputPos++];

			if (length == 0 && (input[inputPos] == nearTag || input[inputPos] == farTag))
			{
				output[outputPos++] = input[inputPos + 1];
				output[outputPos++] = input[inputPos];
				inputPos += 2;
				continue;
			}
			else if (input[inputPos] == nearTag)
			{
				copyPos = outputPos - (input[inputPos + 1] * 2);
				inputPos += 2;
			}
			else if (input[inputPos] == farTag)
			{
				copyPos = BitConverter.ToUInt16(input, inputPos + 1) * 2;
				inputPos += 3;
			}
			else
			{
				output[outputPos++] = length;
				output[outputPos++] = input[inputPos++];
				continue;
			}

			if ((outputPos + (length * 2)) > outputLength)
				break;

			while (length-- > 0)
			{
				output[outputPos++] = output[copyPos++];
				output[outputPos++] = output[copyPos++];
			}
		}

		return output;
	}
}
