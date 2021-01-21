using Blazor.IndexedDB.Framework;

using Microsoft.JSInterop;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Blazor2048
{

    public class Game2048Storage
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public string CellValues { get; set; } = string.Empty;
    }

    public class GameDB : IndexedDb
    {
            public GameDB(IJSRuntime jSRuntime, string name, int version) : base(jSRuntime, name, version) { }

            // These are like tables. Declare as many of them as you want.
            public IndexedSet<Game2048Storage>? Games { get; set; }
        
    }
}
