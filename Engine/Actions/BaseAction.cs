using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Models;

//class to perform basic actions with

namespace Engine.Actions
{
    public abstract class BaseAction
    {
        protected readonly GameItem _itemInUse;

        //eventhandler to notify ui of any messages resulting from executing command object
        public event EventHandler<string> OnActionPerformed;

        protected BaseAction(GameItem itemInUse)
        {
            _itemInUse = itemInUse;
        }

        //reports result of action back to UI
        protected void ReportResult(string result)
        {
            OnActionPerformed?.Invoke(this, result);
        }
    }
}
