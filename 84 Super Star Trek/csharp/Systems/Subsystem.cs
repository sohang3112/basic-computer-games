using System;
using SuperStarTrek.Commands;
using SuperStarTrek.Space;

namespace SuperStarTrek.Systems
{
    internal abstract class Subsystem
    {
        private readonly Output _output;

        protected Subsystem(string name, Command command, Output output)
        {
            Name = name;
            Command = command;
            Condition = 0;
            _output = output;
        }

        public string Name { get; }
        public double Condition { get; private set; }
        public bool IsDamaged => Condition < 0;
        public Command Command { get; }

        protected virtual bool CanExecuteCommand() => true;

        protected bool IsOperational(string notOperationalMessage)
        {
            if (IsDamaged)
            {
                _output.WriteLine(notOperationalMessage.Replace("{name}", Name));
                return false;
            }

            return true;
        }

        public CommandResult ExecuteCommand(Quadrant quadrant)
            => CanExecuteCommand() ? ExecuteCommandCore(quadrant) : CommandResult.Ok;

        protected abstract CommandResult ExecuteCommandCore(Quadrant quadrant);

        public virtual void Repair()
        {
            if (IsDamaged) { Condition = 0; }
        }

        internal void TakeDamage(double damage)
        {
            Condition -= damage;
            _output.WriteLine($"Damage Control reports, '{Name} damaged by the hit.'");
        }
    }
}
