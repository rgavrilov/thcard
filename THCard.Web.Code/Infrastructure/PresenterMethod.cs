using System.Reflection;
using System.Linq;
using System.Collections.Generic;
using System;
using System.Web.Mvc;
using JetBrains.Annotations;

namespace THCard.Web.Infrastructure {
    public class PresenterMethod {
        private readonly MethodInfo method;

        public PresenterMethod(MethodInfo method) {
            this.method = method;
        }

        [NotNull]
        public Type PresenterType {
            get { return this.method.DeclaringType; }
        }

        public Type ActionResultType {
            get { return this.method.GetParameters().Single().ParameterType; }
        }

        public string ViewName {
            get {
                ActionNameAttribute actionNameAttribute =
                        this.method.GetCustomAttributes(typeof (ActionNameAttribute), false).Cast<ActionNameAttribute>().FirstOrDefault();
                return actionNameAttribute != null ? actionNameAttribute.Name : this.method.Name;
            }
        }

        public static bool IsValidPresenterMethod(MethodInfo method, IList<string> validationErrors) {
            validationErrors = validationErrors ?? new List<string>();
            if (method == null) throw new ArgumentNullException("method");
            int errorCount = 0;
            if (!method.IsPublic) {
                validationErrors.Add("Method must be public.");
                ++errorCount;
            }
            if (method.IsStatic) {
                validationErrors.Add("Method must be instance method.");
                ++errorCount;
            }
            if (method.ReturnType == typeof (void)) {
                validationErrors.Add("Method must have a non-void return type.");
                ++errorCount;
            }
            if (method.GetParameters().Count() != 1) {
                validationErrors.Add("Method must have exactly one parameter.");
                ++errorCount;
            }
            return errorCount == 0;
        }

        public object Invoke(object actionResult, IPresenterActivator presenterActivator) {
	        object presenter = presenterActivator.CreateInstance(this.PresenterType);
            return this.method.Invoke(presenter, new object[] { actionResult });
        }

        public static IEnumerable<PresenterMethod> GetMethods(Type presenterType) {
            MethodInfo[] publicInstanceMethods = presenterType.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            foreach (MethodInfo method in publicInstanceMethods) {
                if (IsValidPresenterMethod(method, null)) {
                    yield return new PresenterMethod(method);
                }
            }
        }
    }
}