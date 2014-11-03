using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hook.Models
{
    public class HookId
    {
        private readonly Guid _uid;

        public HookId()
        {
            this._uid = Guid.NewGuid();
        }

        public override string ToString()
        {
            return this._uid.ToString("N").Substring(0, 9);
        }
    }
}