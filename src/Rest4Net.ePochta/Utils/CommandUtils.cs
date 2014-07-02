using System;
using System.Text;

namespace Rest4Net.ePochta.Utils
{
    internal class CommandUtils
    {
        public static CommandResult PrepareRequestAndExecute(Command command, string privateKey, CommandResult.JsonPreparer<Command, CommandResult> baseExecutor)
        {
            var sb = new StringBuilder();
            foreach (
                var parameter in
                    command.Parameters)
                if (String.CompareOrdinal(parameter.Key, @"userapp") != 0)
                    sb.Append(parameter.Value);
            sb.Append(privateKey ?? "");
            return baseExecutor(command.WithParameter("sum", StringUtils.Md5(sb.ToString())));
        }
    }
}
