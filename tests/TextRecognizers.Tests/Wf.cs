using System.Activities;
using System.Collections.Generic;

namespace TextRecognizers.Tests
{
    /// <summary>
    /// Small helpers for running activities through <see cref="WorkflowInvoker"/> - the
    /// same execution path Studio uses - so the tests exercise the real activity surface.
    /// </summary>
    internal static class Wf
    {
        /// <summary>Wraps a value as a literal input argument (value types and strings only).</summary>
        public static InArgument<T> Lit<T>(T value) => new InArgument<T>(value);

        /// <summary>Runs an activity and returns its named outputs (for multi-output activities).</summary>
        public static IDictionary<string, object> Out(Activity activity) => WorkflowInvoker.Invoke(activity);

        /// <summary>Runs a single-result activity and returns its result.</summary>
        public static T Result<T>(Activity<T> activity) => WorkflowInvoker.Invoke(activity);

        /// <summary>Runs a single-result activity, binding reference-type inputs by name.</summary>
        public static T Result<T>(Activity<T> activity, IDictionary<string, object> inputs) =>
            WorkflowInvoker.Invoke(activity, inputs);
    }
}
