// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using Tiledriver.DataModelGenerator.Utilities;
using Tiledriver.DataModelGenerator.Uwmf.MetadataModel;

namespace Tiledriver.DataModelGenerator.Uwmf
{
    public static class UwmfWriterGenerator
    {
        public static void WriteToPath(string basePath)
        {
            if (!Directory.Exists(basePath))
            {
                Directory.CreateDirectory(basePath);
            }

            foreach (var block in UwmfDefinitions.Blocks)
            {
                
            }
        }        
    }
}