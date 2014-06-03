using System;
using System.Linq;
using System.Text;

namespace Rest4Net.ePochta.Utils
{
    internal static class CommandUtils
    {
        public static CommandResult PrepareRequestAndExecute(this Command command, string privateKey,
            Func<Command, CommandResult> baseExecutor)
        {
            var parameters = command.Parameters.OrderBy(x => x.Key);
            var sb = new StringBuilder();
            foreach (
                var parameter in
                    parameters.Where(parameter => String.CompareOrdinal(parameter.Key, @"userapp") != 0))
                sb.Append(parameter.Value);
            sb.Append(privateKey ?? "");
            return baseExecutor(command.WithParameter("sum", sb.ToString().Md5()));
        }
    }
}
