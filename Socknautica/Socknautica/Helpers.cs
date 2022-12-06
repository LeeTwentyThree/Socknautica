using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socknautica;

internal class Helpers
{
    public static FMODAsset GetFmodAsset(string path)
    {
        var asset = ScriptableObject.CreateInstance<FMODAsset>();
        asset.path = path;
        asset.id = path;
        return asset;
    }
}
