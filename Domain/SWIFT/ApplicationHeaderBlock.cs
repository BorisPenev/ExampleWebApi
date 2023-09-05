using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.SWIFT;

/// <summary>
/// Block 2 of SWIFT message
/// </summary>
public class ApplicationHeaderBlock : BasicBlockBase
{
    public const string DictionaryKey = "ApplicationHeader";
}