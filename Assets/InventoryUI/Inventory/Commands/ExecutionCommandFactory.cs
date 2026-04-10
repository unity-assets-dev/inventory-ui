using System.Collections.Generic;
using System.Linq;

public interface IExecutionCommand {
    void Execute();
}

public class ExecutionCommandFactory {
    private readonly IEnumerable<IExecutionCommand> _commands;

    public ExecutionCommandFactory(IEnumerable<IExecutionCommand> commands) {
        _commands = commands;
    }

    public TCommand Resolve<TCommand>() where TCommand : IExecutionCommand {
        var command = _commands.OfType<TCommand>().FirstOrDefault();
        
        return command;
    }
}