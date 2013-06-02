using System;

namespace THCard.Web.Infrastructure {
    public sealed class PresenterTarget : IEquatable<PresenterTarget> {
        public PresenterTarget(string viewName, Type controllerType, Type actionResultType) {
            this.ViewName = viewName;
            this.ControllerType = controllerType;
            this.ActionResultType = actionResultType;
        }

        public string ViewName { get; private set; }
        public Type ControllerType { get; private set; }
        public Type ActionResultType { get; private set; }

        public bool Equals(PresenterTarget other) {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(other.ViewName, this.ViewName, StringComparison.OrdinalIgnoreCase) && other.ControllerType == this.ControllerType &&
                   other.ActionResultType == this.ActionResultType;
        }

        public override bool Equals(object obj) {
            if (obj == null || obj.GetType() != GetType()) return false;
            var other = (PresenterTarget) obj;
            return Equals(this, other);
        }

        public override int GetHashCode() {
            unchecked {
                int result = this.ViewName.GetHashCode();
                result = (result * 397) ^ this.ControllerType.GetHashCode();
                result = (result * 397) ^ this.ActionResultType.GetHashCode();
                return result;
            }
        }
    }
}