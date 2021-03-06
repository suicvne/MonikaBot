﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonikaBot.Commands
{
    public enum ModuleType
    {
        Internal = 0,
        External
    }

    public abstract class IModule
    {
        /// <summary>
        /// The name of the module.
        /// </summary>
        public virtual string Name { get; set; } = "module";

        /// <summary>
        /// A description talking about what the module contains
        /// </summary>
        public virtual string Description { get; set; } = "Please set this in the constructor of your IModule derivative.";

        /// <summary>
        /// A list of the commands this module contains
        /// </summary>
        public virtual List<ICommand> Commands { get; internal set; } = new List<ICommand>();

        /// <summary>
        /// Gets or sets the kind of the module.
        /// </summary>
        /// <value>The kind of the module.</value>
        public virtual ModuleType ModuleKind { get; set; } = ModuleType.Internal;

        /// <summary>
        /// Installs the module's commands into the commands manager
        /// </summary>
        /// <param name="manager"></param>
        public abstract void Install(CommandsManager manager);

        /// <summary>
        /// Event that occurs when the module is about to be shutdown (removed from memory or bot is closing down).
        /// 
        /// You can perform saving events here for databases.
        /// </summary>
        /// <param name="managers">Managers.</param>
        public abstract void ShutdownModule(CommandsManager managers);

        /// <summary>
        /// Uninstall's this modules's commands from the given module manager.
        /// </summary>
        /// <param name="manager"></param>
        public void Uninstall(CommandsManager manager)
        {
            lock (manager.Commands)
            {
                foreach (var command in manager.Commands)
                {
                    var thisModulesCommand = Commands.Select(x => x.ID == command.Value.ID && x.Parent.Name == Name);
                    if (thisModulesCommand != null)
                        manager.Commands.Remove(command.Key);
                }
            }
        }

    }
}
